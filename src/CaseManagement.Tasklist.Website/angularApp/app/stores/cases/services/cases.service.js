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
import { TranslateService } from '@ngx-translate/core';
import { OAuthService } from 'angular-oauth2-oidc';
var CasesService = (function () {
    function CasesService(http, oauthService, translate) {
        this.http = http;
        this.oauthService = oauthService;
        this.translate = translate;
    }
    CasesService.prototype.search = function (startIndex, count, order, direction) {
        var headers = new HttpHeaders();
        var defaultLang = this.translate.currentLang;
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Content-Type', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        headers = headers.set('Accept-Language', defaultLang);
        var targetUrl = process.env.CM_API_URL + "/case-plan-instances/search";
        var request = { startIndex: startIndex, count: count };
        if (order) {
            request["orderBy"] = order;
        }
        if (direction) {
            request["order"] = direction;
        }
        return this.http.post(targetUrl, JSON.stringify(request), { headers: headers });
    };
    CasesService.prototype.get = function (id) {
        var headers = new HttpHeaders();
        var defaultLang = this.translate.currentLang;
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Content-Type', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        headers = headers.set('Accept-Language', defaultLang);
        var targetUrl = process.env.CM_API_URL + "/case-plan-instances/" + id;
        return this.http.get(targetUrl, { headers: headers });
    };
    CasesService.prototype.activate = function (id, elt) {
        var headers = new HttpHeaders();
        var defaultLang = this.translate.currentLang;
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Content-Type', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        headers = headers.set('Accept-Language', defaultLang);
        var targetUrl = process.env.CM_API_URL + "/case-plan-instances/" + id + "/activate/" + elt;
        var request = {};
        return this.http.post(targetUrl, JSON.stringify(request), { headers: headers });
    };
    CasesService.prototype.disable = function (id, elt) {
        var headers = new HttpHeaders();
        var defaultLang = this.translate.currentLang;
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Content-Type', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        headers = headers.set('Accept-Language', defaultLang);
        var targetUrl = process.env.CM_API_URL + "/case-plan-instances/" + id + "/disable/" + elt;
        var request = {};
        return this.http.post(targetUrl, JSON.stringify(request), { headers: headers });
    };
    CasesService.prototype.reenable = function (id, elt) {
        var headers = new HttpHeaders();
        var defaultLang = this.translate.currentLang;
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Content-Type', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        headers = headers.set('Accept-Language', defaultLang);
        var targetUrl = process.env.CM_API_URL + "/case-plan-instances/" + id + "/reenable/" + elt;
        var request = {};
        return this.http.post(targetUrl, JSON.stringify(request), { headers: headers });
    };
    CasesService.prototype.complete = function (id, elt) {
        var headers = new HttpHeaders();
        var defaultLang = this.translate.currentLang;
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Content-Type', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        headers = headers.set('Accept-Language', defaultLang);
        var request = {};
        var targetUrl = process.env.CM_API_URL + "/case-plan-instances/" + id + "/complete/" + elt;
        return this.http.post(targetUrl, JSON.stringify(request), { headers: headers });
    };
    CasesService = __decorate([
        Injectable(),
        __metadata("design:paramtypes", [HttpClient,
            OAuthService,
            TranslateService])
    ], CasesService);
    return CasesService;
}());
export { CasesService };
//# sourceMappingURL=cases.service.js.map