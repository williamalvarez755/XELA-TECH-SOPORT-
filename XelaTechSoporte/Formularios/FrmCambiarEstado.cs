using XelaTechSoporte.Estructuras;
using XelaTechSoporte.Modelos;

namespace XelaTechSoporte.Formularios
{
    public partial class FrmCambiarEstado : Form
    {
        private List<EstadoSolicitud> posibles;

        public EstadoSolicitud EstadoElegido { get; private set; }

        public FrmCambiarEstado(Solicitud solicitud)
        {
            InitializeComponent();
            UiHelper.PonerIcono(btnAplicar, UiHelper.ICONO_CHECK, Color.White);

            lblActual.Text = "Solicitud " + solicitud.Codigo + "  ·  Estado actual: " + Textos.NombreEstado(solicitud.Estado);

            posibles = MaquinaEstados.EstadosPosibles(solicitud.Estado);
            for (int i = 0; i < posibles.Count; i++)
            {
                cboNuevo.Items.Add(Textos.NombreEstado(posibles[i]));
            }

            if (posibles.Count == 0)
            {
                lblNota.Text = "Esta solicitud está cerrada: su fila en la matriz de transiciones está vacía, no admite cambios.";
                btnAplicar.Enabled = false;
            }
            else
            {
                cboNuevo.SelectedIndex = 0;
            }
        }

        private void btnAplicar_Click(object sender, EventArgs e)
        {
            if (cboNuevo.SelectedIndex < 0)
            {
                UiHelper.Aviso("Seleccione el nuevo estado.");
                return;
            }
            EstadoElegido = posibles[cboNuevo.SelectedIndex];
            DialogResult = DialogResult.OK;
        }

        private void lnkMatriz_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBox.Show("Matriz de transiciones (fila = estado actual, columna = estado destino):\n\n" +
                            MaquinaEstados.MostrarMatriz(), "Matriz de transiciones",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
