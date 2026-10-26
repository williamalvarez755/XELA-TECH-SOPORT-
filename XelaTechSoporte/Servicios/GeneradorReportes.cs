using XelaTechSoporte.Algoritmos;
using XelaTechSoporte.Estructuras;
using XelaTechSoporte.Excepciones;
using XelaTechSoporte.Modelos;

namespace XelaTechSoporte.Servicios
{
    public class GeneradorReportes
    {
        public static readonly string[] Nombres =
        {
            "1. Solicitudes en espera por prioridad",
            "2. Solicitudes activas, finalizadas y canceladas",
            "3. Técnicos disponibles y con carga completa",
            "4. Técnico con más servicios finalizados",
            "5. Equipos atendidos por tipo",
            "6. Solicitudes ordenadas por costo total",
            "7. Solicitudes ordenadas por fecha o tiempo",
            "8. Repuestos ordenados por existencia",
            "9. Repuestos con existencia mínima o agotados",
            "10. Ingresos por servicios finalizados",
            "11. Resultados de las búsquedas",
            "12. Matriz semanal de carga de trabajo"
        };

        private CentroServicio centro;

        public GeneradorReportes(CentroServicio centro)
        {
            this.centro = centro;
        }

        public TablaReporte Generar(int numero, bool descendente, CriterioSolicitud criterio7)
        {
            switch (numero)
            {
                case 1: return Reporte1();
                case 2: return Reporte2();
                case 3: return Reporte3();
                case 4: return Reporte4();
                case 5: return Reporte5();
                case 6: return Reporte6(descendente);
                case 7: return Reporte7(descendente, criterio7);
                case 8: return Reporte8(descendente);
                case 9: return Reporte9();
                case 10: return Reporte10();
                case 11: return Reporte11();
                case 12: return Reporte12();
                default:
                    throw new ReglaNegocioException("El reporte " + numero + " no existe.");
            }
        }

        public string TodosComoTexto(bool descendente, CriterioSolicitud criterio7)
        {
            string texto = "XELATECH SOPORTE - REPORTES (" + DateTime.Now.ToString("dd/MM/yyyy HH:mm") + ")\r\n\r\n";
            for (int i = 1; i <= 12; i++)
            {
                texto += Generar(i, descendente, criterio7).ComoTexto() + "\r\n\r\n";
            }
            return texto;
        }

        private TablaReporte Reporte1()
        {
            TablaReporte tabla = new TablaReporte(Nombres[0],
                "Datos: las dos colas, recorridas como arreglo sin desencolar. Orden de llegada (FIFO).",
                new string[] { "Cola", "Turno", "Código", "Cliente", "Equipo", "Recepción", "Espera" });

            Solicitud[] urgentes = centro.Cola.VerEnEspera(NivelPrioridad.Urgente);
            Solicitud[] normales = centro.Cola.VerEnEspera(NivelPrioridad.Normal);

            for (int i = 0; i < urgentes.Length; i++)
            {
                AgregarFilaCola(tabla, "Urgente", i + 1, urgentes[i]);
            }
            for (int i = 0; i < normales.Length; i++)
            {
                AgregarFilaCola(tabla, "Normal", i + 1, normales[i]);
            }
            tabla.Resumen = "En espera: " + urgentes.Length + " urgente(s) y " + normales.Length + " normal(es).";
            return tabla;
        }

        private void AgregarFilaCola(TablaReporte tabla, string cola, int turno, Solicitud s)
        {
            string nombreCola = cola;
            if (s.Escalada)
            {
                nombreCola += " (escalada)";
            }
            tabla.AgregarFila(nombreCola, turno.ToString(), s.Codigo, s.Cliente.NombreCompleto,
                              s.Equipo.Marca + " " + s.Equipo.Modelo, s.FechaRecepcion.ToString("dd/MM/yyyy HH:mm"),
                              TextoEspera(s.FechaRecepcion));
        }

