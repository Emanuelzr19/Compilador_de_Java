using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static CompiladorJava.Modelos.CodigosToken;

namespace CompiladorJava.Sintactico;

public sealed partial class AnalizadorSintactico
{
    private void Expresion()
    {
        Operando();
        while (true)
        {
            if (Es(OPA, OPR, OPL))
            {
                Tomar();
                Operando();
            }
            else if (EsOperandoSuelto())
            {
                throw Falla(Es(VTS) || Ultimo().Codigo == VTS ? "Falta +" : "Falta operador");
            }
            else
            {
                break;
            }
        }
    }

    private bool EsOperandoSuelto()
        => Es(PA) || (Es(VTE, VTD, VTF, VTC, VTS, VTB, ID) && Ver().Linea == Ultimo().Linea);

    private bool EsConversion() => Es(PA) && EsEn(1, TD) && EsEn(2, PC);

    private void Operando()
    {
        if (Es(INC, DEC, OPB, OPN) || EsSigno())
        {
            Tomar();
            Operando();
        }
        else if (EsConversion())
        {
            Tomar();
            Tomar();
            Tomar();
            Operando();
        }
        else if (Es(PA))
        {
            Tomar();
            Expresion();
            Esperar(PC, "expresión entre paréntesis");
        }
        else if (Es(ID, VTS) && EsEn(1, CA))
        {
            Acceso();
        }
        else if (Es(ID) && EsEn(1, PA))
        {
            throw Falla(Ver().Lexema);
        }
        else if (Es(NW))
        {
            NuevoObjeto();
        }
        else
        {
            Valor();
        }
    }

    private int NuevoObjeto()
    {
        Tomar();
        Esperar(ID, "clase");
        if (Es(COA)) throw Falla(Ver().Lexema);
        Argumentos();

        int resultado = 0;
        while (Es(CA))
        {
            Tomar();
            Esperar(ID, "nombre del método o atributo");
            if (Es(PA))
            {
                Argumentos();
                resultado = 2;
            }
            else
            {
                resultado = 1;
            }
        }
        return resultado;
    }

    private void NuevoComoInstruccion()
    {
        int resultado = NuevoObjeto();
        if (Es(ODA)) throw Falla(Ver().Lexema);
        if (resultado == 1) throw Falla("Falta (");
        Esperar(FI, "creación de objeto");
    }

    private bool EsSigno() => Es(OPA) && (Ver().Lexema == "+" || Ver().Lexema == "-");

    private void ValorConSigno()
    {
        if (EsSigno()) Tomar();
        Valor();
    }

    private void Valor()
    {
        if (Es(VTE, VTD, VTF, VTC, VTS, VTB, ID)) Tomar();
        else throw FallaEsperaba("valor");
    }

    private bool Acceso()
    {
        Tomar();
        bool llamada = false;
        while (Es(CA))
        {
            Tomar();
            Esperar(ID, "nombre del método o atributo después de '.'");
            llamada = Es(PA);
            if (llamada) Argumentos();
        }
        return llamada;
    }

    private void Argumentos()
    {
        Esperar(PA, "argumentos");
        if (!Es(PC))
        {
            ValorConSigno();
            while (true)
            {
                if (Es(CO))
                {
                    Tomar();
                    ValorConSigno();
                }
                else if (EsOperandoSuelto())
                {
                    throw Falla("Falta ,");
                }
                else
                {
                    break;
                }
            }
        }
        Esperar(PC, "argumentos");
    }
}
