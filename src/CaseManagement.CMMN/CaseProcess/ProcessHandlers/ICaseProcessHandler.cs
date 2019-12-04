using CaseManagement.CMMN.Domains;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CaseProcess.ProcessHandlers
{
    public interface ICaseProcessHandler
    {
        string ImplementationType { get; }
        Task Handle(ProcessAggregate process, CaseProcessParameter parameter, Func<CaseProcessResponse, Task> callback, CancellationToken token);
    }
}
