using XelaTechSoporte.Algoritmos;
using XelaTechSoporte.Estructuras;
using XelaTechSoporte.Excepciones;
using XelaTechSoporte.Modelos;
using XelaTechSoporte.Servicios;

namespace XelaTechSoporte.Formularios
{
    public partial class FrmSolicitudes : Form
    {
        private CentroServicio centro;
        private Solicitud seleccionada = null;
        private bool cargando = false;

        public FrmSolicitudes(CentroServicio centro)
        {
            InitializeComponent();
            this.centro = centro;

            UiHelper.EstilizarRejilla(dgvSolicitudes);
            UiHelper.EstilizarRejilla(dgvCola);
            UiHelper.EstilizarRejilla(dgvCarga);
            UiHelper.CrearColumnas(dgvSolicitudes, "Código", "Cliente", "Equipo", "Estado", "Prioridad", "Técnico", "Fecha", "Total");
            UiHelper.CrearColumnas(dgvCola, "Código", "Cliente", "Prioridad", "Espera");
            UiHelper.CrearColumnas(dgvCarga, "Técnico", "Lun", "Mar", "Mié", "Jue", "Vie", "Sáb");
            dgvCarga.Columns[0].FillWeight = 260;

            cboFiltroEstado.Items.Add("Todos");
            cboFiltroEstado.Items.AddRange(Textos.Estados);
            cboFiltroPrioridad.Items.Add("Todas");
            cboFiltroPrioridad.Items.AddRange(Textos.Prioridades);
            cboPrioridad.Items.AddRange(Textos.Prioridades);
            cboEspecialidad.Items.AddRange(Textos.Especialidades);

            dtpFecha.MaxDate = DateTime.Today.AddDays(1).AddSeconds(-1);

            UiHelper.PonerIcono(btnNueva, UiHelper.ICONO_AGREGAR, Color.White);
            UiHelper.PonerIcono(btnBuscar, UiHelper.ICONO_BUSCAR, UiHelper.ColorTexto);
            UiHelper.PonerIcono(btnGuardar, UiHelper.ICONO_GUARDAR, Color.White);
            UiHelper.PonerIcono(btnCancelar, UiHelper.ICONO_CANCELAR, UiHelper.ColorTexto);
            UiHelper.PonerIcono(btnDiagnostico, UiHelper.ICONO_DIAGNOSTICO, UiHelper.ColorTexto);
            UiHelper.PonerIcono(btnRepuestos, UiHelper.ICONO_REPUESTOS, UiHelper.ColorTexto);
            UiHelper.PonerIcono(btnCambiarEstado, UiHelper.ICONO_CAMBIAR, UiHelper.ColorTexto);
            UiHelper.PonerIcono(btnHistorial, UiHelper.ICONO_HISTORIAL, UiHelper.ColorTexto);
            UiHelper.PonerIcono(btnDeshacer, UiHelper.ICONO_DESHACER, UiHelper.ColorTexto);
            UiHelper.PonerIcono(btnEntregar, UiHelper.ICONO_COBRAR, UiHelper.ColorTexto);
            UiHelper.PonerIcono(btnCancelarSolicitud, UiHelper.ICONO_CANCELAR, UiHelper.ColorRojo);
        }

        private void FrmSolicitudes_Load(object sender, EventArgs e)
        {
            cargando = true;
            List<Cliente> clientes = centro.Clientes.Listar();
            for (int i = 0; i < clientes.Count; i++)
            {
                cboCliente.Items.Add(clientes[i]);
            }
            cboFiltroEstado.SelectedIndex = 0;
            cboFiltroPrioridad.SelectedIndex = 0;
            cargando = false;

            AplicarFiltros();
            MostrarCola();
            MostrarCarga();
            PrepararNueva();
        }

        private void AplicarFiltros()
        {
            List<Solicitud> lista = centro.Solicitudes.Listar();

            if (txtBuscar.Text.Trim() != "")
            {
                lista = Busquedas.SecuencialSolicitudes(lista, txtBuscar.Text);
            }

            List<Solicitud> filtrada = new List<Solicitud>();
            for (int i = 0; i < lista.Count; i++)
            {
                bool pasaEstado = cboFiltroEstado.SelectedIndex <= 0 ||
                                  (int)lista[i].Estado == cboFiltroEstado.SelectedIndex - 1;
                bool pasaPrioridad = cboFiltroPrioridad.SelectedIndex <= 0 ||
                                     (int)lista[i].Prioridad == cboFiltroPrioridad.SelectedIndex - 1;
                if (pasaEstado && pasaPrioridad)
                {
                    filtrada.Add(lista[i]);
                }
            }
            MostrarSolicitudes(filtrada);
        }

