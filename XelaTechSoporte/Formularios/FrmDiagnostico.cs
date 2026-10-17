using XelaTechSoporte.Excepciones;
using XelaTechSoporte.Modelos;
using XelaTechSoporte.Servicios;

namespace XelaTechSoporte.Formularios
{
    public partial class FrmDiagnostico : Form
    {
        private CentroServicio centro;
        private Solicitud solicitud;

        public FrmDiagnostico(CentroServicio centro, Solicitud solicitud)
        {
            InitializeComponent();
            this.centro = centro;
            this.solicitud = solicitud;
            UiHelper.PonerIcono(btnGuardar, UiHelper.ICONO_GUARDAR, Color.White);

            string tecnico = "-";
            if (solicitud.TecnicoAsignado != null)
            {
                tecnico = solicitud.TecnicoAsignado.NombreCompleto;
            }
            lblSolicitud.Text = solicitud.Codigo + "  ·  " + solicitud.Equipo + "  ·  Técnico: " + tecnico +
                                "\nProblema: " + solicitud.DescripcionProblema;

            string[] checklist = solicitud.Equipo.ObtenerChecklist();
            chkChecklist.Items.AddRange(checklist);
            txtHoras.Text = solicitud.TiempoEstimadoHoras.ToString();

            if (solicitud.Diagnostico != null)
            {
                lblTitulo.Text = "Actualizar diagnóstico";
                txtDescripcion.Text = solicitud.Diagnostico.Descripcion;
                List<string> acciones = solicitud.Diagnostico.Acciones;
                for (int i = 0; i < acciones.Count; i++)
                {
                    int posicion = chkChecklist.Items.IndexOf(acciones[i]);
                    if (posicion >= 0)
                    {
                        chkChecklist.SetItemChecked(posicion, true);
                    }
                    else
                    {
                        txtAcciones.AppendText(acciones[i] + Environment.NewLine);
                    }
                }
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                string descripcion = UiHelper.LeerTexto(txtDescripcion, "Descripción del diagnóstico");
                int horas = UiHelper.LeerEntero(txtHoras, "Tiempo estimado (horas)", 1, 200);

                List<string> acciones = new List<string>();
                for (int i = 0; i < chkChecklist.CheckedItems.Count; i++)
                {
                    acciones.Add(chkChecklist.CheckedItems[i].ToString());
                }
                string[] lineas = txtAcciones.Lines;
                for (int i = 0; i < lineas.Length; i++)
                {
                    if (lineas[i].Trim() != "")
                    {
                        acciones.Add(lineas[i].Trim());
                    }
                }

                centro.Solicitudes.RegistrarDiagnostico(solicitud, descripcion, acciones, horas);
                DialogResult = DialogResult.OK;
            }
            catch (ReglaNegocioException ex)
            {
                UiHelper.Aviso(ex.Message);
            }
        }
    }
}
