import { CaseTranslation } from "./case-translation.model";
var FormElementInstance = (function () {
    function FormElementInstance() {
        this.Names = [];
        this.Descriptions = [];
    }
    FormElementInstance.fromJson = function (json) {
        var result = new FormElementInstance();
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
    return FormElementInstance;
}());
export { FormElementInstance };
var FormInstance = (function () {
    function FormInstance() {
        this.Titles = [];
        this.Content = [];
    }
    FormInstance.fromJson = function (json) {
        var result = new FormInstance();
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
            result.Content.push(FormElementInstance.fromJson(elt));
        });
        return result;
    };
    return FormInstance;
}());
export { FormInstance };
//# sourceMappingURL=forminstance.model.js.map