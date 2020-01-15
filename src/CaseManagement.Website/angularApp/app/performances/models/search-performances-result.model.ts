import { Performance } from './performance.model';

export class SearchPerformancesResult {
    StartIndex: number;
    Count: number;
    TotalLength: number;
    Content: Performance[];

    public static fromJson(json: any): SearchPerformancesResult
    {
        let result = new SearchPerformancesResult();
        result.StartIndex = json["start_index"];
        result.Count = json["count"];
        result.TotalLength = json["total_length"];
        let content: Performance[] = [];
        if (json["content"]) {
            json["content"].forEach(function (c: any) {
                content.push(Performance.fromJson(c));
            });
        }

        result.Content = content;
        return result;
    }
}