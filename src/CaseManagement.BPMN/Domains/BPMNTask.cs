﻿using CaseManagement.Workflow.Domains;

namespace CaseManagement.BPMN.Domains
{
    public class BPMNTask : ProcessFlowInstanceElement
    {
        public BPMNTask(string id, string name) : base(id, name)
        {
        }

        public override string ElementType => throw new System.NotImplementedException();

        public override object Clone()
        {
            throw new System.NotImplementedException();
        }

        public override void HandleEvent(string state)
        {
            throw new System.NotImplementedException();
        }

        public override void HandleLaunch()
        {
            throw new System.NotImplementedException();
        }
    }
}