using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CadastroPacientes.Application.DTOs;
using CadastroPacientes.Domain.Entities;
using CadastroPacientes.Domain.Interfaces;

namespace CadastroPacientes.Application.Services
{
    public class PacienteService
    {
        private readonly IPacienteRepository _repository;

        public PacienteService(IPacienteRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Paciente>> ListarAsync()
            => await _repository.ListarAsync();

        public async Task<Paciente?> ObterPorIdAsync(Guid id)
            => await _repository.ObterPorIdAsync(id);

        public async Task<(bool Sucesso, Guid NovoId)> CriarAsync(CreatePacienteDto dto)
        {
            if (!string.IsNullOrEmpty(dto.CPF))
            {
                if (await _repository.ExisteCpfAsync(dto.CPF))
                    return (false, Guid.Empty);
            }

            var paciente = new Paciente
            {
                Id = Guid.NewGuid(),
                Nome = dto.Nome,
                Sobrenome = dto.Sobrenome,
                DataNascimento = dto.DataNascimento,
                Genero = dto.Genero,
                CPF = dto.CPF,
                RG = dto.RG,
                UfRg = dto.UfRg,
                Email = dto.Email,
                Celular = dto.Celular,
                TelefoneFixo = dto.TelefoneFixo,
                ConvenioId = dto.ConvenioId,
                NumeroCarteirinha = dto.NumeroCarteirinha,
                ValidadeCarteirinha = dto.ValidadeCarteirinha,
                Ativo = true
            };

            await _repository.AdicionarAsync(paciente);
            return (true, paciente.Id);
        }

        public async Task<bool> AtualizarAsync(UpdatePacienteDto dto)
        {
            var existente = await _repository.ObterPorIdAsync(dto.Id);
            if (existente == null)
                return false;

            if (!string.IsNullOrEmpty(dto.CPF) && dto.CPF != existente.CPF
                                         && await _repository.ExisteCpfAsync(dto.CPF))
            {
                return false;
            }

            existente.Nome = dto.Nome;
            existente.Sobrenome = dto.Sobrenome;
            existente.DataNascimento = dto.DataNascimento;
            existente.Genero = dto.Genero;
            existente.CPF = dto.CPF;
            existente.RG = dto.RG;
            existente.UfRg = dto.UfRg;
            existente.Email = dto.Email;
            existente.Celular = dto.Celular;
            existente.TelefoneFixo = dto.TelefoneFixo;
            existente.ConvenioId = dto.ConvenioId;
            existente.NumeroCarteirinha = dto.NumeroCarteirinha;
            existente.ValidadeCarteirinha = dto.ValidadeCarteirinha;
            existente.Ativo = dto.Ativo;

            await _repository.AtualizarAsync(existente);
            return true;
        }

        public async Task<bool> InativarAsync(Guid id)
        {
            var existente = await _repository.ObterPorIdAsync(id);
            if (existente == null)
                return false;

            await _repository.InativarAsync(id);
            return true;
        }
    }
}
