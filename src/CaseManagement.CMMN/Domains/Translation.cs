using System;

namespace CaseManagement.CMMN.Domains
{
    public class Translation : ICloneable
    {
        public Translation(string language, string value)
        {
            Language = language;
            Value = value;
        }

        public string Language { get; set; }
        public string Value { get; set; }

        public object Clone()
        {
            return new Translation(Language, Value);
        }
    }
}
