import { CaseInstance } from './case-instance.model';

export class SearchCaseInstancesResult {
    StartIndex: number;
    Count: number;
    TotalLength: number;
    Content: CaseInstance[];

    public static fromJson(json: any): SearchCaseInstancesResult
    {
        let result = new SearchCaseInstancesResult();
        result.StartIndex = json["start_index"];
        result.Count = json["count"];
        result.TotalLength = json["total_length"];
        let content: CaseInstance[] = [];
        if (json["content"]) {
            json["content"].forEach(function (c: any) {
                content.push(CaseInstance.fromJson(c));
            });
        }

        result.Content = content;
        return result;
    }
}