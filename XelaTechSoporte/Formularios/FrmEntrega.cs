using XelaTechSoporte.Excepciones;
using XelaTechSoporte.Modelos;
using XelaTechSoporte.Servicios;

namespace XelaTechSoporte.Formularios
{
    public partial class FrmEntrega : Form
    {
        private CentroServicio centro;
        private Solicitud solicitud;

        public FrmEntrega(CentroServicio centro, Solicitud solicitud)
        {
            InitializeComponent();
            this.centro = centro;
            this.solicitud = solicitud;
            UiHelper.PonerIcono(btnEntregar, UiHelper.ICONO_COBRAR, Color.White);

            lblSolicitud.Text = solicitud.Codigo + "  ·  " + solicitud.Cliente.NombreCompleto +
                                "\n" + solicitud.Equipo + "  ·  Técnico: " + solicitud.TecnicoAsignado.NombreCompleto;
            lblRepuestosValor.Text = Textos.Moneda(solicitud.CostoRepuestos);
            txtManoObra.Text = solicitud.CostoManoObra.ToString("0.00");
            cboCalificacion.SelectedIndex = 0;
        }

        private void txtManoObra_TextChanged(object sender, EventArgs e)
        {
            decimal manoObra;
            if (decimal.TryParse(txtManoObra.Text.Trim(), out manoObra) && manoObra >= 0)
            {
                lblTotalValor.Text = Textos.Moneda(manoObra + solicitud.CostoRepuestos);
            }
            else
            {
                lblTotalValor.Text = "—";
            }
        }

        private void btnEntregar_Click(object sender, EventArgs e)
        {
            try
            {
                decimal manoObra = UiHelper.LeerDecimal(txtManoObra, "Mano de obra", 0);
                int calificacion = cboCalificacion.SelectedIndex;

                decimal total = centro.Solicitudes.Entregar(solicitud, manoObra, calificacion);

                string comprobante = "COMPROBANTE DE ENTREGA - XELATECH SOPORTE\n\n";
                comprobante += "Solicitud: " + solicitud.Codigo + "\n";
                comprobante += "Fecha: " + solicitud.FechaCierre.ToString("dd/MM/yyyy HH:mm") + "\n";
                comprobante += "Cliente: " + solicitud.Cliente.NombreCompleto + "\n";
                comprobante += "Equipo: " + solicitud.Equipo + "\n";
                comprobante += "Técnico: " + solicitud.TecnicoAsignado.NombreCompleto + "\n\n";
                comprobante += "Mano de obra: " + Textos.Moneda(solicitud.CostoManoObra) + "\n";
                comprobante += "Repuestos: " + Textos.Moneda(solicitud.CostoRepuestos) + "\n";
                for (int i = 0; i < solicitud.Repuestos.Count; i++)
                {
                    comprobante += "    - " + solicitud.Repuestos[i].ToString() + "\n";
                }
                comprobante += "\nTOTAL: " + Textos.Moneda(total);

                UiHelper.Info(comprobante);
                DialogResult = DialogResult.OK;
            }
            catch (ReglaNegocioException ex)
            {
                UiHelper.Aviso(ex.Message);
            }
        }
    }
}