        private void MostrarSolicitudes(List<Solicitud> lista)
        {
            dgvSolicitudes.Rows.Clear();
            for (int i = 0; i < lista.Count; i++)
            {
                Solicitud s = lista[i];
                string tecnico = "-";
                if (s.TecnicoAsignado != null)
                {
                    tecnico = s.TecnicoAsignado.NombreCompleto;
                }
                int fila = dgvSolicitudes.Rows.Add(s.Codigo, s.Cliente.NombreCompleto, s.Equipo.Marca + " " + s.Equipo.Modelo,
                                                   Textos.NombreEstado(s.Estado), Textos.NombrePrioridad(s.Prioridad),
                                                   tecnico, s.FechaRecepcion.ToString("dd/MM/yyyy"), Textos.Moneda(s.Total));
                dgvSolicitudes.Rows[fila].Tag = s;
                UiHelper.MarcarEtiqueta(dgvSolicitudes.Rows[fila].Cells[3], UiHelper.ColorEstado(s.Estado));
                UiHelper.MarcarEtiqueta(dgvSolicitudes.Rows[fila].Cells[4], UiHelper.ColorPrioridad(s.Prioridad));

                if (s == seleccionada)
                {
                    dgvSolicitudes.Rows[fila].Selected = true;
                }
            }
            if (seleccionada == null)
            {
                dgvSolicitudes.ClearSelection();
            }
        }

        private void MostrarCola()
        {
            dgvCola.Rows.Clear();
            Solicitud[] urgentes = centro.Cola.VerEnEspera(NivelPrioridad.Urgente);
            Solicitud[] normales = centro.Cola.VerEnEspera(NivelPrioridad.Normal);

            for (int i = 0; i < urgentes.Length; i++)
            {
                AgregarFilaCola(urgentes[i]);
            }
            for (int i = 0; i < normales.Length; i++)
            {
                AgregarFilaCola(normales[i]);
            }
            dgvCola.ClearSelection();
            lblCola.Text = "Cola de atención  (" + urgentes.Length + " urgentes, " + normales.Length + " normales)";
        }

        private void AgregarFilaCola(Solicitud s)
        {
            int fila = dgvCola.Rows.Add(s.Codigo, s.Cliente.NombreCompleto, Textos.NombrePrioridad(s.Prioridad),
                                        GeneradorReportes.TextoEspera(s.FechaRecepcion));
            UiHelper.MarcarEtiqueta(dgvCola.Rows[fila].Cells[2], UiHelper.ColorPrioridad(s.Prioridad));
        }

        private void MostrarCarga()
        {
            dgvCarga.Rows.Clear();
            List<Tecnico> tecnicos = centro.Tecnicos.Listar();
            MatrizCarga matriz = centro.Matriz;

            for (int f = 0; f < matriz.Filas; f++)
            {
                int fila = dgvCarga.Rows.Add(tecnicos[f].NombreCompleto);
                for (int d = 0; d < MatrizCarga.DIAS; d++)
                {
                    int valor = matriz.Valor(f, d);
                    DataGridViewCell celda = dgvCarga.Rows[fila].Cells[d + 1];
                    celda.Value = valor;
                    if (valor >= Tecnico.LIMITE_TRABAJOS)
                    {
                        celda.Style.ForeColor = UiHelper.ColorRojo;
                        celda.Style.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
                    }
                }
            }
            dgvCarga.ClearSelection();
        }

        private void HabilitarEdicion(bool habilitar)
        {
            cboCliente.Enabled = habilitar;
            cboEquipo.Enabled = habilitar;
            cboPrioridad.Enabled = habilitar;
            cboEspecialidad.Enabled = habilitar;
            dtpFecha.Enabled = habilitar;
            txtDescripcion.ReadOnly = !habilitar;
            btnGuardar.Enabled = habilitar;
        }

