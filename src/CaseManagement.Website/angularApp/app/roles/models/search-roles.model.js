import { Role } from './role.model';
var SearchRolesResult = (function () {
    function SearchRolesResult() {
    }
    SearchRolesResult.fromJson = function (json) {
        var result = new SearchRolesResult();
        result.StartIndex = json["start_index"];
        result.Count = json["count"];
        result.TotalLength = json["total_length"];
        var content = [];
        if (json["content"]) {
            json["content"].forEach(function (c) {
                content.push(Role.fromJson(c));
            });
        }
        result.Content = content;
        return result;
    };
    return SearchRolesResult;
}());
export { SearchRolesResult };
//# sourceMappingURL=search-roles.model.js.map