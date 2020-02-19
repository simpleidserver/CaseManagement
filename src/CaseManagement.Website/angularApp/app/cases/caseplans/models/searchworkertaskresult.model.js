import { WorkerTask } from './workertask.model';
var SearchWorkerTaskResult = (function () {
    function SearchWorkerTaskResult() {
    }
    SearchWorkerTaskResult.fromJson = function (json) {
        var result = new SearchWorkerTaskResult();
        result.StartIndex = json["start_index"];
        result.Count = json["count"];
        result.TotalLength = json["total_length"];
        var content = [];
        if (json["content"]) {
            json["content"].forEach(function (c) {
                content.push(WorkerTask.fromJson(c));
            });
        }
        result.Content = content;
        return result;
    };
    return SearchWorkerTaskResult;
}());
export { SearchWorkerTaskResult };
//# sourceMappingURL=searchworkertaskresult.model.js.map