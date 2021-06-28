using CaseManagement.CMMN.CasePlanInstance.Exceptions;
using CaseManagement.CMMN.CasePlanInstance.Processors;
using CaseManagement.CMMN.Persistence;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CasePlanInstance.Commands.Handlers
{
    public class LaunchCaseInstanceCommandHandler : IRequestHandler<LaunchCaseInstanceCommand, bool>
    {
        private readonly ICasePlanInstanceCommandRepository _casePlanInstanceCommandRepository;
        private readonly ICasePlanInstanceProcessor _casePlanInstanceProcessor;

        public LaunchCaseInstanceCommandHandler(
            ICasePlanInstanceCommandRepository casePlanInstanceCommandRepository,
            ICasePlanInstanceProcessor casePlanInstanceProcessor)
        {
            _casePlanInstanceCommandRepository = casePlanInstanceCommandRepository;
            _casePlanInstanceProcessor = casePlanInstanceProcessor;
        }

        public async Task<bool> Handle(LaunchCaseInstanceCommand launchCaseInstanceCommand, CancellationToken token)
        {
            var caseInstance = await _casePlanInstanceCommandRepository.Get(launchCaseInstanceCommand.CasePlanInstanceId, token);
            if (caseInstance == null || string.IsNullOrWhiteSpace(caseInstance.AggregateId))
            {
                throw new UnknownCasePlanInstanceException(launchCaseInstanceCommand.CasePlanInstanceId);
            }

            await _casePlanInstanceProcessor.Execute(caseInstance, token);
            await _casePlanInstanceCommandRepository.Update(caseInstance, token);
            await _casePlanInstanceCommandRepository.SaveChanges(token);
            return true;
        }
    }
}
