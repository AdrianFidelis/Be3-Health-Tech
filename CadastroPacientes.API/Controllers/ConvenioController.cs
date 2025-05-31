using Microsoft.AspNetCore.Mvc;
using CadastroPacientes.Domain.Interfaces;

namespace CadastroPacientes.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConvenioController : ControllerBase
    {
        private readonly IConvenioRepository _repo;
        public ConvenioController(IConvenioRepository repo) => _repo = repo;

        [HttpGet]
        public async Task<IActionResult> Get() =>
            Ok(await _repo.ListarAsync());
    }
}