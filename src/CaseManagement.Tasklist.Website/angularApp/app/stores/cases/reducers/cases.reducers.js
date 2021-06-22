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
import * as fromActions from '../actions/cases.actions';
export var initialCaseLstState = {
    content: null,
    isLoading: true,
    isErrorLoadOccured: false
};
export var initialCaseState = {
    content: null,
    isLoading: true,
    isErrorLoadOccured: false
};
export function caseLstReducer(state, action) {
    if (state === void 0) { state = initialCaseLstState; }
    switch (action.type) {
        case fromActions.ActionTypes.COMPLETE_SEARCH_CASES:
            state.content = action.content;
            return __assign({}, state);
        default:
            return state;
    }
}
export function caseReducer(state, action) {
    if (state === void 0) { state = initialCaseState; }
    switch (action.type) {
        case fromActions.ActionTypes.COMPLETE_GET_CASE:
            state.content = action.content;
            return __assign({}, state);
        default:
            return state;
    }
}
//# sourceMappingURL=cases.reducers.js.map