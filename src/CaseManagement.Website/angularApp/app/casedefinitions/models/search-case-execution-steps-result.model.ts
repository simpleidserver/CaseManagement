export class SearchCaseExecutionStepItem {
    Id: string;
    Name: string;
    StartDateTime: Date;
    EndDateTime: Date;

    public static fromJson(json: any): SearchCaseExecutionStepItem {
        let result = new SearchCaseExecutionStepItem();
        result.Id = json["id"];
        result.Name = json["name"];
        result.StartDateTime = json["start_datetime"];
        result.EndDateTime = json["end_datetime"];
        return result;
    }
}

export class SearchCaseExecutionStepsResult {
    StartIndex: number;
    Count: number;
    TotalLength: number;
    Content: SearchCaseExecutionStepItem[];

    public static fromJson(json: any): SearchCaseExecutionStepsResult
    {
        let result = new SearchCaseExecutionStepsResult();
        result.StartIndex = json["start_index"];
        result.Count = json["count"];
        result.TotalLength = json["total_length"];
        let content: SearchCaseExecutionStepItem[] = [];
        if (json["content"]) {
            json["content"].forEach(function (c: any) {
                content.push(SearchCaseExecutionStepItem.fromJson(c));
            });
        }

        result.Content = content;
        return result;
    }
}