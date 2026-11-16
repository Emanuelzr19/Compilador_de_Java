using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CompiladorJava.Modelos;
using static CompiladorJava.Modelos.CodigosToken;

namespace CompiladorJava.Semantico;

public sealed partial class AnalizadorSemantico
{
    private const string ClaseMath = "#Math";
    private const string ClaseSystem = "#System";
    private const string ClaseSalida = "#PrintStream";

    private static readonly Dictionary<string, (string[] Parametros, string Retorno)> MetodosMath = new()
    {
        ["random"] = (Array.Empty<string>(), "double"),
        ["sqrt"] = (new[] { "num" }, "double"),
        ["pow"] = (new[] { "num", "num" }, "double"),
        ["abs"] = (new[] { "num" }, "="),
        ["max"] = (new[] { "num", "num" }, "+"),
        ["min"] = (new[] { "num", "num" }, "+"),
        ["floor"] = (new[] { "num" }, "double"),
        ["ceil"] = (new[] { "num" }, "double"),
        ["round"] = (new[] { "num" }, "long")
    };

    private static readonly Dictionary<string, (string[] Parametros, string Retorno)> MetodosString = new()
    {
        ["length"] = (Array.Empty<string>(), "int"),
        ["charAt"] = (new[] { "int" }, "char"),
        ["isEmpty"] = (Array.Empty<string>(), "boolean"),
        ["toUpperCase"] = (Array.Empty<string>(), "String"),
        ["toLowerCase"] = (Array.Empty<string>(), "String"),
        ["trim"] = (Array.Empty<string>(), "String"),
        ["equals"] = (new[] { "String" }, "boolean"),
        ["contains"] = (new[] { "String" }, "boolean"),
        ["concat"] = (new[] { "String" }, "String"),
        ["compareTo"] = (new[] { "String" }, "int")
    };

    private List<Token> _e = new();
    private int _i;
    private int _fin;

    private string TipoDe(List<Token> tokens, int desde, int hasta)
    {
        _e = tokens;
        _i = desde;
        _fin = hasta;
        return TipoExp(1);
    }

    private static int Precedencia(Token t) => t.Codigo is OPA or OPR or OPL
        ? t.Lexema switch
        {
            "||" => 1,
            "&&" => 2,
            "|" => 3,
            "&" => 4,
            "==" or "!=" => 5,
            "<" or ">" or "<=" or ">=" => 6,
            "+" or "-" => 7,
            _ => 8
        }
        : 0;

    private string TipoExp(int minima)
    {
        string izquierdo = TipoOperando();
        while (_i < _fin && Precedencia(_e[_i]) >= minima && Precedencia(_e[_i]) > 0)
        {
            var operador = _e[_i++];
            string derecho = TipoExp(Precedencia(operador) + 1);
            izquierdo = Combinar(operador, izquierdo, derecho);
        }
        return izquierdo;
    }

    private string Combinar(Token operador, string izquierdo, string derecho)
    {
        if (operador.Lexema == "+" && (izquierdo == "String" || derecho == "String")) return "String";
        if (izquierdo == Tipos.Desconocido || derecho == Tipos.Desconocido)
            return operador.Codigo is OPR or OPL ? "boolean" : Tipos.Desconocido;

        string? resultado = Tipos.Operacion(operador.Lexema, izquierdo, derecho);
        if (resultado != null) return resultado;

        Error(operador, operador.Lexema);
        return Tipos.Desconocido;
    }

    private string TipoOperando()
    {
        if (_i >= _fin) return Tipos.Desconocido;
        var t = _e[_i];

        if (t.Codigo is INC or DEC or OPB or OPN || (t.Codigo == OPA && (t.Lexema == "+" || t.Lexema == "-")))
        {
            _i++;
            return TipoUnario(t, TipoOperando());
        }

        if (t.Codigo == PA && _i + 2 < _fin && _e[_i + 1].Codigo == TD && _e[_i + 2].Codigo == PC)
            return TipoConversion();

        if (t.Codigo == NW) return TipoNuevo();

        if (t.Codigo == PA)
        {
            _i++;
            string interno = TipoExp(1);
            if (_i < _fin && _e[_i].Codigo == PC) _i++;
            return interno;
        }

        if ((t.Codigo == ID || t.Codigo == VTS) && _i + 1 < _fin && _e[_i + 1].Codigo == CA) return TipoAcceso();

        return TipoValor();
    }

