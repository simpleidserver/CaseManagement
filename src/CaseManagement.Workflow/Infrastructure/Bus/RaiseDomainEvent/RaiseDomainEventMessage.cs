namespace CaseManagement.Workflow.Infrastructure.Bus.RaiseDomainEvent
{
    public class RaiseDomainEventMessage
    {
        public RaiseDomainEventMessage(string processFlowId, string assemblyQualifiedName, string content)
        {
            ProcessFlowId = processFlowId;
            AssemblyQualifiedName = assemblyQualifiedName;
            Content = content;
        }

        public string ProcessFlowId { get; set; }
        public string AssemblyQualifiedName { get; set; }
        public string Content { get; set; }
    }
}
