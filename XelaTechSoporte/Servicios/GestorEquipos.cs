using XelaTechSoporte.Algoritmos;
using XelaTechSoporte.Excepciones;
using XelaTechSoporte.Modelos;

namespace XelaTechSoporte.Servicios
{
    public class GestorEquipos
    {
        private List<Equipo> equipos;
        private List<Solicitud> solicitudes;

        private int[] porTipo;

        public GestorEquipos(List<Equipo> equipos, List<Solicitud> solicitudes, int[] porTipo)
        {
            this.equipos = equipos;
            this.solicitudes = solicitudes;
            this.porTipo = porTipo;
        }

        public Laptop RegistrarLaptop(Cliente propietario, string serie, string marca, string modelo,
                                      string estadoFisico, List<string> accesorios, bool tieneBateria, double pulgadas)
        {
            ValidarComunes(propietario, serie, marca, modelo, estadoFisico);
            if (pulgadas < 10 || pulgadas > 20)
            {
                throw new ReglaNegocioException("El tamaño de pantalla de una laptop debe estar entre 10 y 20 pulgadas.");
            }
            Laptop nueva = new Laptop(GeneradorCodigos.Siguiente("EQP"), serie, marca, modelo, propietario,
                                      estadoFisico, accesorios, tieneBateria, pulgadas);
            Agregar(nueva);
            return nueva;
        }

        public Escritorio RegistrarEscritorio(Cliente propietario, string serie, string marca, string modelo,
                                              string estadoFisico, List<string> accesorios, int cantidadDiscos, string tipoGabinete)
        {
            ValidarComunes(propietario, serie, marca, modelo, estadoFisico);
            if (cantidadDiscos < 1 || cantidadDiscos > 8)
            {
                throw new ReglaNegocioException("La cantidad de discos debe estar entre 1 y 8.");
            }
            Validador.ExigirTexto(tipoGabinete, "Tipo de gabinete");
            Escritorio nuevo = new Escritorio(GeneradorCodigos.Siguiente("EQP"), serie, marca, modelo, propietario,
                                              estadoFisico, accesorios, cantidadDiscos, tipoGabinete);
            Agregar(nuevo);
            return nuevo;
        }

        public Impresora RegistrarImpresora(Cliente propietario, string serie, string marca, string modelo,
                                            string estadoFisico, List<string> accesorios, bool esLaser, int contadorPaginas)
        {
            ValidarComunes(propietario, serie, marca, modelo, estadoFisico);
            Validador.ExigirNoNegativo(contadorPaginas, "Contador de páginas");
            Impresora nueva = new Impresora(GeneradorCodigos.Siguiente("EQP"), serie, marca, modelo, propietario,
                                            estadoFisico, accesorios, esLaser, contadorPaginas);
            Agregar(nueva);
            return nueva;
        }

        private void ValidarComunes(Cliente propietario, string serie, string marca, string modelo, string estadoFisico)
        {
            if (propietario == null)
            {
                throw new ReglaNegocioException("Seleccione el cliente propietario del equipo.");
            }
            Validador.ExigirTexto(serie, "Número de serie");
            Validador.ExigirTexto(marca, "Marca");
            Validador.ExigirTexto(modelo, "Modelo");
            Validador.ExigirTexto(estadoFisico, "Estado físico recibido");

            if (ExisteSerie(serie))
            {
                throw new CodigoDuplicadoException("el número de serie", serie.Trim().ToUpper());
            }
        }

        private void Agregar(Equipo equipo)
        {
            if (BuscarPorCodigo(equipo.CodigoInterno) != null)
            {
                throw new CodigoDuplicadoException("el código", equipo.CodigoInterno);
            }
            equipos.Add(equipo);
            porTipo[(int)equipo.Tipo]++;
        }

        public bool ExisteSerie(string serie)
        {
            if (serie == null)
            {
                return false;
            }
            string buscada = serie.Trim().ToUpper();
            for (int i = 0; i < equipos.Count; i++)
            {
                if (equipos[i].NumeroSerie == buscada)
                {
                    return true;
                }
            }
            return false;
        }

        public Equipo BuscarPorCodigo(string codigo)
        {
            if (codigo == null)
            {
                return null;
            }
            string buscado = codigo.Trim().ToUpper();
            for (int i = 0; i < equipos.Count; i++)
            {
                if (equipos[i].CodigoInterno == buscado)
                {
                    return equipos[i];
                }
            }
            return null;
        }

        public List<Equipo> BuscarPorMarca(string texto)
        {
            return Busquedas.SecuencialEquiposPorMarca(equipos, texto);
        }

        public List<Solicitud> HistorialDeEquipo(string codigo)
        {
            List<Solicitud> historial = new List<Solicitud>();
            for (int i = 0; i < solicitudes.Count; i++)
            {
                if (solicitudes[i].Equipo.CodigoInterno == codigo)
                {
                    historial.Add(solicitudes[i]);
                }
            }
            return historial;
        }

        public List<Equipo> EquiposDeCliente(Cliente cliente)
        {
            List<Equipo> resultado = new List<Equipo>();
            for (int i = 0; i < equipos.Count; i++)
            {
                if (equipos[i].Propietario == cliente)
                {
                    resultado.Add(equipos[i]);
                }
            }
            return resultado;
        }

        public int[] ContarPorTipo()
        {
            int[] copia = new int[porTipo.Length];
            for (int i = 0; i < porTipo.Length; i++)
            {
                copia[i] = porTipo[i];
            }
            return copia;
        }

        public List<Equipo> Listar()
        {
            return equipos;
        }
    }
}
