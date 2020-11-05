using System;

namespace CaseManagement.HumanTask.Domains
{
    public class Translation : ICloneable
    {
        public long Id { get; set; }
        public string Language { get; set; }
        public string Value { get; set; }

        public object Clone()
        {
            return new Translation
            {
                Language = Language,
                Value = Value
            };
        }
    }
}
