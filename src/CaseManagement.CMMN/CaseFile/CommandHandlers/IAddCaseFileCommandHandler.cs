using CaseManagement.CMMN.CaseFile.Commands;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CaseFile.CommandHandlers
{
    public interface IAddCaseFileCommandHandler
    {
        Task<string> Handle(AddCaseFileCommand uploadCaseFileCommand, CancellationToken cancellationToken);
    }
}
