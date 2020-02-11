using CaseManagement.CMMN.CasePlanInstance.Exceptions;
using CaseManagement.CMMN.Domains.FormInstance.Events;
using CaseManagement.CMMN.Infrastructures;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace CaseManagement.CMMN.Domains
{
    public class FormInstanceAggregate : BaseAggregate
    {
        public FormInstanceAggregate()
        {
            Content = new List<FormInstanceElement>();
            Status = FormInstanceStatus.Created;
        }

        public FormInstanceAggregate(string id, string formId, string casePlanInstanceId, string caseElementInstanceId) : this()
        {
            Id = id;
            FormId = formId;
            CasePlanInstanceId = casePlanInstanceId;
            CaseElementInstanceId = caseElementInstanceId;
        }

        public string FormId { get; set; }
        public string CasePlanInstanceId { get; set; }
        public string CaseElementInstanceId { get; set; }
        public ICollection<FormInstanceElement> Content { get; set; }
        public FormInstanceStatus Status { get; set; }
        public DateTime CreateDateTime { get; set; }
        public DateTime UpdateDateTime { get; set; }
        public string PerformerRole { get; set; }

        public void Submit(IEnumerable<string> performerRoles, FormAggregate form, JObject jObj)
        {
            lock(DomainEvents)
            {
                if (!string.IsNullOrWhiteSpace(PerformerRole) && !performerRoles.Any(r => r == PerformerRole))
                {
                    throw new UnauthorizedCaseWorkerException(string.Empty, CasePlanInstanceId, CaseElementInstanceId);
                }

                var content = CheckConfirmForm(form, jObj);
                var evt = new FormInstanceSubmittedEvent(Guid.NewGuid().ToString(), Id, Version + 1, content, DateTime.UtcNow);
                Handle(evt);
                DomainEvents.Add(evt);
            }
        }

        public static string GetStreamName(string id)
        {
            return $"form-instance-{id}";
        }

        public static FormInstanceAggregate New(List<DomainEvent> evts)
        {
            var result = new FormInstanceAggregate();
            foreach(var evt in evts)
            {
                result.Handle(evt);
            }

            return result;
        }

        public static FormInstanceAggregate New(string formId, string casePlanInstanceId, string caseElementInstanceId, string performerRole)
        {
            var result = new FormInstanceAggregate();
            lock (result.DomainEvents)
            {
                var id = BuildFormInstanceIdentifier(casePlanInstanceId, caseElementInstanceId);
                var evt = new FormInstanceAddedEvent(Guid.NewGuid().ToString(), id, 0, formId, DateTime.UtcNow, performerRole, caseElementInstanceId, casePlanInstanceId);
                result.Handle(evt);
                result.DomainEvents.Add(evt);
                return result;
            }
        }

        public static string BuildFormInstanceIdentifier(string casePlanInstanceId, string caseElementInstanceId)
        {
            using (var sha256Hash = SHA256.Create())
            {
                var bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes($"{casePlanInstanceId}{caseElementInstanceId}"));
                var builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }

                return builder.ToString();
            }
        }

        public override bool Equals(object obj)
        {
            var target = obj as FormInstanceAggregate;
            if (target == null)
            {
                return false;
            }

            return target.GetHashCode() == this.GetHashCode();
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public override void Handle(object obj)
        {
            if (obj is FormInstanceAddedEvent)
            {
                Handle((FormInstanceAddedEvent)obj);
            }

            if (obj is FormInstanceSubmittedEvent)
            {
                Handle((FormInstanceSubmittedEvent)obj);
            }
        }

        private void Handle(FormInstanceAddedEvent evt)
        {
            Id = evt.AggregateId;
            FormId = evt.FormId;
            CasePlanInstanceId = evt.CasePlanInstanceId;
            Status = FormInstanceStatus.Created;
            CreateDateTime = evt.CreateDateTime;
            PerformerRole = evt.PerformerRole;
            CaseElementInstanceId = evt.CaseElementInstanceId;
        }

        private void Handle(FormInstanceSubmittedEvent evt)
        {
            foreach(var kvp in evt.Content)
            {
                Content.Add(new FormInstanceElement { FormElementId = kvp.Key, Value = kvp.Value });
            }

            UpdateDateTime = evt.UpdateDateTime;
            Status = FormInstanceStatus.Completed;
            Version = evt.Version;
        }

        public override object Clone()
        {
            return new FormInstanceAggregate(Id, FormId, CasePlanInstanceId, CaseElementInstanceId)
            {
                Content = Content == null ? new List<FormInstanceElement>() : Content.Select(c => (FormInstanceElement)c.Clone()).ToList(),
                PerformerRole = PerformerRole,
                Version = Version,
                Status = Status,
                CreateDateTime = CreateDateTime,
                UpdateDateTime = UpdateDateTime,
                CasePlanInstanceId = CasePlanInstanceId,
                Id =  Id,
                FormId = FormId
            };
        }
        
        private static Dictionary<string, string> CheckConfirmForm(FormAggregate form, JObject content)
        {
            var result = new Dictionary<string, string>();
            var errors = new Dictionary<string, string>();
            foreach (var elt in form.Elements)
            {
                string value = string.Empty;
                if (elt.IsRequired && (!content.ContainsKey(elt.Id) || string.IsNullOrWhiteSpace((value = content[elt.Id].ToString()))))
                {
                    errors.Add("validation_error", $"field {elt.Id} is required");
                }

                switch (elt.Type)
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

                result.Add(elt.Id, value);
            }

            if (errors.Any())
            {
                throw new AggregateValidationException(errors);
            }

            return result;
        }
    }
}