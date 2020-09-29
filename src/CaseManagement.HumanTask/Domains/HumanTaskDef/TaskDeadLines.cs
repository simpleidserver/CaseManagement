using System;
using System.Collections.Generic;
using System.Linq;

namespace CaseManagement.HumanTask.Domains
{
    public class TaskDeadLines : ICloneable
    {
        public TaskDeadLines()
        {
            StartDeadLines = new List<DeadLine>();
            CompletionDeadLines = new List<DeadLine>();
        }
        /// <summary>
        /// Specifies the time until the task has to start, i.e. it has to reach state InProgress.
        /// </summary>
        public ICollection<DeadLine> StartDeadLines { get; set; }
        /// <summary>
        /// Specifies the due time of the task. 
        /// It is defined as either the period of time until the task gets due or the point in time when the task gets due. 
        /// </summary>
        public ICollection<DeadLine> CompletionDeadLines { get; set; }

        public object Clone()
        {
            return new TaskDeadLines
            {
                StartDeadLines = StartDeadLines.Select(_ => (DeadLine)_.Clone()).ToList(),
                CompletionDeadLines = CompletionDeadLines.Select(_ => (DeadLine)_.Clone()).ToList()
            };
        }
    }

    public class DeadLine : ICloneable
    {
        public DeadLine()
        {
            Escalations = new List<Escalation>();
        }

        public string Name { get; set; }
        public string For { get; set; }
        public string Until { get; set; }
        /// <summary>
        /// If a status is not reached within a certain time then an escalation action can be triggered.
        /// Escalations are triggered if :
        /// The associated point in time is reached, or duration has elasped and the associated condition evaluates to true.
        /// </summary>
        public ICollection<Escalation> Escalations { get; set; }

        public object Clone()
        {
            return new DeadLine
            {
                Name = Name,
                For = For,
                Until = Until,
                Escalations = Escalations.Select(_ => (Escalation)_.Clone()).ToList()
            };
        }
    }
}
