using CaseManagement.CMMN.CaseProcess.ProcessHandlers;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Infrastructures
{
    public abstract class CaseProcessDelegate
    {
        public abstract Task Handle(CaseProcessParameter parameter, Func<CaseProcessResponse, Task> callback, CancellationToken token);
    }
}
