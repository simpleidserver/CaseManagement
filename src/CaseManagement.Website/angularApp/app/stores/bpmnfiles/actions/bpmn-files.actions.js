export var ActionTypes;
(function (ActionTypes) {
    ActionTypes["START_SEARCH_BPMNFILES"] = "[BpmnFiles] START_SEARCH_BPMNFILES";
    ActionTypes["ERROR_SEARCH_BPMNFILES"] = "[BpmnFiles] ERROR_SEARCH_BPMNFILES";
    ActionTypes["COMPLETE_SEARCH_BPMNFILES"] = "[BpmnFiles] COMPLETE_SEARCH_BPMNFILES";
    ActionTypes["START_GET_BPMNFILE"] = "[BpmnFiles] START_GET_BPMNFILE";
    ActionTypes["ERROR_GET_BPMNFILE"] = "[BpmnFiles] ERROR_GET_BPMNFILE";
    ActionTypes["COMPLETE_GET_BPMNFILE"] = "[BpmnFiles] COMPLETE_GET_BPMNFILE";
    ActionTypes["UPDATE_BPMNFILE"] = "[BpmnFiles] UPDATE_BPMNFILE";
    ActionTypes["COMPLETE_UPDATE_BPMNFILE"] = "[BpmnFiles] COMPLETE_UPDATE_BPMNFILE";
    ActionTypes["ERROR_UPDATE_BPMNFILE"] = "[BpmnFiles] ERROR_UPDATE_BPMNFILE";
    ActionTypes["PUBLISH_BPMNFILE"] = "[BpmnFiles] PUBLISH_BPMNFILE";
    ActionTypes["COMPLETE_PUBLISH_BPMNFILE"] = "[BpmnFiles] COMPLETE_PUBLISH_BPMNFILE";
    ActionTypes["ERROR_PUBLISH_BPMNFILE"] = "[BpmnFiles] ERROR_PUBLISH_BPMNFILE";
    ActionTypes["UPDATE_BPMNFILE_PAYLOAD"] = "[BpmnFiles] UPDATE_BPMNFILE_PAYLOAD";
    ActionTypes["COMPLETE_UPDATE_BPMNFILE_PAYLOAD"] = "[BpmnFiles] COMPLETE_UPDATE_BPMNFILE_PAYLOAD";
    ActionTypes["ERROR_UPDATE_BPMNFILE_PAYLOAD"] = "[BpmnFiles] ERROR_UPDATE_BPMNFILE_PAYLOAD";
})(ActionTypes || (ActionTypes = {}));
var SearchBpmnFiles = (function () {
    function SearchBpmnFiles(order, direction, count, startIndex, takeLatest, fileId) {
        this.order = order;
        this.direction = direction;
        this.count = count;
        this.startIndex = startIndex;
        this.takeLatest = takeLatest;
        this.fileId = fileId;
        this.type = ActionTypes.START_SEARCH_BPMNFILES;
    }
    return SearchBpmnFiles;
}());
export { SearchBpmnFiles };
var CompleteBpmnFiles = (function () {
    function CompleteBpmnFiles(content) {
        this.content = content;
        this.type = ActionTypes.COMPLETE_SEARCH_BPMNFILES;
    }
    return CompleteBpmnFiles;
}());
export { CompleteBpmnFiles };
var GetBpmnFile = (function () {
    function GetBpmnFile(id) {
        this.id = id;
        this.type = ActionTypes.START_GET_BPMNFILE;
    }
    return GetBpmnFile;
}());
export { GetBpmnFile };
var CompleteBpmnFile = (function () {
    function CompleteBpmnFile(bpmnFile) {
        this.bpmnFile = bpmnFile;
        this.type = ActionTypes.COMPLETE_GET_BPMNFILE;
    }
    return CompleteBpmnFile;
}());
export { CompleteBpmnFile };
var PublishBpmnFile = (function () {
    function PublishBpmnFile(id) {
        this.id = id;
        this.type = ActionTypes.PUBLISH_BPMNFILE;
    }
    return PublishBpmnFile;
}());
export { PublishBpmnFile };
var CompletePublishBpmnFile = (function () {
    function CompletePublishBpmnFile(id) {
        this.id = id;
        this.type = ActionTypes.COMPLETE_PUBLISH_BPMNFILE;
    }
    return CompletePublishBpmnFile;
}());
export { CompletePublishBpmnFile };
var UpdateBpmnFile = (function () {
    function UpdateBpmnFile(id, name, description, payload) {
        this.id = id;
        this.name = name;
        this.description = description;
        this.payload = payload;
        this.type = ActionTypes.UPDATE_BPMNFILE;
    }
    return UpdateBpmnFile;
}());
export { UpdateBpmnFile };
var CompleteUpdateBpmnFile = (function () {
    function CompleteUpdateBpmnFile(id, name, description, payload) {
        this.id = id;
        this.name = name;
        this.description = description;
        this.payload = payload;
        this.type = ActionTypes.COMPLETE_UPDATE_BPMNFILE;
    }
    return CompleteUpdateBpmnFile;
}());
export { CompleteUpdateBpmnFile };
var UpdateBpmnFilePayload = (function () {
    function UpdateBpmnFilePayload(id, payload) {
        this.id = id;
        this.payload = payload;
        this.type = ActionTypes.UPDATE_BPMNFILE_PAYLOAD;
    }
    return UpdateBpmnFilePayload;
}());
export { UpdateBpmnFilePayload };
var CompleteUpdateBpmnFilePayload = (function () {
    function CompleteUpdateBpmnFilePayload(id, payload) {
        this.id = id;
        this.payload = payload;
        this.type = ActionTypes.COMPLETE_UPDATE_BPMNFILE_PAYLOAD;
    }
    return CompleteUpdateBpmnFilePayload;
}());
export { CompleteUpdateBpmnFilePayload };
//# sourceMappingURL=bpmn-files.actions.js.map