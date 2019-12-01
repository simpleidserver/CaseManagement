var SearchCaseExecutionStepItem = (function () {
    function SearchCaseExecutionStepItem() {
    }
    SearchCaseExecutionStepItem.fromJson = function (json) {
        var result = new SearchCaseExecutionStepItem();
        result.Id = json["id"];
        result.Name = json["name"];
        result.StartDateTime = json["start_datetime"];
        result.EndDateTime = json["end_datetime"];
        return result;
    };
    return SearchCaseExecutionStepItem;
}());
export { SearchCaseExecutionStepItem };
var SearchCaseExecutionStepsResult = (function () {
    function SearchCaseExecutionStepsResult() {
    }
    SearchCaseExecutionStepsResult.fromJson = function (json) {
        var result = new SearchCaseExecutionStepsResult();
        result.StartIndex = json["start_index"];
        result.Count = json["count"];
        result.TotalLength = json["total_length"];
        var content = [];
        if (json["content"]) {
            json["content"].forEach(function (c) {
                content.push(SearchCaseExecutionStepItem.fromJson(c));
            });
        }
        result.Content = content;
        return result;
    };
    return SearchCaseExecutionStepsResult;
}());
export { SearchCaseExecutionStepsResult };
//# sourceMappingURL=search-case-execution-steps-result.model.js.map