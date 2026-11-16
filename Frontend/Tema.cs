using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using CompiladorJava.Modelos;

namespace CompiladorJava.Frontend;

public static class Tema
{
    public static readonly Color Fondo = ColorTranslator.FromHtml("#0F1117");
    public static readonly Color Panel = ColorTranslator.FromHtml("#161A23");
    public static readonly Color Encabezado = ColorTranslator.FromHtml("#1C2230");
    public static readonly Color FilaAlterna = ColorTranslator.FromHtml("#1A1F2B");
    public static readonly Color NumerosFondo = ColorTranslator.FromHtml("#12151D");

    public static readonly Color Texto = ColorTranslator.FromHtml("#E6EAF2");
    public static readonly Color TextoSecundario = ColorTranslator.FromHtml("#8A93A6");
    public static readonly Color Borde = ColorTranslator.FromHtml("#2A3142");

    public static readonly Color Acento = ColorTranslator.FromHtml("#3B82F6");
    public static readonly Color AcentoClaro = ColorTranslator.FromHtml("#60A5FA");
    public static readonly Color AcentoHover = ColorTranslator.FromHtml("#2563EB");
    public static readonly Color AcentoPresionado = ColorTranslator.FromHtml("#1D4ED8");

    public static readonly Color Lexico = ColorTranslator.FromHtml("#4ADE80");
    public static readonly Color Sintactico = ColorTranslator.FromHtml("#F87171");
    public static readonly Color Semantico = ColorTranslator.FromHtml("#60A5FA");
    public static readonly Color Ambar = ColorTranslator.FromHtml("#FBBF24");

    public static readonly Color AvisoFondo = ColorTranslator.FromHtml("#2A2410");

    public static readonly Font FuenteUI = new Font("Segoe UI", 10f);
    public static readonly Font FuenteUINegrita = new Font("Segoe UI", 10f, FontStyle.Bold);
    public static readonly Font FuenteCodigo = new Font("Consolas", 11f);
    public static readonly Font FuenteNumeros = new Font("Consolas", 10f);

    public static Color ColorDeError(TipoError tipo)
    {
        switch (tipo)
        {
            case TipoError.Lexico: return Lexico;
            case TipoError.Sintactico: return Sintactico;
            default: return Semantico;
        }
    }

    public static Color ColorDeEstado(string estado)
    {
        if (estado == EstadoTipos.Correcto) return Lexico;
        if (estado == EstadoTipos.Incorrecto) return Sintactico;
        return Ambar;
    }

    [DllImport("uxtheme.dll", CharSet = CharSet.Unicode)]
    private static extern int SetWindowTheme(IntPtr hWnd, string? subAppName, string? subIdList);

    [DllImport("dwmapi.dll")]
    private static extern int DwmSetWindowAttribute(IntPtr hwnd, int atributo, ref int valor, int tamano);

    public static void ScrollOscuro(Control control)
    {
        void aplicar()
        {
            try { SetWindowTheme(control.Handle, "DarkMode_Explorer", null); }
            catch { }
        }

        if (control.IsHandleCreated) aplicar();
        control.HandleCreated += (s, e) => aplicar();

        foreach (Control hijo in control.Controls)
        {
            if (hijo is ScrollBar) ScrollOscuro(hijo);
        }
    }

    public static void BarraTituloOscura(Form ventana)
    {
        void aplicar()
        {
            try
            {
                int activado = 1;
                DwmSetWindowAttribute(ventana.Handle, 20, ref activado, sizeof(int));
                DwmSetWindowAttribute(ventana.Handle, 19, ref activado, sizeof(int));
            }
            catch { }
        }

        if (ventana.IsHandleCreated) aplicar();
        else ventana.HandleCreated += (s, e) => aplicar();
    }
}
