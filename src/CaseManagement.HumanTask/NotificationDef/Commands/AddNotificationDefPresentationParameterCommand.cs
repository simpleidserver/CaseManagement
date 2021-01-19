using MediatR;
using static CaseManagement.HumanTask.NotificationDef.Results.NotificationDefResult;

namespace CaseManagement.HumanTask.NotificationDef.Commands
{
    public class AddNotificationDefPresentationParameterCommand : IRequest<bool>
    {
        public string Id { get; set; }
        public PresentationParameterResult PresentationParameter { get; set; }
    }
}
