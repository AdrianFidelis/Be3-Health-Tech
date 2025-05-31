using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CadastroPacientes.Application.DTOs;
using CadastroPacientes.Application.Services;
using CadastroPacientes.Domain.Entities;

namespace CadastroPacientes.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PacienteController : ControllerBase
    {
        private readonly PacienteService _service;

        public PacienteController(PacienteService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var lista = await _service.ListarAsync();
            return Ok(lista);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var paciente = await _service.ObterPorIdAsync(id);
            if (paciente == null)
                return NotFound();
            return Ok(paciente);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreatePacienteDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var (sucesso, novoId) = await _service.CriarAsync(dto);
            if (!sucesso)
                return Conflict("CPF já cadastrado ou inválido.");

            return CreatedAtAction(nameof(GetById), new { id = novoId }, new { Id = novoId });
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] UpdatePacienteDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            dto.Id = id;
            var atualizou = await _service.AtualizarAsync(dto);
            if (!atualizou)
                return NotFound();

            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var inativou = await _service.InativarAsync(id);
            if (!inativou)
                return NotFound();

            return NoContent();
        }
    }
}
