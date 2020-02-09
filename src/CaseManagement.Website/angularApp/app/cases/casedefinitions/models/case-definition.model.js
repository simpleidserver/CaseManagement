var CaseDefinition = (function () {
    function CaseDefinition() {
    }
    CaseDefinition.fromJson = function (json) {
        var result = new CaseDefinition();
        result.Id = json["id"];
        result.Name = json["name"];
        result.Description = json["description"];
        result.CaseFile = json["case_file"];
        result.CreateDateTime = json["create_datetime"];
        return result;
    };
    return CaseDefinition;
}());
export { CaseDefinition };
//# sourceMappingURL=case-definition.model.js.map