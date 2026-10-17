namespace XelaTechSoporte.Formularios
{
    partial class FrmCambiarEstado
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
            lblActual = new Label();
            lblNuevo = new Label();
            cboNuevo = new ComboBox();
            lblNota = new Label();
            lnkMatriz = new LinkLabel();
            btnAplicar = new Button();
            btnCancelar = new Button();
            SuspendLayout();
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            lblTitulo.ForeColor = Color.FromArgb(31, 41, 55);
            lblTitulo.Location = new Point(20, 14);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(147, 25);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Cambiar estado";
            lblActual.AutoSize = true;
            lblActual.ForeColor = Color.FromArgb(107, 114, 128);
            lblActual.Location = new Point(20, 48);
            lblActual.Name = "lblActual";
            lblActual.Size = new Size(85, 15);
            lblActual.TabIndex = 1;
            lblActual.Text = "Estado actual:";
            lblNuevo.AutoSize = true;
            lblNuevo.Location = new Point(20, 80);
            lblNuevo.Name = "lblNuevo";
            lblNuevo.Size = new Size(82, 15);
            lblNuevo.TabIndex = 2;
            lblNuevo.Text = "Nuevo estado:";
            cboNuevo.DropDownStyle = ComboBoxStyle.DropDownList;
            cboNuevo.Font = new Font("Segoe UI", 10F);
            cboNuevo.FormattingEnabled = true;
            cboNuevo.Location = new Point(20, 100);
            cboNuevo.Name = "cboNuevo";
            cboNuevo.Size = new Size(380, 25);
            cboNuevo.TabIndex = 3;
            lblNota.ForeColor = Color.FromArgb(107, 114, 128);
            lblNota.Location = new Point(20, 134);
            lblNota.Name = "lblNota";
            lblNota.Size = new Size(380, 50);
            lblNota.TabIndex = 4;
            lblNota.Text = "Solo aparecen los estados permitidos por la matriz de transiciones. \"Entregada\" abre la ventana de cobro y \"Cancelada\" pide confirmación.";
            lnkMatriz.AutoSize = true;
            lnkMatriz.LinkColor = Color.FromArgb(220, 38, 38);
            lnkMatriz.Location = new Point(20, 196);
            lnkMatriz.Name = "lnkMatriz";
            lnkMatriz.Size = new Size(152, 15);
            lnkMatriz.TabIndex = 5;
            lnkMatriz.TabStop = true;
            lnkMatriz.Text = "Ver matriz de transiciones";
            lnkMatriz.LinkClicked += lnkMatriz_LinkClicked;
            btnAplicar.BackColor = Color.FromArgb(220, 38, 38);
            btnAplicar.Cursor = Cursors.Hand;
            btnAplicar.FlatAppearance.BorderSize = 0;
            btnAplicar.FlatStyle = FlatStyle.Flat;
            btnAplicar.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnAplicar.ForeColor = Color.White;
            btnAplicar.Location = new Point(200, 222);
            btnAplicar.Name = "btnAplicar";
            btnAplicar.Size = new Size(100, 34);
            btnAplicar.TabIndex = 6;
            btnAplicar.Text = " Aplicar";
            btnAplicar.UseVisualStyleBackColor = false;
            btnAplicar.Click += btnAplicar_Click;
            btnCancelar.BackColor = Color.White;
            btnCancelar.Cursor = Cursors.Hand;
            btnCancelar.DialogResult = DialogResult.Cancel;
            btnCancelar.FlatAppearance.BorderColor = Color.FromArgb(209, 213, 219);
            btnCancelar.FlatStyle = FlatStyle.Flat;
            btnCancelar.ForeColor = Color.FromArgb(31, 41, 55);
            btnCancelar.Location = new Point(310, 222);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(90, 34);
            btnCancelar.TabIndex = 7;
            btnCancelar.Text = "Cancelar";
            btnCancelar.UseVisualStyleBackColor = false;
            AcceptButton = btnAplicar;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            CancelButton = btnCancelar;
            ClientSize = new Size(420, 272);
            Controls.Add(btnCancelar);
            Controls.Add(btnAplicar);
            Controls.Add(lnkMatriz);
            Controls.Add(lblNota);
            Controls.Add(cboNuevo);
            Controls.Add(lblNuevo);
            Controls.Add(lblActual);
            Controls.Add(lblTitulo);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmCambiarEstado";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Cambiar estado";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblTitulo;
        private Label lblActual;
        private Label lblNuevo;
        private ComboBox cboNuevo;
        private Label lblNota;
        private LinkLabel lnkMatriz;
        private Button btnAplicar;
        private Button btnCancelar;
    }
}
