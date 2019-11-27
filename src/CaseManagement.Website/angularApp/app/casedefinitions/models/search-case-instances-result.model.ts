export class SearchCaseInstanceItem {
    Id: string;
    Name: string;
    Status: string;
    TemplateId: string;
    CreateDateTime: Date;

    public static fromJson(json: any): SearchCaseInstanceItem {
        let result = new SearchCaseInstanceItem();
        result.Id = json["id"];
        result.Name = json["name"];
        result.Status = json["status"];
        result.TemplateId = json["template_id"];
        result.CreateDateTime = json["create_datetime"];
        return result;
    }
}

export class SearchCaseInstancesResult {
    StartIndex: number;
    Count: number;
    TotalLength: number;
    Content: SearchCaseInstanceItem[];

    public static fromJson(json: any): SearchCaseInstancesResult
    {
        let result = new SearchCaseInstancesResult();
        result.StartIndex = json["start_index"];
        result.Count = json["count"];
        result.TotalLength = json["total_length"];
        let content: SearchCaseInstanceItem[] = [];
        if (json["content"]) {
            json["content"].forEach(function (c: any) {
                content.push(SearchCaseInstanceItem.fromJson(c));
            });
        }

        result.Content = content;
        return result;
    }
}