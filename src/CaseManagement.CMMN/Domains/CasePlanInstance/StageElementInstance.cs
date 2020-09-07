using System;
using System.Collections.Generic;
using System.Linq;

namespace CaseManagement.CMMN.Domains
{
    public class StageElementInstance : BaseTaskOrStageElementInstance
    {
        public StageElementInstance()
        {
            Children = new List<CasePlanElementInstance>();
        }

        public ICollection<CasePlanElementInstance> Children { get; set; }
        public int Length => Children.Count();

        public void AddChild(CasePlanElementInstance child)
        {
            Children.Add(child);
        }
        

        public CasePlanElementInstance GetChild(string id)
        {
            var child = Children.FirstOrDefault(_ => _.Id == id);
            if (child != null)
            {
                return child;
            }

            var stages = Children.Where(_ => _ is StageElementInstance).Select(_ => _ as StageElementInstance);
            foreach(var stage in stages)
            {
                child = stage.GetChild(id);
                if (child != null)
                {
                    return child;
                }
            }

            return null;
        }

        protected override void Process(CMMNTransitions transition, DateTime executionDatTime)
        {
            if (transition == CMMNTransitions.Terminate || transition == CMMNTransitions.ParentTerminate)
            {
                foreach(var child in Children)
                {
                    child.MakeTransition(CMMNTransitions.ParentTerminate, executionDatTime);
                }
            }
        }
    }
}
