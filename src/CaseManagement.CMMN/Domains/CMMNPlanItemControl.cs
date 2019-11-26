using System;

namespace CaseManagement.CMMN.Domains
{
    public abstract class CMMNPlanItemControl : ICloneable
    {
        public abstract object Clone();
    }
}
