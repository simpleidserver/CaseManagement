using System;

namespace CaseManagement.BPMN.Domains
{
    [Serializable]
    public class Message : BaseElement, ICloneable
    {
        public Message()
        {

        }

        /// <summary>
        /// Name is a text description of the Message.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// An ItemDefinition is used to define the "payload" of the Message.
        /// </summary>
        public string ItemRef { get; set; }

        public object Clone()
        {
            return new Message
            {
                EltId = EltId,
                Name = Name,
                ItemRef = ItemRef
            };
        }
    }
}
