var CountResult = (function () {
    function CountResult() {
    }
    CountResult.fromJson = function (json) {
        var result = new CountResult();
        result.NbCaseFiles = json["nb_case_files"];
        result.NbCasePlans = json["nb_case_plans"];
        return result;
    };
    return CountResult;
}());
export { CountResult };
//# sourceMappingURL=count-result.model.js.map