namespace CaseManagement.BPMN.Domains
{
    public class ServiceTask : BaseTask
    {
        public ServiceTask() : base() { }

        /// <summary>
        /// This attribute specifies the technology that will be used to send and receive the Messages.
        /// </summary>
        public string Implementation { get; set; }
        /// <summary>
        /// This attribute specifies the operation that is invoked by the service task.
        /// </summary>
        public string OperationRef { get; set; }
        public string ClassName { get; set; }

        public override FlowNodeTypes FlowNode => FlowNodeTypes.SERVICETASK;

        public override object Clone()
        {
            var result = new ServiceTask
            {
                Implementation = Implementation,
                OperationRef = OperationRef,
                ClassName = ClassName
            };
            FeedActivity(result);
            return result;
        }
    }
}
