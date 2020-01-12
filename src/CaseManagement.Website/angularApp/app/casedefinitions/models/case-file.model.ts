export class CaseFile {
    Id: string;
    Name: string;
    Description: string;
    CreateDateTime: Date;
    Payload: string;

    public static fromJson(json: any): CaseFile {
        var result = new CaseFile();
        result.Id = json["id"];
        result.Name = json["name"];
        result.Description = json["description"];
        result.Payload = json["payload"];
        result.CreateDateTime = json["create_datetime"];
        return result;
    }
}