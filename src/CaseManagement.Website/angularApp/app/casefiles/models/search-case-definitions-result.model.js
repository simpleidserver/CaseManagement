var SearchCaseDefinitionItem = (function () {
    function SearchCaseDefinitionItem() {
    }
    SearchCaseDefinitionItem.fromJson = function (json) {
        var result = new SearchCaseDefinitionItem();
        result.CreateDateTime = json["create_datetime"];
        result.Id = json["id"];
        result.Name = json["name"];
        return result;
    };
    return SearchCaseDefinitionItem;
}());
export { SearchCaseDefinitionItem };
var SearchCaseDefinitionsResult = (function () {
    function SearchCaseDefinitionsResult() {
    }
    SearchCaseDefinitionsResult.fromJson = function (json) {
        var result = new SearchCaseDefinitionsResult();
        result.StartIndex = json["start_index"];
        result.Count = json["count"];
        result.TotalLength = json["total_length"];
        var content = [];
        if (json["content"]) {
            json["content"].forEach(function (c) {
                content.push(SearchCaseDefinitionItem.fromJson(c));
            });
        }
        result.Content = content;
        return result;
    };
    return SearchCaseDefinitionsResult;
}());
export { SearchCaseDefinitionsResult };
//# sourceMappingURL=search-case-definitions-result.model.js.map