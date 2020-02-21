export class FormElementInstance {
    constructor() { }

    FormElementId: string;
    Value: string;

    public static fromJson(json: any): FormElementInstance {
        let result = new FormElementInstance();
        result.FormElementId = json["form_element_id"];
        result.Value = json["value"];
        return result;
    }
}

export class FormInstance {
    constructor() {
        this.Content = [];
    }

    Id: string;
    CreateDateTime: Date;
    UpdateDateTime: Date;
    Performer: string;
    CasePlanId: string;
    CasePlanInstanceId: string;
    CasePlanElementInstanceId: string;
    CaseElementInstanceId: string;
    Status: string;
    FormId: string;
    Content: FormElementInstance[];

    public static fromJson(json: any): FormInstance {
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
        json["content"].forEach(function (elt : any) {
            result.Content.push(FormElementInstance.fromJson(elt));
        });

        return result;
    }
}