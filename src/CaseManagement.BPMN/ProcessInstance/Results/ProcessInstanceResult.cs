using CaseManagement.BPMN.Domains;
using Newtonsoft.Json.Linq;
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
        public string ProcessFileName { get; set; }
        public DateTime CreateDateTime { get; set; }
        public DateTime UpdateDateTime { get; set; }
        public ICollection<ExecutionPathResult> ExecutionPaths { get; set; }

        public static ProcessInstanceResult ToDto(ProcessInstanceAggregate processInstance)
        {            
            return new ProcessInstanceResult
            {
                Id = processInstance.AggregateId,
                Status = Enum.GetName(typeof(ProcessInstanceStatus), processInstance.Status),
                ProcessFileId = processInstance.ProcessFileId,
                ProcessFileName = processInstance.ProcessFileName,
                CreateDateTime = processInstance.CreateDateTime,
                UpdateDateTime = processInstance.UpdateDateTime,
                ExecutionPaths = processInstance.ExecutionPathLst.Select(_ => ExecutionPathResult.ToDto(_, processInstance.ElementInstances.ToList())).ToList()
            };
        }

        public class ExecutionPathResult
        {
            public string Id { get; set; }
            public DateTime CreateDateTime { get; set; }
            public ICollection<ExecutionPointerResult> ExecutionPointers { get; set; }

            public static ExecutionPathResult ToDto(ExecutionPath execPath, ICollection<FlowNodeInstance> flowNodesInstances)
            {
                return new ExecutionPathResult
                {
                    Id = execPath.Id,
                    CreateDateTime = execPath.CreateDateTime,
                    ExecutionPointers = execPath.Pointers.Select(_ => ExecutionPointerResult.ToDto(_, flowNodesInstances)).ToList()
                };
            }
        }

        public class ExecutionPointerResult
        {
            public string Id { get; set; }
            public bool IsActive { get; set; }
            public string FlowNodeId { get; set; }
            public ICollection<MessageTokenResult> IncomingTokens { get; set; }
            public ICollection<MessageTokenResult> OutgoingTokens { get; set; }
            public FlowNodeInstanceResult FlowNodeInstance { get; set; }

            public static ExecutionPointerResult ToDto(ExecutionPointer executionPointer, ICollection<FlowNodeInstance> flowNodesInstances)
            {
                return new ExecutionPointerResult
                {
                    Id = executionPointer.Id,
                    IsActive = executionPointer.IsActive,
                    FlowNodeId = executionPointer.FlowNodeId,
                    FlowNodeInstance = FlowNodeInstanceResult.ToDto(flowNodesInstances.First(_ => _.EltId == executionPointer.InstanceFlowNodeId)),
                    IncomingTokens = executionPointer.Incoming.Select(_ => MessageTokenResult.ToDto(_)).ToList(),
                    OutgoingTokens = executionPointer.Outgoing.Select(_ => MessageTokenResult.ToDto(_)).ToList()
                };
            }
        }

        public class MessageTokenResult
        {
            public string Name { get; set; }
            public JObject Content { get; set; }

            public static MessageTokenResult ToDto(MessageToken messageToken)
            {
                return new MessageTokenResult
                {
                    Name = messageToken.Name,
                    Content = messageToken.JObjMessageContent
                };
            }
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
                    Id = flowNodeInstance.EltId,
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
            public string Message { get; set; }

            public static ActivityStateHistoryResult ToDto(ActivityStateHistory activityStateHistory)
            {
                return new ActivityStateHistoryResult
                {
                    ExecutionDateTime = activityStateHistory.ExecutionDateTime,
                    State = Enum.GetName(typeof(ActivityStates), activityStateHistory.State),
                    Message = activityStateHistory.Message
                };
            }
        }
    }
}
