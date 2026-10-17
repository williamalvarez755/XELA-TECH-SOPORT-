using XelaTechSoporte.Algoritmos;
using XelaTechSoporte.Excepciones;
using XelaTechSoporte.Modelos;
using XelaTechSoporte.Servicios;

namespace XelaTechSoporte.Formularios
{
    public partial class FrmBusquedas : Form
    {
        private CentroServicio centro;
        private bool cargando = false;

        private List<Solicitud> listaTrabajo;

        public FrmBusquedas(CentroServicio centro)
        {
            InitializeComponent();
            this.centro = centro;

            UiHelper.EstilizarRejilla(dgvSecuencial);
            UiHelper.EstilizarRejilla(dgvBinaria);
            UiHelper.EstilizarRejilla(dgvMulti);
            UiHelper.CrearColumnas(dgvBinaria, "Posición", "Código", "Cliente", "Estado", "Total");
            UiHelper.CrearColumnas(dgvMulti, "Código", "Cliente", "Estado", "Prioridad", "Técnico", "Fecha de recepción", "Total");

            UiHelper.PonerIcono(btnBuscarSecuencial, UiHelper.ICONO_BUSCAR, Color.White);
            UiHelper.PonerIcono(btnBuscarBinaria, UiHelper.ICONO_BUSCAR, Color.White);
            UiHelper.PonerIcono(btnBuscarMulti, UiHelper.ICONO_BUSCAR, Color.White);
            UiHelper.PonerIcono(btnOrdenarCodigo, UiHelper.ICONO_ORDENAR, UiHelper.ColorTexto);
        }

        private void FrmBusquedas_Load(object sender, EventArgs e)
        {
            cargando = true;
            cboTipoSecuencial.SelectedIndex = 0;

            cboEstado.Items.AddRange(Textos.Estados);
            cboEstado.SelectedIndex = 0;
            cboPrioridad.Items.AddRange(Textos.Prioridades);
            cboPrioridad.SelectedIndex = 0;
            cboTecnico.Items.Add("(Cualquiera)");
            List<Tecnico> tecnicos = centro.Tecnicos.Listar();
            for (int i = 0; i < tecnicos.Count; i++)
            {
                cboTecnico.Items.Add(tecnicos[i]);
            }
            cboTecnico.SelectedIndex = 0;
            dtpDesde.Value = DateTime.Today.AddDays(-7);
            dtpHasta.Value = DateTime.Today;
            cargando = false;

            PrepararListaBinaria();
        }

        private void btnBuscarSecuencial_Click(object sender, EventArgs e)
        {
            string texto = txtSecuencial.Text;
            try
            {
                int coincidencias;
                string tipo = cboTipoSecuencial.Text;

                if (cboTipoSecuencial.SelectedIndex == 0)
                {
                    List<Cliente> resultado = Busquedas.SecuencialClientesPorNombre(centro.Clientes.Listar(), texto);
                    UiHelper.CrearColumnas(dgvSecuencial, "Código", "Nombre completo", "Teléfono", "Correo", "Tipo");
                    for (int i = 0; i < resultado.Count; i++)
                    {
                        Cliente c = resultado[i];
                        dgvSecuencial.Rows.Add(c.Codigo, c.NombreCompleto, c.Telefono, c.Correo, Textos.NombreTipoCliente(c.Tipo));
                    }
                    coincidencias = resultado.Count;
                }
                else if (cboTipoSecuencial.SelectedIndex == 1)
                {
                    List<Solicitud> resultado = Busquedas.SecuencialSolicitudesPorEstado(centro.Solicitudes.Listar(), texto);
                    UiHelper.CrearColumnas(dgvSecuencial, "Código", "Cliente", "Equipo", "Estado", "Prioridad", "Total");
                    for (int i = 0; i < resultado.Count; i++)
                    {
                        Solicitud s = resultado[i];
                        int fila = dgvSecuencial.Rows.Add(s.Codigo, s.Cliente.NombreCompleto, s.Equipo.Marca + " " + s.Equipo.Modelo,
                                                          Textos.NombreEstado(s.Estado), Textos.NombrePrioridad(s.Prioridad),
                                                          Textos.Moneda(s.Total));
                        UiHelper.MarcarEtiqueta(dgvSecuencial.Rows[fila].Cells[3], UiHelper.ColorEstado(s.Estado));
                    }
                    coincidencias = resultado.Count;
                }
                else
                {
                    List<Equipo> resultado = Busquedas.SecuencialEquiposPorMarca(centro.Equipos.Listar(), texto);
                    UiHelper.CrearColumnas(dgvSecuencial, "Código", "Tipo", "Marca", "Modelo", "Serie", "Propietario");
                    for (int i = 0; i < resultado.Count; i++)
                    {
                        Equipo eq = resultado[i];
                        dgvSecuencial.Rows.Add(eq.CodigoInterno, Textos.NombreTipoEquipo(eq.Tipo), eq.Marca, eq.Modelo,
                                               eq.NumeroSerie, eq.Propietario.NombreCompleto);
                    }
                    coincidencias = resultado.Count;
                }
                dgvSecuencial.ClearSelection();

                int comparaciones = Busquedas.UltimasComparaciones;
                lblResultadoSecuencial.Text = coincidencias + " coincidencia(s) para \"" + texto.Trim() + "\" en " + tipo.ToLower() +
                                              "   |   Comparaciones: " + comparaciones + " (se recorrió toda la lista)";
                centro.RegistroBusquedas.Add(new ResultadoBusqueda("Secuencial", tipo + ": " + texto.Trim(), coincidencias,
                                                                   comparaciones, coincidencias + " encontrada(s)"));
                if (coincidencias == 0)
                {
                    UiHelper.Aviso("No se encontró ningún resultado para \"" + texto.Trim() + "\".");
                }
            }
            catch (ReglaNegocioException ex)
            {
                UiHelper.Aviso(ex.Message);
            }
        }

