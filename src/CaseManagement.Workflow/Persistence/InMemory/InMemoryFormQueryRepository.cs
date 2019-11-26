using CaseManagement.Workflow.Domains;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CaseManagement.Workflow.Persistence.InMemory
{
    public class InMemoryFormQueryRepository : IFormQueryRepository
    {
        private ICollection<Form> _forms;

        public InMemoryFormQueryRepository(ICollection<Form> forms)
        {
            _forms = forms;
        }

        public Task<Form> FindFormById(string id)
        {
            return Task.FromResult(_forms.FirstOrDefault(f => f.Id == id));
        }
    }
}
