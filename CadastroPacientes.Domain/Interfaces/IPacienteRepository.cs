using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CadastroPacientes.Domain.Entities;

namespace CadastroPacientes.Domain.Interfaces
{
    public interface IPacienteRepository
    {
        Task<IEnumerable<Paciente>> ListarAsync();
        Task<Paciente?> ObterPorIdAsync(Guid id);
        Task AdicionarAsync(Paciente paciente);
        Task AtualizarAsync(Paciente paciente);
        Task InativarAsync(Guid id);                       
        Task<bool> ExisteCpfAsync(string cpf);
    }
}
