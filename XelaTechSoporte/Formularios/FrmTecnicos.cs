using XelaTechSoporte.Excepciones;
using XelaTechSoporte.Modelos;
using XelaTechSoporte.Servicios;

namespace XelaTechSoporte.Formularios
{
    public partial class FrmTecnicos : Form
    {
        private CentroServicio centro;
        private Tecnico seleccionado = null;

        public FrmTecnicos(CentroServicio centro)
        {
            InitializeComponent();
            this.centro = centro;

            UiHelper.EstilizarRejilla(dgvTecnicos);
            UiHelper.CrearColumnas(dgvTecnicos, "Código", "Nombre completo", "Especialidad", "Estado",
                                   "Trabajos activos", "Finalizados", "Calificación");

            cboEspecialidad.Items.AddRange(Textos.Especialidades);
            cboFiltro.Items.Add("(Todas)");
            cboFiltro.Items.AddRange(Textos.Especialidades);

            UiHelper.PonerIcono(btnNuevo, UiHelper.ICONO_AGREGAR, Color.White);
            UiHelper.PonerIcono(btnGuardar, UiHelper.ICONO_GUARDAR, Color.White);
            UiHelper.PonerIcono(btnCancelar, UiHelper.ICONO_CANCELAR, UiHelper.ColorTexto);
            UiHelper.PonerIcono(btnBuscarCodigo, UiHelper.ICONO_BUSCAR, UiHelper.ColorTexto);
            UiHelper.PonerIcono(btnCalificar, UiHelper.ICONO_ESTRELLA, UiHelper.ColorTexto);
        }

        private void FrmTecnicos_Load(object sender, EventArgs e)
        {
            cboFiltro.SelectedIndex = 0;
            cboEstrellas.SelectedIndex = 4;
            PrepararNuevo();
        }

        private void MostrarTecnicos(List<Tecnico> lista)
        {
            dgvTecnicos.Rows.Clear();
            for (int i = 0; i < lista.Count; i++)
            {
                Tecnico t = lista[i];
                string calificacion = "Sin calificar";
                if (t.TotalCalificaciones > 0)
                {
                    calificacion = "★ " + t.CalificacionPromedio.ToString("0.0") + "  (" + t.TotalCalificaciones + ")";
                }

                int fila = dgvTecnicos.Rows.Add(t.Codigo, t.NombreCompleto, Textos.NombreEspecialidad(t.Especialidad),
                                                t.TextoEstado, t.TrabajosActivos + " / " + Tecnico.LIMITE_TRABAJOS,
                                                t.ServiciosFinalizados, calificacion);
                dgvTecnicos.Rows[fila].Tag = t;
                UiHelper.MarcarEtiqueta(dgvTecnicos.Rows[fila].Cells[3], ColorEstadoTecnico(t));
            }
            dgvTecnicos.ClearSelection();
            lblConteo.Text = lista.Count + " técnico(s) mostrados.  Límite de trabajos activos por técnico: " + Tecnico.LIMITE_TRABAJOS;
        }

        private Color ColorEstadoTecnico(Tecnico t)
        {
            if (!t.Disponible)
            {
                return UiHelper.EtiquetaGris;
            }
            if (t.TieneCargaCompleta)
            {
                return UiHelper.ColorRojo;
            }
            return UiHelper.EtiquetaVerde;
        }

