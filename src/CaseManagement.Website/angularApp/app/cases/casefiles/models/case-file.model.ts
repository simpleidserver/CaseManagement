export class CaseFile {
    Id: string;
    Name: string;
    Description: string;
    Payload: string;
    Version: number;
    FileId: string;
    Owner: string;
    Status: string;
    CreateDateTime: Date;
    UpdateDateTime: Date;

    public static fromJson(json: any): CaseFile {
        var result = new CaseFile();
        result.Id = json["id"];
        result.Name = json["name"];
        result.Description = json["description"];
        result.Payload = json["payload"];
        result.Version = json["version"];
        result.FileId = json["file_id"];
        result.Owner = json["owner"];
        result.Status = json["status"];
        result.CreateDateTime = json["create_datetime"];
        result.UpdateDateTime = json["update_datetime"];
        return result;
    }
}