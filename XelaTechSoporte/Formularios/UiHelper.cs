using System.Drawing.Drawing2D;
using System.Drawing.Text;
using XelaTechSoporte.Excepciones;
using XelaTechSoporte.Modelos;
using XelaTechSoporte.Servicios;

namespace XelaTechSoporte.Formularios
{
    public static class UiHelper
    {
        public static readonly Color ColorRojo = Color.FromArgb(220, 38, 38);
        public static readonly Color ColorMenu = Color.FromArgb(24, 24, 27);
        public static readonly Color ColorFondo = Color.FromArgb(243, 244, 246);
        public static readonly Color ColorTexto = Color.FromArgb(31, 41, 55);
        public static readonly Color ColorTextoSuave = Color.FromArgb(107, 114, 128);
        public static readonly Color ColorBorde = Color.FromArgb(229, 231, 235);
        public static readonly Color ColorSeleccion = Color.FromArgb(254, 226, 226);

        public static readonly Color EtiquetaGris = Color.FromArgb(209, 213, 219);
        public static readonly Color EtiquetaAmarilla = Color.FromArgb(250, 204, 21);
        public static readonly Color EtiquetaCeleste = Color.FromArgb(14, 165, 233);
        public static readonly Color EtiquetaAzul = Color.FromArgb(37, 99, 235);
        public static readonly Color EtiquetaMorada = Color.FromArgb(124, 58, 237);
        public static readonly Color EtiquetaTurquesa = Color.FromArgb(13, 148, 136);
        public static readonly Color EtiquetaVerde = Color.FromArgb(34, 197, 94);
        public static readonly Color EtiquetaGrisOscuro = Color.FromArgb(75, 85, 99);
        public static readonly Color EtiquetaNaranja = Color.FromArgb(245, 158, 11);

        private static readonly Font FuenteEtiqueta = new Font("Segoe UI", 8F, FontStyle.Bold);

        public const string ICONO_INICIO = "\uE80F";
        public const string ICONO_CLIENTES = "\uE77B";
        public const string ICONO_TECNICOS = "\uE90F";
        public const string ICONO_EQUIPOS = "\uE7F8";
        public const string ICONO_SOLICITUDES = "\uE7C3";
        public const string ICONO_REPUESTOS = "\uE7B8";
        public const string ICONO_COLAS = "\uE8FD";
        public const string ICONO_ORDENAR = "\uE8CB";
        public const string ICONO_BUSCAR = "\uE721";
        public const string ICONO_REPORTES = "\uE9F9";
        public const string ICONO_DATOS = "\uE9F5";
        public const string ICONO_AGREGAR = "\uE710";
        public const string ICONO_GUARDAR = "\uE74E";
        public const string ICONO_CANCELAR = "\uE711";
        public const string ICONO_DESHACER = "\uE7A7";
        public const string ICONO_ACTUALIZAR = "\uE72C";
        public const string ICONO_HISTORIAL = "\uE81C";
        public const string ICONO_ELIMINAR = "\uE74D";
        public const string ICONO_PERSONAS = "\uE716";
        public const string ICONO_IMPRIMIR = "\uE749";
        public const string ICONO_ALERTA = "\uE7BA";
        public const string ICONO_COBRAR = "\uE8C7";
        public const string ICONO_CHECK = "\uE73E";
        public const string ICONO_DIAGNOSTICO = "\uEADF";
        public const string ICONO_CAMBIAR = "\uE8AB";
        public const string ICONO_ESTRELLA = "\uE734";
        public const string ICONO_SIGUIENTE = "\uE768";
        public const string ICONO_LISTA = "\uE8A5";

        public static Bitmap Icono(string glifo, Color color, int tamano)
        {
            Bitmap imagen = new Bitmap(tamano + 4, tamano + 4);
            using (Graphics g = Graphics.FromImage(imagen))
            using (Font fuente = new Font("Segoe MDL2 Assets", tamano, FontStyle.Regular, GraphicsUnit.Pixel))
            using (SolidBrush pincel = new SolidBrush(color))
            using (StringFormat formato = new StringFormat())
            {
                g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
                formato.Alignment = StringAlignment.Center;
                formato.LineAlignment = StringAlignment.Center;
                g.DrawString(glifo, fuente, pincel, new RectangleF(0, 1, imagen.Width, imagen.Height), formato);
            }
            return imagen;
        }

