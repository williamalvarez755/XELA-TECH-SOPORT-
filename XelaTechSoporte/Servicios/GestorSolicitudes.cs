using XelaTechSoporte.Estructuras;
using XelaTechSoporte.Excepciones;
using XelaTechSoporte.Modelos;

namespace XelaTechSoporte.Servicios
{
    public class GestorSolicitudes
    {
        private const int HORAS_MAXIMAS = 200;

        private List<Solicitud> solicitudes;
        private ColaAtencion cola;
        private MatrizCarga matriz;
        private GestorClientes gestorClientes;
        private GestorEquipos gestorEquipos;
        private GestorTecnicos gestorTecnicos;

        public GestorSolicitudes(List<Solicitud> solicitudes, ColaAtencion cola, MatrizCarga matriz,
                                 GestorClientes gestorClientes, GestorEquipos gestorEquipos, GestorTecnicos gestorTecnicos)
        {
            this.solicitudes = solicitudes;
            this.cola = cola;
            this.matriz = matriz;
            this.gestorClientes = gestorClientes;
            this.gestorEquipos = gestorEquipos;
            this.gestorTecnicos = gestorTecnicos;
        }

        public Solicitud Crear(Cliente cliente, Equipo equipo, string descripcion, NivelPrioridad prioridad, Especialidad especialidad)
        {
            return Crear(cliente, equipo, descripcion, prioridad, especialidad, DateTime.Now);
        }

        public Solicitud Crear(Cliente cliente, Equipo equipo, string descripcion, NivelPrioridad prioridad,
                               Especialidad especialidad, DateTime fechaRecepcion)
        {
            if (cliente == null || gestorClientes.BuscarPorCodigo(cliente.Codigo) == null)
            {
                throw new ReglaNegocioException("El cliente seleccionado no existe. Regístrelo primero.");
            }
            if (equipo == null || gestorEquipos.BuscarPorCodigo(equipo.CodigoInterno) == null)
            {
                throw new ReglaNegocioException("El equipo seleccionado no existe. Regístrelo primero.");
            }
            if (equipo.Propietario != cliente)
            {
                throw new ReglaNegocioException("El equipo " + equipo.CodigoInterno + " no pertenece a " + cliente.NombreCompleto + ".");
            }

            Solicitud activa = BuscarActivaDeEquipo(equipo);
            if (activa != null)
            {
                throw new ReglaNegocioException("El equipo " + equipo.CodigoInterno + " ya tiene la solicitud activa " +
                    activa.Codigo + " (" + Textos.NombreEstado(activa.Estado) + ").");
            }

            Validador.ExigirTexto(descripcion, "Descripción del problema");
            if (!Validador.LongitudMinima(descripcion, 10))
            {
                throw new ReglaNegocioException("La descripción del problema debe tener al menos 10 caracteres.");
            }
            if (!Validador.FechaNoFutura(fechaRecepcion))
            {
                throw new ReglaNegocioException("La fecha de recepción no puede ser una fecha futura.");
            }

            string codigo = GeneradorCodigos.Siguiente("SOL");
            if (BuscarPorCodigo(codigo) != null)
            {
                throw new CodigoDuplicadoException("el código", codigo);
            }

            Solicitud nueva = new Solicitud(codigo, cliente, equipo, descripcion, prioridad, especialidad, fechaRecepcion);
            solicitudes.Add(nueva);
            cliente.SumarSolicitud();

            cola.Encolar(nueva);
            nueva.CambiarEstado(EstadoSolicitud.EnEspera);
            return nueva;
        }

        public Solicitud BuscarPorCodigo(string codigo)
        {
            if (codigo == null)
            {
                return null;
            }
            string buscado = codigo.Trim().ToUpper();
            for (int i = 0; i < solicitudes.Count; i++)
            {
                if (solicitudes[i].Codigo == buscado)
                {
                    return solicitudes[i];
                }
            }
            return null;
        }

        public Solicitud BuscarActivaDeEquipo(Equipo equipo)
        {
            for (int i = 0; i < solicitudes.Count; i++)
            {
                if (solicitudes[i].Equipo == equipo && solicitudes[i].EstaActiva)
                {
                    return solicitudes[i];
                }
            }
            return null;
        }

        public List<Solicitud> Listar()
        {
            return solicitudes;
        }

        public Solicitud VerSiguienteEnCola()
        {
            return cola.VerSiguiente();
        }

        public Solicitud AsignarSiguiente(Tecnico tecnico)
        {
            return AsignarSiguiente(tecnico, MatrizCarga.DiaDeFecha(DateTime.Now));
        }

