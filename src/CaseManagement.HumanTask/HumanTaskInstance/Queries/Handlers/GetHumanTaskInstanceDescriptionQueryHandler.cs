using CaseManagement.HumanTask.Exceptions;
using CaseManagement.HumanTask.HumanTaskInstance.Queries.Results;
using CaseManagement.HumanTask.Localization;
using CaseManagement.HumanTask.Persistence;
using CaseManagement.HumanTask.Resources;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.HumanTask.HumanTaskInstance.Queries.Handlers
{
    public class GetHumanTaskInstanceDescriptionQueryHandler : IRequestHandler<GetHumanTaskInstanceDescriptionQuery, TaskDescriptionResult>
    {
        private readonly ILogger<GetHumanTaskInstanceDescriptionQueryHandler> _logger;
        private readonly IHumanTaskInstanceQueryRepository _humanTaskInstanceQueryRepository;
        private readonly ITranslationHelper _translationHelper;

        public GetHumanTaskInstanceDescriptionQueryHandler(
            ILogger<GetHumanTaskInstanceDescriptionQueryHandler> logger,
            IHumanTaskInstanceQueryRepository humanTaskInstanceQueryRepository,
            ITranslationHelper translationHelper)
        {
            _logger = logger;
            _humanTaskInstanceQueryRepository = humanTaskInstanceQueryRepository;
            _translationHelper = translationHelper;
        }

        public async Task<TaskDescriptionResult> Handle(GetHumanTaskInstanceDescriptionQuery request, CancellationToken cancellationToken)
        {
            var humanTaskInstance = await _humanTaskInstanceQueryRepository.Get(request.HumanTaskInstanceId, cancellationToken);
            if (humanTaskInstance == null)
            {
                _logger.LogError($"HumanTask '{request.HumanTaskInstanceId}' doesn't exist");
                throw new UnknownHumanTaskInstanceException(string.Format(Global.UnknownHumanTaskInstance, request.HumanTaskInstanceId));
            }

            var translation = _translationHelper.Translate(humanTaskInstance.PresentationElement.Descriptions);
            return new TaskDescriptionResult { Description = translation.Value, ContentType = translation.ContentType };
        }
    }
}
