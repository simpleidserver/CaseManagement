var Role = (function () {
    function Role() {
    }
    Role.fromJson = function (json) {
        var result = new Role();
        result.Id = json["id"];
        result.IsDeleted = json["is_deleted"];
        result.Users = json["users"];
        result.CreateDateTime = json["create_datetime"];
        result.UpdateDateTime = json["update_datetime"];
        return result;
    };
    return Role;
}());
export { Role };
//# sourceMappingURL=role.model.js.map