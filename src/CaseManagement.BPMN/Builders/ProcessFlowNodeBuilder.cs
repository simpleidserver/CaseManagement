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
        public FlowNodeBuilder(string id, string name) : base(id, name) { }

        public abstract BaseFlowNode Build();

        protected void FeedFlowNode(BaseFlowNode node)
        {
            node.EltId = Id;
            node.Name = Name;
        }
    }

    public abstract class ActivityNodeBuilder : FlowNodeBuilder
    {
        public ActivityNodeBuilder(string id, string name) : base(id, name)
        {
            StartQuantity = 1;
            BoundaryEvtRefs = new List<string>();
        }

        public ActivityNodeBuilder SetStartQuantity(int startQuantity)
        {
            StartQuantity = startQuantity;
            return this;
        }

        internal List<string> BoundaryEvtRefs { get; set; }
        internal int StartQuantity { get; set; }

        public ActivityNodeBuilder AddBoundaryEventRef(string boundaryEvtRef)
        {
            BoundaryEvtRefs.Add(boundaryEvtRef);
            return this;
        }

        protected void FeedActivityNode(BaseActivity node)
        {
            FeedFlowNode(node);
            node.StartQuantity = StartQuantity;
            node.BoundaryEvtRefs = BoundaryEvtRefs;
        }
    }

    #endregion

    #region Tasks

    public class ServiceTaskBuilder : ActivityNodeBuilder
    {
        public ServiceTaskBuilder(string id, string name) : base(id, name) { }

        internal string OperationRef { get; set; }
        internal string Implementation { get; set; }
        internal string DelegateId { get; set; }

        public void SetOperationRef(string operationRef)
        {
            OperationRef = operationRef;
        }

        public void SetDelegate(string delegateId)
        {
            Implementation = BPMNConstants.ServiceTaskImplementations.CALLBACK;
            DelegateId = delegateId;
        }

        public override BaseFlowNode Build()
        {
            var result = new ServiceTask
            {
                OperationRef = OperationRef,
                Implementation = Implementation,
                DelegateId = DelegateId
            };
            FeedActivityNode(result);
            return result;
        }
    }

    public class EmptyTaskBuilder : ActivityNodeBuilder
    {
        public EmptyTaskBuilder(string id, string name) : base(id, name) { }

        public override BaseFlowNode Build()
        {
            var result = new EmptyTask();
            FeedActivityNode(result);
            return result;
        }
    }

    public class UserTaskBuilder : ActivityNodeBuilder
    {
        private string _implementation;
        private string _humanTaskName;
        private Dictionary<string, string> _inputParameters;

        public UserTaskBuilder(string id, string name) : base(id, name) { }

        public UserTaskBuilder SetImplementation(string implementation)
        {
            _implementation = implementation;
            return this;
        }

        public UserTaskBuilder SetWsHumanTask(string humanTaskName, Dictionary<string, string> inputParameters = null)
        {
            _humanTaskName = humanTaskName;
            _inputParameters = inputParameters;
            return SetImplementation(BPMNConstants.UserTaskImplementations.WSHUMANTASK);
        }

        public override BaseFlowNode Build()
        {
            var result = new UserTask
            {
                Implementation = _implementation,
                HumanTaskName = _humanTaskName,
                InputParameters = _inputParameters
            };
            FeedActivityNode(result);
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

    public class EndEventBuilder : CatchEventBuilder
    {
        public EndEventBuilder(string id, string name) : base(id, name)
        {
        }

        public override BaseFlowNode Build()
        {
            var result = new EndEvent();
            FeedCatchEvt(result);
            return result;
        }
    }

    public class BoundaryEventBuilder : CatchEventBuilder
    {
        public BoundaryEventBuilder(string id, string name) : base(id, name) { }

        public override BaseFlowNode Build()
        {
            var result = new BoundaryEvent();
            FeedCatchEvt(result);
            return result;
        }
    }

    #endregion

    #region Evt Definitions

    public class MessageEvtDefBuilder
    {
        private readonly string _id;
        private string _messageRef;

        public MessageEvtDefBuilder(string id)
        {
            _id = id;
        }

        public void SetMessageRef(string messageRef)
        {
            _messageRef = messageRef;
        }

        public MessageEventDefinition Build()
        {
            return new MessageEventDefinition
            {
                EltId = _id,
                MessageRef = _messageRef
            };
        }
    }

    #endregion
}