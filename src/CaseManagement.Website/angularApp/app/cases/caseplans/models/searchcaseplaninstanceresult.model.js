import { CasePlanInstance } from './caseplaninstance.model';
var SearchCasePlanInstanceResult = (function () {
    function SearchCasePlanInstanceResult() {
    }
    SearchCasePlanInstanceResult.fromJson = function (json) {
        var result = new SearchCasePlanInstanceResult();
        result.StartIndex = json["start_index"];
        result.Count = json["count"];
        result.TotalLength = json["total_length"];
        var content = [];
        if (json["content"]) {
            json["content"].forEach(function (c) {
                content.push(CasePlanInstance.fromJson(c));
            });
        }
        result.Content = content;
        return result;
    };
    return SearchCasePlanInstanceResult;
}());
export { SearchCasePlanInstanceResult };
//# sourceMappingURL=searchcaseplaninstanceresult.model.js.map