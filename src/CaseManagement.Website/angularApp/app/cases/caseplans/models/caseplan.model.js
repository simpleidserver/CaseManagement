var CasePlan = (function () {
    function CasePlan() {
    }
    CasePlan.fromJson = function (json) {
        var result = new CasePlan();
        result.Id = json["id"];
        result.Name = json["name"];
        result.Description = json["description"];
        result.CaseFile = json["case_file"];
        result.CreateDateTime = json["create_datetime"];
        result.Version = json["version"];
        result.Owner = json["owner"];
        return result;
    };
    return CasePlan;
}());
export { CasePlan };
//# sourceMappingURL=caseplan.model.js.map