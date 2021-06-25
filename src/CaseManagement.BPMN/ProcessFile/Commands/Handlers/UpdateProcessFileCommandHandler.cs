using CaseManagement.BPMN.Exceptions;
using CaseManagement.BPMN.Persistence;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.BPMN.ProcessFile.Commands.Handlers
{
    public class UpdateProcessFileCommandHandler : IRequestHandler<UpdateProcessFileCommand, bool>
    {
        private readonly IProcessFileCommandRepository _processFileCommandRepository;
        private readonly ILogger<UpdateProcessFileCommandHandler> _logger;

        public UpdateProcessFileCommandHandler(
            IProcessFileCommandRepository processFileCommandRepository,
            ILogger<UpdateProcessFileCommandHandler> logger)
        {
            _processFileCommandRepository = processFileCommandRepository;
            _logger = logger;
        }

        public async Task<bool> Handle(UpdateProcessFileCommand request, CancellationToken cancellationToken)
        {
            var processFile = await _processFileCommandRepository.Get(request.Id, cancellationToken);
            if (processFile == null || string.IsNullOrWhiteSpace(processFile.AggregateId))
            {
                _logger.LogError($"Cannot update process file because it doesn't exist : '{request.Id}'");
                throw new UnknownProcessFileException(request.Id);
            }

            processFile.Update(request.Name, request.Description);
            await _processFileCommandRepository.Update(processFile, cancellationToken);
            await _processFileCommandRepository.SaveChanges(cancellationToken);
            return true;
        }
    }
}