        private TablaReporte Reporte2()
        {
            TablaReporte tabla = new TablaReporte(Nombres[1],
                "Datos: lista de solicitudes, agrupada por situación con un conteo por estado.",
                new string[] { "Grupo", "Código", "Cliente", "Equipo", "Estado", "Técnico", "Total" });

            List<Solicitud> lista = centro.Solicitudes.Listar();
            int activas = 0;
            int finalizadas = 0;
            int canceladas = 0;

            for (int i = 0; i < lista.Count; i++)
            {
                if (lista[i].EstaActiva)
                {
                    AgregarFilaSolicitud(tabla, "Activa", lista[i]);
                    activas++;
                }
            }
            for (int i = 0; i < lista.Count; i++)
            {
                if (lista[i].Estado == EstadoSolicitud.Entregada)
                {
                    AgregarFilaSolicitud(tabla, "Finalizada", lista[i]);
                    finalizadas++;
                }
            }
            for (int i = 0; i < lista.Count; i++)
            {
                if (lista[i].Estado == EstadoSolicitud.Cancelada)
                {
                    AgregarFilaSolicitud(tabla, "Cancelada", lista[i]);
                    canceladas++;
                }
            }
            tabla.Resumen = "Activas: " + activas + "  |  Finalizadas: " + finalizadas + "  |  Canceladas: " + canceladas;
            return tabla;
        }

        private void AgregarFilaSolicitud(TablaReporte tabla, string grupo, Solicitud s)
        {
            tabla.AgregarFila(grupo, s.Codigo, s.Cliente.NombreCompleto, s.Equipo.Marca + " " + s.Equipo.Modelo,
                              Textos.NombreEstado(s.Estado), NombreTecnico(s.TecnicoAsignado), Textos.Moneda(s.Total));
        }

        private TablaReporte Reporte3()
        {
            TablaReporte tabla = new TablaReporte(Nombres[2],
                "Datos: lista de técnicos. Se usa PuedeRecibirTrabajo() (límite de " + Tecnico.LIMITE_TRABAJOS + " trabajos activos).",
                new string[] { "Situación", "Código", "Técnico", "Especialidad", "Trabajos activos" });

            List<Tecnico> lista = centro.Tecnicos.Listar();
            int disponibles = 0;
            int completos = 0;
            int noDisponibles = 0;

            for (int i = 0; i < lista.Count; i++)
            {
                if (lista[i].PuedeRecibirTrabajo())
                {
                    AgregarFilaTecnico(tabla, "Disponible", lista[i]);
                    disponibles++;
                }
            }
            for (int i = 0; i < lista.Count; i++)
            {
                if (lista[i].TieneCargaCompleta)
                {
                    AgregarFilaTecnico(tabla, "Carga completa", lista[i]);
                    completos++;
                }
            }
            for (int i = 0; i < lista.Count; i++)
            {
                if (!lista[i].Disponible && !lista[i].TieneCargaCompleta)
                {
                    AgregarFilaTecnico(tabla, "No disponible", lista[i]);
                    noDisponibles++;
                }
            }
            tabla.Resumen = "Disponibles: " + disponibles + "  |  Con carga completa: " + completos + "  |  No disponibles: " + noDisponibles;
            return tabla;
        }

        private void AgregarFilaTecnico(TablaReporte tabla, string situacion, Tecnico t)
        {
            tabla.AgregarFila(situacion, t.Codigo, t.NombreCompleto, Textos.NombreEspecialidad(t.Especialidad),
                              t.TrabajosActivos + " / " + Tecnico.LIMITE_TRABAJOS);
        }

        private TablaReporte Reporte4()
        {
            TablaReporte tabla = new TablaReporte(Nombres[3],
                "Algoritmo: Selection Sort  |  Criterio: servicios finalizados  |  Sentido: descendente (el primero es el mayor)",
                new string[] { "Posición", "Código", "Técnico", "Especialidad", "Servicios finalizados", "Calificación" });

            List<Tecnico> copia = Ordenamientos.Copiar(centro.Tecnicos.Listar());
            if (copia.Count == 0)
            {
                tabla.Resumen = "No hay técnicos registrados.";
                return tabla;
            }
            if (copia.Count >= 2)
            {
                Ordenamientos.SelectionSortPorServicios(copia, true);
            }

            for (int i = 0; i < copia.Count; i++)
            {
                Tecnico t = copia[i];
                tabla.AgregarFila((i + 1).ToString(), t.Codigo, t.NombreCompleto, Textos.NombreEspecialidad(t.Especialidad),
                                  t.ServiciosFinalizados.ToString(), Estrellas(t));
            }

            Tecnico primero = copia[0];
            tabla.Resumen = "El técnico con más servicios finalizados es " + primero.NombreCompleto +
                            " con " + primero.ServiciosFinalizados + " servicio(s).";
            if (copia.Count >= 2 && copia[1].ServiciosFinalizados == primero.ServiciosFinalizados)
            {
                tabla.Resumen += " (Hay empate con " + copia[1].NombreCompleto + ".)";
            }
            return tabla;
        }

