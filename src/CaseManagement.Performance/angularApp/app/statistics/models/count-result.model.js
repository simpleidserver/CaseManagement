var CountResult = (function () {
    function CountResult() {
    }
    CountResult.fromJson = function (json) {
        var result = new CountResult();
        result.Count = json["count"];
        return result;
    };
    return CountResult;
}());
export { CountResult };
//# sourceMappingURL=count-result.model.js.map