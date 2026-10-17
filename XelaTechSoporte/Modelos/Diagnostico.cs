using XelaTechSoporte.Excepciones;
using XelaTechSoporte.Servicios;

namespace XelaTechSoporte.Modelos
{
    public class Diagnostico
    {
        private string codigo;
        private string descripcion;
        private List<string> acciones;
        private int tiempoEstimadoHoras;
        private DateTime fecha;
        private Tecnico autor;

        public Diagnostico(string codigo, string descripcion, int tiempoEstimadoHoras, Tecnico autor)
        {
            Validador.ExigirTexto(codigo, "Código del diagnóstico");
            Validador.ExigirTexto(descripcion, "Descripción del diagnóstico");
            if (tiempoEstimadoHoras <= 0)
            {
                throw new ReglaNegocioException("El tiempo estimado debe ser mayor que cero.");
            }
            this.codigo = codigo;
            this.descripcion = descripcion.Trim();
            this.tiempoEstimadoHoras = tiempoEstimadoHoras;
            this.autor = autor;
            fecha = DateTime.Now;
            acciones = new List<string>();
        }

        public string Codigo
        {
            get { return codigo; }
        }

        public string Descripcion
        {
            get { return descripcion; }
        }

        public List<string> Acciones
        {
            get { return acciones; }
        }

        public int TiempoEstimadoHoras
        {
            get { return tiempoEstimadoHoras; }
        }

        public DateTime Fecha
        {
            get { return fecha; }
        }

        public Tecnico Autor
        {
            get { return autor; }
        }

        public void AgregarAccion(string accion)
        {
            if (Validador.TextoObligatorio(accion))
            {
                acciones.Add(accion.Trim());
            }
        }

        public override string ToString()
        {
            string texto = codigo + ": " + descripcion + " (" + tiempoEstimadoHoras + " h estimadas)";
            for (int i = 0; i < acciones.Count; i++)
            {
                texto += "\n   - " + acciones[i];
            }
            return texto;
        }
    }
}
