using XelaTechSoporte.Excepciones;

namespace XelaTechSoporte.Modelos
{
    public class Laptop : Equipo
    {
        private const decimal COSTO_BASE = 150m;
        private const decimal RECARGO_DESARME = 75m;
        private const decimal REVISION_BATERIA = 25m;

        private bool tieneBateria;
        private double pulgadasPantalla;

        public Laptop(string codigoInterno, string numeroSerie, string marca, string modelo, Cliente propietario,
                      string estadoFisicoRecibido, List<string> accesorios, bool tieneBateria, double pulgadasPantalla)
            : base(codigoInterno, numeroSerie, marca, modelo, propietario, estadoFisicoRecibido, accesorios)
        {
            if (pulgadasPantalla < 10 || pulgadasPantalla > 20)
            {
                throw new ReglaNegocioException("El tamaño de pantalla de una laptop debe estar entre 10 y 20 pulgadas.");
            }
            this.tieneBateria = tieneBateria;
            this.pulgadasPantalla = pulgadasPantalla;
        }

        public bool TieneBateria
        {
            get { return tieneBateria; }
        }

        public double PulgadasPantalla
        {
            get { return pulgadasPantalla; }
        }

        public override TipoEquipo Tipo
        {
            get { return TipoEquipo.Laptop; }
        }

        public override decimal CalcularCostoRevision()
        {
            decimal costo = COSTO_BASE + RECARGO_DESARME;
            if (tieneBateria)
            {
                costo += REVISION_BATERIA;
            }
            return costo;
        }

        public override string[] ObtenerChecklist()
        {
            return new string[]
            {
                "Revisar carcasa y bisagras",
                "Probar pantalla y brillo",
                "Probar teclado y touchpad",
                "Revisar estado de la batería",
                "Limpiar ventilador y cambiar pasta térmica",
                "Verificar cargador y puerto de carga"
            };
        }

        public override string ObtenerDetalle()
        {
            string bateria = tieneBateria ? "Sí" : "No";
            return "Batería: " + bateria + " | Pantalla: " + pulgadasPantalla + "\"";
        }

        public override int HorasEstimadasBase()
        {
            return 3;
        }
    }
}
