using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Extensions;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Persistence.InMemory
{
    public class InMemoryCaseWorkerTaskCommandRepository : ICaseWorkerTaskCommandRepository
    {
        private ConcurrentBag<CaseWorkerTaskAggregate> _caseWorkerTaskLst;

        public InMemoryCaseWorkerTaskCommandRepository(ConcurrentBag<CaseWorkerTaskAggregate> caseWorkerTaskLst)
        {
            _caseWorkerTaskLst = caseWorkerTaskLst;
        }

        public void Delete(CaseWorkerTaskAggregate caseWorkerTask)
        {
            _caseWorkerTaskLst.Remove(_caseWorkerTaskLst.First(a => a.Id == caseWorkerTask.Id));
        }

        public void Add(CaseWorkerTaskAggregate caseWorkerTask)
        {
            _caseWorkerTaskLst.Add((CaseWorkerTaskAggregate)caseWorkerTask.Clone());
        }

        public void Update(CaseWorkerTaskAggregate caseWorkerTask)
        {
            _caseWorkerTaskLst.Remove(_caseWorkerTaskLst.First(a => a.Id == caseWorkerTask.Id));
            _caseWorkerTaskLst.Add((CaseWorkerTaskAggregate)caseWorkerTask.Clone());
        }

        public Task<int> SaveChanges()
        {
            return Task.FromResult(1);
        }
    }
}
