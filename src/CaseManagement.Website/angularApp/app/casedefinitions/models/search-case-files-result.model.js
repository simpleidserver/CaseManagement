import { CaseFile } from './case-file.model';
var SearchCaseFilesResult = (function () {
    function SearchCaseFilesResult() {
    }
    SearchCaseFilesResult.fromJson = function (json) {
        var result = new SearchCaseFilesResult();
        result.StartIndex = json["start_index"];
        result.Count = json["count"];
        result.TotalLength = json["total_length"];
        var content = [];
        if (json["content"]) {
            json["content"].forEach(function (c) {
                content.push(CaseFile.fromJson(c));
            });
        }
        result.Content = content;
        return result;
    };
    return SearchCaseFilesResult;
}());
export { SearchCaseFilesResult };
//# sourceMappingURL=search-case-files-result.model.js.map