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
import * as fromActions from '../actions/cmmn-instances.actions';
export var initialCmmnPlanInstanceState = {
    content: null,
    isLoading: true,
    isErrorLoadOccured: false
};
export var initialCmmnPlanInstanceLstState = {
    content: null,
    isLoading: true,
    isErrorLoadOccured: false
};
export function cmmnPlanInstanceLstReducer(state, action) {
    if (state === void 0) { state = initialCmmnPlanInstanceLstState; }
    switch (action.type) {
        case fromActions.ActionTypes.COMPLETE_SEARCH_CMMN_PLANINSTANCE:
            state.content = action.content;
            return __assign({}, state);
        default:
            return state;
    }
}
//# sourceMappingURL=cmmn-planinstance.reducers.js.map