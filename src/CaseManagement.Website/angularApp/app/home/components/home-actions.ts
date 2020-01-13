import { Action } from '@ngrx/store';
import { DailyStatistic } from '../models/dailystatistic.model';
import { SearchDailyStatisticsResult } from '../models/search-dailystatistics-result.model';

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

export type ActionsUnion = LoadStatisticAction | StatisticLoadedAction | LoadWeekStatisticsAction | WeekStatisticsLoadedAction | LoadMonthStatisticsAction | MonthStatisticsLoadedAction;