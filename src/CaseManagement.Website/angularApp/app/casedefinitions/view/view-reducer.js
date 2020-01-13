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
import { ActionTypes } from './view-actions';
var initialCaseDefAction = {
    caseDefinition: null,
    caseFile: null,
    caseDefinitionHistory: null,
    isLoading: true,
    isErrorLoadOccured: false
};
var initialCaseInstancesAction = {
    content: null,
    isErrorLoadOccured: false,
    isLoading: true
};
var initialFormInstancesAction = {
    content: null,
    isErrorLoadOccured: false,
    isLoading: true
};
var initialCaseActivationsAction = {
    content: null,
    isErrorLoadOccured: false,
    isLoading: true
};
export function caseDefinitionReducer(state, action) {
    if (state === void 0) { state = initialCaseDefAction; }
    switch (action.type) {
        case ActionTypes.CASEDEFINITIONLOADED:
            var caseDefsLoadedAction = action;
            state.caseDefinition = caseDefsLoadedAction.caseDefinition;
            state.caseFile = caseDefsLoadedAction.caseFile;
            state.caseDefinitionHistory = caseDefsLoadedAction.caseDefinitionHistory;
            state.isLoading = false;
            state.isErrorLoadOccured = false;
            return __assign({}, state);
        case ActionTypes.ERRORLOADCASEDEFINITION:
            state.isErrorLoadOccured = true;
            state.isLoading = false;
            return __assign({}, state);
        default:
            return state;
    }
}
export function caseInstancesReducer(state, action) {
    if (state === void 0) { state = initialCaseInstancesAction; }
    switch (action.type) {
        case ActionTypes.CASEINSTANCESLOADED:
            var caseInstancesLoadedAction = action;
            state.content = caseInstancesLoadedAction.result;
            state.isLoading = false;
            state.isErrorLoadOccured = false;
            return __assign({}, state);
        case ActionTypes.ERRORLOADCASEINSTANCES:
            state.isErrorLoadOccured = true;
            state.isLoading = false;
            return __assign({}, state);
        default:
            return state;
    }
}
export function formInstancesReducer(state, action) {
    if (state === void 0) { state = initialFormInstancesAction; }
    switch (action.type) {
        case ActionTypes.CASEFORMINSTANCESLOADED:
            var caseInstancesLoadedAction = action;
            state.content = caseInstancesLoadedAction.result;
            state.isLoading = false;
            state.isErrorLoadOccured = false;
            return __assign({}, state);
        case ActionTypes.ERRORLOADCASEFORMINSTANCES:
            state.isErrorLoadOccured = true;
            state.isLoading = false;
            return __assign({}, state);
        default:
            return state;
    }
}
export function caseActivationsReducer(state, action) {
    if (state === void 0) { state = initialCaseActivationsAction; }
    switch (action.type) {
        case ActionTypes.CASEACTIVATIONSLOADED:
            var caseActivationsLoadedAction = action;
            state.content = caseActivationsLoadedAction.result;
            state.isLoading = false;
            state.isErrorLoadOccured = false;
            return __assign({}, state);
        case ActionTypes.ERRORLOADCASEACTIVATIONS:
            state.isErrorLoadOccured = true;
            state.isLoading = false;
            return __assign({}, state);
        default:
            return state;
    }
}
//# sourceMappingURL=view-reducer.js.map