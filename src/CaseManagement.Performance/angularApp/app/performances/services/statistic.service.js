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
import { map } from 'rxjs/operators';
import { SearchPerformancesResult } from '../models/search-performances-result.model';
import { OAuthService } from 'angular-oauth2-oidc';
var StatisticService = (function () {
    function StatisticService(http, oauthService) {
        this.http = http;
        this.oauthService = oauthService;
    }
    StatisticService.prototype.getPerformances = function () {
        var headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        var targetUrl = process.env.API_URL + "/statistics/performances";
        return this.http.get(targetUrl, { headers: headers });
    };
    StatisticService.prototype.searchPerformances = function (startIndex, count, order, direction, startDateTime) {
        var headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        var targetUrl = process.env.API_URL + "/statistics/performances/search?start_index=" + startIndex + "&count=" + count + "&group_by=machine_name";
        if (order) {
            targetUrl = targetUrl + "&order_by=" + order;
        }
        if (direction) {
            targetUrl = targetUrl + "&order=" + direction;
        }
        if (startDateTime) {
            targetUrl = targetUrl + "&start_datetime=" + startDateTime;
        }
        return this.http.get(targetUrl, { headers: headers }).pipe(map(function (res) {
            return SearchPerformancesResult.fromJson(res);
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