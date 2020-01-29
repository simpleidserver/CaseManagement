var Performance = (function () {
    function Performance() {
    }
    Performance.fromJson = function (json) {
        var result = new Performance();
        result.CaptureDateTime = json["datetime"];
        result.MachineName = json["machine_name"];
        result.NbWorkingThreads = json["nb_working_threads"];
        result.MemoryConsumedMB = json["memory_consumed_mb"];
        return result;
    };
    return Performance;
}());
export { Performance };
//# sourceMappingURL=performance.model.js.map