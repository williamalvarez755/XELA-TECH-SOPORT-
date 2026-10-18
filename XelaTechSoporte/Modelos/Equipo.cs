using XelaTechSoporte.Excepciones;
using XelaTechSoporte.Servicios;

namespace XelaTechSoporte.Modelos
{
    public abstract class Equipo
    {
        protected string codigoInterno;
        protected string numeroSerie;
        protected string marca;
        protected string modelo;
        protected Cliente propietario;
        protected string estadoFisicoRecibido;
        protected List<string> accesorios;

        protected Equipo(string codigoInterno, string numeroSerie, string marca, string modelo,
                         Cliente propietario, string estadoFisicoRecibido, List<string> accesorios)
        {
            Validador.ExigirTexto(codigoInterno, "Código interno");
            Validador.ExigirTexto(numeroSerie, "Número de serie");
            Validador.ExigirTexto(marca, "Marca");
            Validador.ExigirTexto(modelo, "Modelo");
            Validador.ExigirTexto(estadoFisicoRecibido, "Estado físico recibido");
            if (propietario == null)
            {
                throw new ReglaNegocioException("El equipo debe tener un propietario (cliente).");
            }

            this.codigoInterno = codigoInterno;
            this.numeroSerie = numeroSerie.Trim().ToUpper();
            this.marca = marca.Trim();
            this.modelo = modelo.Trim();
            this.propietario = propietario;
            this.estadoFisicoRecibido = estadoFisicoRecibido.Trim();

            this.accesorios = new List<string>();
            if (accesorios != null)
            {
                for (int i = 0; i < accesorios.Count; i++)
                {
                    if (Validador.TextoObligatorio(accesorios[i]))
                    {
                        this.accesorios.Add(accesorios[i].Trim());
                    }
                }
            }
        }

        public string CodigoInterno
        {
            get { return codigoInterno; }
        }

        public string NumeroSerie
        {
            get { return numeroSerie; }
        }

        public string Marca
        {
            get { return marca; }
        }

        public string Modelo
        {
            get { return modelo; }
        }

        public Cliente Propietario
        {
            get { return propietario; }
        }

        public string EstadoFisicoRecibido
        {
            get { return estadoFisicoRecibido; }
        }

        public List<string> Accesorios
        {
            get { return accesorios; }
        }

        public string AccesoriosTexto
        {
            get
            {
                if (accesorios.Count == 0)
                {
                    return "Ninguno";
                }
                string texto = "";
                for (int i = 0; i < accesorios.Count; i++)
                {
                    if (i > 0)
                    {
                        texto += ", ";
                    }
                    texto += accesorios[i];
                }
                return texto;
            }
        }

        public abstract TipoEquipo Tipo { get; }

        public abstract decimal CalcularCostoRevision();

        public abstract string[] ObtenerChecklist();

        public abstract string ObtenerDetalle();

        public virtual Especialidad EspecialidadSugerida()
        {
            return Especialidad.Hardware;
        }

        public virtual int HorasEstimadasBase()
        {
            return 2;
        }

        public override string ToString()
        {
            return codigoInterno + " - " + Textos.NombreTipoEquipo(Tipo) + " " + marca + " " + modelo;
        }
    }
}
