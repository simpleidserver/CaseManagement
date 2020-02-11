export var ActionTypes;
(function (ActionTypes) {
    ActionTypes["STATISTICLOAD"] = "[Statistic] Load";
    ActionTypes["STATISTICLOADED"] = "[Statistic] Loaded";
    ActionTypes["ERRORLOADSTATISTIC"] = "[Statistic] Error Load";
    ActionTypes["SEARCHWEEKSTATISTICS"] = "[SearchWeekStatistic] Load";
    ActionTypes["WEEKSTATISTICSLOADED"] = "[SearchWeekStatistic] Loaded";
    ActionTypes["ERRORWEEKSTATISTICS"] = "[SearchWeekStatistic] Error Load";
    ActionTypes["SEARCHMONTHSTATISTICS"] = "[SearchMonthStatistic] Load";
    ActionTypes["MONTHSTATISTICSLOADED"] = "[SearchMonthStatistic] Loaded";
    ActionTypes["ERRORMONTHSTATISTICS"] = "[SearchMonthStatistic] Error Load";
    ActionTypes["DEPLOYEDLOAD"] = "[Deployed] Load";
    ActionTypes["DEPLOYEDLOADED"] = "[Deployed] Loaded";
    ActionTypes["ERRORLOADDEPLOYED"] = "[Deployed] Error Load";
})(ActionTypes || (ActionTypes = {}));
var LoadStatisticAction = (function () {
    function LoadStatisticAction() {
        this.type = ActionTypes.STATISTICLOAD;
    }
    return LoadStatisticAction;
}());
export { LoadStatisticAction };
var StatisticLoadedAction = (function () {
    function StatisticLoadedAction(result) {
        this.result = result;
        this.type = ActionTypes.STATISTICLOADED;
    }
    return StatisticLoadedAction;
}());
export { StatisticLoadedAction };
var LoadWeekStatisticsAction = (function () {
    function LoadWeekStatisticsAction() {
        this.type = ActionTypes.SEARCHWEEKSTATISTICS;
    }
    return LoadWeekStatisticsAction;
}());
export { LoadWeekStatisticsAction };
var WeekStatisticsLoadedAction = (function () {
    function WeekStatisticsLoadedAction(result) {
        this.result = result;
        this.type = ActionTypes.WEEKSTATISTICSLOADED;
    }
    return WeekStatisticsLoadedAction;
}());
export { WeekStatisticsLoadedAction };
var LoadMonthStatisticsAction = (function () {
    function LoadMonthStatisticsAction() {
        this.type = ActionTypes.SEARCHMONTHSTATISTICS;
    }
    return LoadMonthStatisticsAction;
}());
export { LoadMonthStatisticsAction };
var MonthStatisticsLoadedAction = (function () {
    function MonthStatisticsLoadedAction(result) {
        this.result = result;
        this.type = ActionTypes.MONTHSTATISTICSLOADED;
    }
    return MonthStatisticsLoadedAction;
}());
export { MonthStatisticsLoadedAction };
var LoadDeployedAction = (function () {
    function LoadDeployedAction() {
        this.type = ActionTypes.DEPLOYEDLOAD;
    }
    return LoadDeployedAction;
}());
export { LoadDeployedAction };
var DeployedLoadedAction = (function () {
    function DeployedLoadedAction(nbCaseDefinitions, nbCaseFiles) {
        this.nbCaseDefinitions = nbCaseDefinitions;
        this.nbCaseFiles = nbCaseFiles;
        this.type = ActionTypes.DEPLOYEDLOADED;
    }
    return DeployedLoadedAction;
}());
export { DeployedLoadedAction };
//# sourceMappingURL=statistics-actions.js.map