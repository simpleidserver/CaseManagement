using CaseManagement.BPMN.Common;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.BPMN.Tests.Delegates
{
    public class CreatePersonDelegate : IDelegateHandler
    {
        public Task<ICollection<BaseToken>> Execute(ICollection<BaseToken> incoming, CancellationToken cancellationToken)
        {
            return Task.FromResult(incoming);
        }
    }
}
