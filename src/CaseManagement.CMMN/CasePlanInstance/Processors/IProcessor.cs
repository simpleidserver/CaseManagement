using CaseManagement.CMMN.Domains;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CasePlanInstance.Processors
{
    public interface IProcessor
    {
        CaseElementTypes Type { get; }
        Task<ProcessorParameter> Handle(ProcessorParameter parameter, CancellationToken token);
    }
}