        private void cboFiltro_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboFiltro.SelectedIndex <= 0)
            {
                MostrarTecnicos(centro.Tecnicos.Listar());
            }
            else
            {
                Especialidad especialidad = (Especialidad)(cboFiltro.SelectedIndex - 1);
                MostrarTecnicos(centro.Tecnicos.ListarPorEspecialidad(especialidad));
            }
        }

        private void PrepararNuevo()
        {
            seleccionado = null;
            lblModo.Text = "Nuevo técnico";
            txtCodigo.Text = "(se genera al guardar)";
            txtNombre.Clear();
            txtTelefono.Clear();
            txtCorreo.Clear();
            cboEspecialidad.SelectedIndex = 0;
            chkDisponible.Checked = true;
            lblInfo.Text = "";
            btnCalificar.Enabled = false;
            dgvTecnicos.ClearSelection();
            txtNombre.Focus();
        }

        private void CargarTecnico(Tecnico t)
        {
            seleccionado = t;
            lblModo.Text = "Editando " + t.Codigo;
            txtCodigo.Text = t.Codigo;
            txtNombre.Text = t.NombreCompleto;
            txtTelefono.Text = t.Telefono;
            txtCorreo.Text = t.Correo;
            cboEspecialidad.SelectedIndex = (int)t.Especialidad;
            chkDisponible.Checked = t.Disponible;
            btnCalificar.Enabled = true;
            MostrarInfo(t);
        }

        private void MostrarInfo(Tecnico t)
        {
            string conteo = "";
            for (int estrellas = 1; estrellas <= 5; estrellas++)
            {
                conteo += estrellas + "★: " + t.CantidadCalificaciones(estrellas) + "   ";
            }
            lblInfo.Text = "Trabajos activos: " + t.TrabajosActivos + " de " + Tecnico.LIMITE_TRABAJOS +
                           "   |   Finalizados: " + t.ServiciosFinalizados + "\n" +
                           "Promedio: " + t.CalificacionPromedio.ToString("0.00") + " (" + t.TotalCalificaciones + " calificaciones)\n" +
                           conteo;
        }

        private void dgvTecnicos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }
            CargarTecnico((Tecnico)dgvTecnicos.Rows[e.RowIndex].Tag);
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                string nombre = UiHelper.LeerTexto(txtNombre, "Nombre completo");
                string telefono = UiHelper.LeerTexto(txtTelefono, "Teléfono");
                string correo = UiHelper.LeerTexto(txtCorreo, "Correo electrónico");
                Especialidad especialidad = (Especialidad)cboEspecialidad.SelectedIndex;

                if (seleccionado == null)
                {
                    Tecnico nuevo = centro.Tecnicos.Registrar(nombre, telefono, correo, especialidad);
                    if (!chkDisponible.Checked)
                    {
                        nuevo.Disponible = false;
                    }
                    cboFiltro_SelectedIndexChanged(sender, e);
                    UiHelper.Info("Técnico registrado con el código " + nuevo.Codigo + ".");
                    PrepararNuevo();
                }
                else
                {
                    centro.Tecnicos.Actualizar(seleccionado.Codigo, nombre, telefono, correo, especialidad, chkDisponible.Checked);
                    cboFiltro_SelectedIndexChanged(sender, e);
                    UiHelper.Info("Datos del técnico " + seleccionado.Codigo + " actualizados.");
                    CargarTecnico(seleccionado);
                }
            }
            catch (ReglaNegocioException ex)
            {
                UiHelper.Aviso(ex.Message);
            }
        }

        private void btnCalificar_Click(object sender, EventArgs e)
        {
            if (seleccionado == null)
            {
                UiHelper.Aviso("Seleccione un técnico de la lista.");
                return;
            }
            try
            {
                int estrellas = cboEstrellas.SelectedIndex + 1;
                centro.Tecnicos.Calificar(seleccionado.Codigo, estrellas);
                cboFiltro_SelectedIndexChanged(sender, e);
                CargarTecnico(seleccionado);
                UiHelper.Info("Se registró una calificación de " + estrellas + " estrella(s) para " + seleccionado.NombreCompleto + ".");
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
                UiHelper.Aviso("Escriba el código del técnico. Ejemplo: TEC-0001");
                txtBuscarCodigo.Focus();
                return;
            }
            Tecnico tecnico = centro.Tecnicos.BuscarPorCodigo(txtBuscarCodigo.Text);
            if (tecnico == null)
            {
                UiHelper.Aviso("No existe un técnico con el código \"" + txtBuscarCodigo.Text.Trim().ToUpper() + "\".");
                return;
            }
            List<Tecnico> uno = new List<Tecnico>();
            uno.Add(tecnico);
            MostrarTecnicos(uno);
            CargarTecnico(tecnico);
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
