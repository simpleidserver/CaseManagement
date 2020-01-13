var CaseElementDefinitionHistory = (function () {
    function CaseElementDefinitionHistory() {
    }
    CaseElementDefinitionHistory.fromJson = function (json) {
        var result = new CaseElementDefinitionHistory();
        result.Element = json["element"];
        result.NbInstances = json["nb_instances"];
        return result;
    };
    return CaseElementDefinitionHistory;
}());
export { CaseElementDefinitionHistory };
var CaseDefinitionHistory = (function () {
    function CaseDefinitionHistory() {
        this.Elements = [];
    }
    CaseDefinitionHistory.fromJson = function (json) {
        var result = new CaseDefinitionHistory();
        result.Id = json["id"];
        result.NbInstances = json["nb_instances"];
        json["elements"].forEach(function (sh) {
            result.Elements.push(CaseElementDefinitionHistory.fromJson(sh));
        });
        return result;
    };
    return CaseDefinitionHistory;
}());
export { CaseDefinitionHistory };
//# sourceMappingURL=case-definition-history.model.js.map