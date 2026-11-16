using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
namespace CompiladorJava.Modelos;

public sealed class ResultadoAnalisis
{
    public string CodigoOriginal { get; init; } = "";
    public string? RutaArchivo { get; set; }
    public string CodigoDepurado { get; set; } = "";
    public bool Bloqueado { get; set; }
    public List<FilaToken> Tokens { get; } = new();
    public List<Instruccion> Instrucciones { get; } = new();
    public List<FilaSemantica> Semantica { get; } = new();
    public List<ErrorCompilacion> Errores { get; } = new();

    public int ErroresLexicos => Errores.Count(e => e.Tipo == TipoError.Lexico);
    public int ErroresSintacticos => Errores.Count(e => e.Tipo == TipoError.Sintactico);
    public int ErroresSemanticos => Errores.Count(e => e.Tipo == TipoError.Semantico);
}
