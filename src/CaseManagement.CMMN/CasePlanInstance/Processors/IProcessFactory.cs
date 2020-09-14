using CaseManagement.CMMN.Domains;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CasePlanInstance.Processors
{
    public interface IProcessorFactory
    {
        Task Execute<T>(CMMNExecutionContext executionContext, T instance, CancellationToken token) where T : BaseCaseEltInstance;
        Task Execute(CMMNExecutionContext executionContext, BaseCaseEltInstance instance, Type type, CancellationToken token);
    }
}
