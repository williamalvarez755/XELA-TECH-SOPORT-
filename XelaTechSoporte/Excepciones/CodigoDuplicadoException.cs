namespace XelaTechSoporte.Excepciones
{
    public class CodigoDuplicadoException : ReglaNegocioException
    {
        public CodigoDuplicadoException(string queDato, string valor)
            : base("Ya existe un registro con " + queDato + " \"" + valor + "\". No se permiten duplicados.")
        {
        }
    }
}
