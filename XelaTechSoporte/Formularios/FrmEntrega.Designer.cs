namespace XelaTechSoporte.Formularios
{
    partial class FrmEntrega
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

        #region Código generado por el Diseñador de Windows Forms

        private void InitializeComponent()
        {
            lblTitulo = new Label();
            lblSolicitud = new Label();
            lblRepuestos = new Label();
            lblRepuestosValor = new Label();
            lblManoObra = new Label();
            txtManoObra = new TextBox();
            lblTotal = new Label();
            lblTotalValor = new Label();
            lblCalificacion = new Label();
            cboCalificacion = new ComboBox();
            btnEntregar = new Button();
            btnCancelar = new Button();
            SuspendLayout();
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            lblTitulo.ForeColor = Color.FromArgb(31, 41, 55);
            lblTitulo.Location = new Point(20, 14);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(160, 25);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Entregar y cobrar";
            lblSolicitud.ForeColor = Color.FromArgb(107, 114, 128);
            lblSolicitud.Location = new Point(20, 44);
            lblSolicitud.Name = "lblSolicitud";
            lblSolicitud.Size = new Size(400, 40);
            lblSolicitud.TabIndex = 1;
            lblSolicitud.Text = "Solicitud";
            lblRepuestos.AutoSize = true;
            lblRepuestos.Location = new Point(20, 96);
            lblRepuestos.Name = "lblRepuestos";
            lblRepuestos.Size = new Size(114, 15);
            lblRepuestos.TabIndex = 2;
            lblRepuestos.Text = "Costo de repuestos:";
            lblRepuestosValor.AutoSize = true;
            lblRepuestosValor.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblRepuestosValor.Location = new Point(240, 96);
            lblRepuestosValor.Name = "lblRepuestosValor";
            lblRepuestosValor.Size = new Size(38, 15);
            lblRepuestosValor.TabIndex = 3;
            lblRepuestosValor.Text = "Q 0.00";
            lblManoObra.AutoSize = true;
            lblManoObra.Location = new Point(20, 128);
            lblManoObra.Name = "lblManoObra";
            lblManoObra.Size = new Size(124, 15);
            lblManoObra.TabIndex = 4;
            lblManoObra.Text = "Mano de obra (Q):";
            txtManoObra.BorderStyle = BorderStyle.FixedSingle;
            txtManoObra.Location = new Point(240, 125);
            txtManoObra.Name = "txtManoObra";
            txtManoObra.Size = new Size(180, 23);
            txtManoObra.TabIndex = 5;
            txtManoObra.TextChanged += txtManoObra_TextChanged;
            lblTotal.AutoSize = true;
            lblTotal.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblTotal.Location = new Point(20, 168);
            lblTotal.Name = "lblTotal";
            lblTotal.Size = new Size(106, 19);
            lblTotal.TabIndex = 6;
            lblTotal.Text = "Total a cobrar:";
            lblTotalValor.AutoSize = true;
            lblTotalValor.Font = new Font("Segoe UI", 15F, FontStyle.Bold);
            lblTotalValor.ForeColor = Color.FromArgb(220, 38, 38);
            lblTotalValor.Location = new Point(236, 160);
            lblTotalValor.Name = "lblTotalValor";
            lblTotalValor.Size = new Size(75, 28);
            lblTotalValor.TabIndex = 7;
            lblTotalValor.Text = "Q 0.00";
            lblCalificacion.AutoSize = true;
            lblCalificacion.Location = new Point(20, 212);
            lblCalificacion.Name = "lblCalificacion";
            lblCalificacion.Size = new Size(200, 15);
            lblCalificacion.TabIndex = 8;
            lblCalificacion.Text = "Calificación del cliente al técnico:";
            cboCalificacion.DropDownStyle = ComboBoxStyle.DropDownList;
            cboCalificacion.FormattingEnabled = true;
            cboCalificacion.Items.AddRange(new object[] { "Sin calificar", "1 ★", "2 ★★", "3 ★★★", "4 ★★★★", "5 ★★★★★" });
            cboCalificacion.Location = new Point(240, 209);
            cboCalificacion.Name = "cboCalificacion";
            cboCalificacion.Size = new Size(180, 23);
            cboCalificacion.TabIndex = 9;
            btnEntregar.BackColor = Color.FromArgb(220, 38, 38);
            btnEntregar.Cursor = Cursors.Hand;
            btnEntregar.FlatAppearance.BorderSize = 0;
            btnEntregar.FlatStyle = FlatStyle.Flat;
            btnEntregar.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnEntregar.ForeColor = Color.White;
            btnEntregar.Location = new Point(190, 258);
            btnEntregar.Name = "btnEntregar";
            btnEntregar.Size = new Size(140, 36);
            btnEntregar.TabIndex = 10;
            btnEntregar.Text = " Entregar y cobrar";
            btnEntregar.UseVisualStyleBackColor = false;
            btnEntregar.Click += btnEntregar_Click;
            btnCancelar.BackColor = Color.White;
            btnCancelar.Cursor = Cursors.Hand;
            btnCancelar.DialogResult = DialogResult.Cancel;
            btnCancelar.FlatAppearance.BorderColor = Color.FromArgb(209, 213, 219);
            btnCancelar.FlatStyle = FlatStyle.Flat;
            btnCancelar.ForeColor = Color.FromArgb(31, 41, 55);
            btnCancelar.Location = new Point(338, 258);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(82, 36);
            btnCancelar.TabIndex = 11;
            btnCancelar.Text = "Cancelar";
            btnCancelar.UseVisualStyleBackColor = false;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            CancelButton = btnCancelar;
            ClientSize = new Size(440, 312);
            Controls.Add(btnCancelar);
            Controls.Add(btnEntregar);
            Controls.Add(cboCalificacion);
            Controls.Add(lblCalificacion);
            Controls.Add(lblTotalValor);
            Controls.Add(lblTotal);
            Controls.Add(txtManoObra);
            Controls.Add(lblManoObra);
            Controls.Add(lblRepuestosValor);
            Controls.Add(lblRepuestos);
            Controls.Add(lblSolicitud);
            Controls.Add(lblTitulo);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmEntrega";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Entrega";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblTitulo;
        private Label lblSolicitud;
        private Label lblRepuestos;
        private Label lblRepuestosValor;
        private Label lblManoObra;
        private TextBox txtManoObra;
        private Label lblTotal;
        private Label lblTotalValor;
        private Label lblCalificacion;
        private ComboBox cboCalificacion;
        private Button btnEntregar;
        private Button btnCancelar;
    }
}