        public Solicitud AsignarSiguiente(Tecnico tecnico, int dia)
        {
            Solicitud enTurno = cola.VerSiguiente();
            if (enTurno == null)
            {
                throw new ReglaNegocioException("No hay solicitudes en espera.");
            }
            if (tecnico == null)
            {
                throw new ReglaNegocioException("Seleccione un técnico.");
            }
            if (tecnico.Especialidad != enTurno.EspecialidadRequerida)
            {
                throw new ReglaNegocioException("El técnico " + tecnico.NombreCompleto + " es de " +
                    Textos.NombreEspecialidad(tecnico.Especialidad) + " y la solicitud " + enTurno.Codigo +
                    " requiere " + Textos.NombreEspecialidad(enTurno.EspecialidadRequerida) + ".");
            }

            tecnico.AsignarTrabajo();

            cola.AtenderSiguiente();
            enTurno.CambiarEstado(EstadoSolicitud.Asignada);
            enTurno.AsignarTecnico(tecnico, dia);
            matriz.Registrar(tecnico, dia);
            return enTurno;
        }

        public void ValidarDiagnostico(Solicitud solicitud)
        {
            ValidarModificable(solicitud);
            if (solicitud.TecnicoAsignado == null)
            {
                throw new ReglaNegocioException("Primero debe asignarse un técnico a la solicitud (módulo Atención de colas).");
            }
            if (solicitud.Estado != EstadoSolicitud.Asignada &&
                solicitud.Estado != EstadoSolicitud.EnDiagnostico &&
                solicitud.Estado != EstadoSolicitud.EnReparacion)
            {
                throw new ReglaNegocioException("El diagnóstico solo se registra cuando la solicitud está asignada, en diagnóstico o en reparación.");
            }
        }

        public void RegistrarDiagnostico(Solicitud solicitud, string descripcion, List<string> acciones, int horasEstimadas)
        {
            ValidarDiagnostico(solicitud);
            Validador.ExigirTexto(descripcion, "Descripción del diagnóstico");
            if (horasEstimadas < 1 || horasEstimadas > HORAS_MAXIMAS)
            {
                throw new ReglaNegocioException("El tiempo estimado debe estar entre 1 y " + HORAS_MAXIMAS + " horas.");
            }

            Diagnostico diagnostico = new Diagnostico(GeneradorCodigos.Siguiente("DIA"), descripcion,
                                                      horasEstimadas, solicitud.TecnicoAsignado);
            if (acciones != null)
            {
                for (int i = 0; i < acciones.Count; i++)
                {
                    diagnostico.AgregarAccion(acciones[i]);
                }
            }
            solicitud.RegistrarDiagnostico(diagnostico);

            if (solicitud.Estado == EstadoSolicitud.Asignada)
            {
                solicitud.CambiarEstado(EstadoSolicitud.EnDiagnostico);
            }
        }

        public void ValidarRepuestos(Solicitud solicitud)
        {
            ValidarModificable(solicitud);
            if (solicitud.Estado != EstadoSolicitud.EnDiagnostico && solicitud.Estado != EstadoSolicitud.EnReparacion)
            {
                throw new ReglaNegocioException("Los repuestos se registran cuando la solicitud está en diagnóstico o en reparación.");
            }
        }

        public bool AgregarRepuesto(Solicitud solicitud, Repuesto repuesto, int cantidad)
        {
            ValidarRepuestos(solicitud);
            if (repuesto == null)
            {
                throw new ReglaNegocioException("Seleccione un repuesto.");
            }
            if (cantidad <= 0)
            {
                throw new ReglaNegocioException("La cantidad debe ser mayor que cero.");
            }

            solicitud.AgregarRepuesto(repuesto, cantidad);
            return repuesto.EnAlerta();
        }

        public void CambiarEstado(Solicitud solicitud, EstadoSolicitud nuevoEstado)
        {
            ValidarModificable(solicitud);
            if (!MaquinaEstados.EsValida(solicitud.Estado, nuevoEstado))
            {
                throw new TransicionInvalidaException(solicitud.Estado, nuevoEstado);
            }

            switch (nuevoEstado)
            {
                case EstadoSolicitud.Asignada:
                    throw new ReglaNegocioException("Para asignar un técnico use el módulo \"Atención de colas\"; así se respeta el orden de llegada.");

                case EstadoSolicitud.EnDiagnostico:
                    if (solicitud.Diagnostico == null)
                    {
                        throw new ReglaNegocioException("Registre el diagnóstico con el botón \"Diagnóstico\"; la solicitud pasará sola a \"En diagnóstico\".");
                    }
                    solicitud.CambiarEstado(nuevoEstado);
                    break;

                case EstadoSolicitud.EnEspera:
                    if (solicitud.Estado == EstadoSolicitud.Asignada)
                    {
                        DevolverACola(solicitud);
                    }
                    else
                    {
                        cola.Encolar(solicitud);
                        solicitud.CambiarEstado(nuevoEstado);
                    }
                    break;

                case EstadoSolicitud.Entregada:
                    throw new ReglaNegocioException("Para entregar use el botón \"Entregar\", ahí se registra el cobro.");

                case EstadoSolicitud.Cancelada:
                    Cancelar(solicitud);
                    break;

                default:
                    solicitud.CambiarEstado(nuevoEstado);
                    break;
            }
        }

