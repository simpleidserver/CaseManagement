using CaseManagement.CMMN.Domains;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Persistence.InMemory
{
    public class InMemoryProcessQueryRepository : IProcessQueryRepository
    {
        private ICollection<ProcessAggregate> _processes;

        public InMemoryProcessQueryRepository(ICollection<ProcessAggregate> processes)
        {
            _processes = processes;
        }

        public Task<ProcessAggregate> FindById(string id)
        {
            return Task.FromResult(_processes.FirstOrDefault(p => p.Id == id));
        }
    }
}
