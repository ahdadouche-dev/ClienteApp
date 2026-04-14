using ClientesApp.Api.Services;
using ClientesApp.Domain.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;

namespace ClientesApp.Tests
{
    public class ClienteServiceTests : IDisposable
    {
        private readonly ClienteService _service;
        private readonly string _ficheroTemp;

        public ClienteServiceTests()
        {
            _ficheroTemp = Path.GetTempFileName();
            File.WriteAllText(_ficheroTemp, "[]");

            var config = new Mock<IConfiguration>();
            config.Setup(c => c["DataFile"]).Returns(_ficheroTemp);

            var logger = new Mock<ILogger<ClienteService>>();
            _service = new ClienteService(config.Object, logger.Object);
        }

        public void Dispose()
        {
            if (File.Exists(_ficheroTemp))
                File.Delete(_ficheroTemp);
        }

        [Fact]
        public void ObtenerTodos_FicheroVacio_RetornaListaVacia()
        {
            var result = _service.ObtenerTodos();
            Assert.Empty(result);
        }

        [Fact]
        public void Crear_ClienteValido_SeAgregaCorrectamente()
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

            var (ok, errores) = _service.Crear(cliente);

            Assert.True(ok);
            Assert.Empty(errores);
            Assert.Single(_service.ObtenerTodos());
        }

        [Fact]
        public void Crear_ClienteDuplicado_RetornaError()
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

            _service.Crear(cliente);
            var (ok, errores) = _service.Crear(cliente);

            Assert.False(ok);
            Assert.Contains(errores, e => e.Contains("DNI"));
        }

        [Fact]
        public void ObtenerPorDni_ClienteExistente_RetornaCliente()
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

            _service.Crear(cliente);
            var result = _service.ObtenerPorDni("12345678A");

            Assert.NotNull(result);
            Assert.Equal("Juan", result.Nombre);
        }

        [Fact]
        public void ObtenerPorDni_ClienteNoExistente_RetornaNull()
        {
            var result = _service.ObtenerPorDni("99999999Z");
            Assert.Null(result);
        }

        [Fact]
        public void Eliminar_ClienteExistente_RetornaTrue()
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

            _service.Crear(cliente);
            var result = _service.Eliminar("12345678A");

            Assert.True(result);
            Assert.Empty(_service.ObtenerTodos());
        }

        [Fact]
        public void Eliminar_ClienteNoExistente_RetornaFalse()
        {
            var result = _service.Eliminar("99999999Z");
            Assert.False(result);
        }
    }
}