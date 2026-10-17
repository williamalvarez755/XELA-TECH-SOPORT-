using XelaTechSoporte.Algoritmos;
using XelaTechSoporte.Excepciones;
using XelaTechSoporte.Modelos;
using XelaTechSoporte.Servicios;

namespace XelaTechSoporte.Formularios
{
    public partial class FrmEquipos : Form
    {
        private CentroServicio centro;
        private Equipo seleccionado = null;

        public FrmEquipos(CentroServicio centro)
        {
            InitializeComponent();
            this.centro = centro;

            UiHelper.EstilizarRejilla(dgvEquipos);
            UiHelper.CrearColumnas(dgvEquipos, "Código", "Tipo", "Marca", "Modelo", "Serie", "Propietario", "Revisión");

            cboTipo.Items.AddRange(Textos.TiposEquipo);
            cboGabinete.Items.AddRange(Escritorio.TiposGabinete);

            UiHelper.PonerIcono(btnNuevo, UiHelper.ICONO_AGREGAR, Color.White);
            UiHelper.PonerIcono(btnGuardar, UiHelper.ICONO_GUARDAR, Color.White);
            UiHelper.PonerIcono(btnCancelar, UiHelper.ICONO_CANCELAR, UiHelper.ColorTexto);
            UiHelper.PonerIcono(btnBuscarMarca, UiHelper.ICONO_BUSCAR, UiHelper.ColorTexto);
            UiHelper.PonerIcono(btnHistorial, UiHelper.ICONO_HISTORIAL, UiHelper.ColorTexto);
            UiHelper.PonerIcono(btnChecklist, UiHelper.ICONO_DIAGNOSTICO, UiHelper.ColorTexto);
        }

        private void FrmEquipos_Load(object sender, EventArgs e)
        {
            List<Cliente> clientes = centro.Clientes.Listar();
            for (int i = 0; i < clientes.Count; i++)
            {
                cboPropietario.Items.Add(clientes[i]);
            }

            MostrarEquipos(centro.Equipos.Listar());
            PrepararNuevo();
        }

        private void MostrarEquipos(List<Equipo> lista)
        {
            dgvEquipos.Rows.Clear();
            for (int i = 0; i < lista.Count; i++)
            {
                Equipo eq = lista[i];
                int fila = dgvEquipos.Rows.Add(eq.CodigoInterno, Textos.NombreTipoEquipo(eq.Tipo), eq.Marca, eq.Modelo,
                                               eq.NumeroSerie, eq.Propietario.NombreCompleto,
                                               Textos.Moneda(eq.CalcularCostoRevision()));
                dgvEquipos.Rows[fila].Tag = eq;
            }
            dgvEquipos.ClearSelection();
            lblConteo.Text = lista.Count + " equipo(s) mostrados de " + centro.Equipos.Listar().Count + " registrados";

            int[] porTipo = centro.Equipos.ContarPorTipo();
            lblPorTipo.Text = "Laptops: " + porTipo[0] + "   Escritorios: " + porTipo[1] + "   Impresoras: " + porTipo[2];
        }

        private void cboTipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            panelLaptop.Visible = cboTipo.SelectedIndex == (int)TipoEquipo.Laptop;
            panelEscritorio.Visible = cboTipo.SelectedIndex == (int)TipoEquipo.Escritorio;
            panelImpresora.Visible = cboTipo.SelectedIndex == (int)TipoEquipo.Impresora;
        }

        private void HabilitarCampos(bool habilitar)
        {
            cboTipo.Enabled = habilitar;
            cboPropietario.Enabled = habilitar;
            txtSerie.ReadOnly = !habilitar;
            txtMarca.ReadOnly = !habilitar;
            txtModelo.ReadOnly = !habilitar;
            txtEstadoFisico.ReadOnly = !habilitar;
            txtAccesorios.ReadOnly = !habilitar;
            panelLaptop.Enabled = habilitar;
            panelEscritorio.Enabled = habilitar;
            panelImpresora.Enabled = habilitar;
            btnGuardar.Enabled = habilitar;
            btnHistorial.Enabled = !habilitar;
            btnChecklist.Enabled = !habilitar;
        }

