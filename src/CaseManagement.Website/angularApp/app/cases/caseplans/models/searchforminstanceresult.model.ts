import { FormInstance } from './forminstance.model';

export class SearchFormInstanceResult {
    constructor() {
        this.Content = [];
    }

    StartIndex: number;
    Count: number;
    TotalLength: number;
    Content: FormInstance[];

    public static fromJson(json: any): SearchFormInstanceResult
    {
        let result = new SearchFormInstanceResult();
        result.StartIndex = json["start_index"];
        result.Count = json["count"];
        result.TotalLength = json["total_length"];
        let content: FormInstance[] = [];
        if (json["content"]) {
            json["content"].forEach(function (c: any) {
                content.push(FormInstance.fromJson(c));
            });
        }

        result.Content = content;
        return result;
    }
}