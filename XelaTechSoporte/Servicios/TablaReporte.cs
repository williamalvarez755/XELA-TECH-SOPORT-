namespace XelaTechSoporte.Servicios
{
    public class TablaReporte
    {
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public string[] Columnas { get; set; }
        public List<string[]> Filas { get; set; }
        public string Resumen { get; set; }

        public TablaReporte(string titulo, string descripcion, string[] columnas)
        {
            Titulo = titulo;
            Descripcion = descripcion;
            Columnas = columnas;
            Filas = new List<string[]>();
            Resumen = "";
        }

        public void AgregarFila(params string[] valores)
        {
            Filas.Add(valores);
        }

        public string ComoTexto()
        {
            int[] anchos = new int[Columnas.Length];
            for (int c = 0; c < Columnas.Length; c++)
            {
                anchos[c] = Columnas[c].Length;
            }
            for (int f = 0; f < Filas.Count; f++)
            {
                for (int c = 0; c < Columnas.Length && c < Filas[f].Length; c++)
                {
                    if (Filas[f][c].Length > anchos[c])
                    {
                        anchos[c] = Filas[f][c].Length;
                    }
                }
            }

            string texto = Titulo.ToUpper() + "\r\n";
            texto += Descripcion + "\r\n";
            texto += new string('-', 90) + "\r\n";

            string encabezado = "";
            for (int c = 0; c < Columnas.Length; c++)
            {
                encabezado += Columnas[c].PadRight(anchos[c] + 2);
            }
            texto += encabezado + "\r\n";

            for (int f = 0; f < Filas.Count; f++)
            {
                string linea = "";
                for (int c = 0; c < Columnas.Length && c < Filas[f].Length; c++)
                {
                    linea += Filas[f][c].PadRight(anchos[c] + 2);
                }
                texto += linea + "\r\n";
            }
            if (Filas.Count == 0)
            {
                texto += "(sin datos)\r\n";
            }
            if (Resumen != "")
            {
                texto += ">> " + Resumen + "\r\n";
            }
            return texto;
        }
    }
}
