using CaseManagement.BPMN.Common;
using CaseManagement.BPMN.Domains;
using CaseManagement.BPMN.Infrastructure.Jobs.Notifications;
using CaseManagement.BPMN.Persistence.EF.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Concurrent;
using System.Linq;

namespace CaseManagement.BPMN.Persistence.EF.DomainMapping
{
    public static class ProcessInstanceMapper
    {
        #region To domains

        public static ProcessInstanceAggregate ToDomain(this ProcessInstanceModel processInstance)
        {
            return new ProcessInstanceAggregate
            {
                AggregateId = processInstance.AggregateId,
                CreateDateTime = processInstance.CreateDateTime,
                ProcessFileName = processInstance.ProcessFileName,
                Version = processInstance.Version,
                NameIdentifier = processInstance.NameIdentifier,
                UpdateDateTime = processInstance.UpdateDateTime,
                Status = (ProcessInstanceStatus)processInstance.Status,
                ElementInstances = new ConcurrentBag<FlowNodeInstance>(processInstance.ElementInstances.Select(_ => _.ToDomain()).ToList()),
                Interfaces = new ConcurrentBag<BPMNInterface>(processInstance.Interfaces.Select(_ => _.ToDomain()).ToList()),
                StateTransitions = new ConcurrentBag<StateTransitionNotification>(processInstance.StateTransitions.Select(_ => _.ToDomain()).ToList()),
                ExecutionPathLst = new ConcurrentBag<ExecutionPath>(processInstance.ExecutionPathLst.Select(_ => _.ToDomain()).ToList()),
                ElementDefs = new ConcurrentBag<BaseFlowNode>(processInstance.ElementDefs.Select(_ => _.ToDomain()).ToList()),
                ItemDefs = new ConcurrentBag<ItemDefinition>(processInstance.ItemDefs.Select(_ => _.ToDomain()).ToList()),
                Messages = new ConcurrentBag<Message>(processInstance.Messages.Select(_ => _.ToDomain()).ToList()),
                SequenceFlows = new ConcurrentBag<SequenceFlow>(processInstance.SequenceFlows.Select(_ => _.ToDomain()).ToList()),
                ProcessFileId = processInstance.ProcessFileId,
            };
        }

        public static FlowNodeInstance ToDomain(this FlowNodeInstanceModel flowNodeInstance)
        {
            return new FlowNodeInstance
            {
                State = flowNodeInstance.State,
                Id = flowNodeInstance.Id,
                Metadata = flowNodeInstance.Metadata,
                ActivityState = flowNodeInstance.ActivityState,
                FlowNodeId = flowNodeInstance.FlowNodeId,
                ActivityStates = flowNodeInstance.ActivityStates.Select(_ => _.ToDomain()).ToList()
            };
        }

        public static ActivityStateHistory ToDomain(this ActivityStateHistoryModel activityStateHistory)
        {
            return new ActivityStateHistory
            {
                ExecutionDateTime = activityStateHistory.ExecutionDateTime,
                State = activityStateHistory.State,
                Message = activityStateHistory.Message
            };
        }

        public static BPMNInterface ToDomain(this BPMNInterfaceModel bpmnInterface)
        {
            return new BPMNInterface
            {
                Id = bpmnInterface.Id.ToString(),
                Name = bpmnInterface.Name,
                ImplementationRef = bpmnInterface.ImplementationRef,
                Operations = bpmnInterface.Operations.Select(_ => _.ToDomain()).ToList()
            };
        }

        public static Operation ToDomain(this OperationModel operation)
        {
            return new Operation
            {
                Id = operation.Id.ToString(),
                ImplementationRef = operation.ImplementationRef,
                InMessageRef = operation.InMessageRef,
                Name = operation.Name,
                OutMessageRef = operation.OutMessageRef
            };
        }

        public static StateTransitionNotification ToDomain(this StateTransitionTokenModel stateTransition)
        {
            return new StateTransitionNotification(stateTransition.Id.ToString())
            {
                Content = stateTransition.SerializedContent,
                FlowNodeInstanceId = stateTransition.FlowNodeInstanceId,
                State = stateTransition.State
            };
        }

