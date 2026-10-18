using XelaTechSoporte.Excepciones;
using XelaTechSoporte.Modelos;

namespace XelaTechSoporte.Estructuras
{
    public class MatrizCarga
    {
        public const int DIAS = 6;

        public static readonly string[] NombresDias = { "Lunes", "Martes", "Miércoles", "Jueves", "Viernes", "Sábado" };
        public static readonly string[] DiasCortos = { "Lun", "Mar", "Mié", "Jue", "Vie", "Sáb" };

        private int[,] carga;
        private List<Tecnico> tecnicos;

        public MatrizCarga(List<Tecnico> tecnicos)
        {
            this.tecnicos = tecnicos;
            carga = new int[tecnicos.Count, DIAS];
        }

        public int Filas
        {
            get { return carga.GetLength(0); }
        }

        public void AgregarFila()
        {
            int filasActuales = carga.GetLength(0);
            int[,] nueva = new int[filasActuales + 1, DIAS];

            for (int f = 0; f < filasActuales; f++)
            {
                for (int d = 0; d < DIAS; d++)
                {
                    nueva[f, d] = carga[f, d];
                }
            }
            carga = nueva;
        }

        public void Registrar(Tecnico tecnico, int dia)
        {
            int fila = BuscarFila(tecnico);
            ValidarDia(dia);
            carga[fila, dia]++;
        }

        public void Quitar(Tecnico tecnico, int dia)
        {
            int fila = BuscarFila(tecnico);
            ValidarDia(dia);
            if (carga[fila, dia] > 0)
            {
                carga[fila, dia]--;
            }
        }

        public int Valor(int fila, int dia)
        {
            return carga[fila, dia];
        }

        public int TotalTecnico(int fila)
        {
            int total = 0;
            for (int d = 0; d < DIAS; d++)
            {
                total += carga[fila, d];
            }
            return total;
        }

        public int TotalDia(int dia)
        {
            int total = 0;
            for (int f = 0; f < Filas; f++)
            {
                total += carga[f, dia];
            }
            return total;
        }

        public Tecnico TecnicoConMayorCarga()
        {
            int mejorFila = -1;
            int mayor = 0;
            for (int f = 0; f < Filas; f++)
            {
                int total = TotalTecnico(f);
                if (total > mayor)
                {
                    mayor = total;
                    mejorFila = f;
                }
            }
            if (mejorFila == -1)
            {
                return null;
            }
            return tecnicos[mejorFila];
        }

        public int DiaConMayorCarga()
        {
            int mejorDia = -1;
            int mayor = 0;
            for (int d = 0; d < DIAS; d++)
            {
                int total = TotalDia(d);
                if (total > mayor)
                {
                    mayor = total;
                    mejorDia = d;
                }
            }
            return mejorDia;
        }

        public static int DiaDeFecha(DateTime fecha)
        {
            if (fecha.DayOfWeek == DayOfWeek.Sunday)
            {
                return 0;
            }
            return (int)fecha.DayOfWeek - 1;
        }

        public void Reiniciar()
        {
            carga = new int[tecnicos.Count, DIAS];
        }

        private int BuscarFila(Tecnico tecnico)
        {
            for (int i = 0; i < tecnicos.Count && i < Filas; i++)
            {
                if (tecnicos[i] == tecnico)
                {
                    return i;
                }
            }
            throw new ReglaNegocioException("El técnico no está registrado en la matriz de carga.");
        }

        private void ValidarDia(int dia)
        {
            if (dia < 0 || dia >= DIAS)
            {
                throw new ReglaNegocioException("El día debe estar entre 0 (lunes) y 5 (sábado).");
            }
        }
    }
}
