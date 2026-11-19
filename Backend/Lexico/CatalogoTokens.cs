using System.Collections.Generic;
using static CompiladorJava.Modelos.CodigosToken;

namespace CompiladorJava.Lexico;

public static class CatalogoTokens
{
    public static readonly IReadOnlyDictionary<string, string> PalabrasReservadas = new Dictionary<string, string>
    {
        ["int"] = TD, ["boolean"] = TD, ["double"] = TD, ["float"] = TD,
        ["long"] = TD, ["short"] = TD, ["byte"] = TD, ["char"] = TD,
        ["String"] = STR,
        ["public"] = MA, ["protected"] = MA, ["private"] = MA, ["abstract"] = MA,
        ["class"] = CL, ["void"] = TM, ["static"] = ST,
        ["try"] = ISE, ["catch"] = CE, ["finally"] = FIN, ["throws"] = TH,
        ["synchronized"] = SY, ["assert"] = ASR,
        ["import"] = IMP, ["package"] = PCK, ["return"] = RTN,
        ["break"] = BR, ["case"] = CS, ["default"] = DF,
        ["if"] = PRIF, ["else"] = PRE, ["switch"] = PRSW, ["while"] = PRW, ["for"] = PRF, ["do"] = PRDO,
        ["true"] = VTB, ["false"] = VTB,
        ["super"] = SPR, ["instanceof"] = INOF, ["extends"] = EXTS, ["implements"] = IMPL,
        ["new"] = NW, ["this"] = RF, ["final"] = TDD, ["null"] = NU
    };

    public static readonly IReadOnlyDictionary<string, string> OperadoresDobles = new Dictionary<string, string>
    {
        ["=="] = OPR, ["<="] = OPR, [">="] = OPR, ["!="] = OPR,
        ["&&"] = OPL, ["||"] = OPL, ["++"] = INC, ["--"] = DEC
    };

    public static readonly IReadOnlyDictionary<char, string> OperadoresSimples = new Dictionary<char, string>
    {
        ['='] = ODA, [';'] = FI, ['{'] = LLA, ['}'] = LLC, ['('] = PA, [')'] = PC,
        ['['] = COA, [']'] = COC, [','] = CO, ['.'] = CA, [':'] = DOP, ['?'] = OPT,
        ['+'] = OPA, ['-'] = OPA, ['*'] = OPA, ['/'] = OPA, ['%'] = OPA,
        ['<'] = OPR, ['>'] = OPR, ['&'] = OPL, ['|'] = OPL,
        ['!'] = OPN, ['~'] = OPB, ['^'] = XOR
    };

    public static readonly IReadOnlySet<string> NombresDeJava = new HashSet<string>
    {
        "System", "out", "err", "print", "println", "Math", "random", "sqrt", "pow", "abs", "max", "min",
        "floor", "ceil", "round", "length", "charAt", "isEmpty", "toUpperCase", "toLowerCase", "trim",
        "equals", "contains", "concat", "compareTo", "Scanner", "nextInt", "nextDouble", "nextLine"
    };

    public static string Simbolo(string codigo) => codigo switch
    {
        FI => ";",
        LLA => "{",
        LLC => "}",
        PA => "(",
        PC => ")",
        COA => "[",
        COC => "]",
        CO => ",",
        CA => ".",
        ODA => "=",
        ID => "identificador",
        STR => "String",
        MA => "public",
        CL => "class",
        ST => "static",
        TM => "void",
        PMAIN => "main",
        NW => "new",
        PCK => "package",
        IMP => "import",
        _ => codigo
    };

    public static string Descripcion(string codigo) => codigo switch
    {
        FI => "';'",
        LLA => "'{'",
        LLC => "'}'",
        PA => "'('",
        PC => "')'",
        COA => "'['",
        COC => "']'",
        CO => "','",
        CA => "'.'",
        ODA => "'='",
        ID => "un identificador",
        STR => "'String'",
        MA => "un modificador de acceso (public, private, protected)",
        CL => "'class'",
        ST => "'static'",
        TM => "'void'",
        PMAIN => "'main'",
        NW => "'new'",
        PCK => "'package'",
        IMP => "'import'",
        EOF => "el fin del archivo",
        _ => codigo
    };
}
