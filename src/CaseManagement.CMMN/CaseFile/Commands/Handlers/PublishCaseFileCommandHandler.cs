using CaseManagement.CMMN.CaseFile.Commands;
using CaseManagement.CMMN.CaseFile.Exceptions;
using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Persistence;
using MassTransit;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CaseFile.Command.Handlers
{
    public class PublishCaseFileCommandHandler : IRequestHandler<PublishCaseFileCommand, string>
    {
        private readonly ICaseFileCommandRepository _caseFileCommandRepository;
        private readonly IBusControl _busControl;

        public PublishCaseFileCommandHandler(
            ICaseFileCommandRepository caseFileCommandRepository,
            IBusControl busControl)
        {
            _caseFileCommandRepository = caseFileCommandRepository;
            _busControl = busControl;
        }

        public async Task<string> Handle(PublishCaseFileCommand publishCaseFileCommand, CancellationToken token)
        {
            var caseFile = await _caseFileCommandRepository.Get(publishCaseFileCommand.Id, token);
            if (caseFile == null || string.IsNullOrWhiteSpace(caseFile.AggregateId))
            {
                throw new UnknownCaseFileException(publishCaseFileCommand.Id);
            }

            var newCaseFile = caseFile.Publish();
            await _caseFileCommandRepository.Update(caseFile, token);
            await _caseFileCommandRepository.Add(newCaseFile, token);
            await _caseFileCommandRepository.SaveChanges(token);
            await _busControl.Publish(caseFile.DomainEvents.First() as CaseFilePublishedEvent, token);
            return newCaseFile.AggregateId;
        }
    }
}
