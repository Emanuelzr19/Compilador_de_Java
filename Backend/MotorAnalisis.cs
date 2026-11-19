using System;
using System.IO;
using System.Linq;
using CompiladorJava.Depuracion;
using CompiladorJava.Lexico;
using CompiladorJava.Modelos;
using CompiladorJava.Semantico;
using CompiladorJava.Sintactico;

namespace CompiladorJava;

public sealed class MotorAnalisis
{
    public ResultadoAnalisis AnalizarArchivo(string ruta)
    {
        var resultado = Analizar(File.ReadAllText(ruta));
        resultado.RutaArchivo = ruta;
        return resultado;
    }

    public ResultadoAnalisis Analizar(string? codigo)
    {
        codigo ??= "";
        var resultado = new ResultadoAnalisis { CodigoOriginal = codigo };

        try
        {
            Ejecutar(codigo, resultado);
        }
        catch (Exception ex)
        {
            resultado.Errores.Add(new ErrorCompilacion
            {
                Linea = 1,
                Columna = 1,
                Tipo = TipoError.Sintactico,
                Mensaje = "No se pudo analizar el código: " + ex.Message
            });
        }

        var ordenados = resultado.Errores.OrderBy(e => e.Linea).ThenBy(e => e.Columna).ThenBy(e => e.Tipo).ToList();
        resultado.Errores.Clear();
        resultado.Errores.AddRange(ordenados);
        return resultado;
    }

    private static void Ejecutar(string codigo, ResultadoAnalisis r)
    {
        r.CodigoDepurado = Depurador.Depurar(codigo);

        var lexico = new AnalizadorLexico().Analizar(codigo);
        var estructura = ValidadorEstructura.Validar(lexico.Tokens);
        if (estructura.Count > 0)
        {
            r.Bloqueado = true;
            r.Errores.AddRange(estructura);
            return;
        }

        var sintactico = new AnalizadorSintactico(lexico.Tokens, lexico.Errores.Where(e => e.SuprimeSintaxis).Select(e => e.Linea)).Analizar();
        var semantico = new AnalizadorSemantico(sintactico.Instrucciones, sintactico.NombresConError).Analizar();

        r.Tokens.AddRange(TablaSimbolos.Construir(lexico.Tokens));
        r.Instrucciones.AddRange(sintactico.Instrucciones);
        r.Semantica.AddRange(semantico.Filas);
        r.Errores.AddRange(lexico.Errores);
        r.Errores.AddRange(sintactico.Errores);
        r.Errores.AddRange(semantico.Errores);
    }
}
