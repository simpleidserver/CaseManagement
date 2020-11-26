import { createSelector } from '@ngrx/store';
import * as fromBpmnFiles from './bpmnfiles/reducers/bpmnfile.reducers';
import * as fromBpmnInstances from './bpmninstances/reducers/bpmninstance.reducers';
import * as fromCaseFile from './casefiles/reducers/casefile.reducers';
import * as fromCasePlanInstance from './caseplaninstances/reducers/caseplaninstance.reducers';
import * as fromCasePlan from './caseplans/reducers/caseplan.reducers';
import * as fromHumanTask from './humantaskdefs/reducers/humantaskdef.reducers';

export interface AppState {
    casePlan: fromCasePlan.CasePlanState;
    casePlanLst: fromCasePlan.SearchCasePlanState;
    casePlanHistoryLst: fromCasePlan.SearchCasePlanHistoryState;
    caseFile: fromCaseFile.CaseFileState;
    caseFileLst: fromCaseFile.CaseFileLstState;
    caseFileHistoryLst: fromCaseFile.CaseFileHistoryLstState;
    casePlanInstance: fromCasePlanInstance.CasePlanInstanceState;
    casePlanInstanceLst: fromCasePlanInstance.CasePlanInstanceLstState;
    humanTask: fromHumanTask.HumanTaskDefState;
    humanTasks: fromHumanTask.SearchHumanTaskDefState;
    bpmnFiles: fromBpmnFiles.BpmnFileLstState;
    bpmnFile: fromBpmnFiles.BpmnFileState;
    bpmnInstances: fromBpmnInstances.BpmnInstanceLstState;
    bpmnInstance: fromBpmnInstances.BpmnInstanceState
}

export const selectCasePlan = (state: AppState) => state.casePlan;
export const selectCasePlanLst = (state: AppState) => state.casePlanLst;
export const selectCasePlanHistoryLst = (state: AppState) => state.casePlanHistoryLst;
export const selectCaseFile = (state: AppState) => state.caseFile;
export const selectCaseFileLst = (state: AppState) => state.caseFileLst;
export const selectCaseFileHistoryLst = (state: AppState) => state.caseFileHistoryLst;
export const selectCasePlanInstance = (state: AppState) => state.casePlanInstance;
export const selectCasePlanInstanceLst = (state: AppState) => state.casePlanInstanceLst;
export const selectHumanTask = (state: AppState) => state.humanTask;
export const selectHumanTasks = (state: AppState) => state.humanTasks;
export const selectBpmnFiles = (state: AppState) => state.bpmnFiles;
export const selectBpmnFile = (state: AppState) => state.bpmnFile;
export const selectBpmnInstances = (state: AppState) => state.bpmnInstances;
export const selectBpmnInstance = (state: AppState) => state.bpmnInstance;

export const selectCasePlanResult = createSelector(
    selectCasePlan,
    (state: fromCasePlan.CasePlanState) => {
        if (!state || state.content == null) {
            return null;
        }

        return state.content;
    }
);

export const selectCasePlanLstResult = createSelector(
    selectCasePlanLst,
    (state: fromCasePlan.SearchCasePlanState) => {
        if (!state || state.content == null) {
            return null;
        }

        return state.content;
    }
);

export const selectCasePlanHistoryLstResult = createSelector(
    selectCasePlanHistoryLst,
    (state: fromCasePlan.SearchCasePlanHistoryState) => {
        if (!state || state.content == null) {
            return null;
        }

        return state.content;
    }
);

export const selectCaseFileResult = createSelector(
    selectCaseFile,
    (state: fromCaseFile.CaseFileState) => {
        if (!state || state.content == null) {
            return null;
        }

        return state.content;
    }
);

export const selectCaseFileLstResult = createSelector(
    selectCaseFileLst,
    (state: fromCaseFile.CaseFileLstState) => {
        if (!state || state.content == null) {
            return null;
        }

        return state.content;
    }
);

export const selectCaseFileHistoryLstResult = createSelector(
    selectCaseFileHistoryLst,
    (state: fromCaseFile.CaseFileHistoryLstState) => {
        if (!state || state.content == null) {
            return null;
        }

        return state.content;
    }
); 

export const selectCasePlanInstanceResult = createSelector(
    selectCasePlanInstance,
    (state: fromCasePlanInstance.CasePlanInstanceState) => {
        if (!state || state.content == null) {
            return null;
        }

        return state.content;
    }
);

export const selectCasePlanInstanceLstResult = createSelector(
    selectCasePlanInstanceLst,
    (state: fromCasePlanInstance.CasePlanInstanceLstState) => {
        if (!state || state.content == null) {
            return null;
        }

        return state.content;
    }
);

export const selectBpmnInstanceResult = createSelector(
    selectBpmnInstance,
    (state: fromBpmnInstances.BpmnInstanceState) => {
        if (!state || state.content == null) {
            return null;
        }

        return state.content;
    }
);

export const selectHumanTaskResult = createSelector(
    selectHumanTask,
    (state: fromHumanTask.HumanTaskDefState) => {
        if (!state || state.content == null) {
            return null;
        }

        return state.content;
    }
);

export const selectHumanTasksResult = createSelector(
    selectHumanTasks,
    (state: fromHumanTask.SearchHumanTaskDefState) => {
        if (!state || state.content == null) {
            return null;
        }

        return state.content;
    }
); 

export const selectBpmnFilesResult = createSelector(
    selectBpmnFiles,
    (state: fromBpmnFiles.BpmnFileLstState) => {
        if (!state || state.content == null) {
            return null;
        }

        return state.content;
    }
);

export const selectBpmnFileResult = createSelector(
    selectBpmnFile,
    (state: fromBpmnFiles.BpmnFileState) => {
        if (!state || state.content == null) {
            return null;
        }

        return state.content;
    }
);

export const selectBpmnInstancesResult = createSelector(
    selectBpmnInstances,
    (state: fromBpmnInstances.BpmnInstanceLstState) => {
        if (!state || state.content == null) {
            return null;
        }

        return state.content;
    }
);

export const appReducer = {
    casePlan: fromCasePlan.casePlanReducer,
    casePlanLst: fromCasePlan.casePlanLstReducer,
    casePlanHistoryLst: fromCasePlan.casePlanHistoryLstReducer,
    caseFile: fromCaseFile.caseFileReducer,
    caseFileLst: fromCaseFile.caseFileLstReducer,
    caseFileHistoryLst: fromCaseFile.caseFileHistoryLstReducer,
    casePlanInstance: fromCasePlanInstance.casePlanInstanceReducer,
    casePlanInstanceLst: fromCasePlanInstance.casePlanInstanceLstReducer,
    humanTask: fromHumanTask.humanTaskDefReducer,
    humanTasks: fromHumanTask.humanTaskDefsReducer,
    bpmnFiles: fromBpmnFiles.bpmnFileLstReducer,
    bpmnFile: fromBpmnFiles.bpmnFileReducer,
    bpmnInstances: fromBpmnInstances.bpmnInstanceLstReducer,
    bpmnInstance: fromBpmnInstances.bpmnInstanceReducer
};