﻿using System;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.Workflow.Engine
{
    public interface IProcessFlowElementProcessor
    {
        Type ProcessFlowElementType { get; }
        Task Handle(WorkflowHandlerContext context, CancellationToken token);
    }
}
