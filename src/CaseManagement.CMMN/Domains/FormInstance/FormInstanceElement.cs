using System;
using System.Collections.Generic;
using System.Linq;

namespace CaseManagement.CMMN.Domains
{
    public class FormInstanceElement : ICloneable
    {
        public FormInstanceElement()
        {
            Names = new List<Translation>();
            Descriptions = new List<Translation>();
        }

        public string FormElementId { get; set; }
        public string Value { get; set; }
        public ICollection<Translation> Names { get; set; }
        public ICollection<Translation> Descriptions { get; set; }
        public FormElementTypes Type { get; set; }
        public bool IsRequired { get; set; }

        public object Clone()
        {
            return new FormInstanceElement
            {
                FormElementId = FormElementId,
                Value = Value,
                IsRequired = IsRequired,
                Type = Type,
                Descriptions = Descriptions.Select(d => (Translation)d.Clone()).ToList(),
                Names = Names.Select(d => (Translation)d.Clone()).ToList()
            };
        }
    }
}
