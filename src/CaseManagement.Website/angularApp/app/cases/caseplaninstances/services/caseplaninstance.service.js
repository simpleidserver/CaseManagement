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
import { SearchCasePlanInstanceResult } from '../models/searchcaseplaninstanceresult.model';
import { CasePlanInstance } from '../models/caseplaninstance.model';
var CasePlanInstanceService = (function () {
    function CasePlanInstanceService(http, oauthService) {
        this.http = http;
        this.oauthService = oauthService;
    }
    CasePlanInstanceService.prototype.search = function (startIndex, count, order, direction, casePlanId) {
        var headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        var targetUrl = process.env.API_URL + "/case-plan-instances/search?start_index=" + startIndex + "&count=" + count;
        if (order) {
            targetUrl = targetUrl + "&order_by=" + order;
        }
        if (direction) {
            targetUrl = targetUrl + "&order=" + direction;
        }
        if (casePlanId) {
            targetUrl = targetUrl + "&case_plan_id=" + casePlanId;
        }
        return this.http.get(targetUrl, { headers: headers }).pipe(map(function (res) {
            return SearchCasePlanInstanceResult.fromJson(res);
        }));
    };
    CasePlanInstanceService.prototype.searchMe = function (startIndex, count, order, direction) {
        var headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        var targetUrl = process.env.API_URL + "/case-plan-instances/me/search?start_index=" + startIndex + "&count=" + count;
        if (order) {
            targetUrl = targetUrl + "&order_by=" + order;
        }
        if (direction) {
            targetUrl = targetUrl + "&order=" + direction;
        }
        return this.http.get(targetUrl, { headers: headers }).pipe(map(function (res) {
            return SearchCasePlanInstanceResult.fromJson(res);
        }));
    };
    CasePlanInstanceService.prototype.get = function (casePlanInstanceId) {
        var headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        var targetUrl = process.env.API_URL + "/case-plan-instances/" + casePlanInstanceId;
        return this.http.get(targetUrl, { headers: headers }).pipe(map(function (res) {
            return CasePlanInstance.fromJson(res);
        }));
    };
    CasePlanInstanceService.prototype.enable = function (casePlanInstanceId, casePlanElementInstanceId) {
        var headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        var targetUrl = process.env.API_URL + "/case-plan-instances/" + casePlanInstanceId + "/enable/" + casePlanElementInstanceId;
        return this.http.get(targetUrl, { headers: headers });
    };
    CasePlanInstanceService.prototype.confirmForm = function (casePlanInstanceId, casePlanElementInstanceId, request) {
        var headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Content-Type', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        var targetUrl = process.env.API_URL + "/case-plan-instances/" + casePlanInstanceId + "/confirm/" + casePlanElementInstanceId;
        return this.http.post(targetUrl, JSON.stringify(request), { headers: headers });
    };
    CasePlanInstanceService = __decorate([
        Injectable(),
        __metadata("design:paramtypes", [HttpClient, OAuthService])
    ], CasePlanInstanceService);
    return CasePlanInstanceService;
}());
export { CasePlanInstanceService };
//# sourceMappingURL=caseplaninstance.service.js.map