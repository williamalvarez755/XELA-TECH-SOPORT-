using XelaTechSoporte.Estructuras;
using XelaTechSoporte.Excepciones;
using XelaTechSoporte.Modelos;

namespace XelaTechSoporte.Servicios
{
    public class GeneradorDatos
    {
        private CentroServicio centro;
        private Random random;

        private static readonly string[] nombres =
        {
            "Juan", "Ana", "Carlos", "María", "Luis", "Sofía", "Diego", "Valeria", "Jorge", "Paula",
            "Andrés", "Lucía", "Fernando", "Gabriela", "Ricardo", "Daniela", "José", "Camila", "Miguel", "Andrea"
        };

        private static readonly string[] apellidos =
        {
            "Pérez", "García", "Ruiz", "López", "Fernández", "Ramírez", "Morales", "Castillo", "Salazar",
            "Gómez", "Martínez", "Torres", "Hernández", "Cifuentes", "Barrios", "Monzón", "Ochoa", "Velásquez"
        };

        private static readonly string[] marcasLaptop = { "Dell", "HP", "Lenovo", "Acer", "Asus", "Lenovo", "HP" };
        private static readonly string[] modelosLaptop = { "Inspiron 15", "Pavilion 14", "IdeaPad 3", "Aspire 5", "VivoBook 15", "ThinkPad E14", "ProBook 440" };

        private static readonly string[] marcasEscritorio = { "Dell", "HP", "Lenovo", "Acer", "Genérico" };
        private static readonly string[] modelosEscritorio = { "OptiPlex 3080", "ProDesk 400", "ThinkCentre M70", "Veriton X", "Clon Core i5" };

        private static readonly string[] marcasImpresora = { "Epson", "HP", "Canon", "Brother", "HP", "Epson" };
        private static readonly string[] modelosImpresora = { "L3250", "LaserJet Pro M404", "PIXMA G3110", "HL-1212W", "DeskJet 2775", "L4260" };
        private static readonly bool[] impresoraEsLaser = { false, true, false, true, false, false };

        private static readonly string[] estadosFisicos =
        {
            "Buen estado", "Rayones en la carcasa", "Golpe en una esquina", "Carcasa con mucho polvo",
            "Sin daños visibles", "Tornillos faltantes", "Bisagra floja"
        };

        private static readonly string[] accesoriosPosibles = { "Cargador", "Mouse", "Estuche", "Cable de poder", "Cable USB", "Teclado" };

        private static readonly string[] problemasComputadora =
        {
            "No enciende, posible problema en la fuente.",
            "Se calienta mucho y se apaga sola.",
            "Pantalla azul al iniciar Windows.",
            "Está muy lenta al abrir cualquier programa.",
            "No reconoce el disco duro al encender.",
            "No se conecta a la red WiFi de la oficina.",
            "Varias teclas del teclado no funcionan.",
            "El ventilador hace un ruido extraño."
        };

        private static readonly string[] problemasImpresora =
        {
            "Atasca el papel en cada impresión.",
            "Imprime con rayas y manchas de tinta.",
            "No la reconoce la computadora por USB.",
            "Muestra error de cartucho aunque es nuevo.",
            "No jala las hojas de la bandeja."
        };

        private static readonly string[] nombresRepuesto =
        {
            "Disco SSD 480GB", "Disco duro 1TB", "Memoria RAM 8GB DDR4", "Memoria RAM 16GB DDR4",
            "Fuente de poder 500W", "Cargador universal para laptop", "Pasta térmica", "Tinta negra",
            "Tóner HP 58A", "Cabezal de impresión", "Teclado USB", "Mouse óptico", "Batería para laptop",
            "Ventilador de CPU", "Cable SATA"
        };

