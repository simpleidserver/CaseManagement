using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace CaseManagement.Gateway.Website.CaseFile.DTOs
{
    [DataContract]
    public class UpdateCaseFileParameter
    {
        [DataMember(Name = "name")]
        public string Name { get; set; }
        [DataMember(Name = "description")]
        public string Description { get; set; }
        [DataMember(Name = "payload")]
        public string Payload { get; set; }
    }
}
