﻿using System.Collections.Generic;
using System.Linq;

namespace CaseManagement.BPMN.Domains
{
    public abstract class BaseCatchEvent : BaseEvent
    {
        public BaseCatchEvent()
        {
            EventDefinitions = new List<BaseEventDefinition>();
        }

        /// <summary>
        /// P235 : Defines the event EventDefinitions that are triggers excepted for a catch Event.
        /// If there is no EventDefinition defined, then this is considered a catch None event.
        /// If there is moare than one EventDefinition defined, this is considered a catch Multiple event
        /// </summary>
        public ICollection<BaseEventDefinition> EventDefinitions { get; set; }
        /// <summary>
        /// Revelant when the catch Event has more than EventDefinition.
        /// If this value is true, then all of the types of triggers that are listed in the catch Event MUST BE triggered before the process is instantiated.
        /// </summary>
        public bool ParallelMultiple { get; set; }

        protected void FeedCatchEvent(BaseCatchEvent evt)
        {
            FeedFlowNode(evt);
            evt.EventDefinitions = evt.EventDefinitions.Select(_ => (BaseEventDefinition)_.Clone()).ToList();
            evt.ParallelMultiple = ParallelMultiple;
        }
    }
}
