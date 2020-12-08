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
var CasePlanService = (function () {
    function CasePlanService(http, oauthService) {
        this.http = http;
        this.oauthService = oauthService;
    }
    CasePlanService.prototype.search = function (startIndex, count, order, direction, text, caseFileId) {
        var headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        var targetUrl = process.env.API_URL + "/case-plans/search";
        var request = { startIndex: startIndex, count: count, takeLatest: true };
        if (order) {
            request["orderBy"] = order;
        }
        if (direction) {
            request["order"] = direction;
        }
        if (text) {
            request["text"] = text;
        }
        if (caseFileId) {
            request["caseFileId"] = caseFileId;
        }
        return this.http.post(targetUrl, request, { headers: headers });
    };
    CasePlanService.prototype.searchHistory = function (casePlanId, startIndex, count, order, direction) {
        var headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        var targetUrl = process.env.API_URL + "/case-plans/search";
        var request = { startIndex: startIndex, count: count, casePlanId: casePlanId, takeLatest: false };
        if (order) {
            request["orderBy"] = order;
        }
        if (direction) {
            request["order"] = direction;
        }
        return this.http.post(targetUrl, request, { headers: headers });
    };
    CasePlanService.prototype.get = function (id) {
        var headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        var targetUrl = process.env.API_URL + "/case-plans/" + id;
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