﻿using CaseManagement.CMMN.CaseProcess.ProcessHandlers;
using CaseManagement.CMMN.Infrastructures;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Tests.Delegates
{
    public class ThrowExceptionTask : CaseProcessDelegate
    {
        public ThrowExceptionTask(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public override Task Handle(CaseProcessParameter parameter, Func<CaseProcessResponse, Task> callback, CancellationToken token)
        {
            throw new InvalidOperationException("an error occured while trying to execute the task");
        }
    }
}