        private static readonly CategoriaRepuesto[] categoriasRepuesto =
        {
            CategoriaRepuesto.Almacenamiento, CategoriaRepuesto.Almacenamiento, CategoriaRepuesto.Memoria, CategoriaRepuesto.Memoria,
            CategoriaRepuesto.Fuente, CategoriaRepuesto.Fuente, CategoriaRepuesto.Consumible, CategoriaRepuesto.Consumible,
            CategoriaRepuesto.Consumible, CategoriaRepuesto.Accesorio, CategoriaRepuesto.Accesorio, CategoriaRepuesto.Accesorio,
            CategoriaRepuesto.Fuente, CategoriaRepuesto.Accesorio, CategoriaRepuesto.Accesorio
        };

        public GeneradorDatos(CentroServicio centro)
        {
            this.centro = centro;
            random = new Random();
        }

        public GeneradorDatos(CentroServicio centro, int semilla)
        {
            this.centro = centro;
            random = new Random(semilla);
        }

        public string Generar(int cantClientes, int cantTecnicos, int cantEquipos, int cantRepuestos, int cantSolicitudes)
        {
            string resumen = "";

            int clientesCreados = GenerarClientes(cantClientes);
            resumen += "Clientes creados: " + clientesCreados + "\r\n";

            int tecnicosCreados = GenerarTecnicos(cantTecnicos);
            resumen += "Técnicos creados: " + tecnicosCreados + "\r\n";

            int equiposCreados = GenerarEquipos(cantEquipos);
            resumen += "Equipos creados: " + equiposCreados + "\r\n";

            int repuestosCreados = GenerarRepuestos(cantRepuestos);
            resumen += "Repuestos creados: " + repuestosCreados + "\r\n";

            int solicitudesCreadas = GenerarSolicitudes(cantSolicitudes);
            resumen += "Solicitudes creadas: " + solicitudesCreadas;
            if (solicitudesCreadas < cantSolicitudes)
            {
                resumen += " (no había más equipos libres: cada equipo solo puede tener una solicitud activa)";
            }
            resumen += "\r\n";

            resumen += AtenderAlgunas(solicitudesCreadas);
            return resumen;
        }

        private int GenerarClientes(int cantidad)
        {
            int creados = 0;
            for (int i = 0; i < cantidad; i++)
            {
                string nombre = Azar(nombres);
                string apellido = Azar(apellidos);
                string nombreCompleto = nombre + " " + apellido;
                string correo = QuitarTildes(nombre + "." + apellido).ToLower() + random.Next(10, 100) + "@correo.com";
                TipoCliente tipo = (TipoCliente)random.Next(0, 4);

                centro.Clientes.Registrar(nombreCompleto, TelefonoAlAzar(), correo, tipo);
                creados++;
            }
            return creados;
        }

        private int GenerarTecnicos(int cantidad)
        {
            Especialidad[] reparto = { Especialidad.Hardware, Especialidad.Hardware, Especialidad.Software,
                                       Especialidad.Impresoras, Especialidad.Redes };
            int creados = 0;
            for (int i = 0; i < cantidad; i++)
            {
                Especialidad especialidad;
                if (i < 4)
                {
                    especialidad = (Especialidad)i;
                }
                else
                {
                    especialidad = reparto[random.Next(reparto.Length)];
                }

                string nombre = Azar(nombres);
                string apellido = Azar(apellidos);
                string correo = QuitarTildes(nombre.Substring(0, 1) + apellido).ToLower() + random.Next(10, 100) + "@xelatech.com";
                Tecnico tecnico = centro.Tecnicos.Registrar(nombre + " " + apellido, TelefonoAlAzar(), correo, especialidad);

                int calificaciones = random.Next(0, 5);
                for (int c = 0; c < calificaciones; c++)
                {
                    tecnico.Calificar(random.Next(3, 6));
                }
                creados++;
            }
            return creados;
        }

