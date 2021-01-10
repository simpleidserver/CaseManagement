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
import * as fromActions from '../actions/cmmn-files.actions';
export var initialCmmnFileState = {
    content: null,
    humanTaskDefs: [],
    isLoading: true,
    isErrorLoadOccured: false
};
export var initialCmmnFileLstState = {
    content: null,
    isLoading: true,
    isErrorLoadOccured: false
};
export function cmmnFileReducer(state, action) {
    if (state === void 0) { state = initialCmmnFileState; }
    switch (action.type) {
        case fromActions.ActionTypes.COMPLETE_GET_CMMNFILE:
            state.content = action.content;
            state.humanTaskDefs = action.humanTaskDefs;
            return __assign({}, state);
        case fromActions.ActionTypes.COMPLETE_UPDATE_CMMNFILE:
            return __assign(__assign({}, state), { content: __assign(__assign({}, state.content), { name: action.name, description: action.description, payload: action.xml, version: (state.content.version + 1), updateDateTime: new Date() }) });
        case fromActions.ActionTypes.COMPLETE_UPDATE_CMMNFILE_PAYLOAD:
            return __assign(__assign({}, state), { content: __assign(__assign({}, state.content), { payload: action.payload }) });
        case fromActions.ActionTypes.COMPLETE_PUBLISH_CMMNFILE:
            return __assign(__assign({}, state), { content: __assign(__assign({}, state.content), { status: 'Published' }) });
        default:
            return state;
    }
}
export function cmmnFileLstReducer(state, action) {
    if (state === void 0) { state = initialCmmnFileLstState; }
    switch (action.type) {
        case fromActions.ActionTypes.COMPLETE_SEARCH_CMMNFILES:
            state.content = action.content;
            return __assign({}, state);
        default:
            return state;
    }
}
//# sourceMappingURL=cmmnfile.reducers.js.map