var __assign = (this && this.__assign) || function () {
    __assign = Object.assign || function(t) {
        for (var s, i = 1, n = arguments.length; i < n; i++) {
            s = arguments[i];
            for (var p in s) if (Object.prototype.hasOwnProperty.call(s, p))
                t[p] = s[p];
        }
        return t;
    };
    return __assign.apply(this, arguments);
};
import * as fromActions from '../actions/bpmn-files.actions';
export var initialBpmnFileLstState = {
    content: null,
    isLoading: true,
    isErrorLoadOccured: false
};
export var initialBpmnFileState = {
    content: null,
    isLoading: true,
    isErrorLoadOccured: false
};
export function bpmnFileLstReducer(state, action) {
    if (state === void 0) { state = initialBpmnFileLstState; }
    switch (action.type) {
        case fromActions.ActionTypes.COMPLETE_SEARCH_BPMNFILES:
            state.content = action.content;
            return __assign({}, state);
        default:
            return state;
    }
}
export function bpmnFileReducer(state, action) {
    if (state === void 0) { state = initialBpmnFileState; }
    switch (action.type) {
        case fromActions.ActionTypes.COMPLETE_GET_BPMNFILE:
            state.content = action.bpmnFile;
            return __assign({}, state);
        case fromActions.ActionTypes.COMPLETE_UPDATE_BPMNFILE:
            return __assign(__assign({}, state), { content: __assign(__assign({}, state.content), { name: action.name, description: action.description, version: (state.content.version + 1), updateDateTime: new Date() }) });
        case fromActions.ActionTypes.COMPLETE_UPDATE_BPMNFILE_PAYLOAD:
            return __assign(__assign({}, state), { content: __assign(__assign({}, state.content), { version: (state.content.version + 1), payload: action.payload, updateDateTime: new Date() }) });
        default:
            return state;
    }
}
//# sourceMappingURL=bpmnfile.reducers.js.map