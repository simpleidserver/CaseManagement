using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.BPMN.Common
{
    public interface IDelegateHandler
    {
        Task<ICollection<MessageToken>> Execute(ICollection<MessageToken> incoming, CancellationToken cancellationToken);
    }
}