var CaseFileItem = (function () {
    function CaseFileItem() {
    }
    CaseFileItem.fromJson = function (json) {
        var result = new CaseFileItem();
        result.ElementDefinitionId = json["element_definition_id"];
        result.ElementInstanceId = json["element_instance_id"];
        result.CaseInstanceId = json["case_instance_id"];
        result.Value = json["value"];
        result.Id = json["id"];
        result.Type = json["type"];
        result.CreateDateTime = json["create_datetime"];
        return result;
    };
    return CaseFileItem;
}());
export { CaseFileItem };
//# sourceMappingURL=case-file-item.model.js.map