    private string TipoValor()
    {
        if (_i >= _fin) return Tipos.Desconocido;
        var t = _e[_i++];
        return t.Codigo == ID ? TipoVariable(t) : Tipos.DeLiteral(t.Codigo);
    }

    private string TipoVariable(Token t)
    {
        string? tipo = BuscarVariable(t);
        if (tipo != null)
        {
            if (_sinInicializar.Contains(t.Lexema) && _locales.ContainsKey(t.Lexema)) Error(t, t.Lexema);
            return tipo;
        }
        if (!ConErrorPrevio(t)) Error(t, t.Lexema);
        return Tipos.Desconocido;
    }

    private string TipoUnario(Token operador, string tipo)
    {
        if (tipo == Tipos.Desconocido) return operador.Codigo == OPN ? "boolean" : Tipos.Desconocido;

        bool valido = operador.Codigo switch
        {
            OPN => tipo == "boolean",
            OPB => Tipos.EsEntero(tipo),
            _ => Tipos.EsNumerico(tipo)
        };

        if (!valido)
        {
            Error(operador, operador.Lexema);
            return Tipos.Desconocido;
        }

        return operador.Codigo == OPB ? Tipos.Mayor(tipo, "int") : tipo;
    }

    private string TipoConversion()
    {
        var destino = _e[_i + 1];
        _i += 3;
        int inicio = _i;
        string origen = TipoOperando();

        if (origen == Tipos.Desconocido || _i == inicio) return destino.Lexema;

        bool valido = destino.Lexema == "boolean" ? origen == "boolean" : Tipos.EsNumerico(origen);
        if (valido) return destino.Lexema;

        Error(_e[inicio], TextoCodigo.Unir(_e.Skip(inicio).Take(_i - inicio)));
        return Tipos.Desconocido;
    }

    private string TipoNuevo()
    {
        _i++;
        if (_i >= _fin) return Tipos.Desconocido;

        var clase = _e[_i++];
        string actual = ResolverTipo(clase);

        if (_i < _fin && _e[_i].Codigo == PA)
        {
            var argumentos = TiposArgumentos();
            if (actual == _clase && argumentos.Count > 0) Error(clase, clase.Lexema);
        }

        return TipoCadena(actual);
    }

    private string TipoAcceso()
    {
        var baseToken = _e[_i++];
        string actual;

        if (baseToken.Codigo == VTS) actual = "String";
        else if (_locales.ContainsKey(baseToken.Lexema) || _campos.ContainsKey(baseToken.Lexema)) actual = TipoVariable(baseToken);
        else if (baseToken.Lexema == "Math") actual = ClaseMath;
        else if (baseToken.Lexema == "System") actual = ClaseSystem;
        else if (baseToken.Lexema == _clase || _externas.Contains(baseToken.Lexema)) actual = Tipos.Desconocido;
        else actual = TipoVariable(baseToken);

        return TipoCadena(actual);
    }

    private string TipoCadena(string actual)
    {
        while (_i < _fin && _e[_i].Codigo == CA)
        {
            _i++;
            var miembro = _e[_i++];
            bool esLlamada = _i < _fin && _e[_i].Codigo == PA;
            var argumentos = esLlamada ? TiposArgumentos() : new List<string>();
            actual = TipoMiembro(actual, miembro, esLlamada, argumentos);
        }
        return actual;
    }

