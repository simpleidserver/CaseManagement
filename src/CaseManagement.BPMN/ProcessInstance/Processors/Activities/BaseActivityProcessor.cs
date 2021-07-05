using CaseManagement.BPMN.Domains;
using CaseManagement.BPMN.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.BPMN.ProcessInstance.Processors.Activities
{
    public abstract class BaseActivityProcessor<T>: BaseFlowNodeProcessor<T> where T : BaseActivity
    {
        protected override async Task<BPMNExecutionResult> Handle(BPMNExecutionContext executionContext, T elt,  CancellationToken cancellationToken)
        {
            var pointer = executionContext.Pointer;
            if (pointer.Incoming.Count() < elt.StartQuantity)
            {
                return BPMNExecutionResult.Block();
            }

            // Page : 428 : BPMN2.0.2
            var flowNodeIds = GetNextFlowNodeIds(executionContext, elt);
            var instance = executionContext.Instance.GetInstance(pointer.InstanceFlowNodeId);
            if (instance.ActivityState == null)
            {
                executionContext.Instance.UpdateState(instance, ActivityStates.READY);
            }

            if (instance.ActivityState == ActivityStates.READY)
            {
                executionContext.Instance.UpdateState(instance, ActivityStates.ACTIVE);
            }

            if (instance.ActivityState == ActivityStates.ACTIVE)
            {
                executionContext.Instance.UpdateState(instance, ActivityStates.COMPLETING);
            }

            var outcome = new List<MessageToken>();
            outcome.AddRange(executionContext.Pointer.Incoming);
            if (instance.ActivityState == ActivityStates.COMPLETING)
            {
                try
                {
                    executionContext.Instance.LaunchBoundaryEvts(executionContext.Pointer.ExecutionPathId, elt.BoundaryEvtRefs, executionContext.Pointer.Incoming.ToList());
                    var addOutcome = await Process(executionContext, elt, cancellationToken);
                    outcome.AddRange(addOutcome);
                    executionContext.Instance.UpdateState(instance, ActivityStates.COMPLETED);
                    executionContext.Instance.CompleteBoundaryEvts(executionContext.Pointer.ExecutionPathId, elt.BoundaryEvtRefs);
                }
                catch(FlowNodeInstanceRestartedException)
                {
                    return BPMNExecutionResult.Restart();
                }
                catch(FlowNodeInstanceBlockedException)
                {
                    return BPMNExecutionResult.Block();
                }
                catch(Exception ex)
                {
                    executionContext.Instance.UpdateState(instance, ActivityStates.FAILING, ex.ToString());
                    return BPMNExecutionResult.Block();
                }
            }

            if (instance.ActivityState == ActivityStates.FAILING)
            {
                return BPMNExecutionResult.Block();
            }

            return BPMNExecutionResult.Next(flowNodeIds, outcome);
        }

        protected abstract Task<ICollection<MessageToken>> Process(BPMNExecutionContext context, T elt,  CancellationToken cancellationToken);
    }
}
