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
import { SearchCaseInstancesResult } from '../models/search-case-instances.model';
import { CaseInstance } from '../models/case-instance.model';
import { OAuthService } from 'angular-oauth2-oidc';
var CaseInstancesService = (function () {
    function CaseInstancesService(http, oauthService) {
        this.http = http;
        this.oauthService = oauthService;
    }
    CaseInstancesService.prototype.search = function (caseDefinitionId, startIndex, count, order, direction) {
        var headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        var targetUrl = process.env.API_URL + "/case-instances/search?start_index=" + startIndex + "&count=" + count + "&case_definition_id=" + caseDefinitionId;
        if (order) {
            targetUrl = targetUrl + "&order_by=" + order;
        }
        if (direction) {
            targetUrl = targetUrl + "&order=" + direction;
        }
        return this.http.get(targetUrl, { headers: headers }).pipe(map(function (res) {
            var result = SearchCaseInstancesResult.fromJson(res);
            return result;
        }));
    };
    CaseInstancesService.prototype.create = function (caseDefId) {
        var request = JSON.stringify({ case_definition_id: caseDefId });
        var targetUrl = process.env.API_URL + "/case-instances";
        var headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Content-Type', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        return this.http.post(targetUrl, request, { headers: headers }).pipe(map(function (res) {
            return CaseInstance.fromJson(res);
        }));
    };
    CaseInstancesService.prototype.launch = function (caseInstanceId) {
        var targetUrl = process.env.API_URL + "/case-instances/" + caseInstanceId + "/launch";
        var headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        return this.http.get(targetUrl, { headers: headers });
    };
    CaseInstancesService.prototype.reactivateCaseInstance = function (caseInstanceId) {
        var targetUrl = process.env.API_URL + "/case-instances/" + caseInstanceId + "/reactivate";
        var headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        return this.http.get(targetUrl, { headers: headers });
    };
    CaseInstancesService.prototype.suspendCaseInstance = function (caseInstanceId) {
        var targetUrl = process.env.API_URL + "/case-instances/" + caseInstanceId + "/suspend";
        var headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        return this.http.get(targetUrl, { headers: headers });
    };
    CaseInstancesService.prototype.resumeCaseInstance = function (caseInstanceId) {
        var targetUrl = process.env.API_URL + "/case-instances/" + caseInstanceId + "/resume";
        var headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        return this.http.get(targetUrl, { headers: headers });
    };
    CaseInstancesService.prototype.closeCaseInstance = function (caseInstanceId) {
        var targetUrl = process.env.API_URL + "/case-instances/" + caseInstanceId + "/close";
        var headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        return this.http.get(targetUrl, { headers: headers });
    };
    CaseInstancesService.prototype.getCaseFileItems = function (caseInstanceId) {
        var targetUrl = process.env.API_URL + "/case-instances/" + caseInstanceId + "/casefileitems";
        var headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        return this.http.get(targetUrl, { headers: headers });
    };
    CaseInstancesService.prototype.get = function (caseInstanceId) {
        var targetUrl = process.env.API_URL + "/case-instances/" + caseInstanceId;
        var headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        return this.http.get(targetUrl, { headers: headers }).pipe(map(function (res) {
            var result = CaseInstance.fromJson(res);
            return result;
        }));
    };
    CaseInstancesService = __decorate([
        Injectable(),
        __metadata("design:paramtypes", [HttpClient, OAuthService])
    ], CaseInstancesService);
    return CaseInstancesService;
}());
export { CaseInstancesService };
//# sourceMappingURL=caseinstances.service.js.map