using CaseManagement.CMMN.CaseFile.Commands;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CaseFile.CommandHandlers
{
    public interface IUploadCaseFilesCommandHandler
    {
        Task<IEnumerable<string>> Handle(UploadCaseFilesCommand uploadCaseFilesCommand);
    }
}
