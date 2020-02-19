using System.Runtime.Serialization;

namespace CaseManagement.Gateway.Website.FormInstance.DTOs
{
    [DataContract]
    public class FormInstanceElementResponse
    {
        [DataMember(Name = "form_element_id")]
        public string FormElementId { get; set; }
        [DataMember(Name = "value")]
        public string Value { get; set; }
    }
}