        private TablaReporte Reporte5()
        {
            TablaReporte tabla = new TablaReporte(Nombres[4],
                "Datos: arreglo unidimensional de 3 posiciones (se acumula al registrar cada equipo) y lista de solicitudes.",
                new string[] { "Tipo de equipo", "Equipos registrados", "Solicitudes recibidas", "Solicitudes entregadas" });

            int[] registrados = centro.Equipos.ContarPorTipo();
            int[] recibidas = new int[3];
            int[] entregadas = new int[3];

            List<Solicitud> lista = centro.Solicitudes.Listar();
            for (int i = 0; i < lista.Count; i++)
            {
                int tipo = (int)lista[i].Equipo.Tipo;
                recibidas[tipo]++;
                if (lista[i].Estado == EstadoSolicitud.Entregada)
                {
                    entregadas[tipo]++;
                }
            }

            int totalEquipos = 0;
            for (int t = 0; t < 3; t++)
            {
                tabla.AgregarFila(Textos.TiposEquipo[t], registrados[t].ToString(), recibidas[t].ToString(), entregadas[t].ToString());
                totalEquipos += registrados[t];
            }
            tabla.Resumen = "Total de equipos registrados: " + totalEquipos;
            return tabla;
        }

        private TablaReporte Reporte6(bool descendente)
        {
            TablaReporte tabla = new TablaReporte(Nombres[5],
                "Algoritmo: Bubble Sort  |  Criterio: costo total  |  Sentido: " + TextoSentido(descendente),
                new string[] { "Posición", "Código", "Cliente", "Estado", "Mano de obra", "Repuestos", "Total" });

            List<Solicitud> copia = Ordenamientos.Copiar(centro.Solicitudes.Listar());
            try
            {
                Ordenamientos.BubbleSortPorCosto(copia, descendente);
            }
            catch (ReglaNegocioException ex)
            {
                tabla.Resumen = ex.Message;
                return tabla;
            }

            for (int i = 0; i < copia.Count; i++)
            {
                Solicitud s = copia[i];
                tabla.AgregarFila((i + 1).ToString(), s.Codigo, s.Cliente.NombreCompleto, Textos.NombreEstado(s.Estado),
                                  Textos.Moneda(s.CostoManoObra), Textos.Moneda(s.CostoRepuestos), Textos.Moneda(s.Total));
            }
            tabla.Resumen = "Comparaciones: " + Ordenamientos.Comparaciones + "  |  Intercambios: " + Ordenamientos.Intercambios;
            return tabla;
        }

        private TablaReporte Reporte7(bool descendente, CriterioSolicitud criterio)
        {
            TablaReporte tabla = new TablaReporte(Nombres[6],
                "Algoritmo: Quick Sort  |  Criterio: " + Ordenamientos.NombreCriterio(criterio) + "  |  Sentido: " + TextoSentido(descendente),
                new string[] { "Posición", "Código", "Cliente", "Fecha de recepción", "Tiempo estimado", "Estado" });

            List<Solicitud> copia = Ordenamientos.Copiar(centro.Solicitudes.Listar());
            try
            {
                Ordenamientos.QuickSortSolicitudes(copia, criterio, descendente);
            }
            catch (ReglaNegocioException ex)
            {
                tabla.Resumen = ex.Message;
                return tabla;
            }

            for (int i = 0; i < copia.Count; i++)
            {
                Solicitud s = copia[i];
                tabla.AgregarFila((i + 1).ToString(), s.Codigo, s.Cliente.NombreCompleto,
                                  s.FechaRecepcion.ToString("dd/MM/yyyy HH:mm"), s.TiempoEstimadoHoras + " h",
                                  Textos.NombreEstado(s.Estado));
            }
            tabla.Resumen = "Comparaciones: " + Ordenamientos.Comparaciones + "  |  Intercambios: " + Ordenamientos.Intercambios;
            return tabla;
        }

