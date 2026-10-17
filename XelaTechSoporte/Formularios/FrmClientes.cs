using XelaTechSoporte.Algoritmos;
using XelaTechSoporte.Excepciones;
using XelaTechSoporte.Modelos;
using XelaTechSoporte.Servicios;

namespace XelaTechSoporte.Formularios
{
    public partial class FrmClientes : Form
    {
        private CentroServicio centro;

        private Cliente seleccionado = null;

        public FrmClientes(CentroServicio centro)
        {
            InitializeComponent();
            this.centro = centro;

            UiHelper.EstilizarRejilla(dgvClientes);
            UiHelper.CrearColumnas(dgvClientes, "Código", "Nombre completo", "Teléfono", "Correo", "Tipo", "Solicitudes");
            cboTipo.Items.AddRange(Textos.TiposCliente);

            UiHelper.PonerIcono(btnNuevo, UiHelper.ICONO_AGREGAR, Color.White);
            UiHelper.PonerIcono(btnGuardar, UiHelper.ICONO_GUARDAR, Color.White);
            UiHelper.PonerIcono(btnCancelar, UiHelper.ICONO_CANCELAR, UiHelper.ColorTexto);
            UiHelper.PonerIcono(btnBuscarNombre, UiHelper.ICONO_BUSCAR, UiHelper.ColorTexto);
            UiHelper.PonerIcono(btnBuscarCodigo, UiHelper.ICONO_BUSCAR, UiHelper.ColorTexto);
        }

        private void FrmClientes_Load(object sender, EventArgs e)
        {
            MostrarClientes(centro.Clientes.Listar());
            PrepararNuevo();
        }

        private void MostrarClientes(List<Cliente> lista)
        {
            dgvClientes.Rows.Clear();
            for (int i = 0; i < lista.Count; i++)
            {
                Cliente c = lista[i];
                int fila = dgvClientes.Rows.Add(c.Codigo, c.NombreCompleto, c.Telefono, c.Correo,
                                                Textos.NombreTipoCliente(c.Tipo), c.CantidadSolicitudes);
                dgvClientes.Rows[fila].Tag = c;
            }
            dgvClientes.ClearSelection();
            lblConteo.Text = lista.Count + " cliente(s) mostrados de " + centro.Clientes.Listar().Count + " registrados";
        }

        private void PrepararNuevo()
        {
            seleccionado = null;
            lblModo.Text = "Nuevo cliente";
            txtCodigo.Text = "(se genera al guardar)";
            txtNombre.Clear();
            txtTelefono.Clear();
            txtCorreo.Clear();
            cboTipo.SelectedIndex = 0;
            txtSolicitudes.Text = "0";
            lblInfo.Text = "";
            dgvClientes.ClearSelection();
            txtNombre.Focus();
        }

        private void CargarCliente(Cliente cliente)
        {
            seleccionado = cliente;
            lblModo.Text = "Editando " + cliente.Codigo;
            txtCodigo.Text = cliente.Codigo;
            txtNombre.Text = cliente.NombreCompleto;
            txtTelefono.Text = cliente.Telefono;
            txtCorreo.Text = cliente.Correo;
            cboTipo.SelectedIndex = (int)cliente.Tipo;
            txtSolicitudes.Text = cliente.CantidadSolicitudes.ToString();
            MostrarInfo();
        }

        private void MostrarInfo()
        {
            if (seleccionado == null)
            {
                lblInfo.Text = "";
                return;
            }
            decimal descuento = seleccionado.ObtenerDescuento() * 100;
            lblInfo.Text = "Descuento en revisión: " + descuento.ToString("0") + " %\n" + seleccionado.ObtenerResumen();
        }

        private void dgvClientes_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }
            Cliente cliente = (Cliente)dgvClientes.Rows[e.RowIndex].Tag;
            CargarCliente(cliente);
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                string nombre = UiHelper.LeerTexto(txtNombre, "Nombre completo");
                string telefono = UiHelper.LeerTexto(txtTelefono, "Teléfono");
                string correo = UiHelper.LeerTexto(txtCorreo, "Correo electrónico");
                TipoCliente tipo = (TipoCliente)cboTipo.SelectedIndex;

                if (seleccionado == null)
                {
                    Cliente nuevo = centro.Clientes.Registrar(nombre, telefono, correo, tipo);
                    MostrarClientes(centro.Clientes.Listar());
                    UiHelper.Info("Cliente registrado con el código " + nuevo.Codigo + ".");
                    PrepararNuevo();
                }
                else
                {
                    centro.Clientes.Actualizar(seleccionado.Codigo, nombre, telefono, correo, tipo);
                    MostrarClientes(centro.Clientes.Listar());
                    UiHelper.Info("Datos del cliente " + seleccionado.Codigo + " actualizados.");
                    CargarCliente(seleccionado);
                }
            }
            catch (ReglaNegocioException ex)
            {
                UiHelper.Aviso(ex.Message);
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            PrepararNuevo();
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            PrepararNuevo();
        }

        private void cboTipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (seleccionado != null)
            {
                MostrarInfo();
            }
        }

        private void btnBuscarNombre_Click(object sender, EventArgs e)
        {
            try
            {
                List<Cliente> resultados = centro.Clientes.BuscarPorNombre(txtBuscar.Text);
                int comparaciones = Busquedas.UltimasComparaciones;
                MostrarClientes(resultados);
                lblConteo.Text = resultados.Count + " coincidencia(s) para \"" + txtBuscar.Text.Trim() +
                                 "\"  (búsqueda secuencial, " + comparaciones + " comparaciones)";

                centro.RegistroBusquedas.Add(new ResultadoBusqueda("Secuencial", "Clientes por nombre: " + txtBuscar.Text.Trim(),
                                                                   resultados.Count, comparaciones, resultados.Count + " cliente(s)"));
                if (resultados.Count == 0)
                {
                    UiHelper.Aviso("No se encontró ningún cliente cuyo nombre contenga \"" + txtBuscar.Text.Trim() + "\".");
                }
            }
            catch (ReglaNegocioException ex)
            {
                UiHelper.Aviso(ex.Message);
            }
        }

        private void btnBuscarCodigo_Click(object sender, EventArgs e)
        {
            if (txtBuscar.Text.Trim() == "")
            {
                UiHelper.Aviso("Escriba el código del cliente. Ejemplo: CLI-0001");
                txtBuscar.Focus();
                return;
            }
            Cliente cliente = centro.Clientes.BuscarPorCodigo(txtBuscar.Text);
            if (cliente == null)
            {
                UiHelper.Aviso("No existe un cliente con el código \"" + txtBuscar.Text.Trim().ToUpper() + "\".");
                return;
            }
            List<Cliente> uno = new List<Cliente>();
            uno.Add(cliente);
            MostrarClientes(uno);
            CargarCliente(cliente);
        }

        private void btnVerTodos_Click(object sender, EventArgs e)
        {
            txtBuscar.Clear();
            MostrarClientes(centro.Clientes.Listar());
        }

        private void txtBuscar_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                btnBuscarNombre_Click(sender, e);
            }
        }
    }
}