        public static ExecutionPath ToDomain(this ExecutionPathModel executionPath)
        {
            return new ExecutionPath
            {
                CreateDateTime = executionPath.CreateDateTime,
                Id = executionPath.Id.ToString(),
                Pointers = executionPath.Pointers.Select(_ => _.ToDomain()).ToList()
            };
        }

        public static ExecutionPointer ToDomain(this ExecutionPointerModel executionPointer)
        {
            return new ExecutionPointer
            {
                Id = executionPointer.Id,
                FlowNodeId = executionPointer.FlowNodeId,
                IsActive = executionPointer.IsActive,
                InstanceFlowNodeId = executionPointer.InstanceFlowNodeId,
                Incoming = executionPointer.Incoming.Select(_ => _.ToDomain()).ToList(),
                Outgoing = executionPointer.Outgoing.Select(_ => _.ToDomain()).ToList()
            };
        }

        public static MessageToken ToDomain(this MessageTokenModel messageToken)
        {
            return new MessageToken
            {
                MessageContent = messageToken.SerializedContent,
                Name = messageToken.Name
            };
        }

        public static BaseFlowNode ToDomain(this FlowNodeModel flowNode)
        {
            var result = BaseFlowNode.Deserialize(flowNode.SerializedContent, flowNode.Type);
            result.Name = flowNode.Name;
            return result;
        }

        public static ItemDefinition ToDomain(this ItemDefinitionModel itemDef)
        {
            return new ItemDefinition
            {
                Id = itemDef.Id.ToString(),
                IsCollection = itemDef.IsCollection,
                ItemKind = itemDef.ItemKind,
                StructureRef = itemDef.StructureRef
            };
        }

        public static Message ToDomain(this MessageModel message)
        {
            return new Message
            {
                Id = message.Id.ToString(),
                ItemRef = message.ItemRef,
                Name = message.Name
            };
        }

        public static SequenceFlow ToDomain(this SequenceFlowModel sequenceFlow)
        {
            return new SequenceFlow
            {
                ConditionExpression = sequenceFlow.ConditionExpression,
                Id = sequenceFlow.EltId,
                Name = sequenceFlow.Name,
                SourceRef = sequenceFlow.SourceRef,
                TargetRef = sequenceFlow.TargetRef
            };
        }

        #endregion

        #region To models

        public static ProcessInstanceModel ToModel(this ProcessInstanceAggregate processInstance)
        {
            return new ProcessInstanceModel
            {
                AggregateId = processInstance.AggregateId,
                CreateDateTime = processInstance.CreateDateTime,
                ProcessFileName = processInstance.ProcessFileName,
                Version = processInstance.Version,
                Status = (int)processInstance.Status,
                UpdateDateTime = processInstance.UpdateDateTime,
                ElementInstances = processInstance.ElementInstances.Select(_ => _.ToModel()).ToList(),
                Interfaces = processInstance.Interfaces.Select(_ => _.ToModel()).ToList(),
                StateTransitions = processInstance.StateTransitions.Select(_ => _.ToModel()).ToList(),
                ExecutionPathLst = processInstance.ExecutionPathLst.Select(_ => _.ToModel()).ToList(),
                ElementDefs = processInstance.ElementDefs.Select(_ => _.ToModel()).ToList(),
                ItemDefs = processInstance.ItemDefs.Select(_ => _.ToModel()).ToList(),
                Messages = processInstance.Messages.Select(_ => _.ToModel()).ToList(),
                SequenceFlows = processInstance.SequenceFlows.Select(_ => _.ToModel()).ToList(),
                ProcessFileId = processInstance.ProcessFileId,
            };
        }

        public static FlowNodeInstanceModel ToModel(this FlowNodeInstance flowNodeInstance)
        {
            return new FlowNodeInstanceModel
            {
                State = flowNodeInstance.State,
                Id = flowNodeInstance.Id,
                Metadata = flowNodeInstance.Metadata,
                ActivityState = flowNodeInstance.ActivityState,
                FlowNodeId = flowNodeInstance.FlowNodeId,
                ActivityStates = flowNodeInstance.ActivityStates.Select(_ => _.ToModel()).ToList()
            };
        }