        private void HabilitarAcciones(bool habilitar)
        {
            btnDiagnostico.Enabled = habilitar;
            btnRepuestos.Enabled = habilitar;
            btnCambiarEstado.Enabled = habilitar;
            btnHistorial.Enabled = habilitar;
            btnEntregar.Enabled = habilitar;
            btnCancelarSolicitud.Enabled = habilitar;
            btnDeshacer.Enabled = habilitar && seleccionada != null && seleccionada.PuedeDeshacer;
        }

        private void PrepararNueva()
        {
            seleccionada = null;
            HabilitarEdicion(true);
            HabilitarAcciones(false);
            lblModo.Text = "Nueva solicitud";
            txtCodigo.Text = "(automático)";
            txtEstado.Text = "Recibida";
            txtTecnico.Text = "-";
            cboPrioridad.SelectedIndex = 0;
            dtpFecha.Value = DateTime.Now;
            txtDescripcion.Clear();
            txtTotal.Clear();

            if (cboCliente.Items.Count > 0)
            {
                cboCliente.SelectedIndex = 0;
                LlenarEquipos((Cliente)cboCliente.SelectedItem);
            }
            else
            {
                lblModo.Text = "Primero registre clientes y equipos";
            }
            dgvSolicitudes.ClearSelection();
        }

        private void LlenarEquipos(Cliente cliente)
        {
            cboEquipo.Items.Clear();
            txtTotal.Clear();
            if (cliente == null)
            {
                return;
            }
            List<Equipo> equipos = centro.Equipos.EquiposDeCliente(cliente);
            for (int i = 0; i < equipos.Count; i++)
            {
                cboEquipo.Items.Add(equipos[i]);
            }
            if (cboEquipo.Items.Count > 0)
            {
                cboEquipo.SelectedIndex = 0;
            }
            else
            {
                txtTotal.Text = "El cliente no tiene equipos registrados";
            }
        }

