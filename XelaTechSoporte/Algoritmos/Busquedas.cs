using XelaTechSoporte.Excepciones;
using XelaTechSoporte.Modelos;

namespace XelaTechSoporte.Algoritmos
{
    public static class Busquedas
    {
        public static int UltimasComparaciones = 0;

        public static List<Cliente> SecuencialClientesPorNombre(List<Cliente> lista, string texto)
        {
            string buscado = PrepararTexto(texto);
            List<Cliente> resultados = new List<Cliente>();

            for (int i = 0; i < lista.Count; i++)
            {
                UltimasComparaciones++;
                if (Normalizar(lista[i].NombreCompleto).Contains(buscado))
                {
                    resultados.Add(lista[i]);
                }
            }
            return resultados;
        }

        public static List<Solicitud> SecuencialSolicitudesPorEstado(List<Solicitud> lista, string texto)
        {
            string buscado = PrepararTexto(texto);
            List<Solicitud> resultados = new List<Solicitud>();

            for (int i = 0; i < lista.Count; i++)
            {
                UltimasComparaciones++;
                string estado = Normalizar(Textos.NombreEstado(lista[i].Estado));
                if (estado.Contains(buscado))
                {
                    resultados.Add(lista[i]);
                }
            }
            return resultados;
        }

        public static List<Equipo> SecuencialEquiposPorMarca(List<Equipo> lista, string texto)
        {
            string buscado = PrepararTexto(texto);
            List<Equipo> resultados = new List<Equipo>();

            for (int i = 0; i < lista.Count; i++)
            {
                UltimasComparaciones++;
                if (Normalizar(lista[i].Marca).Contains(buscado))
                {
                    resultados.Add(lista[i]);
                }
            }
            return resultados;
        }

        public static List<Solicitud> SecuencialSolicitudes(List<Solicitud> lista, string texto)
        {
            string buscado = PrepararTexto(texto);
            List<Solicitud> resultados = new List<Solicitud>();

            for (int i = 0; i < lista.Count; i++)
            {
                UltimasComparaciones++;
                Solicitud s = lista[i];
                if (Normalizar(s.Codigo).Contains(buscado) ||
                    Normalizar(s.Cliente.NombreCompleto).Contains(buscado) ||
                    Normalizar(s.Equipo.ToString()).Contains(buscado))
                {
                    resultados.Add(s);
                }
            }
            return resultados;
        }

        private static string PrepararTexto(string texto)
        {
            UltimasComparaciones = 0;
            string buscado = Normalizar(texto);
            if (buscado == "")
            {
                throw new ReglaNegocioException("Escriba al menos un carácter para buscar.");
            }
            return buscado;
        }

        public static string Normalizar(string texto)
        {
            if (texto == null)
            {
                return "";
            }
            string resultado = texto.Trim().ToLower();
            resultado = resultado.Replace('á', 'a');
            resultado = resultado.Replace('é', 'e');
            resultado = resultado.Replace('í', 'i');
            resultado = resultado.Replace('ó', 'o');
            resultado = resultado.Replace('ú', 'u');
            resultado = resultado.Replace('ü', 'u');
            return resultado;
        }

        public static int Binaria(List<Solicitud> lista, string codigo, List<string> pasos)
        {
            UltimasComparaciones = 0;

            if (lista.Count == 0)
            {
                throw new ReglaNegocioException("No hay solicitudes registradas para buscar.");
            }
            string buscado = codigo.Trim().ToUpper();
            if (buscado == "")
            {
                throw new ReglaNegocioException("Escriba el código de la solicitud. Ejemplo: SOL-0003");
            }
            if (!EstaOrdenadaPorCodigo(lista))
            {
                throw new ReglaNegocioException("La lista no está ordenada por código. Ordénela primero (Quick Sort por código).");
            }

            int inicio = 0;
            int fin = lista.Count - 1;
            int paso = 1;

            while (inicio <= fin)
            {
                int medio = (inicio + fin) / 2;
                int resultado = CompararCodigos(buscado, lista[medio].Codigo);
                UltimasComparaciones++;

                string linea = "Paso " + paso + ": inicio=" + inicio + ", fin=" + fin + ", medio=" + medio +
                               " (" + lista[medio].Codigo + ")";

                if (resultado == 0)
                {
                    pasos.Add(linea + " -> ¡encontrado!");
                    return medio;
                }
                if (resultado > 0)
                {
                    pasos.Add(linea + " -> el buscado va después, se descarta la mitad izquierda");
                    inicio = medio + 1;
                }
                else
                {
                    pasos.Add(linea + " -> el buscado va antes, se descarta la mitad derecha");
                    fin = medio - 1;
                }
                paso++;
            }

            pasos.Add("El tramo quedó vacío: el código " + buscado + " no está registrado.");
            return -1;
        }

        public static int CompararCodigos(string a, string b)
        {
            int largoMenor = a.Length;
            if (b.Length < largoMenor)
            {
                largoMenor = b.Length;
            }

            for (int i = 0; i < largoMenor; i++)
            {
                if (a[i] != b[i])
                {
                    if (a[i] < b[i])
                    {
                        return -1;
                    }
                    return 1;
                }
            }
            return a.Length - b.Length;
        }

        public static bool EstaOrdenadaPorCodigo(List<Solicitud> lista)
        {
            for (int i = 0; i < lista.Count - 1; i++)
            {
                if (CompararCodigos(lista[i].Codigo, lista[i + 1].Codigo) > 0)
                {
                    return false;
                }
            }
            return true;
        }

        public static List<Solicitud> Multicriterio(List<Solicitud> lista, FiltroSolicitud filtro)
        {
            UltimasComparaciones = 0;
            if (!filtro.TieneAlgunFiltro())
            {
                throw new ReglaNegocioException("Indique al menos un criterio de búsqueda.");
            }
            if (filtro.UsarFechas && filtro.Desde.Date > filtro.Hasta.Date)
            {
                throw new ReglaNegocioException("La fecha 'desde' no puede ser mayor que la fecha 'hasta'.");
            }

            List<Solicitud> resultados = new List<Solicitud>();
            for (int i = 0; i < lista.Count; i++)
            {
                Solicitud s = lista[i];
                bool cumple = true;

                if (cumple && filtro.UsarEstado)
                {
                    UltimasComparaciones++;
                    if (s.Estado != filtro.Estado)
                    {
                        cumple = false;
                    }
                }
                if (cumple && filtro.UsarPrioridad)
                {
                    UltimasComparaciones++;
                    if (s.Prioridad != filtro.Prioridad)
                    {
                        cumple = false;
                    }
                }
                if (cumple && filtro.Tecnico != null)
                {
                    UltimasComparaciones++;
                    if (s.TecnicoAsignado == null || s.TecnicoAsignado != filtro.Tecnico)
                    {
                        cumple = false;
                    }
                }
                if (cumple && filtro.UsarFechas)
                {
                    UltimasComparaciones++;
                    if (s.FechaRecepcion.Date < filtro.Desde.Date || s.FechaRecepcion.Date > filtro.Hasta.Date)
                    {
                        cumple = false;
                    }
                }

                if (cumple)
                {
                    resultados.Add(s);
                }
            }
            return resultados;
        }
    }
}