        public static void PonerIcono(Button boton, string glifo, Color color)
        {
            boton.Image = Icono(glifo, color, 14);
            boton.ImageAlign = ContentAlignment.MiddleLeft;
            boton.TextImageRelation = TextImageRelation.ImageBeforeText;
        }

        public static void EstilizarRejilla(DataGridView rejilla)
        {
            rejilla.BackgroundColor = Color.White;
            rejilla.BorderStyle = BorderStyle.None;
            rejilla.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            rejilla.GridColor = Color.FromArgb(237, 238, 240);
            rejilla.EnableHeadersVisualStyles = false;
            rejilla.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            rejilla.ColumnHeadersDefaultCellStyle.BackColor = Color.White;
            rejilla.ColumnHeadersDefaultCellStyle.ForeColor = ColorTexto;
            rejilla.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.White;
            rejilla.ColumnHeadersDefaultCellStyle.SelectionForeColor = ColorTexto;
            rejilla.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            rejilla.ColumnHeadersDefaultCellStyle.Padding = new Padding(4, 0, 0, 0);
            rejilla.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.False;
            rejilla.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            rejilla.ColumnHeadersHeight = 34;
            rejilla.DefaultCellStyle.Font = new Font("Segoe UI", 9F);
            rejilla.DefaultCellStyle.ForeColor = ColorTexto;
            rejilla.DefaultCellStyle.SelectionBackColor = ColorSeleccion;
            rejilla.DefaultCellStyle.SelectionForeColor = ColorTexto;
            rejilla.DefaultCellStyle.Padding = new Padding(4, 0, 0, 0);
            rejilla.RowTemplate.Height = 30;
            rejilla.RowHeadersVisible = false;
            rejilla.AllowUserToAddRows = false;
            rejilla.AllowUserToDeleteRows = false;
            rejilla.AllowUserToResizeRows = false;
            rejilla.ReadOnly = true;
            rejilla.MultiSelect = false;
            rejilla.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            rejilla.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            rejilla.CellPainting -= PintarEtiqueta;
            rejilla.CellPainting += PintarEtiqueta;
        }

