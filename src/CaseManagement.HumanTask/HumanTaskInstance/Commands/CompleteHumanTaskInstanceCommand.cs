using MediatR;
using System.Collections.Generic;

namespace CaseManagement.HumanTask.HumanTaskInstance.Commands
{
    /// <summary>
    /// Execution of the task finished successfully.
    /// </summary>
    public class CompleteHumanTaskInstanceCommand : IRequest<bool>
    {
        public string HumanTaskInstanceId { get; set; }
        public Dictionary<string, string> OperationParameters { get; set; }
        public IEnumerable<KeyValuePair<string, string>> Claims { get; set; }
    }
}