namespace CaseManagement.CMMN.Domains
{
    public class ApplicabilityRule
    {
        /// <summary>
        /// The name of the ApplicabilityRule.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// The caseFileItem that servers as starting point for evaluation of the Expression that is specified by the condition of the applicability rule.
        /// If not specified, evaluation starts at the CaseFile object that is referenced by the Case as its caseFileModel.
        /// </summary>
        public string ContextRef { get; set; }
        /// <summary>
        /// The Expression that servers as condition of the ApplicabilityRule. 
        /// If it evaluates to "true", then the associated TableItem is available for planning (if a case worker is also assigned the Role that is authorized
        /// for planning based on the TableItem).
        /// </summary>
        public string Expression { get; set; }
    }
}
