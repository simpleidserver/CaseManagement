import { DailyStatistic } from "../models/dailystatistic.model";
import { SearchDailyStatisticsResult } from "../models/search-dailystatistics-result.model";
import { CountResult } from "../models/count-result.model";

export interface StatisticState {
	isLoading: boolean;
	isErrorLoadOccured: boolean;
    content: DailyStatistic;
}

export interface WeekStatisticsState {
    isLoading: boolean;
    isErrorLoadOccured: boolean;
    content: SearchDailyStatisticsResult;
}

export interface MonthStatisticsState {
    isLoading: boolean;
    isErrorLoadOccured: boolean;
    content: SearchDailyStatisticsResult;
}

export interface DeployedState {
    content: CountResult;
    isLoading: boolean;
    isErrorLoadOccured: boolean;
}