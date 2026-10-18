namespace XelaTechSoporte.Modelos
{
    public class DetalleRepuesto
    {
        private Repuesto repuesto;
        private int cantidad;
        private decimal precioAplicado;

        public DetalleRepuesto(Repuesto repuesto, int cantidad, decimal precioAplicado)
        {
            this.repuesto = repuesto;
            this.cantidad = cantidad;
            this.precioAplicado = precioAplicado;
        }

        public Repuesto Repuesto
        {
            get { return repuesto; }
        }

        public int Cantidad
        {
            get { return cantidad; }
        }

        public decimal PrecioAplicado
        {
            get { return precioAplicado; }
        }

        public decimal Subtotal
        {
            get { return cantidad * precioAplicado; }
        }

        public void SumarCantidad(int extra)
        {
            cantidad += extra;
        }

        public override string ToString()
        {
            return repuesto.Nombre + " x" + cantidad + " = " + Textos.Moneda(Subtotal);
        }
    }
}
