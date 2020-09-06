using CaseManagement.CMMN.CaseFile.Commands;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CaseFile.CommandHandlers
{
    public interface IPublishCaseFileCommandHandler
    {
        Task<string> Handle(PublishCaseFileCommand publishCaseFileCommand, CancellationToken cancellationToken);
    }
}
