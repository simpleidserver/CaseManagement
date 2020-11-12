namespace CaseManagement.BPMN.Domains
{
    /// <summary>
    /// A user task is a typical "workflow" Task where a human performer performs the Task with the assistance of a software application.
    /// The lifecycle of the Task is managed by a software component (call task manager) and is typically executed in the context of a Process.
    /// </summary>
    public class UserTask : BaseTask
    {
        public override FlowNodeTypes FlowNode => FlowNodeTypes.USERTASK;

        /// <summary>
        /// This attribute specifies the technology that will be used to implement the User Task.
        /// Valid values are "##unspecified", "##webservice".
        /// </summary>
        public string Implementation { get; set; }
        /// <summary>
        /// Name of the human task.
        /// </summary>
        public string HumanTaskName { get; set; }

        public override object Clone()
        {
            var result = new UserTask
            {
                Implementation = Implementation
            };
            FeedActivity(result);
            return result;
        }                                                                                                                                                                                                                                                                                                                       
    }
}
