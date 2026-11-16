using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static CompiladorJava.Modelos.CodigosToken;

namespace CompiladorJava.Semantico;

public static class Tipos
{
    public const string Desconocido = "?";
    public const string Nulo = "null";

    private static readonly Dictionary<string, string[]> Ampliaciones = new()
    {
        ["byte"] = new[] { "short", "int", "long", "float", "double" },
        ["short"] = new[] { "int", "long", "float", "double" },
        ["char"] = new[] { "int", "long", "float", "double" },
        ["int"] = new[] { "long", "float", "double" },
        ["long"] = new[] { "float", "double" },
        ["float"] = new[] { "double" }
    };

    private static readonly Dictionary<string, (long Min, long Max)> Rangos = new()
    {
        ["byte"] = (-128, 127),
        ["short"] = (-32768, 32767),
        ["char"] = (0, 65535)
    };

    public static bool EsNumerico(string t) => t is "byte" or "short" or "char" or "int" or "long" or "float" or "double";

    public static bool EsEntero(string t) => t is "byte" or "short" or "char" or "int" or "long";

    public static bool EsPrimitivo(string t) => EsNumerico(t) || t == "boolean";

    public static bool EsAmpliacion(string valor, string variable)
        => Ampliaciones.TryGetValue(valor, out var destinos) && destinos.Contains(variable);

    public static string Mayor(string a, string b)
    {
        if (a == "double" || b == "double") return "double";
        if (a == "float" || b == "float") return "float";
        if (a == "long" || b == "long") return "long";
        return "int";
    }

    public static bool Asignable(string variable, string valor, string? literalEntero)
    {
        if (variable == Desconocido || valor == Desconocido || variable == valor) return true;
        if (EsAmpliacion(valor, variable)) return true;
        if (valor == Nulo) return !EsPrimitivo(variable);
        if (literalEntero != null && Rangos.TryGetValue(variable, out var rango) && long.TryParse(literalEntero, out long n))
            return n >= rango.Min && n <= rango.Max;
        return false;
    }

    public static string DeLiteral(string codigo) => codigo switch
    {
        VTE => "int",
        VTD => "double",
        VTF => "float",
        VTC => "char",
        VTB => "boolean",
        VTS => "String",
        NU => Nulo,
        _ => Desconocido
    };

    public static string? Operacion(string operador, string izquierdo, string derecho)
    {
        bool numeros = EsNumerico(izquierdo) && EsNumerico(derecho);

        if (operador == "+" && (izquierdo == "String" || derecho == "String")) return "String";
        if (operador is "+" or "-" or "*" or "/" or "%") return numeros ? Mayor(izquierdo, derecho) : null;
        if (operador is "<" or ">" or "<=" or ">=") return numeros ? "boolean" : null;
        if (operador is "==" or "!=") return numeros || izquierdo == derecho ? "boolean" : null;
        if (operador is "&&" or "||" or "&" or "|") return izquierdo == "boolean" && derecho == "boolean" ? "boolean" : null;
        return null;
    }
}
