using XelaTechSoporte.Excepciones;

namespace XelaTechSoporte.Modelos
{
    public class Escritorio : Equipo
    {
        private const decimal COSTO_BASE = 125m;
        private const decimal COSTO_DISCO_ADICIONAL = 30m;
        private const decimal RECARGO_TODO_EN_UNO = 40m;

        public static readonly string[] TiposGabinete = { "Torre", "Mini torre", "Todo en uno" };

        private int cantidadDiscos;
        private string tipoGabinete;

        public Escritorio(string codigoInterno, string numeroSerie, string marca, string modelo, Cliente propietario,
                          string estadoFisicoRecibido, List<string> accesorios, int cantidadDiscos, string tipoGabinete)
            : base(codigoInterno, numeroSerie, marca, modelo, propietario, estadoFisicoRecibido, accesorios)
        {
            if (cantidadDiscos < 1 || cantidadDiscos > 8)
            {
                throw new ReglaNegocioException("La cantidad de discos debe estar entre 1 y 8.");
            }
            if (tipoGabinete == null || tipoGabinete.Trim() == "")
            {
                throw new ReglaNegocioException("Seleccione el tipo de gabinete.");
            }
            this.cantidadDiscos = cantidadDiscos;
            this.tipoGabinete = tipoGabinete;
        }

        public int CantidadDiscos
        {
            get { return cantidadDiscos; }
        }

        public string TipoGabinete
        {
            get { return tipoGabinete; }
        }

        public override TipoEquipo Tipo
        {
            get { return TipoEquipo.Escritorio; }
        }

        public override decimal CalcularCostoRevision()
        {
            decimal costo = COSTO_BASE + (cantidadDiscos - 1) * COSTO_DISCO_ADICIONAL;
            if (tipoGabinete == "Todo en uno")
            {
                costo += RECARGO_TODO_EN_UNO;
            }
            return costo;
        }

        public override string[] ObtenerChecklist()
        {
            return new string[]
            {
                "Revisar fuente de poder",
                "Probar memoria RAM",
                "Revisar estado de los discos",
                "Limpiar gabinete y ventiladores",
                "Revisar cables internos y puertos"
            };
        }

        public override string ObtenerDetalle()
        {
            return "Discos: " + cantidadDiscos + " | Gabinete: " + tipoGabinete;
        }
    }
}
