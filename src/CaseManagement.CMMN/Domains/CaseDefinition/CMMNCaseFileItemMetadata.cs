using System;

namespace CaseManagement.CMMN.Domains
{
    public class CMMNCaseFileItemMetadata : ICloneable
    {
        public CMMNCaseFileItemMetadata(string key, string value)
        {
            Key = key;
            Value = value;
        }

        public string Key { get; set; }
        public string Value { get; set; }

        public object Clone()
        {
            return new CMMNCaseFileItemMetadata(Key, Value);
        }
    }
}
