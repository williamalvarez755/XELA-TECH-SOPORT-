using XelaTechSoporte.Excepciones;
using XelaTechSoporte.Modelos;
using XelaTechSoporte.Servicios;

namespace XelaTechSoporte.Formularios
{
    public partial class FrmRepuestos : Form
    {
        private CentroServicio centro;
        private Repuesto seleccionado = null;

        public FrmRepuestos(CentroServicio centro)
        {
            InitializeComponent();
            this.centro = centro;

            UiHelper.EstilizarRejilla(dgvRepuestos);
            UiHelper.CrearColumnas(dgvRepuestos, "Código", "Nombre", "Categoría", "Existencia", "Mínimo",
                                   "Costo", "Precio", "Utilizados", "Estado");
            dgvRepuestos.Columns[1].FillWeight = 220;
            dgvRepuestos.Columns[8].FillWeight = 130;
            cboCategoria.Items.AddRange(Textos.Categorias);

            UiHelper.PonerIcono(btnNuevo, UiHelper.ICONO_AGREGAR, Color.White);
            UiHelper.PonerIcono(btnGuardar, UiHelper.ICONO_GUARDAR, Color.White);
            UiHelper.PonerIcono(btnCancelar, UiHelper.ICONO_CANCELAR, UiHelper.ColorTexto);
            UiHelper.PonerIcono(btnBuscarCodigo, UiHelper.ICONO_BUSCAR, UiHelper.ColorTexto);
            UiHelper.PonerIcono(btnReabastecer, UiHelper.ICONO_AGREGAR, UiHelper.ColorTexto);
        }

        private void FrmRepuestos_Load(object sender, EventArgs e)
        {
            MostrarSegunFiltro();
            PrepararNuevo();
        }

        private void MostrarSegunFiltro()
        {
            if (chkSoloAlerta.Checked)
            {
                MostrarRepuestos(centro.Repuestos.BajoMinimo());
            }
            else
            {
                MostrarRepuestos(centro.Repuestos.Listar());
            }
        }

        private void MostrarRepuestos(List<Repuesto> lista)
        {
            dgvRepuestos.Rows.Clear();
            for (int i = 0; i < lista.Count; i++)
            {
                Repuesto r = lista[i];
                int fila = dgvRepuestos.Rows.Add(r.Codigo, r.Nombre, Textos.NombreCategoria(r.Categoria), r.Existencia,
                                                 r.ExistenciaMinima, Textos.Moneda(r.CostoUnitario), Textos.Moneda(r.PrecioVenta),
                                                 r.CantidadUtilizada, r.TextoEstado);
                dgvRepuestos.Rows[fila].Tag = r;
                UiHelper.MarcarEtiqueta(dgvRepuestos.Rows[fila].Cells[8], ColorEstadoRepuesto(r));
            }
            dgvRepuestos.ClearSelection();
            lblConteo.Text = lista.Count + " repuesto(s) mostrados de " + centro.Repuestos.Listar().Count + " en el catálogo";
            lblAlertas.Text = centro.Repuestos.BajoMinimo().Count + " repuesto(s) en alerta";
        }

        private Color ColorEstadoRepuesto(Repuesto r)
        {
            if (r.EstaAgotado())
            {
                return UiHelper.ColorRojo;
            }
            if (r.EnAlerta())
            {
                return UiHelper.EtiquetaAmarilla;
            }
            return UiHelper.EtiquetaVerde;
        }

        private void PrepararNuevo()
        {
            seleccionado = null;
            lblModo.Text = "Nuevo repuesto";
            lblExistencia.Text = "Existencia inicial:";
            txtCodigo.Text = "(automático)";
            cboCategoria.SelectedIndex = 0;
            txtNombre.Clear();
            txtExistencia.Text = "0";
            txtExistencia.ReadOnly = false;
            txtMinimo.Text = "3";
            txtCosto.Clear();
            txtPrecio.Clear();
            lblInfo.Text = "";
            txtReabastecer.Clear();
            btnReabastecer.Enabled = false;
            txtReabastecer.Enabled = false;
            dgvRepuestos.ClearSelection();
            txtNombre.Focus();
        }

