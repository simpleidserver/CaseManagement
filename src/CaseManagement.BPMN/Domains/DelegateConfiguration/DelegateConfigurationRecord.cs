using System;

namespace CaseManagement.BPMN.Domains
{
    public class DelegateConfigurationRecord : ICloneable
    {
        public string Key { get; set; }
        public string Value { get; set; }

        public object Clone()
        {
            return new DelegateConfigurationRecord
            {
                Key = Key,
                Value = Value
            };
        }
    }
}
