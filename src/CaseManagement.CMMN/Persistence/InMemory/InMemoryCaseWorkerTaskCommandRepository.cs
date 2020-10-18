using CaseManagement.CMMN.Domains;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
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

        public Task Delete(CaseWorkerTaskAggregate caseWorkerTask, CancellationToken token)
        {
            _caseWorkerTaskLst.Remove(_caseWorkerTaskLst.First(a => a.AggregateId == caseWorkerTask.AggregateId));
            return Task.CompletedTask;
        }

        public Task Add(CaseWorkerTaskAggregate caseWorkerTask, CancellationToken token)
        {
            _caseWorkerTaskLst.Add((CaseWorkerTaskAggregate)caseWorkerTask.Clone());
            return Task.CompletedTask;
        }

        public Task Update(CaseWorkerTaskAggregate caseWorkerTask, CancellationToken token)
        {
            _caseWorkerTaskLst.Remove(_caseWorkerTaskLst.First(a => a.AggregateId == caseWorkerTask.AggregateId));
            _caseWorkerTaskLst.Add((CaseWorkerTaskAggregate)caseWorkerTask.Clone());
            return Task.CompletedTask;
        }

        public Task<int> SaveChanges(CancellationToken token)
        {
            return Task.FromResult(1);
        }
    }
}
