using XelaTechSoporte.Modelos;

namespace XelaTechSoporte.Algoritmos
{
    public class FiltroSolicitud
    {
        public bool UsarEstado { get; set; }
        public EstadoSolicitud Estado { get; set; }

        public bool UsarPrioridad { get; set; }
        public NivelPrioridad Prioridad { get; set; }

        public Tecnico Tecnico { get; set; }

        public bool UsarFechas { get; set; }
        public DateTime Desde { get; set; }
        public DateTime Hasta { get; set; }

        public bool TieneAlgunFiltro()
        {
            return UsarEstado || UsarPrioridad || Tecnico != null || UsarFechas;
        }

        public string Describir()
        {
            string texto = "";
            if (UsarEstado)
            {
                texto += "Estado=" + Textos.NombreEstado(Estado) + " ";
            }
            if (UsarPrioridad)
            {
                texto += "Prioridad=" + Textos.NombrePrioridad(Prioridad) + " ";
            }
            if (Tecnico != null)
            {
                texto += "Técnico=" + Tecnico.NombreCompleto + " ";
            }
            if (UsarFechas)
            {
                texto += "Fechas=" + Desde.ToString("dd/MM") + " a " + Hasta.ToString("dd/MM");
            }
            return texto.Trim();
        }
    }
}
