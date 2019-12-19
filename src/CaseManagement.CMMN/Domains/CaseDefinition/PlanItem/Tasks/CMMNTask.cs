using System;

namespace CaseManagement.CMMN.Domains
{
    public class CMMNTask : ICloneable
    {
        public CMMNTask(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
        /// <summary>
        /// The Task is waiting until the work associated with the Task is completed.
        /// </summary>
        public bool IsBlocking { get; set; }

        public virtual object Clone()
        {
            return CloneTask();
        }

        public virtual object CloneTask()
        {
            return new CMMNTask(Name)
            {
                IsBlocking = IsBlocking
            };
        }
    }
}
