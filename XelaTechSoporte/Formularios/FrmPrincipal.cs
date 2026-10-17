using XelaTechSoporte.Servicios;

namespace XelaTechSoporte.Formularios
{
    public partial class FrmPrincipal : Form
    {
        private static readonly Color ColorTextoMenu = Color.FromArgb(212, 212, 216);
        private static readonly Color ColorIconoMenu = Color.FromArgb(161, 161, 170);

        private CentroServicio centro;
        private Form formularioActual = null;
        private Button botonActivo = null;

        public FrmPrincipal()
        {
            InitializeComponent();
            centro = new CentroServicio();

            btnInicio.Tag = UiHelper.ICONO_INICIO;
            btnClientes.Tag = UiHelper.ICONO_CLIENTES;
            btnTecnicos.Tag = UiHelper.ICONO_TECNICOS;
            btnEquipos.Tag = UiHelper.ICONO_EQUIPOS;
            btnSolicitudes.Tag = UiHelper.ICONO_SOLICITUDES;
            btnRepuestos.Tag = UiHelper.ICONO_REPUESTOS;
            btnColas.Tag = UiHelper.ICONO_COLAS;
            btnOrdenamientos.Tag = UiHelper.ICONO_ORDENAR;
            btnBusquedas.Tag = UiHelper.ICONO_BUSCAR;
            btnReportes.Tag = UiHelper.ICONO_REPORTES;
            btnDatosPrueba.Tag = UiHelper.ICONO_DATOS;

            Button[] botones = { btnInicio, btnClientes, btnTecnicos, btnEquipos, btnSolicitudes, btnRepuestos,
                                 btnColas, btnOrdenamientos, btnBusquedas, btnReportes, btnDatosPrueba };
            for (int i = 0; i < botones.Length; i++)
            {
                UiHelper.PonerIcono(botones[i], (string)botones[i].Tag, ColorIconoMenu);
            }
        }

        private void FrmPrincipal_Load(object sender, EventArgs e)
        {
            AbrirFormulario(new FrmInicio(centro), btnInicio);
        }

        private void AbrirFormulario(Form formulario, Button boton)
        {
            if (formularioActual != null)
            {
                formularioActual.Close();
                formularioActual.Dispose();
            }

            formularioActual = formulario;
            formulario.TopLevel = false;
            formulario.FormBorderStyle = FormBorderStyle.None;
            formulario.Dock = DockStyle.Fill;
            panelContenido.Controls.Add(formulario);
            formulario.Show();

            MarcarBotonActivo(boton);
            ActualizarBarraEstado();
        }

        private void MarcarBotonActivo(Button boton)
        {
            if (botonActivo != null)
            {
                botonActivo.BackColor = UiHelper.ColorMenu;
                botonActivo.ForeColor = ColorTextoMenu;
                UiHelper.PonerIcono(botonActivo, (string)botonActivo.Tag, ColorIconoMenu);
            }
            botonActivo = boton;
            boton.BackColor = UiHelper.ColorRojo;
            boton.ForeColor = Color.White;
            UiHelper.PonerIcono(boton, (string)boton.Tag, Color.White);
        }

        private void ActualizarBarraEstado()
        {
            lblEstado.Text = "Clientes: " + centro.Clientes.Listar().Count +
                             "     Técnicos: " + centro.Tecnicos.Listar().Count +
                             "     Equipos: " + centro.Equipos.Listar().Count +
                             "     Solicitudes: " + centro.Solicitudes.Listar().Count +
                             "     Cola urgente: " + centro.Cola.CantidadUrgentes +
                             "     Cola normal: " + centro.Cola.CantidadNormales;
        }

        private void timerEstado_Tick(object sender, EventArgs e)
        {
            ActualizarBarraEstado();
        }

        private void btnInicio_Click(object sender, EventArgs e)
        {
            AbrirFormulario(new FrmInicio(centro), btnInicio);
        }

        private void btnClientes_Click(object sender, EventArgs e)
        {
            AbrirFormulario(new FrmClientes(centro), btnClientes);
        }

        private void btnTecnicos_Click(object sender, EventArgs e)
        {
            AbrirFormulario(new FrmTecnicos(centro), btnTecnicos);
        }

        private void btnEquipos_Click(object sender, EventArgs e)
        {
            AbrirFormulario(new FrmEquipos(centro), btnEquipos);
        }

        private void btnSolicitudes_Click(object sender, EventArgs e)
        {
            AbrirFormulario(new FrmSolicitudes(centro), btnSolicitudes);
        }

        private void btnRepuestos_Click(object sender, EventArgs e)
        {
            AbrirFormulario(new FrmRepuestos(centro), btnRepuestos);
        }

        private void btnColas_Click(object sender, EventArgs e)
        {
            AbrirFormulario(new FrmColas(centro), btnColas);
        }

        private void btnOrdenamientos_Click(object sender, EventArgs e)
        {
            AbrirFormulario(new FrmOrdenamientos(centro), btnOrdenamientos);
        }

        private void btnBusquedas_Click(object sender, EventArgs e)
        {
            AbrirFormulario(new FrmBusquedas(centro), btnBusquedas);
        }

        private void btnReportes_Click(object sender, EventArgs e)
        {
            AbrirFormulario(new FrmReportes(centro), btnReportes);
        }

        private void btnDatosPrueba_Click(object sender, EventArgs e)
        {
            AbrirFormulario(new FrmDatosPrueba(centro), btnDatosPrueba);
        }
    }
}
