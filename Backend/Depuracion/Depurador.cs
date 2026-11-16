using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CompiladorJava.Modelos;

namespace CompiladorJava.Depuracion;

public static class Depurador
{
    public static string Depurar(string? codigo)
    {
        string s = codigo ?? "";
        var salida = new StringBuilder(s.Length);
        bool hayEspacio = false;
        int i = 0;

        while (i < s.Length)
        {
            char c = s[i];
            char siguiente = i + 1 < s.Length ? s[i + 1] : '\0';

            if (c == '/' && siguiente == '/')
            {
                while (i < s.Length && s[i] != '\n') i++;
                hayEspacio = true;
            }
            else if (c == '/' && siguiente == '*')
            {
                int fin = s.IndexOf("*/", i + 2, StringComparison.Ordinal);
                i = fin < 0 ? s.Length : fin + 2;
                hayEspacio = true;
            }
            else if (c == '\n')
            {
                if (salida.Length > 0 && salida[^1] != '\n') salida.Append('\n');
                hayEspacio = false;
                i++;
            }
            else if (char.IsWhiteSpace(c))
            {
                hayEspacio = true;
                i++;
            }
            else
            {
                bool lineaConTexto = salida.Length > 0 && salida[^1] != '\n';
                if (hayEspacio && lineaConTexto && TextoCodigo.NecesitaEspacio(salida[^1], c)) salida.Append(' ');
                hayEspacio = false;
                if (c == '"' || c == '\'') i = CopiarLiteral(s, i, salida);
                else
                {
                    salida.Append(c);
                    i++;
                }
            }
        }

        return UnirLlavesDeCierre(salida.ToString());
    }

    private static string UnirLlavesDeCierre(string texto)
    {
        var lineas = new List<string>();
        foreach (string linea in texto.Split('\n'))
        {
            if (linea.Length == 0) continue;
            if (lineas.Count > 0 && linea.All(ch => ch == '}')) lineas[^1] += linea;
            else lineas.Add(linea);
        }
        return string.Join("\n", lineas);
    }

    private static int CopiarLiteral(string s, int i, StringBuilder salida)
    {
        char comilla = s[i];
        salida.Append(comilla);
        i++;
        while (i < s.Length && s[i] != '\n')
        {
            char c = s[i];
            salida.Append(c);
            i++;
            if (c == '\\' && i < s.Length && s[i] != '\n')
            {
                salida.Append(s[i]);
                i++;
            }
            else if (c == comilla)
            {
                break;
            }
        }
        return i;
    }
}
