var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { OAuthService } from 'angular-oauth2-oidc';
import { map } from 'rxjs/operators';
import { CountResult } from '../models/count-result.model';
import { DailyStatistic } from '../models/dailystatistic.model';
import { SearchDailyStatisticsResult } from '../models/search-dailystatistics-result.model';
var StatisticService = (function () {
    function StatisticService(http, oauthService) {
        this.http = http;
        this.oauthService = oauthService;
    }
    StatisticService.prototype.count = function () {
        var headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        var targetUrl = process.env.API_URL + "/statistics/count";
        return this.http.get(targetUrl, { headers: headers }).pipe(map(function (res) {
            return CountResult.fromJson(res);
        }));
    };
    StatisticService.prototype.get = function () {
        var headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        var targetUrl = process.env.API_URL + "/statistics";
        return this.http.get(targetUrl, { headers: headers }).pipe(map(function (res) {
            return DailyStatistic.fromJson(res);
        }));
    };
    StatisticService.prototype.search = function (startIndex, count, order, direction, startDate, endDate) {
        var headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        var targetUrl = process.env.API_URL + "/statistics/search?start_index=" + startIndex + "&count=" + count;
        if (order) {
            targetUrl = targetUrl + "&order_by=" + order;
        }
        if (direction) {
            targetUrl = targetUrl + "&order=" + direction;
        }
        if (startDate) {
            targetUrl = targetUrl + "&start_datetime=" + startDate;
        }
        if (endDate) {
            targetUrl = targetUrl + "&end_datetime=" + endDate;
        }
        return this.http.get(targetUrl, { headers: headers }).pipe(map(function (res) {
            return SearchDailyStatisticsResult.fromJson(res);
        }));
    };
    StatisticService = __decorate([
        Injectable(),
        __metadata("design:paramtypes", [HttpClient, OAuthService])
    ], StatisticService);
    return StatisticService;
}());
export { StatisticService };
//# sourceMappingURL=statistic.service.js.map