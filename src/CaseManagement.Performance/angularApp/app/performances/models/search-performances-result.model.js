import { Performance } from './performance.model';
var SearchPerformancesResult = (function () {
    function SearchPerformancesResult() {
    }
    SearchPerformancesResult.fromJson = function (json) {
        var result = new SearchPerformancesResult();
        result.StartIndex = json["start_index"];
        result.Count = json["count"];
        result.TotalLength = json["total_length"];
        var content = [];
        if (json["content"]) {
            json["content"].forEach(function (c) {
                content.push(Performance.fromJson(c));
            });
        }
        result.Content = content;
        return result;
    };
    return SearchPerformancesResult;
}());
export { SearchPerformancesResult };
//# sourceMappingURL=search-performances-result.model.js.map