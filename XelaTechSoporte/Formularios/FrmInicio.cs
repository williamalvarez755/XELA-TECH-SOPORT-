using XelaTechSoporte.Modelos;
using XelaTechSoporte.Servicios;

namespace XelaTechSoporte.Formularios
{
    public partial class FrmInicio : Form
    {
        private CentroServicio centro;

        public FrmInicio(CentroServicio centro)
        {
            InitializeComponent();
            this.centro = centro;
            UiHelper.EstilizarRejilla(dgvRecientes);
            UiHelper.CrearColumnas(dgvRecientes, "Código", "Cliente", "Equipo", "Estado", "Prioridad", "Técnico", "Fecha");
        }

        private void FrmInicio_Load(object sender, EventArgs e)
        {
            MostrarResumen();
            MostrarRecientes();
        }

        private void MostrarResumen()
        {
            lblValor1.Text = centro.Clientes.Listar().Count.ToString();
            lblDetalle1.Text = "Equipos registrados: " + centro.Equipos.Listar().Count;

            List<Solicitud> solicitudes = centro.Solicitudes.Listar();
            int activas = 0;
            int entregadas = 0;
            int canceladas = 0;
            decimal ingresos = 0;
            for (int i = 0; i < solicitudes.Count; i++)
            {
                if (solicitudes[i].EstaActiva)
                {
                    activas++;
                }
                else if (solicitudes[i].Estado == EstadoSolicitud.Entregada)
                {
                    entregadas++;
                    ingresos += solicitudes[i].Total;
                }
                else
                {
                    canceladas++;
                }
            }
            lblValor2.Text = activas.ToString();
            lblDetalle2.Text = "Entregadas: " + entregadas + "   Canceladas: " + canceladas;

            lblValor3.Text = (centro.Cola.CantidadUrgentes + centro.Cola.CantidadNormales).ToString();
            lblDetalle3.Text = "Urgentes: " + centro.Cola.CantidadUrgentes + "   Normales: " + centro.Cola.CantidadNormales;

            List<Repuesto> alerta = centro.Repuestos.BajoMinimo();
            int agotados = 0;
            for (int i = 0; i < alerta.Count; i++)
            {
                if (alerta[i].EstaAgotado())
                {
                    agotados++;
                }
            }
            lblValor4.Text = alerta.Count.ToString();
            lblDetalle4.Text = "Agotados: " + agotados;

            lblIngresos.Text = "Ingresos por servicios finalizados: " + Textos.Moneda(ingresos);
        }

        private void MostrarRecientes()
        {
            dgvRecientes.Rows.Clear();
            List<Solicitud> lista = centro.Solicitudes.Listar();
            int mostradas = 0;

            for (int i = lista.Count - 1; i >= 0 && mostradas < 10; i--)
            {
                Solicitud s = lista[i];
                string tecnico = "-";
                if (s.TecnicoAsignado != null)
                {
                    tecnico = s.TecnicoAsignado.NombreCompleto;
                }

                int fila = dgvRecientes.Rows.Add(s.Codigo, s.Cliente.NombreCompleto, s.Equipo.Marca + " " + s.Equipo.Modelo,
                                                 Textos.NombreEstado(s.Estado), Textos.NombrePrioridad(s.Prioridad),
                                                 tecnico, s.FechaRecepcion.ToString("dd/MM/yyyy"));
                UiHelper.MarcarEtiqueta(dgvRecientes.Rows[fila].Cells[3], UiHelper.ColorEstado(s.Estado));
                UiHelper.MarcarEtiqueta(dgvRecientes.Rows[fila].Cells[4], UiHelper.ColorPrioridad(s.Prioridad));
                mostradas++;
            }
            dgvRecientes.ClearSelection();
        }
    }
}