        private void PrepararNuevo()
        {
            seleccionado = null;
            HabilitarCampos(true);
            lblModo.Text = "Nuevo equipo";
            if (cboPropietario.Items.Count == 0)
            {
                lblModo.Text = "Primero registre al menos un cliente";
            }
            txtCodigo.Text = "(automático)";
            cboTipo.SelectedIndex = 0;
            if (cboPropietario.Items.Count > 0)
            {
                cboPropietario.SelectedIndex = 0;
            }
            txtSerie.Clear();
            txtMarca.Clear();
            txtModelo.Clear();
            txtEstadoFisico.Text = "Buen estado";
            txtAccesorios.Clear();
            chkBateria.Checked = true;
            txtPulgadas.Text = "15.6";
            txtDiscos.Text = "1";
            cboGabinete.SelectedIndex = 0;
            chkLaser.Checked = false;
            txtContador.Text = "0";
            lblCosto.Text = "";
            dgvEquipos.ClearSelection();
        }

        private void CargarEquipo(Equipo eq)
        {
            seleccionado = eq;
            lblModo.Text = "Consultando " + eq.CodigoInterno;
            txtCodigo.Text = eq.CodigoInterno;
            cboTipo.SelectedIndex = (int)eq.Tipo;
            cboPropietario.SelectedItem = eq.Propietario;
            txtSerie.Text = eq.NumeroSerie;
            txtMarca.Text = eq.Marca;
            txtModelo.Text = eq.Modelo;
            txtEstadoFisico.Text = eq.EstadoFisicoRecibido;
            txtAccesorios.Text = eq.AccesoriosTexto;

            if (eq is Laptop)
            {
                Laptop laptop = (Laptop)eq;
                chkBateria.Checked = laptop.TieneBateria;
                txtPulgadas.Text = laptop.PulgadasPantalla.ToString();
            }
            else if (eq is Escritorio)
            {
                Escritorio escritorio = (Escritorio)eq;
                txtDiscos.Text = escritorio.CantidadDiscos.ToString();
                cboGabinete.SelectedItem = escritorio.TipoGabinete;
            }
            else if (eq is Impresora)
            {
                Impresora impresora = (Impresora)eq;
                chkLaser.Checked = impresora.EsLaser;
                txtContador.Text = impresora.ContadorPaginas.ToString();
            }

            lblCosto.Text = "Costo de revisión: " + Textos.Moneda(eq.CalcularCostoRevision()) + "\n" + eq.ObtenerDetalle();
            HabilitarCampos(false);
        }

