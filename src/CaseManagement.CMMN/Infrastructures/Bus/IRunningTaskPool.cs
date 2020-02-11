using System.Collections.Generic;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Infrastructures.Bus
{
    public interface IRunningTaskPool
    {
        void RemoveTask(string id);
        void AddTask(RunningTask task);
        RunningTask Get(string processId);
        IEnumerable<Task> Tasks { get; }
    }
}
