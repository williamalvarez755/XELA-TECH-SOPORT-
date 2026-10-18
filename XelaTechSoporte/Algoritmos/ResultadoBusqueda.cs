namespace XelaTechSoporte.Algoritmos
{
    public class ResultadoBusqueda
    {
        public DateTime Hora { get; set; }
        public string Tipo { get; set; }
        public string Termino { get; set; }
        public int Coincidencias { get; set; }
        public int Comparaciones { get; set; }
        public string Detalle { get; set; }

        public ResultadoBusqueda(string tipo, string termino, int coincidencias, int comparaciones, string detalle)
        {
            Hora = DateTime.Now;
            Tipo = tipo;
            Termino = termino;
            Coincidencias = coincidencias;
            Comparaciones = comparaciones;
            Detalle = detalle;
        }
    }
}
