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
import { SearchCaseActivationsResult } from '../models/search-case-activations-result.model';
var CaseActivationsService = (function () {
    function CaseActivationsService(http) {
        this.http = http;
    }
    CaseActivationsService.prototype.search = function (caseDefinitionId, startIndex, count, order, direction) {
        var headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        var targetUrl = process.env.API_URL + "/case-activations/search?start_index=" + startIndex + "&count=" + count + "&case_definition_id=" + caseDefinitionId;
        if (order) {
            targetUrl = targetUrl + "&order_by=" + order;
        }
        if (direction) {
            targetUrl = targetUrl + "&order=" + direction;
        }
        return this.http.get(targetUrl, { headers: headers }).pipe(map(function (res) {
            return SearchCaseActivationsResult.fromJson(res);
        }));
    };
    CaseActivationsService = __decorate([
        Injectable(),
        __metadata("design:paramtypes", [HttpClient])
    ], CaseActivationsService);
    return CaseActivationsService;
}());
export { CaseActivationsService };
//# sourceMappingURL=caseactivations.service.js.map