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
import { Role } from '../models/role.model';
import { SearchRolesResult } from '../models/search-roles.model';
var RolesService = (function () {
    function RolesService(http, oauthService) {
        this.http = http;
        this.oauthService = oauthService;
    }
    RolesService.prototype.search = function (startIndex, count, order, direction) {
        var headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        var targetUrl = process.env.API_URL + "/roles/search?start_index=" + startIndex + "&count=" + count;
        if (order) {
            targetUrl = targetUrl + "&order_by=" + order;
        }
        if (direction) {
            targetUrl = targetUrl + "&order=" + direction;
        }
        return this.http.get(targetUrl, { headers: headers }).pipe(map(function (res) {
            return SearchRolesResult.fromJson(res);
        }));
    };
    RolesService.prototype.get = function (role) {
        var headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        var targetUrl = process.env.API_URL + "/roles/" + role;
        return this.http.get(targetUrl, { headers: headers }).pipe(map(function (res) {
            return Role.fromJson(res);
        }));
    };
    RolesService.prototype.add = function (role) {
        var request = JSON.stringify({ role: role });
        var headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Content-Type', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        var targetUrl = process.env.API_URL + "/roles";
        return this.http.post(targetUrl, request, { headers: headers }).pipe(map(function (res) {
            return Role.fromJson(res);
        }));
    };
    RolesService.prototype.update = function (role, users) {
        var request = JSON.stringify({ users: users });
        var headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Content-Type', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        var targetUrl = process.env.API_URL + "/roles/" + role;
        return this.http.put(targetUrl, request, { headers: headers });
    };
    RolesService.prototype.delete = function (role) {
        var headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        var targetUrl = process.env.API_URL + "/roles/" + role;
        return this.http.delete(targetUrl, { headers: headers });
    };
    RolesService = __decorate([
        Injectable(),
        __metadata("design:paramtypes", [HttpClient, OAuthService])
    ], RolesService);
    return RolesService;
}());
export { RolesService };
//# sourceMappingURL=role.service.js.map