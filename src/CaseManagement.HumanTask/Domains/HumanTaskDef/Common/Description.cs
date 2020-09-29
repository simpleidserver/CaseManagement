using System;

namespace CaseManagement.HumanTask.Domains
{
    public class Description : ICloneable
    {
        public string Language { get; set; }
        public string Value { get; set; }
        public string ContentType { get; set; }

        public object Clone()
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
