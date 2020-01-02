using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CaseInstance.Processors.CaseFileItem
{
    public interface ICaseFileItemListener
    {
        string CaseFileItemType { get; }
        Task Start(ProcessorParameter parameter, CancellationToken token);
    }
}
