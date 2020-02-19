import { FormInstance } from './forminstance.model';
var SearchFormInstanceResult = (function () {
    function SearchFormInstanceResult() {
        this.Content = [];
    }
    SearchFormInstanceResult.fromJson = function (json) {
        var result = new SearchFormInstanceResult();
        result.StartIndex = json["start_index"];
        result.Count = json["count"];
        result.TotalLength = json["total_length"];
        var content = [];
        if (json["content"]) {
            json["content"].forEach(function (c) {
                content.push(FormInstance.fromJson(c));
            });
        }
        result.Content = content;
        return result;
    };
    return SearchFormInstanceResult;
}());
export { SearchFormInstanceResult };
//# sourceMappingURL=searchforminstanceresult.model.js.map