using CaseManagement.HumanTask.Domains;
using CaseManagement.HumanTask.Exceptions;
using CaseManagement.HumanTask.HumanTaskInstance.Queries.Results;
using CaseManagement.HumanTask.Localization;
using CaseManagement.HumanTask.Parser;
using CaseManagement.HumanTask.Persistence;
using CaseManagement.HumanTask.Resources;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.HumanTask.HumanTaskInstance.Queries.Handlers
{
    public class GetHumanTaskInstanceDescriptionQueryHandler : IRequestHandler<GetHumanTaskInstanceDescriptionQuery, TaskDescriptionResult>
    {
        private readonly ILogger<GetHumanTaskInstanceDescriptionQueryHandler> _logger;
        private readonly IHumanTaskInstanceQueryRepository _humanTaskInstanceQueryRepository;
        private readonly IHumanTaskDefQueryRepository _humanTaskDefQueryRepository;
        private readonly ITranslationHelper _translationHelper;
        private readonly IParameterParser _parameterParser;

        public GetHumanTaskInstanceDescriptionQueryHandler(
            ILogger<GetHumanTaskInstanceDescriptionQueryHandler> logger,
            IHumanTaskInstanceQueryRepository humanTaskInstanceQueryRepository,
            IHumanTaskDefQueryRepository humanTaskDefQueryRepository,
            ITranslationHelper translationHelper,
            IParameterParser parameterParser)
        {
            _logger = logger;
            _humanTaskInstanceQueryRepository = humanTaskInstanceQueryRepository;
            _humanTaskDefQueryRepository = humanTaskDefQueryRepository;
            _translationHelper = translationHelper;
            _parameterParser = parameterParser;
        }

        public async Task<TaskDescriptionResult> Handle(GetHumanTaskInstanceDescriptionQuery request, CancellationToken cancellationToken)
        {
            var humanTaskInstance = await _humanTaskInstanceQueryRepository.Get(request.HumanTaskInstanceId, cancellationToken);
            if (humanTaskInstance == null)
            {
                _logger.LogError($"HumanTask '{request.HumanTaskInstanceId}' doesn't exist");
                throw new UnknownHumanTaskInstanceException(string.Format(Global.UnknownHumanTaskInstance, request.HumanTaskInstanceId));
            }

            var humanTaskDef = await _humanTaskDefQueryRepository.Get(humanTaskInstance.HumanTaskDefName, cancellationToken);
            var parameters = _parameterParser.ParsePresentationParameters(humanTaskDef.PresentationElement.PresentationParameters, humanTaskInstance);
            ICollection<Text> descriptions = humanTaskDef.PresentationElement.Descriptions.Cast<Text>().ToList();
            var translation = _translationHelper.Translate(descriptions, parameters);
            return new TaskDescriptionResult { Description = translation.Value, ContentType = translation.ContentType };
        }
    }
}
