using System.Runtime.Serialization;

namespace CaseManagement.Gateway.Website.CaseFile.DTOs
{
    [DataContract]
    public class AddCaseFileParameter
    {
        [DataMember(Name = "name")]
        public string Name { get; set; }
        [DataMember(Name = "description")]
        public string Description { get; set; }
        [DataMember(Name = "payload")]
        public string Payload { get; set; }
    }
}
