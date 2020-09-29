using System;

namespace CaseManagement.HumanTask.Domains
{
    public class Text : ICloneable
    {
        public string Language { get; set; }
        public string Value { get; set; }

        public object Clone()
        {
            return new Text
            {
                Language = Language,
                Value = Value
            };
        }
    }
}
