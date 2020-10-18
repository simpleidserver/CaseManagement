using CaseManagement.Common.Parameters;
using CaseManagement.Common.Results;
using CaseManagement.HumanTask.HumanTaskDef.Results;
using MediatR;

namespace CaseManagement.HumanTask.HumanTaskDef.Queries
{
    public class SearchHumanTaskDefQuery : BaseSearchParameter, IRequest<SearchResult<HumanTaskDefResult>>
    {
    }
}