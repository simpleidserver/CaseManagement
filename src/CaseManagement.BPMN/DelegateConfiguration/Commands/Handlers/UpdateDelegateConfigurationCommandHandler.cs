using CaseManagement.BPMN.Exceptions;
using CaseManagement.BPMN.Persistence;
using CaseManagement.BPMN.Resources;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.BPMN.DelegateConfiguration.Commands.Handlers
{
    public class UpdateDelegateConfigurationCommandHandler : IRequestHandler<UpdateDelegateConfigurationCommand, bool>
    {
        private readonly IDelegateConfigurationRepository _delegateConfigurationRepository;
        private readonly ILogger<UpdateDelegateConfigurationCommandHandler> _logger;

        public UpdateDelegateConfigurationCommandHandler(
            IDelegateConfigurationRepository delegateConfigurationRepository,
            ILogger<UpdateDelegateConfigurationCommandHandler> logger)
        {
            _delegateConfigurationRepository = delegateConfigurationRepository;
            _logger = logger;
        }

        public async Task<bool> Handle(UpdateDelegateConfigurationCommand request, CancellationToken cancellationToken)
        {
            var delegateConfiguration = await _delegateConfigurationRepository.Get(request.Id, cancellationToken);
            if (delegateConfiguration == null)
            {
                _logger.LogError($"Cannot update the delegate configuration '{request.Id}' because it doesn't exist");
                throw new UnknownDelegateConfigurationException(string.Format(Global.UnknownDelegate, request.Id));
            }

            delegateConfiguration.Update(request.Records);
            await _delegateConfigurationRepository.Update(delegateConfiguration, cancellationToken);
            await _delegateConfigurationRepository.SaveChanges(cancellationToken);
            return true;
        }
    }
}
