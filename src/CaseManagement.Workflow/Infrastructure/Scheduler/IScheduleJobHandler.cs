using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.Workflow.Infrastructure.Scheduler
{
    public interface IScheduleJobHandler<T> where T : JobMessage
    {
        Task Handle(T message, CancellationToken token);
    }
}
