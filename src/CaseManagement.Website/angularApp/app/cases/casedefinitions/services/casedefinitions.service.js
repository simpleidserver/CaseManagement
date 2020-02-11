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
import { CaseDefinitionHistory } from '../models/case-definition-history.model';
import { CaseDefinition } from '../models/case-definition.model';
import { SearchCaseDefinitionsResult } from '../models/search-case-definitions-result.model';
var CaseDefinitionsService = (function () {
    function CaseDefinitionsService(http, oauthService) {
        this.http = http;
        this.oauthService = oauthService;
    }
    CaseDefinitionsService.prototype.search = function (startIndex, count, order, direction, text, owner) {
        var headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        var targetUrl = process.env.API_URL + "/case-definitions/search?start_index=" + startIndex + "&count=" + count;
        if (order) {
            targetUrl = targetUrl + "&order_by=" + order;
        }
        if (direction) {
            targetUrl = targetUrl + "&order=" + direction;
        }
        if (text) {
            targetUrl = targetUrl + "&text=" + text;
        }
        if (owner) {
            targetUrl = targetUrl + "&owner=" + owner;
        }
        return this.http.get(targetUrl, { headers: headers }).pipe(map(function (res) {
            return SearchCaseDefinitionsResult.fromJson(res);
        }));
    };
    CaseDefinitionsService.prototype.get = function (id) {
        var headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        var targetUrl = process.env.API_URL + "/case-definitions/" + id;
        return this.http.get(targetUrl, { headers: headers }).pipe(map(function (res) {
            return CaseDefinition.fromJson(res);
        }));
    };
    CaseDefinitionsService.prototype.getHistory = function (id) {
        var headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        var targetUrl = process.env.API_URL + "/case-definitions/" + id + "/history";
        return this.http.get(targetUrl, { headers: headers }).pipe(map(function (res) {
            return CaseDefinitionHistory.fromJson(res);
        }));
    };
    CaseDefinitionsService = __decorate([
        Injectable(),
        __metadata("design:paramtypes", [HttpClient, OAuthService])
    ], CaseDefinitionsService);
    return CaseDefinitionsService;
}());
export { CaseDefinitionsService };
//# sourceMappingURL=casedefinitions.service.js.map