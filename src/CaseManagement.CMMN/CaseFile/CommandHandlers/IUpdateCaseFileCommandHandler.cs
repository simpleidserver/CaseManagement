using CaseManagement.CMMN.CaseFile.Commands;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CaseFile.CommandHandlers
{
    public interface IUpdateCaseFileCommandHandler
    {
        Task<bool> Handle(UpdateCaseFileCommand command);
    }
}
