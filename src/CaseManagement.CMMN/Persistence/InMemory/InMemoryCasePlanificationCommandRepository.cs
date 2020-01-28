using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Extensions;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Persistence.InMemory
{
    public class InMemoryCasePlanificationCommandRepository : ICasePlanificationCommandRepository
    {
        private readonly ConcurrentBag<CasePlanificationAggregate> _casePlanifications;

        public InMemoryCasePlanificationCommandRepository(ConcurrentBag<CasePlanificationAggregate> casePlanifications)
        {
            _casePlanifications = casePlanifications;
        }

        public void Add(CasePlanificationAggregate casePlanification)
        {
            _casePlanifications.Add((CasePlanificationAggregate)casePlanification.Clone());
        }

        public void Delete(CasePlanificationAggregate casePlanification)
        {
            _casePlanifications.Remove(_casePlanifications.First(a => a.CaseElementId == casePlanification.CaseElementId && a.CaseInstanceId == casePlanification.CaseInstanceId));
        }

        public Task<int> SaveChanges()
        {
            return Task.FromResult(1);
        }

        public void Update(CasePlanificationAggregate casePlanification)
        {
            _casePlanifications.Remove(_casePlanifications.First(a => a.CaseElementId == casePlanification.CaseElementId && a.CaseInstanceId == casePlanification.CaseInstanceId));
            _casePlanifications.Add((CasePlanificationAggregate)casePlanification.Clone());
        }
    }
}
