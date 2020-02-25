using System;
using System.Collections.Generic;

namespace CaseManagement.Gateway.Website.Form.DTOs
{
    public class FormResponse
    {
        public string Id { get; set; }
        public int Version { get; set; }
        public string Status { get; set; }
        public DateTime CreateDateTime { get; set; }
        public DateTime UpdateDateTime { get; set; }
        public ICollection<TranslationResponse> Titles { get; set; }
        public ICollection<FormElementResponse> Elements { get; set; }
    }
}
