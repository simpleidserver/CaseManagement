using CaseManagement.CMMN.Infrastructures;
using System.Collections.Generic;

namespace CaseManagement.CMMN.Domains
{
    public class FormAggregate : BaseAggregate
    {
        public FormAggregate()
        {
            Titles = new List<Translation>();
            Elements = new List<FormElement>();
        }

        public ICollection<Translation> Titles { get; set; }
        public ICollection<FormElement> Elements { get; set; }

        public override object Clone()
        {
            return new FormAggregate
            {
                Id =  Id
            };
        }

        public override void Handle(object obj)
        {

        }
    }
}
