import { Action } from '@ngrx/store';
import { DailyStatistic } from '../models/dailystatistic.model';
import { SearchDailyStatisticsResult } from '../models/search-dailystatistics-result.model';
import { CountResult } from '../models/count-result.model';

export enum ActionTypes {
    STATISTICLOAD = "[Statistic] Load",
    STATISTICLOADED = "[Statistic] Loaded",
    ERRORLOADSTATISTIC = "[Statistic] Error Load",
    SEARCHWEEKSTATISTICS = "[SearchWeekStatistic] Load",
    WEEKSTATISTICSLOADED = "[SearchWeekStatistic] Loaded",
    ERRORWEEKSTATISTICS = "[SearchWeekStatistic] Error Load",
    SEARCHMONTHSTATISTICS = "[SearchMonthStatistic] Load",
    MONTHSTATISTICSLOADED = "[SearchMonthStatistic] Loaded",
    ERRORMONTHSTATISTICS = "[SearchMonthStatistic] Error Load",
    DEPLOYEDLOAD = "[Deployed] Load",
    DEPLOYEDLOADED = "[Deployed] Loaded",
    ERRORLOADDEPLOYED = "[Deployed] Error Load"
}

export class LoadStatisticAction implements Action {
    type = ActionTypes.STATISTICLOAD
    constructor() { }
}

export class StatisticLoadedAction implements Action {
    type = ActionTypes.STATISTICLOADED
    constructor(public result: DailyStatistic) { }
}

export class LoadWeekStatisticsAction implements Action {
    type = ActionTypes.SEARCHWEEKSTATISTICS
    constructor() { }
}

export class WeekStatisticsLoadedAction implements Action {
    type = ActionTypes.WEEKSTATISTICSLOADED
    constructor(public result: SearchDailyStatisticsResult) { }
}

export class LoadMonthStatisticsAction implements Action {
    type = ActionTypes.SEARCHMONTHSTATISTICS
    constructor() { }
}

export class MonthStatisticsLoadedAction implements Action {
    type = ActionTypes.MONTHSTATISTICSLOADED
    constructor(public result: SearchDailyStatisticsResult) { }
}

export class LoadDeployedAction implements Action {
    type = ActionTypes.DEPLOYEDLOAD
    constructor() { }
}

export class DeployedLoadedAction implements Action {
    type = ActionTypes.DEPLOYEDLOADED
    constructor(public result: CountResult) { }
}

export type ActionsUnion = LoadStatisticAction | StatisticLoadedAction | LoadWeekStatisticsAction | WeekStatisticsLoadedAction | LoadMonthStatisticsAction | MonthStatisticsLoadedAction | LoadDeployedAction | DeployedLoadedAction;