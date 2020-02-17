using System;
using System.Runtime.Serialization;

namespace CaseManagement.Gateway.Website.CasePlans.DTOs
{
    [DataContract]
    public class CasePlanResponse
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }
        [DataMember(Name = "name")]
        public string Name { get; set; }
        [DataMember(Name = "description")]
        public string Description { get; set; }
        [DataMember(Name = "case_file")]
        public string CaseFileId { get; set; }
        [DataMember(Name = "create_datetime")]
        public DateTime CreateDateTime { get; set; }
        [DataMember(Name = "version")]
        public int Version { get; set; }
        [DataMember(Name = "owner")]
        public string Owner { get; set; }
    }
}