        private void txtSecuencial_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                btnBuscarSecuencial_Click(sender, e);
            }
        }

        private void PrepararListaBinaria()
        {
            List<Solicitud> ultima = centro.UltimaListaOrdenada;
            if (ultima != null && ultima.Count == centro.Solicitudes.Listar().Count)
            {
                listaTrabajo = Ordenamientos.Copiar(ultima);
                lblLista.Text = "Lista de trabajo: " + listaTrabajo.Count + " solicitudes en el orden del último ordenamiento (" +
                                centro.DescripcionUltimoOrden + ").";
            }
            else
            {
                listaTrabajo = Ordenamientos.Copiar(centro.Solicitudes.Listar());
                lblLista.Text = "Lista de trabajo: " + listaTrabajo.Count + " solicitudes en el orden en que se registraron.";
            }
            MostrarListaBinaria(-1);
        }

        private void MostrarListaBinaria(int posicionEncontrada)
        {
            dgvBinaria.Rows.Clear();
            for (int i = 0; i < listaTrabajo.Count; i++)
            {
                Solicitud s = listaTrabajo[i];
                int fila = dgvBinaria.Rows.Add(i, s.Codigo, s.Cliente.NombreCompleto, Textos.NombreEstado(s.Estado), Textos.Moneda(s.Total));
                if (i == posicionEncontrada)
                {
                    dgvBinaria.Rows[fila].DefaultCellStyle.BackColor = UiHelper.EtiquetaVerde;
                    dgvBinaria.Rows[fila].DefaultCellStyle.ForeColor = Color.White;
                    dgvBinaria.Rows[fila].DefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
                }
            }
            dgvBinaria.ClearSelection();
            if (posicionEncontrada >= 0 && dgvBinaria.DisplayedRowCount(false) > 0)
            {
                dgvBinaria.FirstDisplayedScrollingRowIndex = posicionEncontrada;
            }

            if (Busquedas.EstaOrdenadaPorCodigo(listaTrabajo))
            {
                lblEstadoOrden.Text = "¿Ordenada por código?  SÍ — se puede usar la búsqueda binaria.";
                lblEstadoOrden.ForeColor = UiHelper.EtiquetaVerde;
            }
            else
            {
                lblEstadoOrden.Text = "¿Ordenada por código?  NO — antes de buscar hay que ordenarla por código.";
                lblEstadoOrden.ForeColor = UiHelper.ColorRojo;
            }
        }

        private void OrdenarListaPorCodigo()
        {
            if (listaTrabajo.Count >= 2)
            {
                Ordenamientos.QuickSortSolicitudes(listaTrabajo, CriterioSolicitud.Codigo, false);
            }
            lblLista.Text = "Lista de trabajo: " + listaTrabajo.Count + " solicitudes ordenadas por código con Quick Sort (ascendente).";
            MostrarListaBinaria(-1);
        }

        private void btnOrdenarCodigo_Click(object sender, EventArgs e)
        {
            OrdenarListaPorCodigo();
        }

        private void btnBuscarBinaria_Click(object sender, EventArgs e)
        {
            string codigo = txtCodigoBinaria.Text.Trim();
            lstPasos.Items.Clear();
            try
            {
                if (listaTrabajo.Count == 0)
                {
                    throw new ReglaNegocioException("No hay solicitudes registradas para buscar.");
                }
                if (codigo == "")
                {
                    txtCodigoBinaria.Focus();
                    throw new ReglaNegocioException("Escriba el código de la solicitud. Ejemplo: SOL-0003");
                }

                if (!Busquedas.EstaOrdenadaPorCodigo(listaTrabajo))
                {
                    UiHelper.Aviso("La lista de trabajo NO está ordenada por código.\n\n" +
                                   "La búsqueda binaria solo funciona si los datos están ordenados por el mismo campo " +
                                   "que se compara, así que primero se ordenará con Quick Sort por código.");
                    OrdenarListaPorCodigo();
                }

                List<string> pasos = new List<string>();
                int posicion = Busquedas.Binaria(listaTrabajo, codigo, pasos);
                int comparaciones = Busquedas.UltimasComparaciones;

                for (int i = 0; i < pasos.Count; i++)
                {
                    lstPasos.Items.Add(pasos[i]);
                }
                MostrarListaBinaria(posicion);

                if (posicion >= 0)
                {
                    Solicitud s = listaTrabajo[posicion];
                    lblResultadoBinaria.Text = "Encontrada en la posición " + posicion + ": " + s.Codigo + " - " + s.Cliente.NombreCompleto +
                                               " - " + Textos.NombreEstado(s.Estado) + "   |   Comparaciones: " + comparaciones +
                                               " (de " + listaTrabajo.Count + " registros)";
                    centro.RegistroBusquedas.Add(new ResultadoBusqueda("Binaria", "Código " + codigo.ToUpper(), 1, comparaciones,
                                                                       "Posición " + posicion));
                }
                else
                {
                    lblResultadoBinaria.Text = "El código " + codigo.ToUpper() + " no está registrado   |   Comparaciones: " + comparaciones;
                    centro.RegistroBusquedas.Add(new ResultadoBusqueda("Binaria", "Código " + codigo.ToUpper(), 0, comparaciones,
                                                                       "No existe"));
                    UiHelper.Aviso("No existe una solicitud con el código " + codigo.ToUpper() + ".");
                }
            }
            catch (ReglaNegocioException ex)
            {
                UiHelper.Aviso(ex.Message);
            }
        }

        private void txtCodigoBinaria_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                btnBuscarBinaria_Click(sender, e);
            }
        }

        private void btnBuscarMulti_Click(object sender, EventArgs e)
        {
            try
            {
                FiltroSolicitud filtro = new FiltroSolicitud();
                filtro.UsarEstado = chkEstado.Checked;
                filtro.Estado = (EstadoSolicitud)cboEstado.SelectedIndex;
                filtro.UsarPrioridad = chkPrioridad.Checked;
                filtro.Prioridad = (NivelPrioridad)cboPrioridad.SelectedIndex;
                if (cboTecnico.SelectedIndex > 0)
                {
                    filtro.Tecnico = (Tecnico)cboTecnico.SelectedItem;
                }
                filtro.UsarFechas = chkFechas.Checked;
                filtro.Desde = dtpDesde.Value;
                filtro.Hasta = dtpHasta.Value;

                List<Solicitud> resultado = Busquedas.Multicriterio(centro.Solicitudes.Listar(), filtro);
                int comparaciones = Busquedas.UltimasComparaciones;

                dgvMulti.Rows.Clear();
                for (int i = 0; i < resultado.Count; i++)
                {
                    Solicitud s = resultado[i];
                    string tecnico = "-";
                    if (s.TecnicoAsignado != null)
                    {
                        tecnico = s.TecnicoAsignado.NombreCompleto;
                    }
                    int fila = dgvMulti.Rows.Add(s.Codigo, s.Cliente.NombreCompleto, Textos.NombreEstado(s.Estado),
                                                 Textos.NombrePrioridad(s.Prioridad), tecnico,
                                                 s.FechaRecepcion.ToString("dd/MM/yyyy HH:mm"), Textos.Moneda(s.Total));
                    UiHelper.MarcarEtiqueta(dgvMulti.Rows[fila].Cells[2], UiHelper.ColorEstado(s.Estado));
                    UiHelper.MarcarEtiqueta(dgvMulti.Rows[fila].Cells[3], UiHelper.ColorPrioridad(s.Prioridad));
                }
                dgvMulti.ClearSelection();

                lblResultadoMulti.Text = resultado.Count + " solicitud(es) cumplen: " + filtro.Describir() +
                                         "   |   Comparaciones: " + comparaciones;
                centro.RegistroBusquedas.Add(new ResultadoBusqueda("Multicriterio", filtro.Describir(), resultado.Count,
                                                                   comparaciones, resultado.Count + " encontrada(s)"));
                if (resultado.Count == 0)
                {
                    UiHelper.Aviso("Ninguna solicitud cumple todos los criterios indicados.");
                }
            }
            catch (ReglaNegocioException ex)
            {
                UiHelper.Aviso(ex.Message);
            }
        }

        private void cboEstado_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!cargando)
            {
                chkEstado.Checked = true;
            }
        }

        private void cboPrioridad_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!cargando)
            {
                chkPrioridad.Checked = true;
            }
        }

        private void dtpFechas_ValueChanged(object sender, EventArgs e)
        {
            if (!cargando)
            {
                chkFechas.Checked = true;
            }
        }
    }
}
