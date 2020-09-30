using System;
using System.Collections.Generic;
using System.Linq;

namespace CaseManagement.HumanTask.Domains
{
    public class PresentationElement : ICloneable
    {
        public PresentationElement()
        {
            Names = new List<Text>();
            Subjects = new List<Text>();
            Descriptions = new List<Description>();
            PresentationParameters = new List<PresentationParameter>();
        }

        /// <summary>
        /// This element is the short title of a task.
        /// </summary>
        public ICollection<Text> Names { get; set; }
        /// <summary>
        /// This element is a longer text that describes the task.
        /// </summary>
        public ICollection<Text> Subjects { get; set; }
        /// <summary>
        /// This element is a long description of the task.
        /// </summary>
        public ICollection<Description> Descriptions { get; set; }
        /// <summary>
        /// This element specifies parameters used in presentation elements subject and description.
        /// </summary>
        public ICollection<PresentationParameter> PresentationParameters { get; set; }

        public object Clone()
        {
            return new PresentationElement
            {
                Names = Names.Select(_ => (Text)_.Clone()).ToList(),
                Subjects = Subjects.Select(_ => (Text)_.Clone()).ToList(),
                Descriptions = Descriptions.Select(_ => (Description)_.Clone()).ToList(),
                PresentationParameters = PresentationParameters.Select(_ => (PresentationParameter)_.Clone()).ToList()
            };
        }
    }
}
