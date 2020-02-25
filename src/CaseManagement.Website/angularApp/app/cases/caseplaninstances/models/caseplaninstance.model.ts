export class Translation {
    Language: string;
    Value: string;

    public static fromJson(key: string, value: string): Translation {
        let result = new Translation();
        result.Language = key.split('#')[1];
        result.Value = value;
        return result;
    }
}

export class FormElement {
    constructor() {
        this.Titles = [];
        this.Descriptions = [];
    }

    Id: string;
    IsRequired: boolean;
    Type: string;
    Titles: Array<Translation>;
    Descriptions: Array<Translation>;

    public static fromJson(json: any): FormElement {
        let result = new FormElement();
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
    }
}

export class Form {
    constructor() {
        this.Titles = [];
        this.Elements = [];
    }

    Id: string;
    Version: number;
    Status: string;
    CreateDateTime: Date;
    UpdateDateTime: Date;
    Titles: Array<Translation>;
    Elements: Array<FormElement>;

    public static fromJson(json: any): Form {
        let form = new Form();
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

        let elements = json["elements"];
        if (elements) {
            json["elements"].forEach(function (fe: any) {
                form.Elements.push(FormElement.fromJson(fe));
            });
        }

        return form;
    }
}

export class FormInstance {
    CaseElementInstanceId: string;
    CasePlanId: string;
    CasePlanInstanceId: string;
    CreateDateTime: Date;
    UpdateDateTime: Date;
    FormId: string;

    public static fromJson(json: any): FormInstance {
        let result = new FormInstance();
        result.CaseElementInstanceId = json["case_element_instance_id"];
        result.CasePlanId = json["case_plan_id"];
        result.CasePlanInstanceId = json["case_plan_instance_id"];
        result.CreateDateTime = json["create_datetime"];
        result.UpdateDateTime = json["update_datetime"];
        result.FormId = json["form_id"];
        return result;
    }
}

export class StateHistory {
    State: string;
    DateTime: string;

    public static fromJson(json: any): StateHistory{
        let result = new StateHistory();
        result.State = json["state"];
        result.DateTime = json["datetime"];
        return result;
    }
}

export class TransitionHistory {
    Transition: string;
    DateTime: string;

    public static fromJson(json: any): TransitionHistory {
        let result = new TransitionHistory();
        result.Transition = json["transition"];
        result.DateTime = json["datetime"];
        return result;
    }
}

export class CasePlanElementInstance {
    constructor() {
        this.StateHistories = [];
        this.TransitionHistories = [];
    }

    Id: string;
    Name: string;
    Type: string;
    Version: string;
    CreateDateTime: Date;
    DefinitionId: string;
    State: string;
    StateHistories: StateHistory[];
    TransitionHistories: TransitionHistory[];
    FormInstance: FormInstance;
    Form: Form;

    public static fromJson(json: any): CasePlanElementInstance {
        let result = new CasePlanElementInstance();
        result.Id = json["id"];
        result.Name = json["name"];
        result.Type = json["type"];
        result.Version = json["version"];
        result.CreateDateTime = json["create_datetime"];
        result.DefinitionId = json["definition_id"];
        result.State = json["state"];
        json["state_histories"].forEach(function (sh: any) {
            result.StateHistories.push(StateHistory.fromJson(sh));
        });
        json["transition_histories"].forEach(function (th: any) {
            result.TransitionHistories.push(TransitionHistory.fromJson(th));
        });

        let formInstance = json["form_instance"];
        if (formInstance) {
            result.FormInstance = FormInstance.fromJson(formInstance);
        }

        let form = json["form"];
        if (form) {
            result.Form = Form.fromJson(form);
        }

        return result;
    }
}

export class CasePlanInstance {
    constructor() {
        this.StateHistories = [];
        this.TransitionHistories = [];
        this.Elements = [];
    }

    Id: string;
    Name: string;
    CreateDateTime: Date;
    CasePlanId: string;
    Context: any;
    State: string;
    StateHistories: StateHistory[];
    TransitionHistories: TransitionHistory[];
    Elements: CasePlanElementInstance[];

    public static fromJson(json: any): CasePlanInstance {
        var result = new CasePlanInstance();
        result.Id = json["id"];
        result.Name = json["name"];
        result.CreateDateTime = json["create_datetime"];
        result.CasePlanId = json["case_plan_id"];
        result.Context = json["context"];
        result.State = json["state"];
        json["state_histories"].forEach(function (sh: any) {
            result.StateHistories.push(StateHistory.fromJson(sh));
        });
        json["transition_histories"].forEach(function (th: any) {
            result.TransitionHistories.push(TransitionHistory.fromJson(th));
        });
        json["elements"].forEach(function (elt: any) {
            result.Elements.push(CasePlanElementInstance.fromJson(elt));
        });
        return result;
    }
}