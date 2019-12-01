using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Infrastructures;
using System;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CaseProcess.ProcessHandlers
{
    public class CaseManagementCallbackProcessHandler : ICaseProcessHandler
    {
        public string ImplementationType => CMMNConstants.ProcessImplementationTypes.CASEMANAGEMENTCALLBACK;

        public async Task<CaseProcessResponse> Handle(ProcessAggregate process, CaseProcessParameter parameter)
        {
            var caseManagementProcessAggregate = (CaseManagementProcessAggregate)process;
            var instance = (CaseProcessDelegate)Activator.CreateInstance(Type.GetType(caseManagementProcessAggregate.AssemblyQualifiedName));
            var result = await instance.Handle(parameter);
            return result;
        }
    }
}
