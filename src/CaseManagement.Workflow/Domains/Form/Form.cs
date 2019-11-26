using System.Collections.Generic;

namespace CaseManagement.Workflow.Domains
{
    public class Form
    {
        public string Id { get; set; }
        public ICollection<FormElementTranslation> Titles { get; set; }
        public ICollection<FormElement> Elements { get; set; }
    }
}