        private TablaReporte Reporte8(bool descendente)
        {
            TablaReporte tabla = new TablaReporte(Nombres[7],
                "Algoritmo: Shell Sort  |  Criterio: existencia disponible  |  Sentido: " + TextoSentido(descendente),
                new string[] { "Posición", "Código", "Repuesto", "Categoría", "Existencia", "Mínimo", "Estado" });

            List<Repuesto> copia = Ordenamientos.Copiar(centro.Repuestos.Listar());
            try
            {
                Ordenamientos.ShellSortPorExistencia(copia, descendente);
            }
            catch (ReglaNegocioException ex)
            {
                tabla.Resumen = ex.Message;
                return tabla;
            }

            for (int i = 0; i < copia.Count; i++)
            {
                Repuesto r = copia[i];
                tabla.AgregarFila((i + 1).ToString(), r.Codigo, r.Nombre, Textos.NombreCategoria(r.Categoria),
                                  r.Existencia.ToString(), r.ExistenciaMinima.ToString(), r.TextoEstado);
            }
            tabla.Resumen = "Comparaciones: " + Ordenamientos.Comparaciones + "  |  Intercambios: " + Ordenamientos.Intercambios;
            return tabla;
        }

        private TablaReporte Reporte9()
        {
            TablaReporte tabla = new TablaReporte(Nombres[8],
                "Datos: lista de repuestos. Se usa EnAlerta(): existencia menor o igual a la existencia mínima.",
                new string[] { "Código", "Repuesto", "Existencia", "Mínimo", "Faltan para el mínimo", "Estado" });

            List<Repuesto> alerta = centro.Repuestos.BajoMinimo();
            int agotados = 0;
            for (int i = 0; i < alerta.Count; i++)
            {
                Repuesto r = alerta[i];
                int faltan = r.ExistenciaMinima - r.Existencia;
                if (faltan < 0)
                {
                    faltan = 0;
                }
                if (r.EstaAgotado())
                {
                    agotados++;
                }
                tabla.AgregarFila(r.Codigo, r.Nombre, r.Existencia.ToString(), r.ExistenciaMinima.ToString(),
                                  faltan.ToString(), r.TextoEstado);
            }
            tabla.Resumen = "Repuestos en alerta: " + alerta.Count + "  |  Agotados: " + agotados;
            return tabla;
        }

        private TablaReporte Reporte10()
        {
            TablaReporte tabla = new TablaReporte(Nombres[9],
                "Datos: solicitudes en estado Entregada. Total = mano de obra + repuestos.",
                new string[] { "Código", "Cliente", "Técnico", "Fecha de entrega", "Mano de obra", "Repuestos", "Total" });

            List<Solicitud> lista = centro.Solicitudes.Listar();
            decimal totalManoObra = 0;
            decimal totalRepuestos = 0;
            int cantidad = 0;

            for (int i = 0; i < lista.Count; i++)
            {
                Solicitud s = lista[i];
                if (s.Estado == EstadoSolicitud.Entregada)
                {
                    tabla.AgregarFila(s.Codigo, s.Cliente.NombreCompleto, NombreTecnico(s.TecnicoAsignado),
                                      s.FechaCierre.ToString("dd/MM/yyyy HH:mm"), Textos.Moneda(s.CostoManoObra),
                                      Textos.Moneda(s.CostoRepuestos), Textos.Moneda(s.Total));
                    totalManoObra += s.CostoManoObra;
                    totalRepuestos += s.CostoRepuestos;
                    cantidad++;
                }
            }
            tabla.Resumen = "Servicios finalizados: " + cantidad + "  |  Mano de obra: " + Textos.Moneda(totalManoObra) +
                            "  |  Repuestos: " + Textos.Moneda(totalRepuestos) +
                            "  |  INGRESOS TOTALES: " + Textos.Moneda(totalManoObra + totalRepuestos);
            return tabla;
        }

