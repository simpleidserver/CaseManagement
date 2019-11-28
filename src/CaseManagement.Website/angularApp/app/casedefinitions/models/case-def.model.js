var CasePlanModel = (function () {
    function CasePlanModel() {
    }
    CasePlanModel.fromJson = function (json) {
        var result = new CasePlanModel();
        result.Id = json["id"];
        result.Name = json["name"];
        return result;
    };
    return CasePlanModel;
}());
export { CasePlanModel };
var CaseDefinition = (function () {
    function CaseDefinition() {
        this.CasePlanModels = [];
    }
    CaseDefinition.fromJson = function (json) {
        var result = new CaseDefinition();
        result.Id = json["id"];
        result.Name = json["name"];
        result.CreateDateTime = json["create_datetime"];
        result.Xml = json["xml"];
        var casePlanModels = [];
        if (json["cases"]) {
            json["cases"].forEach(function (c) {
                casePlanModels.push(CasePlanModel.fromJson(c));
            });
        }
        result.CasePlanModels = casePlanModels;
        return result;
    };
    return CaseDefinition;
}());
export { CaseDefinition };
//# sourceMappingURL=case-def.model.js.map