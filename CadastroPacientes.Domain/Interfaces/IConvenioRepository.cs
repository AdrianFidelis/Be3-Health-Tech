using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CadastroPacientes.Domain.Interfaces
{
    public interface IConvenioRepository
    {
        Task<IEnumerable<Entities.Convenio>> ListarAsync();
    }
}
