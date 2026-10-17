namespace XelaTechSoporte.Servicios
{
    public static class GeneradorCodigos
    {
        private static Dictionary<string, int> contadores = new Dictionary<string, int>();

        public static string Siguiente(string prefijo)
        {
            if (!contadores.ContainsKey(prefijo))
            {
                contadores[prefijo] = 0;
            }
            contadores[prefijo] = contadores[prefijo] + 1;
            return prefijo + "-" + contadores[prefijo].ToString("D4");
        }

        public static void Reiniciar()
        {
            contadores.Clear();
        }
    }
}
