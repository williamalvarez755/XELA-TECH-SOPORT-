namespace XelaTechSoporte.Modelos
{
    public class Cliente : Persona
    {
        private TipoCliente tipo;
        private int cantidadSolicitudes;

        public Cliente(string codigo, string nombreCompleto, string telefono, string correo, TipoCliente tipo)
            : base(codigo, nombreCompleto, telefono, correo)
        {
            this.tipo = tipo;
            cantidadSolicitudes = 0;
        }

        public Cliente(string codigo, string nombreCompleto, string telefono, string correo)
            : this(codigo, nombreCompleto, telefono, correo, TipoCliente.Estudiante)
        {
        }

        public TipoCliente Tipo
        {
            get { return tipo; }
            set { tipo = value; }
        }

        public int CantidadSolicitudes
        {
            get { return cantidadSolicitudes; }
        }

        public void SumarSolicitud()
        {
            cantidadSolicitudes++;
        }

        public decimal ObtenerDescuento()
        {
            switch (tipo)
            {
                case TipoCliente.Estudiante:
                    return 0.10m;
                case TipoCliente.PequenoNegocio:
                    return 0.05m;
                case TipoCliente.Institucion:
                    return 0.15m;
                default:
                    return 0m;
            }
        }

        public override string ObtenerResumen()
        {
            return base.ObtenerResumen() + " | " + Textos.NombreTipoCliente(tipo)
                + " | Solicitudes: " + cantidadSolicitudes;
        }
    }
}