        private void CargarRepuesto(Repuesto r)
        {
            seleccionado = r;
            lblModo.Text = "Editando " + r.Codigo;
            lblExistencia.Text = "Existencia actual:";
            txtCodigo.Text = r.Codigo;
            cboCategoria.SelectedIndex = (int)r.Categoria;
            txtNombre.Text = r.Nombre;
            txtExistencia.Text = r.Existencia.ToString();
            txtExistencia.ReadOnly = true;
            txtMinimo.Text = r.ExistenciaMinima.ToString();
            txtCosto.Text = r.CostoUnitario.ToString("0.00");
            txtPrecio.Text = r.PrecioVenta.ToString("0.00");
            lblInfo.Text = "Margen por unidad: " + Textos.Moneda(r.Margen) +
                           "\nUnidades utilizadas en solicitudes: " + r.CantidadUtilizada +
                           "\nEstado del inventario: " + r.TextoEstado;
            btnReabastecer.Enabled = true;
            txtReabastecer.Enabled = true;
        }

        private void dgvRepuestos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }
            CargarRepuesto((Repuesto)dgvRepuestos.Rows[e.RowIndex].Tag);
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                string nombre = UiHelper.LeerTexto(txtNombre, "Nombre");
                CategoriaRepuesto categoria = (CategoriaRepuesto)cboCategoria.SelectedIndex;
                int minimo = UiHelper.LeerEntero(txtMinimo, "Existencia mínima", 0, 10000);
                decimal costo = UiHelper.LeerDecimal(txtCosto, "Costo unitario", 0);
                decimal precio = UiHelper.LeerDecimal(txtPrecio, "Precio de venta", 0);

                if (seleccionado == null)
                {
                    int existencia = UiHelper.LeerEntero(txtExistencia, "Existencia inicial", 0, 10000);
                    Repuesto nuevo = centro.Repuestos.Registrar(nombre, categoria, existencia, minimo, costo, precio);
                    MostrarSegunFiltro();
                    string mensaje = "Repuesto registrado con el código " + nuevo.Codigo + ".";
                    if (nuevo.EnAlerta())
                    {
                        mensaje += "\n\nAtención: la existencia (" + nuevo.Existencia + ") ya está en o por debajo del mínimo (" + nuevo.ExistenciaMinima + ").";
                    }
                    UiHelper.Info(mensaje);
                    PrepararNuevo();
                }
                else
                {
                    centro.Repuestos.Actualizar(seleccionado.Codigo, nombre, categoria, minimo, costo, precio);
                    MostrarSegunFiltro();
                    UiHelper.Info("Repuesto " + seleccionado.Codigo + " actualizado.");
                    CargarRepuesto(seleccionado);
                }
            }
            catch (ReglaNegocioException ex)
            {
                UiHelper.Aviso(ex.Message);
            }
        }

        private void btnReabastecer_Click(object sender, EventArgs e)
        {
            if (seleccionado == null)
            {
                UiHelper.Aviso("Seleccione un repuesto de la lista.");
                return;
            }
            try
            {
                int cantidad = UiHelper.LeerEntero(txtReabastecer, "Cantidad a reabastecer", 1, 1000);
                centro.Repuestos.Reabastecer(seleccionado.Codigo, cantidad);
                MostrarSegunFiltro();
                CargarRepuesto(seleccionado);
                txtReabastecer.Clear();
                UiHelper.Info("Se agregaron " + cantidad + " unidades. Existencia actual: " + seleccionado.Existencia + ".");
            }
            catch (ReglaNegocioException ex)
            {
                UiHelper.Aviso(ex.Message);
            }
        }

        private void btnBuscarCodigo_Click(object sender, EventArgs e)
        {
            if (txtBuscarCodigo.Text.Trim() == "")
            {
                UiHelper.Aviso("Escriba el código del repuesto. Ejemplo: REP-0001");
                txtBuscarCodigo.Focus();
                return;
            }
            Repuesto repuesto = centro.Repuestos.BuscarPorCodigo(txtBuscarCodigo.Text);
            if (repuesto == null)
            {
                UiHelper.Aviso("No existe un repuesto con el código \"" + txtBuscarCodigo.Text.Trim().ToUpper() + "\".");
                return;
            }
            List<Repuesto> uno = new List<Repuesto>();
            uno.Add(repuesto);
            MostrarRepuestos(uno);
            CargarRepuesto(repuesto);
        }

        private void chkSoloAlerta_CheckedChanged(object sender, EventArgs e)
        {
            MostrarSegunFiltro();
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            PrepararNuevo();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            PrepararNuevo();
        }
    }
}
