using CaseManagement.BPMN.Domains.ProcessInstance;
using System;

namespace CaseManagement.BPMN.Domains
{
    public abstract class BaseEventDefinition : BaseElement, ICloneable
    {
        public abstract object Clone();
    }
}
