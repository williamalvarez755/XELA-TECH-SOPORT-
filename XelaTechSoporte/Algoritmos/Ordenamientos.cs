using XelaTechSoporte.Excepciones;
using XelaTechSoporte.Modelos;

namespace XelaTechSoporte.Algoritmos
{
    public enum CriterioSolicitud
    {
        FechaRecepcion,
        TiempoEstimado,
        Codigo
    }

    public static class Ordenamientos
    {
        public static int Comparaciones = 0;
        public static int Intercambios = 0;

        public static List<T> Copiar<T>(List<T> original)
        {
            List<T> copia = new List<T>();
            for (int i = 0; i < original.Count; i++)
            {
                copia.Add(original[i]);
            }
            return copia;
        }

        public static void BubbleSortPorCosto(List<Solicitud> lista, bool descendente)
        {
            ValidarDatosSuficientes(lista.Count);
            ReiniciarContadores();

            int n = lista.Count;
            bool huboIntercambio = true;

            for (int vuelta = 0; vuelta < n - 1 && huboIntercambio; vuelta++)
            {
                huboIntercambio = false;
                for (int j = 0; j < n - 1 - vuelta; j++)
                {
                    Comparaciones++;
                    bool fueraDeOrden;
                    if (descendente)
                    {
                        fueraDeOrden = lista[j].Total < lista[j + 1].Total;
                    }
                    else
                    {
                        fueraDeOrden = lista[j].Total > lista[j + 1].Total;
                    }

                    if (fueraDeOrden)
                    {
                        Intercambiar(lista, j, j + 1);
                        huboIntercambio = true;
                    }
                }
            }
        }

        public static void SelectionSortPorServicios(List<Tecnico> lista, bool descendente)
        {
            ValidarDatosSuficientes(lista.Count);
            ReiniciarContadores();

            int n = lista.Count;
            for (int i = 0; i < n - 1; i++)
            {
                int posicionElegida = i;
                for (int j = i + 1; j < n; j++)
                {
                    Comparaciones++;
                    bool esMejor;
                    if (descendente)
                    {
                        esMejor = lista[j].ServiciosFinalizados > lista[posicionElegida].ServiciosFinalizados;
                    }
                    else
                    {
                        esMejor = lista[j].ServiciosFinalizados < lista[posicionElegida].ServiciosFinalizados;
                    }

                    if (esMejor)
                    {
                        posicionElegida = j;
                    }
                }

                if (posicionElegida != i)
                {
                    Intercambiar(lista, i, posicionElegida);
                }
            }
        }

        public static void ShellSortPorExistencia(List<Repuesto> lista, bool descendente)
        {
            ValidarDatosSuficientes(lista.Count);
            ReiniciarContadores();

            int n = lista.Count;
            for (int salto = n / 2; salto > 0; salto = salto / 2)
            {
                for (int i = salto; i < n; i++)
                {
                    int j = i;
                    while (j >= salto && RepuestoVaDespues(lista[j - salto], lista[j], descendente))
                    {
                        Intercambiar(lista, j, j - salto);
                        j = j - salto;
                    }
                }
            }
        }

        private static bool RepuestoVaDespues(Repuesto a, Repuesto b, bool descendente)
        {
            Comparaciones++;
            if (descendente)
            {
                return a.Existencia < b.Existencia;
            }
            return a.Existencia > b.Existencia;
        }

        public static void QuickSortSolicitudes(List<Solicitud> lista, CriterioSolicitud criterio, bool descendente)
        {
            ValidarDatosSuficientes(lista.Count);
            ReiniciarContadores();
            QuickSort(lista, 0, lista.Count - 1, criterio, descendente);
        }

        private static void QuickSort(List<Solicitud> lista, int inicio, int fin, CriterioSolicitud criterio, bool descendente)
        {
            if (inicio < fin)
            {
                int posicionPivote = Particionar(lista, inicio, fin, criterio, descendente);
                QuickSort(lista, inicio, posicionPivote - 1, criterio, descendente);
                QuickSort(lista, posicionPivote + 1, fin, criterio, descendente);
            }
        }

        private static int Particionar(List<Solicitud> lista, int inicio, int fin, CriterioSolicitud criterio, bool descendente)
        {
            Solicitud pivote = lista[fin];
            int i = inicio - 1;

            for (int j = inicio; j < fin; j++)
            {
                Comparaciones++;
                int resultado = CompararSolicitudes(lista[j], pivote, criterio);

                bool vaAntes;
                if (descendente)
                {
                    vaAntes = resultado >= 0;
                }
                else
                {
                    vaAntes = resultado <= 0;
                }

                if (vaAntes)
                {
                    i++;
                    Intercambiar(lista, i, j);
                }
            }
            Intercambiar(lista, i + 1, fin);
            return i + 1;
        }

        private static int CompararSolicitudes(Solicitud a, Solicitud b, CriterioSolicitud criterio)
        {
            if (criterio == CriterioSolicitud.FechaRecepcion)
            {
                return DateTime.Compare(a.FechaRecepcion, b.FechaRecepcion);
            }
            if (criterio == CriterioSolicitud.TiempoEstimado)
            {
                return a.TiempoEstimadoHoras - b.TiempoEstimadoHoras;
            }
            return Busquedas.CompararCodigos(a.Codigo, b.Codigo);
        }

        private static void Intercambiar<T>(List<T> lista, int i, int j)
        {
            T temporal = lista[i];
            lista[i] = lista[j];
            lista[j] = temporal;
            Intercambios++;
        }

        private static void ValidarDatosSuficientes(int cantidad)
        {
            if (cantidad < 2)
            {
                throw new ReglaNegocioException("No hay datos suficientes para ordenar: se necesitan al menos 2 registros y hay " + cantidad + ".");
            }
        }

        private static void ReiniciarContadores()
        {
            Comparaciones = 0;
            Intercambios = 0;
        }

        public static string NombreCriterio(CriterioSolicitud criterio)
        {
            if (criterio == CriterioSolicitud.FechaRecepcion)
            {
                return "Fecha de recepción";
            }
            if (criterio == CriterioSolicitud.TiempoEstimado)
            {
                return "Tiempo estimado (horas)";
            }
            return "Código";
        }
    }
}
