namespace XelaTechSoporte.Excepciones
{
    public class ExistenciaInsuficienteException : ReglaNegocioException
    {
        public ExistenciaInsuficienteException(string repuesto, int existencia, int cantidadPedida)
            : base("No hay existencia suficiente de \"" + repuesto + "\". Se pidieron " + cantidadPedida +
                   " y solo hay " + existencia + " en inventario.")
        {
        }
    }
}
