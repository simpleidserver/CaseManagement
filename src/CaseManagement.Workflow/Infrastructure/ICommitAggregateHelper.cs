using System.Threading.Tasks;

namespace CaseManagement.Workflow.Infrastructure
{
    public interface ICommitAggregateHelper
    {
        Task Commit<T>(T aggregate, string streamName) where T : BaseAggregate;
    }
}
