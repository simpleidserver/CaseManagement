using CaseManagement.CMMN.CaseProcess.ProcessHandlers;

namespace CaseManagement.CMMN.CaseProcess.ProcessHandlers
{
    public static class CaseProcessParameterExtensions
    {
        public static string GetCaseInstanceId(this CaseProcessParameter caseProcessParameter)
        {
            return caseProcessParameter.GetStringParameter(CMMNConstants.StandardProcessMappingVariables.CaseInstanceId);
        }
    }
}
