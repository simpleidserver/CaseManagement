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
import * as fromActions from '../actions/caseplan.actions';
export var initialCasePlanState = {
    content: null,
    isLoading: true,
    isErrorLoadOccured: false
};
export var initialSearchCasePlanState = {
    content: null,
    isLoading: true,
    isErrorLoadOccured: false
};
export var initialSearchCasePlanHistoryState = {
    content: null,
    isLoading: true,
    isErrorLoadOccured: false
};
export function casePlanReducer(state, action) {
    if (state === void 0) { state = initialCasePlanState; }
    switch (action.type) {
        case fromActions.ActionTypes.COMPLETE_GET:
            state.content = action.content;
            return __assign({}, state);
        default:
            return state;
    }
}
export function casePlanLstReducer(state, action) {
    if (state === void 0) { state = initialSearchCasePlanState; }
    switch (action.type) {
        case fromActions.ActionTypes.COMPLETE_SEARCH:
            state.content = action.content;
            return __assign({}, state);
        default:
            return state;
    }
}
export function casePlanHistoryLstReducer(state, action) {
    if (state === void 0) { state = initialSearchCasePlanHistoryState; }
    switch (action.type) {
        case fromActions.ActionTypes.COMPLETE_SEARCH_HISTORY:
            state.content = action.content;
            return __assign({}, state);
        default:
            return state;
    }
}
//# sourceMappingURL=caseplan.reducers.js.map