using CaseManagement.BPMN.Exceptions;
using CaseManagement.BPMN.Persistence;
using CaseManagement.BPMN.ProcessFile.Results;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.BPMN.ProcessFile.Commands.Handlers
{
    public class PublishProcessFileCommandHandler : IRequestHandler<PublishProcessFileCommand, PublishProcessFileResult>
    {
        private readonly IProcessFileCommandRepository _processFileCommandRepository;

        public PublishProcessFileCommandHandler(
            IProcessFileCommandRepository processFileCommandRepository)
        {
            _processFileCommandRepository = processFileCommandRepository;
        }

        public async Task<PublishProcessFileResult> Handle(PublishProcessFileCommand request, CancellationToken cancellationToken)
        {
            var processFile = await _processFileCommandRepository.Get(request.Id, cancellationToken);
            if (request == null || string.IsNullOrWhiteSpace(processFile.AggregateId))
            {
                throw new UnknownProcessFileException(request.Id);
            }

            var newProcessFile = processFile.Publish();
            await _processFileCommandRepository.Update(processFile, cancellationToken);
            await _processFileCommandRepository.Add(newProcessFile, cancellationToken);
            await _processFileCommandRepository.SaveChanges(cancellationToken);
            return new PublishProcessFileResult
            {
                Id = newProcessFile.AggregateId
            };
        }
    }
}
