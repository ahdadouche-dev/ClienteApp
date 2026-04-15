using ClientesApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ClientesApp.Domain.Validators
{
    public static class ClientValidator
    {
        public static bool EsDniValido(string dni, Pais pais)
        {
            if (string.IsNullOrWhiteSpace(dni)) return false;

            if (pais == Pais.España)
                return Regex.IsMatch(dni, @"^\d{8}[A-Za-z]$");
            else if (pais == Pais.Francia)
                return Regex.IsMatch(dni, @"^\d{13}$");
            else if (pais == Pais.Portugal)
                return Regex.IsMatch(dni, @"^\d{9}$");
            else
                return false;
        }

        public static bool EsEmailValido(string email)
        {
            email = email.ToLower();
            return !string.IsNullOrWhiteSpace(email) &&
            System.Text.RegularExpressions.Regex.IsMatch(email,
                @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        }

        public static bool EsTelefonoValido(string telefono, Pais pais)
        {
            if (string.IsNullOrWhiteSpace(telefono)) return false;

            if (pais == Pais.España)
                return Regex.IsMatch(telefono, @"^[6789]\d{8}$");
            else if (pais == Pais.Francia)
                return Regex.IsMatch(telefono, @"^0\d{9}$");
            else if (pais == Pais.Portugal)
                return Regex.IsMatch(telefono, @"^9\d{8}$");
            else
                return false;
        }

        public static bool EsFechaValida(DateTime fecha)
        {
            return fecha < DateTime.Now && fecha > new DateTime(1900, 1, 1);
        }

        public static (bool esValido, List<string> errores) Validar(Cliente cliente)
        {
            var errores = new List<string>();

            if (!EsDniValido(cliente.Dni, cliente.Pais))
                errores.Add("El DNI no tiene un formato válido.");

            if (!EsEmailValido(cliente.Email))
                errores.Add("El email no tiene un formato válido.");

            if (!EsTelefonoValido(cliente.Telefono, cliente.Pais))
                errores.Add("El teléfono no tiene un formato válido.");

            if (!EsFechaValida(cliente.FechaNacimiento))
                errores.Add("La fecha de nacimiento no es válida.");

            return (!errores.Any(), errores);
        }
    }
}
