using CaseManagement.HumanTask.Domains;
using MediatR;

namespace CaseManagement.HumanTask.NotificationDef.Commands
{
    public class DeleteNotificationDefPresentationElementCommand : IRequest<bool>
    {
        public string Id { get; set; }
        public PresentationElementUsages Usage { get; set; }
        public string Language { get; set; }
    }
}
