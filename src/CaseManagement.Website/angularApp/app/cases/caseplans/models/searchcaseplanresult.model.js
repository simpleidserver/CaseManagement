import { CasePlan } from './caseplan.model';
var SearchCasePlanResult = (function () {
    function SearchCasePlanResult() {
    }
    SearchCasePlanResult.fromJson = function (json) {
        var result = new SearchCasePlanResult();
        result.StartIndex = json["start_index"];
        result.Count = json["count"];
        result.TotalLength = json["total_length"];
        var content = [];
        if (json["content"]) {
            json["content"].forEach(function (c) {
                content.push(CasePlan.fromJson(c));
            });
        }
        result.Content = content;
        return result;
    };
    return SearchCasePlanResult;
}());
export { SearchCasePlanResult };
//# sourceMappingURL=searchcaseplanresult.model.js.map