    private List<string> TiposArgumentos()
    {
        var argumentos = new List<string>();
        _i++;
        while (_i < _fin && _e[_i].Codigo != PC)
        {
            if (_e[_i].Codigo == CO)
            {
                _i++;
            }
            else if (_e[_i].Codigo == OPA && (_e[_i].Lexema == "+" || _e[_i].Lexema == "-"))
            {
                var signo = _e[_i++];
                argumentos.Add(TipoUnario(signo, TipoValor()));
            }
            else
            {
                argumentos.Add(TipoValor());
            }
        }
        _i++;
        return argumentos;
    }

    private string TipoMiembro(string actual, Token miembro, bool esLlamada, List<string> argumentos)
    {
        if (actual == Tipos.Desconocido) return actual;
        if (actual == ClaseSystem)
        {
            if (miembro.Lexema is "out" or "err" && !esLlamada) return ClaseSalida;
            Error(miembro, miembro.Lexema);
            return Tipos.Desconocido;
        }
        if (actual == ClaseSalida)
        {
            if (esLlamada && miembro.Lexema is "print" or "println") return "void";
            Error(miembro, miembro.Lexema);
            return Tipos.Desconocido;
        }
        if (actual == ClaseMath) return LlamarApi("Math", MetodosMath, miembro, esLlamada, argumentos);
        if (actual == "String") return LlamarApi("String", MetodosString, miembro, esLlamada, argumentos);

        if (actual == "String[]")
        {
            if (miembro.Lexema == "length" && !esLlamada) return "int";
            Error(miembro, miembro.Lexema);
            return Tipos.Desconocido;
        }

        if (Tipos.EsPrimitivo(actual))
        {
            Error(miembro, miembro.Lexema);
            return Tipos.Desconocido;
        }

        if (actual != _clase) return Tipos.Desconocido;
        if (esLlamada) return LlamarMetodoPropio(miembro, argumentos);
        if (_campos.TryGetValue(miembro.Lexema, out var tipo)) return tipo;

        Error(miembro, miembro.Lexema);
        return Tipos.Desconocido;
    }

    private string LlamarApi(string clase, Dictionary<string, (string[] Parametros, string Retorno)> tabla, Token miembro, bool esLlamada, List<string> argumentos)
    {
        if (!esLlamada || !tabla.TryGetValue(miembro.Lexema, out var info))
        {
            Error(miembro, miembro.Lexema);
            return Tipos.Desconocido;
        }

        if (argumentos.Count != info.Parametros.Length)
        {
            Error(miembro, miembro.Lexema);
            return Tipos.Desconocido;
        }

        for (int k = 0; k < argumentos.Count; k++)
        {
            if (!CumpleParametro(info.Parametros[k], argumentos[k]))
                Error(miembro, miembro.Lexema);
        }

        if (argumentos.Contains(Tipos.Desconocido) && info.Retorno is "=" or "+") return Tipos.Desconocido;
        return info.Retorno switch
        {
            "=" => Tipos.Mayor(argumentos[0], "int"),
            "+" => Tipos.Mayor(argumentos[0], argumentos[1]),
            _ => info.Retorno
        };
    }

    private static bool CumpleParametro(string esperado, string tipo)
    {
        if (tipo == Tipos.Desconocido) return true;
        return esperado switch
        {
            "num" => Tipos.EsNumerico(tipo),
            "int" => tipo is "int" or "short" or "byte" or "char",
            _ => tipo == esperado
        };
    }

    private string LlamarMetodoPropio(Token miembro, List<string> argumentos)
    {
        if (!_metodos.TryGetValue(miembro.Lexema, out var lista))
        {
            Error(miembro, miembro.Lexema);
            return Tipos.Desconocido;
        }

        var metodo = lista.FirstOrDefault(m => m.Parametros.Count == argumentos.Count);
        if (metodo == null)
        {
            Error(miembro, miembro.Lexema);
            return lista[0].Retorno;
        }

        for (int k = 0; k < argumentos.Count; k++)
        {
            if (!Tipos.Asignable(metodo.Parametros[k], argumentos[k], null))
                Error(miembro, miembro.Lexema);
        }

        return metodo.Retorno;
    }
}
