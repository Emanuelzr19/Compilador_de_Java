using System.Collections.Generic;
namespace CompiladorJava.Sintactico;

public static class Gramatica
{
    public const string DPACKAGE = "DPACKAGE";
    public const string IMPORT = "IMPORT";
    public const string CLASE = "CLASE";
    public const string MAIN = "MAIN";
    public const string METODO = "METODO";
    public const string D_GLOB = "D_GLOB";
    public const string DEC = "DEC";
    public const string ASIG = "ASIG";
    public const string OBJ = "OBJ";
    public const string ACC = "ACC";
    public const string SOUT = "SOUT";
    public const string REE = "REE";
    public const string INST = "INST";
    public const string CONTROL = "CONTROL";

    public static readonly IReadOnlyDictionary<string, string> Producciones = new Dictionary<string, string>
    {
        [DPACKAGE] = "DPACKAGE ::= PCK ID FI",
        [IMPORT] = "IMPORT ::= IMP ID CA ID FI",
        [CLASE] = "CLASE ::= MA CL ID LLA CONTENT LLC",
        [MAIN] = "MAIN ::= MA ST TM PMAIN PA STR COA COC ID PC LLA CUERPO LLC",
        [METODO] = "METODO ::= MA TYPE ID PA PARAMS PC LLA CUERPO REE LLC",
        [D_GLOB] = "D_GLOB ::= MAO TY ID FI | MAO TY ID ODA EXP FI",
        [DEC] = "DEC ::= TY ID FI | TY ID ODA EXP FI",
        [ASIG] = "ASIG ::= ID ODA EXP FI",
        [OBJ] = "OBJ ::= ID ID ODA NW ID PA ARGS PC FI",
        [ACC] = "ACC ::= CLA OT FI",
        [SOUT] = "SOUT ::= ID CA ID CA ID PA EXP PC FI",
        [REE] = "REE ::= RTN FI | RTN EXP FI",
        [INST] = "INST ::= DEC | STAT_B | ASIG | ACC | OBJ",
        [CONTROL] = "STATMNT (if, switch, while, for, do): fuera del alcance"
    };
}
