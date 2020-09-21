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

        public async Task Execute(ProcessInstanceAggregate processInstance, CancellationToken cancellationToken)
        {
            ICollection<BaseFlowNode> elts = processInstance.Elements.ToList();
            var branch = ExecutionBranch.Build(elts, null);
            var executionContext = new BPMNExecutionContext
            {
                Instance = processInstance
            };
            await ExecuteBranch(executionContext, branch, cancellationToken);
        }

        private async Task ExecuteBranch(BPMNExecutionContext executionContext, BaseExecutionBranch<BaseFlowNode> branch, CancellationToken token)
        {
            var taskLst = new List<Task>();
            foreach(var node in branch.Nodes)
            {
                taskLst.Add(HandleActivity(executionContext, node, token));
            }

            await Task.WhenAll(taskLst);
            if (branch.NextBranch != null)
            {
                await ExecuteBranch(executionContext, branch.NextBranch, token);
            }
        }

        private Task HandleActivity(BPMNExecutionContext executionContext, BaseFlowNode activity, CancellationToken token)
        {
            return _processorFactory.Execute(executionContext, activity, token);
        }
    }
}
