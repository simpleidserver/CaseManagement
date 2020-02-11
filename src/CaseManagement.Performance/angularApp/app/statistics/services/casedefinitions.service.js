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
import { CountResult } from '../models/count-result.model';
import { OAuthService } from 'angular-oauth2-oidc';
var CaseDefinitionsService = (function () {
    function CaseDefinitionsService(http, oauthService) {
        this.http = http;
        this.oauthService = oauthService;
    }
    CaseDefinitionsService.prototype.count = function () {
        var headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        var targetUrl = process.env.API_URL + "/case-definitions/count";
        return this.http.get(targetUrl, { headers: headers }).pipe(map(function (res) {
            return CountResult.fromJson(res);
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