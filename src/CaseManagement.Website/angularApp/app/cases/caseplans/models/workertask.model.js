var WorkerTask = (function () {
    function WorkerTask() {
    }
    WorkerTask.fromJson = function (json) {
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
    };
    return WorkerTask;
}());
export { WorkerTask };
//# sourceMappingURL=workertask.model.js.map