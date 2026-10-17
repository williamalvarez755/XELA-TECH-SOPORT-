using XelaTechSoporte.Excepciones;

namespace XelaTechSoporte.Servicios
{
    public static class Validador
    {
        public static bool TextoObligatorio(string texto)
        {
            return texto != null && texto.Trim().Length > 0;
        }

        public static bool NumeroNoNegativo(decimal numero)
        {
            return numero >= 0;
        }

        public static bool LongitudMinima(string texto, int minimo)
        {
            return texto != null && texto.Trim().Length >= minimo;
        }

        public static bool CorreoValido(string correo)
        {
            if (!TextoObligatorio(correo))
            {
                return false;
            }
            string c = correo.Trim();
            if (c.Contains(" "))
            {
                return false;
            }
            int posicionArroba = c.IndexOf('@');
            if (posicionArroba <= 0)
            {
                return false;
            }
            if (c.IndexOf('@', posicionArroba + 1) != -1)
            {
                return false;
            }
            int posicionPunto = c.IndexOf('.', posicionArroba);
            return posicionPunto > posicionArroba + 1 && posicionPunto < c.Length - 1;
        }

        public static bool TelefonoValido(string telefono)
        {
            if (telefono == null)
            {
                return false;
            }
            string t = telefono.Trim();
            if (t.Length != 8)
            {
                return false;
            }
            for (int i = 0; i < t.Length; i++)
            {
                if (!char.IsDigit(t[i]))
                {
                    return false;
                }
            }
            return true;
        }

        public static bool RangoCalificacion(int estrellas)
        {
            return estrellas >= 1 && estrellas <= 5;
        }

        public static bool FechaNoFutura(DateTime fecha)
        {
            return fecha.Date <= DateTime.Today;
        }

        public static void ExigirTexto(string valor, string campo)
        {
            if (!TextoObligatorio(valor))
            {
                throw new ReglaNegocioException("El campo \"" + campo + "\" es obligatorio.");
            }
        }

        public static void ExigirNoNegativo(decimal valor, string campo)
        {
            if (!NumeroNoNegativo(valor))
            {
                throw new ReglaNegocioException("El campo \"" + campo + "\" no puede ser negativo.");
            }
        }

        public static void ValidarDatosPersona(string nombre, string telefono, string correo)
        {
            ExigirTexto(nombre, "Nombre completo");
            ExigirTexto(telefono, "Teléfono");
            if (!TelefonoValido(telefono))
            {
                throw new ReglaNegocioException("El teléfono debe tener exactamente 8 dígitos (formato de Guatemala).");
            }
            ExigirTexto(correo, "Correo electrónico");
            if (!CorreoValido(correo))
            {
                throw new ReglaNegocioException("El correo debe tener una arroba (@) y un punto después de ella. Ejemplo: ana@correo.com");
            }
        }
    }
}
