using System.Globalization;
using XelaTechSoporte.Formularios;

namespace XelaTechSoporte
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            CultureInfo guatemala = new CultureInfo("es-GT");
            CultureInfo.DefaultThreadCurrentCulture = guatemala;
            CultureInfo.DefaultThreadCurrentUICulture = guatemala;
            Thread.CurrentThread.CurrentCulture = guatemala;
            Thread.CurrentThread.CurrentUICulture = guatemala;

            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.ThreadException += ErrorNoControlado;

            Application.Run(new FrmPrincipal());
        }

        private static void ErrorNoControlado(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            MessageBox.Show("Ocurrió un error inesperado:\n\n" + e.Exception.Message,
                "XelaTech Soporte", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
