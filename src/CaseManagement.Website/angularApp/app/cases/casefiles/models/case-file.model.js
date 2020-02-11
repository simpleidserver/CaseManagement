var CaseFile = (function () {
    function CaseFile() {
    }
    CaseFile.fromJson = function (json) {
        var result = new CaseFile();
        result.Id = json["id"];
        result.Name = json["name"];
        result.Description = json["description"];
        result.Payload = json["payload"];
        result.CreateDateTime = json["create_datetime"];
        result.UpdateDateTime = json["update_datetime"];
        return result;
    };
    return CaseFile;
}());
export { CaseFile };
//# sourceMappingURL=case-file.model.js.map