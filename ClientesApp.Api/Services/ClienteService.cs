using System.Text.Json;
using ClientesApp.Domain.Models;
using ClientesApp.Domain.Validators;

namespace ClientesApp.Api.Services
{
    public class ClienteService : IClienteService
    {
        private readonly string _rutaFichero;
        private readonly ILogger<ClienteService> _logger;

        public ClienteService(IConfiguration config, ILogger<ClienteService> logger)
        {
            _logger = logger;
            _rutaFichero = config["DataFile"] ?? "Data/clientes.json";

            var carpeta = Path.GetDirectoryName(_rutaFichero);
            if (!string.IsNullOrEmpty(carpeta) && !Directory.Exists(carpeta))
                Directory.CreateDirectory(carpeta);

            if (!File.Exists(_rutaFichero))
                File.WriteAllText(_rutaFichero, "[]");
        }

        public List<Cliente> ObtenerTodos()
        {
            try
            {
                var json = File.ReadAllText(_rutaFichero);
                return JsonSerializer.Deserialize<List<Cliente>>(json) ?? [];
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al leer el fichero de clientes");
                return [];
            }
        }

        public Cliente? ObtenerPorDni(string dni)
        {
            return ObtenerTodos().FirstOrDefault(c =>
                c.Dni.Equals(dni, StringComparison.OrdinalIgnoreCase));
        }

        public (bool ok, List<string> errores) Crear(Cliente cliente)
        {
            var (esValido, errores) = ClientValidator.Validar(cliente);
            if (!esValido) return (false, errores);

            var clientes = ObtenerTodos();

            if (clientes.Any(c => c.Dni.Equals(cliente.Dni, StringComparison.OrdinalIgnoreCase)))
                return (false, ["Ya existe un cliente con ese DNI."]);

            clientes.Add(cliente);
            Guardar(clientes);
            return (true, []);
        }

        public bool Eliminar(string dni)
        {
            var clientes = ObtenerTodos();
            var cliente = clientes.FirstOrDefault(c =>
                c.Dni.Equals(dni, StringComparison.OrdinalIgnoreCase));

            if (cliente is null) return false;

            clientes.Remove(cliente);
            Guardar(clientes);
            return true;
        }

        private void Guardar(List<Cliente> clientes)
        {
            try
            {
                var json = JsonSerializer.Serialize(clientes,
                    new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(_rutaFichero, json);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al guardar el fichero de clientes");
                throw;
            }
        }

        public (bool ok, List<string> errores) Actualizar(string dni, Cliente clienteActualizado)
        {
            var (esValido, errores) = ClientValidator.Validar(clienteActualizado);
            if (!esValido) return (false, errores);

            var clientes = ObtenerTodos();
            var index = clientes.FindIndex(c =>
                c.Dni.Equals(dni, StringComparison.OrdinalIgnoreCase));

            if (index == -1) return (false, new List<string> { "No existe cliente con ese DNI." });

            clientes[index] = clienteActualizado;
            Guardar(clientes);
            return (true, new List<string>());
        }
    }
}