using CaseManagement.Common.Domains;
using CaseManagement.Common.Exceptions;
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
            Elements = new ConcurrentBag<BaseFlowNode>();
        }

        public string ProcessFileId { get; set; }
        public string ProcessId { get; set; }
        public DateTime CreateDateTime { get; set; }
        public DateTime UpdateDateTime { get; set; }
        public ConcurrentBag<BaseFlowNode> Elements { get; set; }

        #region Getters

        public bool IsIncomingSatisfied(BaseFlowNode node)
        {
            // Documentation : Page 151.
            // when a token arrives from one of the Paths, the Activity will be instantiated.
            // It will not wait for the arrival of tokens from the other paths.
            if (node.Incoming == null || !node.Incoming.Any())
            {
                return true;
            }

            var dependencies = node.Incoming.Select(_ => GetChild(BaseFlowNode.BuildTechnicalId(_, node.NbOccurrence)));
            if (!dependencies.Any(_ => _.LastTransition == BPMNTransitions.COMPLETE))
            {
                return false;
            }

            return true;
        }

        public BaseFlowNode GetChild(string technicalId)
        {
            return Elements.FirstOrDefault(_ => _.TechnicalId == technicalId);
        }

        #endregion

        #region Operations

        public void MakeTransition(BaseFlowNode node, BPMNTransitions transition)
        {
            MakeTransition(node.TechnicalId, transition);
        }

        public void MakeTransition(string technicalId, BPMNTransitions transition)
        {
            var evt = new FlowNodeTransitionRaisedEvent(Guid.NewGuid().ToString(), AggregateId, Version + 1, technicalId, transition, DateTime.UtcNow);
            Handle(evt);
            DomainEvents.Add(evt);
        }

        public void AddNode(BaseFlowNode node)
        {
            var evt = new FlowNodeCreatedEvent(Guid.NewGuid().ToString(), AggregateId, Version + 1, node.GetType().FullName, JsonConvert.SerializeObject(node),  DateTime.UtcNow);
            Handle(evt);
            DomainEvents.Add(evt);
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

        public static ProcessInstanceAggregate New(string processFileId, string processId, ICollection<BaseFlowNode> elements)
        {
            var result = new ProcessInstanceAggregate();
            var evt = new ProcessInstanceCreatedEvent(Guid.NewGuid().ToString(), BuildId(processFileId, processId), 0, processFileId, processId, DateTime.UtcNow);
            result.Handle(evt);
            foreach (var elt in elements)
            {
                result.AddNode(elt);
            }

            result.DomainEvents.Add(evt);
            return result;
        }

        public override object Clone()
        {
            return new ProcessInstanceAggregate
            {
                AggregateId = AggregateId,
                ProcessFileId = ProcessFileId,
                ProcessId = ProcessId,
                Version = Version,
                CreateDateTime = CreateDateTime,
                UpdateDateTime = UpdateDateTime,
                Elements = new ConcurrentBag<BaseFlowNode>(Elements.Select(e => (BaseFlowNode)e.Clone()))
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
        }

        private void Handle(FlowNodeCreatedEvent evt)
        {
            var assm = typeof(ProcessInstanceAggregate).Assembly;
            var type = assm.GetType(evt.NodeType);
            var elt = (BaseFlowNode)JsonConvert.DeserializeObject(evt.SerializedContent, type);
            Elements.Add(elt);
            Version = evt.Version;
            UpdateDateTime = evt.UpdateDateTime;
        }

        private void Handle(FlowNodeTransitionRaisedEvent evt)
        {
            var child = GetChild(evt.TechnicalId);
            if (child == null)
            {
                throw new AggregateValidationException(new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>( "validation",  $"unknown child '{evt.TechnicalId}'")
                });
            }

            child.MakeTransition(evt.Transition, evt.ExecutionDateTime);
            UpdateDateTime = evt.ExecutionDateTime;
            Version = evt.Version;
        }

        #endregion

        public static string GetStreamName(string id)
        {
            return $"planinstance-{id}";
        }

        public static string BuildId(string processFileId, string processId)
        {
            using (var sha256Hash = SHA256.Create())
            {
                var bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes($"{processFileId}{processId}"));
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