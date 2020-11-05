using CaseManagement.HumanTask.Domains;
using CaseManagement.HumanTask.Exceptions;
using CaseManagement.HumanTask.HumanTaskInstance.Queries.Results;
using CaseManagement.HumanTask.Localization;
using CaseManagement.HumanTask.Persistence;
using CaseManagement.HumanTask.Resources;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.HumanTask.HumanTaskInstance.Queries.Handlers
{
    public class GetHumanTaskInstanceDetailsQueryHandler : IRequestHandler<GetHumanTaskInstanceDetailsQuery, TaskInstanceDetailsResult>
    {
        private readonly ILogger<GetHumanTaskInstanceDetailsQueryHandler> _logger;
        private readonly ITranslationHelper _translationHelper;
        private readonly IHumanTaskInstanceQueryRepository _humanTaskInstanceQueryRepository;

        public GetHumanTaskInstanceDetailsQueryHandler(
            ILogger<GetHumanTaskInstanceDetailsQueryHandler> logger,
            ITranslationHelper translationHelper,
            IHumanTaskInstanceQueryRepository humanTaskInstanceQueryRepository)
        {
            _logger = logger;
            _translationHelper = translationHelper;
            _humanTaskInstanceQueryRepository = humanTaskInstanceQueryRepository;
        }

        public async Task<TaskInstanceDetailsResult> Handle(GetHumanTaskInstanceDetailsQuery request, CancellationToken cancellationToken)
        {
            var humanTaskInstance = await _humanTaskInstanceQueryRepository.Get(request.HumanTaskInstanceId, cancellationToken);
            if (humanTaskInstance == null)
            {
                _logger.LogError($"HumanTask '{request.HumanTaskInstanceId}' doesn't exist");
                throw new UnknownHumanTaskInstanceException(string.Format(Global.UnknownHumanTaskInstance, request.HumanTaskInstanceId));
            }

            var callbackTxt = new Func<ICollection<PresentationElementInstance>, Localization.Translation>((t) =>
            {
                if (t == null || !t.Any())
                {
                    return null;
                }

                try
                {
                    return _translationHelper.Translate(t);
                }
                catch (BadOperationExceptions) { return null; }
            });
            var name = callbackTxt(humanTaskInstance.Names);
            var subject = callbackTxt(humanTaskInstance.Subjects);
            return TaskInstanceDetailsResult.ToDto(humanTaskInstance, name, subject, null, null);
        }
    }
}
