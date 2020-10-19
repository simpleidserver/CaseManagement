using CaseManagement.HumanTask.Exceptions;
using CaseManagement.HumanTask.HumanTaskDef.Results;
using CaseManagement.HumanTask.Persistence;
using CaseManagement.HumanTask.Resources;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.HumanTask.HumanTaskDef.Queries.Handlers
{
    public class GetHumanTaskDefQueryHandler : IRequestHandler<GetHumanTaskDefQuery, HumanTaskDefResult>
    {
        private readonly IHumanTaskDefQueryRepository _humanTaskDefQueryRepository;
        private readonly ILogger<GetHumanTaskDefQueryHandler> _logger;

        public GetHumanTaskDefQueryHandler(
            IHumanTaskDefQueryRepository humanTaskDefQueryRepository,
            ILogger<GetHumanTaskDefQueryHandler> logger)
        {
            _humanTaskDefQueryRepository = humanTaskDefQueryRepository;
            _logger = logger;
        }

        public async Task<HumanTaskDefResult> Handle(GetHumanTaskDefQuery request, CancellationToken cancellationToken)
        {
            var result = await _humanTaskDefQueryRepository.Get(request.Id, cancellationToken);
            if (result == null)
            {
                _logger.LogError($"The human task definition '{request.Id}' doesn't exist");
                throw new UnknownHumanTaskDefException(string.Format(Global.UnknownHumanTaskDef, request.Id));
            }

            return HumanTaskDefResult.ToDto(result);
        }
    }
}
