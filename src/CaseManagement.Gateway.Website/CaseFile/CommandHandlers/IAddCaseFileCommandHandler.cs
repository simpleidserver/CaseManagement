using CaseManagement.Gateway.Website.CaseFile.Commands;
using System.Threading.Tasks;

namespace CaseManagement.Gateway.Website.CaseFile.CommandHandlers
{
    public interface IAddCaseFileCommandHandler
    {
        Task<string> Handle(AddCaseFileCommand command);
    }
}