export var ActionTypes;
(function (ActionTypes) {
    ActionTypes["START_SEARCH_CASEFILES"] = "[CaseFiles] START_SEARCH_CASEFILES";
    ActionTypes["ERROR_SEARCH_CASEFILES"] = "[CaseFiles] ERROR_SEARCH_CASEFILES";
    ActionTypes["COMPLETE_SEARCH_CASEFILES"] = "[CaseFiles] COMPLETE_SEARCH_CASEFILES";
    ActionTypes["START_SEARCH_CASEFILES_HISTORY"] = "[CasesFiles] START_SEARCH_CASEFILES_HISTORY";
    ActionTypes["ERROR_SEARCH_CASEFILES_HISTORY"] = "[CasesFiles] ERROR_SEARCH_CASEFILES_HISTORY";
    ActionTypes["COMPLETE_SEARCH_CASEFILES_HISTORY"] = "[CasesFiles] COMPLETE_SEARCH_CASEFILES_HISTORY";
    ActionTypes["START_GET_CASEFILE"] = "[CaseFiles] START_GET_CASEFILE";
    ActionTypes["ERROR_GET_CASEFILE"] = "[CaseFiles] ERROR_GET_CASEFILE";
    ActionTypes["COMPLETE_GET_CASEFILE"] = "[CaseFiles] COMPLETE_GET_CASEFILE";
    ActionTypes["ADD_CASEFILE"] = "[CaseFiles] ADD_CASEFILE";
    ActionTypes["ERROR_ADD_CASEFILE"] = "[CaseFiles] ERROR_ADD_CASEFILE";
    ActionTypes["COMPLETE_ADD_CASEFILE"] = "[CaseFiles] COMPLETE_ADD_CASEFILE";
    ActionTypes["PUBLISH_CASEFILE"] = "[CaseFiles] PUBLISH_CASEFILE";
    ActionTypes["ERROR_PUBLISH_CASEFILE"] = "[CaseFiles] ERROR_PUBLISH_CASEFILE";
    ActionTypes["COMPLETE_PUBLISH_CASEFILE"] = "[CaseFiles] COMPLETE_PUBLISH_CASEFILE";
    ActionTypes["UPDATE_CASEFILE"] = "[CaseFiles] UPDATE_CASEFILE";
    ActionTypes["ERROR_UPDATE_CASEFILE"] = "[CaseFiles] ERROR_UPDATE_CASEFILE";
    ActionTypes["COMPLETE_UPDATE_CASEFILE"] = "[CaseFiles] COMPLETE_UPDATE_CASEFILE";
})(ActionTypes || (ActionTypes = {}));
var SearchCaseFiles = (function () {
    function SearchCaseFiles(order, direction, count, startIndex, text) {
        this.order = order;
        this.direction = direction;
        this.count = count;
        this.startIndex = startIndex;
        this.text = text;
        this.type = ActionTypes.START_SEARCH_CASEFILES;
    }
    return SearchCaseFiles;
}());
export { SearchCaseFiles };
var CompleteSearchCaseFiles = (function () {
    function CompleteSearchCaseFiles(content) {
        this.content = content;
        this.type = ActionTypes.COMPLETE_SEARCH_CASEFILES;
    }
    return CompleteSearchCaseFiles;
}());
export { CompleteSearchCaseFiles };
var SearchCaseFilesHistory = (function () {
    function SearchCaseFilesHistory(caseFileId, order, direction, count, startIndex) {
        this.caseFileId = caseFileId;
        this.order = order;
        this.direction = direction;
        this.count = count;
        this.startIndex = startIndex;
        this.type = ActionTypes.START_SEARCH_CASEFILES_HISTORY;
    }
    return SearchCaseFilesHistory;
}());
export { SearchCaseFilesHistory };
var CompleteSearchCaseFilesHistory = (function () {
    function CompleteSearchCaseFilesHistory(content) {
        this.content = content;
        this.type = ActionTypes.COMPLETE_SEARCH_CASEFILES_HISTORY;
    }
    return CompleteSearchCaseFilesHistory;
}());
export { CompleteSearchCaseFilesHistory };
var GetCaseFile = (function () {
    function GetCaseFile(id) {
        this.id = id;
        this.type = ActionTypes.START_GET_CASEFILE;
    }
    return GetCaseFile;
}());
export { GetCaseFile };
var CompleteGetCaseFile = (function () {
    function CompleteGetCaseFile(content) {
        this.content = content;
        this.type = ActionTypes.COMPLETE_GET_CASEFILE;
    }
    return CompleteGetCaseFile;
}());
export { CompleteGetCaseFile };
var AddCaseFile = (function () {
    function AddCaseFile(name, description) {
        this.name = name;
        this.description = description;
        this.type = ActionTypes.ADD_CASEFILE;
    }
    return AddCaseFile;
}());
export { AddCaseFile };
var CompleteAddCaseFile = (function () {
    function CompleteAddCaseFile(id) {
        this.id = id;
        this.type = ActionTypes.COMPLETE_ADD_CASEFILE;
    }
    return CompleteAddCaseFile;
}());
export { CompleteAddCaseFile };
var PublishCaseFile = (function () {
    function PublishCaseFile(id) {
        this.id = id;
        this.type = ActionTypes.PUBLISH_CASEFILE;
    }
    return PublishCaseFile;
}());
export { PublishCaseFile };
var CompletePublishCaseFile = (function () {
    function CompletePublishCaseFile(id) {
        this.id = id;
        this.type = ActionTypes.COMPLETE_PUBLISH_CASEFILE;
    }
    return CompletePublishCaseFile;
}());
export { CompletePublishCaseFile };
var UpdateCaseFile = (function () {
    function UpdateCaseFile(id, name, description, payload) {
        this.id = id;
        this.name = name;
        this.description = description;
        this.payload = payload;
        this.type = ActionTypes.UPDATE_CASEFILE;
    }
    return UpdateCaseFile;
}());
export { UpdateCaseFile };
//# sourceMappingURL=case-files.actions.js.map