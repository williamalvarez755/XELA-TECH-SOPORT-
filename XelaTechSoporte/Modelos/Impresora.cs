using XelaTechSoporte.Excepciones;

namespace XelaTechSoporte.Modelos
{
    public class Impresora : Equipo
    {
        private const decimal COSTO_BASE = 100m;
        private const decimal LIMPIEZA_CABEZALES = 50m;
        private const decimal REVISION_FUSOR = 40m;
        private const decimal MANTENIMIENTO_USO = 35m;
        private const int PAGINAS_USO_ALTO = 50000;

        private bool esLaser;
        private int contadorPaginas;

        public Impresora(string codigoInterno, string numeroSerie, string marca, string modelo, Cliente propietario,
                         string estadoFisicoRecibido, List<string> accesorios, bool esLaser, int contadorPaginas)
            : base(codigoInterno, numeroSerie, marca, modelo, propietario, estadoFisicoRecibido, accesorios)
        {
            if (contadorPaginas < 0)
            {
                throw new ReglaNegocioException("El contador de páginas no puede ser negativo.");
            }
            this.esLaser = esLaser;
            this.contadorPaginas = contadorPaginas;
        }

        public bool EsLaser
        {
            get { return esLaser; }
        }

        public int ContadorPaginas
        {
            get { return contadorPaginas; }
        }

        public override TipoEquipo Tipo
        {
            get { return TipoEquipo.Impresora; }
        }

        public override decimal CalcularCostoRevision()
        {
            decimal costo = COSTO_BASE + LIMPIEZA_CABEZALES;
            if (esLaser)
            {
                costo += REVISION_FUSOR;
            }
            if (contadorPaginas > PAGINAS_USO_ALTO)
            {
                costo += MANTENIMIENTO_USO;
            }
            return costo;
        }

        public override string[] ObtenerChecklist()
        {
            return new string[]
            {
                "Limpiar cabezales",
                "Revisar rodillos de alimentación",
                "Revisar niveles de tinta o tóner",
                "Imprimir página de prueba",
                "Revisar conexión USB y red"
            };
        }

        public override string ObtenerDetalle()
        {
            string tipo = esLaser ? "Láser" : "Inyección de tinta";
            return "Tipo: " + tipo + " | Páginas impresas: " + contadorPaginas;
        }

        public override Especialidad EspecialidadSugerida()
        {
            return Especialidad.Impresoras;
        }
    }
}
