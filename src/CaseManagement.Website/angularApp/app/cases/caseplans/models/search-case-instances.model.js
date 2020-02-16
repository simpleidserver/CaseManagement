import { CaseInstance } from './case-instance.model';
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
//# sourceMappingURL=search-case-instances.model.js.map