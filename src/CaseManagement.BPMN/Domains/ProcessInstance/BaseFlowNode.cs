using CaseManagement.Common.Processors;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace CaseManagement.BPMN.Domains
{
    public abstract class BaseFlowNode : BaseFlowElement, IBranchNode, ICloneable
    {
        public BaseFlowNode()
        {
            Incoming = new List<string>();
            Outgoing = new List<string>();
            TransitionHistories = new ConcurrentBag<FlowNodeTransitionHistory>();
        }

        public string TechnicalId { get; set; }
        public int NbOccurrence { get; set; }
        public BPMNTransitions? LastTransition { get; set; }
        public ConcurrentBag<FlowNodeTransitionHistory> TransitionHistories { get; set; }
        public ICollection<string> Incoming { get; set; }
        public ICollection<string> Outgoing { get; set; }

        public bool IsLeaf()
        {
            return this is StartEvent;
        }

        public void MakeTransition(BPMNTransitions transition, DateTime executionDateTime)
        {
            TransitionHistories.Add(new FlowNodeTransitionHistory
            {
                ExecutionDateTime = executionDateTime,
                Transition = transition
            });
            LastTransition = transition;
            UpdateState(transition);
        }

        protected void FeedFlowNode(BaseFlowNode node)
        {
            FeedFlowElt(node);
            node.TechnicalId = TechnicalId;
            node.NbOccurrence = NbOccurrence;
            node.LastTransition = LastTransition;
            node.TransitionHistories = new ConcurrentBag<FlowNodeTransitionHistory>(TransitionHistories.Select(_ => new FlowNodeTransitionHistory
            {
                ExecutionDateTime = _.ExecutionDateTime,
                Transition = _.Transition
            }));
            node.Incoming = Incoming.ToList();
            node.Outgoing = Outgoing.ToList();
        }

        protected virtual void UpdateState(BPMNTransitions transition) { }

        public abstract object Clone();

        public static string BuildTechnicalId(string id, int nbOccurrence)
        {
            using (var sha256Hash = SHA256.Create())
            {
                var bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes($"{id}{nbOccurrence}"));
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
