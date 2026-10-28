using XelaTechSoporte.Estructuras;
using XelaTechSoporte.Excepciones;
using XelaTechSoporte.Modelos;
using XelaTechSoporte.Servicios;

namespace XelaTechSoporte.Formularios
{
    public partial class FrmColas : Form
    {
        private CentroServicio centro;

        private List<Tecnico> candidatos = new List<Tecnico>();

        public FrmColas(CentroServicio centro)
        {
            InitializeComponent();
            this.centro = centro;

            UiHelper.EstilizarRejilla(dgvUrgente);
            UiHelper.EstilizarRejilla(dgvNormal);
            UiHelper.EstilizarRejilla(dgvMatriz);
            UiHelper.CrearColumnas(dgvUrgente, "Turno", "Código", "Cliente", "Especialidad", "Espera");
            UiHelper.CrearColumnas(dgvNormal, "Turno", "Código", "Cliente", "Especialidad", "Espera");

            string[] columnas = new string[MatrizCarga.DIAS + 2];
            columnas[0] = "Técnico";
            for (int d = 0; d < MatrizCarga.DIAS; d++)
            {
                columnas[d + 1] = MatrizCarga.NombresDias[d];
            }
            columnas[MatrizCarga.DIAS + 1] = "Total semana";
            UiHelper.CrearColumnas(dgvMatriz, columnas);
            dgvMatriz.Columns[0].FillWeight = 200;
            dgvUrgente.Columns[0].FillWeight = 50;
            dgvNormal.Columns[0].FillWeight = 50;

            UiHelper.PonerIcono(btnRevisarEscalamientos, UiHelper.ICONO_ALERTA, UiHelper.ColorTexto);
            UiHelper.PonerIcono(btnVerSiguiente, UiHelper.ICONO_SIGUIENTE, UiHelper.ColorTexto);
            UiHelper.PonerIcono(btnAsignar, UiHelper.ICONO_PERSONAS, Color.White);
        }

        private void FrmColas_Load(object sender, EventArgs e)
        {
            RefrescarTodo();
        }

        private void RefrescarTodo()
        {
            MostrarColas();
            MostrarTurno();
            MostrarMatriz();
        }

        private void MostrarColas()
        {
            Solicitud[] urgentes = centro.Cola.VerEnEspera(NivelPrioridad.Urgente);
            Solicitud[] normales = centro.Cola.VerEnEspera(NivelPrioridad.Normal);
            LlenarCola(dgvUrgente, urgentes);
            LlenarCola(dgvNormal, normales);
            lblUrgente.Text = "Cola urgente  (" + urgentes.Length + ")";
            lblNormal.Text = "Cola normal  (" + normales.Length + ")";
            lblTotales.Text = "Urgentes: " + urgentes.Length + "     Normales: " + normales.Length +
                              "     Total en espera: " + (urgentes.Length + normales.Length);
        }

        private void LlenarCola(DataGridView rejilla, Solicitud[] cola)
        {
            rejilla.Rows.Clear();
            for (int i = 0; i < cola.Length; i++)
            {
                Solicitud s = cola[i];
                string codigo = s.Codigo;
                if (s.Escalada)
                {
                    codigo += " (escalada)";
                }
                rejilla.Rows.Add(i + 1, codigo, s.Cliente.NombreCompleto, Textos.NombreEspecialidad(s.EspecialidadRequerida),
                                 GeneradorReportes.TextoEspera(s.FechaRecepcion));
            }
            rejilla.ClearSelection();
        }

        private void MostrarTurno()
        {
            cboTecnico.Items.Clear();
            candidatos.Clear();

            Solicitud turno = centro.Solicitudes.VerSiguienteEnCola();
            if (turno == null)
            {
                lblTurnoInfo.Text = "No hay solicitudes en espera.";
                btnAsignar.Enabled = false;
                return;
            }

            string prioridad = Textos.NombrePrioridad(turno.Prioridad).ToUpper();
            if (turno.Escalada)
            {
                prioridad += " (escalada por tiempo de espera)";
            }
            lblTurnoInfo.Text = turno.Codigo + "   ·   " + prioridad + "\n" +
                                "Cliente: " + turno.Cliente.NombreCompleto + "   ·   Equipo: " + turno.Equipo + "\n" +
                                "Especialidad requerida: " + Textos.NombreEspecialidad(turno.EspecialidadRequerida) +
                                "   ·   Esperando: " + GeneradorReportes.TextoEspera(turno.FechaRecepcion) + "\n" +
                                "Problema: " + turno.DescripcionProblema;

            if (chkMostrarTodos.Checked)
            {
                List<Tecnico> todos = centro.Tecnicos.Listar();
                for (int i = 0; i < todos.Count; i++)
                {
                    candidatos.Add(todos[i]);
                }
            }
            else
            {
                candidatos = centro.Tecnicos.DisponiblesPorEspecialidad(turno.EspecialidadRequerida);
            }

            for (int i = 0; i < candidatos.Count; i++)
            {
                Tecnico t = candidatos[i];
                cboTecnico.Items.Add(t.NombreCompleto + "  ·  " + Textos.NombreEspecialidad(t.Especialidad) +
                                     "  ·  activos " + t.TrabajosActivos + "/" + Tecnico.LIMITE_TRABAJOS);
            }

            if (candidatos.Count == 0)
            {
                cboTecnico.Items.Add("(no hay técnicos de " + Textos.NombreEspecialidad(turno.EspecialidadRequerida) + " con cupo)");
                cboTecnico.SelectedIndex = 0;
                btnAsignar.Enabled = false;
                return;
            }

            Tecnico mejor = centro.Tecnicos.AsignarMejorTecnico(turno.EspecialidadRequerida);
            int posicion = candidatos.IndexOf(mejor);
            if (posicion < 0)
            {
                posicion = 0;
            }
            cboTecnico.SelectedIndex = posicion;
            btnAsignar.Enabled = true;
        }