        private int GenerarEquipos(int cantidad)
        {
            List<Cliente> clientes = centro.Clientes.Listar();
            if (clientes.Count == 0)
            {
                return 0;
            }

            int creados = 0;
            for (int i = 0; i < cantidad; i++)
            {
                Cliente propietario = clientes[random.Next(clientes.Count)];
                string serie = SerieUnica();
                string estado = Azar(estadosFisicos);
                List<string> accesorios = AccesoriosAlAzar();

                int tipo = random.Next(0, 100);
                if (tipo < 45)
                {
                    int m = random.Next(marcasLaptop.Length);
                    double[] pantallas = { 13.3, 14, 15.6, 17.3 };
                    centro.Equipos.RegistrarLaptop(propietario, serie, marcasLaptop[m], modelosLaptop[m], estado, accesorios,
                                                   random.Next(0, 100) < 85, pantallas[random.Next(pantallas.Length)]);
                }
                else if (tipo < 75)
                {
                    int m = random.Next(marcasEscritorio.Length);
                    centro.Equipos.RegistrarEscritorio(propietario, serie, marcasEscritorio[m], modelosEscritorio[m], estado, accesorios,
                                                       random.Next(1, 4), Escritorio.TiposGabinete[random.Next(Escritorio.TiposGabinete.Length)]);
                }
                else
                {
                    int m = random.Next(marcasImpresora.Length);
                    centro.Equipos.RegistrarImpresora(propietario, serie, marcasImpresora[m], modelosImpresora[m], estado, accesorios,
                                                      impresoraEsLaser[m], random.Next(500, 80000));
                }
                creados++;
            }
            return creados;
        }

        private int GenerarRepuestos(int cantidad)
        {
            int creados = 0;
            int intentos = 0;
            while (creados < cantidad && intentos < cantidad * 5)
            {
                intentos++;
                int indice = random.Next(nombresRepuesto.Length);
                string nombre = nombresRepuesto[indice];

                decimal costo = random.Next(15, 600);
                decimal precio = Math.Round(costo * (decimal)(1.25 + random.NextDouble() * 0.35), 2);
                int existencia = random.Next(0, 26);
                int minimo = random.Next(2, 7);

                try
                {
                    centro.Repuestos.Registrar(nombre, categoriasRepuesto[indice], existencia, minimo, costo, precio);
                    creados++;
                }
                catch (CodigoDuplicadoException)
                {
                }
            }
            return creados;
        }

        private int GenerarSolicitudes(int cantidad)
        {
            List<Equipo> libres = new List<Equipo>();
            List<Equipo> equipos = centro.Equipos.Listar();
            for (int i = 0; i < equipos.Count; i++)
            {
                if (centro.Solicitudes.BuscarActivaDeEquipo(equipos[i]) == null)
                {
                    libres.Add(equipos[i]);
                }
            }

            int creadas = 0;
            while (creadas < cantidad && libres.Count > 0)
            {
                int posicion = random.Next(libres.Count);
                Equipo equipo = libres[posicion];
                libres.RemoveAt(posicion);

                NivelPrioridad prioridad = NivelPrioridad.Normal;
                if (random.Next(0, 100) < 30)
                {
                    prioridad = NivelPrioridad.Urgente;
                }

                string problema;
                Especialidad especialidad = equipo.EspecialidadSugerida();
                if (equipo.Tipo == TipoEquipo.Impresora)
                {
                    problema = Azar(problemasImpresora);
                }
                else
                {
                    problema = Azar(problemasComputadora);
                    int r = random.Next(0, 100);
                    if (r < 20)
                    {
                        especialidad = Especialidad.Software;
                    }
                    else if (r < 28)
                    {
                        especialidad = Especialidad.Redes;
                    }
                }

                DateTime fecha = DateTime.Now.AddMinutes(-random.Next(30, 5 * 24 * 60));

                centro.Solicitudes.Crear(equipo.Propietario, equipo, problema, prioridad, especialidad, fecha);
                creadas++;
            }
            return creadas;
        }

