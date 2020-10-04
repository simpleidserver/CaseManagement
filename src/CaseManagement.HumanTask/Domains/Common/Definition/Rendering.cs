using System;
using System.Collections.Generic;
using System.Linq;

namespace CaseManagement.HumanTask.Domains
{
    public class Rendering : ICloneable
    {
        public Rendering()
        {
            Input = new List<InputRenderingElement>();
            Output = new List<OutputRenderingElement>();
        }

        /// <summary>
        /// Used to render input information to the task instance in the user interface.
        /// </summary>
        public ICollection<InputRenderingElement> Input { get; set; }
        /// <summary>
        /// Used to render user workspace (to generate html form that need to filled by the task assignee) and
        /// to populate response message to the callback service when task instance get completed
        /// </summary>
        public ICollection<OutputRenderingElement> Output { get; set; }

        public object Clone()
        {
            return new Rendering
            {
                Input = Input.Select(i => (InputRenderingElement)i.Clone()).ToList(),
                Output = Output.Select(o => (OutputRenderingElement)o.Clone()).ToList()
            };
        }
    }

    public class NotificationRendering : ICloneable
    {
        public string Content { get; set; }

        public object Clone()
        {
            return new NotificationRendering
            {
                Content = Content
            };
        }
    }

    public abstract class RenderingElement : ICloneable
    {
        /// <summary>
        /// Unique ID for each element.
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// Label (or text) to display with the value.
        /// </summary>
        public string Label { get; set; }

        public abstract object Clone();
    }

    public class InputRenderingElement : RenderingElement
    {
        /// <summary>
        /// XPATH to get the value from the input message / presentationParameter
        /// </summary>
        public string Value { get; set; }

        public override object Clone()
        {
            return new InputRenderingElement
            {
                Id = Id,
                Label = Label,
                Value = Value
            };
        }
    }

    public class OutputRenderingElement : RenderingElement
    {
        /// <summary>
        /// XPATH of the element in the output message to be filled with this form field
        /// </summary>
        public string XPath { get; set; }
        public OutputRenderingElementValue Value { get; set; }
        /// <summary>
        /// Can provide default value to display in the form
        /// </summary>
        public string Default { get; set; }

        public override object Clone()
        {
            return new OutputRenderingElement
            {
                XPath = XPath,
                Value = (OutputRenderingElementValue)Value?.Clone()
            };
        }
    }

    public class OutputRenderingElementValue : ICloneable
    {
        public OutputRenderingElementValue()
        {
            Values = new List<string>();
        }

        /// <summary>
        /// Used to define the type of the form field.
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// Values
        /// </summary>
        public IEnumerable<string> Values { get; set; }

        public object Clone()
        {
            return new OutputRenderingElementValue
            {
                Type = Type,
                Values = Values.ToList()
            };
        }
    }
}
