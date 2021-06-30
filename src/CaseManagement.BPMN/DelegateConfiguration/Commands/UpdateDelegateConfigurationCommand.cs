using MediatR;
using System.Collections.Generic;

namespace CaseManagement.BPMN.DelegateConfiguration.Commands
{
    public class UpdateDelegateConfigurationCommand : IRequest<bool>
    {
        public string Id { get; set; }
        public Dictionary<string, string> Records { get; set; }
    }
}