        private string AtenderAlgunas(int creadas)
        {
            int aAtender = creadas * 7 / 10;
            int atendidas = 0;
            string aviso = "";

            for (int k = 0; k < aAtender; k++)
            {
                Solicitud turno = centro.Solicitudes.VerSiguienteEnCola();
                if (turno == null)
                {
                    break;
                }
                Tecnico tecnico = centro.Tecnicos.AsignarMejorTecnico(turno.EspecialidadRequerida);
                if (tecnico == null)
                {
                    aviso = "La cola se detuvo en " + turno.Codigo + ": no hay técnico libre de " +
                            Textos.NombreEspecialidad(turno.EspecialidadRequerida) + ".\r\n";
                    break;
                }

                int dia = random.Next(0, MatrizCarga.DIAS);
                centro.Solicitudes.AsignarSiguiente(tecnico, dia);
                AvanzarAlAzar(turno);
                atendidas++;
            }
            return "Solicitudes atendidas desde la cola: " + atendidas + "\r\n" + aviso;
        }

        private void AvanzarAlAzar(Solicitud s)
        {
            int destino = random.Next(0, 100);
            if (destino < 15)
            {
                return;
            }

            string[] checklist = s.Equipo.ObtenerChecklist();
            List<string> acciones = new List<string>();
            acciones.Add(checklist[random.Next(checklist.Length)]);
            centro.Solicitudes.RegistrarDiagnostico(s, "Revisión general: " + s.DescripcionProblema, acciones, random.Next(1, 13));
            if (destino < 30)
            {
                return;
            }

            List<Repuesto> repuestos = centro.Repuestos.Listar();
            int cuantos = random.Next(0, 3);
            for (int i = 0; i < cuantos && repuestos.Count > 0; i++)
            {
                Repuesto r = repuestos[random.Next(repuestos.Count)];
                int cantidad = random.Next(1, 3);
                if (r.HayExistencia(cantidad))
                {
                    centro.Solicitudes.AgregarRepuesto(s, r, cantidad);
                }
            }

            if (destino < 45)
            {
                centro.Solicitudes.CambiarEstado(s, EstadoSolicitud.EnReparacion);
                return;
            }
            if (destino < 55)
            {
                centro.Solicitudes.Cancelar(s);
                return;
            }

            centro.Solicitudes.CambiarEstado(s, EstadoSolicitud.ListaParaEntregar);
            if (destino < 70)
            {
                return;
            }

            decimal manoObra = s.CostoManoObra + random.Next(0, 6) * 25;
            centro.Solicitudes.Entregar(s, manoObra, random.Next(3, 6));
        }

        private string Azar(string[] arreglo)
        {
            return arreglo[random.Next(arreglo.Length)];
        }

        private string TelefonoAlAzar()
        {
            string[] inicios = { "3", "4", "5" };
            return Azar(inicios) + random.Next(1000000, 10000000).ToString();
        }

        private string SerieUnica()
        {
            string letras = "ABCDEFGHJKLMNPQRSTUVWXYZ";
            string serie;
            do
            {
                serie = "SN";
                for (int i = 0; i < 3; i++)
                {
                    serie += letras[random.Next(letras.Length)];
                }
                serie += random.Next(10000, 100000).ToString();
            }
            while (centro.Equipos.ExisteSerie(serie));
            return serie;
        }

        private List<string> AccesoriosAlAzar()
        {
            List<string> lista = new List<string>();
            int cuantos = random.Next(0, 3);
            for (int i = 0; i < cuantos; i++)
            {
                string accesorio = Azar(accesoriosPosibles);
                if (!lista.Contains(accesorio))
                {
                    lista.Add(accesorio);
                }
            }
            return lista;
        }

        private static string QuitarTildes(string texto)
        {
            string resultado = texto.Replace("á", "a").Replace("é", "e").Replace("í", "i").Replace("ó", "o").Replace("ú", "u");
            resultado = resultado.Replace("Á", "A").Replace("É", "E").Replace("Í", "I").Replace("Ó", "O").Replace("Ú", "U");
            resultado = resultado.Replace("ñ", "n").Replace("Ñ", "N");
            return resultado;
        }
    }
}
