using System.Collections.Generic;
using CompiladorJava.Modelos;
using static CompiladorJava.Modelos.CodigosToken;

namespace CompiladorJava.Lexico;

public sealed class ResultadoLexico
{
    public List<Token> Tokens { get; } = new();
    public List<ErrorCompilacion> Errores { get; } = new();
}

public sealed class AnalizadorLexico
{
    private string _s = "";
    private int _i;
    private int _linea;
    private int _columna;
    private ResultadoLexico _r = new();

    private char Ver(int adelante = 0) => _i + adelante < _s.Length ? _s[_i + adelante] : '\0';

    private void Avanzar()
    {
        if (_i >= _s.Length) return;
        if (_s[_i] == '\n')
        {
            _linea++;
            _columna = 1;
        }
        else
        {
            _columna++;
        }
        _i++;
    }

    public ResultadoLexico Analizar(string? codigo)
    {
        _s = codigo ?? "";
        _i = 0;
        _linea = 1;
        _columna = 1;
        _r = new ResultadoLexico();

        while (_i < _s.Length)
        {
            char c = Ver();
            int linea = _linea, columna = _columna, inicio = _i;

            if (char.IsWhiteSpace(c)) Avanzar();
            else if (c == '/' && Ver(1) == '/') ComentarioDeLinea();
            else if (c == '/' && Ver(1) == '*') ComentarioDeBloque(linea, columna);
            else if (char.IsLetter(c) || c == '_' || c == '$') Palabra(linea, columna, inicio);
            else if (char.IsDigit(c)) Numero(linea, columna, inicio);
            else if (c == '"') Literal('"', VTS, linea, columna, inicio);
            else if (c == '\'') Literal('\'', VTC, linea, columna, inicio);
            else Operador(c, linea, columna, inicio);
        }

        MarcarMain();
        return _r;
    }

    private void ComentarioDeLinea()
    {
        while (_i < _s.Length && Ver() != '\n') Avanzar();
    }

    private void ComentarioDeBloque(int linea, int columna)
    {
        Avanzar();
        Avanzar();
        while (_i < _s.Length && !(Ver() == '*' && Ver(1) == '/')) Avanzar();
        if (_i >= _s.Length)
        {
            Error(linea, columna, "Comentario de bloque sin cerrar (falta */)");
            return;
        }
        Avanzar();
        Avanzar();
    }

    private void Palabra(int linea, int columna, int inicio)
    {
        while (char.IsLetterOrDigit(Ver()) || Ver() == '_' || Ver() == '$') Avanzar();
        string lexema = _s.Substring(inicio, _i - inicio);
        string codigo = CatalogoTokens.PalabrasReservadas.TryGetValue(lexema, out var c) ? c : ID;
        _r.Tokens.Add(new Token(codigo, lexema, linea, columna));
    }

    private void Numero(int linea, int columna, int inicio)
    {
        string codigo = VTE;
        while (char.IsDigit(Ver())) Avanzar();

        if (Ver() == '.' && char.IsDigit(Ver(1)))
        {
            codigo = VTD;
            Avanzar();
            while (char.IsDigit(Ver())) Avanzar();
        }

        if ((Ver() == 'f' || Ver() == 'F') && !char.IsLetterOrDigit(Ver(1)) && Ver(1) != '_')
        {
            codigo = VTF;
            Avanzar();
        }

        if (char.IsLetterOrDigit(Ver()) || Ver() == '_' || Ver() == '$' || (Ver() == '.' && char.IsDigit(Ver(1))))
        {
            while (char.IsLetterOrDigit(Ver()) || Ver() == '_' || Ver() == '$' || (Ver() == '.' && char.IsDigit(Ver(1)))) Avanzar();
            Error(linea, columna, _s.Substring(inicio, _i - inicio));
            return;
        }

        _r.Tokens.Add(new Token(codigo, _s.Substring(inicio, _i - inicio), linea, columna));
    }

    private void Literal(char comilla, string codigo, int linea, int columna, int inicio)
    {
        string nombre = comilla == '"' ? "Cadena" : "Carácter";
        int simbolos = 0;
        Avanzar();

        while (_i < _s.Length && Ver() != '\n' && Ver() != comilla)
        {
            if (Ver() == '\\')
            {
                Avanzar();
                if (_i < _s.Length && Ver() != '\n') Avanzar();
            }
            else
            {
                Avanzar();
            }
            simbolos++;
        }

        bool cerrado = Ver() == comilla;
        if (cerrado) Avanzar();
        string lexema = _s.Substring(inicio, _i - inicio).TrimEnd('\r');

        if (!cerrado)
        {
            Error(linea, columna, lexema, false);
            _r.Tokens.Add(new Token(codigo, lexema, linea, columna));
        }
        else if (comilla == '\'' && simbolos != 1) Error(linea, columna, lexema);
        else _r.Tokens.Add(new Token(codigo, lexema, linea, columna));
    }

    private void Operador(char c, int linea, int columna, int inicio)
    {
        string par = new string(new[] { c, Ver(1) });
        if (CatalogoTokens.OperadoresDobles.TryGetValue(par, out var codigoDoble))
        {
            Avanzar();
            Avanzar();
            _r.Tokens.Add(new Token(codigoDoble, par, linea, columna));
            return;
        }

        Avanzar();
        if (CatalogoTokens.OperadoresSimples.TryGetValue(c, out var codigo))
            _r.Tokens.Add(new Token(codigo, c.ToString(), linea, columna));
        else
            Error(linea, columna, c.ToString());
    }

    private void MarcarMain()
    {
        var t = _r.Tokens;
        for (int k = 2; k < t.Count - 1; k++)
        {
            if (t[k].Codigo == ID && t[k].Lexema == "main" && t[k - 1].Codigo == TM && t[k - 2].Codigo == ST && t[k + 1].Codigo == PA)
                t[k] = t[k] with { Codigo = PMAIN };
        }
    }

    private void Error(int linea, int columna, string mensaje, bool suprimeSintaxis = true)
        => _r.Errores.Add(new ErrorCompilacion { Linea = linea, Columna = columna, Tipo = TipoError.Lexico, Mensaje = mensaje, SuprimeSintaxis = suprimeSintaxis });
}
