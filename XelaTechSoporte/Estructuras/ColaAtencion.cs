using XelaTechSoporte.Excepciones;
using XelaTechSoporte.Modelos;

namespace XelaTechSoporte.Estructuras
{
    public class ColaAtencion
    {
        public const int HORAS_ESCALAMIENTO = 48;

        private Queue<Solicitud> colaUrgente;
        private Queue<Solicitud> colaNormal;

        public ColaAtencion()
        {
            colaUrgente = new Queue<Solicitud>();
            colaNormal = new Queue<Solicitud>();
        }

        public int CantidadUrgentes
        {
            get { return colaUrgente.Count; }
        }

        public int CantidadNormales
        {
            get { return colaNormal.Count; }
        }

        public bool HayPendientes
        {
            get { return colaUrgente.Count + colaNormal.Count > 0; }
        }

        public void Encolar(Solicitud solicitud)
        {
            if (solicitud == null)
            {
                throw new ReglaNegocioException("No se puede encolar una solicitud vacía.");
            }
            if (solicitud.EnCola)
            {
                throw new ReglaNegocioException("La solicitud " + solicitud.Codigo + " ya está en la cola.");
            }

            if (solicitud.Prioridad == NivelPrioridad.Urgente)
            {
                colaUrgente.Enqueue(solicitud);
            }
            else
            {
                colaNormal.Enqueue(solicitud);
            }
            solicitud.EnCola = true;
        }

        public Solicitud VerSiguiente()
        {
            if (colaUrgente.Count > 0)
            {
                return colaUrgente.Peek();
            }
            if (colaNormal.Count > 0)
            {
                return colaNormal.Peek();
            }
            return null;
        }

        public Solicitud AtenderSiguiente()
        {
            Solicitud siguiente;
            if (colaUrgente.Count > 0)
            {
                siguiente = colaUrgente.Dequeue();
            }
            else if (colaNormal.Count > 0)
            {
                siguiente = colaNormal.Dequeue();
            }
            else
            {
                throw new ReglaNegocioException("No hay solicitudes en espera.");
            }
            siguiente.EnCola = false;
            return siguiente;
        }

        public Solicitud[] VerEnEspera(NivelPrioridad prioridad)
        {
            if (prioridad == NivelPrioridad.Urgente)
            {
                return colaUrgente.ToArray();
            }
            return colaNormal.ToArray();
        }

        public int PosicionEnCola(Solicitud solicitud)
        {
            Solicitud[] arreglo = VerEnEspera(solicitud.Prioridad);
            for (int i = 0; i < arreglo.Length; i++)
            {
                if (arreglo[i] == solicitud)
                {
                    return i + 1;
                }
            }
            return 0;
        }

        public int RevisarEscalamientos(DateTime ahora)
        {
            int escaladas = 0;
            int cantidad = colaNormal.Count;

            for (int i = 0; i < cantidad; i++)
            {
                Solicitud solicitud = colaNormal.Dequeue();
                TimeSpan espera = ahora - solicitud.FechaRecepcion;

                if (espera.TotalHours >= HORAS_ESCALAMIENTO)
                {
                    solicitud.Escalar();
                    colaUrgente.Enqueue(solicitud);
                    escaladas++;
                }
                else
                {
                    colaNormal.Enqueue(solicitud);
                }
            }
            return escaladas;
        }

        public bool Quitar(Solicitud solicitud)
        {
            bool encontrada = QuitarDe(colaUrgente, solicitud) || QuitarDe(colaNormal, solicitud);
            if (encontrada)
            {
                solicitud.EnCola = false;
            }
            return encontrada;
        }

        private bool QuitarDe(Queue<Solicitud> cola, Solicitud buscada)
        {
            bool encontrada = false;
            int cantidad = cola.Count;
            for (int i = 0; i < cantidad; i++)
            {
                Solicitud actual = cola.Dequeue();
                if (actual == buscada)
                {
                    encontrada = true;
                }
                else
                {
                    cola.Enqueue(actual);
                }
            }
            return encontrada;
        }

        public void Vaciar()
        {
            while (colaUrgente.Count > 0)
            {
                colaUrgente.Dequeue().EnCola = false;
            }
            while (colaNormal.Count > 0)
            {
                colaNormal.Dequeue().EnCola = false;
            }
        }
    }
}
