using CaseManagement.HumanTask.Exceptions;
using CaseManagement.HumanTask.Persistence;
using CaseManagement.HumanTask.Resources;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.HumanTask.HumanTaskInstance.Commands.Handlers
{
    public class ForceClaimHumanTaskInstanceCommandHandler : IRequestHandler<ForceClaimHumanTaskInstanceCommand, bool>
    {
        private readonly IHumanTaskInstanceQueryRepository _humanTaskInstanceQueryRepository;
        private readonly IHumanTaskInstanceCommandRepository _humanTaskInstanceCommandRepository;
        private readonly ILogger<ForceClaimHumanTaskInstanceCommandHandler> _logger;

        public ForceClaimHumanTaskInstanceCommandHandler(
            IHumanTaskInstanceQueryRepository humanTaskInstanceQueryRepository,
            IHumanTaskInstanceCommandRepository humanTaskInstanceCommandRepository,
            ILogger<ForceClaimHumanTaskInstanceCommandHandler> logger)
        {
            _humanTaskInstanceQueryRepository = humanTaskInstanceQueryRepository;
            _humanTaskInstanceCommandRepository = humanTaskInstanceCommandRepository;
            _logger = logger;
        }

        public async Task<bool> Handle(ForceClaimHumanTaskInstanceCommand request, CancellationToken cancellationToken)
        {
            var humanTaskInstance = await _humanTaskInstanceQueryRepository.Get(request.HumanTaskInstanceId, cancellationToken);
            if (humanTaskInstance == null)
            {
                _logger.LogError($"HumanTask '{request.HumanTaskInstanceId}' doesn't exist");
                throw new UnknownHumanTaskInstanceException(string.Format(Global.UnknownHumanTaskInstance, request.HumanTaskInstanceId));
            }

            humanTaskInstance.Claim(request.UserId);
            await _humanTaskInstanceCommandRepository.Update(humanTaskInstance, cancellationToken);
            await _humanTaskInstanceCommandRepository.SaveChanges(cancellationToken);
            return true;
        }
    }
}
