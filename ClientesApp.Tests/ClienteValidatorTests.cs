using ClientesApp.Domain.Models;
using ClientesApp.Domain.Validators;

namespace ClientesApp.Tests
{
    public class ClienteValidatorTests
    {
        [Fact]
        public void Validar_ClienteCorrecto_RetornaValido()
        {
            var cliente = new Cliente
            {
                Dni = "12345678A",
                Nombre = "Juan",
                Apellidos = "García",
                FechaNacimiento = new DateTime(1990, 1, 1),
                Telefono = "612345678",
                Email = "juan@email.com"
            };

            var (esValido, errores) = ClientValidator.Validar(cliente);

            Assert.True(esValido);
            Assert.Empty(errores);
        }

        [Fact]
        public void Validar_DniVacio_RetornaError()
        {
            var cliente = new Cliente { Dni = "", Email = "juan@email.com", Telefono = "612345678", FechaNacimiento = new DateTime(1990, 1, 1) };

            var (esValido, errores) = ClientValidator.Validar(cliente);

            Assert.False(esValido);
            Assert.Contains(errores, e => e.Contains("DNI"));
        }

        [Fact]
        public void Validar_EmailInvalido_RetornaError()
        {
            var cliente = new Cliente { Dni = "12345678A", Email = "noesunemail", Telefono = "612345678", FechaNacimiento = new DateTime(1990, 1, 1) };

            var (esValido, errores) = ClientValidator.Validar(cliente);

            Assert.False(esValido);
            Assert.Contains(errores, e => e.Contains("email"));
        }

        [Fact]
        public void Validar_TelefonoInvalido_RetornaError()
        {
            var cliente = new Cliente { Dni = "12345678A", Email = "juan@email.com", Telefono = "123", FechaNacimiento = new DateTime(1990, 1, 1) };

            var (esValido, errores) = ClientValidator.Validar(cliente);

            Assert.False(esValido);
            Assert.Contains(errores, e => e.Contains("teléfono"));
        }

        [Fact]
        public void Validar_FechaInvalida_RetornaError()
        {
            var cliente = new Cliente { Dni = "12345678A", Email = "juan@email.com", Telefono = "612345678", FechaNacimiento = DateTime.Now.AddDays(1) };

            var (esValido, errores) = ClientValidator.Validar(cliente);

            Assert.False(esValido);
            Assert.Contains(errores, e => e.Contains("fecha"));
        }
    }
}
