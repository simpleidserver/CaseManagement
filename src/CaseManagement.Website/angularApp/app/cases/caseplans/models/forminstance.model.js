var FormElementInstance = (function () {
    function FormElementInstance() {
    }
    FormElementInstance.fromJson = function (json) {
        var result = new FormElementInstance();
        result.FormElementId = json["form_element_id"];
        result.Value = json["value"];
        return result;
    };
    return FormElementInstance;
}());
export { FormElementInstance };
var FormInstance = (function () {
    function FormInstance() {
        this.Content = [];
    }
    FormInstance.fromJson = function (json) {
        var result = new FormInstance();
        result.Id = json["id"];
        result.CreateDateTime = json["create_datetime"];
        result.UpdateDateTime = json["update_datetime"];
        result.Performer = json["performer"];
        result.CasePlanId = json["case_plan_id"];
        result.CasePlanInstanceId = json["case_plan_instance_id"];
        result.CasePlanElementInstanceId = json["case_plan_element_instance_id"];
        result.Status = json["status"];
        result.FormId = json["form_id"];
        json["content"].forEach(function (elt) {
            result.Content.push(FormElementInstance.fromJson(elt));
        });
        return result;
    };
    return FormInstance;
}());
export { FormInstance };
//# sourceMappingURL=forminstance.model.js.map