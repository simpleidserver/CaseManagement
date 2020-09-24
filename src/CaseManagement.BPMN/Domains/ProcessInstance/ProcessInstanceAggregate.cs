using CaseManagement.BPMN.Common;
using CaseManagement.BPMN.Helpers;
using CaseManagement.Common.Domains;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace CaseManagement.BPMN.Domains
{
    public class ProcessInstanceAggregate : BaseAggregate
    {
        public ProcessInstanceAggregate()
        {
            ElementDefs = new ConcurrentBag<BaseFlowNode>();
            ElementInstances = new ConcurrentBag<FlowNodeInstance>();
            ExecutionPathLst = new ConcurrentBag<ExecutionPath>();
            ItemDefs = new ConcurrentBag<ItemDefinition>();
            Interfaces = new ConcurrentBag<BPMNInterface>();
            Messages = new ConcurrentBag<Message>();
        }

        public string InstanceId { get; set; }
        public string CommonId { get; set; }
        public string ProcessFileId { get; set; }
        public string ProcessId { get; set; }
        public DateTime CreateDateTime { get; set; }
        public DateTime UpdateDateTime { get; set; }
        public ICollection<StartEvent> StartEvts => ElementDefs.Where(_ => _ is StartEvent).Cast<StartEvent>().ToList();
        public ConcurrentBag<ItemDefinition> ItemDefs { get; set; }
        public ConcurrentBag<BPMNInterface> Interfaces { get; set; }
        public ConcurrentBag<Message> Messages { get; set; }
        public ConcurrentBag<BaseFlowNode> ElementDefs { get; set; }
        public ConcurrentBag<FlowNodeInstance> ElementInstances { get; set; }
        public ConcurrentBag<ExecutionPath> ExecutionPathLst { get; set; }

        #region Getters

        public bool IsIncomingSatisfied(ExecutionPointer pointer)
        {
            var nodeDef = GetDefinition(pointer.FlowNodeId);
            if (nodeDef.Incoming == null || !nodeDef.Incoming.Any())
            {
                return true;
            }

            var activity = nodeDef as BaseActivity;
            if (activity == null)
            {
                if (pointer.Incoming.Any())
                {
                    return true;
                }
            }
            else
            {
                return pointer.Incoming.Count() >= activity.StartQuantity;
            }

            return false;
        }

        public BaseFlowNode GetDefinition(string elementId)
        {
            return ElementDefs.FirstOrDefault(_ => _.Id == elementId);
        }

        public BaseActivity GetActivity(string elementId)
        {
            return GetDefinition(elementId) as BaseActivity;
        }

        public FlowNodeInstance GetInstance(string id)
        {
            return ElementInstances.FirstOrDefault(_ => _.Id == id);
        }

        public FlowNodeInstance GetActiveInstance(string elementId)
        {
            return ElementInstances.FirstOrDefault(_ => _.FlowNodeId == elementId && _.State == FlowNodeStates.Active);
        }

        public ExecutionPath GetExecutionPath(string executionPathId)
        {
            return ExecutionPathLst.FirstOrDefault(_ => _.Id == executionPathId);
        }

        public ExecutionPointer GetExecutionPointer(string executionPathId, string executionPointerId)
        {
            var path = GetExecutionPath(executionPathId);
            return path.Pointers.FirstOrDefault(_ => _.Id == executionPointerId);
        }

        public ExecutionPointer GetActiveExecutionPointer(string executionPathId, string flowInstanceId)
        {
            var path = GetExecutionPath(executionPathId);
            return path.Pointers.FirstOrDefault(_ => _.InstanceFlowNodeId == flowInstanceId && _.IsActive);
        }

        public Message GetMessage(string messageRef)
        {
            return Messages.FirstOrDefault(_ => _.Id == messageRef);
        }

        public Operation GetOperation(string operationRef)
        {
            return Interfaces.SelectMany(_ => _.Operations).FirstOrDefault(_ => _.Id == operationRef);
        }

        public ItemDefinition GetItem(string itemRef)
        {
            return ItemDefs.FirstOrDefault(_ => _.Id == itemRef);
        }

        public bool IsMessageCorrect(MessageToken messageToken)
        {
            var message = GetMessage(messageToken.Name);
            if (message == null || messageToken.Name != messageToken.Name)
            {
                return false;
            }

            var item = GetItem(message.ItemRef);
            if (item == null)
            {
                return true;
            }

            var type = TypeResolver.ResolveType(item.StructureRef);
            if (item ==  null || type == null)
            {
                return false;
            }

            object result = null;
            try
            {
                result = JsonConvert.DeserializeObject(messageToken.MessageContent.ToString(), type);
            }
            catch
            {
                return false;
            }

            return result != null;
        }

        #endregion

        #region Operations

        public void NewExecutionPath()
        {
            var pathId = Guid.NewGuid().ToString();
            var evt = new ExecutionPathCreatedEvent(Guid.NewGuid().ToString(), AggregateId, Version + 1, pathId, DateTime.UtcNow);
            Handle(evt);
            DomainEvents.Add(evt);
            foreach (var startEvt in StartEvts)
            {
                TryAddExecutionPointer(pathId, startEvt);
            }
        }

        public ICollection<string> CompleteExecutionPointer(ExecutionPointer pointer, ICollection<BaseToken> outcomeValues)
        {
            var node = GetDefinition(pointer.FlowNodeId);
            var evt = new ExecutionPointerCompletedEvent(Guid.NewGuid().ToString(), AggregateId, Version + 1, pointer.ExecutionPathId, pointer.Id, DateTime.UtcNow);
            Handle(evt);
            DomainEvents.Add(evt);
            var result = new List<string>();
            foreach (var elementId in node.Outgoing)
            {
                result.Add(TryAddExecutionPointer(pointer.ExecutionPathId, GetDefinition(elementId), outcomeValues));
            }

            return result;
        }

        public string LaunchNewExecutionPointer(ExecutionPointer pointer)
        {
            return TryAddExecutionPointer(pointer.ExecutionPathId, GetDefinition(pointer.FlowNodeId));
        }

        public void CompleteFlowNodeInstance(string id)
        {
            var evt = new FlowNodeInstanceCompletedEvent(Guid.NewGuid().ToString(), AggregateId, Version + 1, id, DateTime.UtcNow);
            Handle(evt);
            DomainEvents.Add(evt);
        }

        public void UpdateState(FlowNodeInstance instance, ActivityStates state)
        {
            var evt = new ActivityStateUpdatedEvent(Guid.NewGuid().ToString(), AggregateId, Version + 1, instance.Id, state, DateTime.UtcNow);
            Handle(evt);
            DomainEvents.Add(evt);
        }

        public void AddFlowNodeDef(BaseFlowNode node)
        {
            var evt = new FlowNodeDefCreatedEvent(Guid.NewGuid().ToString(), AggregateId, Version + 1, JsonConvert.SerializeObject(node),  DateTime.UtcNow);
            Handle(evt);
            DomainEvents.Add(evt);
        }

        public void ConsumeMessage(MessageToken messageToken)
        {
            foreach(var executionPath in ExecutionPathLst)
            {
                ConsumeMessage(executionPath, messageToken);
            }
        }

        #endregion

        #region Helpers

        private string TryAddExecutionPointer(string pathId, BaseFlowNode flowNode, ICollection<BaseToken> outcomeValues = null)
        {
            ICollection<BaseToken> incoming = new List<BaseToken>();
            if (outcomeValues != null)
            {
                incoming = outcomeValues;
            }

            var instanceId = string.Empty;
            if (!TryAddInstance(flowNode, out instanceId))
            {
                var pointer = GetActiveExecutionPointer(pathId, instanceId);
                if (pointer != null)
                {
                    var e = new IncomingTokenAddedEvent(Guid.NewGuid().ToString(), AggregateId, Version + 1, pointer.ExecutionPathId, pointer.Id, JsonConvert.SerializeObject(incoming), DateTime.UtcNow);
                    Handle(e);
                    DomainEvents.Add(e);
                    return pointer.Id;
                }
            }

            var id = Guid.NewGuid().ToString();
            var evt = new ExecutionPointerAddedEvent(Guid.NewGuid().ToString(), AggregateId, Version + 1, id, pathId, instanceId, flowNode.Id, JsonConvert.SerializeObject(incoming), DateTime.UtcNow);
            Handle(evt);
            DomainEvents.Add(evt);
            return id;
        }

        private bool TryAddInstance(BaseFlowNode node, out string instanceId)
        {
            return TryAddInstance(node.Id, out instanceId);
        }

        private bool TryAddInstance(string elementId, out string instanceId)
        {
            var instance = GetActiveInstance(elementId);
            if (instance != null)
            {
                instanceId = instance.Id;
                return false;
            }

            instanceId = Guid.NewGuid().ToString();
            var evt = new FlowNodeInstanceAddedEvent(Guid.NewGuid().ToString(), AggregateId, Version + 1, instanceId, elementId, DateTime.UtcNow);
            Handle(evt);
            DomainEvents.Add(evt);
            return true;
        }

        private void ConsumeMessage(ExecutionPath executionPath, MessageToken messageToken)
        {
            foreach(var pointer in executionPath.ActivePointers)
            {
                var nodeDef = GetDefinition(pointer.FlowNodeId) as BaseCatchEvent;
                if(nodeDef == null)
                {
                    continue;
                }

                if (nodeDef.EventDefinitions.Any(_ => _.IsSatisfied(this, messageToken)))
                {
                    var tokens = new List<BaseToken> { messageToken };
                    var evt = new IncomingTokenAddedEvent(Guid.NewGuid().ToString(), AggregateId, Version + 1, executionPath.Id, pointer.Id, JsonConvert.SerializeObject(tokens), DateTime.UtcNow);
                    Handle(evt);
                    DomainEvents.Add(evt);
                }
            }
        }

        #endregion

        public static ProcessInstanceAggregate New(List<DomainEvent> evts)
        {
            var result = new ProcessInstanceAggregate();
            foreach(var evt in evts)
            {
                result.Handle(evt);
            }

            return result;
        }

        public static ProcessInstanceAggregate New(string instanceId, string processId, string processFileId, ICollection<BaseFlowNode> elements, ICollection<BPMNInterface> interfaces, ICollection<Message> messages, ICollection<ItemDefinition> itemDefs)
        {
            var result = new ProcessInstanceAggregate();
            var evt = new ProcessInstanceCreatedEvent(Guid.NewGuid().ToString(), BuildId(instanceId, processId, processFileId), 0, processFileId, processId, interfaces, messages, itemDefs, DateTime.UtcNow);
            result.Handle(evt);
            foreach (var elt in elements)
            {
                result.AddFlowNodeDef(elt);
            }

            result.DomainEvents.Add(evt);
            return result;
        }

        public override object Clone()
        {
            return new ProcessInstanceAggregate
            {
                AggregateId = AggregateId,
                CommonId = CommonId,
                InstanceId = InstanceId,
                ProcessFileId = ProcessFileId,
                ProcessId = ProcessId,
                Version = Version,
                CreateDateTime = CreateDateTime,
                UpdateDateTime = UpdateDateTime,
                ElementDefs = new ConcurrentBag<BaseFlowNode>(ElementDefs.Select(_ => (BaseFlowNode)_.Clone())),
                ElementInstances =new ConcurrentBag<FlowNodeInstance>(ElementInstances.Select(_ => (FlowNodeInstance)_.Clone())),
                ExecutionPathLst = new ConcurrentBag<ExecutionPath>(ExecutionPathLst.Select(_ => (ExecutionPath)_.Clone())),
                ItemDefs = new ConcurrentBag<ItemDefinition>(ItemDefs.Select(_ => (ItemDefinition)_.Clone())),
                Messages = new ConcurrentBag<Message>(Messages.Select(_ => (Message)_.Clone())),
                Interfaces = new ConcurrentBag<BPMNInterface>(Interfaces.Select(_ => (BPMNInterface)_.Clone()))
            };
        }

        public string GetStreamName()
        {
            return GetStreamName(AggregateId);
        }

        #region Handle events

        public override void Handle(dynamic evt)
        {
            Handle(evt);
        }

        private void Handle(ProcessInstanceCreatedEvent evt)
        {
            AggregateId = evt.AggregateId;
            Version = evt.Version;
            ProcessFileId = evt.ProcessFileId;
            ProcessId = evt.ProcessId;
            CreateDateTime = evt.CreateDateTime;
            Interfaces = new ConcurrentBag<BPMNInterface>(evt.Interfaces);
            Messages = new ConcurrentBag<Message>(evt.Messages);
            ItemDefs = new ConcurrentBag<ItemDefinition>(evt.ItemDefs);
        }

        private void Handle(FlowNodeDefCreatedEvent evt)
        {
            var elt = BaseFlowNode.Deserialize(evt.SerializedContent);
            ElementDefs.Add(elt);
            Version = evt.Version;
            UpdateDateTime = evt.UpdateDateTime;
        }

        private void Handle(FlowNodeInstanceAddedEvent evt)
        {
            var instance = new FlowNodeInstance { FlowNodeId = evt.FlowNodeId, Id = evt.FlowNodeInstanceId };
            ElementInstances.Add(instance);
            Version = evt.Version;
            UpdateDateTime = evt.CreateDateTime;
        }

        private void Handle(ExecutionPathCreatedEvent evt)
        {
            var path = new ExecutionPath { Id = evt.ExecutionPathId, CreateDateTime = evt.CreateDateTime };
            ExecutionPathLst.Add(path);
            Version = evt.Version;
            UpdateDateTime = evt.CreateDateTime;
        }

        private void Handle(ExecutionPointerAddedEvent evt)
        {
            var tokens = BaseToken.DeserializeLst(evt.SerializedTokens);
            var path = GetExecutionPath(evt.ExecutionPathId);
            path.Pointers.Add(new ExecutionPointer
            {
                Id = evt.ExecutionPointerId,
                ExecutionPathId = path.Id,
                FlowNodeId = evt.FlowNodeId,
                InstanceFlowNodeId = evt.FlowNodeInstanceId,   
                Incoming = tokens
            });
            Version = evt.Version;
            UpdateDateTime = evt.CreateDateTime;
        }

        private void Handle(ExecutionPointerCompletedEvent evt)
        {
            var pointer = GetExecutionPointer(evt.ExecutionPathId, evt.ExecutionPointerId);
            pointer.IsActive = false;
            Version = evt.Version;
            UpdateDateTime = evt.CompletionDateTime;
        }

        private void Handle(ActivityStateUpdatedEvent evt)
        {
            var instance = GetInstance(evt.FlowNodeInstanceId);
            instance.UpdateState(evt.State, evt.UpdateDateTime);
            Version = evt.Version;
            UpdateDateTime = evt.UpdateDateTime;
        }

        private void Handle(IncomingTokenAddedEvent evt)
        {
            var pointer = GetExecutionPointer(evt.ExecutionPathId, evt.ExecutionPointerId);
            var tokens = BaseToken.DeserializeLst(evt.SerializedToken);
            foreach(var token in tokens)
            {
                pointer.Incoming.Add(token);
            }

            Version = evt.Version;
            UpdateDateTime = evt.CreateDateTime;
        }

        private void Handle(FlowNodeInstanceCompletedEvent evt)
        {
            var instance = GetInstance(evt.FlowNodeInstanceId);
            instance.State = FlowNodeStates.Complete;
            Version = evt.Version;
            UpdateDateTime = evt.ExecutionDateTime;
        }

        #endregion

        public static string GetStreamName(string id)
        {
            return $"planinstance-{id}";
        }

        public static string BuildId(string instanceId, string processId, string processFileId)
        {
            using (var sha256Hash = SHA256.Create())
            {
                var bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes($"{instanceId}{processId}{processFileId}"));
                var builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }

                return builder.ToString();
            }
        }
    }
}