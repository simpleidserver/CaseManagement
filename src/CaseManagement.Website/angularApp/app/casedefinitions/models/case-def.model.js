var CaseDefinition = (function () {
    function CaseDefinition() {
    }
    CaseDefinition.fromJson = function (json) {
        var result = new CaseDefinition();
        result.Id = json["id"];
        result.Name = json["name"];
        result.CreateDateTime = json["create_datetime"];
        result.Xml = json["xml"];
        return result;
    };
    return CaseDefinition;
}());
export { CaseDefinition };
//# sourceMappingURL=case-def.model.js.map