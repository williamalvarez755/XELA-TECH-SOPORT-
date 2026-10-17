using XelaTechSoporte.Estructuras;
using XelaTechSoporte.Excepciones;
using XelaTechSoporte.Servicios;

namespace XelaTechSoporte.Modelos
{
    public class Solicitud
    {
        private string codigo;
        private Cliente cliente;
        private Equipo equipo;
        private string descripcionProblema;
        private DateTime fechaRecepcion;
        private NivelPrioridad prioridad;
        private EstadoSolicitud estado;
        private Especialidad especialidadRequerida;
        private Tecnico tecnicoAsignado;
        private int diaAsignacion;
        private Diagnostico diagnostico;
        private List<DetalleRepuesto> repuestos;
        private decimal costoManoObra;
        private int tiempoEstimadoHoras;
        private Stack<EstadoSolicitud> historial;
        private bool enCola;
        private bool escalada;
        private DateTime fechaCierre;

        public Solicitud(string codigo, Cliente cliente, Equipo equipo, string descripcionProblema,
                         NivelPrioridad prioridad, Especialidad especialidadRequerida)
            : this(codigo, cliente, equipo, descripcionProblema, prioridad, especialidadRequerida, DateTime.Now)
        {
        }

        public Solicitud(string codigo, Cliente cliente, Equipo equipo, string descripcionProblema,
                         NivelPrioridad prioridad, Especialidad especialidadRequerida, DateTime fechaRecepcion)
        {
            Validador.ExigirTexto(codigo, "Código");
            if (cliente == null)
            {
                throw new ReglaNegocioException("La solicitud necesita un cliente.");
            }
            if (equipo == null)
            {
                throw new ReglaNegocioException("La solicitud necesita un equipo.");
            }
            Validador.ExigirTexto(descripcionProblema, "Descripción del problema");
            if (!Validador.LongitudMinima(descripcionProblema, 10))
            {
                throw new ReglaNegocioException("La descripción del problema debe tener al menos 10 caracteres.");
            }
            if (!Validador.FechaNoFutura(fechaRecepcion))
            {
                throw new ReglaNegocioException("La fecha de recepción no puede ser una fecha futura.");
            }

            this.codigo = codigo;
            this.cliente = cliente;
            this.equipo = equipo;
            this.descripcionProblema = descripcionProblema.Trim();
            this.prioridad = prioridad;
            this.especialidadRequerida = especialidadRequerida;
            this.fechaRecepcion = fechaRecepcion;

            estado = EstadoSolicitud.Recibida;
            historial = new Stack<EstadoSolicitud>();
            repuestos = new List<DetalleRepuesto>();
            tecnicoAsignado = null;
            diagnostico = null;
            enCola = false;
            escalada = false;
            fechaCierre = DateTime.MinValue;

            decimal revision = equipo.CalcularCostoRevision();
            costoManoObra = Math.Round(revision - revision * cliente.ObtenerDescuento(), 2);
            tiempoEstimadoHoras = equipo.HorasEstimadasBase();
        }

        public string Codigo
        {
            get { return codigo; }
        }

        public Cliente Cliente
        {
            get { return cliente; }
        }

        public Equipo Equipo
        {
            get { return equipo; }
        }

        public string DescripcionProblema
        {
            get { return descripcionProblema; }
        }

        public DateTime FechaRecepcion
        {
            get { return fechaRecepcion; }
        }

        public NivelPrioridad Prioridad
        {
            get { return prioridad; }
        }

        public EstadoSolicitud Estado
        {
            get { return estado; }
        }

        public Especialidad EspecialidadRequerida
        {
            get { return especialidadRequerida; }
        }

        public Tecnico TecnicoAsignado
        {
            get { return tecnicoAsignado; }
        }

        public int DiaAsignacion
        {
            get { return diaAsignacion; }
        }

        public Diagnostico Diagnostico
        {
            get { return diagnostico; }
        }

        public List<DetalleRepuesto> Repuestos
        {
            get { return repuestos; }
        }

