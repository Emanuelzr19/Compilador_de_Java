using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CompiladorJava.Modelos;

public sealed record FilaToken(string Id, string Lexema, string Token, int Linea);

public static class EstadoTipos
{
    public const string Correcto = "Correcto";
    public const string Incorrecto = "Incorrecto";
    public const string NoVerificable = "No verificable";
}

public sealed record FilaSemantica(
    int Linea,
    string Variable,
    string Tipo,
    string Valor,
    string TipoValor,
    string Estado,
    string Explicacion)
{
    public bool Correcto => Estado == EstadoTipos.Correcto;
}

public sealed class Instruccion
{
    public int Linea { get; init; }
    public string Texto { get; init; } = "";
    public string Regla { get; init; } = "";
    public string Produccion { get; init; } = "";
    public string Derivacion { get; init; } = "";
    public bool Valida { get; init; }
    public string Motivo { get; init; } = "";
    public int Nivel { get; init; }
    public List<Token> Tokens { get; init; } = new();
}

public static class TextoCodigo
{
    public static string Unir(IEnumerable<Token> tokens)
    {
        var sb = new StringBuilder();
        string anterior = "";
        foreach (var t in tokens)
        {
            if (anterior.Length > 0 && NecesitaEspacio(anterior[^1], t.Lexema[0])) sb.Append(' ');
            sb.Append(t.Lexema);
            anterior = t.Lexema;
        }
        return sb.ToString();
    }

    public static bool NecesitaEspacio(char previo, char siguiente)
        => (EsPalabra(previo) && EsPalabra(siguiente))
           || CompiladorJava.Lexico.CatalogoTokens.OperadoresDobles.ContainsKey(new string(new[] { previo, siguiente }));

    private static bool EsPalabra(char c) => char.IsLetterOrDigit(c) || c == '_' || c == '$' || c == '"' || c == '\'';
}
