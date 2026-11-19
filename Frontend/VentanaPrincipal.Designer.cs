#nullable disable
namespace CompiladorJava.Frontend
{
    partial class VentanaPrincipal
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.barraSuperior = new System.Windows.Forms.Panel();
            this.etiquetaEstado = new System.Windows.Forms.Label();
            this.botonAbrir = new System.Windows.Forms.Button();
            this.lineaBarra = new System.Windows.Forms.Panel();
            this.divisor = new System.Windows.Forms.SplitContainer();
            this.contenedorEditor = new System.Windows.Forms.Panel();
            this.editor = new System.Windows.Forms.RichTextBox();
            this.separador = new System.Windows.Forms.Panel();
            this.numerosLinea = new CompiladorJava.Frontend.NumerosLinea();
            this.pestanas = new CompiladorJava.Frontend.TabControlOscuro();
            this.paginaDepurado = new System.Windows.Forms.TabPage();
            this.paginaLexico = new System.Windows.Forms.TabPage();
            this.paginaErrores = new System.Windows.Forms.TabPage();
            this.paginaSintactico = new System.Windows.Forms.TabPage();
            this.paginaSemantico = new System.Windows.Forms.TabPage();
            this.cajaDepurado = new System.Windows.Forms.TextBox();
            this.tablaLexico = new CompiladorJava.Frontend.TablaOscura();
            this.tablaErrores = new CompiladorJava.Frontend.TablaOscura();
            this.tablaSintactico = new CompiladorJava.Frontend.TablaOscura();
            this.tablaSemantico = new CompiladorJava.Frontend.TablaOscura();
            this.avisoLexico = new System.Windows.Forms.Label();
            this.avisoSintactico = new System.Windows.Forms.Label();
            this.avisoSemantico = new System.Windows.Forms.Label();
            this.colLexId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLexLexema = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLexToken = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLexLinea = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colErrLinea = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colErrError = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colErrTipo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSinLinea = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSinInstruccion = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSinRegla = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSinDerivacion = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSemLinea = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSemVariable = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSemTipo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSemValor = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSemTipoValor = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSemResultado = new System.Windows.Forms.DataGridViewTextBoxColumn();

