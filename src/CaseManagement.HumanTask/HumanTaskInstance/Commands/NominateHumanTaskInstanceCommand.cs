using MediatR;
using System.Collections.Generic;

namespace CaseManagement.HumanTask.HumanTaskInstance.Commands
{
    /// <summary>
    /// Nominate an organization entity to process the task.
    /// If it is nominated to one person then the new state of the task is Reserved.
    /// If it is nominated to several people then the new state of the task is Ready. 
    /// </summary>
    public class NominateHumanTaskInstanceCommand : IRequest<bool>
    {
        public string HumanTaskInstanceId { get; set; }
        public IEnumerable<KeyValuePair<string, string>> Claims { get; set; }
        public ICollection<string> GroupNames { get; set; }
        public ICollection<string> UserIdentifiers { get; set; }
    }
}