        private void btnAsignar_Click(object sender, EventArgs e)
        {
            if (cboTecnico.SelectedIndex < 0 || cboTecnico.SelectedIndex >= candidatos.Count)
            {
                UiHelper.Aviso("Seleccione un técnico.");
                return;
            }
            try
            {
                Tecnico tecnico = candidatos[cboTecnico.SelectedIndex];
                Solicitud atendida = centro.Solicitudes.AsignarSiguiente(tecnico);
                RefrescarTodo();
                UiHelper.Info("La solicitud " + atendida.Codigo + " salió de la cola y fue asignada a " + tecnico.NombreCompleto + ".\n\n" +
                              "Estado: " + Textos.NombreEstado(atendida.Estado) + "\n" +
                              "Carga registrada en la matriz: " + MatrizCarga.NombresDias[atendida.DiaAsignacion] + "\n" +
                              "Trabajos activos del técnico: " + tecnico.TrabajosActivos + " de " + Tecnico.LIMITE_TRABAJOS);
            }
            catch (ReglaNegocioException ex)
            {
                UiHelper.Aviso(ex.Message);
            }
        }

        private void btnVerSiguiente_Click(object sender, EventArgs e)
        {
            RefrescarTodo();
        }

        private void chkMostrarTodos_CheckedChanged(object sender, EventArgs e)
        {
            MostrarTurno();
        }

        private void btnRevisarEscalamientos_Click(object sender, EventArgs e)
        {
            int escaladas = centro.Solicitudes.RevisarEscalamientos();
            RefrescarTodo();
            if (escaladas == 0)
            {
                UiHelper.Info("Ninguna solicitud normal lleva " + ColaAtencion.HORAS_ESCALAMIENTO + " horas o más esperando.");
            }
            else
            {
                UiHelper.Info(escaladas + " solicitud(es) pasaron de la cola normal a la urgente por esperar " +
                              ColaAtencion.HORAS_ESCALAMIENTO + " horas o más.");
            }
        }

        private void MostrarMatriz()
        {
            dgvMatriz.Rows.Clear();
            MatrizCarga matriz = centro.Matriz;
            List<Tecnico> tecnicos = centro.Tecnicos.Listar();
            Tecnico mayor = matriz.TecnicoConMayorCarga();
            int diaMayor = matriz.DiaConMayorCarga();

            for (int f = 0; f < matriz.Filas; f++)
            {
                int fila = dgvMatriz.Rows.Add(tecnicos[f].NombreCompleto);
                for (int d = 0; d < MatrizCarga.DIAS; d++)
                {
                    dgvMatriz.Rows[fila].Cells[d + 1].Value = matriz.Valor(f, d);
                }
                dgvMatriz.Rows[fila].Cells[MatrizCarga.DIAS + 1].Value = matriz.TotalTecnico(f);

                if (tecnicos[f] == mayor)
                {
                    dgvMatriz.Rows[fila].DefaultCellStyle.ForeColor = UiHelper.ColorRojo;
                    dgvMatriz.Rows[fila].DefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
                }
            }

            if (matriz.Filas > 0)
            {
                int filaTotal = dgvMatriz.Rows.Add("TOTAL POR DÍA");
                for (int d = 0; d < MatrizCarga.DIAS; d++)
                {
                    dgvMatriz.Rows[filaTotal].Cells[d + 1].Value = matriz.TotalDia(d);
                }
                dgvMatriz.Rows[filaTotal].DefaultCellStyle.BackColor = UiHelper.ColorFondo;
                dgvMatriz.Rows[filaTotal].DefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
                if (diaMayor >= 0)
                {
                    dgvMatriz.Rows[filaTotal].Cells[diaMayor + 1].Style.ForeColor = UiHelper.ColorRojo;
                }
            }
            dgvMatriz.ClearSelection();
            lblMayor.Text = GeneradorReportes.TextoMayorCarga(matriz);
        }
    }
}
