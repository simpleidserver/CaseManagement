using System;
using System.Collections.Generic;
using System.Linq;

namespace CaseManagement.HumanTask.Domains
{
    public class RenderingElement : ICloneable
    {
        public RenderingElement()
        {
            Labels = new List<Translation>();
        }

        /// <summary>
        /// Unique ID for each element.
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// Label (or text) to display with the value.
        /// </summary>
        public ICollection<Translation> Labels { get; set; }
        /// <summary>
        /// XPATH of the element in the output message to be filled with this form field
        /// </summary>
        public string XPath { get; set; }
        public string ValueType { get; set; }
        public IEnumerable<OptionValue> Values { get; set; }
        /// <summary>
        /// Can provide default value to display in the form
        /// </summary>
        public string Default { get; set; }

        public object Clone()
        {
            return new RenderingElement
            {
                Id = Id,
                Labels = Labels.Select(_ => (Translation)_.Clone()).ToList(),
                XPath = XPath,
                ValueType = ValueType,
                Values = Values.Select(_ => (OptionValue)_.Clone()).ToList(),
                Default = Default
            };
        }
    }


    public class OptionValue : ICloneable
    {
        public OptionValue()
        {
            DisplayNames = new List<Translation>();
        }

        public long Id { get; set; }
        public string Value { get; set; }
        public ICollection<Translation> DisplayNames { get; set; }

        public object Clone()
        {
            return new OptionValue
            {
                Id = Id,
                Value = Value,
                DisplayNames = DisplayNames.Select(_ => (Translation)_.Clone()).ToList()
            };
        }
    }
}
