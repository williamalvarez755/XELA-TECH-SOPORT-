using XelaTechSoporte.Estructuras;
using XelaTechSoporte.Excepciones;
using XelaTechSoporte.Modelos;

namespace XelaTechSoporte.Servicios
{
    public class GestorTecnicos
    {
        private List<Tecnico> tecnicos;
        private MatrizCarga matriz;

        public GestorTecnicos(List<Tecnico> tecnicos, MatrizCarga matriz)
        {
            this.tecnicos = tecnicos;
            this.matriz = matriz;
        }

        public Tecnico Registrar(string nombre, string telefono, string correo, Especialidad especialidad)
        {
            Validador.ValidarDatosPersona(nombre, telefono, correo);

            string codigo = GeneradorCodigos.Siguiente("TEC");
            if (BuscarPorCodigo(codigo) != null)
            {
                throw new CodigoDuplicadoException("el código", codigo);
            }

            Tecnico nuevo = new Tecnico(codigo, nombre, telefono, correo, especialidad);
            tecnicos.Add(nuevo);
            matriz.AgregarFila();
            return nuevo;
        }

        public void Actualizar(string codigo, string nombre, string telefono, string correo,
                               Especialidad especialidad, bool disponible)
        {
            Tecnico tecnico = BuscarPorCodigo(codigo);
            if (tecnico == null)
            {
                throw new ReglaNegocioException("No existe un técnico con el código " + codigo + ".");
            }
            Validador.ValidarDatosPersona(nombre, telefono, correo);

            if (especialidad != tecnico.Especialidad && tecnico.TrabajosActivos > 0)
            {
                throw new ReglaNegocioException("No se puede cambiar la especialidad de " + tecnico.NombreCompleto +
                    " porque tiene " + tecnico.TrabajosActivos + " trabajo(s) activo(s).");
            }

            tecnico.NombreCompleto = nombre;
            tecnico.Telefono = telefono;
            tecnico.Correo = correo;
            tecnico.Especialidad = especialidad;
            tecnico.Disponible = disponible;
        }

        public Tecnico BuscarPorCodigo(string codigo)
        {
            if (codigo == null)
            {
                return null;
            }
            string buscado = codigo.Trim().ToUpper();
            for (int i = 0; i < tecnicos.Count; i++)
            {
                if (tecnicos[i].Codigo == buscado)
                {
                    return tecnicos[i];
                }
            }
            return null;
        }

        public List<Tecnico> DisponiblesPorEspecialidad(Especialidad especialidad)
        {
            List<Tecnico> candidatos = new List<Tecnico>();
            for (int i = 0; i < tecnicos.Count; i++)
            {
                if (tecnicos[i].Especialidad == especialidad && tecnicos[i].PuedeRecibirTrabajo())
                {
                    candidatos.Add(tecnicos[i]);
                }
            }
            return candidatos;
        }

        public Tecnico AsignarMejorTecnico(Especialidad especialidad)
        {
            List<Tecnico> candidatos = DisponiblesPorEspecialidad(especialidad);
            Tecnico mejor = null;

            for (int i = 0; i < candidatos.Count; i++)
            {
                Tecnico t = candidatos[i];
                if (mejor == null)
                {
                    mejor = t;
                }
                else if (t.TrabajosActivos < mejor.TrabajosActivos)
                {
                    mejor = t;
                }
                else if (t.TrabajosActivos == mejor.TrabajosActivos && t.CalificacionPromedio > mejor.CalificacionPromedio)
                {
                    mejor = t;
                }
            }
            return mejor;
        }

        public List<Tecnico> ListarPorEspecialidad(Especialidad especialidad)
        {
            List<Tecnico> resultado = new List<Tecnico>();
            for (int i = 0; i < tecnicos.Count; i++)
            {
                if (tecnicos[i].Especialidad == especialidad)
                {
                    resultado.Add(tecnicos[i]);
                }
            }
            return resultado;
        }

        public void Calificar(string codigo, int estrellas)
        {
            Tecnico tecnico = BuscarPorCodigo(codigo);
            if (tecnico == null)
            {
                throw new ReglaNegocioException("No existe un técnico con el código " + codigo + ".");
            }
            tecnico.Calificar(estrellas);
        }

        public List<Tecnico> Listar()
        {
            return tecnicos;
        }
    }
}
