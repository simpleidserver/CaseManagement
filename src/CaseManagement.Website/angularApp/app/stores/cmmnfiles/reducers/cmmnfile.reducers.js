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
import * as fromActions from '../actions/case-files.actions';
export var initialCaseFileState = {
    content: null,
    isLoading: true,
    isErrorLoadOccured: false
};
export var initialCaseFileLstState = {
    content: null,
    isLoading: true,
    isErrorLoadOccured: false
};
export var initialCaseFileHistoryLstState = {
    content: null,
    isLoading: true,
    isErrorLoadOccured: false
};
export function caseFileReducer(state, action) {
    if (state === void 0) { state = initialCaseFileState; }
    switch (action.type) {
        case fromActions.ActionTypes.COMPLETE_GET_CASEFILE:
            state.content = action.content;
            return __assign({}, state);
        default:
            return state;
    }
}
export function caseFileLstReducer(state, action) {
    if (state === void 0) { state = initialCaseFileLstState; }
    switch (action.type) {
        case fromActions.ActionTypes.COMPLETE_SEARCH_CASEFILES:
            state.content = action.content;
            return __assign({}, state);
        default:
            return state;
    }
}
export function caseFileHistoryLstReducer(state, action) {
    if (state === void 0) { state = initialCaseFileHistoryLstState; }
    switch (action.type) {
        case fromActions.ActionTypes.COMPLETE_SEARCH_CASEFILES_HISTORY:
            state.content = action.content;
            return __assign({}, state);
        default:
            return state;
    }
}
//# sourceMappingURL=casefile.reducers.js.map