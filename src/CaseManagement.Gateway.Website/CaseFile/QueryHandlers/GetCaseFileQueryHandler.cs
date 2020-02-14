using CaseManagement.Gateway.Website.CaseFile.DTOs;
using CaseManagement.Gateway.Website.CaseFile.Queries;
using CaseManagement.Gateway.Website.CaseFile.Services;
using System;
using System.Threading.Tasks;

namespace CaseManagement.Gateway.Website.CaseFile.QueryHandlers
{
    public class GetCaseFileQueryHandler : IGetCaseFileQueryHandler
    {
        private readonly ICaseFileService _caseFileService;

        public GetCaseFileQueryHandler(ICaseFileService caseFileService)
        {
            _caseFileService = caseFileService;
        }

        public Task<CaseFileResponse> Handle(GetCaseFileQuery getCaseFileQuery)
        {
            return _caseFileService.Get(getCaseFileQuery.CaseFileId);
        }
    }
}
