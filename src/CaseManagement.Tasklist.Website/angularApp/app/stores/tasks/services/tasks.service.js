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
import { of } from 'rxjs';
import { Task } from '../models/task.model';
import { map, catchError } from 'rxjs/operators';
var TasksService = (function () {
    function TasksService(http, oauthService, translate) {
        this.http = http;
        this.oauthService = oauthService;
        this.translate = translate;
    }
    TasksService.prototype.search = function (startIndex, count, order, direction, owner, status) {
        var headers = new HttpHeaders();
        var defaultLang = this.translate.currentLang;
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Content-Type', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        headers = headers.set('Accept-Language', defaultLang);
        var targetUrl = process.env.API_URL + "/humantaskinstances/.search";
        var request = { startIndex: startIndex, count: count, actualOwner: owner, statusLst: status };
        if (order) {
            request["orderBy"] = order;
        }
        if (direction) {
            request["order"] = direction;
        }
        return this.http.post(targetUrl, JSON.stringify(request), { headers: headers });
    };
    TasksService.prototype.start = function (humanTaskInstanceId) {
        var headers = new HttpHeaders();
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        var targetUrl = process.env.API_URL + "/humantaskinstances/" + humanTaskInstanceId + "/start";
        return this.http.get(targetUrl, { headers: headers });
    };
    TasksService.prototype.claim = function (humanTaskInstanceId) {
        var headers = new HttpHeaders();
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        var targetUrl = process.env.API_URL + "/humantaskinstances/" + humanTaskInstanceId + "/claim";
        return this.http.get(targetUrl, { headers: headers });
    };
    TasksService.prototype.nominate = function (humanTaskInstanceId, nominate) {
        var headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Content-Type', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        var targetUrl = process.env.API_URL + "/humantaskinstances/" + humanTaskInstanceId + "/nominate";
        return this.http.post(targetUrl, JSON.stringify(nominate), { headers: headers });
    };
    TasksService.prototype.getRendering = function (humanTaskInstanceId) {
        var headers = new HttpHeaders();
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        headers = headers.set('Content-Type', 'application/json');
        var targetUrl = process.env.API_URL + "/humantaskinstances/" + humanTaskInstanceId + "/rendering";
        return this.http.get(targetUrl, { headers: headers }).pipe(map(function (r) { return r; }), catchError(function () { return of([]); }));
    };
    TasksService.prototype.getDetails = function (humanTaskInstanceId) {
        var headers = new HttpHeaders();
        var defaultLang = this.translate.currentLang;
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        headers = headers.set('Content-Type', 'application/json');
        headers = headers.set('Accept-Language', defaultLang);
        var targetUrl = process.env.API_URL + "/humantaskinstances/" + humanTaskInstanceId + "/details";
        return this.http.get(targetUrl, { headers: headers }).pipe(map(function (r) { return r; }), catchError(function () { return of(new Task()); }));
    };
    TasksService.prototype.getDescription = function (humanTaskInstanceId) {
        var headers = new HttpHeaders();
        var defaultLang = this.translate.currentLang;
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        headers = headers.set('Accept-Language', defaultLang);
        var targetUrl = process.env.API_URL + "/humantaskinstances/" + humanTaskInstanceId + "/description";
        return this.http.get(targetUrl, { headers: headers }).pipe(map(function (r) { return r; }), catchError(function () { return of(""); }));
    };
    TasksService.prototype.searchTaskHistory = function (humanTaskInstanceId, startIndex, count, order, direction) {
        var headers = new HttpHeaders();
        var defaultLang = this.translate.currentLang;
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Content-Type', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        headers = headers.set('Accept-Language', defaultLang);
        var targetUrl = process.env.API_URL + "/humantaskinstances/" + humanTaskInstanceId + "/history";
        var request = { startIndex: startIndex, count: count };
        if (order) {
            request["orderBy"] = order;
        }
        if (direction) {
            request["order"] = direction;
        }
        return this.http.post(targetUrl, JSON.stringify(request), { headers: headers });
    };
    TasksService.prototype.completeTask = function (humanTaskInstanceId, operationParameters) {
        var headers = new HttpHeaders();
        var defaultLang = this.translate.currentLang;
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Content-Type', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        headers = headers.set('Accept-Language', defaultLang);
        var targetUrl = process.env.API_URL + "/humantaskinstances/" + humanTaskInstanceId + "/complete";
        var request = { operationParameters: operationParameters };
        return this.http.post(targetUrl, JSON.stringify(request), { headers: headers });
    };
    TasksService = __decorate([
        Injectable(),
        __metadata("design:paramtypes", [HttpClient,
            OAuthService,
            TranslateService])
    ], TasksService);
    return TasksService;
}());
export { TasksService };
//# sourceMappingURL=tasks.service.js.map