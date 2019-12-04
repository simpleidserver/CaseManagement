using System.Collections.Generic;
using System.Threading.Tasks;

namespace CaseManagement.Workflow.Infrastructure.Bus
{
    public interface IRunningTaskPool
    {
        int NbTasks();
        void RemoveTask(string id);
        void AddTask(RunningTask task);
        RunningTask Get(string id);
        IEnumerable<Task> Tasks { get; }
    }
}
