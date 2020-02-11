using System;

namespace CaseManagement.CMMN.Domains
{
    public class FormInstanceElement : ICloneable
    {
        public string FormElementId { get; set; }
        public string Value { get; set; }

        public object Clone()
        {
            return new FormInstanceElement
            {
                FormElementId = FormElementId,
                Value = Value
            };
        }
    }
}
