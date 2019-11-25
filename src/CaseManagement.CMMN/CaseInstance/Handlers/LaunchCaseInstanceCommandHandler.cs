using CaseManagement.CMMN.CaseInstance.Commands;
using CaseManagement.Workflow.Engine;
using CaseManagement.Workflow.Persistence;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CaseInstance.Handlers
{
    public class LaunchCaseInstanceCommandHandler : ILaunchCaseInstanceCommandHandler
    {
        private readonly IProcessFlowInstanceQueryRepository _processFlowInstanceQueryRepository;
        private readonly IProcessFlowInstanceCommandRepository _processFlowInstanceCommandRepository;
        private readonly IWorkflowEngine _workflowEngine;

        public LaunchCaseInstanceCommandHandler(IProcessFlowInstanceQueryRepository processFlowInstanceQueryRepository, IProcessFlowInstanceCommandRepository processFlowInstanceCommandRepository, IWorkflowEngine workflowEngine)
        {
            _processFlowInstanceQueryRepository = processFlowInstanceQueryRepository;
            _processFlowInstanceCommandRepository = processFlowInstanceCommandRepository;
            _workflowEngine = workflowEngine;
        }

        public async Task Handle(LaunchCaseInstanceCommand launchCaseInstanceCommand)
        {
            var flowInstance = await _processFlowInstanceQueryRepository.FindFlowInstanceById(launchCaseInstanceCommand.CaseInstanceId);
            if (flowInstance == null)
            {
                // TODO : THROW EXCEPTION.
            }

            var context = new ProcessFlowInstanceExecutionContext(flowInstance);
            await _workflowEngine.Start(flowInstance, context);
            // _processFlowInstanceCommandRepository.Update();
            await _processFlowInstanceCommandRepository.SaveChanges();
        }
    }
}
