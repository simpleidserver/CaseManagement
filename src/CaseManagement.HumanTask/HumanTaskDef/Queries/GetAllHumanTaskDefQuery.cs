using CaseManagement.HumanTask.HumanTaskDef.Results;
using MediatR;
using System.Collections.Generic;

namespace CaseManagement.HumanTask.HumanTaskDef.Queries
{
    public class GetAllHumanTaskDefQuery : IRequest<ICollection<HumanTaskDefResult>>
    {
    }
}