        private void cboCliente_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cargando || seleccionada != null)
            {
                return;
            }
            LlenarEquipos((Cliente)cboCliente.SelectedItem);
        }

        private void cboEquipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cargando || seleccionada != null)
            {
                return;
            }
            Equipo equipo = (Equipo)cboEquipo.SelectedItem;
            Cliente cliente = (Cliente)cboCliente.SelectedItem;
            if (equipo == null || cliente == null)
            {
                return;
            }
            cboEspecialidad.SelectedIndex = (int)equipo.EspecialidadSugerida();

            decimal revision = equipo.CalcularCostoRevision();
            decimal descuento = cliente.ObtenerDescuento();
            txtTotal.Text = "Revisión: " + Textos.Moneda(revision - revision * descuento) +
                            "  (descuento " + (descuento * 100).ToString("0") + " %)";
        }

        private void CargarSolicitud(Solicitud s)
        {
            seleccionada = s;
            cargando = true;

            cboCliente.SelectedItem = s.Cliente;
            cboEquipo.Items.Clear();
            cboEquipo.Items.Add(s.Equipo);
            cboEquipo.SelectedIndex = 0;
            cboPrioridad.SelectedIndex = (int)s.Prioridad;
            cboEspecialidad.SelectedIndex = (int)s.EspecialidadRequerida;
            dtpFecha.Value = s.FechaRecepcion;
            txtDescripcion.Text = s.DescripcionProblema;

            cargando = false;

            lblModo.Text = "Consultando " + s.Codigo;
            txtCodigo.Text = s.Codigo;
            txtEstado.Text = Textos.NombreEstado(s.Estado);
            txtTecnico.Text = "-";
            if (s.TecnicoAsignado != null)
            {
                txtTecnico.Text = s.TecnicoAsignado.NombreCompleto;
            }
            txtTotal.Text = Textos.Moneda(s.Total) + "  (M.O. " + Textos.Moneda(s.CostoManoObra) +
                            " + rep. " + Textos.Moneda(s.CostoRepuestos) + ")";

            HabilitarEdicion(false);
            HabilitarAcciones(true);
        }

        private void dgvSolicitudes_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }
            CargarSolicitud((Solicitud)dgvSolicitudes.Rows[e.RowIndex].Tag);
        }

        private void Refrescar()
        {
            AplicarFiltros();
            MostrarCola();
            MostrarCarga();
            if (seleccionada != null)
            {
                CargarSolicitud(seleccionada);
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                Cliente cliente = (Cliente)cboCliente.SelectedItem;
                Equipo equipo = (Equipo)cboEquipo.SelectedItem;
                NivelPrioridad prioridad = (NivelPrioridad)cboPrioridad.SelectedIndex;
                Especialidad especialidad = (Especialidad)cboEspecialidad.SelectedIndex;

                DateTime fecha = dtpFecha.Value.Date.Add(DateTime.Now.TimeOfDay);

                Solicitud nueva = centro.Solicitudes.Crear(cliente, equipo, txtDescripcion.Text, prioridad, especialidad, fecha);

                int posicion = centro.Cola.PosicionEnCola(nueva);
                UiHelper.Info("Solicitud " + nueva.Codigo + " creada.\n\n" +
                              "Estado: " + Textos.NombreEstado(nueva.Estado) + "\n" +
                              "Quedó en la cola " + Textos.NombrePrioridad(nueva.Prioridad).ToLower() + " en el turno " + posicion + ".\n" +
                              "Costo de revisión: " + Textos.Moneda(nueva.CostoManoObra));
                PrepararNueva();
                Refrescar();
            }
            catch (ReglaNegocioException ex)
            {
                UiHelper.Aviso(ex.Message);
            }
        }

        private void btnNueva_Click(object sender, EventArgs e)
        {
            PrepararNueva();
            txtDescripcion.Focus();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            PrepararNueva();
        }

        private bool HaySeleccion()
        {
            if (seleccionada == null)
            {
                UiHelper.Aviso("Seleccione una solicitud de la lista.");
                return false;
            }
            return true;
        }

        private void btnDiagnostico_Click(object sender, EventArgs e)
        {
            if (!HaySeleccion())
            {
                return;
            }
            try
            {
                centro.Solicitudes.ValidarDiagnostico(seleccionada);
                FrmDiagnostico ventana = new FrmDiagnostico(centro, seleccionada);
                if (ventana.ShowDialog() == DialogResult.OK)
                {
                    Refrescar();
                    UiHelper.Info("Diagnóstico registrado. Estado actual: " + Textos.NombreEstado(seleccionada.Estado) + ".");
                }
                ventana.Dispose();
            }
            catch (ReglaNegocioException ex)
            {
                UiHelper.Aviso(ex.Message);
            }
        }

        private void btnRepuestos_Click(object sender, EventArgs e)
        {
            if (!HaySeleccion())
            {
                return;
            }
            try
            {
                centro.Solicitudes.ValidarRepuestos(seleccionada);
                FrmRepuestosSolicitud ventana = new FrmRepuestosSolicitud(centro, seleccionada);
                ventana.ShowDialog();
                ventana.Dispose();
                Refrescar();
            }
            catch (ReglaNegocioException ex)
            {
                UiHelper.Aviso(ex.Message);
            }
        }

        private void btnCambiarEstado_Click(object sender, EventArgs e)
        {
            if (!HaySeleccion())
            {
                return;
            }
            try
            {
                centro.Solicitudes.ValidarModificable(seleccionada);

                FrmCambiarEstado ventana = new FrmCambiarEstado(seleccionada);
                DialogResult resultado = ventana.ShowDialog();
                EstadoSolicitud nuevo = ventana.EstadoElegido;
                ventana.Dispose();
                if (resultado != DialogResult.OK)
                {
                    return;
                }

                if (nuevo == EstadoSolicitud.Entregada)
                {
                    AbrirEntrega();
                }
                else if (nuevo == EstadoSolicitud.Cancelada)
                {
                    CancelarSeleccionada();
                }
                else
                {
                    centro.Solicitudes.CambiarEstado(seleccionada, nuevo);
                    Refrescar();
                    UiHelper.Info("La solicitud " + seleccionada.Codigo + " ahora está \"" + Textos.NombreEstado(seleccionada.Estado) + "\".");
                }
            }
            catch (ReglaNegocioException ex)
            {
                UiHelper.Aviso(ex.Message);
                Refrescar();
            }
        }

        private void btnHistorial_Click(object sender, EventArgs e)
        {
            if (!HaySeleccion())
            {
                return;
            }
            Solicitud s = seleccionada;
            EstadoSolicitud[] pila = s.ObtenerHistorial();

            string texto = "Solicitud " + s.Codigo + "\n";
            texto += "Estado actual: " + Textos.NombreEstado(s.Estado) + "\n\n";
            texto += "Pila de estados anteriores (" + pila.Length + "), el primero es el tope:\n";
            if (pila.Length == 0)
            {
                texto += "   (vacía)\n";
            }
            for (int i = 0; i < pila.Length; i++)
            {
                texto += "   " + (i + 1) + ". " + Textos.NombreEstado(pila[i]) + "\n";
            }

            texto += "\nDiagnóstico: ";
            if (s.Diagnostico == null)
            {
                texto += "sin registrar\n";
            }
            else
            {
                texto += s.Diagnostico.ToString() + "\n";
            }

            texto += "\nRepuestos utilizados:\n";
            if (s.Repuestos.Count == 0)
            {
                texto += "   (ninguno)\n";
            }
            for (int i = 0; i < s.Repuestos.Count; i++)
            {
                texto += "   - " + s.Repuestos[i].ToString() + "\n";
            }
            texto += "\nTotal: " + Textos.Moneda(s.Total);
            UiHelper.Info(texto);
        }

        private void btnDeshacer_Click(object sender, EventArgs e)
        {
            if (!HaySeleccion())
            {
                return;
            }
            try
            {
                EstadoSolicitud anterior = seleccionada.VerEstadoAnterior();
                bool confirmar = UiHelper.Confirmar("¿Deshacer el último cambio de la solicitud " + seleccionada.Codigo + "?\n\n" +
                                                    "Pasará de \"" + Textos.NombreEstado(seleccionada.Estado) + "\" a \"" +
                                                    Textos.NombreEstado(anterior) + "\".");
                if (!confirmar)
                {
                    return;
                }
                EstadoSolicitud quedo = centro.Solicitudes.Deshacer(seleccionada);
                Refrescar();
                UiHelper.Info("Cambio deshecho. La solicitud quedó en \"" + Textos.NombreEstado(quedo) + "\".");
            }
            catch (ReglaNegocioException ex)
            {
                UiHelper.Aviso(ex.Message);
            }
        }

        private void btnEntregar_Click(object sender, EventArgs e)
        {
            if (!HaySeleccion())
            {
                return;
            }
            try
            {
                AbrirEntrega();
            }
            catch (ReglaNegocioException ex)
            {
                UiHelper.Aviso(ex.Message);
            }
        }

        private void AbrirEntrega()
        {
            centro.Solicitudes.ValidarEntrega(seleccionada);
            FrmEntrega ventana = new FrmEntrega(centro, seleccionada);
            ventana.ShowDialog();
            ventana.Dispose();
            Refrescar();
        }

        private void btnCancelarSolicitud_Click(object sender, EventArgs e)
        {
            if (!HaySeleccion())
            {
                return;
            }
            try
            {
                CancelarSeleccionada();
            }
            catch (ReglaNegocioException ex)
            {
                UiHelper.Aviso(ex.Message);
            }
        }

        private void CancelarSeleccionada()
        {
            centro.Solicitudes.ValidarModificable(seleccionada);
            bool confirmar = UiHelper.Confirmar("¿Cancelar la solicitud " + seleccionada.Codigo + "?\n\n" +
                                                "Los repuestos utilizados regresarán al inventario. Esta acción no se puede deshacer.");
            if (!confirmar)
            {
                return;
            }
            int repuestos = seleccionada.Repuestos.Count;
            centro.Solicitudes.Cancelar(seleccionada);
            Refrescar();
            UiHelper.Info("Solicitud " + seleccionada.Codigo + " cancelada. Se devolvieron " + repuestos + " línea(s) de repuestos al inventario.");
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            AplicarFiltros();
            if (txtBuscar.Text.Trim() != "")
            {
                centro.RegistroBusquedas.Add(new ResultadoBusqueda("Secuencial", "Solicitudes (código/cliente/equipo): " + txtBuscar.Text.Trim(),
                                                                   dgvSolicitudes.Rows.Count, Busquedas.UltimasComparaciones,
                                                                   dgvSolicitudes.Rows.Count + " solicitud(es)"));
                if (dgvSolicitudes.Rows.Count == 0)
                {
                    UiHelper.Aviso("No se encontró ninguna solicitud con \"" + txtBuscar.Text.Trim() + "\" y los filtros elegidos.");
                }
            }
        }

        private void cboFiltro_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!cargando)
            {
                AplicarFiltros();
            }
        }

        private void txtBuscar_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                btnBuscar_Click(sender, e);
            }
        }
    }
}
