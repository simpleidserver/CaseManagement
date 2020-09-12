using System;

namespace CaseManagement.CMMN.Domains
{
    [Serializable]
    public class CasePlanFileItem : ICloneable
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string DefinitionType { get; set; }

        public object Clone()
        {
            return new CasePlanFileItem
            {
                Id = Id,
                DefinitionType = DefinitionType,
                Name = Name
            };
        }
    }
}
