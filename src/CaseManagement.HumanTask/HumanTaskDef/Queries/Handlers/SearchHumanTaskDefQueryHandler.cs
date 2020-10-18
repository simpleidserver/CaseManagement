using CaseManagement.Common.Results;
using CaseManagement.HumanTask.HumanTaskDef.Results;
using CaseManagement.HumanTask.Persistence;
using CaseManagement.HumanTask.Persistence.Parameters;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.HumanTask.HumanTaskDef.Queries.Handlers
{
    public class SearchHumanTaskDefQueryHandler : IRequestHandler<SearchHumanTaskDefQuery, SearchResult<HumanTaskDefResult>>
    {
        private readonly IHumanTaskDefQueryRepository _humanTaskDefQueryRepository;

        public SearchHumanTaskDefQueryHandler(IHumanTaskDefQueryRepository humanTaskDefQueryRepository)
        {
            _humanTaskDefQueryRepository = humanTaskDefQueryRepository;
        }

        public async Task<SearchResult<HumanTaskDefResult>> Handle(SearchHumanTaskDefQuery request, CancellationToken cancellationToken)
        {
            var result = await _humanTaskDefQueryRepository.Search(new SearchHumanTaskDefParameter
            {
                Count = request.Count,
                Order = request.Order,
                OrderBy = request.OrderBy,
                StartIndex = request.StartIndex
            }, cancellationToken);
            return new SearchResult<HumanTaskDefResult>
            {
                Count = result.Count,
                StartIndex = result.StartIndex,
                TotalLength = result.TotalLength,
                Content = result.Content.Select(_ => HumanTaskDefResult.ToDto(_))
            };
        }
    }
}
