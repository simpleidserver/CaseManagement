using MediatR;
using static CaseManagement.HumanTask.HumanTaskDef.Results.HumanTaskDefResult;

namespace CaseManagement.HumanTask.NotificationDef.Commands
{
    public class AddNotificationDefParameterCommand : IRequest<string>
    {
        public string Id { get; set; }
        public ParameterResult Parameter { get; set; }
    }
}
