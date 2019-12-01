var CaseInstancePlanItem = (function () {
    function CaseInstancePlanItem() {
    }
    CaseInstancePlanItem.fromJson = function (json) {
        var result = new CaseInstancePlanItem();
        result.Id = json["id"];
        result.Name = json["name"];
        result.Status = json["status"];
        return result;
    };
    return CaseInstancePlanItem;
}());
export { CaseInstancePlanItem };
var CaseInstance = (function () {
    function CaseInstance() {
    }
    CaseInstance.fromJson = function (json) {
        var result = new CaseInstance();
        result.Id = json["id"];
        result.Name = json["name"];
        result.Status = json["status"];
        result.TemplateId = json["template_id"];
        result.CreateDateTime = json["create_datetime"];
        var items = [];
        if (json["items"]) {
            json["items"].forEach(function (i) {
                items.push(CaseInstancePlanItem.fromJson(i));
            });
        }
        result.PlanItems = items;
        return result;
    };
    return CaseInstance;
}());
export { CaseInstance };
var SearchCaseInstancesResult = (function () {
    function SearchCaseInstancesResult() {
    }
    SearchCaseInstancesResult.fromJson = function (json) {
        var result = new SearchCaseInstancesResult();
        result.StartIndex = json["start_index"];
        result.Count = json["count"];
        result.TotalLength = json["total_length"];
        var content = [];
        if (json["content"]) {
            json["content"].forEach(function (c) {
                content.push(CaseInstance.fromJson(c));
            });
        }
        result.Content = content;
        return result;
    };
    return SearchCaseInstancesResult;
}());
export { SearchCaseInstancesResult };
//# sourceMappingURL=search-case-instances-result.model.js.map