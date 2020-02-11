using System;

namespace CaseManagement.CMMN.Domains
{
    public abstract class PlanItemControl : ICloneable
    {
        public abstract object Clone();
    }
}