        public static ActivityStateHistoryModel ToModel(this ActivityStateHistory activityStateHistory)
        {
            return new ActivityStateHistoryModel
            {
                ExecutionDateTime = activityStateHistory.ExecutionDateTime,
                State = activityStateHistory.State,
                Message = activityStateHistory.Message
            };
        }

        public static BPMNInterfaceModel ToModel(this BPMNInterface bpmnInterface)
        {
            return new BPMNInterfaceModel
            {
                Name = bpmnInterface.Name,
                ImplementationRef = bpmnInterface.ImplementationRef,
                Operations = bpmnInterface.Operations.Select(_ => _.ToModel()).ToList()
            };
        }

        public static OperationModel ToModel(this Operation operation)
        {
            return new OperationModel
            {
                ImplementationRef = operation.ImplementationRef,
                InMessageRef = operation.InMessageRef,
                Name = operation.Name,
                OutMessageRef = operation.OutMessageRef
            };
        }

        public static StateTransitionTokenModel ToModel(this StateTransitionNotification stateTransition)
        {
            return new StateTransitionTokenModel
            {
                SerializedContent = stateTransition.Content == null ? null : stateTransition.Content.ToString(),
                FlowNodeInstanceId = stateTransition.FlowNodeInstanceId,
                State = stateTransition.State
            };
        }

        public static ExecutionPathModel ToModel(this ExecutionPath executionPath)
        {
            return new ExecutionPathModel
            {
                Id = executionPath.Id,
                CreateDateTime = executionPath.CreateDateTime,
                Pointers = executionPath.Pointers.Select(_ => _.ToModel()).ToList()
            };
        }

        public static ExecutionPointerModel ToModel(this ExecutionPointer executionPointer)
        {
            var tokens = executionPointer.Incoming.Select(_ => _.ToModel(MessageTokenDirections.INCOMING)).ToList();
            tokens.AddRange(executionPointer.Outgoing.Select(_ => _.ToModel(MessageTokenDirections.OUTGOING)).ToList());
            return new ExecutionPointerModel
            {
                Id = executionPointer.Id,
                FlowNodeId = executionPointer.FlowNodeId,
                IsActive = executionPointer.IsActive,
                InstanceFlowNodeId = executionPointer.InstanceFlowNodeId,
                Tokens = tokens
            };
        }

        public static MessageTokenModel ToModel(this MessageToken messageToken, MessageTokenDirections direction)
        {
            var msg = messageToken as MessageToken;
            return new MessageTokenModel
            {
                SerializedContent = msg.MessageContent == null ? null : msg.MessageContent.ToString(),
                Name = messageToken.Name,
                Direction = direction
            };
        }

        public static FlowNodeModel ToModel(this BaseFlowNode flowNode)
        {
            return new FlowNodeModel
            {
                Name = flowNode.Name,
                Type = flowNode.FlowNode,
                SerializedContent = JsonConvert.SerializeObject(flowNode).ToString()
            };
        }

        public static ItemDefinitionModel ToModel(this ItemDefinition itemDef)
        {
            return new ItemDefinitionModel
            {
                IsCollection = itemDef.IsCollection,
                ItemKind = itemDef.ItemKind,
                StructureRef = itemDef.StructureRef
            };
        }

        public static MessageModel ToModel(this Message message)
        {
            return new MessageModel
            {
                ItemRef = message.ItemRef,
                Name = message.Name
            };
        }

        public static SequenceFlowModel ToModel(this SequenceFlow sequenceFlow)
        {
            return new SequenceFlowModel
            {
                ConditionExpression = sequenceFlow.ConditionExpression,
                Name = sequenceFlow.Name,
                SourceRef = sequenceFlow.SourceRef,
                TargetRef = sequenceFlow.TargetRef,
                EltId = sequenceFlow.Id
            };
        }

        #endregion
    }
}
