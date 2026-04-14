using ClientesApp.Api.Services;
using ClientesApp.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace ClientesApp.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClientesController : ControllerBase
    {
        private readonly IClienteService _service;
        private readonly ILogger<ClientesController> _logger;

        public ClientesController(IClienteService service, ILogger<ClientesController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        public ActionResult<List<Cliente>> ObtenerTodos()
        {
            try
            {
                return Ok(_service.ObtenerTodos());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener clientes");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpGet("{dni}")]
        public ActionResult<Cliente> ObtenerPorDni(string dni)
        {
            try
            {
                var cliente = _service.ObtenerPorDni(dni);
                return cliente is null ? NotFound($"No existe cliente con DNI {dni}") : Ok(cliente);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener cliente por DNI");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpPost]
        public ActionResult Crear([FromBody] Cliente cliente)
        {
            try
            {
                var (ok, errores) = _service.Crear(cliente);
                if (!ok) return BadRequest(errores);
                return CreatedAtAction(nameof(ObtenerPorDni), new { dni = cliente.Dni }, cliente);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear cliente");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpDelete("{dni}")]
        public ActionResult Eliminar(string dni)
        {
            try
            {
                return _service.Eliminar(dni) ? NoContent() : NotFound($"No existe cliente con DNI {dni}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar cliente");
                return StatusCode(500, "Error interno del servidor");
            }
        }
    }
}