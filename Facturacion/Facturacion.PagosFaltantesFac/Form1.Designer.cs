namespace Facturacion.PagosFaltantesFac
{
    partial class Form1
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblTitulo = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnProcesar = new System.Windows.Forms.Button();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.panel2 = new System.Windows.Forms.Panel();
            this.cmbProceso = new System.Windows.Forms.ComboBox();
            this.lblProceso = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.pbAvance = new System.Windows.Forms.ProgressBar();
            this.lstResumen = new System.Windows.Forms.ListBox();
            this.pnlBotones = new System.Windows.Forms.Panel();
            this.gbCFDI = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.dtpFechaIni = new System.Windows.Forms.DateTimePicker();
            this.dtpFechaFin = new System.Windows.Forms.DateTimePicker();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.pnlBotones.SuspendLayout();
            this.gbCFDI.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblTitulo
            // 
            this.lblTitulo.BackColor = System.Drawing.Color.DarkGreen;
            this.lblTitulo.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTitulo.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitulo.ForeColor = System.Drawing.Color.White;
            this.lblTitulo.Location = new System.Drawing.Point(0, 0);
            this.lblTitulo.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(836, 30);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "Herramientas Facturación 3.3";
            this.lblTitulo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panel1.Controls.Add(this.gbCFDI);
            this.panel1.Controls.Add(this.pnlBotones);
            this.panel1.Controls.Add(this.lblProceso);
            this.panel1.Controls.Add(this.cmbProceso);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 30);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(297, 331);
            this.panel1.TabIndex = 1;
            // 
            // btnProcesar
            // 
            this.btnProcesar.Location = new System.Drawing.Point(69, 10);
            this.btnProcesar.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnProcesar.Name = "btnProcesar";
            this.btnProcesar.Size = new System.Drawing.Size(161, 52);
            this.btnProcesar.TabIndex = 0;
            this.btnProcesar.Text = "Procesar";
            this.btnProcesar.UseVisualStyleBackColor = true;
            this.btnProcesar.Click += new System.EventHandler(this.btnProcesar_Click);
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(297, 30);
            this.splitter1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(3, 331);
            this.splitter1.TabIndex = 2;
            this.splitter1.TabStop = false;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.lstResumen);
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(300, 30);
            this.panel2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(536, 331);
            this.panel2.TabIndex = 3;
            // 
            // cmbProceso
            // 
            this.cmbProceso.FormattingEnabled = true;
            this.cmbProceso.Location = new System.Drawing.Point(12, 28);
            this.cmbProceso.Name = "cmbProceso";
            this.cmbProceso.Size = new System.Drawing.Size(275, 24);
            this.cmbProceso.TabIndex = 1;
            this.cmbProceso.SelectedIndexChanged += new System.EventHandler(this.cmbProceso_SelectedIndexChanged);
            // 
            // lblProceso
            // 
            this.lblProceso.AutoSize = true;
            this.lblProceso.Location = new System.Drawing.Point(13, 8);
            this.lblProceso.Name = "lblProceso";
            this.lblProceso.Size = new System.Drawing.Size(147, 17);
            this.lblProceso.TabIndex = 2;
            this.lblProceso.Text = "Seleccione el proceso";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.pbAvance);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 287);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(536, 44);
            this.panel3.TabIndex = 0;
            // 
            // pbAvance
            // 
            this.pbAvance.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbAvance.Location = new System.Drawing.Point(0, 0);
            this.pbAvance.Name = "pbAvance";
            this.pbAvance.Size = new System.Drawing.Size(536, 44);
            this.pbAvance.Step = 1;
            this.pbAvance.TabIndex = 0;
            // 
            // lstResumen
            // 
            this.lstResumen.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstResumen.FormattingEnabled = true;
            this.lstResumen.ItemHeight = 16;
            this.lstResumen.Location = new System.Drawing.Point(0, 0);
            this.lstResumen.Name = "lstResumen";
            this.lstResumen.ScrollAlwaysVisible = true;
            this.lstResumen.Size = new System.Drawing.Size(536, 287);
            this.lstResumen.TabIndex = 1;
            // 
            // pnlBotones
            // 
            this.pnlBotones.BackColor = System.Drawing.Color.DarkGreen;
            this.pnlBotones.Controls.Add(this.btnProcesar);
            this.pnlBotones.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBotones.Location = new System.Drawing.Point(0, 258);
            this.pnlBotones.Name = "pnlBotones";
            this.pnlBotones.Size = new System.Drawing.Size(297, 73);
            this.pnlBotones.TabIndex = 3;
            // 
            // gbCFDI
            // 
            this.gbCFDI.BackColor = System.Drawing.Color.WhiteSmoke;
            this.gbCFDI.Controls.Add(this.dtpFechaFin);
            this.gbCFDI.Controls.Add(this.dtpFechaIni);
            this.gbCFDI.Controls.Add(this.label2);
            this.gbCFDI.Controls.Add(this.label1);
            this.gbCFDI.Location = new System.Drawing.Point(12, 58);
            this.gbCFDI.Name = "gbCFDI";
            this.gbCFDI.Size = new System.Drawing.Size(275, 161);
            this.gbCFDI.TabIndex = 4;
            this.gbCFDI.TabStop = false;
            this.gbCFDI.Text = "Parametros";
            this.gbCFDI.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 36);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(86, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Fecha Inicial";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 101);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(81, 17);
            this.label2.TabIndex = 1;
            this.label2.Text = "Fecha Final";
            // 
            // dtpFechaIni
            // 
            this.dtpFechaIni.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFechaIni.Location = new System.Drawing.Point(10, 56);
            this.dtpFechaIni.Name = "dtpFechaIni";
            this.dtpFechaIni.Size = new System.Drawing.Size(219, 22);
            this.dtpFechaIni.TabIndex = 2;
            // 
            // dtpFechaFin
            // 
            this.dtpFechaFin.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFechaFin.Location = new System.Drawing.Point(10, 121);
            this.dtpFechaFin.Name = "dtpFechaFin";
            this.dtpFechaFin.Size = new System.Drawing.Size(219, 22);
            this.dtpFechaFin.TabIndex = 3;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(836, 361);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.lblTitulo);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "Form1";
            this.Text = "Facturación 3.3 Tools";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.pnlBotones.ResumeLayout(false);
            this.gbCFDI.ResumeLayout(false);
            this.gbCFDI.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnProcesar;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel pnlBotones;
        private System.Windows.Forms.Label lblProceso;
        private System.Windows.Forms.ComboBox cmbProceso;
        private System.Windows.Forms.ListBox lstResumen;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.ProgressBar pbAvance;
        private System.Windows.Forms.GroupBox gbCFDI;
        private System.Windows.Forms.DateTimePicker dtpFechaFin;
        private System.Windows.Forms.DateTimePicker dtpFechaIni;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
    }
}

