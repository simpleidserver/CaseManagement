using CaseManagement.HumanTask.Domains;
using MediatR;
using System.Collections.Generic;

namespace CaseManagement.HumanTask.NotificationInstance.Commands
{
    public class CreateNotificationInstanceCommand : IRequest<string>
    {
        public string NotificationId { get; set; }
        public Dictionary<string, string> Parameters { get; set; }
    }
}
