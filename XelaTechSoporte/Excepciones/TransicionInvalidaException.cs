using XelaTechSoporte.Modelos;

namespace XelaTechSoporte.Excepciones
{
    public class TransicionInvalidaException : ReglaNegocioException
    {
        public TransicionInvalidaException(EstadoSolicitud actual, EstadoSolicitud destino)
            : base("No se puede pasar del estado \"" + Textos.NombreEstado(actual) + "\" al estado \"" +
                   Textos.NombreEstado(destino) + "\". Ese cambio no respeta el proceso de atención.")
        {
        }
    }
}
