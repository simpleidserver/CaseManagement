using CaseManagement.BPMN.Domains;
using CaseManagement.Common.Processors;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.BPMN.ProcessInstance.Processors
{
    public class ProcessInstanceProcessor : IProcessInstanceProcessor
    {
        private readonly IProcessorFactory _processorFactory;

        public ProcessInstanceProcessor(IProcessorFactory processorFactory)
        {
            _processorFactory = processorFactory;
        }

        public async Task Execute(ProcessInstanceAggregate processInstance, CancellationToken token)
        {
            foreach(var executionPath in processInstance.ExecutionPathLst)
            {
                var activePointers = executionPath.Pointers.Where(_ => _.IsActive).ToList();
                foreach(var activePointer in activePointers)
                {
                    var executionContext = new BPMNExecutionContext
                    {
                        Instance = processInstance,
                        Path = executionPath,
                        Pointer = activePointer
                    };
                    await Execute(executionContext, token);
                }
            }
        }

        private async Task Execute(BPMNExecutionContext context, CancellationToken token)
        {
            var pointer = context.Pointer;
            var nodeDef = context.Instance.GetDefinition(pointer.FlowNodeId);
            var result = (await _processorFactory.Execute(context, nodeDef, token)) as BPMNExecutionResult;
            if (result.IsNext)
            {
                var ids = context.Instance.CompleteExecutionPointer(pointer, result.Tokens);
                if (result.IsEltInstanceCompleted)
                {
                    context.Instance.CompleteFlowNodeInstance(pointer.InstanceFlowNodeId);
                }

                if (result.IsNewExecutionPointerRequired)
                {
                    context.Instance.LaunchNewExecutionPointer(pointer);
                }

                foreach (var id in ids)
                {
                    var executionPointer = context.Instance.GetExecutionPointer(pointer.ExecutionPathId, id);
                    var newExecutionContext = context.New(executionPointer);
                    await Execute(newExecutionContext, token);
                }
            }
        }
    }
}
