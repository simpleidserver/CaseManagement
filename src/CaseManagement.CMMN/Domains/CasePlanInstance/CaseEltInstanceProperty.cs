using System;

namespace CaseManagement.CMMN.Domains.CasePlanInstance
{
    public class CaseEltInstanceProperty : ICloneable
    {
        public string Key { get; set; }
        public string Value { get; set; }

        public object Clone()
        {
            return new CaseEltInstanceProperty
            {
                Key = Key,
                Value = Value
            };
        }
    }
}
