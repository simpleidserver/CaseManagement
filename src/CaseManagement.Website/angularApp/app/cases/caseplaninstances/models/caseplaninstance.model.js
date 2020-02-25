var Translation = (function () {
    function Translation() {
    }
    Translation.fromJson = function (key, value) {
        var result = new Translation();
        result.Language = key.split('#')[1];
        result.Value = value;
        return result;
    };
    return Translation;
}());
export { Translation };
var FormElement = (function () {
    function FormElement() {
        this.Titles = [];
        this.Descriptions = [];
    }
    FormElement.fromJson = function (json) {
        var result = new FormElement();
        result.Id = json["id"];
        result.IsRequired = json["is_required"];
        result.Type = json["type"];
        for (var key in json) {
            if (key.startsWith("title")) {
                result.Titles.push(Translation.fromJson(key, json[key]));
            }
        }
        for (var key in json) {
            if (key.startsWith("description")) {
                result.Descriptions.push(Translation.fromJson(key, json[key]));
            }
        }
        return result;
    };
    return FormElement;
}());
export { FormElement };
var Form = (function () {
    function Form() {
        this.Titles = [];
        this.Elements = [];
    }
    Form.fromJson = function (json) {
        var form = new Form();
        form.Id = json["id"];
        form.Version = json["version"];
        form.Status = json["status"];
        form.CreateDateTime = json["create_datetime"];
        form.UpdateDateTime = json["update_datetime"];
        for (var key in json) {
            if (key.startsWith("title")) {
                form.Titles.push(Translation.fromJson(key, json[key]));
            }
        }
        var elements = json["elements"];
        if (elements) {
            json["elements"].forEach(function (fe) {
                form.Elements.push(FormElement.fromJson(fe));
            });
        }
        return form;
    };
    return Form;
}());
export { Form };
var FormInstance = (function () {
    function FormInstance() {
    }
    FormInstance.fromJson = function (json) {
        var result = new FormInstance();
        result.CaseElementInstanceId = json["case_element_instance_id"];
        result.CasePlanId = json["case_plan_id"];
        result.CasePlanInstanceId = json["case_plan_instance_id"];
        result.CreateDateTime = json["create_datetime"];
        result.UpdateDateTime = json["update_datetime"];
        result.FormId = json["form_id"];
        return result;
    };
    return FormInstance;
}());
export { FormInstance };
var StateHistory = (function () {
    function StateHistory() {
    }
    StateHistory.fromJson = function (json) {
        var result = new StateHistory();
        result.State = json["state"];
        result.DateTime = json["datetime"];
        return result;
    };
    return StateHistory;
}());
export { StateHistory };
var TransitionHistory = (function () {
    function TransitionHistory() {
    }
    TransitionHistory.fromJson = function (json) {
        var result = new TransitionHistory();
        result.Transition = json["transition"];
        result.DateTime = json["datetime"];
        return result;
    };
    return TransitionHistory;
}());
export { TransitionHistory };
var CasePlanElementInstance = (function () {
    function CasePlanElementInstance() {
        this.StateHistories = [];
        this.TransitionHistories = [];
    }
    CasePlanElementInstance.fromJson = function (json) {
        var result = new CasePlanElementInstance();
        result.Id = json["id"];
        result.Name = json["name"];
        result.Type = json["type"];
        result.Version = json["version"];
        result.CreateDateTime = json["create_datetime"];
        result.DefinitionId = json["definition_id"];
        result.State = json["state"];
        json["state_histories"].forEach(function (sh) {
            result.StateHistories.push(StateHistory.fromJson(sh));
        });
        json["transition_histories"].forEach(function (th) {
            result.TransitionHistories.push(TransitionHistory.fromJson(th));
        });
        var formInstance = json["form_instance"];
        if (formInstance) {
            result.FormInstance = FormInstance.fromJson(formInstance);
        }
        var form = json["form"];
        if (form) {
            result.Form = Form.fromJson(form);
        }
        return result;
    };
    return CasePlanElementInstance;
}());
export { CasePlanElementInstance };
var CasePlanInstance = (function () {
    function CasePlanInstance() {
        this.StateHistories = [];
        this.TransitionHistories = [];
        this.Elements = [];
    }
    CasePlanInstance.fromJson = function (json) {
        var result = new CasePlanInstance();
        result.Id = json["id"];
        result.Name = json["name"];
        result.CreateDateTime = json["create_datetime"];
        result.CasePlanId = json["case_plan_id"];
        result.Context = json["context"];
        result.State = json["state"];
        json["state_histories"].forEach(function (sh) {
            result.StateHistories.push(StateHistory.fromJson(sh));
        });
        json["transition_histories"].forEach(function (th) {
            result.TransitionHistories.push(TransitionHistory.fromJson(th));
        });
        json["elements"].forEach(function (elt) {
            result.Elements.push(CasePlanElementInstance.fromJson(elt));
        });
        return result;
    };
    return CasePlanInstance;
}());
export { CasePlanInstance };
//# sourceMappingURL=caseplaninstance.model.js.map