            ((System.ComponentModel.ISupportInitialize)(this.divisor)).BeginInit();
            this.divisor.Panel1.SuspendLayout();
            this.divisor.Panel2.SuspendLayout();
            this.divisor.SuspendLayout();
            this.barraSuperior.SuspendLayout();
            this.contenedorEditor.SuspendLayout();
            this.pestanas.SuspendLayout();
            this.paginaDepurado.SuspendLayout();
            this.paginaLexico.SuspendLayout();
            this.paginaErrores.SuspendLayout();
            this.paginaSintactico.SuspendLayout();
            this.paginaSemantico.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tablaLexico)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tablaErrores)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tablaSintactico)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tablaSemantico)).BeginInit();
            this.SuspendLayout();
            //
            // barraSuperior
            //
            this.barraSuperior.BackColor = System.Drawing.Color.FromArgb(28, 34, 48);
            this.barraSuperior.Controls.Add(this.etiquetaEstado);
            this.barraSuperior.Controls.Add(this.botonAbrir);
            this.barraSuperior.Dock = System.Windows.Forms.DockStyle.Top;
            this.barraSuperior.Location = new System.Drawing.Point(0, 0);
            this.barraSuperior.Name = "barraSuperior";
            this.barraSuperior.Padding = new System.Windows.Forms.Padding(16, 12, 16, 12);
            this.barraSuperior.Size = new System.Drawing.Size(1284, 56);
            this.barraSuperior.TabIndex = 0;

            //
            // etiquetaEstado
            //
            this.etiquetaEstado.AutoSize = false;
            this.etiquetaEstado.Dock = System.Windows.Forms.DockStyle.Fill;
            this.etiquetaEstado.ForeColor = System.Drawing.Color.FromArgb(138, 147, 166);
            this.etiquetaEstado.Location = new System.Drawing.Point(206, 12);
            this.etiquetaEstado.Name = "etiquetaEstado";
            this.etiquetaEstado.Padding = new System.Windows.Forms.Padding(18, 0, 0, 0);
            this.etiquetaEstado.Size = new System.Drawing.Size(1062, 32);
            this.etiquetaEstado.TabIndex = 1;
            this.etiquetaEstado.Text = "Escribe o abre código Java";
            this.etiquetaEstado.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            //
            // botonAbrir
            //
            this.botonAbrir.BackColor = System.Drawing.Color.FromArgb(59, 130, 246);
            this.botonAbrir.Cursor = System.Windows.Forms.Cursors.Hand;
            this.botonAbrir.Dock = System.Windows.Forms.DockStyle.Left;
            this.botonAbrir.FlatAppearance.BorderSize = 0;
            this.botonAbrir.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(29, 78, 216);
            this.botonAbrir.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(37, 99, 235);
            this.botonAbrir.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.botonAbrir.ForeColor = System.Drawing.Color.White;
            this.botonAbrir.Location = new System.Drawing.Point(16, 12);
            this.botonAbrir.Name = "botonAbrir";
            this.botonAbrir.Size = new System.Drawing.Size(190, 32);
            this.botonAbrir.TabIndex = 0;
            this.botonAbrir.Text = "Abrir archivo .txt";
            this.botonAbrir.UseVisualStyleBackColor = false;
            this.botonAbrir.Click += new System.EventHandler(this.BotonAbrir_Click);

            //
            // lineaBarra
            //
            this.lineaBarra.BackColor = System.Drawing.Color.FromArgb(42, 49, 66);
            this.lineaBarra.Dock = System.Windows.Forms.DockStyle.Top;
            this.lineaBarra.Location = new System.Drawing.Point(0, 56);
            this.lineaBarra.Name = "lineaBarra";
            this.lineaBarra.Size = new System.Drawing.Size(1284, 1);
            this.lineaBarra.TabIndex = 1;

            //
            // divisor
            //
            this.divisor.BackColor = System.Drawing.Color.FromArgb(42, 49, 66);
            this.divisor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.divisor.Location = new System.Drawing.Point(0, 57);
            this.divisor.Name = "divisor";
            this.divisor.Panel1.BackColor = System.Drawing.Color.FromArgb(22, 26, 35);
            this.divisor.Panel1.Controls.Add(this.contenedorEditor);
            this.divisor.Panel2.BackColor = System.Drawing.Color.FromArgb(15, 17, 23);
            this.divisor.Panel2.Controls.Add(this.pestanas);
            this.divisor.Panel2.Padding = new System.Windows.Forms.Padding(12);
            this.divisor.Size = new System.Drawing.Size(1284, 664);
            this.divisor.SplitterDistance = 460;
            this.divisor.SplitterWidth = 5;
            this.divisor.TabIndex = 2;

            //
            // contenedorEditor
            //
            this.contenedorEditor.BackColor = System.Drawing.Color.FromArgb(22, 26, 35);
            this.contenedorEditor.Controls.Add(this.editor);
            this.contenedorEditor.Controls.Add(this.separador);
            this.contenedorEditor.Controls.Add(this.numerosLinea);
            this.contenedorEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.contenedorEditor.Location = new System.Drawing.Point(0, 0);
            this.contenedorEditor.Name = "contenedorEditor";
            this.contenedorEditor.Padding = new System.Windows.Forms.Padding(0, 10, 0, 10);
            this.contenedorEditor.Size = new System.Drawing.Size(460, 664);
            this.contenedorEditor.TabIndex = 0;

            //
            // editor
            //
            this.editor.AcceptsTab = true;
            this.editor.BackColor = System.Drawing.Color.FromArgb(22, 26, 35);
            this.editor.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.editor.DetectUrls = false;
            this.editor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.editor.EnableAutoDragDrop = false;
            this.editor.Font = new System.Drawing.Font("Consolas", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.editor.ForeColor = System.Drawing.Color.FromArgb(230, 234, 242);
            this.editor.HideSelection = false;
            this.editor.Location = new System.Drawing.Point(74, 10);
            this.editor.Name = "editor";
            this.editor.Size = new System.Drawing.Size(386, 644);
            this.editor.TabIndex = 0;
            this.editor.Text = "";
            this.editor.WordWrap = false;
            this.editor.TextChanged += new System.EventHandler(this.Editor_TextChanged);
            this.editor.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Editor_KeyDown);

            //
            // separador
            //
            this.separador.BackColor = System.Drawing.Color.FromArgb(22, 26, 35);
            this.separador.Dock = System.Windows.Forms.DockStyle.Left;
            this.separador.Location = new System.Drawing.Point(64, 10);
            this.separador.Name = "separador";
            this.separador.Size = new System.Drawing.Size(10, 644);
            this.separador.TabIndex = 1;

            //
            // numerosLinea
            //
            this.numerosLinea.Dock = System.Windows.Forms.DockStyle.Left;
            this.numerosLinea.Location = new System.Drawing.Point(0, 10);
            this.numerosLinea.Name = "numerosLinea";
            this.numerosLinea.Size = new System.Drawing.Size(64, 644);
            this.numerosLinea.TabIndex = 2;

            //
            // pestanas
            //
            this.pestanas.Controls.Add(this.paginaDepurado);
            this.pestanas.Controls.Add(this.paginaLexico);
            this.pestanas.Controls.Add(this.paginaErrores);
            this.pestanas.Controls.Add(this.paginaSintactico);
            this.pestanas.Controls.Add(this.paginaSemantico);
            this.pestanas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pestanas.Location = new System.Drawing.Point(12, 12);
            this.pestanas.Name = "pestanas";
            this.pestanas.SelectedIndex = 0;
            this.pestanas.Size = new System.Drawing.Size(795, 640);
            this.pestanas.TabIndex = 0;

            //
            // paginaDepurado
            //
            this.paginaDepurado.BackColor = System.Drawing.Color.FromArgb(22, 26, 35);
            this.paginaDepurado.Controls.Add(this.cajaDepurado);
            this.paginaDepurado.ForeColor = System.Drawing.Color.FromArgb(230, 234, 242);
            this.paginaDepurado.Location = new System.Drawing.Point(4, 40);
            this.paginaDepurado.Name = "paginaDepurado";
            this.paginaDepurado.Padding = new System.Windows.Forms.Padding(12);
            this.paginaDepurado.Size = new System.Drawing.Size(787, 596);
            this.paginaDepurado.TabIndex = 0;
            this.paginaDepurado.Text = "Código depurado";
            this.paginaDepurado.UseVisualStyleBackColor = false;

            //
            // paginaLexico
            //
            this.paginaLexico.BackColor = System.Drawing.Color.FromArgb(22, 26, 35);
            this.paginaLexico.Controls.Add(this.tablaLexico);
            this.paginaLexico.Controls.Add(this.avisoLexico);
            this.paginaLexico.ForeColor = System.Drawing.Color.FromArgb(230, 234, 242);
            this.paginaLexico.Location = new System.Drawing.Point(4, 40);
            this.paginaLexico.Name = "paginaLexico";
            this.paginaLexico.Padding = new System.Windows.Forms.Padding(12);
            this.paginaLexico.Size = new System.Drawing.Size(787, 596);
            this.paginaLexico.TabIndex = 1;
            this.paginaLexico.Text = "Análisis léxico";
            this.paginaLexico.UseVisualStyleBackColor = false;

            //
            // paginaErrores
            //
            this.paginaErrores.BackColor = System.Drawing.Color.FromArgb(22, 26, 35);
            this.paginaErrores.Controls.Add(this.tablaErrores);
            this.paginaErrores.ForeColor = System.Drawing.Color.FromArgb(230, 234, 242);
            this.paginaErrores.Location = new System.Drawing.Point(4, 40);
            this.paginaErrores.Name = "paginaErrores";
            this.paginaErrores.Padding = new System.Windows.Forms.Padding(12);
            this.paginaErrores.Size = new System.Drawing.Size(787, 596);
            this.paginaErrores.TabIndex = 2;
            this.paginaErrores.Text = "Errores";
            this.paginaErrores.UseVisualStyleBackColor = false;

            //
            // paginaSintactico
            //
            this.paginaSintactico.BackColor = System.Drawing.Color.FromArgb(22, 26, 35);
            this.paginaSintactico.Controls.Add(this.tablaSintactico);
            this.paginaSintactico.Controls.Add(this.avisoSintactico);
            this.paginaSintactico.ForeColor = System.Drawing.Color.FromArgb(230, 234, 242);
            this.paginaSintactico.Location = new System.Drawing.Point(4, 40);
            this.paginaSintactico.Name = "paginaSintactico";
            this.paginaSintactico.Padding = new System.Windows.Forms.Padding(12);
            this.paginaSintactico.Size = new System.Drawing.Size(787, 596);
            this.paginaSintactico.TabIndex = 3;
            this.paginaSintactico.Text = "Análisis sintáctico";
            this.paginaSintactico.UseVisualStyleBackColor = false;

            //
            // paginaSemantico
            //
            this.paginaSemantico.BackColor = System.Drawing.Color.FromArgb(22, 26, 35);
            this.paginaSemantico.Controls.Add(this.tablaSemantico);
            this.paginaSemantico.Controls.Add(this.avisoSemantico);
            this.paginaSemantico.ForeColor = System.Drawing.Color.FromArgb(230, 234, 242);
            this.paginaSemantico.Location = new System.Drawing.Point(4, 40);
            this.paginaSemantico.Name = "paginaSemantico";
            this.paginaSemantico.Padding = new System.Windows.Forms.Padding(12);
            this.paginaSemantico.Size = new System.Drawing.Size(787, 596);
            this.paginaSemantico.TabIndex = 4;
            this.paginaSemantico.Text = "Análisis semántico";
            this.paginaSemantico.UseVisualStyleBackColor = false;

            //
            // cajaDepurado
            //
            this.cajaDepurado.BackColor = System.Drawing.Color.FromArgb(22, 26, 35);
            this.cajaDepurado.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.cajaDepurado.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cajaDepurado.Font = new System.Drawing.Font("Consolas", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cajaDepurado.ForeColor = System.Drawing.Color.FromArgb(230, 234, 242);
            this.cajaDepurado.Location = new System.Drawing.Point(12, 12);
            this.cajaDepurado.Multiline = true;
            this.cajaDepurado.Name = "cajaDepurado";
            this.cajaDepurado.ReadOnly = true;
            this.cajaDepurado.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.cajaDepurado.Size = new System.Drawing.Size(763, 572);
            this.cajaDepurado.TabIndex = 0;
            this.cajaDepurado.WordWrap = false;

            //
            // tablaLexico
            //
            this.tablaLexico.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] { this.colLexId, this.colLexLexema, this.colLexToken, this.colLexLinea });
            this.tablaLexico.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tablaLexico.Location = new System.Drawing.Point(12, 12);
            this.tablaLexico.Name = "tablaLexico";
            this.tablaLexico.Size = new System.Drawing.Size(763, 572);
            this.tablaLexico.TabIndex = 0;
            this.tablaLexico.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.Tabla_CellDoubleClick);

            //
            // colLexId
            //
            this.colLexId.FillWeight = 10F;
            this.colLexId.HeaderText = "ID";
            this.colLexId.MinimumWidth = 72;
            this.colLexId.Name = "colLexId";
            this.colLexId.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;

            //
            // colLexLexema
            //
            this.colLexLexema.FillWeight = 50F;
            this.colLexLexema.HeaderText = "LEXEMA";
            this.colLexLexema.MinimumWidth = 72;
            this.colLexLexema.Name = "colLexLexema";
            this.colLexLexema.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;

            //
            // colLexToken
            //
            this.colLexToken.FillWeight = 30F;
            this.colLexToken.HeaderText = "TOKEN";
            this.colLexToken.MinimumWidth = 72;
            this.colLexToken.Name = "colLexToken";
            this.colLexToken.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;

            //
            // colLexLinea
            //
            this.colLexLinea.FillWeight = 10F;
            this.colLexLinea.HeaderText = "LÍNEA";
            this.colLexLinea.MinimumWidth = 72;
            this.colLexLinea.Name = "colLexLinea";
            this.colLexLinea.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;

            //
            // tablaErrores
            //
            this.tablaErrores.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] { this.colErrLinea, this.colErrError, this.colErrTipo });
            this.tablaErrores.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tablaErrores.Location = new System.Drawing.Point(12, 12);
            this.tablaErrores.Name = "tablaErrores";
            this.tablaErrores.Size = new System.Drawing.Size(763, 572);
            this.tablaErrores.TabIndex = 0;
            this.tablaErrores.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.Tabla_CellDoubleClick);

            //
            // colErrLinea
            //
            this.colErrLinea.FillWeight = 10F;
            this.colErrLinea.HeaderText = "LÍNEA";
            this.colErrLinea.MinimumWidth = 72;
            this.colErrLinea.Name = "colErrLinea";
            this.colErrLinea.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;

            //
            // colErrError
            //
            this.colErrError.FillWeight = 75F;
            this.colErrError.HeaderText = "ERROR";
            this.colErrError.MinimumWidth = 72;
            this.colErrError.Name = "colErrError";
            this.colErrError.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;

            //
            // colErrTipo
            //
            this.colErrTipo.FillWeight = 25F;
            this.colErrTipo.HeaderText = "TIPO";
            this.colErrTipo.MinimumWidth = 72;
            this.colErrTipo.Name = "colErrTipo";
            this.colErrTipo.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;

            //
            // tablaSintactico
            //
            this.tablaSintactico.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] { this.colSinLinea, this.colSinInstruccion, this.colSinRegla, this.colSinDerivacion });
            this.tablaSintactico.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tablaSintactico.Location = new System.Drawing.Point(12, 12);
            this.tablaSintactico.Name = "tablaSintactico";
            this.tablaSintactico.Size = new System.Drawing.Size(763, 572);
            this.tablaSintactico.TabIndex = 0;
            this.tablaSintactico.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.Tabla_CellDoubleClick);

            //
            // colSinLinea
            //
            this.colSinLinea.FillWeight = 10F;
            this.colSinLinea.HeaderText = "Línea";
            this.colSinLinea.MinimumWidth = 72;
            this.colSinLinea.Name = "colSinLinea";
            this.colSinLinea.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;

            //
            // colSinInstruccion
            //
            this.colSinInstruccion.FillWeight = 34F;
            this.colSinInstruccion.HeaderText = "Instrucción";
            this.colSinInstruccion.MinimumWidth = 72;
            this.colSinInstruccion.Name = "colSinInstruccion";
            this.colSinInstruccion.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;

            //
            // colSinRegla
            //
            this.colSinRegla.FillWeight = 36F;
            this.colSinRegla.HeaderText = "Regla de la GLC";
            this.colSinRegla.MinimumWidth = 72;
            this.colSinRegla.Name = "colSinRegla";
            this.colSinRegla.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;

            //
            // colSinDerivacion
            //
            this.colSinDerivacion.FillWeight = 30F;
            this.colSinDerivacion.HeaderText = "Derivación";
            this.colSinDerivacion.MinimumWidth = 72;
            this.colSinDerivacion.Name = "colSinDerivacion";
            this.colSinDerivacion.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;

            //
            // tablaSemantico
            //
            this.tablaSemantico.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] { this.colSemLinea, this.colSemVariable, this.colSemTipo, this.colSemValor, this.colSemTipoValor, this.colSemResultado });
            this.tablaSemantico.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tablaSemantico.Location = new System.Drawing.Point(12, 12);
            this.tablaSemantico.Name = "tablaSemantico";
            this.tablaSemantico.Size = new System.Drawing.Size(763, 572);
            this.tablaSemantico.TabIndex = 0;
            this.tablaSemantico.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.Tabla_CellDoubleClick);

            //
            // colSemLinea
            //
            this.colSemLinea.FillWeight = 10F;
            this.colSemLinea.HeaderText = "Línea";
            this.colSemLinea.MinimumWidth = 72;
            this.colSemLinea.Name = "colSemLinea";
            this.colSemLinea.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;

            //
            // colSemVariable
            //
            this.colSemVariable.FillWeight = 18F;
            this.colSemVariable.HeaderText = "Variable";
            this.colSemVariable.MinimumWidth = 72;
            this.colSemVariable.Name = "colSemVariable";
            this.colSemVariable.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;

            //
            // colSemTipo
            //
            this.colSemTipo.FillWeight = 14F;
            this.colSemTipo.HeaderText = "Tipo";
            this.colSemTipo.MinimumWidth = 72;
            this.colSemTipo.Name = "colSemTipo";
            this.colSemTipo.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;

            //
            // colSemValor
            //
            this.colSemValor.FillWeight = 22F;
            this.colSemValor.HeaderText = "Valor asignado";
            this.colSemValor.MinimumWidth = 72;
            this.colSemValor.Name = "colSemValor";
            this.colSemValor.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;

            //
            // colSemTipoValor
            //
            this.colSemTipoValor.FillWeight = 20F;
            this.colSemTipoValor.HeaderText = "Tipo del valor";
            this.colSemTipoValor.MinimumWidth = 72;
            this.colSemTipoValor.Name = "colSemTipoValor";
            this.colSemTipoValor.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;

            //
            // colSemResultado
            //
            this.colSemResultado.FillWeight = 18F;
            this.colSemResultado.HeaderText = "Resultado";
            this.colSemResultado.MinimumWidth = 72;
            this.colSemResultado.Name = "colSemResultado";
            this.colSemResultado.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;

            //
            // avisoLexico
            //
            this.avisoLexico.AutoSize = false;
            this.avisoLexico.BackColor = System.Drawing.Color.FromArgb(42, 36, 16);
            this.avisoLexico.Dock = System.Windows.Forms.DockStyle.Top;
            this.avisoLexico.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.avisoLexico.ForeColor = System.Drawing.Color.FromArgb(251, 191, 36);
            this.avisoLexico.Location = new System.Drawing.Point(12, 12);
            this.avisoLexico.Name = "avisoLexico";
            this.avisoLexico.Padding = new System.Windows.Forms.Padding(14, 0, 14, 0);
            this.avisoLexico.Size = new System.Drawing.Size(763, 48);
            this.avisoLexico.TabIndex = 1;
            this.avisoLexico.Text = "El código no es una clase principal válida con main";
            this.avisoLexico.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.avisoLexico.Visible = false;

            //
            // avisoSintactico
            //
            this.avisoSintactico.AutoSize = false;
            this.avisoSintactico.BackColor = System.Drawing.Color.FromArgb(42, 36, 16);
            this.avisoSintactico.Dock = System.Windows.Forms.DockStyle.Top;
            this.avisoSintactico.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.avisoSintactico.ForeColor = System.Drawing.Color.FromArgb(251, 191, 36);
            this.avisoSintactico.Location = new System.Drawing.Point(12, 12);
            this.avisoSintactico.Name = "avisoSintactico";
            this.avisoSintactico.Padding = new System.Windows.Forms.Padding(14, 0, 14, 0);
            this.avisoSintactico.Size = new System.Drawing.Size(763, 48);
            this.avisoSintactico.TabIndex = 1;
            this.avisoSintactico.Text = "El código no es una clase principal válida con main";
            this.avisoSintactico.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.avisoSintactico.Visible = false;

            //
            // avisoSemantico
            //
            this.avisoSemantico.AutoSize = false;
            this.avisoSemantico.BackColor = System.Drawing.Color.FromArgb(42, 36, 16);
            this.avisoSemantico.Dock = System.Windows.Forms.DockStyle.Top;
            this.avisoSemantico.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.avisoSemantico.ForeColor = System.Drawing.Color.FromArgb(251, 191, 36);
            this.avisoSemantico.Location = new System.Drawing.Point(12, 12);
            this.avisoSemantico.Name = "avisoSemantico";
            this.avisoSemantico.Padding = new System.Windows.Forms.Padding(14, 0, 14, 0);
            this.avisoSemantico.Size = new System.Drawing.Size(763, 48);
            this.avisoSemantico.TabIndex = 1;
            this.avisoSemantico.Text = "El código no es una clase principal válida con main";
            this.avisoSemantico.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.avisoSemantico.Visible = false;

            //
            // VentanaPrincipal
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(15, 17, 23);
            this.ClientSize = new System.Drawing.Size(1284, 721);
            this.Controls.Add(this.divisor);
            this.Controls.Add(this.lineaBarra);
            this.Controls.Add(this.barraSuperior);
            this.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.FromArgb(230, 234, 242);
            this.MinimumSize = new System.Drawing.Size(1100, 600);
            this.Name = "VentanaPrincipal";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "";
            this.Load += new System.EventHandler(this.VentanaPrincipal_Load);
            ((System.ComponentModel.ISupportInitialize)(this.tablaLexico)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tablaErrores)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tablaSintactico)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tablaSemantico)).EndInit();
            this.paginaDepurado.ResumeLayout(false);
            this.paginaLexico.ResumeLayout(false);
            this.paginaErrores.ResumeLayout(false);
            this.paginaSintactico.ResumeLayout(false);
            this.paginaSemantico.ResumeLayout(false);
            this.pestanas.ResumeLayout(false);
            this.contenedorEditor.ResumeLayout(false);
            this.barraSuperior.ResumeLayout(false);
            this.divisor.Panel1.ResumeLayout(false);
            this.divisor.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.divisor)).EndInit();
            this.divisor.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel barraSuperior;
        private System.Windows.Forms.Label etiquetaEstado;
        private System.Windows.Forms.Button botonAbrir;
        private System.Windows.Forms.Panel lineaBarra;
        private System.Windows.Forms.SplitContainer divisor;
        private System.Windows.Forms.Panel contenedorEditor;
        private System.Windows.Forms.RichTextBox editor;
        private System.Windows.Forms.Panel separador;
        private CompiladorJava.Frontend.NumerosLinea numerosLinea;
        private CompiladorJava.Frontend.TabControlOscuro pestanas;
        private System.Windows.Forms.TabPage paginaDepurado;
        private System.Windows.Forms.TabPage paginaLexico;
        private System.Windows.Forms.TabPage paginaErrores;
        private System.Windows.Forms.TabPage paginaSintactico;
        private System.Windows.Forms.TabPage paginaSemantico;
        private System.Windows.Forms.TextBox cajaDepurado;
        private CompiladorJava.Frontend.TablaOscura tablaLexico;
        private CompiladorJava.Frontend.TablaOscura tablaErrores;
        private CompiladorJava.Frontend.TablaOscura tablaSintactico;
        private CompiladorJava.Frontend.TablaOscura tablaSemantico;
        private System.Windows.Forms.Label avisoLexico;
        private System.Windows.Forms.Label avisoSintactico;
        private System.Windows.Forms.Label avisoSemantico;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLexId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLexLexema;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLexToken;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLexLinea;
        private System.Windows.Forms.DataGridViewTextBoxColumn colErrLinea;
        private System.Windows.Forms.DataGridViewTextBoxColumn colErrError;
        private System.Windows.Forms.DataGridViewTextBoxColumn colErrTipo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSinLinea;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSinInstruccion;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSinRegla;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSinDerivacion;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSemLinea;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSemVariable;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSemTipo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSemValor;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSemTipoValor;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSemResultado;
    }
}
