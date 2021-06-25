using System;
using System.Collections.Generic;
using System.Linq;

namespace CaseManagement.BPMN.Domains
{
    public class ExecutionPointer : ICloneable
    {
        public ExecutionPointer()
        {
            Tokens = new List<MessageToken>();
            IsActive = true;
        }

        public string Id { get; set; }
        public string ExecutionPathId { get; set; }
        public string InstanceFlowNodeId { get; set; }
        public string FlowNodeId { get; set; }
        public bool IsActive { get; set; }
        public IEnumerable<MessageToken> Incoming
        {
            get
            {
                return Tokens.Where(t => t.Type == MessageTokenTypes.INCOMING);
            }
        }
        public IEnumerable<MessageToken> Outgoing
        {
            get
            {
                return Tokens.Where(t => t.Type == MessageTokenTypes.OUTGOING);
            }
        }
        public ICollection<MessageToken> Tokens { get; set; }

        public void AddIncoming(IEnumerable<MessageToken> tokens)
        {
            if (tokens == null)
            {
                return;
            }

            foreach(var token in tokens)
            {
                Tokens.Add(new MessageToken
                {
                    MessageContent = token.MessageContent,
                    Name = token.Name,
                    Type = MessageTokenTypes.INCOMING
                });
            }
        }

        public void AddOutgoing(IEnumerable<MessageToken> tokens)
        {
            if (tokens == null)
            {
                return;
            }

            foreach(var token in tokens)
            {
                Tokens.Add(new MessageToken
                {
                    MessageContent = token.MessageContent,
                    Name = token.Name,
                    Type = MessageTokenTypes.OUTGOING
                });
            }
        }

        public object Clone()
        {
            return new ExecutionPointer
            {
                Id = Id,
                ExecutionPathId = ExecutionPathId,
                FlowNodeId = FlowNodeId,
                InstanceFlowNodeId = InstanceFlowNodeId,
                IsActive = IsActive,
                Tokens = Tokens.Select(_ => (MessageToken)_.Clone()).ToList()
            };
        }
    }
}
