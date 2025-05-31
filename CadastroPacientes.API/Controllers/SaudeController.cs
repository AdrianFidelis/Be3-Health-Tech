using CadastroPacientes.Infrastructure.Database;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace CadastroPacientes.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SaudeController : ControllerBase
    {
        private readonly DbConnectionFactory _factory;

        public SaudeController(DbConnectionFactory factory)
        {
            _factory = factory;
        }

        [HttpGet]
        public IActionResult Get()
        {
            using var conn = _factory.CreateConnection();
            conn.Open();
            return Ok("Conectado com sucesso ao banco: " + conn.Database);
        }
    }
}