        private void dgvEquipos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }
            CargarEquipo((Equipo)dgvEquipos.Rows[e.RowIndex].Tag);
        }

        private List<string> LeerAccesorios()
        {
            List<string> lista = new List<string>();
            string[] partes = txtAccesorios.Text.Split(',');
            for (int i = 0; i < partes.Length; i++)
            {
                if (partes[i].Trim() != "")
                {
                    lista.Add(partes[i].Trim());
                }
            }
            return lista;
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                Cliente propietario = (Cliente)cboPropietario.SelectedItem;
                if (propietario == null)
                {
                    throw new ReglaNegocioException("Seleccione el cliente propietario. Si no hay clientes, regístrelos primero.");
                }
                string serie = UiHelper.LeerTexto(txtSerie, "Número de serie");
                string marca = UiHelper.LeerTexto(txtMarca, "Marca");
                string modelo = UiHelper.LeerTexto(txtModelo, "Modelo");
                string estadoFisico = UiHelper.LeerTexto(txtEstadoFisico, "Estado físico recibido");
                List<string> accesorios = LeerAccesorios();

                Equipo nuevo;
                TipoEquipo tipo = (TipoEquipo)cboTipo.SelectedIndex;
                if (tipo == TipoEquipo.Laptop)
                {
                    double pulgadas = (double)UiHelper.LeerDecimal(txtPulgadas, "Pantalla (pulgadas)", 0);
                    nuevo = centro.Equipos.RegistrarLaptop(propietario, serie, marca, modelo, estadoFisico, accesorios,
                                                           chkBateria.Checked, pulgadas);
                }
                else if (tipo == TipoEquipo.Escritorio)
                {
                    int discos = UiHelper.LeerEntero(txtDiscos, "Cantidad de discos", 1, 8);
                    nuevo = centro.Equipos.RegistrarEscritorio(propietario, serie, marca, modelo, estadoFisico, accesorios,
                                                               discos, (string)cboGabinete.SelectedItem);
                }
                else
                {
                    int contador = UiHelper.LeerEntero(txtContador, "Contador de páginas", 0, 10000000);
                    nuevo = centro.Equipos.RegistrarImpresora(propietario, serie, marca, modelo, estadoFisico, accesorios,
                                                              chkLaser.Checked, contador);
                }

                MostrarEquipos(centro.Equipos.Listar());
                UiHelper.Info("Equipo registrado con el código " + nuevo.CodigoInterno + ".\n" +
                              "Costo de revisión (" + Textos.NombreTipoEquipo(nuevo.Tipo) + "): " +
                              Textos.Moneda(nuevo.CalcularCostoRevision()));
                PrepararNuevo();
            }
            catch (ReglaNegocioException ex)
            {
                UiHelper.Aviso(ex.Message);
            }
        }

        private void btnHistorial_Click(object sender, EventArgs e)
        {
            if (seleccionado == null)
            {
                return;
            }
            List<Solicitud> historial = centro.Equipos.HistorialDeEquipo(seleccionado.CodigoInterno);
            if (historial.Count == 0)
            {
                UiHelper.Info("El equipo " + seleccionado.CodigoInterno + " todavía no tiene solicitudes.");
                return;
            }
            string texto = "Historial del equipo " + seleccionado + ":\n\n";
            for (int i = 0; i < historial.Count; i++)
            {
                Solicitud s = historial[i];
                texto += s.Codigo + "  |  " + s.FechaRecepcion.ToString("dd/MM/yyyy") + "  |  " +
                         Textos.NombreEstado(s.Estado) + "  |  " + Textos.Moneda(s.Total) + "\n";
            }
            UiHelper.Info(texto);
        }

        private void btnChecklist_Click(object sender, EventArgs e)
        {
            if (seleccionado == null)
            {
                return;
            }
            string[] pasos = seleccionado.ObtenerChecklist();
            string texto = "Checklist de revisión para " + Textos.NombreTipoEquipo(seleccionado.Tipo) + ":\n\n";
            for (int i = 0; i < pasos.Length; i++)
            {
                texto += (i + 1) + ". " + pasos[i] + "\n";
            }
            UiHelper.Info(texto);
        }

        private void btnBuscarMarca_Click(object sender, EventArgs e)
        {
            try
            {
                List<Equipo> resultados = centro.Equipos.BuscarPorMarca(txtBuscar.Text);
                int comparaciones = Busquedas.UltimasComparaciones;
                MostrarEquipos(resultados);
                lblConteo.Text = resultados.Count + " coincidencia(s) para la marca \"" + txtBuscar.Text.Trim() +
                                 "\"  (búsqueda secuencial, " + comparaciones + " comparaciones)";
                centro.RegistroBusquedas.Add(new ResultadoBusqueda("Secuencial", "Equipos por marca: " + txtBuscar.Text.Trim(),
                                                                   resultados.Count, comparaciones, resultados.Count + " equipo(s)"));
                if (resultados.Count == 0)
                {
                    UiHelper.Aviso("No se encontró ningún equipo de la marca \"" + txtBuscar.Text.Trim() + "\".");
                }
            }
            catch (ReglaNegocioException ex)
            {
                UiHelper.Aviso(ex.Message);
            }
        }

        private void btnVerTodos_Click(object sender, EventArgs e)
        {
            txtBuscar.Clear();
            MostrarEquipos(centro.Equipos.Listar());
        }

        private void txtBuscar_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                btnBuscarMarca_Click(sender, e);
            }
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
