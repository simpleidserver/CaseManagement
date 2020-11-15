namespace CaseManagement.BPMN
{
    public static class BPMNConstants
    {
        public static class RouteNames
        {
            public const string ProcessInstances = "processinstances";
        }

        public static class QueueNames
        {
            public const string ProcessInstances = "processinstances";
            public const string Messages = "messages";
            public const string StateTransitions = "statetransitions";
        }

        public static class ImplementationNames
        {
            public const string CALLBACK = "##csharpcallback";
        }

        public static class UserTaskImplementations
        {
            public const string WEBSERVICE = "##WebService";
        }
    }
}
