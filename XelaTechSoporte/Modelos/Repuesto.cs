using XelaTechSoporte.Excepciones;
using XelaTechSoporte.Servicios;

namespace XelaTechSoporte.Modelos
{
    public class Repuesto
    {
        private string codigo;
        private string nombre;
        private CategoriaRepuesto categoria;
        private int existencia;
        private int existenciaMinima;
        private decimal costoUnitario;
        private decimal precioVenta;
        private int cantidadUtilizada;

        public Repuesto(string codigo, string nombre, CategoriaRepuesto categoria, int existencia,
                        int existenciaMinima, decimal costoUnitario, decimal precioVenta)
        {
            Validador.ExigirTexto(codigo, "Código");
            Validador.ExigirTexto(nombre, "Nombre");
            Validador.ExigirNoNegativo(existencia, "Existencia");
            Validador.ExigirNoNegativo(existenciaMinima, "Existencia mínima");
            ValidarPrecios(costoUnitario, precioVenta);

            this.codigo = codigo;
            this.nombre = nombre.Trim();
            this.categoria = categoria;
            this.existencia = existencia;
            this.existenciaMinima = existenciaMinima;
            this.costoUnitario = costoUnitario;
            this.precioVenta = precioVenta;
            cantidadUtilizada = 0;
        }

        public string Codigo
        {
            get { return codigo; }
        }

        public string Nombre
        {
            get { return nombre; }
            set
            {
                Validador.ExigirTexto(value, "Nombre");
                nombre = value.Trim();
            }
        }

        public CategoriaRepuesto Categoria
        {
            get { return categoria; }
            set { categoria = value; }
        }

        public int Existencia
        {
            get { return existencia; }
        }

        public int ExistenciaMinima
        {
            get { return existenciaMinima; }
            set
            {
                Validador.ExigirNoNegativo(value, "Existencia mínima");
                existenciaMinima = value;
            }
        }

        public decimal CostoUnitario
        {
            get { return costoUnitario; }
        }

        public decimal PrecioVenta
        {
            get { return precioVenta; }
        }

        public int CantidadUtilizada
        {
            get { return cantidadUtilizada; }
        }

        public decimal Margen
        {
            get { return precioVenta - costoUnitario; }
        }

        public void CambiarPrecios(decimal nuevoCosto, decimal nuevoPrecio)
        {
            ValidarPrecios(nuevoCosto, nuevoPrecio);
            costoUnitario = nuevoCosto;
            precioVenta = nuevoPrecio;
        }

        public bool HayExistencia(int cantidad)
        {
            return cantidad <= existencia;
        }

        public void Descontar(int cantidad)
        {
            if (cantidad <= 0)
            {
                throw new ReglaNegocioException("La cantidad a usar debe ser mayor que cero.");
            }
            if (!HayExistencia(cantidad))
            {
                throw new ExistenciaInsuficienteException(nombre, existencia, cantidad);
            }
            existencia -= cantidad;
            cantidadUtilizada += cantidad;
        }

        public void Devolver(int cantidad)
        {
            if (cantidad <= 0)
            {
                return;
            }
            existencia += cantidad;
            cantidadUtilizada -= cantidad;
            if (cantidadUtilizada < 0)
            {
                cantidadUtilizada = 0;
            }
        }

        public void Reabastecer(int cantidad)
        {
            if (cantidad <= 0)
            {
                throw new ReglaNegocioException("La cantidad a reabastecer debe ser mayor que cero.");
            }
            existencia += cantidad;
        }

        public bool EnAlerta()
        {
            return existencia <= existenciaMinima;
        }

        public bool EstaAgotado()
        {
            return existencia == 0;
        }

        public string TextoEstado
        {
            get
            {
                if (EstaAgotado())
                {
                    return "Agotado";
                }
                if (EnAlerta())
                {
                    return "Bajo mínimo";
                }
                return "Normal";
            }
        }

        private void ValidarPrecios(decimal costo, decimal precio)
        {
            Validador.ExigirNoNegativo(costo, "Costo unitario");
            Validador.ExigirNoNegativo(precio, "Precio de venta");
            if (precio < costo)
            {
                throw new ReglaNegocioException("El precio de venta no puede ser menor que el costo unitario (se vendería con pérdida).");
            }
        }

        public override string ToString()
        {
            return codigo + " - " + nombre + " (exist. " + existencia + ")";
        }
    }
}
