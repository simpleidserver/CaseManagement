using CaseManagement.Gateway.Website.CaseFile.DTOs;
using CaseManagement.Gateway.Website.CaseFile.Queries;
using System.Threading.Tasks;

namespace CaseManagement.Gateway.Website.CaseFile.QueryHandlers
{
    public interface IGetCaseFileQueryHandler
    {
        Task<CaseFileResponse> Handle(GetCaseFileQuery getCaseFileQuery);
    }
}
