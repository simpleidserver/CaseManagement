using CaseManagement.CMMN.Domains;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CaseInstance.Processors
{
    public interface ICMMNPlanItemProcessor
    {
        CMMNWorkflowElementTypes Type { get; }
        Task Handle(PlanItemProcessorParameter parameter, CancellationToken token);
    }
}
