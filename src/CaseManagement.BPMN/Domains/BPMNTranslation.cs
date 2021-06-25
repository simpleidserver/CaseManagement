using System;

namespace CaseManagement.BPMN.Domains
{
    public class BPMNTranslation : ICloneable
    {
        public BPMNTranslation() { }

        public BPMNTranslation(string key, string value, string language)
        {
            Key = key;
            Value = value;
            Language = language;
        }

        public string Key { get; set; }
        public string Value { get; set; }
        public string Language { get; set; }
        public string Type { get; set; }

        public static BPMNTranslation Create(string key, string value, string language, string type)
        {
            return new BPMNTranslation
            {
                Key = key,
                Value = value,
                Language = language,
                Type = type
            };
        }

        public object Clone()
        {
            return new BPMNTranslation(Key, Value, Language)
            {
                Type = Type
            };
        }
    }
}
