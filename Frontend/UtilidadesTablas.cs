using System;
using System.Windows.Forms;

namespace CompiladorJava.Frontend;

public static class UtilidadesTablas
{
    public static void Numericas(params DataGridViewColumn[] columnas)
    {
        foreach (DataGridViewColumn c in columnas)
        {
            c.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            c.Width = 84;
            c.Resizable = DataGridViewTriState.False;
            c.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            c.DefaultCellStyle.Font = Tema.FuenteNumeros;
            c.DefaultCellStyle.Padding = new Padding(6, 5, 18, 5);
            c.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
            c.HeaderCell.Style.Padding = new Padding(6, 0, 18, 0);
        }
    }

    public static void Reemplazar(DataGridView tabla, Action llenar)
    {
        int primera = -1;
        try { primera = tabla.FirstDisplayedScrollingRowIndex; }
        catch { }

        tabla.SuspendLayout();
        try
        {
            tabla.Rows.Clear();
            llenar();
        }
        finally
        {
            tabla.ResumeLayout();
        }

        if (primera > 0 && primera < tabla.RowCount)
        {
            try { tabla.FirstDisplayedScrollingRowIndex = primera; }
            catch { }
        }
    }

    public static DataGridViewRow Agregar(DataGridView tabla, int linea, params object[] celdas)
    {
        int indice = tabla.Rows.Add(celdas);
        DataGridViewRow fila = tabla.Rows[indice];
        fila.Tag = linea;
        return fila;
    }
}
