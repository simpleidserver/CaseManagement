using CaseManagement.HumanTask.Domains;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.HumanTask.Persistence.EF.Persistence
{
    public class HumanTaskDefCommandRepository : IHumanTaskDefCommandRepository
    {
        private readonly HumanTaskDBContext _humanTaskDBContext;

        public HumanTaskDefCommandRepository(HumanTaskDBContext humanTaskDBContext)
        {
            _humanTaskDBContext = humanTaskDBContext;
        }

        public Task<bool> Add(HumanTaskDefinitionAggregate humanTaskDef, CancellationToken token)
        {
            _humanTaskDBContext.HumanTaskDefinitions.Add(humanTaskDef);
            return Task.FromResult(true);
        }

        public Task<bool> Update(HumanTaskDefinitionAggregate humanTaskDef, CancellationToken token)
        {
            _humanTaskDBContext.Entry(humanTaskDef).State = EntityState.Modified;
            return Task.FromResult(true);
        }

        public async Task<bool> Delete(string name, CancellationToken token)
        {
            var result = await _humanTaskDBContext.HumanTaskDefinitions.FirstOrDefaultAsync(_ => _.Name == name, token);
            if (result == null)
            {
                return false;
            }

            _humanTaskDBContext.HumanTaskDefinitions.Remove(result);
            return true;
        }

        public Task<int> SaveChanges(CancellationToken token)
        {
            return _humanTaskDBContext.SaveChangesAsync(token);
        }
    }
}
