import { createSelector } from '@ngrx/store';
import * as fromCaseFile from './casefiles/reducers/casefile.reducers';
import * as fromCasePlanInstance from './caseplaninstances/reducers/caseplaninstance.reducers';
import * as fromCasePlan from './caseplans/reducers/caseplan.reducers';
import * as fromHumanTask from './humantaskdefs/reducers/humantaskdef.reducers';
export var selectCasePlan = function (state) { return state.casePlan; };
export var selectCasePlanLst = function (state) { return state.casePlanLst; };
export var selectCasePlanHistoryLst = function (state) { return state.casePlanHistoryLst; };
export var selectCaseFile = function (state) { return state.caseFile; };
export var selectCaseFileLst = function (state) { return state.caseFileLst; };
export var selectCaseFileHistoryLst = function (state) { return state.caseFileHistoryLst; };
export var selectCasePlanInstance = function (state) { return state.casePlanInstance; };
export var selectCasePlanInstanceLst = function (state) { return state.casePlanInstanceLst; };
export var selectHumanTask = function (state) { return state.humanTask; };
export var selectCasePlanResult = createSelector(selectCasePlan, function (state) {
    if (!state || state.content == null) {
        return null;
    }
    return state.content;
});
export var selectCasePlanLstResult = createSelector(selectCasePlanLst, function (state) {
    if (!state || state.content == null) {
        return null;
    }
    return state.content;
});
export var selectCasePlanHistoryLstResult = createSelector(selectCasePlanHistoryLst, function (state) {
    if (!state || state.content == null) {
        return null;
    }
    return state.content;
});
export var selectCaseFileResult = createSelector(selectCaseFile, function (state) {
    if (!state || state.content == null) {
        return null;
    }
    return state.content;
});
export var selectCaseFileLstResult = createSelector(selectCaseFileLst, function (state) {
    if (!state || state.content == null) {
        return null;
    }
    return state.content;
});
export var selectCaseFileHistoryLstResult = createSelector(selectCaseFileHistoryLst, function (state) {
    if (!state || state.content == null) {
        return null;
    }
    return state.content;
});
export var selectCasePlanInstanceResult = createSelector(selectCasePlanInstance, function (state) {
    if (!state || state.content == null) {
        return null;
    }
    return state.content;
});
export var selectCasePlanInstanceLstResult = createSelector(selectCasePlanInstanceLst, function (state) {
    if (!state || state.content == null) {
        return null;
    }
    return state.content;
});
export var selectHumanTaskResult = createSelector(selectHumanTask, function (state) {
    if (!state || state.content == null) {
        return null;
    }
    return state.content;
});
export var appReducer = {
    casePlan: fromCasePlan.casePlanReducer,
    casePlanLst: fromCasePlan.casePlanLstReducer,
    casePlanHistoryLst: fromCasePlan.casePlanHistoryLstReducer,
    caseFile: fromCaseFile.caseFileReducer,
    caseFileLst: fromCaseFile.caseFileLstReducer,
    caseFileHistoryLst: fromCaseFile.caseFileHistoryLstReducer,
    casePlanInstance: fromCasePlanInstance.casePlanInstanceReducer,
    casePlanInstanceLst: fromCasePlanInstance.casePlanInstanceLstReducer,
    humanTask: fromHumanTask.humanTaskDefReducer
};
//# sourceMappingURL=appstate.js.map