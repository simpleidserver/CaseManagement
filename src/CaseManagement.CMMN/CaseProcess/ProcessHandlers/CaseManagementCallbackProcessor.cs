using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Infrastructures;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CaseProcess.ProcessHandlers
{
    public class CaseManagementCallbackProcessHandler : ICaseProcessHandler
    {
        private readonly IServiceProvider _serviceProvider;

        public CaseManagementCallbackProcessHandler(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public string ImplementationType => CMMNConstants.ProcessImplementationTypes.CASEMANAGEMENTCALLBACK;

        public Task Handle(ProcessAggregate process, CaseProcessParameter parameter, Func<CaseProcessResponse, Task> callback, CancellationToken token)
        {
            var caseManagementProcessAggregate = (CaseManagementProcessAggregate)process;
            var instance = (CaseProcessDelegate)Activator.CreateInstance(Type.GetType(caseManagementProcessAggregate.AssemblyQualifiedName), _serviceProvider);
            return instance.Handle(parameter, callback, token);
        }
    }
}
