using System;

namespace CaseManagement.CMMN.Domains.CaseFile
{
    public class CaseFileDefinitionAggregate : ICloneable
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreateDateTime { get; set; }
        public DateTime UpdateDateTime { get; set; }
        public string Payload { get; set; }
        public string Owner { get; set; }

        public object Clone()
        {
            return new CaseFileDefinitionAggregate
            {
                Id = Id,
                Name = Name,
                Description = Description,
                CreateDateTime = CreateDateTime,
                Payload = Payload,
                Owner = Owner,
                UpdateDateTime = UpdateDateTime
            };
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}