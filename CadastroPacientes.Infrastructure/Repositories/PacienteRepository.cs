using System;
using System.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using CadastroPacientes.Domain.Entities;
using CadastroPacientes.Domain.Interfaces;
using CadastroPacientes.Infrastructure.Database;

namespace CadastroPacientes.Infrastructure.Repositories
{
    public class PacienteRepository : IPacienteRepository
    {
        private readonly DbConnectionFactory _factory;

        public PacienteRepository(DbConnectionFactory factory)
        {
            _factory = factory;
        }

        public async Task<IEnumerable<Paciente>> ListarAsync()
        {
            using var conn = _factory.CreateConnection();
            const string sql = "SELECT * FROM Pacientes";
            return await conn.QueryAsync<Paciente>(sql);
        }

        public async Task<Paciente?> ObterPorIdAsync(Guid id)
        {
            using var conn = _factory.CreateConnection();
            const string sql = "SELECT * FROM Pacientes WHERE Id = @Id";
            return await conn.QueryFirstOrDefaultAsync<Paciente>(sql, new { Id = id });
        }

        public async Task AdicionarAsync(Paciente paciente)
        {
            using var conn = _factory.CreateConnection();
            const string sql = @"
                INSERT INTO Pacientes (
                    Id, Nome, Sobrenome, DataNascimento, Genero, CPF, RG, UfRg,
                    Email, Celular, TelefoneFixo, ConvenioId, NumeroCarteirinha,
                    ValidadeCarteirinha, Ativo
                ) VALUES (
                    @Id, @Nome, @Sobrenome, @DataNascimento, @Genero, @CPF, @RG, @UfRg,
                    @Email, @Celular, @TelefoneFixo, @ConvenioId, @NumeroCarteirinha,
                    @ValidadeCarteirinha, @Ativo
                )";
            await conn.ExecuteAsync(sql, paciente);
        }

        public async Task AtualizarAsync(Paciente paciente)
        {
            using var conn = _factory.CreateConnection();
            const string sql = @"
                UPDATE Pacientes SET
                    Nome = @Nome,
                    Sobrenome = @Sobrenome,
                    DataNascimento = @DataNascimento,
                    Genero = @Genero,
                    CPF = @CPF,
                    RG = @RG,
                    UfRg = @UfRg,
                    Email = @Email,
                    Celular = @Celular,
                    TelefoneFixo = @TelefoneFixo,
                    ConvenioId = @ConvenioId,
                    NumeroCarteirinha = @NumeroCarteirinha,
                    ValidadeCarteirinha = @ValidadeCarteirinha,
                    Ativo = @Ativo
                WHERE Id = @Id";
            await conn.ExecuteAsync(sql, paciente);
        }

        public async Task InativarAsync(Guid id)
        {
            using var conn = _factory.CreateConnection();
            const string sql = "UPDATE Pacientes SET Ativo = 0 WHERE Id = @Id";
            await conn.ExecuteAsync(sql, new { Id = id });
        }

        public async Task<bool> ExisteCpfAsync(string cpf)
        {
            using var conn = _factory.CreateConnection();
            const string sql = "SELECT COUNT(1) FROM Pacientes WHERE CPF = @CPF AND Ativo = 1";
            var count = await conn.ExecuteScalarAsync<int>(sql, new { CPF = cpf });
            return count > 0;
        }
    }
}
