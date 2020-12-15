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
var BpmnFilesService = (function () {
    function BpmnFilesService(http, oauthService) {
        this.http = http;
        this.oauthService = oauthService;
    }
    BpmnFilesService.prototype.search = function (startIndex, count, order, direction, takeLatest, fileId) {
        var headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Content-Type', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        var targetUrl = process.env.BPMN_API_URL + "/processfiles/search";
        var request = { startIndex: startIndex, count: count, takeLatest: takeLatest };
        if (order) {
            request["orderBy"] = order;
        }
        if (direction) {
            request["order"] = direction;
        }
        if (fileId) {
            request["fileId"] = fileId;
        }
        return this.http.post(targetUrl, JSON.stringify(request), { headers: headers });
    };
    BpmnFilesService.prototype.get = function (id) {
        var headers = new HttpHeaders();
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        var targetUrl = process.env.BPMN_API_URL + "/processfiles/" + id;
        return this.http.get(targetUrl, { headers: headers });
    };
    BpmnFilesService.prototype.update = function (id, name, description) {
        var headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Content-Type', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        var targetUrl = process.env.BPMN_API_URL + "/processfiles/" + id;
        var request = { name: name, description: description };
        return this.http.put(targetUrl, JSON.stringify(request), { headers: headers });
    };
    BpmnFilesService.prototype.updatePayload = function (id, payload) {
        var headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Content-Type', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        var targetUrl = process.env.BPMN_API_URL + "/processfiles/" + id + "/payload";
        var request = { payload: payload };
        return this.http.put(targetUrl, JSON.stringify(request), { headers: headers });
    };
    BpmnFilesService.prototype.publish = function (id) {
        var headers = new HttpHeaders();
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        var targetUrl = process.env.BPMN_API_URL + "/processfiles/" + id + "/publish";
        return this.http.get(targetUrl, { headers: headers }).pipe(map(function (res) {
            return res['id'];
        }));
    };
    BpmnFilesService = __decorate([
        Injectable(),
        __metadata("design:paramtypes", [HttpClient, OAuthService])
    ], BpmnFilesService);
    return BpmnFilesService;
}());
export { BpmnFilesService };
//# sourceMappingURL=bpmnfiles.service.js.map