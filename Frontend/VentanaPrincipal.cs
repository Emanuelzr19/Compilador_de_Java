using System;
using System.IO;
using System.Windows.Forms;
using CompiladorJava.Modelos;
using CompiladorJava.Servicios;

namespace CompiladorJava.Frontend;

public partial class VentanaPrincipal : Form
{
    private readonly AnalizadorReactivo analizador = new AnalizadorReactivo();
    private bool cargandoTexto;

    public VentanaPrincipal()
    {
        InitializeComponent();

        numerosLinea.Editor = editor;
        UtilidadesTablas.Numericas(colLexId, colLexLinea, colErrLinea, colSinLinea, colSemLinea);

        analizador.ResultadoActualizado += Analizador_ResultadoActualizado;
        FormClosed += (s, e) => analizador.Dispose();

        Tema.BarraTituloOscura(this);
        Tema.ScrollOscuro(editor);
        Tema.ScrollOscuro(cajaDepurado);
    }

    private void VentanaPrincipal_Load(object? sender, EventArgs e)
    {
        try
        {
            divisor.Panel1MinSize = 300;
            divisor.Panel2MinSize = 760;
            divisor.SplitterDistance = 460;
        }
        catch (InvalidOperationException)
        {
        }
    }

    private void Editor_TextChanged(object? sender, EventArgs e)
    {
        if (cargandoTexto) return;
        analizador.NotificarCambio(editor.Text);
    }

    private void Editor_KeyDown(object? sender, KeyEventArgs e)
    {
        bool pegar = (e.Control && e.KeyCode == Keys.V) || (e.Shift && e.KeyCode == Keys.Insert);
        if (!pegar) return;

        e.SuppressKeyPress = true;
        e.Handled = true;
        try
        {
            if (Clipboard.ContainsText()) editor.Paste(DataFormats.GetFormat(DataFormats.Text));
        }
        catch (Exception)
        {
        }
    }

    private void Analizador_ResultadoActualizado(object? sender, ResultadoAnalisis resultado)
    {
        if (IsDisposed || Disposing || !IsHandleCreated) return;

        try
        {
            BeginInvoke(new Action(() =>
            {
                if (resultado.CodigoOriginal != editor.Text) return;
                Mostrar(resultado);
            }));
        }
        catch (ObjectDisposedException)
        {
        }
        catch (InvalidOperationException)
        {
        }
    }

    private void BotonAbrir_Click(object? sender, EventArgs e)
    {
        using var dialogo = new OpenFileDialog
        {
            Title = "Abrir código Java",
            Filter = "Código Java (*.txt;*.java)|*.txt;*.java|Todos los archivos (*.*)|*.*"
        };
        if (dialogo.ShowDialog(this) != DialogResult.OK) return;

        string texto;
        try
        {
            texto = File.ReadAllText(dialogo.FileName);
        }
        catch (Exception ex)
        {
            MessageBox.Show(this, "No se pudo abrir el archivo:\n" + ex.Message, "Abrir archivo",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        cargandoTexto = true;
        try { editor.Text = texto; }
        finally { cargandoTexto = false; }

        Text = "Simulador de análisis de compilador Java — " + Path.GetFileName(dialogo.FileName);
        Mostrar(analizador.AnalizarAhora(editor.Text));
    }

    private void Tabla_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0 || sender is not DataGridView tabla) return;
        if (tabla.Rows[e.RowIndex].Tag is int linea) SeleccionarLinea(linea);
    }

    private void SeleccionarLinea(int linea)
    {
        if (linea < 1) return;

        int inicio = editor.GetFirstCharIndexFromLine(linea - 1);
        if (inicio < 0) return;

        int siguiente = editor.GetFirstCharIndexFromLine(linea);
        int fin = siguiente < 0 ? editor.TextLength : siguiente - 1;

        editor.Focus();
        editor.Select(inicio, Math.Max(0, fin - inicio));
        editor.ScrollToCaret();
    }

    private void Mostrar(ResultadoAnalisis r)
    {
        try
        {
            MostrarEstado(r);
            MostrarDepurado(r);

            avisoLexico.Visible = r.Bloqueado;
            avisoSintactico.Visible = r.Bloqueado;
            avisoSemantico.Visible = r.Bloqueado;

            MostrarLexico(r);
            MostrarErrores(r);
            MostrarSintactico(r);
            MostrarSemantico(r);
        }
        catch (Exception ex)
        {
            etiquetaEstado.ForeColor = Tema.Ambar;
            etiquetaEstado.Text = "No se pudieron mostrar los resultados: " + ex.Message;
        }
    }

    private void MostrarEstado(ResultadoAnalisis r)
    {
        if (r.Errores.Count == 0)
        {
            etiquetaEstado.ForeColor = Tema.Lexico;
            etiquetaEstado.Text = "● Sin errores";
            return;
        }

        etiquetaEstado.ForeColor = Tema.Sintactico;
        etiquetaEstado.Text = "● " + r.ErroresLexicos + " léxicos, " +
                              r.ErroresSintacticos + " sintácticos, " +
                              r.ErroresSemanticos + " semánticos";
    }

    private void MostrarDepurado(ResultadoAnalisis r)
    {
        string texto = r.CodigoDepurado.Replace("\r\n", "\n").Replace("\n", "\r\n");
        if (cajaDepurado.Text != texto) cajaDepurado.Text = texto;
    }

    private void MostrarLexico(ResultadoAnalisis r)
    {
        UtilidadesTablas.Reemplazar(tablaLexico, () =>
        {
            if (r.Bloqueado) return;
            foreach (FilaToken t in r.Tokens)
                UtilidadesTablas.Agregar(tablaLexico, t.Linea, t.Id, t.Lexema, t.Token, t.Linea);
        });
    }

    private void MostrarErrores(ResultadoAnalisis r)
    {
        UtilidadesTablas.Reemplazar(tablaErrores, () =>
        {
            foreach (ErrorCompilacion error in r.Errores)
            {
                DataGridViewRow fila = UtilidadesTablas.Agregar(tablaErrores, error.Linea,
                    error.Linea, error.Mensaje, error.TipoTexto);
                fila.DefaultCellStyle.ForeColor = Tema.ColorDeError(error.Tipo);
            }
        });
    }

    private void MostrarSintactico(ResultadoAnalisis r)
    {
        UtilidadesTablas.Reemplazar(tablaSintactico, () =>
        {
            if (r.Bloqueado) return;
            foreach (Instruccion i in r.Instrucciones)
            {
                if (!i.Valida) continue;
                UtilidadesTablas.Agregar(tablaSintactico, i.Linea,
                    i.Linea, i.Texto, i.Produccion, i.Derivacion);
            }
        });
    }

    private void MostrarSemantico(ResultadoAnalisis r)
    {
        UtilidadesTablas.Reemplazar(tablaSemantico, () =>
        {
            if (r.Bloqueado) return;
            foreach (FilaSemantica f in r.Semantica)
            {
                DataGridViewRow fila = UtilidadesTablas.Agregar(tablaSemantico, f.Linea,
                    f.Linea, f.Variable, f.Tipo, f.Valor, f.TipoValor, f.Estado);
                fila.Cells[colSemResultado.Index].Style.ForeColor = Tema.ColorDeEstado(f.Estado);
                fila.Cells[colSemResultado.Index].Style.Font = Tema.FuenteUINegrita;
            }
        });
    }
}
