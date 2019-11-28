using CaseManagement.CMMN.CaseInstance.Commands;
using CaseManagement.Workflow.Domains;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CaseInstance.CommandHandlers
{
    public interface ICreateCaseInstanceCommandHandler
    {
        Task<ProcessFlowInstance> Handle(CreateCaseInstanceCommand createCaseInstanceCommand);
    }
}
