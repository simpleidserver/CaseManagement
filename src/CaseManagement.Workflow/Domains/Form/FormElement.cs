using System.Collections.Generic;

namespace CaseManagement.Workflow.Domains
{
    public class FormElement
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
    }
}
