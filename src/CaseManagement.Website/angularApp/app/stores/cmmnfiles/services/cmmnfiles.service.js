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
var CmmnFilesService = (function () {
    function CmmnFilesService(http, oauthService) {
        this.http = http;
        this.oauthService = oauthService;
    }
    CmmnFilesService.prototype.search = function (startIndex, count, order, direction, text, caseFileId, takeLatest) {
        var headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Content-Type', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        var targetUrl = process.env.API_URL + "/case-files/search";
        var request = { startIndex: startIndex, count: count, takeLatest: takeLatest };
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
        return this.http.post(targetUrl, JSON.stringify(request), { headers: headers });
    };
    CmmnFilesService.prototype.get = function (id) {
        var headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        var targetUrl = process.env.API_URL + "/case-files/" + id;
        return this.http.get(targetUrl, { headers: headers });
    };
    CmmnFilesService.prototype.add = function (name, description) {
        var request = JSON.stringify({ name: name, description: description });
        var headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Content-Type', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        var targetUrl = process.env.API_URL + "/case-files";
        return this.http.post(targetUrl, request, { headers: headers }).pipe(map(function (res) {
            return res['id'];
        }));
    };
    CmmnFilesService.prototype.update = function (id, name, description) {
        var request = JSON.stringify({ name: name, description: description });
        var headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Content-Type', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        var targetUrl = process.env.API_URL + "/case-files/" + id;
        return this.http.put(targetUrl, request, { headers: headers });
    };
    CmmnFilesService.prototype.updatePayload = function (id, payload) {
        var request = JSON.stringify({ payload: payload });
        var headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Content-Type', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        var targetUrl = process.env.API_URL + "/case-files/" + id + "/payload";
        return this.http.put(targetUrl, request, { headers: headers });
    };
    CmmnFilesService.prototype.publish = function (id) {
        var headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        var targetUrl = process.env.API_URL + "/case-files/" + id + "/publish";
        return this.http.get(targetUrl, { headers: headers }).pipe(map(function (res) {
            return res['id'];
        }));
    };
    CmmnFilesService = __decorate([
        Injectable(),
        __metadata("design:paramtypes", [HttpClient, OAuthService])
    ], CmmnFilesService);
    return CmmnFilesService;
}());
export { CmmnFilesService };
//# sourceMappingURL=cmmnfiles.service.js.map