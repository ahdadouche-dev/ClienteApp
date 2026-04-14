using ClientesApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ClientesApp.Domain.Validators
{
    public static class ClientValidator
    {
        public static bool EsDniValido(string dni)
        {
            return !string.IsNullOrWhiteSpace(dni);
        }

        public static bool EsEmailValido(string email)
        {
            return !string.IsNullOrWhiteSpace(email) &&
            System.Text.RegularExpressions.Regex.IsMatch(email,
                @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        }

        public static bool EsTelefonoValido(string telefono)
        {
            return !string.IsNullOrWhiteSpace(telefono) &&
            System.Text.RegularExpressions.Regex.IsMatch(telefono, @"^\+?[\d\s\-]{7,15}$");
        }

        public static bool EsFechaValida(DateTime fecha)
        {
            return fecha < DateTime.Now && fecha > new DateTime(1900, 1, 1);
        }

        public static (bool esValido, List<string> errores) Validar(Cliente cliente)
        {
            var errores = new List<string>();

            if (!EsDniValido(cliente.Dni))
                errores.Add("El DNI no puede estar vacío.");

            if (!EsEmailValido(cliente.Email))
                errores.Add("El email no tiene un formato válido.");

            if (!EsTelefonoValido(cliente.Telefono))
                errores.Add("El teléfono no tiene un formato válido.");

            if (!EsFechaValida(cliente.FechaNacimiento))
                errores.Add("La fecha de nacimiento no es válida.");

            return (!errores.Any(), errores);
        }
    }
}
