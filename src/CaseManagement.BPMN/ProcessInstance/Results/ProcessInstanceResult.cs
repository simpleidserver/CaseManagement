using CaseManagement.BPMN.Domains;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CaseManagement.BPMN.ProcessInstance.Results
{
    public class ProcessInstanceResult
    {
        public string Id { get; set; }
        public string Status { get; set; }
        public string ProcessFileId { get; set; }
        public DateTime CreateDateTime { get; set; }
        public DateTime UpdateDateTime { get; set; }
        public ICollection<FlowNodeInstanceResult> ElementInstances { get; set; }

        public static ProcessInstanceResult ToDto(ProcessInstanceAggregate processInstance)
        {
            return new ProcessInstanceResult
            {
                Id = processInstance.AggregateId,
                Status = Enum.GetName(typeof(ProcessInstanceStatus), processInstance.Status),
                ProcessFileId = processInstance.ProcessFileId,
                CreateDateTime = processInstance.CreateDateTime,
                UpdateDateTime = processInstance.UpdateDateTime,
                ElementInstances = processInstance.ElementInstances.Select(_ => FlowNodeInstanceResult.ToDto(_)).ToList()
            };
        }

        public class FlowNodeInstanceResult
        {
            public string Id { get; set; }
            public string FlowNodeId { get; set; }
            public string State { get; set; }
            public string ActivityState { get; set; }
            public Dictionary<string, string> Metadata { get; set; }
            public ICollection<ActivityStateHistoryResult> ActivityStates { get; set; }

            public static FlowNodeInstanceResult ToDto(FlowNodeInstance flowNodeInstance)
            {
                return new FlowNodeInstanceResult
                {
                    Id = flowNodeInstance.Id,
                    ActivityState = flowNodeInstance.ActivityState == null ? null : Enum.GetName(typeof(ActivityStates), flowNodeInstance.ActivityState),
                    FlowNodeId = flowNodeInstance.FlowNodeId,
                    Metadata = flowNodeInstance.Metadata,
                    State = Enum.GetName(typeof(FlowNodeStates), flowNodeInstance.State),
                    ActivityStates = flowNodeInstance.ActivityStates.Select(_ => ActivityStateHistoryResult.ToDto(_)).ToList()
                };
            }
        }

        public class ActivityStateHistoryResult
        {
            public string State { get; set; }
            public DateTime ExecutionDateTime { get; set; }

            public static ActivityStateHistoryResult ToDto(ActivityStateHistory activityStateHistory)
            {
                return new ActivityStateHistoryResult
                {
                    ExecutionDateTime = activityStateHistory.ExecutionDateTime,
                    State = Enum.GetName(typeof(ActivityStates), activityStateHistory.State)
                };
            }
        }
    }
}
