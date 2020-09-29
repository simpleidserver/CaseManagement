namespace CaseManagement.BPMN.Domains
{
    public class UserTask : BaseTask
    {
        public override FlowNodeTypes FlowNode => FlowNodeTypes.USERTASK;

        /// <summary>
        /// This attribute specifies the technology that will be used to implement the User Task.
        /// Valid values are "##unspecified", "##webservice".
        /// </summary>
        public string Implementation { get; set; }
        // public string Renderings { get; set; }

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