        private TablaReporte Reporte11()
        {
            TablaReporte tabla = new TablaReporte(Nombres[10],
                "Datos: búsquedas realizadas durante esta sesión (secuencial, binaria y multicriterio).",
                new string[] { "Hora", "Tipo", "Búsqueda", "Coincidencias", "Comparaciones", "Resultado" });

            List<ResultadoBusqueda> registro = centro.RegistroBusquedas;
            int secuenciales = 0;
            int binarias = 0;
            int multicriterio = 0;

            for (int i = 0; i < registro.Count; i++)
            {
                ResultadoBusqueda r = registro[i];
                tabla.AgregarFila(r.Hora.ToString("HH:mm:ss"), r.Tipo, r.Termino, r.Coincidencias.ToString(),
                                  r.Comparaciones.ToString(), r.Detalle);
                if (r.Tipo == "Secuencial")
                {
                    secuenciales++;
                }
                else if (r.Tipo == "Binaria")
                {
                    binarias++;
                }
                else
                {
                    multicriterio++;
                }
            }

            if (registro.Count == 0)
            {
                tabla.Resumen = "Todavía no se ha hecho ninguna búsqueda en esta sesión. Use el módulo Búsquedas.";
            }
            else
            {
                tabla.Resumen = "Secuenciales: " + secuenciales + "  |  Binarias: " + binarias + "  |  Multicriterio: " + multicriterio;
            }
            return tabla;
        }

        private TablaReporte Reporte12()
        {
            string[] columnas = new string[MatrizCarga.DIAS + 2];
            columnas[0] = "Técnico";
            for (int d = 0; d < MatrizCarga.DIAS; d++)
            {
                columnas[d + 1] = MatrizCarga.DiasCortos[d];
            }
            columnas[MatrizCarga.DIAS + 1] = "Total";

            TablaReporte tabla = new TablaReporte(Nombres[11],
                "Datos: arreglo bidimensional (filas = técnicos, columnas = días). Se recorre por filas y columnas.",
                columnas);

            MatrizCarga matriz = centro.Matriz;
            List<Tecnico> tecnicos = centro.Tecnicos.Listar();

            for (int f = 0; f < matriz.Filas; f++)
            {
                string[] fila = new string[MatrizCarga.DIAS + 2];
                fila[0] = tecnicos[f].NombreCompleto;
                for (int d = 0; d < MatrizCarga.DIAS; d++)
                {
                    fila[d + 1] = matriz.Valor(f, d).ToString();
                }
                fila[MatrizCarga.DIAS + 1] = matriz.TotalTecnico(f).ToString();
                tabla.AgregarFila(fila);
            }

            tabla.Resumen = TextoMayorCarga(matriz);
            return tabla;
        }

        public static string TextoMayorCarga(MatrizCarga matriz)
        {
            Tecnico tecnicoMayor = matriz.TecnicoConMayorCarga();
            int diaMayor = matriz.DiaConMayorCarga();
            if (tecnicoMayor == null || diaMayor == -1)
            {
                return "Todavía no hay asignaciones registradas en la matriz.";
            }
            return "Técnico con mayor carga: " + tecnicoMayor.NombreCompleto +
                   "  |  Día con mayor carga: " + MatrizCarga.NombresDias[diaMayor] +
                   " (" + matriz.TotalDia(diaMayor) + " asignaciones)";
        }

        private static string TextoSentido(bool descendente)
        {
            if (descendente)
            {
                return "descendente";
            }
            return "ascendente";
        }

        private static string NombreTecnico(Tecnico tecnico)
        {
            if (tecnico == null)
            {
                return "-";
            }
            return tecnico.NombreCompleto;
        }

        private static string Estrellas(Tecnico t)
        {
            if (t.TotalCalificaciones == 0)
            {
                return "Sin calificar";
            }
            return t.CalificacionPromedio.ToString("0.0") + " (" + t.TotalCalificaciones + " calif.)";
        }

        public static string TextoEspera(DateTime desde)
        {
            TimeSpan espera = DateTime.Now - desde;
            if (espera.TotalDays >= 1)
            {
                return (int)espera.TotalDays + "d " + espera.Hours + "h";
            }
            if (espera.TotalHours >= 1)
            {
                return (int)espera.TotalHours + "h " + espera.Minutes + "m";
            }
            return espera.Minutes + "m";
        }
    }
}
