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
import { ActionTypes } from './case-def-actions';
var initialCaseDefState = {
    isCaseDefinitionLoading: true,
    isCaseDefinitionErrorLoadOccured: false,
    caseDefinitionContent: null,
    isCaseInstancesLoading: true,
    isCaseInstancesErrorLoadOccured: false,
    caseInstancesContent: null
};
export function reducer(state, action) {
    if (state === void 0) { state = initialCaseDefState; }
    switch (action.type) {
        case ActionTypes.CASEDEFLOADED:
            var caseDefLoadedAction = action;
            state.caseDefinitionContent = caseDefLoadedAction.result;
            state.isCaseDefinitionLoading = false;
            state.isCaseDefinitionErrorLoadOccured = false;
            return __assign({}, state);
        case ActionTypes.ERRORLOADCASEDEF:
            state.isCaseDefinitionErrorLoadOccured = true;
            state.isCaseDefinitionLoading = false;
            return __assign({}, state);
        case ActionTypes.CASEINSTANCESLOADED:
            var caseInstancesLoadedAction = action;
            state.caseInstancesContent = caseInstancesLoadedAction.result;
            state.isCaseInstancesLoading = false;
            state.isCaseInstancesErrorLoadOccured = false;
            return __assign({}, state);
        case ActionTypes.ERRORLOADCASEINSTANCES:
            state.isCaseInstancesErrorLoadOccured = true;
            state.isCaseInstancesLoading = false;
            return __assign({}, state);
        default:
            return state;
    }
}
//# sourceMappingURL=case-def-reducer.js.map