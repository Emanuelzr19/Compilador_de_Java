using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace CompiladorJava.Frontend;

public sealed class TablaOscura : DataGridView
{
    public TablaOscura()
    {
        DoubleBuffered = true;
        BackgroundColor = Tema.Panel;
        BorderStyle = BorderStyle.None;
        GridColor = Tema.Borde;
        CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
        ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
        EnableHeadersVisualStyles = false;
        RowHeadersVisible = false;
        AllowUserToAddRows = false;
        AllowUserToDeleteRows = false;
        AllowUserToResizeRows = false;
        ReadOnly = true;
        MultiSelect = false;
        SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
        ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
        ColumnHeadersHeight = 38;

        ColumnHeadersDefaultCellStyle.BackColor = Tema.Encabezado;
        ColumnHeadersDefaultCellStyle.ForeColor = Tema.AcentoClaro;
        ColumnHeadersDefaultCellStyle.SelectionBackColor = Tema.Encabezado;
        ColumnHeadersDefaultCellStyle.SelectionForeColor = Tema.AcentoClaro;
        ColumnHeadersDefaultCellStyle.Font = Tema.FuenteUINegrita;
        ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
        ColumnHeadersDefaultCellStyle.Padding = new Padding(6, 0, 6, 0);

        DefaultCellStyle.BackColor = Tema.Panel;
        DefaultCellStyle.ForeColor = Tema.Texto;
        DefaultCellStyle.SelectionBackColor = Tema.Acento;
        DefaultCellStyle.SelectionForeColor = Color.White;
        DefaultCellStyle.Font = Tema.FuenteUI;
        DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
        DefaultCellStyle.Padding = new Padding(6, 5, 6, 5);
        DefaultCellStyle.WrapMode = DataGridViewTriState.True;

        AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle
        {
            BackColor = Tema.FilaAlterna
        };
    }
}

public sealed class TabControlOscuro : TabControl
{
    public TabControlOscuro()
    {
        SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint |
                 ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
        DrawMode = TabDrawMode.OwnerDrawFixed;
        SizeMode = TabSizeMode.Fixed;
        ItemSize = new Size(150, 36);
        Multiline = true;
        Font = Tema.FuenteUI;
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        e.Graphics.Clear(Tema.Fondo);
        for (int i = 0; i < TabCount; i++) DibujarPestana(e.Graphics, i);

        Rectangle area = DisplayRectangle;
        using (var lapiz = new Pen(Tema.Borde))
        {
            e.Graphics.DrawRectangle(lapiz, area.X - 1, area.Y - 1, area.Width + 1, area.Height + 1);
        }
    }

    protected override void OnDrawItem(DrawItemEventArgs e)
    {
        if (e.Index >= 0 && e.Index < TabCount) DibujarPestana(e.Graphics, e.Index);
    }

    private void DibujarPestana(Graphics g, int indice)
    {
        Rectangle r = Rectangle.Inflate(GetTabRect(indice), -1, 0);
        bool activa = indice == SelectedIndex;

        using (var fondo = new SolidBrush(activa ? Tema.Panel : Tema.Encabezado))
        {
            g.FillRectangle(fondo, r);
        }

        if (activa)
        {
            using (var barra = new SolidBrush(Tema.Acento))
            {
                g.FillRectangle(barra, r.X, r.Y, r.Width, 2);
            }
        }

        TextRenderer.DrawText(g, TabPages[indice].Text, Tema.FuenteUI, r,
            activa ? Tema.Texto : Tema.TextoSecundario,
            TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter |
            TextFormatFlags.EndEllipsis | TextFormatFlags.SingleLine);
    }
}

public sealed class NumerosLinea : Control
{
    private RichTextBox? editor;

    public NumerosLinea()
    {
        SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint |
                 ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
        BackColor = Tema.NumerosFondo;
        Font = Tema.FuenteCodigo;
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public RichTextBox? Editor
    {
        get { return editor; }
        set
        {
            editor = value;
            if (editor != null)
            {
                editor.TextChanged += (s, e) => Invalidate();
                editor.VScroll += (s, e) => Invalidate();
                editor.SelectionChanged += (s, e) => Invalidate();
                editor.Resize += (s, e) => Invalidate();
            }
            Invalidate();
        }
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        Graphics g = e.Graphics;
        g.Clear(BackColor);

        using (var lapiz = new Pen(Tema.Borde))
        {
            g.DrawLine(lapiz, Width - 1, 0, Width - 1, Height);
        }

        if (editor == null) return;

        int primera = editor.GetLineFromCharIndex(editor.GetCharIndexFromPosition(new Point(1, 1)));
        int ultima = editor.GetLineFromCharIndex(editor.TextLength);
        int actual = editor.GetLineFromCharIndex(editor.SelectionStart);

        for (int i = primera; i <= ultima; i++)
        {
            int indice = editor.GetFirstCharIndexFromLine(i);
            if (indice < 0) break;

            int y = editor.GetPositionFromCharIndex(indice).Y;
            if (y > Height) break;

            var area = new Rectangle(0, y, Width - 12, editor.Font.Height + 2);
            TextRenderer.DrawText(g, (i + 1).ToString(), Font, area,
                i == actual ? Tema.Texto : Tema.TextoSecundario,
                TextFormatFlags.Right | TextFormatFlags.Top | TextFormatFlags.NoPrefix);
        }
    }
}
