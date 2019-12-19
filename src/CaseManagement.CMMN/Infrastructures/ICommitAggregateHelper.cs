using CaseManagement.Workflow.Infrastructure;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Infrastructures
{
    public interface ICommitAggregateHelper
    {
        Task Commit<T>(T aggregate, string streamName) where T : BaseAggregate;
    }
}
