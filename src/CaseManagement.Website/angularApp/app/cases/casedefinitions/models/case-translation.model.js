var CaseTranslation = (function () {
    function CaseTranslation() {
    }
    CaseTranslation.fromJson = function (json) {
        var result = new CaseTranslation();
        var splitted = json.split('#');
        result.Language = splitted[0];
        result.Value = splitted[1];
        return result;
    };
    return CaseTranslation;
}());
export { CaseTranslation };
//# sourceMappingURL=case-translation.model.js.map