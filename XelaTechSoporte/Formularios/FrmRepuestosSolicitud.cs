using XelaTechSoporte.Excepciones;
using XelaTechSoporte.Modelos;
using XelaTechSoporte.Servicios;

namespace XelaTechSoporte.Formularios
{
    public partial class FrmRepuestosSolicitud : Form
    {
        private CentroServicio centro;
        private Solicitud solicitud;

        public FrmRepuestosSolicitud(CentroServicio centro, Solicitud solicitud)
        {
            InitializeComponent();
            this.centro = centro;
            this.solicitud = solicitud;

            UiHelper.PonerIcono(btnAgregar, UiHelper.ICONO_AGREGAR, Color.White);
            UiHelper.EstilizarRejilla(dgvDetalles);
            dgvDetalles.BorderStyle = BorderStyle.FixedSingle;
            UiHelper.CrearColumnas(dgvDetalles, "Código", "Repuesto", "Cantidad", "Precio aplicado", "Subtotal");

            lblSolicitud.Text = solicitud.Codigo + "  ·  " + solicitud.Cliente.NombreCompleto + "  ·  " + solicitud.Equipo +
                                "  ·  Estado: " + Textos.NombreEstado(solicitud.Estado);
            LlenarCombo();
            MostrarDetalles();
        }

        private void LlenarCombo()
        {
            int posicion = cboRepuesto.SelectedIndex;
            cboRepuesto.Items.Clear();
            List<Repuesto> repuestos = centro.Repuestos.Listar();
            for (int i = 0; i < repuestos.Count; i++)
            {
                cboRepuesto.Items.Add(repuestos[i]);
            }
            if (cboRepuesto.Items.Count == 0)
            {
                lblExistencia.Text = "No hay repuestos en el catálogo. Regístrelos en el módulo Repuestos.";
                btnAgregar.Enabled = false;
                return;
            }
            if (posicion < 0 || posicion >= cboRepuesto.Items.Count)
            {
                posicion = 0;
            }
            cboRepuesto.SelectedIndex = posicion;
        }

        private void MostrarDetalles()
        {
            dgvDetalles.Rows.Clear();
            for (int i = 0; i < solicitud.Repuestos.Count; i++)
            {
                DetalleRepuesto d = solicitud.Repuestos[i];
                dgvDetalles.Rows.Add(d.Repuesto.Codigo, d.Repuesto.Nombre, d.Cantidad,
                                     Textos.Moneda(d.PrecioAplicado), Textos.Moneda(d.Subtotal));
            }
            dgvDetalles.ClearSelection();
            lblTotal.Text = "Costo de repuestos: " + Textos.Moneda(solicitud.CostoRepuestos) +
                            "     Total de la solicitud: " + Textos.Moneda(solicitud.Total);
        }

        private void cboRepuesto_SelectedIndexChanged(object sender, EventArgs e)
        {
            Repuesto r = (Repuesto)cboRepuesto.SelectedItem;
            if (r == null)
            {
                return;
            }
            lblExistencia.Text = "Existencia: " + r.Existencia + "   |   Mínimo: " + r.ExistenciaMinima +
                                 "   |   Precio de venta: " + Textos.Moneda(r.PrecioVenta) + "   |   " + r.TextoEstado;
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                Repuesto repuesto = (Repuesto)cboRepuesto.SelectedItem;
                int cantidad = UiHelper.LeerEntero(txtCantidad, "Cantidad", 1, 1000);

                bool quedoEnAlerta = centro.Solicitudes.AgregarRepuesto(solicitud, repuesto, cantidad);

                LlenarCombo();
                MostrarDetalles();
                txtCantidad.Text = "1";

                if (quedoEnAlerta)
                {
                    UiHelper.Aviso("Atención: \"" + repuesto.Nombre + "\" quedó con " + repuesto.Existencia +
                                   " unidad(es) y su mínimo es " + repuesto.ExistenciaMinima + ". Hay que reabastecerlo.");
                }
            }
            catch (ReglaNegocioException ex)
            {
                UiHelper.Aviso(ex.Message);
            }
        }
    }
}
