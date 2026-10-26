using XelaTechSoporte.Algoritmos;
using XelaTechSoporte.Servicios;

namespace XelaTechSoporte.Formularios
{
    public partial class FrmReportes : Form
    {
        private CentroServicio centro;
        private GeneradorReportes generador;
        private bool cargando = false;

        public FrmReportes(CentroServicio centro)
        {
            InitializeComponent();
            this.centro = centro;
            generador = new GeneradorReportes(centro);

            UiHelper.EstilizarRejilla(dgvReporte);
            UiHelper.PonerIcono(btnVerTodos, UiHelper.ICONO_IMPRIMIR, Color.White);
        }

        private void FrmReportes_Load(object sender, EventArgs e)
        {
            cargando = true;
            lstReportes.Items.AddRange(GeneradorReportes.Nombres);
            cboCriterio7.SelectedIndex = 0;
            cargando = false;
            lstReportes.SelectedIndex = 0;
        }

        private bool Descendente()
        {
            return rdoDescendente.Checked;
        }

        private CriterioSolicitud CriterioReporte7()
        {
            if (cboCriterio7.SelectedIndex == 1)
            {
                return CriterioSolicitud.TiempoEstimado;
            }
            return CriterioSolicitud.FechaRecepcion;
        }

        private void MostrarReporte()
        {
            if (lstReportes.SelectedIndex < 0)
            {
                return;
            }
            int numero = lstReportes.SelectedIndex + 1;
            TablaReporte tabla = generador.Generar(numero, Descendente(), CriterioReporte7());

            txtTodos.Visible = false;
            dgvReporte.Visible = true;
            lblTituloReporte.Text = tabla.Titulo;
            lblDescripcion.Text = tabla.Descripcion;
            UiHelper.MostrarTabla(dgvReporte, tabla);
            lblResumen.Text = tabla.Resumen;
            if (tabla.Filas.Count == 0 && tabla.Resumen == "")
            {
                lblResumen.Text = "Sin datos para este reporte.";
            }
        }

        private void lstReportes_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0)
            {
                return;
            }
            bool seleccionado = (e.State & DrawItemState.Selected) == DrawItemState.Selected;
            Color fondo = lstReportes.BackColor;
            Color texto = UiHelper.ColorTexto;
            if (seleccionado)
            {
                fondo = UiHelper.ColorSeleccion;
                texto = UiHelper.ColorRojo;
            }

            using (SolidBrush pincel = new SolidBrush(fondo))
            {
                e.Graphics.FillRectangle(pincel, e.Bounds);
            }
            Rectangle area = new Rectangle(e.Bounds.X + 8, e.Bounds.Y, e.Bounds.Width - 8, e.Bounds.Height);
            TextRenderer.DrawText(e.Graphics, lstReportes.Items[e.Index].ToString(), lstReportes.Font, area, texto,
                                  TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);
        }

        private void lstReportes_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!cargando)
            {
                MostrarReporte();
            }
        }

        private void opciones_Changed(object sender, EventArgs e)
        {
            if (cargando)
            {
                return;
            }
            if (txtTodos.Visible)
            {
                btnVerTodos_Click(sender, e);
            }
            else
            {
                MostrarReporte();
            }
        }

        private void btnVerTodos_Click(object sender, EventArgs e)
        {
            txtTodos.Text = generador.TodosComoTexto(Descendente(), CriterioReporte7());
            dgvReporte.Visible = false;
            txtTodos.Visible = true;
            lblTituloReporte.Text = "Los 12 reportes en secuencia";
            lblDescripcion.Text = "Puede seleccionar el texto y copiarlo (Ctrl + C) para pegarlo en un documento.";
            lblResumen.Text = "";
            lstReportes.ClearSelected();
        }
    }
}
