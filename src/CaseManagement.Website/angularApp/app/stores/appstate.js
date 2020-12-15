import { createSelector } from '@ngrx/store';
import * as fromBpmnFiles from './bpmnfiles/reducers/bpmnfile.reducers';
import * as fromBpmnInstances from './bpmninstances/reducers/bpmninstance.reducers';
import * as fromCmmnPlan from './cmmnplans/reducers/cmmnplan.reducers';
import * as fromCmmnFile from './cmmnfiles/reducers/cmmnfile.reducers';
import * as fromCmmnPlanInstance from './cmmninstances/reducers/cmmn-planinstance.reducers';
import * as fromHumanTask from './humantaskdefs/reducers/humantaskdef.reducers';
export var selectCmmnPlan = function (state) { return state.cmmnPlan; };
export var selectCmmnPlanLst = function (state) { return state.cmmnPlanLst; };
export var selectCmmnFile = function (state) { return state.cmmnFile; };
export var selectCmmnFileLst = function (state) { return state.cmmnFileLst; };
export var selectCmmnPlanInstance = function (state) { return state.cmmnPlanInstance; };
export var selectCmmnPlanInstanceLst = function (state) { return state.cmmnPlanInstanceLst; };
export var selectHumanTask = function (state) { return state.humanTask; };
export var selectHumanTasks = function (state) { return state.humanTasks; };
export var selectBpmnFiles = function (state) { return state.bpmnFiles; };
export var selectBpmnFile = function (state) { return state.bpmnFile; };
export var selectBpmnInstances = function (state) { return state.bpmnInstances; };
export var selectBpmnInstance = function (state) { return state.bpmnInstance; };
export var selectCmmnPlanResult = createSelector(selectCmmnPlan, function (state) {
    if (!state || state.content == null) {
        return null;
    }
    return state.content;
});
export var selectCmmnPlanLstResult = createSelector(selectCmmnPlanLst, function (state) {
    if (!state || state.content == null) {
        return null;
    }
    return state.content;
});
export var selectCmmnFileResult = createSelector(selectCmmnFile, function (state) {
    if (!state || state.content == null) {
        return null;
    }
    return state.content;
});
export var selectCmmnFileLstResult = createSelector(selectCmmnFileLst, function (state) {
    if (!state || state.content == null) {
        return null;
    }
    return state.content;
});
export var selectCmmnPlanInstanceResult = createSelector(selectCmmnPlanInstance, function (state) {
    if (!state || state.content == null) {
        return null;
    }
    return state.content;
});
export var selectCmmnPlanInstanceLstResult = createSelector(selectCmmnPlanInstanceLst, function (state) {
    if (!state || state.content == null) {
        return null;
    }
    return state.content;
});
export var selectBpmnInstanceResult = createSelector(selectBpmnInstance, function (state) {
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
export var selectHumanTasksResult = createSelector(selectHumanTasks, function (state) {
    if (!state || state.content == null) {
        return null;
    }
    return state.content;
});
export var selectBpmnFilesResult = createSelector(selectBpmnFiles, function (state) {
    if (!state || state.content == null) {
        return null;
    }
    return state.content;
});
export var selectBpmnFileResult = createSelector(selectBpmnFile, function (state) {
    if (!state || state.content == null) {
        return null;
    }
    return state.content;
});
export var selectBpmnInstancesResult = createSelector(selectBpmnInstances, function (state) {
    if (!state || state.content == null) {
        return null;
    }
    return state.content;
});
export var appReducer = {
    cmmnPlan: fromCmmnPlan.cmmnPlanReducer,
    cmmnPlanLst: fromCmmnPlan.cmmnPlanLstReducer,
    cmmnFile: fromCmmnFile.cmmnFileReducer,
    cmmnFileLst: fromCmmnFile.cmmnFileLstReducer,
    cmmnPlanInstance: fromCmmnPlanInstance.cmmnPlanInstanceReducer,
    cmmnPlanInstanceLst: fromCmmnPlanInstance.cmmnPlanInstanceLstReducer,
    humanTask: fromHumanTask.humanTaskDefReducer,
    humanTasks: fromHumanTask.humanTaskDefsReducer,
    bpmnFiles: fromBpmnFiles.bpmnFileLstReducer,
    bpmnFile: fromBpmnFiles.bpmnFileReducer,
    bpmnInstances: fromBpmnInstances.bpmnInstanceLstReducer,
    bpmnInstance: fromBpmnInstances.bpmnInstanceReducer
};
//# sourceMappingURL=appstate.js.map