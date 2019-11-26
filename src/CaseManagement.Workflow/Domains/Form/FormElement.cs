using System.Collections.Generic;

namespace CaseManagement.Workflow.Domains
{
    public class FormElement
    {
        public FormElement()
        {
            Names = new List<FormElementTranslation>();
            Descriptions = new List<FormElementTranslation>();
        }

        public string Id { get; set; }
        public ICollection<FormElementTranslation> Names { get; set; }
        public ICollection<FormElementTranslation> Descriptions { get; set; }
        public FormElementTypes Type { get; set; }
        public bool IsRequired { get; set; }
    }
}
