import { CaseTranslation } from "./case-translation.model";
var CaseFormElementInstance = (function () {
    function CaseFormElementInstance() {
        this.Names = [];
        this.Descriptions = [];
    }
    CaseFormElementInstance.fromJson = function (json) {
        var result = new CaseFormElementInstance();
        result.FormElementId = json["form_element_id"];
        result.IsRequired = json["is_required"];
        result.Value = json["value"];
        result.Type = json["type"];
        for (var key in json) {
            if (key.startsWith("name")) {
                result.Names.push(CaseTranslation.fromJson(json["key"]));
            }
            else if (key.startsWith("description")) {
                result.Descriptions.push(CaseTranslation.fromJson(json["description"]));
            }
        }
        return result;
    };
    return CaseFormElementInstance;
}());
export { CaseFormElementInstance };
var CaseFormInstance = (function () {
    function CaseFormInstance() {
        this.Titles = [];
        this.Content = [];
    }
    CaseFormInstance.fromJson = function (json) {
        var result = new CaseFormInstance();
        result.Id = json["id"];
        result.CreateDateTime = json["create_datetime"];
        result.UpdateDateTime = json["update_datetime"];
        result.Performer = json["performer"];
        result.CaseDefinitionId = json["case_definition_id"];
        result.CaseInstanceId = json["case_instance_id"];
        result.CaseElementDefinitionId = json["case_element_definition_id"];
        result.CaseElementInstanceId = json["case_element_instance_id"];
        result.Status = json["status"];
        result.FormId = json["form_id"];
        for (var key in json) {
            if (key.startsWith("title")) {
                result.Titles.push(CaseTranslation.fromJson(json[key]));
            }
        }
        json["content"].forEach(function (elt) {
            result.Content.push(CaseFormElementInstance.fromJson(elt));
        });
        return result;
    };
    return CaseFormInstance;
}());
export { CaseFormInstance };
//# sourceMappingURL=case-form-instance.model.js.map