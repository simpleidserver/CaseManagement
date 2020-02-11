using CaseManagement.CMMN.CasePlanInstance.Commands;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CasePlanInstance.CommandHandlers
{
    public interface ICreateCaseInstanceCommandHandler
    {
        Task<Domains.CasePlanInstanceAggregate> Handle(CreateCaseInstanceCommand command);
    }
}
