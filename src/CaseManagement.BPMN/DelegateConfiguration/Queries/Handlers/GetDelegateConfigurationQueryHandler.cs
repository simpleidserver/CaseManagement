using CaseManagement.BPMN.DelegateConfiguration.Results;
using CaseManagement.BPMN.Exceptions;
using CaseManagement.BPMN.Persistence;
using CaseManagement.BPMN.Resources;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.BPMN.DelegateConfiguration.Queries.Handlers
{
    public class GetDelegateConfigurationQueryHandler : IRequestHandler<GetDelegateConfigurationQuery, DelegateConfigurationResult>
    {
        private readonly IDelegateConfigurationRepository _delegateConfigurationRepository;

        public GetDelegateConfigurationQueryHandler(IDelegateConfigurationRepository delegateConfigurationRepository)
        {
            _delegateConfigurationRepository = delegateConfigurationRepository;
        }

        public async Task<DelegateConfigurationResult> Handle(GetDelegateConfigurationQuery request, CancellationToken cancellationToken)
        {
            var delegateConfiguration = await _delegateConfigurationRepository.GetResult(request.Id, cancellationToken);
            if (delegateConfiguration == null)
            {
                throw new UnknownDelegateConfigurationException(string.Format(Global.UnknownDelegate, request.Id));
            }

            return delegateConfiguration;
        }
    }
}
