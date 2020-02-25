using System.Collections.Generic;

namespace CaseManagement.Gateway.Website.Form.DTOs
{
    public class FormElementResponse
    {
        public string Id { get; set; }
        public bool IsRequired { get; set; }
        public string Type { get; set; }
        public ICollection<TranslationResponse> Tiles{ get; set; }
        public ICollection<TranslationResponse> Descriptions { get; set; }
    }
}
