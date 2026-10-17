namespace XelaTechSoporte.Modelos
{
    public enum TipoCliente
    {
        Estudiante,
        Profesional,
        PequenoNegocio,
        Institucion
    }

    public enum Especialidad
    {
        Hardware,
        Software,
        Impresoras,
        Redes
    }

    public enum TipoEquipo
    {
        Laptop,
        Escritorio,
        Impresora
    }

    public enum NivelPrioridad
    {
        Normal,
        Urgente
    }

    public enum EstadoSolicitud
    {
        Recibida,
        EnEspera,
        Asignada,
        EnDiagnostico,
        EnReparacion,
        ListaParaEntregar,
        Entregada,
        Cancelada
    }

    public enum CategoriaRepuesto
    {
        Almacenamiento,
        Memoria,
        Fuente,
        Consumible,
        Accesorio
    }
}
