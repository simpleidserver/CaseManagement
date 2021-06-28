using System;

namespace CaseManagement.CMMN.Domains
{
    [Serializable]
    public class CasePlanFileItem : ICloneable
    {
        public string EltId { get; set; }
        public string Name { get; set; }
        public string DefinitionType { get; set; }

        public object Clone()
        {
            return new CasePlanFileItem
            {
                EltId = EltId,
                DefinitionType = DefinitionType,
                Name = Name
            };
        }
    }
}
