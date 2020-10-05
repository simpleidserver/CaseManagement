using MediatR;
using System.Collections.Generic;

namespace CaseManagement.HumanTask.HumanTaskInstance.Commands
{
    /// <summary>
    /// Creates an instantiateable subtask for the task from the definition of the task.
    /// </summary>
    public class InstantiateSubTaskCommand : IRequest<string>
    {
        public string HumanTaskInstanceId { get; set; }
        public string SubTaskName { get; set; }
        public IEnumerable<KeyValuePair<string, string>> Claims { get; set; }
    }
}
