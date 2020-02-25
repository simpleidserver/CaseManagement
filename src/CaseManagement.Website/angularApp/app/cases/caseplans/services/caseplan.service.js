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
import { CasePlan } from '../models/caseplan.model';
import { SearchCasePlanInstanceResult } from '../models/searchcaseplaninstanceresult.model';
import { SearchCasePlanResult } from '../models/searchcaseplanresult.model';
import { SearchFormInstanceResult } from '../models/searchforminstanceresult.model';
import { SearchWorkerTaskResult } from '../models/searchworkertaskresult.model';
var CasePlanService = (function () {
    function CasePlanService(http, oauthService) {
        this.http = http;
        this.oauthService = oauthService;
    }
    CasePlanService.prototype.search = function (startIndex, count, order, direction, text, caseFileId) {
        var headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        var targetUrl = process.env.API_URL + "/case-plans/me/search?start_index=" + startIndex + "&count=" + count;
        if (order) {
            targetUrl = targetUrl + "&order_by=" + order;
        }
        if (direction) {
            targetUrl = targetUrl + "&order=" + direction;
        }
        if (text) {
            targetUrl = targetUrl + "&text=" + text;
        }
        if (caseFileId) {
            targetUrl = targetUrl + "&case_file=" + caseFileId;
        }
        return this.http.get(targetUrl, { headers: headers }).pipe(map(function (res) {
            return SearchCasePlanResult.fromJson(res);
        }));
    };
    CasePlanService.prototype.get = function (id) {
        var headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        var targetUrl = process.env.API_URL + "/case-plans/" + id;
        return this.http.get(targetUrl, { headers: headers }).pipe(map(function (res) {
            return CasePlan.fromJson(res);
        }));
    };
    CasePlanService.prototype.searchHistory = function (casePlanId, startIndex, count, order, direction) {
        var headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        var targetUrl = process.env.API_URL + "/case-plans/" + casePlanId + "/history/search?start_index=" + startIndex + "&count=" + count;
        if (order) {
            targetUrl = targetUrl + "&order_by=" + order;
        }
        if (direction) {
            targetUrl = targetUrl + "&order=" + direction;
        }
        return this.http.get(targetUrl, { headers: headers }).pipe(map(function (res) {
            var result = SearchCasePlanResult.fromJson(res);
            return result;
        }));
    };
    CasePlanService.prototype.searchCasePlanInstance = function (casePlanId, startIndex, count, order, direction) {
        var headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        var targetUrl = process.env.API_URL + "/case-plans/" + casePlanId + "/caseplaninstances/search?start_index=" + startIndex + "&count=" + count;
        if (order) {
            targetUrl = targetUrl + "&order_by=" + order;
        }
        if (direction) {
            targetUrl = targetUrl + "&order=" + direction;
        }
        return this.http.get(targetUrl, { headers: headers }).pipe(map(function (res) {
            var result = SearchCasePlanInstanceResult.fromJson(res);
            return result;
        }));
    };
    CasePlanService.prototype.searchFormInstance = function (casePlanId, startIndex, count, order, direction) {
        var headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        var targetUrl = process.env.API_URL + "/case-plans/" + casePlanId + "/forminstances/search?start_index=" + startIndex + "&count=" + count;
        if (order) {
            targetUrl = targetUrl + "&order_by=" + order;
        }
        if (direction) {
            targetUrl = targetUrl + "&order=" + direction;
        }
        return this.http.get(targetUrl, { headers: headers }).pipe(map(function (res) {
            var result = SearchFormInstanceResult.fromJson(res);
            return result;
        }));
    };
    CasePlanService.prototype.searchWorkerTask = function (casePlanId, startIndex, count, order, direction) {
        var headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        var targetUrl = process.env.API_URL + "/case-plans/" + casePlanId + "/caseworkertasks/search?start_index=" + startIndex + "&count=" + count;
        if (order) {
            targetUrl = targetUrl + "&order_by=" + order;
        }
        if (direction) {
            targetUrl = targetUrl + "&order=" + direction;
        }
        return this.http.get(targetUrl, { headers: headers }).pipe(map(function (res) {
            var result = SearchWorkerTaskResult.fromJson(res);
            return result;
        }));
    };
    CasePlanService.prototype.launchCasePlanInstance = function (casePlanId) {
        var headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        var targetUrl = process.env.API_URL + "/case-plans/" + casePlanId + "/caseplaninstances/launch";
        return this.http.get(targetUrl, { headers: headers });
    };
    CasePlanService.prototype.closeCasePlanInstance = function (casePlanId, casePlanInstanceId) {
        var headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        var targetUrl = process.env.API_URL + "/case-plans/" + casePlanId + "/caseplaninstances/" + casePlanInstanceId + "/close";
        return this.http.get(targetUrl, { headers: headers });
    };
    CasePlanService.prototype.reactivateCasePlanInstance = function (casePlanId, casePlanInstanceId) {
        var headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        var targetUrl = process.env.API_URL + "/case-plans/" + casePlanId + "/caseplaninstances/" + casePlanInstanceId + "/reactivate";
        return this.http.get(targetUrl, { headers: headers });
    };
    CasePlanService.prototype.resumeCasePlanInstance = function (casePlanId, casePlanInstanceId) {
        var headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        var targetUrl = process.env.API_URL + "/case-plans/" + casePlanId + "/caseplaninstances/" + casePlanInstanceId + "/resume";
        return this.http.get(targetUrl, { headers: headers });
    };
    CasePlanService.prototype.suspendCasePlanInstance = function (casePlanId, casePlanInstanceId) {
        var headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        var targetUrl = process.env.API_URL + "/case-plans/" + casePlanId + "/caseplaninstances/" + casePlanInstanceId + "/suspend";
        return this.http.get(targetUrl, { headers: headers });
    };
    CasePlanService.prototype.terminateCasePlanInstance = function (casePlanId, casePlanInstanceId) {
        var headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        var targetUrl = process.env.API_URL + "/case-plans/" + casePlanId + "/caseplaninstances/" + casePlanInstanceId + "/terminate";
        return this.http.get(targetUrl, { headers: headers });
    };
    CasePlanService = __decorate([
        Injectable(),
        __metadata("design:paramtypes", [HttpClient, OAuthService])
    ], CasePlanService);
    return CasePlanService;
}());
export { CasePlanService };
//# sourceMappingURL=caseplan.service.js.map