using System;
using System.Collections.Generic;
using System.Linq;

namespace CaseManagement.CMMN.Domains
{
    public class FormInstanceAggregate : ICloneable
    {
        public FormInstanceAggregate(string id, string formId)
        {
            Id = id;
            FormId = formId;
            Content = new List<FormInstanceElement>();
            Titles = new List<Translation>();
            Status = FormInstanceStatus.Create;
        }

        public string Id { get; set; }
        public string FormId { get; set; }
        public ICollection<Translation> Titles { get; set; }
        public ICollection<FormInstanceElement> Content { get; set; }
        public FormInstanceStatus Status { get; set; }
        public DateTime CreateDateTime { get; set; }
        public DateTime UpdateDateTime { get; set; }
        public string RoleId { get; set; }

        public void AddElement(FormInstanceElement elt)
        {
            Content.Add(elt);
        }

        public void UpdateElement(string key, string value)
        {
            Content.First(c => c.FormElementId == key).Value = value;
        }

        public static FormInstanceAggregate New(string id, string formId)
        {
            return new FormInstanceAggregate(id, formId);
        }

        public object Clone()
        {
            return new FormInstanceAggregate(Id, FormId)
            {
                Content = Content.Select(c => (FormInstanceElement)c.Clone()).ToList(),
                RoleId = RoleId,
                Status = Status,
                CreateDateTime = CreateDateTime,
                UpdateDateTime = UpdateDateTime,
                Titles = Titles.Select(n => (Translation)n.Clone()).ToList()
            };
        }
    }
}