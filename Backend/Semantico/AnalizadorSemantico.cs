using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CompiladorJava.Modelos;
using CompiladorJava.Sintactico;
using static CompiladorJava.Modelos.CodigosToken;

namespace CompiladorJava.Semantico;

public sealed class ResultadoSemantico
{
    public List<FilaSemantica> Filas { get; } = new();
    public List<ErrorCompilacion> Errores { get; } = new();
}

public sealed partial class AnalizadorSemantico
{
    private sealed record Metodo(string Retorno, List<string> Parametros);

    private readonly IReadOnlyList<Instruccion> _instrucciones;
    private readonly Dictionary<string, int> _nombresConError;
    private readonly ResultadoSemantico _r = new();
    private readonly Dictionary<string, string> _campos = new();
    private readonly Dictionary<string, string> _locales = new();
    private readonly HashSet<string> _sinInicializar = new();
    private readonly Dictionary<string, List<Metodo>> _metodos = new();
    private readonly HashSet<string> _externas = new();
    private string _clase = "";
    private string _retorno = "void";
    private bool _enMain;

    public AnalizadorSemantico(IReadOnlyList<Instruccion> instrucciones, Dictionary<string, int> nombresConError)
    {
        _instrucciones = instrucciones;
        _nombresConError = nombresConError;
    }

    public ResultadoSemantico Analizar()
    {
        Preparar();
        for (int k = 0; k < _instrucciones.Count; k++)
        {
            if (_instrucciones[k].Valida) Revisar(_instrucciones[k], k);
            else RevisarInvalida(_instrucciones[k]);
        }
        return _r;
    }

    private void Preparar()
    {
        foreach (var inst in _instrucciones.Where(i => i.Valida))
        {
            var t = inst.Tokens;
            if (inst.Regla == Gramatica.CLASE) _clase = t[2].Lexema;
            else if (inst.Regla == Gramatica.IMPORT) _externas.Add(t[3].Lexema);
            else if (inst.Regla == Gramatica.METODO) RegistrarMetodo(t);
        }
    }

    private void RegistrarMetodo(List<Token> t)
    {
        var parametros = new List<string>();
        for (int i = 4; i < t.Count && t[i].Codigo != PC; i++)
        {
            if (t[i].Codigo == CO) continue;
            parametros.Add(t[i].Lexema);
            i++;
        }

        if (!_metodos.TryGetValue(t[2].Lexema, out var lista)) _metodos[t[2].Lexema] = lista = new List<Metodo>();
        lista.Add(new Metodo(t[1].Lexema, parametros));
    }

    private void Revisar(Instruccion inst, int posicion)
    {
        if (inst.Nivel == 1 && inst.Regla is Gramatica.D_GLOB or Gramatica.OBJ)
        {
            _enMain = false;
            _locales.Clear();
        _sinInicializar.Clear();
        }

        switch (inst.Regla)
        {
            case Gramatica.MAIN:
                IniciarMain(inst);
                break;
            case Gramatica.METODO:
                IniciarMetodo(inst, posicion);
                break;
            case Gramatica.DEC:
            case Gramatica.D_GLOB:
                Declaracion(inst);
                break;
            case Gramatica.ASIG:
                Asignacion(inst);
                break;
            case Gramatica.OBJ:
                Objeto(inst);
                break;
            case Gramatica.ACC:
                TipoDe(inst.Tokens, 0, inst.Tokens.Count - 1);
                break;
            case Gramatica.SOUT:
                if (inst.Tokens.Count > 8) TipoDe(inst.Tokens, 6, inst.Tokens.Count - 2);
                break;
            case Gramatica.REE:
                Retorno(inst);
                break;
        }
    }

    private void RevisarInvalida(Instruccion inst)
    {
        if (inst.Nivel < 2 || _sinInicializar.Count == 0) return;

        var t = inst.Tokens;
        int inicio = 0;
        string? destino = null;
        if (t.Count > 1 && t[0].Codigo == ID && t[1].Codigo == ODA)
        {
            destino = t[0].Lexema;
            inicio = 2;
        }

        for (int k = inicio; k < t.Count; k++)
        {
            var x = t[k];
            if (x.Codigo != ID || !_sinInicializar.Contains(x.Lexema)) continue;
            if (k > 0 && (t[k - 1].Codigo == CA || t[k - 1].Codigo is TD or STR or ID)) continue;
            if (k + 1 < t.Count && t[k + 1].Codigo == PA) continue;
            Error(x, x.Lexema);
        }

        if (destino != null) _sinInicializar.Remove(destino);
    }

    private void IniciarMain(Instruccion inst)
    {
        _locales.Clear();
        _sinInicializar.Clear();
        _enMain = true;
        _retorno = "void";
        _locales[inst.Tokens[8].Lexema] = "String[]";
    }

