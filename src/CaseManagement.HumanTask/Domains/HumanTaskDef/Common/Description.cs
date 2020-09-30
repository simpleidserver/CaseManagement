using System;

namespace CaseManagement.HumanTask.Domains
{
    public class Description : Text
    {
        public string ContentType { get; set; }

        public override object Clone()
        {
            return new Description
            {
                Language = Language,
                Value = Value,
                ContentType = ContentType
            };
        }
    }
}
