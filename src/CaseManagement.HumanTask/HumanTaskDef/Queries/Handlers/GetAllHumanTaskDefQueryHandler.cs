using CaseManagement.HumanTask.HumanTaskDef.Results;
using CaseManagement.HumanTask.Persistence;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.HumanTask.HumanTaskDef.Queries.Handlers
{
    public class GetAllHumanTaskDefQueryHandler : IRequestHandler<GetAllHumanTaskDefQuery, ICollection<HumanTaskDefResult>>
    {
        private readonly IHumanTaskDefQueryRepository _humanTaskDefQueryRepository;

        public GetAllHumanTaskDefQueryHandler(IHumanTaskDefQueryRepository humanTaskDefQueryRepository)
        {
            _humanTaskDefQueryRepository = humanTaskDefQueryRepository;
        }

        public async Task<ICollection<HumanTaskDefResult>> Handle(GetAllHumanTaskDefQuery request, CancellationToken cancellationToken)
        {
            var result = await _humanTaskDefQueryRepository.GetAll(cancellationToken);
            return result.Select(_ => HumanTaskDefResult.ToDto(_)).ToList();
        }
    }
}
