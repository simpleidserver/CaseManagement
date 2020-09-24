using CaseManagement.BPMN.Domains;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CaseManagement.BPMN.Builders
{
    public class ProcessInstanceBuilder
    {
        private ProcessInstanceBuilder(string instanceId, string processId, string processFileId)
        {
            InstanceId = instanceId;
            ProcessId = processId;
            ProcessFileId = processFileId;
            Builders = new List<FlowNodeBuilder>();
            Messages = new List<Message>();
            ItemDefs = new List<ItemDefinition>();
            InterfaceBuilders = new List<BPMNInterfaceBuilder>();
        }

        protected string InstanceId { get; set; }
        protected string ProcessId { get; set; }
        protected string ProcessFileId { get; set; }
        protected ICollection<FlowNodeBuilder> Builders;
        protected ICollection<Message> Messages { get; set; }
        protected ICollection<ItemDefinition> ItemDefs { get; set; } 
        protected ICollection<BPMNInterfaceBuilder> InterfaceBuilders { get; set; }

        public static ProcessInstanceBuilder New(string instanceId, string processId, string processFileId)
        {
            return new ProcessInstanceBuilder(instanceId, processId, processFileId);
        }

        public ProcessInstanceBuilder AddMessage(string id, string name, string itemRef)
        {
            Messages.Add(new Message { Id = id, ItemRef = itemRef, Name = name });
            return this;
        }

        public ProcessInstanceBuilder AddItemDef(string id, ItemKinds itemKind, bool isCollection, string structuredRef)
        {
            ItemDefs.Add(new ItemDefinition { Id = id, ItemKind = itemKind, IsCollection = isCollection, StructureRef = structuredRef });
            return this;
        }

        public ProcessInstanceBuilder AddInterface(string id, string name, string implementationRef, Action<BPMNInterfaceBuilder> callback = null)
        {
            var builder = new BPMNInterfaceBuilder(id, name, implementationRef);
            if (callback != null)
            {
                callback(builder);
            }

            InterfaceBuilders.Add(builder);
            return this;
        }

        #region Build events

        public ProcessInstanceBuilder AddStartEvent(string id, string name, Action<StartEventBuilder> callback = null)
        {
            var startEvtBuilder = new StartEventBuilder(id, name);
            if (callback != null)
            {
                callback(startEvtBuilder);
            }

            Builders.Add(startEvtBuilder);
            return this;
        }

        #endregion

        #region Build tasks

        public ProcessInstanceBuilder AddEmptyTask(string id, string name, Action<EmptyTaskBuilder> callback = null)
        {
            var emptyTaskBuilder = new EmptyTaskBuilder(id, name);
            if (callback != null)
            {
                callback(emptyTaskBuilder);
            }

            Builders.Add(emptyTaskBuilder);
            return this;
        }

        public ProcessInstanceBuilder AddServiceTask(string id, string name, Action<ServiceTaskBuilder> callback = null)
        {
            var serviceTaskBuilder = new ServiceTaskBuilder(id, name);
            if (callback != null)
            {
                callback(serviceTaskBuilder);
            }

            Builders.Add(serviceTaskBuilder);
            return this;
        }

        #endregion

        public ProcessInstanceAggregate Build()
        {
            var elts = new List<BaseFlowNode>();
            var interfaces = new List<BPMNInterface>();
            foreach(var builder in Builders)
            {
                elts.Add(builder.Build());
            }

            foreach(var elt in elts)
            {
                elt.Outgoing = elts.Where(_ => _.Incoming.Contains(elt.Id)).Select(_ => _.Id).ToList();
            }

            foreach(var interfaceBuilder in InterfaceBuilders)
            {
                interfaces.Add(interfaceBuilder.Build());
            }

            var result = ProcessInstanceAggregate.New(InstanceId, ProcessId, ProcessFileId, elts, interfaces, Messages, ItemDefs);
            return result;
        }
    }
}
