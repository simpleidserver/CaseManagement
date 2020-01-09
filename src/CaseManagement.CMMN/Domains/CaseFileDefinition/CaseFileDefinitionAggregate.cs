using System;

namespace CaseManagement.CMMN.Domains.CaseFile
{
    public class CaseFileDefinitionAggregate
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreateDateTime { get; set; }
        public string Payload { get; set; }
    }
}