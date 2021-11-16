namespace FacturacionTicketFac
{
    partial class frmGenerarFacturas
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmGenerarFacturas));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.imgListaImagenes = new System.Windows.Forms.ImageList(this.components);
            this.pnlEncabezado = new System.Windows.Forms.Panel();
            this.lblTitulo = new System.Windows.Forms.Label();
            this.picLogoViva = new System.Windows.Forms.PictureBox();
            this.pnlDatosFact = new System.Windows.Forms.Panel();
            this.lblNumPagos = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblLeyRFC = new System.Windows.Forms.Label();
            this.btnFacturar = new System.Windows.Forms.Button();
            this.dgPagos = new System.Windows.Forms.DataGridView();
            this.estaMarcadoParaFacturacionDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.idPagosCabDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.paymentIDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.fechaPagoDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.folioPrefacturaDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.fechaFacturaDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.montoTotalDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.currencyCodeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lugarExpedicionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.eNTPagosPorFacturarBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.rdbPorDefinir = new System.Windows.Forms.RadioButton();
            this.ddlPaisResidencia = new System.Windows.Forms.ComboBox();
            this.txtPNR = new System.Windows.Forms.TextBox();
            this.rdbGastosGral = new System.Windows.Forms.RadioButton();
            this.chkExtranjero = new System.Windows.Forms.CheckBox();
            this.lblLeyPNR = new System.Windows.Forms.Label();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.lblLeyEmail = new System.Windows.Forms.Label();
            this.txtRFC_TAXID = new System.Windows.Forms.TextBox();
            this.eNTPagosFacturadosBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.lblVersion = new System.Windows.Forms.Label();
            this.pnlEncabezado.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picLogoViva)).BeginInit();
            this.pnlDatosFact.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgPagos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.eNTPagosPorFacturarBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.eNTPagosFacturadosBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // imgListaImagenes
            // 
            this.imgListaImagenes.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgListaImagenes.ImageStream")));
            this.imgListaImagenes.TransparentColor = System.Drawing.Color.Transparent;
            this.imgListaImagenes.Images.SetKeyName(0, "FlechaDer.jpg");
            this.imgListaImagenes.Images.SetKeyName(1, "FlechaIzq.jpg");
            // 
            // pnlEncabezado
            // 
            this.pnlEncabezado.BackColor = System.Drawing.Color.White;
            this.pnlEncabezado.Controls.Add(this.lblTitulo);
            this.pnlEncabezado.Controls.Add(this.picLogoViva);
            this.pnlEncabezado.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlEncabezado.Location = new System.Drawing.Point(0, 0);
            this.pnlEncabezado.Name = "pnlEncabezado";
            this.pnlEncabezado.Size = new System.Drawing.Size(302, 62);
            this.pnlEncabezado.TabIndex = 0;
            // 
            // lblTitulo
            // 
            this.lblTitulo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(177)))), ((int)(((byte)(101)))));
            this.lblTitulo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblTitulo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTitulo.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.ForeColor = System.Drawing.Color.White;
            this.lblTitulo.Location = new System.Drawing.Point(77, 0);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(225, 62);
            this.lblTitulo.TabIndex = 1;
            this.lblTitulo.Text = "Facturación";
            this.lblTitulo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // picLogoViva
            // 
            this.picLogoViva.BackColor = System.Drawing.Color.White;
            this.picLogoViva.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.picLogoViva.Dock = System.Windows.Forms.DockStyle.Left;
            this.picLogoViva.Image = ((System.Drawing.Image)(resources.GetObject("picLogoViva.Image")));
            this.picLogoViva.Location = new System.Drawing.Point(0, 0);
            this.picLogoViva.Name = "picLogoViva";
            this.picLogoViva.Size = new System.Drawing.Size(77, 62);
            this.picLogoViva.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picLogoViva.TabIndex = 0;
            this.picLogoViva.TabStop = false;
            // 
            // pnlDatosFact
            // 
            this.pnlDatosFact.BackColor = System.Drawing.Color.WhiteSmoke;
            this.pnlDatosFact.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pnlDatosFact.Controls.Add(this.lblNumPagos);
            this.pnlDatosFact.Controls.Add(this.label2);
            this.pnlDatosFact.Controls.Add(this.lblLeyRFC);
            this.pnlDatosFact.Controls.Add(this.btnFacturar);
            this.pnlDatosFact.Controls.Add(this.dgPagos);
            this.pnlDatosFact.Controls.Add(this.rdbPorDefinir);
            this.pnlDatosFact.Controls.Add(this.ddlPaisResidencia);
            this.pnlDatosFact.Controls.Add(this.txtPNR);
            this.pnlDatosFact.Controls.Add(this.rdbGastosGral);
            this.pnlDatosFact.Controls.Add(this.chkExtranjero);
            this.pnlDatosFact.Controls.Add(this.lblLeyPNR);
            this.pnlDatosFact.Controls.Add(this.txtEmail);
            this.pnlDatosFact.Controls.Add(this.lblLeyEmail);
            this.pnlDatosFact.Controls.Add(this.txtRFC_TAXID);
            this.pnlDatosFact.Location = new System.Drawing.Point(8, 68);
            this.pnlDatosFact.Name = "pnlDatosFact";
            this.pnlDatosFact.Size = new System.Drawing.Size(286, 382);
            this.pnlDatosFact.TabIndex = 1;
            // 
            // lblNumPagos
            // 
            this.lblNumPagos.BackColor = System.Drawing.Color.Transparent;
            this.lblNumPagos.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNumPagos.Location = new System.Drawing.Point(216, 5);
            this.lblNumPagos.Name = "lblNumPagos";
            this.lblNumPagos.Size = new System.Drawing.Size(59, 30);
            this.lblNumPagos.TabIndex = 28;
            this.lblNumPagos.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Microsoft Tai Le", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(13, 256);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(249, 17);
            this.label2.TabIndex = 27;
            this.label2.Text = "Uso CFDI";
            // 
            // lblLeyRFC
            // 
            this.lblLeyRFC.BackColor = System.Drawing.Color.Transparent;
            this.lblLeyRFC.Font = new System.Drawing.Font("Microsoft Tai Le", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLeyRFC.Location = new System.Drawing.Point(13, 226);
            this.lblLeyRFC.Name = "lblLeyRFC";
            this.lblLeyRFC.Size = new System.Drawing.Size(87, 22);
            this.lblLeyRFC.TabIndex = 26;
            this.lblLeyRFC.Text = "RFC";
            // 
            // btnFacturar
            // 
            this.btnFacturar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(177)))), ((int)(((byte)(101)))));
            this.btnFacturar.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFacturar.ForeColor = System.Drawing.Color.White;
            this.btnFacturar.Location = new System.Drawing.Point(11, 332);
            this.btnFacturar.Name = "btnFacturar";
            this.btnFacturar.Size = new System.Drawing.Size(264, 39);
            this.btnFacturar.TabIndex = 10;
            this.btnFacturar.Text = "FACTURAR";
            this.btnFacturar.UseVisualStyleBackColor = false;
            this.btnFacturar.Click += new System.EventHandler(this.btnFacturar_Click);
            // 
            // dgPagos
            // 
            this.dgPagos.AllowUserToAddRows = false;
            this.dgPagos.AllowUserToDeleteRows = false;
            this.dgPagos.AllowUserToResizeColumns = false;
            this.dgPagos.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.dgPagos.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgPagos.AutoGenerateColumns = false;
            this.dgPagos.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.estaMarcadoParaFacturacionDataGridViewCheckBoxColumn,
            this.idPagosCabDataGridViewTextBoxColumn,
            this.paymentIDDataGridViewTextBoxColumn,
            this.fechaPagoDataGridViewTextBoxColumn,
            this.folioPrefacturaDataGridViewTextBoxColumn,
            this.fechaFacturaDataGridViewTextBoxColumn,
            this.montoTotalDataGridViewTextBoxColumn,
            this.currencyCodeDataGridViewTextBoxColumn,
            this.lugarExpedicionDataGridViewTextBoxColumn});
            this.dgPagos.DataSource = this.eNTPagosPorFacturarBindingSource;
            this.dgPagos.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dgPagos.Location = new System.Drawing.Point(8, 40);
            this.dgPagos.MultiSelect = false;
            this.dgPagos.Name = "dgPagos";
            this.dgPagos.RowHeadersVisible = false;
            this.dgPagos.RowTemplate.Height = 24;
            this.dgPagos.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dgPagos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgPagos.ShowCellErrors = false;
            this.dgPagos.ShowCellToolTips = false;
            this.dgPagos.ShowEditingIcon = false;
            this.dgPagos.Size = new System.Drawing.Size(267, 90);
            this.dgPagos.TabIndex = 25;
            this.dgPagos.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgPagos_CellFormatting);
            // 
            // estaMarcadoParaFacturacionDataGridViewCheckBoxColumn
            // 
            this.estaMarcadoParaFacturacionDataGridViewCheckBoxColumn.DataPropertyName = "EstaMarcadoParaFacturacion";
            this.estaMarcadoParaFacturacionDataGridViewCheckBoxColumn.HeaderText = "";
            this.estaMarcadoParaFacturacionDataGridViewCheckBoxColumn.Name = "estaMarcadoParaFacturacionDataGridViewCheckBoxColumn";
            this.estaMarcadoParaFacturacionDataGridViewCheckBoxColumn.Width = 30;
            // 
            // idPagosCabDataGridViewTextBoxColumn
            // 
            this.idPagosCabDataGridViewTextBoxColumn.DataPropertyName = "IdPagosCab";
            this.idPagosCabDataGridViewTextBoxColumn.HeaderText = "IdPagosCab";
            this.idPagosCabDataGridViewTextBoxColumn.Name = "idPagosCabDataGridViewTextBoxColumn";
            this.idPagosCabDataGridViewTextBoxColumn.Visible = false;
            // 
            // paymentIDDataGridViewTextBoxColumn
            // 
            this.paymentIDDataGridViewTextBoxColumn.DataPropertyName = "PaymentID";
            this.paymentIDDataGridViewTextBoxColumn.HeaderText = "PaymentID";
            this.paymentIDDataGridViewTextBoxColumn.Name = "paymentIDDataGridViewTextBoxColumn";
            this.paymentIDDataGridViewTextBoxColumn.Visible = false;
            // 
            // fechaPagoDataGridViewTextBoxColumn
            // 
            this.fechaPagoDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.fechaPagoDataGridViewTextBoxColumn.DataPropertyName = "FechaPago";
            this.fechaPagoDataGridViewTextBoxColumn.HeaderText = "Fecha Pago";
            this.fechaPagoDataGridViewTextBoxColumn.Name = "fechaPagoDataGridViewTextBoxColumn";
            this.fechaPagoDataGridViewTextBoxColumn.Visible = false;
            // 
            // folioPrefacturaDataGridViewTextBoxColumn
            // 
            this.folioPrefacturaDataGridViewTextBoxColumn.DataPropertyName = "FolioPrefactura";
            this.folioPrefacturaDataGridViewTextBoxColumn.HeaderText = "Factura";
            this.folioPrefacturaDataGridViewTextBoxColumn.Name = "folioPrefacturaDataGridViewTextBoxColumn";
            this.folioPrefacturaDataGridViewTextBoxColumn.ReadOnly = true;
            this.folioPrefacturaDataGridViewTextBoxColumn.Width = 70;
            // 
            // fechaFacturaDataGridViewTextBoxColumn
            // 
            this.fechaFacturaDataGridViewTextBoxColumn.DataPropertyName = "FechaFactura";
            this.fechaFacturaDataGridViewTextBoxColumn.HeaderText = "Fecha Factura";
            this.fechaFacturaDataGridViewTextBoxColumn.Name = "fechaFacturaDataGridViewTextBoxColumn";
            this.fechaFacturaDataGridViewTextBoxColumn.Visible = false;
            this.fechaFacturaDataGridViewTextBoxColumn.Width = 110;
            // 
            // montoTotalDataGridViewTextBoxColumn
            // 
            this.montoTotalDataGridViewTextBoxColumn.DataPropertyName = "MontoTotal";
            this.montoTotalDataGridViewTextBoxColumn.HeaderText = "Total";
            this.montoTotalDataGridViewTextBoxColumn.Name = "montoTotalDataGridViewTextBoxColumn";
            this.montoTotalDataGridViewTextBoxColumn.ReadOnly = true;
            this.montoTotalDataGridViewTextBoxColumn.Width = 80;
            // 
            // currencyCodeDataGridViewTextBoxColumn
            // 
            this.currencyCodeDataGridViewTextBoxColumn.DataPropertyName = "CurrencyCode";
            this.currencyCodeDataGridViewTextBoxColumn.HeaderText = "Moneda";
            this.currencyCodeDataGridViewTextBoxColumn.Name = "currencyCodeDataGridViewTextBoxColumn";
            this.currencyCodeDataGridViewTextBoxColumn.Visible = false;
            this.currencyCodeDataGridViewTextBoxColumn.Width = 60;
            // 
            // lugarExpedicionDataGridViewTextBoxColumn
            // 
            this.lugarExpedicionDataGridViewTextBoxColumn.DataPropertyName = "LugarExpedicion";
            this.lugarExpedicionDataGridViewTextBoxColumn.HeaderText = "Lugar Expedición";
            this.lugarExpedicionDataGridViewTextBoxColumn.Name = "lugarExpedicionDataGridViewTextBoxColumn";
            this.lugarExpedicionDataGridViewTextBoxColumn.Visible = false;
            this.lugarExpedicionDataGridViewTextBoxColumn.Width = 90;
            // 
            // eNTPagosPorFacturarBindingSource
            // 
            this.eNTPagosPorFacturarBindingSource.DataSource = typeof(Facturacion.ENT.Portal.Facturacion.ENTPagosPorFacturar);
            // 
            // rdbPorDefinir
            // 
            this.rdbPorDefinir.AutoSize = true;
            this.rdbPorDefinir.Location = new System.Drawing.Point(21, 305);
            this.rdbPorDefinir.Name = "rdbPorDefinir";
            this.rdbPorDefinir.Size = new System.Drawing.Size(96, 21);
            this.rdbPorDefinir.TabIndex = 7;
            this.rdbPorDefinir.TabStop = true;
            this.rdbPorDefinir.Text = "Por Definir";
            this.rdbPorDefinir.UseVisualStyleBackColor = true;
            // 
            // ddlPaisResidencia
            // 
            this.ddlPaisResidencia.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlPaisResidencia.FormattingEnabled = true;
            this.ddlPaisResidencia.Location = new System.Drawing.Point(106, 191);
            this.ddlPaisResidencia.Name = "ddlPaisResidencia";
            this.ddlPaisResidencia.Size = new System.Drawing.Size(169, 24);
            this.ddlPaisResidencia.TabIndex = 19;
            this.ddlPaisResidencia.Visible = false;
            // 
            // txtPNR
            // 
            this.txtPNR.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPNR.Location = new System.Drawing.Point(71, 6);
            this.txtPNR.MaxLength = 6;
            this.txtPNR.Name = "txtPNR";
            this.txtPNR.Size = new System.Drawing.Size(107, 29);
            this.txtPNR.TabIndex = 1;
            this.txtPNR.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtPNR.TextChanged += new System.EventHandler(this.txtPNR_TextChanged);
            this.txtPNR.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtPNR_KeyPress);
            // 
            // rdbGastosGral
            // 
            this.rdbGastosGral.AutoSize = true;
            this.rdbGastosGral.Checked = true;
            this.rdbGastosGral.Location = new System.Drawing.Point(21, 282);
            this.rdbGastosGral.Name = "rdbGastosGral";
            this.rdbGastosGral.Size = new System.Drawing.Size(149, 21);
            this.rdbGastosGral.TabIndex = 6;
            this.rdbGastosGral.TabStop = true;
            this.rdbGastosGral.Text = "Gastos en General";
            this.rdbGastosGral.UseVisualStyleBackColor = true;
            // 
            // chkExtranjero
            // 
            this.chkExtranjero.AutoSize = true;
            this.chkExtranjero.Location = new System.Drawing.Point(13, 192);
            this.chkExtranjero.Name = "chkExtranjero";
            this.chkExtranjero.Size = new System.Drawing.Size(94, 21);
            this.chkExtranjero.TabIndex = 4;
            this.chkExtranjero.Text = "Extranjero";
            this.chkExtranjero.UseVisualStyleBackColor = true;
            this.chkExtranjero.CheckedChanged += new System.EventHandler(this.chkExtranjero_CheckedChanged);
            // 
            // lblLeyPNR
            // 
            this.lblLeyPNR.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lblLeyPNR.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLeyPNR.ForeColor = System.Drawing.Color.Red;
            this.lblLeyPNR.Location = new System.Drawing.Point(13, 12);
            this.lblLeyPNR.Name = "lblLeyPNR";
            this.lblLeyPNR.Size = new System.Drawing.Size(66, 17);
            this.lblLeyPNR.TabIndex = 3;
            this.lblLeyPNR.Text = "PNR";
            // 
            // txtEmail
            // 
            this.txtEmail.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtEmail.Location = new System.Drawing.Point(12, 158);
            this.txtEmail.MaxLength = 150;
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(263, 24);
            this.txtEmail.TabIndex = 3;
            // 
            // lblLeyEmail
            // 
            this.lblLeyEmail.BackColor = System.Drawing.Color.Transparent;
            this.lblLeyEmail.Font = new System.Drawing.Font("Microsoft Tai Le", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLeyEmail.Location = new System.Drawing.Point(13, 137);
            this.lblLeyEmail.Name = "lblLeyEmail";
            this.lblLeyEmail.Size = new System.Drawing.Size(249, 17);
            this.lblLeyEmail.TabIndex = 9;
            this.lblLeyEmail.Text = "Email";
            // 
            // txtRFC_TAXID
            // 
            this.txtRFC_TAXID.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtRFC_TAXID.Location = new System.Drawing.Point(106, 222);
            this.txtRFC_TAXID.MaxLength = 13;
            this.txtRFC_TAXID.Name = "txtRFC_TAXID";
            this.txtRFC_TAXID.Size = new System.Drawing.Size(169, 26);
            this.txtRFC_TAXID.TabIndex = 5;
            this.txtRFC_TAXID.TextChanged += new System.EventHandler(this.txtRFC_TAXID_TextChanged);
            this.txtRFC_TAXID.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtRFC_TAXID_KeyPress);
            // 
            // eNTPagosFacturadosBindingSource
            // 
            this.eNTPagosFacturadosBindingSource.DataSource = typeof(Facturacion.ENT.ProcesoFacturacion.ENTPagosFacturados);
            // 
            // lblVersion
            // 
            this.lblVersion.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblVersion.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblVersion.Font = new System.Drawing.Font("Arial Narrow", 7.2F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVersion.Location = new System.Drawing.Point(0, 453);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(302, 22);
            this.lblVersion.TabIndex = 30;
            this.lblVersion.Text = "Version 1.0.3";
            this.lblVersion.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // frmGenerarFacturas
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.ClientSize = new System.Drawing.Size(302, 475);
            this.Controls.Add(this.lblVersion);
            this.Controls.Add(this.pnlDatosFact);
            this.Controls.Add(this.pnlEncabezado);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "frmGenerarFacturas";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.frmGenerarFacturas_Load);
            this.pnlEncabezado.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picLogoViva)).EndInit();
            this.pnlDatosFact.ResumeLayout(false);
            this.pnlDatosFact.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgPagos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.eNTPagosPorFacturarBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.eNTPagosFacturadosBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ImageList imgListaImagenes;
        private System.Windows.Forms.BindingSource eNTPagosFacturadosBindingSource;
        private System.Windows.Forms.BindingSource eNTPagosPorFacturarBindingSource;
        private System.Windows.Forms.PictureBox picLogoViva;
        private System.Windows.Forms.Panel pnlEncabezado;
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Panel pnlDatosFact;
        private System.Windows.Forms.Label lblNumPagos;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblLeyRFC;
        private System.Windows.Forms.Button btnFacturar;
        private System.Windows.Forms.DataGridView dgPagos;
        private System.Windows.Forms.DataGridViewCheckBoxColumn estaMarcadoParaFacturacionDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn idPagosCabDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn paymentIDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn fechaPagoDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn folioPrefacturaDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn fechaFacturaDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn montoTotalDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn currencyCodeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn lugarExpedicionDataGridViewTextBoxColumn;
        private System.Windows.Forms.RadioButton rdbPorDefinir;
        private System.Windows.Forms.ComboBox ddlPaisResidencia;
        private System.Windows.Forms.TextBox txtPNR;
        private System.Windows.Forms.RadioButton rdbGastosGral;
        private System.Windows.Forms.CheckBox chkExtranjero;
        private System.Windows.Forms.Label lblLeyPNR;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.Label lblLeyEmail;
        private System.Windows.Forms.TextBox txtRFC_TAXID;
        private System.Windows.Forms.Label lblVersion;
    }
}

