import { CaseFormInstance } from './case-form-instance.model';
var SearchCaseFormInstancesResult = (function () {
    function SearchCaseFormInstancesResult() {
        this.Content = [];
    }
    SearchCaseFormInstancesResult.fromJson = function (json) {
        var result = new SearchCaseFormInstancesResult();
        result.StartIndex = json["start_index"];
        result.Count = json["count"];
        result.TotalLength = json["total_length"];
        var content = [];
        if (json["content"]) {
            json["content"].forEach(function (c) {
                content.push(CaseFormInstance.fromJson(c));
            });
        }
        result.Content = content;
        return result;
    };
    return SearchCaseFormInstancesResult;
}());
export { SearchCaseFormInstancesResult };
//# sourceMappingURL=search-case-form-instances-result.model.js.map