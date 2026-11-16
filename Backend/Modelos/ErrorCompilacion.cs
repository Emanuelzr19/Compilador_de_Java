using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
namespace CompiladorJava.Modelos;

public enum TipoError
{
    Lexico,
    Sintactico,
    Semantico
}

public sealed class ErrorCompilacion
{
    public int Linea { get; init; }
    public int Columna { get; init; }
    public TipoError Tipo { get; init; }
    public string Mensaje { get; init; } = "";
    public bool SuprimeSintaxis { get; init; } = true;

    public string TipoTexto => Tipo switch
    {
        TipoError.Lexico => "Léxico",
        TipoError.Sintactico => "Sintáctico",
        _ => "Semántico"
    };

    public string ColorSugerido => Tipo switch
    {
        TipoError.Lexico => "#2E7D32",
        TipoError.Sintactico => "#C62828",
        _ => "#1565C0"
    };

    public override string ToString() => $"[{TipoTexto}] Línea {Linea}: {Mensaje}";
}
