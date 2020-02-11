using CaseManagement.CMMN.CaseFile.Commands;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CaseFile.CommandHandlers
{
    public interface IPublishCaseFileCommandHandler
    {
        Task<bool> Handle(PublishCaseFileCommand publishCaseFileCommand);
    }
}
