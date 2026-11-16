using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CompiladorJava.Modelos;
using static CompiladorJava.Modelos.CodigosToken;

namespace CompiladorJava.Lexico;

public static class TablaSimbolos
{
    public static List<FilaToken> Construir(IReadOnlyList<Token> tokens)
    {
        var filas = new List<FilaToken>();
        var globales = new Dictionary<string, int>();
        var locales = new Dictionary<string, int>();
        var sinDeclarar = new Dictionary<string, int>();
        int contador = 0;
        int llaves = 0;
        int parentesis = 0;
        bool enPaqueteOImport = false;

        for (int k = 0; k < tokens.Count; k++)
        {
            var t = tokens[k];

            if (t.Codigo != ID)
            {
                filas.Add(new FilaToken("", t.Lexema, t.Codigo, t.Linea));
                Actualizar(t.Codigo);
                continue;
            }

            string anterior = k > 0 ? tokens[k - 1].Codigo : "";
            string siguiente = k + 1 < tokens.Count ? tokens[k + 1].Codigo : "";
            bool esMiembro = anterior == CA && !enPaqueteOImport;

            if (enPaqueteOImport || anterior == CL)
                Declarar(t, globales);
            else if (siguiente == PA && (anterior is TD or STR or TM or ID or COC))
                Declarar(t, globales);
            else if (anterior is TD or STR or TM or ID or COC)
                Declarar(t, parentesis > 0 || llaves >= 2 ? locales : globales);
            else
                Usar(t, esMiembro, globales, locales, sinDeclarar);
        }

        return filas;

        void Actualizar(string codigo)
        {
            switch (codigo)
            {
                case LLA: llaves++; break;
                case LLC:
                    llaves--;
                    if (llaves <= 1) locales.Clear();
                    break;
                case PA: parentesis++; break;
                case PC: parentesis--; break;
                case PCK:
                case IMP: enPaqueteOImport = true; break;
                case FI: enPaqueteOImport = false; break;
            }
        }

        void Declarar(Token t, Dictionary<string, int> ambito)
        {
            if (ambito.TryGetValue(t.Lexema, out int existente))
            {
                filas.Add(new FilaToken("", "ID" + existente, ID, t.Linea));
                return;
            }

            int numero;
            if (ReferenceEquals(ambito, globales) && sinDeclarar.Remove(t.Lexema, out int previo))
            {
                numero = previo;
                ambito[t.Lexema] = numero;
                filas.Add(new FilaToken("", "ID" + numero, ID, t.Linea));
                return;
            }

            numero = ++contador;
            ambito[t.Lexema] = numero;
            filas.Add(new FilaToken(numero.ToString(), t.Lexema, ID, t.Linea));
        }

        void Usar(Token t, bool esMiembro, Dictionary<string, int> glob, Dictionary<string, int> loc, Dictionary<string, int> pendientes)
        {
            int numero;
            if (!esMiembro && loc.TryGetValue(t.Lexema, out numero)) { filas.Add(new FilaToken("", "ID" + numero, ID, t.Linea)); return; }
            if (glob.TryGetValue(t.Lexema, out numero)) { filas.Add(new FilaToken("", "ID" + numero, ID, t.Linea)); return; }
            if (CatalogoTokens.NombresDeJava.Contains(t.Lexema)) { filas.Add(new FilaToken("", t.Lexema, ID, t.Linea)); return; }
            if (pendientes.TryGetValue(t.Lexema, out numero)) { filas.Add(new FilaToken("", "ID" + numero, ID, t.Linea)); return; }

            numero = ++contador;
            pendientes[t.Lexema] = numero;
            filas.Add(new FilaToken(numero.ToString(), t.Lexema, ID, t.Linea));
        }
    }
}
