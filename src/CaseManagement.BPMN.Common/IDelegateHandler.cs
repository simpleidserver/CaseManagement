using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.BPMN.Common
{
    public interface IDelegateHandler
    {
        Task<ICollection<BaseToken>> Execute(ICollection<BaseToken> incoming, CancellationToken cancellationToken);
    }
}