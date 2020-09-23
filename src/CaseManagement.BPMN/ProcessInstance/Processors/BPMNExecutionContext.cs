using CaseManagement.BPMN.Domains;
using CaseManagement.Common.Processors;

namespace CaseManagement.BPMN.ProcessInstance.Processors
{
    public class BPMNExecutionContext : ExecutionContext<ProcessInstanceAggregate>
    {
        public ExecutionPath Path { get; set; }
        public ExecutionPointer Pointer { get; set; }

        public BPMNExecutionContext New(ExecutionPointer pointer)
        {
            return new BPMNExecutionContext
            {
                Instance = Instance,
                Pointer = pointer
            };
        }
    }
}
