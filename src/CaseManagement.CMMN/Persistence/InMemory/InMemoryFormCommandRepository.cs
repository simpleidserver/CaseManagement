using CaseManagement.CMMN.Domains;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Persistence.InMemory
{
    public class InMemoryFormCommandRepository : IFormCommandRepository
    {
        private ICollection<FormAggregate> _forms;

        public InMemoryFormCommandRepository(ICollection<FormAggregate> forms)
        {
            _forms = forms;
        }

        public void Add(FormAggregate form)
        {
            _forms.Add(form);
        }

        public Task<int> SaveChanges()
        {
            return Task.FromResult(1);
        }
    }
}
