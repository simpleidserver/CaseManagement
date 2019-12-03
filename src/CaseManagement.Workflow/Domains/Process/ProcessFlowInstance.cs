using CaseManagement.Workflow.Domains.Events;
using CaseManagement.Workflow.Domains.Process.Exceptions;
using CaseManagement.Workflow.Infrastructure;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CaseManagement.Workflow.Domains
{
    public class ProcessFlowInstance : BaseAggregate
    {
        public ProcessFlowInstance() : base()
        {
            ExecutionContext = new ProcessFlowInstanceExecutionContext();
            ExecutionSteps = new List<ProcessFlowInstanceExecutionStep>();
            Elements = new List<ProcessFlowInstanceElement>();
            Connectors = new List<ProcessFlowConnector>();
        }

        public DateTime CreateDateTime { get; set; }
        public string ProcessFlowTemplateId { get; set; }
        public string ProcessFlowName { get; set; }

        public ProcessFlowInstanceStatus? Status { get; set; }
        public ProcessFlowInstanceExecutionContext ExecutionContext { get; set; }
        public ICollection<ProcessFlowInstanceExecutionStep> ExecutionSteps { get; set; }
        public ICollection<ProcessFlowInstanceElement> Elements { get; set; }
        public ICollection<ProcessFlowConnector> Connectors { get; set; }

        #region Accessors

        public bool IsFinished()
        {
            var startElts = GetStartElements();
            var result = true;
            foreach(var startElt in startElts)
            {
                if (!IsFinished(startElt))
                {
                    result = false;
                }
            }

            return result;
        }

        public ProcessFlowInstanceElementForm GetFormInstance(string eltId)
        {
            var elt = Elements.FirstOrDefault(e => e.Id == eltId);
            if (elt == null)
            {
                return null;
            }

            return elt.FormInstance;
        }

        public bool IsElementComplete(string eltId)
        {
            var elt = Elements.FirstOrDefault(e => e.Id == eltId);
            if (elt == null)
            {
                return false;
            }

            return elt.Status == ProcessFlowInstanceElementStatus.Finished;
        }

        public bool CanStartElement(string eltId)
        {
            var elt = Elements.FirstOrDefault(e => e.Id == eltId);
            if (elt == null)
            {
                return false;
            }

            var previousElts = PreviousElements(eltId);
            if (previousElts.Any(p => !IsElementComplete(p.Id)))
            {
                return false;
            }

            return true;
        }

        public ICollection<ProcessFlowInstanceElement> NextElements(string nodeId)
        {
            return Connectors.Where(c => c.SourceId == nodeId).SelectMany(c => Elements.Where(e => e.Id == c.TargetId)).ToList();
        }

        public ICollection<ProcessFlowInstanceElement> PreviousElements(string nodeId)
        {
            return Connectors.Where(c => c.TargetId == nodeId).SelectMany(c => Elements.Where(e => e.Id == c.SourceId)).ToList();
        }

        public ICollection<ProcessFlowInstanceElement> GetStartElements()
        {
            return Elements.Where(e => Connectors.All(c => c.TargetId != e.Id)).ToList();
        }

        public bool ContainsVariable(string key)
        {
            return ExecutionContext.ContainsVariable(key);
        }

        public string GetVariable(string key)
        {
            return ExecutionContext.GetVariable(key);
        }

        public int GetNumberVariable(string key)
        {
            return int.Parse(GetVariable(key));
        }

        #endregion

        #region Commands

        public void Launch()
        {
            if (Status == ProcessFlowInstanceStatus.Started)
            {
                throw new ProcessFlowInstanceDomainException
                {
                    Errors = new Dictionary<string, string>
                    {
                        { "validation_error", "process instance is already launched" }
                    }
                };
            }
            
            var evt = new ProcessFlowInstanceLaunchedEvent(Id);
            DomainEvents.Add(evt);
            Handle(evt);
        }

        public void LaunchElement(string eltId)
        {
            var evt = new ProcessFlowElementLaunchedEvent(Id, eltId, DateTime.UtcNow);
            DomainEvents.Add(evt);
            Handle(evt);
        }

        public void ConfirmForm(string eltId, Form form, JObject content)
        {
            var formInstance = CheckConfirmForm(form, content);
            formInstance.Status = Process.ProcessFlowInstanceElementFormStatus.Complete;
            var evt = new ProcessFlowInstanceFormConfirmedEvent(Id, eltId, formInstance);
            DomainEvents.Add(evt);
            Handle(evt);
        }

        public void StartElement(ProcessFlowInstanceElement elt)
        {
            StartElement(elt.Id);
        }

        public void StartElement(string eltId)
        {
            var elt = Elements.FirstOrDefault(e => e.Id == eltId);
            if (elt == null)
            {
                throw new ProcessFlowInstanceDomainException
                {
                    Errors = new Dictionary<string, string>
                    {
                        { "validation_error", "process instance element doesn't exist" }
                    }
                };
            }
            
            var evt = new ProcessFlowElementStartedEvent(Id, eltId, DateTime.UtcNow);
            DomainEvents.Add(evt);
            Handle(evt);
        }

        public void CompleteElement(ProcessFlowInstanceElement elt)
        {
            CompleteElement(elt.Id);
        }

        public void CompleteElement(string eltId)
        {
            var elt = Elements.FirstOrDefault(e => e.Id == eltId);
            if (elt == null)
            {
                throw new ProcessFlowInstanceDomainException
                {
                    Errors = new Dictionary<string, string>
                    {
                        { "validation_error", "process instance element doesn't exist" }
                    }
                };
            }

            var evt = new ProcessFlowElementCompletedEvent(Id, eltId, DateTime.UtcNow);
            DomainEvents.Add(evt);
            Handle(evt);
        }

        public void Complete()
        {
            var evt = new ProcessFlowInstanceCompletedEvent(Id);
            DomainEvents.Add(evt);
            Handle(evt);
        }

        public void SetVariable(string key, int value)
        {
            SetVariable(key, value.ToString());
        }

        public void SetVariable(string key, string value)
        {
            var evt = new ProcessFlowInstanceVariableAddedEvent(Id, key, value);
            DomainEvents.Add(evt);
            Handle(evt);
        }

        #endregion

        public static ProcessFlowInstance New(string processFlowTemplateId, string processFlowName, ICollection<ProcessFlowInstanceElement> elements, ICollection<ProcessFlowConnector> connectors)
        {
            var result = new ProcessFlowInstance();
            var evt = new ProcessFlowInstanceCreatedEvent(Guid.NewGuid().ToString(), processFlowTemplateId, processFlowName, DateTime.UtcNow, elements, connectors);
            result.Handle(evt);
            result.DomainEvents.Add(evt);
            return result;
        }

        public static ProcessFlowInstance New(ICollection<DomainEvent> evts)
        {
            var result = new ProcessFlowInstance();
            foreach(var evt in evts)
            {
                result.Handle(evt);
            }

            return result;
        }

        public override void Handle(object obj)
        {
            if (obj is ProcessFlowInstanceCreatedEvent)
            {
                Handle((ProcessFlowInstanceCreatedEvent)obj);
            }

            if (obj is ProcessFlowInstanceLaunchedEvent)
            {
                Handle((ProcessFlowInstanceLaunchedEvent)obj);
            }

            if (obj is ProcessFlowInstanceFormConfirmedEvent)
            {
                Handle((ProcessFlowInstanceFormConfirmedEvent)obj);
            }

            if (obj is ProcessFlowInstanceCompletedEvent)
            {
                Handle((ProcessFlowInstanceCompletedEvent)obj);
            }

            if (obj is ProcessFlowElementStartedEvent)
            {
                Handle((ProcessFlowElementStartedEvent)obj);
            }

            if (obj is ProcessFlowElementCompletedEvent)
            {
                Handle((ProcessFlowElementCompletedEvent)obj);
            }

            if (obj is ProcessFlowInstanceVariableAddedEvent)
            {
                Handle((ProcessFlowInstanceVariableAddedEvent)obj);
            }

            if (obj is ProcessFlowElementLaunchedEvent)
            {
                Handle((ProcessFlowElementLaunchedEvent)obj);
            }
        }

        public void Handle(ProcessFlowInstanceCreatedEvent evt)
        {
            Id = evt.Id;
            ProcessFlowTemplateId = evt.ProcessFlowTemplateId;
            ProcessFlowName = evt.ProcessFlowName;
            CreateDateTime = evt.CreateDateTime;
            Elements = evt.Elements;
            Connectors = evt.Connectors;
        }

        public void Handle(ProcessFlowInstanceLaunchedEvent evt)
        {
            Status = ProcessFlowInstanceStatus.Started;
        }

        public void Handle(ProcessFlowInstanceCompletedEvent evt)
        {
            Status = ProcessFlowInstanceStatus.Completed;
        }

        public void Handle(ProcessFlowInstanceFormConfirmedEvent evt)
        {
            var elt = Elements.FirstOrDefault(e => e.Id == evt.ElementId);
            elt.FormInstance = evt.FormInstance;
        }

        public void Handle(ProcessFlowElementLaunchedEvent evt) { }
        
        public void Handle(ProcessFlowElementStartedEvent evt)
        {
            var elt = Elements.FirstOrDefault(e => e.Id == evt.ProcessFlowInstanceElementId);
            elt.Status = ProcessFlowInstanceElementStatus.Launched;
            var existingElt = Elements.First(e => e.Id == evt.ProcessFlowInstanceElementId);
            ExecutionSteps.Add(new ProcessFlowInstanceExecutionStep(existingElt.Id, existingElt.Name, evt.StartDateTime));
        }

        public void Handle(ProcessFlowElementCompletedEvent evt)
        {
            var elt = Elements.FirstOrDefault(e => e.Id == evt.ProcessFlowInstanceElementId);
            elt.Status = ProcessFlowInstanceElementStatus.Finished;
            var executionStep = ExecutionSteps.First(e => e.ElementId == elt.Id && e.EndDateTime == null);
            executionStep.EndDateTime = evt.CompletedDateTime;
        }

        public void Handle(ProcessFlowInstanceVariableAddedEvent evt)
        {
            ExecutionContext.SetVariable(evt.Key, evt.Value);
        }

        public override object Clone()
        {
            return new ProcessFlowInstance
            {
                Id = Id,
                CreateDateTime = CreateDateTime,
                ProcessFlowTemplateId = ProcessFlowTemplateId,
                ProcessFlowName = ProcessFlowName,
                Version = Version,
                Status = Status,
                ExecutionSteps = ExecutionSteps.Select(c => (ProcessFlowInstanceExecutionStep)c.Clone()).ToList(),
                Connectors = Connectors.Select(c => (ProcessFlowConnector)c.Clone()).ToList(),
                Elements = Elements.Select(e => (ProcessFlowInstanceElement)e.Clone()).ToList(),
                ExecutionContext = (ProcessFlowInstanceExecutionContext)ExecutionContext.Clone()
            };
        }

        public string GetStreamName()
        {
            return GetStreamName(Id);
        }

        public static string GetStreamName(string id)
        {
            return $"ProcessFlowInstance_{id}";
        }

        private static ProcessFlowInstanceElementForm CheckConfirmForm(Form form, JObject content)
        {
            var errors = new Dictionary<string, string>();
            var result = ProcessFlowInstanceElementForm.New(form.Id);
            foreach (var elt in form.Elements)
            {
                string value = string.Empty;
                if (elt.IsRequired && (!content.ContainsKey(elt.Id) || string.IsNullOrWhiteSpace((value = content[elt.Id].ToString())) ))
                {
                    errors.Add("validation_error", $"field {elt.Id} is required");
                }
                
                switch(elt.Type)
                {
                    case FormElementTypes.INT:
                        int i;
                        if (!int.TryParse(value, out i))
                        {
                            errors.Add("validation_error", $"field {elt.Id} is not an integer");
                        }
                        break;
                    case FormElementTypes.BOOL:
                        bool b;
                        if (!bool.TryParse(value, out b))
                        {
                            errors.Add("validation_error", $"field {elt.Id} is not a boolean");
                        }
                        break;
                }

                result.Content.Add(new ProcessFlowInstanceElementFormElement
                {
                    FormElementId = elt.Id,
                    Value = value
                });
            }

            if (errors.Any())
            {
                throw new ProcessFlowInstanceDomainException
                {
                    Errors = errors
                };
            }

            return result;
        }

        private bool IsFinished(ProcessFlowInstanceElement elt)
        {
            if (elt.Status != ProcessFlowInstanceElementStatus.Finished)
            {
                return false;
            }

            var nextElts = NextElements(elt.Id);
            var result = true;
            foreach(var nextElt in nextElts)
            {
                if (!IsFinished(nextElt))
                {
                    result = false;
                }
            }

            return result;
        }
    }
}