        public decimal CostoManoObra
        {
            get { return costoManoObra; }
        }

        public int TiempoEstimadoHoras
        {
            get { return tiempoEstimadoHoras; }
        }

        public bool EnCola
        {
            get { return enCola; }
            set { enCola = value; }
        }

        public bool Escalada
        {
            get { return escalada; }
        }

        public DateTime FechaCierre
        {
            get { return fechaCierre; }
        }

        public decimal CostoRepuestos
        {
            get
            {
                decimal suma = 0;
                for (int i = 0; i < repuestos.Count; i++)
                {
                    suma += repuestos[i].Subtotal;
                }
                return suma;
            }
        }

        public decimal Total
        {
            get { return costoManoObra + CostoRepuestos; }
        }

        public bool EstaActiva
        {
            get { return estado != EstadoSolicitud.Entregada && estado != EstadoSolicitud.Cancelada; }
        }

        public int CantidadHistorial
        {
            get { return historial.Count; }
        }

        public bool PuedeDeshacer
        {
            get { return EstaActiva && historial.Count > 0; }
        }

        public void CambiarEstado(EstadoSolicitud nuevoEstado)
        {
            if (!MaquinaEstados.EsValida(estado, nuevoEstado))
            {
                throw new TransicionInvalidaException(estado, nuevoEstado);
            }
            historial.Push(estado);
            estado = nuevoEstado;
        }

        public EstadoSolicitud VerEstadoAnterior()
        {
            if (historial.Count == 0)
            {
                throw new HistorialVacioException(codigo);
            }
            return historial.Peek();
        }

        public void DeshacerEstado()
        {
            if (estado == EstadoSolicitud.Entregada || estado == EstadoSolicitud.Cancelada)
            {
                throw new ReglaNegocioException("La solicitud " + codigo + " ya fue " +
                    Textos.NombreEstado(estado).ToLower() + "; no se puede deshacer.");
            }
            if (historial.Count == 0)
            {
                throw new HistorialVacioException(codigo);
            }
            estado = historial.Pop();
        }

        public EstadoSolicitud[] ObtenerHistorial()
        {
            return historial.ToArray();
        }

        public void AgregarRepuesto(Repuesto repuesto, int cantidad)
        {
            if (!EstaActiva)
            {
                throw new ReglaNegocioException("No se pueden agregar repuestos a una solicitud cerrada.");
            }
            if (repuesto == null)
            {
                throw new ReglaNegocioException("Seleccione un repuesto.");
            }

            repuesto.Descontar(cantidad);

            for (int i = 0; i < repuestos.Count; i++)
            {
                if (repuestos[i].Repuesto == repuesto)
                {
                    repuestos[i].SumarCantidad(cantidad);
                    return;
                }
            }
            repuestos.Add(new DetalleRepuesto(repuesto, cantidad, repuesto.PrecioVenta));
        }

        public void AgregarRepuesto(Repuesto repuesto)
        {
            AgregarRepuesto(repuesto, 1);
        }

        public void AsignarTecnico(Tecnico tecnico, int dia)
        {
            tecnicoAsignado = tecnico;
            diaAsignacion = dia;
        }

        public void QuitarTecnico()
        {
            tecnicoAsignado = null;
        }

        public void RegistrarDiagnostico(Diagnostico nuevoDiagnostico)
        {
            diagnostico = nuevoDiagnostico;
            tiempoEstimadoHoras = nuevoDiagnostico.TiempoEstimadoHoras;
        }

        public void FijarManoObra(decimal monto)
        {
            Validador.ExigirNoNegativo(monto, "Costo de mano de obra");
            costoManoObra = monto;
        }

        public void Escalar()
        {
            prioridad = NivelPrioridad.Urgente;
            escalada = true;
        }

        public void Cerrar(DateTime fecha)
        {
            fechaCierre = fecha;
        }

        public override string ToString()
        {
            return codigo + " - " + cliente.NombreCompleto + " - " + Textos.NombreEstado(estado);
        }
    }
}
