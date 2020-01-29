import { DailyStatistic } from './dailystatistic.model';

export class SearchDailyStatisticsResult {
    StartIndex: number;
    Count: number;
    TotalLength: number;
    Content: DailyStatistic[];

    public static fromJson(json: any): SearchDailyStatisticsResult
    {
        let result = new SearchDailyStatisticsResult();
        result.StartIndex = json["start_index"];
        result.Count = json["count"];
        result.TotalLength = json["total_length"];
        let content: DailyStatistic[] = [];
        if (json["content"]) {
            json["content"].forEach(function (c: any) {
                content.push(DailyStatistic.fromJson(c));
            });
        }

        result.Content = content;
        return result;
    }
}