        private void DevolverACola(Solicitud solicitud)
        {
            solicitud.CambiarEstado(EstadoSolicitud.EnEspera);
            LiberarTecnico(solicitud);
            cola.Encolar(solicitud);
        }

        public EstadoSolicitud Deshacer(Solicitud solicitud)
        {
            ValidarModificable(solicitud);
            if (solicitud.CantidadHistorial == 0)
            {
                throw new HistorialVacioException(solicitud.Codigo);
            }

            EstadoSolicitud actual = solicitud.Estado;
            EstadoSolicitud anterior = solicitud.VerEstadoAnterior();

            if (anterior == EstadoSolicitud.Asignada && solicitud.TecnicoAsignado == null)
            {
                throw new ReglaNegocioException("No se puede deshacer: la solicitud ya no tiene técnico. Asígnela de nuevo desde Atención de colas.");
            }

            solicitud.DeshacerEstado();

            if (actual == EstadoSolicitud.Asignada && solicitud.Estado == EstadoSolicitud.EnEspera)
            {
                LiberarTecnico(solicitud);
                cola.Encolar(solicitud);
            }
            else if (actual == EstadoSolicitud.EnEspera && solicitud.Estado == EstadoSolicitud.Recibida)
            {
                cola.Quitar(solicitud);
            }
            return solicitud.Estado;
        }

        public void ValidarEntrega(Solicitud solicitud)
        {
            ValidarModificable(solicitud);
            if (solicitud.Estado != EstadoSolicitud.ListaParaEntregar)
            {
                throw new TransicionInvalidaException(solicitud.Estado, EstadoSolicitud.Entregada);
            }
            if (solicitud.Diagnostico == null)
            {
                throw new ReglaNegocioException("No se puede entregar la solicitud " + solicitud.Codigo + " sin un diagnóstico registrado.");
            }
        }

        public decimal Entregar(Solicitud solicitud, decimal manoObra, int calificacion)
        {
            ValidarEntrega(solicitud);
            Validador.ExigirNoNegativo(manoObra, "Costo de mano de obra");
            if (calificacion != 0 && !Validador.RangoCalificacion(calificacion))
            {
                throw new ReglaNegocioException("La calificación debe estar entre 1 y 5 estrellas.");
            }

            solicitud.FijarManoObra(manoObra);
            solicitud.CambiarEstado(EstadoSolicitud.Entregada);
            solicitud.TecnicoAsignado.FinalizarTrabajo();
            if (calificacion > 0)
            {
                solicitud.TecnicoAsignado.Calificar(calificacion);
            }
            solicitud.Cerrar(DateTime.Now);
            return solicitud.Total;
        }

        public void Cancelar(Solicitud solicitud)
        {
            ValidarModificable(solicitud);
            solicitud.CambiarEstado(EstadoSolicitud.Cancelada);

            for (int i = 0; i < solicitud.Repuestos.Count; i++)
            {
                DetalleRepuesto detalle = solicitud.Repuestos[i];
                detalle.Repuesto.Devolver(detalle.Cantidad);
            }
            if (solicitud.EnCola)
            {
                cola.Quitar(solicitud);
            }
            if (solicitud.TecnicoAsignado != null)
            {
                solicitud.TecnicoAsignado.LiberarTrabajo();
            }
            solicitud.Cerrar(DateTime.Now);
        }

        public int RevisarEscalamientos()
        {
            return cola.RevisarEscalamientos(DateTime.Now);
        }

        public void ValidarModificable(Solicitud solicitud)
        {
            if (solicitud == null)
            {
                throw new ReglaNegocioException("Seleccione una solicitud.");
            }
            if (solicitud.Estado == EstadoSolicitud.Entregada)
            {
                throw new ReglaNegocioException("La solicitud " + solicitud.Codigo + " ya fue entregada; no se puede modificar ni cancelar.");
            }
            if (solicitud.Estado == EstadoSolicitud.Cancelada)
            {
                throw new ReglaNegocioException("La solicitud " + solicitud.Codigo + " está cancelada; no se puede modificar.");
            }
        }

        private void LiberarTecnico(Solicitud solicitud)
        {
            Tecnico tecnico = solicitud.TecnicoAsignado;
            if (tecnico == null)
            {
                return;
            }
            tecnico.LiberarTrabajo();
            matriz.Quitar(tecnico, solicitud.DiaAsignacion);
            solicitud.QuitarTecnico();
        }
    }
}
