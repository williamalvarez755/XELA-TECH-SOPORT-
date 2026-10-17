using XelaTechSoporte.Excepciones;
using XelaTechSoporte.Servicios;

namespace XelaTechSoporte.Modelos
{
    public class Tecnico : Persona
    {
        public const int LIMITE_TRABAJOS = 3;

        private Especialidad especialidad;
        private bool disponible;
        private int trabajosActivos;
        private int serviciosFinalizados;

        private int[] calificaciones = new int[5];

        public Tecnico(string codigo, string nombreCompleto, string telefono, string correo, Especialidad especialidad)
            : base(codigo, nombreCompleto, telefono, correo)
        {
            this.especialidad = especialidad;
            disponible = true;
            trabajosActivos = 0;
            serviciosFinalizados = 0;
        }

        public Especialidad Especialidad
        {
            get { return especialidad; }
            set { especialidad = value; }
        }

        public bool Disponible
        {
            get { return disponible; }
            set { disponible = value; }
        }

        public int TrabajosActivos
        {
            get { return trabajosActivos; }
        }

        public int ServiciosFinalizados
        {
            get { return serviciosFinalizados; }
        }

        public bool TieneCargaCompleta
        {
            get { return trabajosActivos >= LIMITE_TRABAJOS; }
        }

        public bool PuedeRecibirTrabajo()
        {
            return disponible && trabajosActivos < LIMITE_TRABAJOS;
        }

        public void AsignarTrabajo()
        {
            if (!disponible)
            {
                throw new ReglaNegocioException("El técnico " + nombreCompleto + " no está disponible.");
            }
            if (trabajosActivos >= LIMITE_TRABAJOS)
            {
                throw new LimiteTrabajosException(nombreCompleto, trabajosActivos, LIMITE_TRABAJOS);
            }
            trabajosActivos++;
        }

        public void FinalizarTrabajo()
        {
            if (trabajosActivos > 0)
            {
                trabajosActivos--;
            }
            serviciosFinalizados++;
        }

        public void LiberarTrabajo()
        {
            if (trabajosActivos > 0)
            {
                trabajosActivos--;
            }
        }

        public void Calificar(int estrellas)
        {
            if (!Validador.RangoCalificacion(estrellas))
            {
                throw new ReglaNegocioException("La calificación debe estar entre 1 y 5 estrellas.");
            }
            calificaciones[estrellas - 1]++;
        }

        public int CantidadCalificaciones(int estrellas)
        {
            return calificaciones[estrellas - 1];
        }

        public int TotalCalificaciones
        {
            get
            {
                int total = 0;
                for (int i = 0; i < calificaciones.Length; i++)
                {
                    total += calificaciones[i];
                }
                return total;
            }
        }

        public double CalificacionPromedio
        {
            get
            {
                int cantidad = 0;
                int sumaEstrellas = 0;
                for (int i = 0; i < calificaciones.Length; i++)
                {
                    cantidad += calificaciones[i];
                    sumaEstrellas += calificaciones[i] * (i + 1);
                }
                if (cantidad == 0)
                {
                    return 0;
                }
                return (double)sumaEstrellas / cantidad;
            }
        }

        public string TextoEstado
        {
            get
            {
                if (!disponible)
                {
                    return "No disponible";
                }
                if (TieneCargaCompleta)
                {
                    return "Carga completa";
                }
                return "Disponible";
            }
        }

        public override string ObtenerResumen()
        {
            return base.ObtenerResumen() + " | " + Textos.NombreEspecialidad(especialidad)
                + " | Activos: " + trabajosActivos + "/" + LIMITE_TRABAJOS
                + " | Finalizados: " + serviciosFinalizados
                + " | Promedio: " + CalificacionPromedio.ToString("0.0");
        }
    }
}
