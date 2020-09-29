using CaseManagement.Common.EvtStore;
using CaseManagement.HumanTask.Domains;
using CaseManagement.HumanTask.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.HumanTask.HumanTaskInstance.Commands.Handlers
{
    public class StartHumanTaskInstanceCommandHandler : IRequestHandler<StartHumanTaskInstanceCommand, bool>
    {
        private readonly IEventStoreRepository _eventStoreRepository;
        private readonly ILogger<StartHumanTaskInstanceCommandHandler> _logger;

        public StartHumanTaskInstanceCommandHandler(
            IEventStoreRepository eventStoreRepository,
            ILogger<StartHumanTaskInstanceCommandHandler> logger)
        {
            _eventStoreRepository = eventStoreRepository;
            _logger = logger;
        }

        public async Task<bool> Handle(StartHumanTaskInstanceCommand request, CancellationToken cancellationToken)
        {
            var humanTaskInstanceAggregate = await _eventStoreRepository.GetLastAggregate<HumanTaskInstanceAggregate>(request.HumanTaskInstanceId, HumanTaskInstanceAggregate.GetStreamName(request.HumanTaskInstanceId));
            if (humanTaskInstanceAggregate == null || string.IsNullOrWhiteSpace(humanTaskInstanceAggregate.AggregateId))
            {
                _logger.LogError($"Human task instance '{request.HumanTaskInstanceId}' doesn't exist");
                throw new UnknownHumanTaskInstanceException(request.HumanTaskInstanceId);
            }

            
            throw new System.NotImplementedException();
        }
    }
}
