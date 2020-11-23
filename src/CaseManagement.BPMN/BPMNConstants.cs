namespace CaseManagement.BPMN
{
    public static class BPMNConstants
    {
        public const string BPMNNamespace = "https://github.com/simpleidserver/CaseManagement";
        public static class RouteNames
        {
            public const string ProcessInstances = "processinstances";
            public const string ProcessFiles = "processfiles";
        }

        public static class QueueNames
        {
            public const string ProcessInstances = "processinstances";
            public const string Messages = "messages";
            public const string StateTransitions = "statetransitions";
        }

        public static class ServiceTaskImplementations
        {
            public const string CALLBACK = "##csharpcallback";
        }

        public static class UserTaskImplementations
        {
            public const string WEBSERVICE = "##WebService";
            public const string WSHUMANTASK = "##WsHumanTask";
        }
    }
}
