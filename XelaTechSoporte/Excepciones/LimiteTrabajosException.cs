namespace XelaTechSoporte.Excepciones
{
    public class LimiteTrabajosException : ReglaNegocioException
    {
        public LimiteTrabajosException(string tecnico, int trabajosActivos, int limite)
            : base("El técnico " + tecnico + " ya tiene " + trabajosActivos + " trabajos activos (límite: " +
                   limite + "). No puede recibir otra solicitud.")
        {
        }
    }
}
