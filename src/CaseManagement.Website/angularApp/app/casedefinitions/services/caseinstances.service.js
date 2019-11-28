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
import { SearchCaseInstancesResult, CaseInstance } from '../models/search-case-instances-result.model';
var url = "http://localhost:54942";
var CaseInstancesService = (function () {
    function CaseInstancesService(http) {
        this.http = http;
    }
    CaseInstancesService.prototype.create = function (caseDefId, caseId) {
        var request = JSON.stringify({ case_definition_id: caseDefId, case_id: caseId });
        var targetUrl = url + "/case-instances";
        var headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Content-Type', 'application/json');
        return this.http.post(targetUrl, request, { headers: headers }).pipe(map(function (res) {
            return CaseInstance.fromJson(res);
        }));
    };
    CaseInstancesService.prototype.launch = function (caseInstanceId) {
        var targetUrl = url + "/case-instances/" + caseInstanceId + "/launch";
        var headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        return this.http.get(targetUrl, { headers: headers });
    };
    CaseInstancesService.prototype.search = function (startIndex, count, templateId, order, direction) {
        var headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        var targetUrl = url + "/case-instances/.search?start_index=" + startIndex + "&count=" + count + "&template_id=" + templateId;
        if (order) {
            targetUrl = targetUrl + "&order_by=" + order;
        }
        if (direction) {
            targetUrl = targetUrl + "&order=" + direction;
        }
        return this.http.get(targetUrl, { headers: headers }).pipe(map(function (res) {
            return SearchCaseInstancesResult.fromJson(res);
        }));
    };
    CaseInstancesService = __decorate([
        Injectable(),
        __metadata("design:paramtypes", [HttpClient])
    ], CaseInstancesService);
    return CaseInstancesService;
}());
export { CaseInstancesService };
//# sourceMappingURL=caseinstances.service.js.map