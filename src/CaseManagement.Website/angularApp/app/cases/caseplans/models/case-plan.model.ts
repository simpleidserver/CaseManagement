export class CasePlan {
    Id: string;
    Name: string;
    Description: string;
    CaseFile: string;
    CreateDateTime: Date;

    public static fromJson(json: any): CasePlan {
        var result = new CasePlan();
        result.Id = json["id"];
        result.Name = json["name"];
        result.Description = json["description"];
        result.CaseFile = json["case_file"];
        result.CreateDateTime = json["create_datetime"];
        return result;
    }
}