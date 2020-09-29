using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.Common.Jobs.Persistence
{
    public interface IScheduledJobStore
    {
        Task<SchedulingResult> TryGetNextScheduling(string jobName, CancellationToken token);
    }
}
