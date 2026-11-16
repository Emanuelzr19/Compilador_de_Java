using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CompiladorJava.Lexico;
using CompiladorJava.Modelos;
using static CompiladorJava.Modelos.CodigosToken;

namespace CompiladorJava.Sintactico;

public sealed class ResultadoSintactico
{
    public List<Instruccion> Instrucciones { get; } = new();
    public List<ErrorCompilacion> Errores { get; } = new();
    public Dictionary<string, int> NombresConError { get; } = new();
}

public sealed partial class AnalizadorSintactico
{
    private sealed class ErrorSintaxis : Exception
    {
        public ErrorCompilacion Error { get; }
        public bool Saltar { get; }

        public ErrorSintaxis(ErrorCompilacion error, bool saltar = true)
        {
            Error = error;
            Saltar = saltar;
        }
    }

    private readonly IReadOnlyList<Token> _t;
    private readonly HashSet<int> _lineasLexicas;
    private readonly ResultadoSintactico _r = new();
    private readonly Token _eof;
    private int _p;
    private int _nivel;

    public AnalizadorSintactico(IReadOnlyList<Token> tokens, IEnumerable<int> lineasConErrorLexico)
    {
        _t = tokens;
        _lineasLexicas = new HashSet<int>(lineasConErrorLexico);
        var ultimo = tokens.Count > 0 ? tokens[^1] : null;
        _eof = new Token(EOF, "fin del archivo", ultimo?.Linea ?? 1, ultimo == null ? 1 : ultimo.Columna + ultimo.Lexema.Length);
    }

    public ResultadoSintactico Analizar()
    {
        Programa();
        return _r;
    }

    private Token Ver(int adelante = 0) => _p + adelante < _t.Count ? _t[_p + adelante] : _eof;

    private bool Es(params string[] codigos) => Array.IndexOf(codigos, Ver().Codigo) >= 0;

    private bool EsEn(int adelante, string codigo) => Ver(adelante).Codigo == codigo;

    private Token Tomar()
    {
        var t = Ver();
        if (_p < _t.Count) _p++;
        return t;
    }

    private Token Ultimo() => _p > 0 ? _t[_p - 1] : _eof;

    private string MensajeFalta(string simbolo)
    {
        bool otraLinea = _p > 0 && Ultimo().Linea < Ver().Linea;
        if (otraLinea || Es(FI, PC, LLC, LLA, COC, EOF)) return $"Falta {simbolo}";
        return Ver().Lexema;
    }

    private static ErrorCompilacion NuevoError(Token t, string mensaje)
        => new() { Linea = t.Linea, Columna = t.Columna, Tipo = TipoError.Sintactico, Mensaje = mensaje };

    private ErrorSintaxis Falla(string mensaje) => new(NuevoError(Ver(), mensaje));

    private ErrorSintaxis FallaEsperaba(string esperado) => Falla(MensajeFalta(esperado));

    private Token Esperar(string codigo, string contexto = "")
    {
        if (Es(codigo)) return Tomar();

        string mensaje = MensajeFalta(CatalogoTokens.Simbolo(codigo));

        var anterior = Ultimo();
        if (_p > 0 && anterior.Linea < Ver().Linea)
        {
            var error = new ErrorCompilacion
            {
                Linea = anterior.Linea,
                Columna = anterior.Columna + anterior.Lexema.Length,
                Tipo = TipoError.Sintactico,
                Mensaje = mensaje
            };
            throw new ErrorSintaxis(error, false);
        }

        throw Falla(mensaje);
    }

    private string Reportar(ErrorCompilacion e)
    {
        if (_lineasLexicas.Contains(e.Linea)) return $"No cumple por el error léxico de la línea {e.Linea}";
        if (!_r.Errores.Any(x => x.Linea == e.Linea && x.Columna == e.Columna && x.Mensaje == e.Mensaje)) _r.Errores.Add(e);
        return e.Mensaje;
    }

    private void ErrorSuave(Token t, string mensaje) => Reportar(NuevoError(t, mensaje));

