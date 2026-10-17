namespace XelaTechSoporte.Modelos
{
    public static class Textos
    {
        public static readonly string[] Estados =
        {
            "Recibida", "En espera", "Asignada", "En diagnóstico",
            "En reparación", "Lista para entregar", "Entregada", "Cancelada"
        };

        public static readonly string[] Prioridades = { "Normal", "Urgente" };

        public static readonly string[] TiposCliente = { "Estudiante", "Profesional", "Pequeño negocio", "Institución" };

        public static readonly string[] Especialidades = { "Hardware", "Software", "Impresoras", "Redes" };

        public static readonly string[] TiposEquipo = { "Laptop", "Escritorio", "Impresora" };

        public static readonly string[] Categorias = { "Almacenamiento", "Memoria", "Fuente", "Consumible", "Accesorio" };

        public static string NombreEstado(EstadoSolicitud estado)
        {
            return Estados[(int)estado];
        }

        public static string NombrePrioridad(NivelPrioridad prioridad)
        {
            return Prioridades[(int)prioridad];
        }

        public static string NombreTipoCliente(TipoCliente tipo)
        {
            return TiposCliente[(int)tipo];
        }

        public static string NombreEspecialidad(Especialidad especialidad)
        {
            return Especialidades[(int)especialidad];
        }

        public static string NombreTipoEquipo(TipoEquipo tipo)
        {
            return TiposEquipo[(int)tipo];
        }

        public static string NombreCategoria(CategoriaRepuesto categoria)
        {
            return Categorias[(int)categoria];
        }

        public static string Moneda(decimal valor)
        {
            return "Q " + valor.ToString("N2");
        }
    }
}
