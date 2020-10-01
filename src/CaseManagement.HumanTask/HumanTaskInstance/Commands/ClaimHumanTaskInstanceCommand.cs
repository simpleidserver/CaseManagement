using MediatR;
using System.Collections.Generic;

namespace CaseManagement.HumanTask.HumanTaskInstance.Commands
{
    /// <summary>
    /// Claim responsibility for a task, i.e. set the task to status Reserved
    /// </summary>
    public class ClaimHumanTaskInstanceCommand : IRequest<bool>
    {
        public string HumanTaskInstanceId { get; set; }
        public IEnumerable<KeyValuePair<string, string>> Claims { get; set; }
    }
}
