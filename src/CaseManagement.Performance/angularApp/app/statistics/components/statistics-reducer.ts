import { ActionsUnion, ActionTypes, MonthStatisticsLoadedAction, StatisticLoadedAction, WeekStatisticsLoadedAction, DeployedLoadedAction } from './statistics-actions';
import { MonthStatisticsState, StatisticState, WeekStatisticsState, DeployedState } from './statistics-states';

const initialStatisticAction: StatisticState = {
    content: null,
    isLoading: true,
    isErrorLoadOccured: false
};

const initialWeekStatisticAction: WeekStatisticsState = {
    content: null,
    isLoading: true,
    isErrorLoadOccured: false
};

const initialMonthStatisticAction: MonthStatisticsState = {
    content: null,
    isLoading: true,
    isErrorLoadOccured: false
};

const initalDeployedState: DeployedState = {
    isErrorLoadOccured: false,
    isLoading: true,
    content: null
};

export function statisticReducer(state = initialStatisticAction, action: ActionsUnion) {
    switch (action.type) {
        case ActionTypes.STATISTICLOADED:
            let statisticLoadedAction = <StatisticLoadedAction>action;
            state.content = statisticLoadedAction.result;
            state.isLoading = false;
            state.isErrorLoadOccured = false;
            return { ...state };
        case ActionTypes.ERRORLOADSTATISTIC:
            state.isErrorLoadOccured = true;
            state.isLoading = false;
            return { ...state };
        default:
            return state;
    }
}

export function weekStatisticsReducer(state = initialWeekStatisticAction, action: ActionsUnion) {
    switch (action.type) {
        case ActionTypes.WEEKSTATISTICSLOADED:
            let statisticLoadedAction = <WeekStatisticsLoadedAction>action;
            state.content = statisticLoadedAction.result;
            state.isLoading = false;
            state.isErrorLoadOccured = false;
            return { ...state };
        case ActionTypes.ERRORWEEKSTATISTICS:
            state.isErrorLoadOccured = true;
            state.isLoading = false;
            return { ...state };
        default:
            return state;
    }
}

export function monthStatisticsReducer(state = initialMonthStatisticAction, action: ActionsUnion) {
    switch (action.type) {
        case ActionTypes.MONTHSTATISTICSLOADED:
            let statisticLoadedAction = <MonthStatisticsLoadedAction>action;
            state.content = statisticLoadedAction.result;
            state.isLoading = false;
            state.isErrorLoadOccured = false;
            return { ...state };
        case ActionTypes.ERRORMONTHSTATISTICS:
            state.isErrorLoadOccured = true;
            state.isLoading = false;
            return { ...state };
        default:
            return state;
    }
}

export function deployedReducer(state = initalDeployedState, action: ActionsUnion) {
    switch (action.type) {
        case ActionTypes.DEPLOYEDLOADED:
            let deployedLoadedAction = <DeployedLoadedAction>action;
            state.content = deployedLoadedAction.result;
            state.isLoading = false;
            state.isErrorLoadOccured = false;
            return { ...state };
        case ActionTypes.ERRORLOADDEPLOYED:
            state.isErrorLoadOccured = true;
            state.isLoading = false;
            return { ...state };
        default:
            return state;
    }
}