using XelaTechSoporte.Excepciones;
using XelaTechSoporte.Servicios;

namespace XelaTechSoporte.Formularios
{
    public partial class FrmDatosPrueba : Form
    {
        private CentroServicio centro;

        public FrmDatosPrueba(CentroServicio centro)
        {
            InitializeComponent();
            this.centro = centro;
            UiHelper.PonerIcono(btnGenerar, UiHelper.ICONO_DATOS, Color.White);
            UiHelper.PonerIcono(btnReiniciar, UiHelper.ICONO_ACTUALIZAR, UiHelper.ColorRojo);
        }

        private void FrmDatosPrueba_Load(object sender, EventArgs e)
        {
            txtResumen.Text = EstadoActual();
        }

        private string EstadoActual()
        {
            return "Datos actuales del sistema:\r\n" +
                   "  Clientes:    " + centro.Clientes.Listar().Count + "\r\n" +
                   "  Técnicos:    " + centro.Tecnicos.Listar().Count + "\r\n" +
                   "  Equipos:     " + centro.Equipos.Listar().Count + "\r\n" +
                   "  Repuestos:   " + centro.Repuestos.Listar().Count + "\r\n" +
                   "  Solicitudes: " + centro.Solicitudes.Listar().Count + "\r\n" +
                   "  En cola:     " + centro.Cola.CantidadUrgentes + " urgentes, " + centro.Cola.CantidadNormales + " normales\r\n";
        }

        private void btnGenerar_Click(object sender, EventArgs e)
        {
            try
            {
                GeneradorDatos generador;
                if (chkSemilla.Checked)
                {
                    generador = new GeneradorDatos(centro, (int)nudSemilla.Value);
                }
                else
                {
                    generador = new GeneradorDatos(centro);
                }

                string resumen = generador.Generar((int)nudClientes.Value, (int)nudTecnicos.Value, (int)nudEquipos.Value,
                                                   (int)nudRepuestos.Value, (int)nudSolicitudes.Value);

                txtResumen.Text = "Generación terminada (" + DateTime.Now.ToString("HH:mm:ss") + ")\r\n\r\n" +
                                  resumen + "\r\n" + EstadoActual();
                UiHelper.Info("Datos de prueba generados. Revise los módulos del menú para verlos.");
            }
            catch (ReglaNegocioException ex)
            {
                UiHelper.Aviso("No se pudieron generar todos los datos: " + ex.Message);
                txtResumen.Text = EstadoActual();
            }
        }

        private void btnReiniciar_Click(object sender, EventArgs e)
        {
            bool confirmar = UiHelper.Confirmar("¿Reiniciar el sistema?\n\nSe borrarán todos los clientes, técnicos, equipos, " +
                                                "solicitudes, repuestos, colas y la matriz de carga. Los códigos vuelven a empezar en 0001.");
            if (!confirmar)
            {
                return;
            }
            centro.Reiniciar();
            txtResumen.Text = "Sistema reiniciado (" + DateTime.Now.ToString("HH:mm:ss") + ").\r\n\r\n" + EstadoActual();
        }
    }
}
