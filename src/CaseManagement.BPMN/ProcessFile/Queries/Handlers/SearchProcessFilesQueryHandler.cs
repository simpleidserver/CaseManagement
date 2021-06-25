using CaseManagement.BPMN.Persistence;
using CaseManagement.BPMN.ProcessFile.Results;
using CaseManagement.Common.Results;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.BPMN.ProcessFile.Queries.Handlers
{
    public class SearchProcessFilesQueryHandler : IRequestHandler<SearchProcessFilesQuery, SearchResult<ProcessFileResult>>
    {
        private readonly IProcessFileQueryRepository _processFileQueryRepository;

        public SearchProcessFilesQueryHandler(IProcessFileQueryRepository processFileQueryRepository)
        {
            _processFileQueryRepository = processFileQueryRepository;
        }

        public Task<SearchResult<ProcessFileResult>> Handle(SearchProcessFilesQuery request, CancellationToken cancellationToken)
        {
            return _processFileQueryRepository.Find(new Persistence.Parameters.FindProcessFilesParameter
            {
                Count = request.Count,
                Order = request.Order,
                OrderBy = request.OrderBy,
                StartIndex = request.StartIndex,
                TakeLatest = request.TakeLatest,
                FileId = request.FileId
            }, cancellationToken);
        }
    }
}