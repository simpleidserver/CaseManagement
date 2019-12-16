using System;

namespace CaseManagement.CMMN.Domains
{
    public class CaseActivationAggregate : ICloneable
    {
        public string Id { get; set; }
        public string ProcessId { get; set; }
        public string ProcessName { get; set; }
        public string ElementId { get; set; }
        public string ElementName { get; set; }
        public DateTime CreateDateTime { get; set; }
        
        public object Clone()
        {
            return new CaseActivationAggregate
            {
                Id = Id,
                ProcessId = ProcessId,
                ProcessName = ProcessName,
                ElementId = ElementId,
                ElementName = ElementName,
                CreateDateTime = CreateDateTime
            };
        }
    }
}
