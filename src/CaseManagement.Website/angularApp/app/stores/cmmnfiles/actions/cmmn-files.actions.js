export var ActionTypes;
(function (ActionTypes) {
    ActionTypes["START_SEARCH_CMMNFILES"] = "[CmmnFiles] START_SEARCH_CMMNFILES";
    ActionTypes["ERROR_SEARCH_CMMNFILES"] = "[CmmnFiles] ERROR_SEARCH_CMMNFILES";
    ActionTypes["COMPLETE_SEARCH_CMMNFILES"] = "[CmmnFiles] COMPLETE_SEARCH_CASEFILES";
    ActionTypes["START_SEARCH_CASEFILES_HISTORY"] = "[CmmnFiles] START_SEARCH_CASEFILES_HISTORY";
    ActionTypes["ERROR_SEARCH_CASEFILES_HISTORY"] = "[CmmnFiles] ERROR_SEARCH_CASEFILES_HISTORY";
    ActionTypes["COMPLETE_SEARCH_CASEFILES_HISTORY"] = "[CmmnFiles] COMPLETE_SEARCH_CASEFILES_HISTORY";
    ActionTypes["START_GET_CMMNFILE"] = "[CmmnFiles] START_GET_CMMNFILE";
    ActionTypes["ERROR_GET_CMMNFILE"] = "[CmmnFiles] ERROR_GET_CMMNFILE";
    ActionTypes["COMPLETE_GET_CMMNFILE"] = "[CmmnFiles] COMPLETE_GET_CMMNFILE";
    ActionTypes["ADD_CMMNFILE"] = "[CmmnFiles] ADD_CMMNFILE";
    ActionTypes["ERROR_ADD_CMMNFILE"] = "[CmmnFiles] ERROR_ADD_CMMNFILE";
    ActionTypes["COMPLETE_ADD_CMMNFILE"] = "[CmmnFiles] COMPLETE_ADD_CMMNFILE";
    ActionTypes["PUBLISH_CMMNFILE"] = "[CmmnFiles] PUBLISH_CMMNFILE";
    ActionTypes["ERROR_PUBLISH_CMMNFILE"] = "[CmmnFiles] ERROR_PUBLISH_CMMNFILE";
    ActionTypes["COMPLETE_PUBLISH_CMMNFILE"] = "[CmmnFiles] COMPLETE_PUBLISH_CMMNFILE";
    ActionTypes["UPDATE_CMMNFILE"] = "[CmmnFiles] UPDATE_CMMNFILE";
    ActionTypes["ERROR_UPDATE_CMMNFILE"] = "[CmmnFiles] ERROR_UPDATE_CMMNFILE";
    ActionTypes["COMPLETE_UPDATE_CMMNFILE"] = "[CmmnFiles] COMPLETE_UPDATE_CMMNFILE";
    ActionTypes["UPDATE_CMMNFILE_PAYLOAD"] = "[CmmnFiles] UPDATE_CMMNFILE_PAYLOAD";
    ActionTypes["COMPLETE_UPDATE_CMMNFILE_PAYLOAD"] = "[CmmnFiles] COMPLETE_UPDATE_CMMNFILE_PAYLOAD";
    ActionTypes["ERROR_UPDATE_CMMNFILE_PAYLOAD"] = "[CmmnFiles] ERROR_UPDATE_CMMNFILE_PAYLOAD";
})(ActionTypes || (ActionTypes = {}));
var SearchCmmnFiles = (function () {
    function SearchCmmnFiles(order, direction, count, startIndex, text, caseFileId, takeLatest) {
        this.order = order;
        this.direction = direction;
        this.count = count;
        this.startIndex = startIndex;
        this.text = text;
        this.caseFileId = caseFileId;
        this.takeLatest = takeLatest;
        this.type = ActionTypes.START_SEARCH_CMMNFILES;
    }
    return SearchCmmnFiles;
}());
export { SearchCmmnFiles };
var CompleteSearchCmmnFiles = (function () {
    function CompleteSearchCmmnFiles(content) {
        this.content = content;
        this.type = ActionTypes.COMPLETE_SEARCH_CMMNFILES;
    }
    return CompleteSearchCmmnFiles;
}());
export { CompleteSearchCmmnFiles };
var GetCmmnFile = (function () {
    function GetCmmnFile(id) {
        this.id = id;
        this.type = ActionTypes.START_GET_CMMNFILE;
    }
    return GetCmmnFile;
}());
export { GetCmmnFile };
var CompleteGetCmmnFile = (function () {
    function CompleteGetCmmnFile(humanTaskDefs, content) {
        this.humanTaskDefs = humanTaskDefs;
        this.content = content;
        this.type = ActionTypes.COMPLETE_GET_CMMNFILE;
    }
    return CompleteGetCmmnFile;
}());
export { CompleteGetCmmnFile };
var AddCmmnFile = (function () {
    function AddCmmnFile(name, description) {
        this.name = name;
        this.description = description;
        this.type = ActionTypes.ADD_CMMNFILE;
    }
    return AddCmmnFile;
}());
export { AddCmmnFile };
var CompleteAddCmmnFile = (function () {
    function CompleteAddCmmnFile(id) {
        this.id = id;
        this.type = ActionTypes.COMPLETE_ADD_CMMNFILE;
    }
    return CompleteAddCmmnFile;
}());
export { CompleteAddCmmnFile };
var PublishCmmnFile = (function () {
    function PublishCmmnFile(id) {
        this.id = id;
        this.type = ActionTypes.PUBLISH_CMMNFILE;
    }
    return PublishCmmnFile;
}());
export { PublishCmmnFile };
var CompletePublishCmmnFile = (function () {
    function CompletePublishCmmnFile(id) {
        this.id = id;
        this.type = ActionTypes.COMPLETE_PUBLISH_CMMNFILE;
    }
    return CompletePublishCmmnFile;
}());
export { CompletePublishCmmnFile };
var UpdateCmmnFile = (function () {
    function UpdateCmmnFile(id, name, description, xml) {
        this.id = id;
        this.name = name;
        this.description = description;
        this.xml = xml;
        this.type = ActionTypes.UPDATE_CMMNFILE;
    }
    return UpdateCmmnFile;
}());
export { UpdateCmmnFile };
var CompleteUpdateCmmnFile = (function () {
    function CompleteUpdateCmmnFile(id, name, description, xml) {
        this.id = id;
        this.name = name;
        this.description = description;
        this.xml = xml;
        this.type = ActionTypes.COMPLETE_UPDATE_CMMNFILE;
    }
    return CompleteUpdateCmmnFile;
}());
export { CompleteUpdateCmmnFile };
var UpdateCmmnFilePayload = (function () {
    function UpdateCmmnFilePayload(id, payload) {
        this.id = id;
        this.payload = payload;
        this.type = ActionTypes.UPDATE_CMMNFILE_PAYLOAD;
    }
    return UpdateCmmnFilePayload;
}());
export { UpdateCmmnFilePayload };
var CompleteUpdateCmmnFilePayload = (function () {
    function CompleteUpdateCmmnFilePayload(id, payload) {
        this.id = id;
        this.payload = payload;
        this.type = ActionTypes.COMPLETE_UPDATE_CMMNFILE_PAYLOAD;
    }
    return CompleteUpdateCmmnFilePayload;
}());
export { CompleteUpdateCmmnFilePayload };
//# sourceMappingURL=cmmn-files.actions.js.map