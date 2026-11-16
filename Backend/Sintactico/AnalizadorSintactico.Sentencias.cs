using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CompiladorJava.Modelos;
using static CompiladorJava.Modelos.CodigosToken;

namespace CompiladorJava.Sintactico;

public sealed partial class AnalizadorSintactico
{
    private void Sentencia()
    {
        var t = Ver();
        switch (t.Codigo)
        {
            case FI:
                Tomar();
                break;
            case TD:
            case STR:
                Intentar(Gramatica.DEC, Declaracion);
                break;
            case PRIF:
            case PRE:
            case PRW:
            case PRF:
            case PRDO:
            case PRSW:
                Control();
                break;
            case ID:
                SentenciaConIdentificador();
                break;
            case NW:
                Intentar(Gramatica.ACC, NuevoComoInstruccion);
                break;
            default:
                Intentar(Gramatica.INST, () => throw Falla(MensajeNoValida(t)));
                break;
        }
    }

    private static string MensajeNoValida(Token t) => t.Lexema;

    private void SentenciaConIdentificador()
    {
        if (EsSout()) Intentar(Gramatica.SOUT, Sout);
        else if (EsObjetoSimple()) Intentar(Gramatica.OBJ, Objeto);
        else if (EsEn(1, ID)) Intentar(Gramatica.DEC, Declaracion);
        else if (EsEn(1, ODA)) Intentar(Gramatica.ASIG, Asignacion);
        else if (EsEn(1, CA)) Intentar(Gramatica.ACC, AccesoComoInstruccion);
        else if (EsEn(1, INC) || EsEn(1, DEC))
            Intentar(Gramatica.INST, () => throw Falla(Ver(1).Lexema));
        else if (EsEn(1, PA))
            Intentar(Gramatica.INST, () => throw Falla(Ver().Lexema));
        else if (EsEn(1, COA))
            Intentar(Gramatica.INST, () => throw Falla(Ver(1).Lexema));
        else
            Intentar(Gramatica.INST, () =>
            {
                Tomar();
                throw FallaEsperaba("=");
            });
    }

    private bool EsObjetoSimple()
    {
        if (!(EsEn(1, ID) && EsEn(2, ODA) && EsEn(3, NW))) return false;

        int k = 4;
        while (!EsEn(k, PC) && !EsEn(k, FI) && !EsEn(k, EOF)) k++;
        return !(EsEn(k, PC) && EsEn(k + 1, CA));
    }

    private bool EsSout()
        => Ver().Lexema == "System" && EsEn(1, CA) && Ver(2).Lexema is "out" or "err" && EsEn(3, CA)
           && (Ver(4).Lexema is "print" or "println") && EsEn(5, PA);

    private void Control()
    {
        int ini = _p;
        var t = Tomar();
        string motivo = Reportar(NuevoError(t, t.Lexema));
        SaltarEstructura(t.Codigo == PRDO);
        Registrar(ini, HastaLlave(ini, _p), Gramatica.CONTROL, false, motivo);
    }

    private void SaltarEstructura(bool esDo)
    {
        while (!Es(EOF, LLC))
        {
            if (Es(FI))
            {
                Tomar();
                return;
            }
            if (Es(LLA))
            {
                SaltarBloque();
                if (Es(PRE) || (esDo && Es(PRW)))
                {
                    Tomar();
                    continue;
                }
                return;
            }
            Tomar();
        }
    }

    private void Declaracion()
    {
        Tomar();
        if (Es(COA)) throw Falla(Ver().Lexema);
        Esperar(ID, "nombre de la variable");
        if (Es(CO)) throw Falla(Ver().Lexema);
        if (Es(ODA))
        {
            Tomar();
            Expresion();
        }
        Esperar(FI, "declaración");
    }

    private void Asignacion()
    {
        Tomar();
        Tomar();
        Expresion();
        Esperar(FI, "asignación");
    }

    private void Objeto()
    {
        Tomar();
        Tomar();
        Tomar();
        Tomar();
        Esperar(ID, "nombre de la clase después de new");
        if (Es(COA)) throw Falla(Ver().Lexema);
        Argumentos();
        Esperar(FI, "creación de objeto");
    }

    private void AccesoComoInstruccion()
    {
        bool esLlamada = Acceso();
        if (Es(ODA)) throw Falla(Ver().Lexema);
        if (!esLlamada) throw Falla("Falta (");
        Esperar(FI, "llamada a método");
    }

    private void Sout()
    {
        _p += 5;
        Esperar(PA, "System.out.print");
        if (!Es(PC)) Expresion();
        Esperar(PC, "System.out.print");
        Esperar(FI, "System.out.print");
    }

    private void Retorno()
    {
        Tomar();
        if (!Es(FI)) Expresion();
        Esperar(FI, "return");
    }
}
