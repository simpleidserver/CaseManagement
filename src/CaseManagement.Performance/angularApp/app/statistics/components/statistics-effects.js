var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
import { Injectable } from '@angular/core';
import { Actions, Effect, ofType } from '@ngrx/effects';
import { of } from 'rxjs';
import { catchError, map, mergeMap } from 'rxjs/operators';
import { Observable } from 'rxjs/Rx';
import { CaseDefinitionsService } from '../services/casedefinitions.service';
import { CaseFilesService } from '../services/casefiles.service';
import { StatisticService } from '../services/statistic.service';
import { ActionTypes } from './home-actions';
function getFirstDayOfMonth() {
    var date = new Date(), y = date.getFullYear(), m = date.getMonth();
    return getDate(new Date(y, m, 1));
}
function getCurrentMonday() {
    var d = new Date();
    var day = d.getDay(), diff = d.getDate() - day + (day == 0 ? -6 : 1);
    return getDate(new Date(d.setDate(diff)));
}
function getDate(d) {
    return d.getFullYear() + '-' + (d.getMonth() + 1) + '-' + d.getDate();
}
var HomeEffects = (function () {
    function HomeEffects(actions$, statisticService, caseDefinitionsService, caseFilesService) {
        var _this = this;
        this.actions$ = actions$;
        this.statisticService = statisticService;
        this.caseDefinitionsService = caseDefinitionsService;
        this.caseFilesService = caseFilesService;
        this.loadStatistic = this.actions$
            .pipe(ofType(ActionTypes.STATISTICLOAD), mergeMap(function () {
            return _this.statisticService.get()
                .pipe(map(function (statistic) { return { type: ActionTypes.STATISTICLOADED, result: statistic }; }), catchError(function () { return of({ type: ActionTypes.ERRORLOADSTATISTIC }); }));
        }));
        this.searchWeekStatistics = this.actions$
            .pipe(ofType(ActionTypes.SEARCHWEEKSTATISTICS), mergeMap(function (evt) {
            var date = getCurrentMonday();
            return _this.statisticService.search(evt.startIndex, evt.count, evt.order, evt.direction, date, null)
                .pipe(map(function (statistic) { return { type: ActionTypes.WEEKSTATISTICSLOADED, result: statistic }; }), catchError(function () { return of({ type: ActionTypes.ERRORWEEKSTATISTICS }); }));
        }));
        this.searchMonthStatistics = this.actions$
            .pipe(ofType(ActionTypes.SEARCHMONTHSTATISTICS), mergeMap(function (evt) {
            var date = getFirstDayOfMonth();
            return _this.statisticService.search(evt.startIndex, evt.count, evt.order, evt.direction, date, null)
                .pipe(map(function (statistic) { return { type: ActionTypes.MONTHSTATISTICSLOADED, result: statistic }; }), catchError(function () { return of({ type: ActionTypes.ERRORMONTHSTATISTICS }); }));
        }));
        this.loadDeployed = this.actions$
            .pipe(ofType(ActionTypes.DEPLOYEDLOAD), mergeMap(function () {
            return Observable.forkJoin([_this.caseDefinitionsService.count(), _this.caseFilesService.count()]).pipe(map(function (responses) { return { type: ActionTypes.DEPLOYEDLOADED, nbCaseDefinitions: responses[0], nbCaseFiles: responses[1] }; }), catchError(function () { return of({ type: ActionTypes.ERRORLOADDEPLOYED }); }));
        }));
    }
    __decorate([
        Effect(),
        __metadata("design:type", Object)
    ], HomeEffects.prototype, "loadStatistic", void 0);
    __decorate([
        Effect(),
        __metadata("design:type", Object)
    ], HomeEffects.prototype, "searchWeekStatistics", void 0);
    __decorate([
        Effect(),
        __metadata("design:type", Object)
    ], HomeEffects.prototype, "searchMonthStatistics", void 0);
    __decorate([
        Effect(),
        __metadata("design:type", Object)
    ], HomeEffects.prototype, "loadDeployed", void 0);
    HomeEffects = __decorate([
        Injectable(),
        __metadata("design:paramtypes", [Actions,
            StatisticService,
            CaseDefinitionsService,
            CaseFilesService])
    ], HomeEffects);
    return HomeEffects;
}());
export { HomeEffects };
//# sourceMappingURL=home-effects.js.map