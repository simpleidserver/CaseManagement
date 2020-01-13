import { CaseActivation } from './case-activation.model';
var SearchCaseActivationsResult = (function () {
    function SearchCaseActivationsResult() {
    }
    SearchCaseActivationsResult.fromJson = function (json) {
        var result = new SearchCaseActivationsResult();
        result.StartIndex = json["start_index"];
        result.Count = json["count"];
        result.TotalLength = json["total_length"];
        var content = [];
        if (json["content"]) {
            json["content"].forEach(function (c) {
                content.push(CaseActivation.fromJson(c));
            });
        }
        result.Content = content;
        return result;
    };
    return SearchCaseActivationsResult;
}());
export { SearchCaseActivationsResult };
//# sourceMappingURL=search-case-activations-result.model.js.map