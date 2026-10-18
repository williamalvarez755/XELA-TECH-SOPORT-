using XelaTechSoporte.Excepciones;
using XelaTechSoporte.Servicios;

namespace XelaTechSoporte.Modelos
{
    public abstract class Persona
    {
        protected string codigo;
        protected string nombreCompleto;
        protected string telefono;
        protected string correo;

        protected Persona(string codigo, string nombreCompleto, string telefono, string correo)
        {
            Validador.ExigirTexto(codigo, "Código");
            this.codigo = codigo;

            NombreCompleto = nombreCompleto;
            Telefono = telefono;
            Correo = correo;
        }

        public string Codigo
        {
            get { return codigo; }
        }

        public string NombreCompleto
        {
            get { return nombreCompleto; }
            set
            {
                Validador.ExigirTexto(value, "Nombre completo");
                nombreCompleto = value.Trim();
            }
        }

        public string Telefono
        {
            get { return telefono; }
            set
            {
                Validador.ExigirTexto(value, "Teléfono");
                if (!Validador.TelefonoValido(value))
                {
                    throw new ReglaNegocioException("El teléfono debe tener exactamente 8 dígitos (formato de Guatemala).");
                }
                telefono = value.Trim();
            }
        }

        public string Correo
        {
            get { return correo; }
            set
            {
                Validador.ExigirTexto(value, "Correo electrónico");
                if (!Validador.CorreoValido(value))
                {
                    throw new ReglaNegocioException("El correo debe tener una arroba (@) y un punto después de ella. Ejemplo: ana@correo.com");
                }
                correo = value.Trim();
            }
        }

        public virtual string ObtenerResumen()
        {
            return codigo + " | " + nombreCompleto + " | Tel. " + telefono + " | " + correo;
        }

        public override string ToString()
        {
            return codigo + " - " + nombreCompleto;
        }
    }
}
