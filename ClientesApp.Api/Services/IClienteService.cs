using ClientesApp.Domain.Models;

namespace ClientesApp.Api.Services
{
    public interface IClienteService
    {
        List<Cliente> ObtenerTodos();
        Cliente? ObtenerPorDni(string dni);
        (bool ok, List<string> errores) Crear(Cliente cliente);
        bool Eliminar(string dni);
    }
}
