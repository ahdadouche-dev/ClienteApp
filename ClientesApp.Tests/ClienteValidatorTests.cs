using ClientesApp.Domain.Models;
using ClientesApp.Domain.Validators;

namespace ClientesApp.Tests
{
    public class ClienteValidatorTests
    {
        // Tests generales
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
                Email = "juan@email.com",
                Pais = Pais.España
            };

            var (esValido, errores) = ClientValidator.Validar(cliente);

            Assert.True(esValido);
            Assert.Empty(errores);
        }

        [Fact]
        public void Validar_DniVacio_RetornaError()
        {
            var cliente = new Cliente
            {
                Dni = "",
                Email = "juan@email.com",
                Telefono = "612345678",
                FechaNacimiento = new DateTime(1990, 1, 1),
                Pais = Pais.España
            };

            var (esValido, errores) = ClientValidator.Validar(cliente);

            Assert.False(esValido);
            Assert.Contains(errores, e => e.Contains("DNI"));
        }

        [Fact]
        public void Validar_EmailInvalido_RetornaError()
        {
            var cliente = new Cliente
            {
                Dni = "12345678A",
                Email = "noesunemail",
                Telefono = "612345678",
                FechaNacimiento = new DateTime(1990, 1, 1),
                Pais = Pais.España
            };

            var (esValido, errores) = ClientValidator.Validar(cliente);

            Assert.False(esValido);
            Assert.Contains(errores, e => e.Contains("email"));
        }

        [Fact]
        public void Validar_TelefonoInvalido_RetornaError()
        {
            var cliente = new Cliente
            {
                Dni = "12345678A",
                Email = "juan@email.com",
                Telefono = "123",
                FechaNacimiento = new DateTime(1990, 1, 1),
                Pais = Pais.España
            };

            var (esValido, errores) = ClientValidator.Validar(cliente);

            Assert.False(esValido);
            Assert.Contains(errores, e => e.Contains("teléfono"));
        }

        [Fact]
        public void Validar_FechaInvalida_RetornaError()
        {
            var cliente = new Cliente
            {
                Dni = "12345678A",
                Email = "juan@email.com",
                Telefono = "612345678",
                FechaNacimiento = DateTime.Now.AddDays(1),
                Pais = Pais.España
            };

            var (esValido, errores) = ClientValidator.Validar(cliente);

            Assert.False(esValido);
            Assert.Contains(errores, e => e.Contains("fecha"));
        }

        // Tests para España
        [Fact]
        public void Validar_DniEspanaValido_RetornaValido()
        {
            var cliente = new Cliente
            {
                Dni = "12345678A",
                Nombre = "Juan",
                Apellidos = "García",
                FechaNacimiento = new DateTime(1990, 1, 1),
                Telefono = "612345678",
                Email = "juan@email.com",
                Pais = Pais.España
            };

            var (esValido, errores) = ClientValidator.Validar(cliente);

            Assert.True(esValido);
            Assert.Empty(errores);
        }

        [Fact]
        public void Validar_DniEspanaInvalido_RetornaError()
        {
            var cliente = new Cliente
            {
                Dni = "1234",
                Nombre = "Juan",
                Apellidos = "García",
                FechaNacimiento = new DateTime(1990, 1, 1),
                Telefono = "612345678",
                Email = "juan@email.com",
                Pais = Pais.España
            };

            var (esValido, errores) = ClientValidator.Validar(cliente);

            Assert.False(esValido);
            Assert.Contains(errores, e => e.Contains("DNI"));
        }

        // Tests para Francia
        [Fact]
        public void Validar_DniFranciaValido_RetornaValido()
        {
            var cliente = new Cliente
            {
                Dni = "1234567890123",
                Nombre = "Pierre",
                Apellidos = "Dupont",
                FechaNacimiento = new DateTime(1988, 7, 10),
                Telefono = "0612345678",
                Email = "pierre@email.fr",
                Pais = Pais.Francia
            };

            var (esValido, errores) = ClientValidator.Validar(cliente);

            Assert.True(esValido);
            Assert.Empty(errores);
        }

        [Fact]
        public void Validar_TelefonoFranciaInvalido_RetornaError()
        {
            var cliente = new Cliente
            {
                Dni = "1234567890123",
                Nombre = "Pierre",
                Apellidos = "Dupont",
                FechaNacimiento = new DateTime(1988, 7, 10),
                Telefono = "612345678",
                Email = "pierre@email.fr",
                Pais = Pais.Francia
            };

            var (esValido, errores) = ClientValidator.Validar(cliente);

            Assert.False(esValido);
            Assert.Contains(errores, e => e.Contains("teléfono"));
        }

        // Tests para Portugal
        [Fact]
        public void Validar_DniPortugalValido_RetornaValido()
        {
            var cliente = new Cliente
            {
                Dni = "123456789",
                Nombre = "João",
                Apellidos = "Silva",
                FechaNacimiento = new DateTime(1992, 11, 30),
                Telefono = "912345678",
                Email = "joao@email.pt",
                Pais = Pais.Portugal
            };

            var (esValido, errores) = ClientValidator.Validar(cliente);

            Assert.True(esValido);
            Assert.Empty(errores);
        }

        [Fact]
        public void Validar_TelefonoPortugalInvalido_RetornaError()
        {
            var cliente = new Cliente
            {
                Dni = "123456789",
                Nombre = "João",
                Apellidos = "Silva",
                FechaNacimiento = new DateTime(1992, 11, 30),
                Telefono = "612345678",
                Email = "joao@email.pt",
                Pais = Pais.Portugal
            };

            var (esValido, errores) = ClientValidator.Validar(cliente);

            Assert.False(esValido);
            Assert.Contains(errores, e => e.Contains("teléfono"));
        }
    }
}