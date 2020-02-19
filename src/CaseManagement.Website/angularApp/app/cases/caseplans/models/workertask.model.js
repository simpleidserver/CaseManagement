var WorkerTask = (function () {
    function WorkerTask() {
    }
    WorkerTask.fromJson = function (json) {
        var result = new WorkerTask();
        result.CaseDefinitionId = json["case_definition_id"];
        result.CaseInstanceId = json["case_instance_id"];
        result.CaseInstanceName = json["case_instance_name"];
        result.CaseElementId = json["case_element_id"];
        result.CaseElementInstanceId = json["case_element_instance_id"];
        result.CaseElementName = json["case_element_name"];
        result.CreateDateTime = json["create_datetime"];
        result.Performer = json["performer"];
        return result;
    };
    return WorkerTask;
}());
export { WorkerTask };
//# sourceMappingURL=workertask.model.js.map