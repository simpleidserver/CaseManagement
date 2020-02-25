using System;
using System.Collections.Generic;
using System.Linq;

namespace CaseManagement.CMMN.Domains
{
    public class FormElement : ICloneable
    {
        public FormElement()
        {
            Names = new List<Translation>();
            Descriptions = new List<Translation>();
        }

        public string Id { get; set; }
        public ICollection<Translation> Names { get; set; }
        public ICollection<Translation> Descriptions { get; set; }
        public FormElementTypes Type { get; set; }
        public bool IsRequired { get; set; }

        public object Clone()
        {
            return new FormElement
            {
                Id = Id,
                Names = Names.Select(n => n.Clone() as Translation).ToList(),
                Descriptions = Descriptions.Select(n => n.Clone() as Translation).ToList(),
                Type = Type,
                IsRequired = IsRequired
            };
        }
    }
}
