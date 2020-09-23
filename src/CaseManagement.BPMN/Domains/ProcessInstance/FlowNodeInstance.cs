﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace CaseManagement.BPMN.Domains
{
    public class FlowNodeInstance : ICloneable
    {
        public FlowNodeInstance()
        {
            ActivityStates = new List<ActivityStateHistory>();
        }

        public string Id { get; set; }
        public string FlowNodeId { get; set; }
        public FlowNodeStates State { get; set; }
        public ActivityStates? ActivityState { get; set; }
        public ICollection<ActivityStateHistory> ActivityStates { get; set; }

        public object Clone()
        {
            return new FlowNodeInstance
            {
                Id = Id,
                FlowNodeId = FlowNodeId,
                State = State,
                ActivityState = ActivityState,
                ActivityStates = ActivityStates.Select(_ => (ActivityStateHistory)_.Clone()).ToList()
            };
        }

        public void UpdateState(ActivityStates state, DateTime updateDateTime)
        {
            ActivityStates.Add(new ActivityStateHistory(state, updateDateTime));
            ActivityState = state;
        }
    }
}
