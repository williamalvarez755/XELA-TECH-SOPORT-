using XelaTechSoporte.Algoritmos;
using XelaTechSoporte.Excepciones;
using XelaTechSoporte.Modelos;
using XelaTechSoporte.Servicios;

namespace XelaTechSoporte.Formularios
{
    public partial class FrmOrdenamientos : Form
    {
        private CentroServicio centro;
        private bool cargando = false;

        public FrmOrdenamientos(CentroServicio centro)
        {
            InitializeComponent();
            this.centro = centro;
            UiHelper.EstilizarRejilla(dgvResultado);
            UiHelper.PonerIcono(btnOrdenar, UiHelper.ICONO_ORDENAR, Color.White);
        }

        private void FrmOrdenamientos_Load(object sender, EventArgs e)
        {
            cargando = true;
            cboCriterioQuick.SelectedIndex = 0;
            cargando = false;
        }

        private void cboCriterioQuick_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!cargando)
            {
                rdoQuick.Checked = true;
            }
        }

        private void btnOrdenar_Click(object sender, EventArgs e)
        {
            bool descendente = rdoDescendente.Checked;
            string sentido = "Ascendente";
            if (descendente)
            {
                sentido = "Descendente";
            }

            try
            {
                if (rdoBubble.Checked)
                {
                    List<Solicitud> copia = Ordenamientos.Copiar(centro.Solicitudes.Listar());
                    Ordenamientos.BubbleSortPorCosto(copia, descendente);
                    MostrarSolicitudes(copia, 6);
                    MostrarResumen("Bubble Sort", "Costo total de la solicitud", sentido, copia.Count);
                    centro.GuardarUltimoOrden(copia, "Bubble Sort por costo total (" + sentido.ToLower() + ")");
                }
                else if (rdoSelection.Checked)
                {
                    List<Tecnico> copia = Ordenamientos.Copiar(centro.Tecnicos.Listar());
                    Ordenamientos.SelectionSortPorServicios(copia, descendente);
                    MostrarTecnicos(copia);
                    MostrarResumen("Selection Sort", "Servicios finalizados del técnico", sentido, copia.Count);
                }
                else if (rdoShell.Checked)
                {
                    List<Repuesto> copia = Ordenamientos.Copiar(centro.Repuestos.Listar());
                    Ordenamientos.ShellSortPorExistencia(copia, descendente);
                    MostrarRepuestos(copia);
                    MostrarResumen("Shell Sort", "Existencia disponible del repuesto", sentido, copia.Count);
                }
                else
                {
                    CriterioSolicitud criterio = (CriterioSolicitud)cboCriterioQuick.SelectedIndex;
                    List<Solicitud> copia = Ordenamientos.Copiar(centro.Solicitudes.Listar());
                    Ordenamientos.QuickSortSolicitudes(copia, criterio, descendente);

                    int columnaCriterio = 3;
                    if (criterio == CriterioSolicitud.TiempoEstimado)
                    {
                        columnaCriterio = 4;
                    }
                    else if (criterio == CriterioSolicitud.Codigo)
                    {
                        columnaCriterio = 1;
                    }
                    MostrarSolicitudes(copia, columnaCriterio);
                    MostrarResumen("Quick Sort", Ordenamientos.NombreCriterio(criterio), sentido, copia.Count);
                    centro.GuardarUltimoOrden(copia, "Quick Sort por " + Ordenamientos.NombreCriterio(criterio).ToLower() + " (" + sentido.ToLower() + ")");
                }
            }
            catch (ReglaNegocioException ex)
            {
                dgvResultado.Columns.Clear();
                lblResultado.Text = ex.Message;
                UiHelper.Aviso(ex.Message);
            }
        }

        private void MostrarResumen(string algoritmo, string criterio, string sentido, int cantidad)
        {
            lblResultado.Text = "Algoritmo: " + algoritmo + "     |     Criterio: " + criterio + "     |     Sentido: " + sentido + "\n" +
                                "Registros ordenados: " + cantidad + "     |     Comparaciones: " + Ordenamientos.Comparaciones +
                                "     |     Intercambios: " + Ordenamientos.Intercambios;
        }

        private void MostrarSolicitudes(List<Solicitud> lista, int columnaCriterio)
        {
            UiHelper.CrearColumnas(dgvResultado, "Posición", "Código", "Cliente", "Fecha de recepción",
                                   "Tiempo estimado", "Estado", "Costo total");
            for (int i = 0; i < lista.Count; i++)
            {
                Solicitud s = lista[i];
                dgvResultado.Rows.Add(i + 1, s.Codigo, s.Cliente.NombreCompleto, s.FechaRecepcion.ToString("dd/MM/yyyy HH:mm"),
                                      s.TiempoEstimadoHoras + " h", Textos.NombreEstado(s.Estado), Textos.Moneda(s.Total));
            }
            ResaltarColumna(columnaCriterio);
        }

        private void MostrarTecnicos(List<Tecnico> lista)
        {
            UiHelper.CrearColumnas(dgvResultado, "Posición", "Código", "Técnico", "Especialidad",
                                   "Servicios finalizados", "Trabajos activos");
            for (int i = 0; i < lista.Count; i++)
            {
                Tecnico t = lista[i];
                dgvResultado.Rows.Add(i + 1, t.Codigo, t.NombreCompleto, Textos.NombreEspecialidad(t.Especialidad),
                                      t.ServiciosFinalizados, t.TrabajosActivos + " / " + Tecnico.LIMITE_TRABAJOS);
            }
            ResaltarColumna(4);
        }

        private void MostrarRepuestos(List<Repuesto> lista)
        {
            UiHelper.CrearColumnas(dgvResultado, "Posición", "Código", "Repuesto", "Categoría",
                                   "Existencia", "Mínimo", "Estado");
            for (int i = 0; i < lista.Count; i++)
            {
                Repuesto r = lista[i];
                dgvResultado.Rows.Add(i + 1, r.Codigo, r.Nombre, Textos.NombreCategoria(r.Categoria),
                                      r.Existencia, r.ExistenciaMinima, r.TextoEstado);
            }
            ResaltarColumna(4);
        }

        private void ResaltarColumna(int columna)
        {
            dgvResultado.Columns[columna].DefaultCellStyle.ForeColor = UiHelper.ColorRojo;
            dgvResultado.Columns[columna].DefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            dgvResultado.Columns[0].FillWeight = 45;
            dgvResultado.ClearSelection();
        }
    }
}
