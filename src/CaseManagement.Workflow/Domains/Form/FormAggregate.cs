using System.Collections.Generic;

namespace CaseManagement.Workflow.Domains
{
    public class FormAggregate
    {
        public FormAggregate()
        {
            Titles = new List<Translation>();
            Elements = new List<FormElement>();
        }

        public string Id { get; set; }
        public ICollection<Translation> Titles { get; set; }
        public ICollection<FormElement> Elements { get; set; }
    }
}
