namespace CaseManagement.CMMN
{
    public static class CMMNConstants
    {
        public const int WAIT_INTERVAL_MS = 20;

        public static class StandardProcessMappingVariables
        {
            public const string CaseInstanceId = "$caseinstanceid$";
        }

        public static class QueueNames
        {
            public const string ExternalEvents = "externalevts";
            public const string CasePlanInstances = "caseplaninstances";
        }

        public static class ExternalTransitionNames
        {
            public const string Terminate = "terminate";
            public const string ManualStart = "manualstart";
            public const string Complete = "complete";
            public const string Reactivate = "reactivate";
            public const string Close = "close";
            public const string Resume = "resume";
            public const string Suspend = "suspend";
            public const string Update = "update";
            public const string Replace = "replace";
            public const string RemoveChild = "removechild";
            public const string AddChild = "addchild";
            public const string AddReference = "addreference";
            public const string RemoveReference = "removereference";
            public const string Delete = "delete";
            public const string Occur = "occur";
            public const string Disable = "disable";
        }

        public static class RouteNames
        {
            public const string CaseFiles = "case-files";
            public const string CasePlans = "case-plans";
            public const string CasePlanInstances = "case-plan-instances";
            public const string CaseProcesses = "case-processes";
            public const string CaseFormInstances = "case-form-instances";
            public const string CaseWorkerTasks = "case-worker-tasks";
            public const string Statistics = "statistics";
            public const string Performances = "performances";
            public const string Forms = "forms";
            public const string Roles = "roles";
        }

        public static class ProcessImplementationTypes
        {
            public const string BMNN20 = "http://www.omg.org/spec/CMMN/ProcessType/BPMN20";
            public const string XPDL2 = "http://www.omg.org/spec/CMMN/ProcessType/XPDL2";
            public const string WSBPEL20 = "http://www.omg.org/spec/CMMN/ProcessType/WSBPEL20";
            public const string WSBPEL1 = "http://www.omg.org/spec/CMMN/ProcessType/WSBPEL1";
            public const string CASEMANAGEMENTCALLBACK = "https://github.com/simpleidserver/CaseManagement/callback";
        }

        public static class ContentManagementTypes
        {
            public const string FAKE_CMIS_DIRECTORY = "https://github.com/simpleidserver/CaseManagement/fakecmis/folder";
        }

        public static class UserTaskImplementations
        {
            public const string WEBSERVICE = "##WebService";
            public const string WSHUMANTASK = "##WsHumanTask";
        }
    }
}