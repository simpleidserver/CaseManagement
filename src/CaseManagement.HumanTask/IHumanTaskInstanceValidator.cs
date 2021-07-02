using CaseManagement.HumanTask.Domains;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.HumanTask
{
    public interface IHumanTaskInstanceValidator
    {
        Task Validate(HumanTaskInstanceAggregate humanTaskInstance, Dictionary<string, string> parameters, CancellationToken cancellationToken);
    }
}
