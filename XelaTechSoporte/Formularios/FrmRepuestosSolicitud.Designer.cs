namespace XelaTechSoporte.Formularios
{
    partial class FrmRepuestosSolicitud
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

        private void InitializeComponent()
        {
            lblTitulo = new Label();
            lblSolicitud = new Label();
            lblRepuesto = new Label();
            cboRepuesto = new ComboBox();
            lblCantidad = new Label();
            txtCantidad = new TextBox();
            btnAgregar = new Button();
            lblExistencia = new Label();
            dgvDetalles = new DataGridView();
            lblTotal = new Label();
            btnCerrar = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvDetalles).BeginInit();
            SuspendLayout();
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            lblTitulo.ForeColor = Color.FromArgb(31, 41, 55);
            lblTitulo.Location = new Point(20, 14);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(229, 25);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Repuestos de la solicitud";
            lblSolicitud.ForeColor = Color.FromArgb(107, 114, 128);
            lblSolicitud.Location = new Point(20, 44);
            lblSolicitud.Name = "lblSolicitud";
            lblSolicitud.Size = new Size(600, 20);
            lblSolicitud.TabIndex = 1;
            lblSolicitud.Text = "Solicitud";
            lblRepuesto.AutoSize = true;
            lblRepuesto.Location = new Point(20, 76);
            lblRepuesto.Name = "lblRepuesto";
            lblRepuesto.Size = new Size(59, 15);
            lblRepuesto.TabIndex = 2;
            lblRepuesto.Text = "Repuesto:";
            cboRepuesto.DropDownStyle = ComboBoxStyle.DropDownList;
            cboRepuesto.FormattingEnabled = true;
            cboRepuesto.Location = new Point(20, 96);
            cboRepuesto.Name = "cboRepuesto";
            cboRepuesto.Size = new Size(360, 23);
            cboRepuesto.TabIndex = 3;
            cboRepuesto.SelectedIndexChanged += cboRepuesto_SelectedIndexChanged;
            lblCantidad.AutoSize = true;
            lblCantidad.Location = new Point(390, 76);
            lblCantidad.Name = "lblCantidad";
            lblCantidad.Size = new Size(58, 15);
            lblCantidad.TabIndex = 4;
            lblCantidad.Text = "Cantidad:";
            txtCantidad.BorderStyle = BorderStyle.FixedSingle;
            txtCantidad.Location = new Point(390, 96);
            txtCantidad.Name = "txtCantidad";
            txtCantidad.Size = new Size(70, 23);
            txtCantidad.TabIndex = 5;
            txtCantidad.Text = "1";
            btnAgregar.BackColor = Color.FromArgb(220, 38, 38);
            btnAgregar.Cursor = Cursors.Hand;
            btnAgregar.FlatAppearance.BorderSize = 0;
            btnAgregar.FlatStyle = FlatStyle.Flat;
            btnAgregar.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnAgregar.ForeColor = Color.White;
            btnAgregar.Location = new Point(470, 93);
            btnAgregar.Name = "btnAgregar";
            btnAgregar.Size = new Size(150, 30);
            btnAgregar.TabIndex = 6;
            btnAgregar.Text = " Agregar";
            btnAgregar.UseVisualStyleBackColor = false;
            btnAgregar.Click += btnAgregar_Click;
            lblExistencia.ForeColor = Color.FromArgb(107, 114, 128);
            lblExistencia.Location = new Point(20, 126);
            lblExistencia.Name = "lblExistencia";
            lblExistencia.Size = new Size(600, 20);
            lblExistencia.TabIndex = 7;
            dgvDetalles.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvDetalles.Location = new Point(20, 152);
            dgvDetalles.Name = "dgvDetalles";
            dgvDetalles.Size = new Size(600, 222);
            dgvDetalles.TabIndex = 8;
            lblTotal.AutoSize = true;
            lblTotal.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblTotal.ForeColor = Color.FromArgb(31, 41, 55);
            lblTotal.Location = new Point(20, 392);
            lblTotal.Name = "lblTotal";
            lblTotal.Size = new Size(180, 19);
            lblTotal.TabIndex = 9;
            lblTotal.Text = "Costo de repuestos: Q 0.00";
            btnCerrar.BackColor = Color.White;
            btnCerrar.Cursor = Cursors.Hand;
            btnCerrar.DialogResult = DialogResult.OK;
            btnCerrar.FlatAppearance.BorderColor = Color.FromArgb(209, 213, 219);
            btnCerrar.FlatStyle = FlatStyle.Flat;
            btnCerrar.ForeColor = Color.FromArgb(31, 41, 55);
            btnCerrar.Location = new Point(520, 386);
            btnCerrar.Name = "btnCerrar";
            btnCerrar.Size = new Size(100, 34);
            btnCerrar.TabIndex = 10;
            btnCerrar.Text = "Cerrar";
            btnCerrar.UseVisualStyleBackColor = false;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            CancelButton = btnCerrar;
            ClientSize = new Size(640, 436);
            Controls.Add(btnCerrar);
            Controls.Add(lblTotal);
            Controls.Add(dgvDetalles);
            Controls.Add(lblExistencia);
            Controls.Add(btnAgregar);
            Controls.Add(txtCantidad);
            Controls.Add(lblCantidad);
            Controls.Add(cboRepuesto);
            Controls.Add(lblRepuesto);
            Controls.Add(lblSolicitud);
            Controls.Add(lblTitulo);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmRepuestosSolicitud";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Repuestos utilizados";
            ((System.ComponentModel.ISupportInitialize)dgvDetalles).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        private Label lblTitulo;
        private Label lblSolicitud;
        private Label lblRepuesto;
        private ComboBox cboRepuesto;
        private Label lblCantidad;
        private TextBox txtCantidad;
        private Button btnAgregar;
        private Label lblExistencia;
        private DataGridView dgvDetalles;
        private Label lblTotal;
        private Button btnCerrar;
    }
}
