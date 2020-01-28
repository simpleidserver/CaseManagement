import { DailyStatistic } from './dailystatistic.model';
var SearchDailyStatisticsResult = (function () {
    function SearchDailyStatisticsResult() {
    }
    SearchDailyStatisticsResult.fromJson = function (json) {
        var result = new SearchDailyStatisticsResult();
        result.StartIndex = json["start_index"];
        result.Count = json["count"];
        result.TotalLength = json["total_length"];
        var content = [];
        if (json["content"]) {
            json["content"].forEach(function (c) {
                content.push(DailyStatistic.fromJson(c));
            });
        }
        result.Content = content;
        return result;
    };
    return SearchDailyStatisticsResult;
}());
export { SearchDailyStatisticsResult };
//# sourceMappingURL=search-dailystatistics-result.model.js.map