var CaseInstanceState = (function () {
    function CaseInstanceState() {
        this.caseDefinition = null;
        this.caseInstance = null;
        this.executionStepsResult = null;
        this.isCaseInstanceLoading = true;
        this.isCaseInstanceErrorLoadOccured = false;
        this.isCaseExecutionStepsLoading = true;
        this.isCaseExecutionStepsErrorLoadOccured = false;
    }
    return CaseInstanceState;
}());
export { CaseInstanceState };
//# sourceMappingURL=case-instance-states.js.map