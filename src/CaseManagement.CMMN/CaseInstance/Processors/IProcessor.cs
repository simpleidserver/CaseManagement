using CaseManagement.CMMN.Domains;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CaseInstance.Processors
{
    public interface IProcessor
    {
        CaseElementTypes Type { get; }
        Task<ProcessorParameter> Handle(ProcessorParameter parameter, CancellationToken token);
    }
}
