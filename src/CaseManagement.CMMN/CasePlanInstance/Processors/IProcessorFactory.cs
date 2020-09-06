using CaseManagement.CMMN.Domains;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CasePlanInstance.Processors
{
    public interface IProcessorFactory
    {
        Task Execute<T>(ExecutionContext executionContext, T instance, CancellationToken token) where T : CasePlanElementInstance;
        Task Execute(ExecutionContext executionContext, CasePlanElementInstance instance, Type type, CancellationToken token);
    }
}
