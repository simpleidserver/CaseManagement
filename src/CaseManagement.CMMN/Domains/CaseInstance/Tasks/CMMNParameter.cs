using System;

namespace CaseManagement.CMMN.Domains
{
    public class CMMNParameter : ICloneable
    {
        public string Name { get; set; }

        public object Clone()
        {
            return new CMMNParameter
            {
                Name = Name
            };
        }
    }
}
