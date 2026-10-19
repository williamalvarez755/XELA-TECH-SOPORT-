using XelaTechSoporte.Algoritmos;
using XelaTechSoporte.Excepciones;
using XelaTechSoporte.Modelos;

namespace XelaTechSoporte.Servicios
{
    public class GestorClientes
    {
        private List<Cliente> clientes;

        public GestorClientes(List<Cliente> clientes)
        {
            this.clientes = clientes;
        }

        public Cliente Registrar(string nombre, string telefono, string correo, TipoCliente tipo)
        {
            Validador.ValidarDatosPersona(nombre, telefono, correo);

            string codigo = GeneradorCodigos.Siguiente("CLI");
            if (BuscarPorCodigo(codigo) != null)
            {
                throw new CodigoDuplicadoException("el código", codigo);
            }

            Cliente nuevo = new Cliente(codigo, nombre, telefono, correo, tipo);
            clientes.Add(nuevo);
            return nuevo;
        }

        public Cliente BuscarPorCodigo(string codigo)
        {
            if (codigo == null)
            {
                return null;
            }
            string buscado = codigo.Trim().ToUpper();
            for (int i = 0; i < clientes.Count; i++)
            {
                if (clientes[i].Codigo == buscado)
                {
                    return clientes[i];
                }
            }
            return null;
        }

        public List<Cliente> BuscarPorNombre(string texto)
        {
            return Busquedas.SecuencialClientesPorNombre(clientes, texto);
        }

        public void Actualizar(string codigo, string nombre, string telefono, string correo, TipoCliente tipo)
        {
            Cliente cliente = BuscarPorCodigo(codigo);
            if (cliente == null)
            {
                throw new ReglaNegocioException("No existe un cliente con el código " + codigo + ".");
            }
            Validador.ValidarDatosPersona(nombre, telefono, correo);

            cliente.NombreCompleto = nombre;
            cliente.Telefono = telefono;
            cliente.Correo = correo;
            cliente.Tipo = tipo;
        }

        public List<Cliente> Listar()
        {
            return clientes;
        }
    }
}