        public static void CrearColumnas(DataGridView rejilla, params string[] titulos)
        {
            rejilla.Rows.Clear();
            rejilla.Columns.Clear();
            for (int i = 0; i < titulos.Length; i++)
            {
                rejilla.Columns.Add("col" + i, titulos[i]);
                rejilla.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        public static void MostrarTabla(DataGridView rejilla, TablaReporte tabla)
        {
            CrearColumnas(rejilla, tabla.Columnas);
            for (int i = 0; i < tabla.Filas.Count; i++)
            {
                rejilla.Rows.Add(tabla.Filas[i]);
            }
            rejilla.ClearSelection();
        }

        public static void MarcarEtiqueta(DataGridViewCell celda, Color color)
        {
            celda.Tag = color;
        }

        private static void PintarEtiqueta(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
            {
                return;
            }
            DataGridView rejilla = (DataGridView)sender;
            object marca = rejilla.Rows[e.RowIndex].Cells[e.ColumnIndex].Tag;
            if (!(marca is Color))
            {
                return;
            }

            Color color = (Color)marca;
            string texto = Convert.ToString(e.FormattedValue);
            Font fuente = FuenteEtiqueta;

            e.PaintBackground(e.CellBounds, true);

            Size medida = TextRenderer.MeasureText(texto, fuente);
            int ancho = medida.Width + 14;
            if (ancho > e.CellBounds.Width - 10)
            {
                ancho = e.CellBounds.Width - 10;
            }
            Rectangle rect = new Rectangle(e.CellBounds.X + 6, e.CellBounds.Y + (e.CellBounds.Height - 20) / 2, ancho, 20);

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using (GraphicsPath forma = RectanguloRedondeado(rect, 9))
            using (SolidBrush pincel = new SolidBrush(color))
            {
                e.Graphics.FillPath(pincel, forma);
            }
            TextRenderer.DrawText(e.Graphics, texto, fuente, rect, ColorTextoSobre(color),
                                  TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);
            e.Handled = true;
        }

        private static GraphicsPath RectanguloRedondeado(Rectangle r, int radio)
        {
            int d = radio * 2;
            GraphicsPath forma = new GraphicsPath();
            forma.AddArc(r.X, r.Y, d, d, 180, 90);
            forma.AddArc(r.Right - d, r.Y, d, d, 270, 90);
            forma.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
            forma.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
            forma.CloseFigure();
            return forma;
        }

        private static Color ColorTextoSobre(Color fondo)
        {
            int brillo = (fondo.R * 299 + fondo.G * 587 + fondo.B * 114) / 1000;
            if (brillo > 165)
            {
                return ColorTexto;
            }
            return Color.White;
        }

        public static Color ColorEstado(EstadoSolicitud estado)
        {
            switch (estado)
            {
                case EstadoSolicitud.Recibida: return EtiquetaGris;
                case EstadoSolicitud.EnEspera: return EtiquetaAmarilla;
                case EstadoSolicitud.Asignada: return EtiquetaCeleste;
                case EstadoSolicitud.EnDiagnostico: return EtiquetaAzul;
                case EstadoSolicitud.EnReparacion: return EtiquetaMorada;
                case EstadoSolicitud.ListaParaEntregar: return EtiquetaTurquesa;
                case EstadoSolicitud.Entregada: return EtiquetaVerde;
                default: return EtiquetaGrisOscuro;
            }
        }

        public static Color ColorPrioridad(NivelPrioridad prioridad)
        {
            if (prioridad == NivelPrioridad.Urgente)
            {
                return ColorRojo;
            }
            return EtiquetaGris;
        }

        public static void Aviso(string mensaje)
        {
            MessageBox.Show(mensaje, "XelaTech Soporte", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        public static void Error(string mensaje)
        {
            MessageBox.Show(mensaje, "XelaTech Soporte", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static void Info(string mensaje)
        {
            MessageBox.Show(mensaje, "XelaTech Soporte", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public static bool Confirmar(string mensaje)
        {
            DialogResult respuesta = MessageBox.Show(mensaje, "XelaTech Soporte", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            return respuesta == DialogResult.Yes;
        }

        public static int LeerEntero(TextBox caja, string campo, int minimo, int maximo)
        {
            int valor;
            if (!int.TryParse(caja.Text.Trim(), out valor))
            {
                caja.Focus();
                throw new ReglaNegocioException("El campo \"" + campo + "\" debe ser un número entero.");
            }
            if (valor < minimo || valor > maximo)
            {
                caja.Focus();
                throw new ReglaNegocioException("El campo \"" + campo + "\" debe estar entre " + minimo + " y " + maximo + ".");
            }
            return valor;
        }

        public static decimal LeerDecimal(TextBox caja, string campo, decimal minimo)
        {
            decimal valor;
            if (!decimal.TryParse(caja.Text.Trim(), out valor))
            {
                caja.Focus();
                throw new ReglaNegocioException("El campo \"" + campo + "\" debe ser un número. Ejemplo: 150.50");
            }
            if (valor < minimo)
            {
                caja.Focus();
                if (minimo == 0)
                {
                    throw new ReglaNegocioException("El campo \"" + campo + "\" no puede ser negativo.");
                }
                throw new ReglaNegocioException("El campo \"" + campo + "\" debe ser mayor o igual a " + minimo + ".");
            }
            return valor;
        }

        public static string LeerTexto(TextBox caja, string campo)
        {
            if (!Validador.TextoObligatorio(caja.Text))
            {
                caja.Focus();
                throw new ReglaNegocioException("El campo \"" + campo + "\" es obligatorio.");
            }
            return caja.Text.Trim();
        }
    }
}
