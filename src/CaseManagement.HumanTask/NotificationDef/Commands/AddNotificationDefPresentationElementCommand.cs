using MediatR;
using static CaseManagement.HumanTask.NotificationDef.Results.NotificationDefResult;

namespace CaseManagement.HumanTask.NotificationDef.Commands
{
    public class AddNotificationDefPresentationElementCommand : IRequest<bool>
    {
        public string Id { get; set; }
        public NotificationPresentationElementDefinitionResult PresentationElement { get; set; }
    }
}