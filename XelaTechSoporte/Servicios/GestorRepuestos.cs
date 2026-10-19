using XelaTechSoporte.Algoritmos;
using XelaTechSoporte.Excepciones;
using XelaTechSoporte.Modelos;

namespace XelaTechSoporte.Servicios
{
    public class GestorRepuestos
    {
        private const int MAXIMO_REABASTECER = 1000;

        private List<Repuesto> repuestos;

        public GestorRepuestos(List<Repuesto> repuestos)
        {
            this.repuestos = repuestos;
        }

        public Repuesto Registrar(string nombre, CategoriaRepuesto categoria, int existencia, int existenciaMinima,
                                  decimal costoUnitario, decimal precioVenta)
        {
            Validador.ExigirTexto(nombre, "Nombre");
            Validador.ExigirNoNegativo(existencia, "Existencia");
            Validador.ExigirNoNegativo(existenciaMinima, "Existencia mínima");
            Validador.ExigirNoNegativo(costoUnitario, "Costo unitario");
            Validador.ExigirNoNegativo(precioVenta, "Precio de venta");
            if (precioVenta < costoUnitario)
            {
                throw new ReglaNegocioException("El precio de venta no puede ser menor que el costo unitario (se vendería con pérdida).");
            }
            if (BuscarPorNombreExacto(nombre) != null)
            {
                throw new CodigoDuplicadoException("el nombre", nombre.Trim());
            }

            string codigo = GeneradorCodigos.Siguiente("REP");
            if (BuscarPorCodigo(codigo) != null)
            {
                throw new CodigoDuplicadoException("el código", codigo);
            }

            Repuesto nuevo = new Repuesto(codigo, nombre, categoria, existencia, existenciaMinima, costoUnitario, precioVenta);
            repuestos.Add(nuevo);
            return nuevo;
        }

        public void Actualizar(string codigo, string nombre, CategoriaRepuesto categoria, int existenciaMinima,
                               decimal costoUnitario, decimal precioVenta)
        {
            Repuesto repuesto = BuscarPorCodigo(codigo);
            if (repuesto == null)
            {
                throw new ReglaNegocioException("No existe un repuesto con el código " + codigo + ".");
            }
            Validador.ExigirTexto(nombre, "Nombre");
            Validador.ExigirNoNegativo(existenciaMinima, "Existencia mínima");
            Repuesto otro = BuscarPorNombreExacto(nombre);
            if (otro != null && otro != repuesto)
            {
                throw new CodigoDuplicadoException("el nombre", nombre.Trim());
            }

            repuesto.CambiarPrecios(costoUnitario, precioVenta);
            repuesto.ExistenciaMinima = existenciaMinima;
            repuesto.Nombre = nombre;
            repuesto.Categoria = categoria;
        }

        public void Reabastecer(string codigo, int cantidad)
        {
            Repuesto repuesto = BuscarPorCodigo(codigo);
            if (repuesto == null)
            {
                throw new ReglaNegocioException("No existe un repuesto con el código " + codigo + ".");
            }
            if (cantidad > MAXIMO_REABASTECER)
            {
                throw new ReglaNegocioException("No se pueden ingresar más de " + MAXIMO_REABASTECER + " unidades en un solo movimiento.");
            }
            repuesto.Reabastecer(cantidad);
        }

        public Repuesto BuscarPorCodigo(string codigo)
        {
            if (codigo == null)
            {
                return null;
            }
            string buscado = codigo.Trim().ToUpper();
            for (int i = 0; i < repuestos.Count; i++)
            {
                if (repuestos[i].Codigo == buscado)
                {
                    return repuestos[i];
                }
            }
            return null;
        }

        private Repuesto BuscarPorNombreExacto(string nombre)
        {
            string buscado = Busquedas.Normalizar(nombre);
            for (int i = 0; i < repuestos.Count; i++)
            {
                if (Busquedas.Normalizar(repuestos[i].Nombre) == buscado)
                {
                    return repuestos[i];
                }
            }
            return null;
        }

        public List<Repuesto> BajoMinimo()
        {
            List<Repuesto> resultado = new List<Repuesto>();
            for (int i = 0; i < repuestos.Count; i++)
            {
                if (repuestos[i].EnAlerta())
                {
                    resultado.Add(repuestos[i]);
                }
            }
            return resultado;
        }

        public List<Repuesto> Listar()
        {
            return repuestos;
        }
    }
}
