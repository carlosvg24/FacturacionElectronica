namespace FinanzasTools
{
    partial class frmFinanzasTools
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
            this.pnlTitulo = new System.Windows.Forms.Panel();
            this.lblTitulo = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pnlPiePagina = new System.Windows.Forms.Panel();
            this.pnlFiltros = new System.Windows.Forms.Panel();
            this.gpFacturar = new System.Windows.Forms.GroupBox();
            this.lbPais = new System.Windows.Forms.Label();
            this.cbPais = new System.Windows.Forms.ComboBox();
            this.lblTAX = new System.Windows.Forms.Label();
            this.txtTAX = new System.Windows.Forms.TextBox();
            this.btnSoyExtranjero = new System.Windows.Forms.Button();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.lblEmail = new System.Windows.Forms.Label();
            this.lblRFC = new System.Windows.Forms.Label();
            this.txtRFC = new System.Windows.Forms.TextBox();
            this.gpPago = new System.Windows.Forms.GroupBox();
            this.btnProcesarPNR = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.lblFormaPagoSAT = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lblPaymentID = new System.Windows.Forms.Label();
            this.cmbMetodoPagoFact = new System.Windows.Forms.ComboBox();
            this.btnEditar = new System.Windows.Forms.Button();
            this.btnGuardar = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnBuscar = new System.Windows.Forms.Button();
            this.txtPNR = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.dgViewPagos = new System.Windows.Forms.DataGridView();
            this.DescripcionFormaPagoNavitaire = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DescripcionFormaPagoSAT = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EsFacturable = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.FolioPrefactura = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EsFacturado = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.btnFacturar = new System.Windows.Forms.Button();
            this.cbUsoCFDI = new System.Windows.Forms.ComboBox();
            this.lblUsoCFDI = new System.Windows.Forms.Label();
            this.paymentIDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.fechaPagoDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.paymentMethodCodeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.currencyCodeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.folioFacturaDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.fechaFacturaDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.eNTPagosCambiaFormaPagoBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.pnlTitulo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.pnlFiltros.SuspendLayout();
            this.gpFacturar.SuspendLayout();
            this.gpPago.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgViewPagos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.eNTPagosCambiaFormaPagoBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlTitulo
            // 
            this.pnlTitulo.Controls.Add(this.lblTitulo);
            this.pnlTitulo.Controls.Add(this.pictureBox1);
            this.pnlTitulo.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTitulo.Location = new System.Drawing.Point(0, 0);
            this.pnlTitulo.Margin = new System.Windows.Forms.Padding(2);
            this.pnlTitulo.Name = "pnlTitulo";
            this.pnlTitulo.Size = new System.Drawing.Size(1028, 136);
            this.pnlTitulo.TabIndex = 0;
            // 
            // lblTitulo
            // 
            this.lblTitulo.BackColor = System.Drawing.Color.White;
            this.lblTitulo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblTitulo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTitulo.Font = new System.Drawing.Font("Verdana", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitulo.ForeColor = System.Drawing.Color.Green;
            this.lblTitulo.Location = new System.Drawing.Point(0, 100);
            this.lblTitulo.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(1028, 36);
            this.lblTitulo.TabIndex = 1;
            this.lblTitulo.Text = "Reclasificación Forma de Pago Grupos";
            this.lblTitulo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.AliceBlue;
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.pictureBox1.Image = global::FinanzasTools.Properties.Resources.logo;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(1028, 100);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // pnlPiePagina
            // 
            this.pnlPiePagina.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlPiePagina.Location = new System.Drawing.Point(0, 686);
            this.pnlPiePagina.Margin = new System.Windows.Forms.Padding(2);
            this.pnlPiePagina.Name = "pnlPiePagina";
            this.pnlPiePagina.Size = new System.Drawing.Size(1028, 38);
            this.pnlPiePagina.TabIndex = 1;
            // 
            // pnlFiltros
            // 
            this.pnlFiltros.Controls.Add(this.gpFacturar);
            this.pnlFiltros.Controls.Add(this.gpPago);
            this.pnlFiltros.Controls.Add(this.groupBox1);
            this.pnlFiltros.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlFiltros.Location = new System.Drawing.Point(0, 136);
            this.pnlFiltros.Margin = new System.Windows.Forms.Padding(2);
            this.pnlFiltros.Name = "pnlFiltros";
            this.pnlFiltros.Size = new System.Drawing.Size(303, 550);
            this.pnlFiltros.TabIndex = 2;
            // 
            // gpFacturar
            // 
            this.gpFacturar.Controls.Add(this.lblUsoCFDI);
            this.gpFacturar.Controls.Add(this.cbUsoCFDI);
            this.gpFacturar.Controls.Add(this.btnFacturar);
            this.gpFacturar.Controls.Add(this.lbPais);
            this.gpFacturar.Controls.Add(this.cbPais);
            this.gpFacturar.Controls.Add(this.lblTAX);
            this.gpFacturar.Controls.Add(this.txtTAX);
            this.gpFacturar.Controls.Add(this.btnSoyExtranjero);
            this.gpFacturar.Controls.Add(this.txtEmail);
            this.gpFacturar.Controls.Add(this.lblEmail);
            this.gpFacturar.Controls.Add(this.lblRFC);
            this.gpFacturar.Controls.Add(this.txtRFC);
            this.gpFacturar.Location = new System.Drawing.Point(12, 320);
            this.gpFacturar.Name = "gpFacturar";
            this.gpFacturar.Size = new System.Drawing.Size(278, 228);
            this.gpFacturar.TabIndex = 20;
            this.gpFacturar.TabStop = false;
            this.gpFacturar.Text = "Facturar";
            // 
            // lbPais
            // 
            this.lbPais.AutoSize = true;
            this.lbPais.Location = new System.Drawing.Point(40, 169);
            this.lbPais.Name = "lbPais";
            this.lbPais.Size = new System.Drawing.Size(27, 13);
            this.lbPais.TabIndex = 8;
            this.lbPais.Text = "Pais";
            // 
            // cbPais
            // 
            this.cbPais.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbPais.FormattingEnabled = true;
            this.cbPais.Location = new System.Drawing.Point(73, 164);
            this.cbPais.Name = "cbPais";
            this.cbPais.Size = new System.Drawing.Size(187, 24);
            this.cbPais.TabIndex = 7;
            // 
            // lblTAX
            // 
            this.lblTAX.AutoSize = true;
            this.lblTAX.Location = new System.Drawing.Point(25, 141);
            this.lblTAX.Name = "lblTAX";
            this.lblTAX.Size = new System.Drawing.Size(42, 13);
            this.lblTAX.TabIndex = 6;
            this.lblTAX.Text = "TAX ID";
            // 
            // txtTAX
            // 
            this.txtTAX.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTAX.Location = new System.Drawing.Point(73, 136);
            this.txtTAX.Name = "txtTAX";
            this.txtTAX.Size = new System.Drawing.Size(186, 22);
            this.txtTAX.TabIndex = 5;
            // 
            // btnSoyExtranjero
            // 
            this.btnSoyExtranjero.Location = new System.Drawing.Point(160, 34);
            this.btnSoyExtranjero.Name = "btnSoyExtranjero";
            this.btnSoyExtranjero.Size = new System.Drawing.Size(99, 23);
            this.btnSoyExtranjero.TabIndex = 4;
            this.btnSoyExtranjero.Text = "Extranjero";
            this.btnSoyExtranjero.UseVisualStyleBackColor = true;
            this.btnSoyExtranjero.Click += new System.EventHandler(this.btnSoyExtranjero_Click);
            // 
            // txtEmail
            // 
            this.txtEmail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtEmail.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtEmail.Location = new System.Drawing.Point(14, 78);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(245, 22);
            this.txtEmail.TabIndex = 3;
            // 
            // lblEmail
            // 
            this.lblEmail.AutoSize = true;
            this.lblEmail.Location = new System.Drawing.Point(14, 62);
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.Size = new System.Drawing.Size(34, 13);
            this.lblEmail.TabIndex = 2;
            this.lblEmail.Text = "e-mail";
            // 
            // lblRFC
            // 
            this.lblRFC.AutoSize = true;
            this.lblRFC.Location = new System.Drawing.Point(14, 19);
            this.lblRFC.Name = "lblRFC";
            this.lblRFC.Size = new System.Drawing.Size(28, 13);
            this.lblRFC.TabIndex = 1;
            this.lblRFC.Text = "RFC";
            // 
            // txtRFC
            // 
            this.txtRFC.BackColor = System.Drawing.SystemColors.Window;
            this.txtRFC.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtRFC.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtRFC.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtRFC.Location = new System.Drawing.Point(13, 35);
            this.txtRFC.Name = "txtRFC";
            this.txtRFC.Size = new System.Drawing.Size(130, 22);
            this.txtRFC.TabIndex = 0;
            // 
            // gpPago
            // 
            this.gpPago.Controls.Add(this.btnProcesarPNR);
            this.gpPago.Controls.Add(this.btnCancelar);
            this.gpPago.Controls.Add(this.lblFormaPagoSAT);
            this.gpPago.Controls.Add(this.label5);
            this.gpPago.Controls.Add(this.lblPaymentID);
            this.gpPago.Controls.Add(this.cmbMetodoPagoFact);
            this.gpPago.Controls.Add(this.btnEditar);
            this.gpPago.Controls.Add(this.btnGuardar);
            this.gpPago.Controls.Add(this.label4);
            this.gpPago.Controls.Add(this.label2);
            this.gpPago.Location = new System.Drawing.Point(9, 68);
            this.gpPago.Margin = new System.Windows.Forms.Padding(2);
            this.gpPago.Name = "gpPago";
            this.gpPago.Padding = new System.Windows.Forms.Padding(2);
            this.gpPago.Size = new System.Drawing.Size(281, 247);
            this.gpPago.TabIndex = 19;
            this.gpPago.TabStop = false;
            this.gpPago.Text = "Información Pago Seleccionado ...";
            this.gpPago.Enter += new System.EventHandler(this.gpPago_Enter);
            // 
            // btnProcesarPNR
            // 
            this.btnProcesarPNR.Location = new System.Drawing.Point(17, 178);
            this.btnProcesarPNR.Margin = new System.Windows.Forms.Padding(2);
            this.btnProcesarPNR.Name = "btnProcesarPNR";
            this.btnProcesarPNR.Size = new System.Drawing.Size(114, 26);
            this.btnProcesarPNR.TabIndex = 27;
            this.btnProcesarPNR.Text = "Procesar PNR";
            this.btnProcesarPNR.UseVisualStyleBackColor = true;
            this.btnProcesarPNR.Visible = false;
            this.btnProcesarPNR.Click += new System.EventHandler(this.btnActivarFact_Click);
            // 
            // btnCancelar
            // 
            this.btnCancelar.Location = new System.Drawing.Point(148, 212);
            this.btnCancelar.Margin = new System.Windows.Forms.Padding(2);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(114, 26);
            this.btnCancelar.TabIndex = 26;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = true;
            this.btnCancelar.Visible = false;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // lblFormaPagoSAT
            // 
            this.lblFormaPagoSAT.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.lblFormaPagoSAT.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblFormaPagoSAT.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFormaPagoSAT.Location = new System.Drawing.Point(17, 128);
            this.lblFormaPagoSAT.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblFormaPagoSAT.Name = "lblFormaPagoSAT";
            this.lblFormaPagoSAT.Size = new System.Drawing.Size(245, 43);
            this.lblFormaPagoSAT.TabIndex = 25;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(17, 111);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(162, 13);
            this.label5.TabIndex = 24;
            this.label5.Text = "Forma de Pago Facturación SAT";
            // 
            // lblPaymentID
            // 
            this.lblPaymentID.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.lblPaymentID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblPaymentID.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPaymentID.Location = new System.Drawing.Point(17, 37);
            this.lblPaymentID.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblPaymentID.Name = "lblPaymentID";
            this.lblPaymentID.Size = new System.Drawing.Size(245, 23);
            this.lblPaymentID.TabIndex = 23;
            this.lblPaymentID.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmbMetodoPagoFact
            // 
            this.cmbMetodoPagoFact.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cmbMetodoPagoFact.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbMetodoPagoFact.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbMetodoPagoFact.FormattingEnabled = true;
            this.cmbMetodoPagoFact.Location = new System.Drawing.Point(17, 81);
            this.cmbMetodoPagoFact.Margin = new System.Windows.Forms.Padding(2);
            this.cmbMetodoPagoFact.Name = "cmbMetodoPagoFact";
            this.cmbMetodoPagoFact.Size = new System.Drawing.Size(246, 24);
            this.cmbMetodoPagoFact.TabIndex = 22;
            this.cmbMetodoPagoFact.SelectedIndexChanged += new System.EventHandler(this.cmbMetodoPagoFact_SelectedIndexChanged);
            // 
            // btnEditar
            // 
            this.btnEditar.Location = new System.Drawing.Point(148, 178);
            this.btnEditar.Margin = new System.Windows.Forms.Padding(2);
            this.btnEditar.Name = "btnEditar";
            this.btnEditar.Size = new System.Drawing.Size(114, 26);
            this.btnEditar.TabIndex = 21;
            this.btnEditar.Text = "Editar";
            this.btnEditar.UseVisualStyleBackColor = true;
            this.btnEditar.Visible = false;
            this.btnEditar.Click += new System.EventHandler(this.btnEditar_Click);
            // 
            // btnGuardar
            // 
            this.btnGuardar.Location = new System.Drawing.Point(17, 212);
            this.btnGuardar.Margin = new System.Windows.Forms.Padding(2);
            this.btnGuardar.Name = "btnGuardar";
            this.btnGuardar.Size = new System.Drawing.Size(114, 26);
            this.btnGuardar.TabIndex = 20;
            this.btnGuardar.Text = "Guardar";
            this.btnGuardar.UseVisualStyleBackColor = true;
            this.btnGuardar.Visible = false;
            this.btnGuardar.Click += new System.EventHandler(this.btnGuardar_Click);
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(17, 65);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(153, 24);
            this.label4.TabIndex = 19;
            this.label4.Text = "Forma de Pago Navitaire";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(17, 20);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 13);
            this.label2.TabIndex = 18;
            this.label2.Text = "Payment Id";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnBuscar);
            this.groupBox1.Controls.Add(this.txtPNR);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(9, 5);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox1.Size = new System.Drawing.Size(281, 55);
            this.groupBox1.TabIndex = 18;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Filtro Búsqueda ...";
            // 
            // btnBuscar
            // 
            this.btnBuscar.Location = new System.Drawing.Point(174, 19);
            this.btnBuscar.Margin = new System.Windows.Forms.Padding(2);
            this.btnBuscar.Name = "btnBuscar";
            this.btnBuscar.Size = new System.Drawing.Size(91, 27);
            this.btnBuscar.TabIndex = 12;
            this.btnBuscar.Text = "Buscar";
            this.btnBuscar.UseVisualStyleBackColor = true;
            this.btnBuscar.Click += new System.EventHandler(this.btnBuscar_Click);
            // 
            // txtPNR
            // 
            this.txtPNR.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPNR.Location = new System.Drawing.Point(50, 20);
            this.txtPNR.Margin = new System.Windows.Forms.Padding(2);
            this.txtPNR.Name = "txtPNR";
            this.txtPNR.Size = new System.Drawing.Size(120, 26);
            this.txtPNR.TabIndex = 11;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 26);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(30, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "PNR";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.dgViewPagos);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(303, 136);
            this.panel1.Margin = new System.Windows.Forms.Padding(2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(725, 550);
            this.panel1.TabIndex = 3;
            // 
            // dgViewPagos
            // 
            this.dgViewPagos.AllowUserToAddRows = false;
            this.dgViewPagos.AllowUserToDeleteRows = false;
            this.dgViewPagos.AllowUserToResizeColumns = false;
            this.dgViewPagos.AllowUserToResizeRows = false;
            this.dgViewPagos.AutoGenerateColumns = false;
            this.dgViewPagos.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.dgViewPagos.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.paymentIDDataGridViewTextBoxColumn,
            this.fechaPagoDataGridViewTextBoxColumn,
            this.paymentMethodCodeDataGridViewTextBoxColumn,
            this.DescripcionFormaPagoNavitaire,
            this.Column1,
            this.DescripcionFormaPagoSAT,
            this.currencyCodeDataGridViewTextBoxColumn,
            this.EsFacturable,
            this.FolioPrefactura,
            this.EsFacturado,
            this.folioFacturaDataGridViewTextBoxColumn,
            this.fechaFacturaDataGridViewTextBoxColumn});
            this.dgViewPagos.DataSource = this.eNTPagosCambiaFormaPagoBindingSource;
            this.dgViewPagos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgViewPagos.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgViewPagos.Location = new System.Drawing.Point(0, 0);
            this.dgViewPagos.Margin = new System.Windows.Forms.Padding(2);
            this.dgViewPagos.MultiSelect = false;
            this.dgViewPagos.Name = "dgViewPagos";
            this.dgViewPagos.ReadOnly = true;
            this.dgViewPagos.RowTemplate.Height = 24;
            this.dgViewPagos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgViewPagos.ShowCellErrors = false;
            this.dgViewPagos.ShowCellToolTips = false;
            this.dgViewPagos.ShowEditingIcon = false;
            this.dgViewPagos.ShowRowErrors = false;
            this.dgViewPagos.Size = new System.Drawing.Size(725, 550);
            this.dgViewPagos.TabIndex = 0;
            this.dgViewPagos.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgViewPagos_RowEnter);
            this.dgViewPagos.Enter += new System.EventHandler(this.gpPago_Enter);
            // 
            // DescripcionFormaPagoNavitaire
            // 
            this.DescripcionFormaPagoNavitaire.DataPropertyName = "DescripcionFormaPagoNavitaire";
            this.DescripcionFormaPagoNavitaire.HeaderText = "Forma Pago Navitaire";
            this.DescripcionFormaPagoNavitaire.Name = "DescripcionFormaPagoNavitaire";
            this.DescripcionFormaPagoNavitaire.ReadOnly = true;
            this.DescripcionFormaPagoNavitaire.Width = 134;
            // 
            // Column1
            // 
            this.Column1.DataPropertyName = "CveFormaPagoSAT";
            this.Column1.HeaderText = "Cve Pago SAT";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Width = 103;
            // 
            // DescripcionFormaPagoSAT
            // 
            this.DescripcionFormaPagoSAT.DataPropertyName = "DescripcionFormaPagoSAT";
            this.DescripcionFormaPagoSAT.HeaderText = "Forma Pago SAT";
            this.DescripcionFormaPagoSAT.Name = "DescripcionFormaPagoSAT";
            this.DescripcionFormaPagoSAT.ReadOnly = true;
            this.DescripcionFormaPagoSAT.Width = 113;
            // 
            // EsFacturable
            // 
            this.EsFacturable.DataPropertyName = "EsFacturable";
            this.EsFacturable.HeaderText = "Es Facturable";
            this.EsFacturable.Name = "EsFacturable";
            this.EsFacturable.ReadOnly = true;
            this.EsFacturable.Width = 78;
            // 
            // FolioPrefactura
            // 
            this.FolioPrefactura.DataPropertyName = "FolioPrefactura";
            this.FolioPrefactura.HeaderText = "Folio Prefactura";
            this.FolioPrefactura.Name = "FolioPrefactura";
            this.FolioPrefactura.ReadOnly = true;
            this.FolioPrefactura.Width = 106;
            // 
            // EsFacturado
            // 
            this.EsFacturado.DataPropertyName = "EsFacturado";
            this.EsFacturado.HeaderText = "Es Facturado";
            this.EsFacturado.Name = "EsFacturado";
            this.EsFacturado.ReadOnly = true;
            this.EsFacturado.Width = 76;
            // 
            // btnFacturar
            // 
            this.btnFacturar.Location = new System.Drawing.Point(160, 194);
            this.btnFacturar.Name = "btnFacturar";
            this.btnFacturar.Size = new System.Drawing.Size(102, 23);
            this.btnFacturar.TabIndex = 9;
            this.btnFacturar.Text = "Facturar";
            this.btnFacturar.UseVisualStyleBackColor = true;
            this.btnFacturar.Click += new System.EventHandler(this.btnFacturar_Click);
            // 
            // cbUsoCFDI
            // 
            this.cbUsoCFDI.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbUsoCFDI.FormattingEnabled = true;
            this.cbUsoCFDI.Location = new System.Drawing.Point(73, 106);
            this.cbUsoCFDI.Name = "cbUsoCFDI";
            this.cbUsoCFDI.Size = new System.Drawing.Size(186, 24);
            this.cbUsoCFDI.TabIndex = 10;
            // 
            // lblUsoCFDI
            // 
            this.lblUsoCFDI.AutoSize = true;
            this.lblUsoCFDI.Location = new System.Drawing.Point(14, 111);
            this.lblUsoCFDI.Name = "lblUsoCFDI";
            this.lblUsoCFDI.Size = new System.Drawing.Size(53, 13);
            this.lblUsoCFDI.TabIndex = 11;
            this.lblUsoCFDI.Text = "Uso CFDI";
            // 
            // paymentIDDataGridViewTextBoxColumn
            // 
            this.paymentIDDataGridViewTextBoxColumn.DataPropertyName = "PaymentID";
            this.paymentIDDataGridViewTextBoxColumn.HeaderText = "Payment ID";
            this.paymentIDDataGridViewTextBoxColumn.Name = "paymentIDDataGridViewTextBoxColumn";
            this.paymentIDDataGridViewTextBoxColumn.ReadOnly = true;
            this.paymentIDDataGridViewTextBoxColumn.Width = 87;
            // 
            // fechaPagoDataGridViewTextBoxColumn
            // 
            this.fechaPagoDataGridViewTextBoxColumn.DataPropertyName = "FechaPago";
            this.fechaPagoDataGridViewTextBoxColumn.HeaderText = "Fecha Pago";
            this.fechaPagoDataGridViewTextBoxColumn.Name = "fechaPagoDataGridViewTextBoxColumn";
            this.fechaPagoDataGridViewTextBoxColumn.ReadOnly = true;
            this.fechaPagoDataGridViewTextBoxColumn.Width = 90;
            // 
            // paymentMethodCodeDataGridViewTextBoxColumn
            // 
            this.paymentMethodCodeDataGridViewTextBoxColumn.DataPropertyName = "PaymentMethodCode";
            this.paymentMethodCodeDataGridViewTextBoxColumn.HeaderText = "Cve Pago Nav";
            this.paymentMethodCodeDataGridViewTextBoxColumn.Name = "paymentMethodCodeDataGridViewTextBoxColumn";
            this.paymentMethodCodeDataGridViewTextBoxColumn.ReadOnly = true;
            this.paymentMethodCodeDataGridViewTextBoxColumn.Width = 102;
            // 
            // currencyCodeDataGridViewTextBoxColumn
            // 
            this.currencyCodeDataGridViewTextBoxColumn.DataPropertyName = "CurrencyCode";
            this.currencyCodeDataGridViewTextBoxColumn.HeaderText = "Moneda";
            this.currencyCodeDataGridViewTextBoxColumn.Name = "currencyCodeDataGridViewTextBoxColumn";
            this.currencyCodeDataGridViewTextBoxColumn.ReadOnly = true;
            this.currencyCodeDataGridViewTextBoxColumn.Width = 71;
            // 
            // folioFacturaDataGridViewTextBoxColumn
            // 
            this.folioFacturaDataGridViewTextBoxColumn.DataPropertyName = "FolioFactura";
            this.folioFacturaDataGridViewTextBoxColumn.HeaderText = "Folio Factura";
            this.folioFacturaDataGridViewTextBoxColumn.Name = "folioFacturaDataGridViewTextBoxColumn";
            this.folioFacturaDataGridViewTextBoxColumn.ReadOnly = true;
            this.folioFacturaDataGridViewTextBoxColumn.Width = 93;
            // 
            // fechaFacturaDataGridViewTextBoxColumn
            // 
            this.fechaFacturaDataGridViewTextBoxColumn.DataPropertyName = "FechaFactura";
            this.fechaFacturaDataGridViewTextBoxColumn.HeaderText = "Fecha Factura";
            this.fechaFacturaDataGridViewTextBoxColumn.Name = "fechaFacturaDataGridViewTextBoxColumn";
            this.fechaFacturaDataGridViewTextBoxColumn.ReadOnly = true;
            this.fechaFacturaDataGridViewTextBoxColumn.Width = 101;
            // 
            // eNTPagosCambiaFormaPagoBindingSource
            // 
            this.eNTPagosCambiaFormaPagoBindingSource.DataSource = typeof(FinanzasTools.ENTPagosCambiaFormaPago);
            // 
            // frmFinanzasTools
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1028, 724);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.pnlFiltros);
            this.Controls.Add(this.pnlPiePagina);
            this.Controls.Add(this.pnlTitulo);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "frmFinanzasTools";
            this.Text = "Finanzas Facturación...";
            this.pnlTitulo.ResumeLayout(false);
            this.pnlTitulo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.pnlFiltros.ResumeLayout(false);
            this.gpFacturar.ResumeLayout(false);
            this.gpFacturar.PerformLayout();
            this.gpPago.ResumeLayout(false);
            this.gpPago.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgViewPagos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.eNTPagosCambiaFormaPagoBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlTitulo;
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel pnlPiePagina;
        private System.Windows.Forms.Panel pnlFiltros;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridView dgViewPagos;
        private System.Windows.Forms.BindingSource eNTPagosCambiaFormaPagoBindingSource;
        private System.Windows.Forms.GroupBox gpPago;
        private System.Windows.Forms.Button btnProcesarPNR;
        private System.Windows.Forms.Button btnCancelar;
        private System.Windows.Forms.Label lblFormaPagoSAT;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lblPaymentID;
        private System.Windows.Forms.ComboBox cmbMetodoPagoFact;
        private System.Windows.Forms.Button btnEditar;
        private System.Windows.Forms.Button btnGuardar;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnBuscar;
        private System.Windows.Forms.TextBox txtPNR;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridViewTextBoxColumn paymentIDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn fechaPagoDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn paymentMethodCodeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn DescripcionFormaPagoNavitaire;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn DescripcionFormaPagoSAT;
        private System.Windows.Forms.DataGridViewTextBoxColumn currencyCodeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn EsFacturable;
        private System.Windows.Forms.DataGridViewTextBoxColumn FolioPrefactura;
        private System.Windows.Forms.DataGridViewCheckBoxColumn EsFacturado;
        private System.Windows.Forms.DataGridViewTextBoxColumn folioFacturaDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn fechaFacturaDataGridViewTextBoxColumn;
        private System.Windows.Forms.GroupBox gpFacturar;
        private System.Windows.Forms.Button btnSoyExtranjero;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.Label lblEmail;
        private System.Windows.Forms.Label lblRFC;
        private System.Windows.Forms.TextBox txtRFC;
        private System.Windows.Forms.Label lbPais;
        private System.Windows.Forms.ComboBox cbPais;
        private System.Windows.Forms.Label lblTAX;
        private System.Windows.Forms.TextBox txtTAX;
        private System.Windows.Forms.Button btnFacturar;
        private System.Windows.Forms.Label lblUsoCFDI;
        private System.Windows.Forms.ComboBox cbUsoCFDI;
    }
}

