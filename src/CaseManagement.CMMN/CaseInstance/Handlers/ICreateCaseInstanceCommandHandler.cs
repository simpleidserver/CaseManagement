using CaseManagement.CMMN.CaseInstance.Commands;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CaseInstance.Handlers
{
    public interface ICreateCaseInstanceCommandHandler
    {
        Task<string> Handle(CreateCaseInstanceCommand createCaseInstanceCommand);
    }
}
