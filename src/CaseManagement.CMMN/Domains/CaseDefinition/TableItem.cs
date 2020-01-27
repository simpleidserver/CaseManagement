namespace CaseManagement.CMMN.Domains
{
    public class TableItem
    {
        public TableItem()
        {
            AuthorizedRoleRef = string.Empty;
            ApplicabilityRuleRef = null;
        }

        /// <summary>
        /// References to zero or more Role objects that are authorized to plan based on the TableItem.
        /// </summary>
        public string AuthorizedRoleRef { get; set; }
        /// <summary>
        /// If the condition of the ApplicabilityRule object evaluates to "true" then the TableItem is applicable for planning, otherwise it is not.
        /// </value>
        public ApplicabilityRule ApplicabilityRuleRef { get; set; }
    }
}