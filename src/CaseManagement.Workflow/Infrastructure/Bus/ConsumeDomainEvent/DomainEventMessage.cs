namespace CaseManagement.Workflow.Infrastructure.Bus.ConsumeDomainEvent
{
    public class DomainEventMessage
    {
        public string AssemblyQualifiedName { get; set; }
        public string Content { get; set; }
    }
}
