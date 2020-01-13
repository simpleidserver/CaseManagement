import { DailyStatistic } from "../models/dailystatistic.model";
import { SearchDailyStatisticsResult } from "../models/search-dailystatistics-result.model";

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