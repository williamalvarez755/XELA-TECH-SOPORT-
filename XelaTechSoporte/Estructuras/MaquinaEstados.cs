using XelaTechSoporte.Modelos;

namespace XelaTechSoporte.Estructuras
{
    public static class MaquinaEstados
    {
        private const int CANTIDAD_ESTADOS = 8;

        private static bool[,] transiciones = new bool[CANTIDAD_ESTADOS, CANTIDAD_ESTADOS];

        static MaquinaEstados()
        {
            Permitir(EstadoSolicitud.Recibida, EstadoSolicitud.EnEspera);
            Permitir(EstadoSolicitud.Recibida, EstadoSolicitud.Cancelada);

            Permitir(EstadoSolicitud.EnEspera, EstadoSolicitud.Asignada);
            Permitir(EstadoSolicitud.EnEspera, EstadoSolicitud.Cancelada);

            Permitir(EstadoSolicitud.Asignada, EstadoSolicitud.EnEspera);
            Permitir(EstadoSolicitud.Asignada, EstadoSolicitud.EnDiagnostico);
            Permitir(EstadoSolicitud.Asignada, EstadoSolicitud.Cancelada);

            Permitir(EstadoSolicitud.EnDiagnostico, EstadoSolicitud.EnReparacion);
            Permitir(EstadoSolicitud.EnDiagnostico, EstadoSolicitud.ListaParaEntregar);
            Permitir(EstadoSolicitud.EnDiagnostico, EstadoSolicitud.Cancelada);

            Permitir(EstadoSolicitud.EnReparacion, EstadoSolicitud.ListaParaEntregar);
            Permitir(EstadoSolicitud.EnReparacion, EstadoSolicitud.Cancelada);

            Permitir(EstadoSolicitud.ListaParaEntregar, EstadoSolicitud.Entregada);
            Permitir(EstadoSolicitud.ListaParaEntregar, EstadoSolicitud.Cancelada);
        }

        private static void Permitir(EstadoSolicitud desde, EstadoSolicitud hacia)
        {
            transiciones[(int)desde, (int)hacia] = true;
        }

        public static bool EsValida(EstadoSolicitud actual, EstadoSolicitud destino)
        {
            return transiciones[(int)actual, (int)destino];
        }

        public static List<EstadoSolicitud> EstadosPosibles(EstadoSolicitud actual)
        {
            List<EstadoSolicitud> posibles = new List<EstadoSolicitud>();
            int fila = (int)actual;
            for (int columna = 0; columna < CANTIDAD_ESTADOS; columna++)
            {
                if (transiciones[fila, columna])
                {
                    posibles.Add((EstadoSolicitud)columna);
                }
            }
            return posibles;
        }

        public static string MostrarMatriz()
        {
            string[] abreviaturas = { "Rec", "Esp", "Asig", "Diag", "Rep", "Lista", "Entr", "Canc" };
            string texto = "Desde \\ Hacia".PadRight(22);
            for (int c = 0; c < CANTIDAD_ESTADOS; c++)
            {
                texto += abreviaturas[c].PadRight(6);
            }
            texto += "\n";

            for (int f = 0; f < CANTIDAD_ESTADOS; f++)
            {
                texto += Textos.Estados[f].PadRight(22);
                for (int c = 0; c < CANTIDAD_ESTADOS; c++)
                {
                    if (transiciones[f, c])
                    {
                        texto += "Sí".PadRight(6);
                    }
                    else
                    {
                        texto += "-".PadRight(6);
                    }
                }
                texto += "\n";
            }
            return texto;
        }
    }
}
