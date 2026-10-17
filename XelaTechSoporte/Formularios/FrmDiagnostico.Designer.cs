namespace XelaTechSoporte.Formularios
{
    partial class FrmDiagnostico
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
            lblChecklist = new Label();
            chkChecklist = new CheckedListBox();
            lblDescripcion = new Label();
            txtDescripcion = new TextBox();
            lblAcciones = new Label();
            txtAcciones = new TextBox();
            lblHoras = new Label();
            txtHoras = new TextBox();
            btnGuardar = new Button();
            btnCancelar = new Button();
            SuspendLayout();
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            lblTitulo.ForeColor = Color.FromArgb(31, 41, 55);
            lblTitulo.Location = new Point(20, 14);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(197, 25);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Registrar diagnóstico";
            lblSolicitud.ForeColor = Color.FromArgb(107, 114, 128);
            lblSolicitud.Location = new Point(20, 44);
            lblSolicitud.Name = "lblSolicitud";
            lblSolicitud.Size = new Size(440, 40);
            lblSolicitud.TabIndex = 1;
            lblSolicitud.Text = "Solicitud";
            lblChecklist.AutoSize = true;
            lblChecklist.Location = new Point(20, 88);
            lblChecklist.Name = "lblChecklist";
            lblChecklist.Size = new Size(281, 15);
            lblChecklist.TabIndex = 2;
            lblChecklist.Text = "Checklist del equipo (marque lo que se realizó):";
            chkChecklist.BorderStyle = BorderStyle.FixedSingle;
            chkChecklist.CheckOnClick = true;
            chkChecklist.FormattingEnabled = true;
            chkChecklist.Location = new Point(20, 108);
            chkChecklist.Name = "chkChecklist";
            chkChecklist.Size = new Size(440, 110);
            chkChecklist.TabIndex = 3;
            lblDescripcion.AutoSize = true;
            lblDescripcion.Location = new Point(20, 228);
            lblDescripcion.Name = "lblDescripcion";
            lblDescripcion.Size = new Size(160, 15);
            lblDescripcion.TabIndex = 4;
            lblDescripcion.Text = "Descripción del diagnóstico:";
            txtDescripcion.BorderStyle = BorderStyle.FixedSingle;
            txtDescripcion.Location = new Point(20, 248);
            txtDescripcion.MaxLength = 300;
            txtDescripcion.Multiline = true;
            txtDescripcion.Name = "txtDescripcion";
            txtDescripcion.ScrollBars = ScrollBars.Vertical;
            txtDescripcion.Size = new Size(440, 54);
            txtDescripcion.TabIndex = 5;
            lblAcciones.AutoSize = true;
            lblAcciones.Location = new Point(20, 310);
            lblAcciones.Name = "lblAcciones";
            lblAcciones.Size = new Size(240, 15);
            lblAcciones.TabIndex = 6;
            lblAcciones.Text = "Otras acciones realizadas (una por línea):";
            txtAcciones.BorderStyle = BorderStyle.FixedSingle;
            txtAcciones.Location = new Point(20, 330);
            txtAcciones.Multiline = true;
            txtAcciones.Name = "txtAcciones";
            txtAcciones.ScrollBars = ScrollBars.Vertical;
            txtAcciones.Size = new Size(440, 54);
            txtAcciones.TabIndex = 7;
            lblHoras.AutoSize = true;
            lblHoras.Location = new Point(20, 400);
            lblHoras.Name = "lblHoras";
            lblHoras.Size = new Size(146, 15);
            lblHoras.TabIndex = 8;
            lblHoras.Text = "Tiempo estimado (horas):";
            txtHoras.BorderStyle = BorderStyle.FixedSingle;
            txtHoras.Location = new Point(180, 397);
            txtHoras.Name = "txtHoras";
            txtHoras.Size = new Size(80, 23);
            txtHoras.TabIndex = 9;
            btnGuardar.BackColor = Color.FromArgb(220, 38, 38);
            btnGuardar.Cursor = Cursors.Hand;
            btnGuardar.FlatAppearance.BorderSize = 0;
            btnGuardar.FlatStyle = FlatStyle.Flat;
            btnGuardar.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnGuardar.ForeColor = Color.White;
            btnGuardar.Location = new Point(240, 434);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.Size = new Size(110, 34);
            btnGuardar.TabIndex = 10;
            btnGuardar.Text = " Guardar";
            btnGuardar.UseVisualStyleBackColor = false;
            btnGuardar.Click += btnGuardar_Click;
            btnCancelar.BackColor = Color.White;
            btnCancelar.Cursor = Cursors.Hand;
            btnCancelar.DialogResult = DialogResult.Cancel;
            btnCancelar.FlatAppearance.BorderColor = Color.FromArgb(209, 213, 219);
            btnCancelar.FlatStyle = FlatStyle.Flat;
            btnCancelar.ForeColor = Color.FromArgb(31, 41, 55);
            btnCancelar.Location = new Point(360, 434);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(100, 34);
            btnCancelar.TabIndex = 11;
            btnCancelar.Text = "Cancelar";
            btnCancelar.UseVisualStyleBackColor = false;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            CancelButton = btnCancelar;
            ClientSize = new Size(480, 484);
            Controls.Add(btnCancelar);
            Controls.Add(btnGuardar);
            Controls.Add(txtHoras);
            Controls.Add(lblHoras);
            Controls.Add(txtAcciones);
            Controls.Add(lblAcciones);
            Controls.Add(txtDescripcion);
            Controls.Add(lblDescripcion);
            Controls.Add(chkChecklist);
            Controls.Add(lblChecklist);
            Controls.Add(lblSolicitud);
            Controls.Add(lblTitulo);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmDiagnostico";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Diagnóstico";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblTitulo;
        private Label lblSolicitud;
        private Label lblChecklist;
        private CheckedListBox chkChecklist;
        private Label lblDescripcion;
        private TextBox txtDescripcion;
        private Label lblAcciones;
        private TextBox txtAcciones;
        private Label lblHoras;
        private TextBox txtHoras;
        private Button btnGuardar;
        private Button btnCancelar;
    }
}
