using CaseManagement.Gateway.Website.CaseFile.Commands;
using System.Threading.Tasks;

namespace CaseManagement.Gateway.Website.CaseFile.CommandHandlers
{
    public interface IUpdateCaseFileCommandHandler
    {
        Task Handle(UpdateCaseFileCommand command);
    }
}
