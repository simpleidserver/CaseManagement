export class WorkerTask {
    CasePlanId: string;
    CasePlanInstanceId: string;
    CasePlanElementInstanceId: string;
    CreateDateTime: Date;
    UpdateDateTime: Date;
    Performer: string;
    Type: string;
    Status: string;

    public static fromJson(json: any): WorkerTask {
        var result = new WorkerTask();
        result.CasePlanId = json["case_plan_id"];
        result.CasePlanInstanceId = json["case_plan_instance_id"];
        result.CasePlanElementInstanceId = json["case_plan_element_instance_id"];
        result.CreateDateTime = json["create_datetime"];
        result.UpdateDateTime = json["update_datetime"];
        result.Performer = json["performer"];
        result.Type = json["type"];
        result.Status = json["status"];
        return result;
    }
}