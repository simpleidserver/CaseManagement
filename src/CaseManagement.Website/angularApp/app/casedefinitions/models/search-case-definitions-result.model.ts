export class SearchCaseDefinitionItem {
    Id: string;
    Name: string;
    CreateDateTime: Date;

    public static fromJson(json: any): SearchCaseDefinitionItem {
        let result = new SearchCaseDefinitionItem();
        result.CreateDateTime = json["create_datetime"];
        result.Id = json["id"];
        result.Name = json["name"];
        return result;
    }
}

export class SearchCaseDefinitionsResult {
    StartIndex: number;
    Count: number;
    TotalLength: number;
    Content: SearchCaseDefinitionItem[];

    public static fromJson(json: any): SearchCaseDefinitionsResult
    {
        let result = new SearchCaseDefinitionsResult();
        result.StartIndex = json["start_index"];
        result.Count = json["count"];
        result.TotalLength = json["total_length"];
        let content: SearchCaseDefinitionItem[] = [];
        if (json["content"]) {
            json["content"].forEach(function (c: any) {
                content.push(SearchCaseDefinitionItem.fromJson(c));
            });
        }

        result.Content = content;
        return result;
    }
}