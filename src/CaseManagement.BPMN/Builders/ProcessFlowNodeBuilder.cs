using CaseManagement.BPMN.Domains;
using System;
using System.Collections.Generic;

namespace CaseManagement.BPMN.Builders
{
    #region Base classes

    public abstract class EltBuilder
    {
        public EltBuilder(string id)
        {
            Id = id;
        }

        internal string Id { get; set; }
    }

    public abstract class FlowEltBuilder : EltBuilder
    {
        public FlowEltBuilder(string id, string name) : base(id)
        {
            Name = name;
        }

        internal string Name { get; set; }
    }

    public abstract class FlowNodeBuilder : FlowEltBuilder
    {
        public FlowNodeBuilder(string id, string name) : base(id, name)
        {
            Incoming = new List<string>();
            Outgoing = new List<string>();
        }

        internal ICollection<string> Incoming { get; set; }
        internal ICollection<string> Outgoing { get; set; }

        public FlowNodeBuilder AddIncoming(string incoming)
        {
            Incoming.Add(incoming);
            return this;
        }

        public FlowNodeBuilder AddOutgoing(string outgoing)
        {
            Outgoing.Add(outgoing);
            return this;
        }

        public abstract BaseFlowNode Build();

        protected void FeedFlowNode(BaseFlowNode node)
        {
            node.Id = Id;
            node.TechnicalId = BaseFlowNode.BuildTechnicalId(Id, 0);
            node.Name = Name;
            node.Incoming = Incoming;
            node.Outgoing = Outgoing;
        }
    }

    #endregion

    #region Tasks

    public class EmptyTaskBuilder : FlowNodeBuilder
    {
        public EmptyTaskBuilder(string id, string name) : base(id, name) { }

        public override BaseFlowNode Build()
        {
            var result = new EmptyTask();
            FeedFlowNode(result);
            return result;
        }
    }

    #endregion

    #region Events

    public abstract class CatchEventBuilder : FlowNodeBuilder
    {
        public CatchEventBuilder(string id, string name) : base(id, name) 
        {
            EvtDefinitions = new List<BaseEventDefinition>();
        }

        public ICollection<BaseEventDefinition> EvtDefinitions { get; set; }

        public void AddMessageEvtDef(string id, Action<MessageEvtDefBuilder> callback)
        {
            var builder = new MessageEvtDefBuilder(id);
            callback(builder);
            EvtDefinitions.Add(builder.Build());
        }

        protected void FeedCatchEvt(BaseCatchEvent evt)
        {
            FeedFlowNode(evt);
            evt.EventDefinitions = EvtDefinitions;
        }
    }

    public class StartEventBuilder : CatchEventBuilder
    {
        public StartEventBuilder(string id, string name) : base(id, name)
        {
        }

        public override BaseFlowNode Build()
        {
            var result = new StartEvent();
            FeedCatchEvt(result);
            return result;
        }
    }

    #endregion

    #region Evt Definitions

    public class MessageEvtDefBuilder
    {
        private readonly string _id;
        private Message _messageRef;

        public MessageEvtDefBuilder(string id)
        {
            _id = id;
        }

        public void SetMessageRef(Message messageRef)
        {
            _messageRef = messageRef;
        }

        public MessageEventDefinition Build()
        {
            return new MessageEventDefinition
            {
                Id = _id,
                MessageRef = _messageRef
            };
        }
    }

    #endregion
}