    private void Registrar(int ini, int fin, string regla, bool valida, string motivo = "")
    {
        fin = Math.Min(fin, _t.Count);
        if (fin <= ini) return;

        var tokens = _t.Skip(ini).Take(fin - ini).ToList();
        if (valida && tokens.Any(x => _lineasLexicas.Contains(x.Linea)))
        {
            valida = false;
            motivo = "La línea contiene un error léxico";
        }
        if (!valida)
        {
            for (int k = 1; k < tokens.Count; k++)
                if (tokens[k].Codigo == ID && tokens[k - 1].Codigo is TD or STR or ID) _r.NombresConError.TryAdd(tokens[k].Lexema, tokens[k].Linea);
        }

        _r.Instrucciones.Add(new Instruccion
        {
            Linea = tokens[0].Linea,
            Texto = TextoCodigo.Unir(tokens),
            Regla = regla,
            Produccion = Gramatica.Producciones[regla],
            Derivacion = string.Join(" ", tokens.Select(x => x.Codigo)),
            Valida = valida,
            Motivo = motivo,
            Nivel = _nivel,
            Tokens = tokens
        });
    }

    private void Intentar(string regla, Action produccion)
    {
        int ini = _p;
        try
        {
            produccion();
            Registrar(ini, _p, regla, true);
        }
        catch (ErrorSintaxis e)
        {
            Fallar(e, ini, regla);
        }
    }

    private bool Encabezado(string regla, Action produccion)
    {
        int ini = _p;
        try
        {
            produccion();
            Registrar(ini, _p, regla, true);
            return true;
        }
        catch (ErrorSintaxis e)
        {
            Fallar(e, ini, regla);
            return false;
        }
    }

    private void Fallar(ErrorSintaxis e, int ini, string regla)
    {
        bool saltar = e.Saltar;
        string motivo;
        if (ini < _t.Count && _lineasLexicas.Contains(_t[ini].Linea))
        {
            motivo = $"No cumple por el error léxico de la línea {_t[ini].Linea}";
            if (e.Error.Linea > _t[ini].Linea) saltar = false;
        }
        else
        {
            motivo = Reportar(e.Error);
        }
        if (saltar)
        {
            if (regla is Gramatica.DPACKAGE or Gramatica.IMPORT) SaltarHastaClase();
            else Recuperar(ini);
        }
        if (_p == ini && !Es(EOF, LLC)) Tomar();
        Registrar(ini, HastaLlave(ini, _p), regla, false, motivo);
    }

    private int HastaLlave(int ini, int fin)
    {
        for (int k = ini; k < fin && k < _t.Count; k++)
            if (_t[k].Codigo == LLA) return k + 1;
        return fin;
    }

    private void Recuperar(int ini)
    {
        int abiertas = 0;
        for (int k = ini; k < _p; k++)
        {
            if (_t[k].Codigo == LLA) abiertas++;
            else if (_t[k].Codigo == LLC) abiertas--;
        }

        while (!Es(EOF))
        {
            if (Es(LLA))
            {
                abiertas++;
            }
            else if (Es(LLC))
            {
                if (abiertas <= 0) return;
                abiertas--;
                Tomar();
                if (abiertas == 0) return;
                continue;
            }
            else if (Es(FI) && abiertas <= 0)
            {
                Tomar();
                return;
            }
            Tomar();
        }
    }

    private void SaltarBloque()
    {
        int abiertas = 0;
        do
        {
            if (Es(LLA)) abiertas++;
            else if (Es(LLC)) abiertas--;
            Tomar();
        } while (abiertas > 0 && !Es(EOF));
    }

    private void Programa()
    {
        if (Es(PCK)) Intentar(Gramatica.DPACKAGE, Paquete);
        SaltarBasuraAntesDeClase();

        while (Es(IMP))
        {
            Intentar(Gramatica.IMPORT, Importar);
            SaltarBasuraAntesDeClase();
        }

        Clase();
    }

    private void SaltarBasuraAntesDeClase()
    {
        if (!Es(IMP, MA, CL, EOF)) Intentar(Gramatica.DPACKAGE, () => throw Falla(Ver().Lexema));
    }

    private void SaltarHastaClase()
    {
        while (!Es(IMP, MA, CL, EOF)) Tomar();
    }

    private void Paquete()
    {
        Tomar();
        Esperar(ID, "nombre del paquete");
        if (Es(CA)) throw Falla(Ver().Lexema);
        Esperar(FI, "package");
    }

    private void Importar()
    {
        Tomar();
        Esperar(ID, "import");
        Esperar(CA, "import");
        Esperar(ID, "import");
        if (Es(CA)) throw Falla(Ver().Lexema);
        Esperar(FI, "import");
    }

