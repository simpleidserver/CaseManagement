using CaseManagement.BPMN.Domains;
using CaseManagement.Common.Processors;
using System.Collections.Generic;
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

        public async Task<bool> Execute(ProcessInstanceAggregate processInstance, CancellationToken token)
        {
            var isRestarted = false;
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
                    var secondResult = await Execute(executionContext, token);
                    isRestarted = secondResult || isRestarted;
                }
            }

            return isRestarted;
        }

        private async Task<bool> Execute(BPMNExecutionContext context, CancellationToken token)
        {
            var pointer = context.Pointer;
            var nodeDef = context.Instance.GetDefinition(pointer.FlowNodeId);
            var res = await _processorFactory.Execute(context, nodeDef, token);
            var result = res as BPMNExecutionResult;
            var isRestarted = result.IsRestarted;
            if (result.IsNext)
            {
                var ids = context.Instance.CompleteExecutionPointer(pointer, result.NextFlowNodeIds, result.Tokens.ToList());
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
                    var secondResult = await Execute(newExecutionContext, token);
                    isRestarted = isRestarted || secondResult;
                }
            }

            return isRestarted;
        }
    }
}