    private void IniciarMetodo(Instruccion inst, int posicion)
    {
        var t = inst.Tokens;
        _locales.Clear();
        _sinInicializar.Clear();
        _enMain = false;
        _retorno = t[1].Lexema;
        if (t[1].Codigo == ID) ResolverTipo(t[1]);

        for (int i = 4; i < t.Count && t[i].Codigo != PC; i++)
        {
            if (t[i].Codigo == CO) continue;
            string tipo = ResolverTipo(t[i]);
            var nombre = t[i + 1];
            if (_locales.ContainsKey(nombre.Lexema)) Error(nombre, nombre.Lexema);
            else _locales[nombre.Lexema] = tipo;
            i++;
        }

        if (_retorno == "void") return;
        bool tieneReturn = false;
        for (int j = posicion + 1; j < _instrucciones.Count && _instrucciones[j].Nivel >= 2; j++)
            if (_instrucciones[j].Regla == Gramatica.REE) tieneReturn = true;
        if (!tieneReturn) Error(t[2], "Falta return");
    }

    private void Declaracion(Instruccion inst)
    {
        var t = inst.Tokens;
        int i = t[0].Codigo == MA ? 1 : 0;
        string tipo = ResolverTipo(t[i]);
        var nombre = t[i + 1];
        var ambito = inst.Nivel == 1 ? _campos : _locales;

        bool repetida = ambito.ContainsKey(nombre.Lexema);
        if (repetida) Error(nombre, nombre.Lexema);
        else ambito[nombre.Lexema] = tipo;

        if (t[i + 2].Codigo != ODA)
        {
            if (inst.Nivel != 1 && !repetida) _sinInicializar.Add(nombre.Lexema);
            Fila(inst.Linea, nombre.Lexema, tipo, "(sin valor)", "-", repetida ? EstadoTipos.Incorrecto : EstadoTipos.Correcto,
                repetida ? "Redeclaración: el nombre ya existe en este ámbito." : "Declarada sin valor asignado.");
            return;
        }

        Comparar(inst.Linea, nombre, tipo, t, i + 3, t.Count - 1, repetida);
    }

    private void Asignacion(Instruccion inst)
    {
        var t = inst.Tokens;
        var nombre = t[0];
        string? tipo = BuscarVariable(nombre);

        if (tipo == null)
        {
            if (ConErrorPrevio(nombre)) return;
            Error(nombre, nombre.Lexema);
            string tipoValorSinVariable = TipoDe(t, 2, t.Count - 1);
            Fila(inst.Linea, nombre.Lexema, "?", TextoCodigo.Unir(t.Skip(2).Take(t.Count - 3)), tipoValorSinVariable, EstadoTipos.Incorrecto, "La variable no está declarada.");
            return;
        }

        Comparar(inst.Linea, nombre, tipo, t, 2, t.Count - 1, false);
        _sinInicializar.Remove(nombre.Lexema);
    }

    private string? BuscarVariable(Token nombre)
    {
        if (_locales.TryGetValue(nombre.Lexema, out var tipo)) return tipo;
        if (!_campos.TryGetValue(nombre.Lexema, out tipo)) return null;
        if (_enMain) Error(nombre, nombre.Lexema);
        return tipo;
    }

    private void Objeto(Instruccion inst)
    {
        var t = inst.Tokens;
        var nombre = t[1];
        var ambito = inst.Nivel == 1 ? _campos : _locales;
        string tipo = ResolverTipo(t[0]);
        string tipoNuevo = ResolverTipo(t[4]);

        bool repetida = ambito.ContainsKey(nombre.Lexema);
        if (repetida) Error(nombre, nombre.Lexema);
        else ambito[nombre.Lexema] = tipo;

        int erroresAntes = _r.Errores.Count;
        var argumentos = t.Skip(6).Take(t.Count - 8).Where(x => x.Codigo != CO).ToList();
        foreach (var argumento in argumentos) TipoDe(new List<Token> { argumento }, 0, 1);

        if (tipoNuevo == _clase && argumentos.Count > 0)
            Error(t[4], t[4].Lexema);

        bool conErrores = _r.Errores.Count > erroresAntes;
        string estado;
        string explicacion;
        if (repetida)
        {
            estado = EstadoTipos.Incorrecto;
            explicacion = "Redeclaración: el nombre ya existe en este ámbito.";
        }
        else if (conErrores)
        {
            estado = EstadoTipos.Incorrecto;
            explicacion = "La instrucción contiene errores (ver tabla de errores).";
        }
        else if (tipo == Tipos.Desconocido || tipoNuevo == Tipos.Desconocido)
        {
            estado = EstadoTipos.NoVerificable;
            explicacion = "El tipo no está definido, no se puede verificar.";
        }
        else if (tipo == tipoNuevo)
        {
            estado = EstadoTipos.Correcto;
            explicacion = $"El objeto es de la clase {tipoNuevo}, igual que la variable.";
        }
        else
        {
            estado = EstadoTipos.Incorrecto;
            explicacion = $"Se crea un objeto {tipoNuevo} y la variable es de tipo {tipo}.";
            Error(nombre, $"new {t[4].Lexema}({TextoCodigo.Unir(t.Skip(6).Take(t.Count - 8))})");
        }

        string valor = $"new {t[4].Lexema}({TextoCodigo.Unir(t.Skip(6).Take(t.Count - 8))})";
        Fila(inst.Linea, nombre.Lexema, tipo, valor, tipoNuevo, estado, explicacion);
    }

