using MediatR;
using System.Collections.Generic;

namespace CaseManagement.HumanTask.HumanTaskInstance.Commands
{
    /// <summary>
    /// Start the execution of the task, i.e. set the task to status InProgress.
    /// </summary>
    public class StartHumanTaskInstanceCommand : IRequest<bool>
    {
        public string HumanTaskInstanceId { get; set; }
        public IEnumerable<KeyValuePair<string, string>> Claims { get; set; }
    }
}
