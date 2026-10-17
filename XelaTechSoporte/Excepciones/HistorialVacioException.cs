namespace XelaTechSoporte.Excepciones
{
    public class HistorialVacioException : ReglaNegocioException
    {
        public HistorialVacioException(string codigoSolicitud)
            : base("La solicitud " + codigoSolicitud + " no tiene cambios de estado para deshacer (el historial está vacío).")
        {
        }
    }
}