    private void Clase()
    {
        if (!Encabezado(Gramatica.CLASE, EncabezadoClase)) return;

        _nivel = 1;
        while (!Es(LLC, EOF))
        {
            if (Es(FI)) Tomar();
            else Miembro();
        }
        _nivel = 0;

        if (Es(LLC)) Tomar();
        else ErrorSuave(Ultimo(), "Falta }");

        ReportarSobrantes();
    }

    private void ReportarSobrantes()
    {
        int lineaAnterior = -1;
        while (!Es(EOF))
        {
            var t = Tomar();
            if (t.Codigo == LLC || t.Linea != lineaAnterior) ErrorSuave(t, t.Lexema);
            lineaAnterior = t.Linea;
        }
    }

    private void EncabezadoClase()
    {
        Esperar(MA, "clase");
        Esperar(CL, "clase");
        Esperar(ID, "nombre de la clase");
        if (Es(EXTS, IMPL)) throw Falla(Ver().Lexema);
        Esperar(LLA, "clase");
    }

    private void Miembro()
    {
        if (Es(MA) && EsEn(1, ST)) BloqueMain();
        else if (Es(MA) && (EsEn(1, TM) || EsEn(3, PA))) BloqueMetodo();
        else if (Es(ST)) Intentar(Gramatica.D_GLOB, () => throw Falla(Ver().Lexema));
        else if (Es(ID) && EsObjetoSimple()) Intentar(Gramatica.OBJ, Objeto);
        else if (Es(MA, TD, STR, ID)) Intentar(Gramatica.D_GLOB, AtributoGlobal);
        else Intentar(Gramatica.D_GLOB, () => throw Falla(Ver().Lexema));
    }

    private void AtributoGlobal()
    {
        bool conModificador = Es(MA);
        if (conModificador) Tomar();

        if (!Es(TD, STR, ID)) throw FallaEsperaba("tipo");
        if (conModificador && Es(ID) && EsObjetoSimple())
            throw Falla(Ultimo().Lexema);

        Declaracion();
    }

    private void BloqueMain()
    {
        if (!Encabezado(Gramatica.MAIN, EncabezadoMain)) return;
        _nivel++;
        Cuerpo(false);
        _nivel--;
        CerrarBloque("cierre de main");
    }

    private void EncabezadoMain()
    {
        Tomar();
        Tomar();
        if (!(Es(TM) && EsEn(1, PMAIN)))
            throw Falla(Ultimo().Lexema);
        Tomar();
        Tomar();
        foreach (var codigo in new[] { PA, STR, COA, COC, ID, PC, LLA })
            Esperar(codigo, "firma de main: public static void main(String[] args)");
    }

    private void BloqueMetodo()
    {
        if (!Encabezado(Gramatica.METODO, EncabezadoMetodo)) return;

        _nivel++;
        Cuerpo(true);
        if (Es(RTN))
        {
            Intentar(Gramatica.REE, Retorno);
            if (!Es(LLC, MA, EOF))
            {
                ErrorSuave(Ver(), Ver().Lexema);
                while (!Es(LLC, MA, EOF))
                {
                    if (Es(LLA)) SaltarBloque();
                    else Tomar();
                }
            }
        }
        _nivel--;
        CerrarBloque("cierre del método");
    }

    private void EncabezadoMetodo()
    {
        Tomar();
        if (!Es(TM, TD, STR, ID)) throw FallaEsperaba("tipo de retorno");
        Tomar();
        Esperar(ID, "nombre del método");
        Esperar(PA, "método");
        if (!Es(PC))
        {
            Parametro();
            while (Es(CO))
            {
                Tomar();
                Parametro();
            }
        }
        Esperar(PC, "parámetros del método");
        Esperar(LLA, "método");
    }

    private void Parametro()
    {
        if (!Es(TD, STR, ID)) throw FallaEsperaba("tipo de parámetro");
        Tomar();
        if (Es(COA)) throw Falla(Ver().Lexema);
        Esperar(ID, "nombre del parámetro");
    }

    private void CerrarBloque(string que)
    {
        if (Es(LLC)) Tomar();
        else ErrorSuave(Ultimo(), "Falta }");
    }

    private void Cuerpo(bool esMetodo)
    {
        while (!Es(LLC, EOF, MA) && !(esMetodo && Es(RTN))) Sentencia();
    }
}
