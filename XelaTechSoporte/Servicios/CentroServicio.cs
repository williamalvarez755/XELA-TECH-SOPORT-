using XelaTechSoporte.Algoritmos;
using XelaTechSoporte.Estructuras;
using XelaTechSoporte.Modelos;

namespace XelaTechSoporte.Servicios
{
    public class CentroServicio
    {
        private List<Cliente> clientes;
        private List<Tecnico> tecnicos;
        private List<Equipo> equipos;
        private List<Solicitud> solicitudes;
        private List<Repuesto> repuestos;

        private ColaAtencion cola;
        private MatrizCarga matriz;

        private int[] equiposPorTipo;

        private List<ResultadoBusqueda> registroBusquedas;

        private List<Solicitud> ultimaListaOrdenada;
        private string descripcionUltimoOrden;

        private GestorClientes gestorClientes;
        private GestorTecnicos gestorTecnicos;
        private GestorEquipos gestorEquipos;
        private GestorSolicitudes gestorSolicitudes;
        private GestorRepuestos gestorRepuestos;

        public CentroServicio()
        {
            Inicializar();
        }

        public void Inicializar()
        {
            clientes = new List<Cliente>();
            tecnicos = new List<Tecnico>();
            equipos = new List<Equipo>();
            solicitudes = new List<Solicitud>();
            repuestos = new List<Repuesto>();

            cola = new ColaAtencion();
            matriz = new MatrizCarga(tecnicos);
            equiposPorTipo = new int[3];
            registroBusquedas = new List<ResultadoBusqueda>();
            ultimaListaOrdenada = null;
            descripcionUltimoOrden = "";

            gestorClientes = new GestorClientes(clientes);
            gestorTecnicos = new GestorTecnicos(tecnicos, matriz);
            gestorEquipos = new GestorEquipos(equipos, solicitudes, equiposPorTipo);
            gestorRepuestos = new GestorRepuestos(repuestos);
            gestorSolicitudes = new GestorSolicitudes(solicitudes, cola, matriz, gestorClientes, gestorEquipos, gestorTecnicos);
        }

        public void Reiniciar()
        {
            cola.Vaciar();
            GeneradorCodigos.Reiniciar();
            Inicializar();
        }

        public GestorClientes Clientes
        {
            get { return gestorClientes; }
        }

        public GestorTecnicos Tecnicos
        {
            get { return gestorTecnicos; }
        }

        public GestorEquipos Equipos
        {
            get { return gestorEquipos; }
        }

        public GestorSolicitudes Solicitudes
        {
            get { return gestorSolicitudes; }
        }

        public GestorRepuestos Repuestos
        {
            get { return gestorRepuestos; }
        }

        public ColaAtencion Cola
        {
            get { return cola; }
        }

        public MatrizCarga Matriz
        {
            get { return matriz; }
        }

        public List<ResultadoBusqueda> RegistroBusquedas
        {
            get { return registroBusquedas; }
        }

        public List<Solicitud> UltimaListaOrdenada
        {
            get { return ultimaListaOrdenada; }
        }

        public string DescripcionUltimoOrden
        {
            get { return descripcionUltimoOrden; }
        }

        public void GuardarUltimoOrden(List<Solicitud> lista, string descripcion)
        {
            ultimaListaOrdenada = lista;
            descripcionUltimoOrden = descripcion;
        }
    }
}
