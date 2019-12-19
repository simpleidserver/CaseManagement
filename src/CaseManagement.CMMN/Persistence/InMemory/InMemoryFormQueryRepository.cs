using CaseManagement.CMMN.Domains;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Persistence.InMemory
{
    public class InMemoryFormQueryRepository : IFormQueryRepository
    {
        private ICollection<FormAggregate> _forms;

        public InMemoryFormQueryRepository(ICollection<FormAggregate> forms)
        {
            _forms = forms;
        }

        public Task<FormAggregate> FindFormById(string id)
        {
            return Task.FromResult(_forms.FirstOrDefault(f => f.Id == id));
        }
    }
}
