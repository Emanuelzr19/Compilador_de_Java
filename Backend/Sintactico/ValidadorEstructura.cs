using System.Collections.Generic;
using CompiladorJava.Modelos;
using static CompiladorJava.Modelos.CodigosToken;

namespace CompiladorJava.Sintactico;

public static class ValidadorEstructura
{
    private static readonly string[] FirmaMain = { MA, ST, TM, PMAIN, PA, STR, COA, COC, ID, PC, LLA };

    public static List<ErrorCompilacion> Validar(IReadOnlyList<Token> t)
    {
        var errores = new List<ErrorCompilacion>();

        int iClase = -1;
        for (int k = 0; k < t.Count && iClase < 0; k++)
            if (t[k].Codigo == CL) iClase = k;

        if (iClase < 0)
        {
            Agregar(errores, t.Count > 0 ? t[0] : null, "Falta class");
            return errores;
        }

        if (iClase + 1 >= t.Count || t[iClase + 1].Codigo != ID)
        {
            Agregar(errores, t[iClase], "Falta nombre de la clase");
            return errores;
        }

        int iAbre = iClase + 2;
        while (iAbre < t.Count && t[iAbre].Codigo != LLA) iAbre++;
        if (iAbre >= t.Count)
        {
            Agregar(errores, t[iClase], "Falta {");
            return errores;
        }

        int profundidad = 0;
        int iCierra = -1;
        for (int k = iAbre; k < t.Count && iCierra < 0; k++)
        {
            if (t[k].Codigo == LLA) profundidad++;
            else if (t[k].Codigo == LLC && --profundidad == 0) iCierra = k;
        }

        if (iCierra < 0)
        {
            Agregar(errores, t[iAbre], "Falta }");
            return errores;
        }

        bool hayMain = false;
        bool firmaCorrecta = false;
        profundidad = 0;
        for (int k = iAbre; k < iCierra && !firmaCorrecta; k++)
        {
            if (t[k].Codigo == LLA) profundidad++;
            else if (t[k].Codigo == LLC) profundidad--;
            else if (profundidad == 1 && t[k].Codigo == PMAIN) hayMain = true;
            else if (profundidad == 1 && t[k].Codigo == MA) firmaCorrecta = CoincideFirma(t, k);
        }

        if (!firmaCorrecta)
        {
            string mensaje = hayMain
                ? "Firma de main incorrecta"
                : "Falta método main";
            Agregar(errores, t[iClase + 1], mensaje);
        }

        return errores;
    }

    private static bool CoincideFirma(IReadOnlyList<Token> t, int inicio)
    {
        if (inicio + FirmaMain.Length > t.Count) return false;
        for (int j = 0; j < FirmaMain.Length; j++)
            if (t[inicio + j].Codigo != FirmaMain[j]) return false;
        return true;
    }

    private static void Agregar(List<ErrorCompilacion> errores, Token? donde, string mensaje)
        => errores.Add(new ErrorCompilacion
        {
            Linea = donde?.Linea ?? 1,
            Columna = donde?.Columna ?? 1,
            Tipo = TipoError.Sintactico,
            Mensaje = mensaje
        });
}
