export class Role {
    Id: string;
    IsDeleted: boolean;
    Users: Array<string>;
    CreateDateTime: Date;
    UpdateDateTime: Date;

    public static fromJson(json: any): Role {
        var result = new Role();
        result.Id = json["id"];
        result.IsDeleted = json["is_deleted"];
        result.Users = json["users"];
        result.CreateDateTime = json["create_datetime"];
        result.UpdateDateTime = json["update_datetime"];
        return result;
    }
}