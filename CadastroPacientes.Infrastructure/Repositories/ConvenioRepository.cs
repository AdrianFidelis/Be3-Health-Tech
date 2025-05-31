using System.Data;
using Dapper;
using CadastroPacientes.Domain.Entities;
using CadastroPacientes.Domain.Interfaces;
using CadastroPacientes.Infrastructure.Database;

namespace CadastroPacientes.Infrastructure.Repositories
{
    public class ConvenioRepository : IConvenioRepository
    {
        private readonly DbConnectionFactory _factory;
        public ConvenioRepository(DbConnectionFactory factory) => _factory = factory;

        public async Task<IEnumerable<Convenio>> ListarAsync()
        {
            using var conn = _factory.CreateConnection();
            const string sql = "SELECT Id, Nome FROM Convenios";
            return await conn.QueryAsync<Convenio>(sql);
        }
    }
}