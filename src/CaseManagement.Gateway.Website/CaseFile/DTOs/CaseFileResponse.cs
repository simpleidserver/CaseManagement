using System;
using System.Runtime.Serialization;

namespace CaseManagement.Gateway.Website.CaseFile.DTOs
{
    [DataContract]
    public class CaseFileResponse
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }
        [DataMember(Name = "name")]
        public string Name { get; set; }
        [DataMember(Name = "description")]
        public string Description { get; set; }
        [DataMember(Name = "payload")]
        public string Payload { get; set; }
        [DataMember(Name = "create_datetime")]
        public DateTime CreateDateTime { get; set; }
        [DataMember(Name = "update_datetime")]
        public DateTime UpdateDateTime { get; set; }
        [DataMember(Name = "version")]
        public int Version { get; set; }
        [DataMember(Name = "file_id")]
        public string FileId { get; set; }
        [DataMember(Name = "owner")]
        public string Owner { get; set; }
        [DataMember(Name = "status")]
        public string Status { get; set; }
    }
}