    private void Retorno(Instruccion inst)
    {
        var t = inst.Tokens;
        bool tieneValor = t.Count > 2;

        if (_retorno == "void")
        {
            if (tieneValor) Error(t[0], t[0].Lexema);
            return;
        }

        if (!tieneValor)
        {
            Error(t[0], t[0].Lexema);
            return;
        }

        string tipoValor = TipoDe(t, 1, t.Count - 1);
        string? literal = t.Count == 3 && t[1].Codigo == VTE ? t[1].Lexema : null;
        if (!Tipos.Asignable(_retorno, tipoValor, literal))
            Error(t[0], TextoCodigo.Unir(t.Skip(1).Take(t.Count - 2)));
    }

    private void Comparar(int linea, Token nombre, string tipo, List<Token> t, int desde, int hasta, bool repetida)
    {
        int erroresAntes = _r.Errores.Count;
        string tipoValor = TipoDe(t, desde, hasta);
        bool conErrores = _r.Errores.Count > erroresAntes;
        string? literal = hasta - desde == 1 && t[desde].Codigo == VTE ? t[desde].Lexema : null;
        string valor = TextoCodigo.Unir(t.Skip(desde).Take(hasta - desde));

        string estado;
        string explicacion;
        bool dependeDeError = t.Skip(desde).Take(hasta - desde).Any(x => x.Codigo == ID && ConErrorPrevio(x));

        if (repetida)
        {
            estado = EstadoTipos.Incorrecto;
            explicacion = "Redeclaración: el nombre ya existe en este ámbito.";
        }
        else if (conErrores)
        {
            estado = EstadoTipos.Incorrecto;
            explicacion = "El valor contiene errores (ver tabla de errores).";
        }
        else if (dependeDeError || tipo == Tipos.Desconocido)
        {
            estado = EstadoTipos.NoVerificable;
            explicacion = "Usa un nombre o tipo cuya declaración tiene errores; no se puede verificar.";
        }
        else if (tipoValor == Tipos.Desconocido)
        {
            estado = EstadoTipos.Correcto;
            explicacion = "El valor viene de una clase importada; se acepta.";
        }
        else if (Tipos.Asignable(tipo, tipoValor, literal))
        {
            estado = EstadoTipos.Correcto;
            explicacion = Explicar(tipo, tipoValor);
        }
        else
        {
            estado = EstadoTipos.Incorrecto;
            explicacion = Tipos.EsNumerico(tipo) && Tipos.EsNumerico(tipoValor)
                ? $"Convertir {tipoValor} a {tipo} puede perder información."
                : $"Un valor {tipoValor} no se puede guardar en una variable {tipo}.";
            Error(nombre, valor);
        }

        Fila(linea, nombre.Lexema, tipo, valor, tipoValor, estado, explicacion);
    }

    private static string Explicar(string tipo, string tipoValor)
    {
        if (tipo == tipoValor) return $"Mismo tipo ({tipo}).";
        if (Tipos.EsAmpliacion(tipoValor, tipo)) return $"Java convierte {tipoValor} a {tipo} sin perder información.";
        return $"El valor cabe en {tipo}.";
    }

    private bool ConErrorPrevio(Token t)
        => _nombresConError.TryGetValue(t.Lexema, out int linea) && linea <= t.Linea;

    private string ResolverTipo(Token t)
    {
        if (t.Codigo != ID) return t.Lexema;
        if (t.Lexema == _clase || _externas.Contains(t.Lexema)) return t.Lexema;
        if (!ConErrorPrevio(t)) Error(t, t.Lexema);
        return Tipos.Desconocido;
    }

    private void Fila(int linea, string variable, string tipo, string valor, string tipoValor, string estado, string explicacion)
        => _r.Filas.Add(new FilaSemantica(linea, variable, tipo, valor, tipoValor, estado, explicacion));

    private void Error(Token t, string mensaje)
    {
        if (_r.Errores.Any(e => e.Linea == t.Linea && e.Mensaje == mensaje)) return;
        _r.Errores.Add(new ErrorCompilacion { Linea = t.Linea, Columna = t.Columna, Tipo = TipoError.Semantico, Mensaje = mensaje });
    }
}
