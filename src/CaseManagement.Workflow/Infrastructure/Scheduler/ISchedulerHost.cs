using System.Threading.Tasks;

namespace CaseManagement.Workflow.Infrastructure.Scheduler
{
    public interface ISchedulerHost
    {
        Task Start();
        Task Stop();
    }
}
