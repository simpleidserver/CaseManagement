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
import { of } from 'rxjs';
import { map } from 'rxjs/operators';
var HumanTaskDefService = (function () {
    function HumanTaskDefService(http, oauthService) {
        this.http = http;
        this.oauthService = oauthService;
    }
    HumanTaskDefService.prototype.get = function (humanTaskDefId) {
        var headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        var targetUrl = process.env.HUMANTASK_API_URL + "/humantasksdefs/" + humanTaskDefId;
        return this.http.get(targetUrl, { headers: headers });
    };
    HumanTaskDefService.prototype.update = function (humanTaskDef) {
        return of(humanTaskDef);
    };
    HumanTaskDefService.prototype.addStartDeadline = function (id, deadline) {
        var headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Content-Type', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        var targetUrl = process.env.HUMANTASK_API_URL + "/humantasksdefs/" + id + "/deadlines/start";
        var request = { deadLine: deadline };
        return this.http.post(targetUrl, JSON.stringify(request), { headers: headers }).pipe(map(function (_) {
            deadline.id = _.id;
            return deadline;
        }));
    };
    HumanTaskDefService.prototype.addCompletionDeadline = function (id, deadline) {
        var headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Content-Type', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        var targetUrl = process.env.HUMANTASK_API_URL + "/humantasksdefs/" + id + "/deadlines/completion";
        var request = { deadLine: deadline };
        return this.http.post(targetUrl, JSON.stringify(request), { headers: headers }).pipe(map(function (_) {
            deadline.id = _.id;
            return deadline;
        }));
    };
    HumanTaskDefService.prototype.updateInfo = function (id, name, priority) {
        var headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Content-Type', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        var targetUrl = process.env.HUMANTASK_API_URL + "/humantasksdefs/" + id + "/info";
        var request = { name: name, priority: priority };
        return this.http.put(targetUrl, JSON.stringify(request), { headers: headers });
    };
    HumanTaskDefService.prototype.addInputParameter = function (id, parameter) {
        var headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Content-Type', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        var targetUrl = process.env.HUMANTASK_API_URL + "/humantasksdefs/" + id + "/parameters/input";
        var request = { parameter: parameter };
        return this.http.post(targetUrl, JSON.stringify(request), { headers: headers });
    };
    HumanTaskDefService.prototype.addOutputParameter = function (id, parameter) {
        var headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Content-Type', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        var targetUrl = process.env.HUMANTASK_API_URL + "/humantasksdefs/" + id + "/parameters/output";
        var request = { parameter: parameter };
        return this.http.post(targetUrl, JSON.stringify(request), { headers: headers });
    };
    HumanTaskDefService.prototype.deleteInputParameter = function (id, name) {
        var headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Content-Type', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        var targetUrl = process.env.HUMANTASK_API_URL + "/humantasksdefs/" + id + "/parameters/input/" + name;
        return this.http.delete(targetUrl, { headers: headers });
    };
    HumanTaskDefService.prototype.deleteOutputParameter = function (id, name) {
        var headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Content-Type', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        var targetUrl = process.env.HUMANTASK_API_URL + "/humantasksdefs/" + id + "/parameters/output/" + name;
        return this.http.delete(targetUrl, { headers: headers });
    };
    HumanTaskDefService.prototype.updateRendering = function (id, rendering) {
        var headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Content-Type', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        var targetUrl = process.env.HUMANTASK_API_URL + "/humantasksdefs/" + id + "/rendering";
        var request = { rendering: rendering };
        return this.http.put(targetUrl, JSON.stringify(request), { headers: headers });
    };
    HumanTaskDefService.prototype.deleteStartDeadline = function (id, deadLineId) {
        var headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Content-Type', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        var targetUrl = process.env.HUMANTASK_API_URL + "/humantasksdefs/" + id + "/deadlines/start/" + deadLineId;
        return this.http.delete(targetUrl, { headers: headers });
    };
    HumanTaskDefService.prototype.deleteCompletionDeadline = function (id, deadLineId) {
        var headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Content-Type', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        var targetUrl = process.env.HUMANTASK_API_URL + "/humantasksdefs/" + id + "/deadlines/completion/" + deadLineId;
        return this.http.delete(targetUrl, { headers: headers });
    };
    HumanTaskDefService.prototype.updateStartDealine = function (id, deadline) {
        var headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Content-Type', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        var request = { deadLineInfo: deadline };
        var targetUrl = process.env.HUMANTASK_API_URL + "/humantasksdefs/" + id + "/deadlines/start/" + deadline.id;
        return this.http.put(targetUrl, JSON.stringify(request), { headers: headers });
    };
    HumanTaskDefService.prototype.updateCompletionDealine = function (id, deadline) {
        var headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Content-Type', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        var request = { deadLineInfo: deadline };
        var targetUrl = process.env.HUMANTASK_API_URL + "/humantasksdefs/" + id + "/deadlines/completion/" + deadline.id;
        return this.http.put(targetUrl, JSON.stringify(request), { headers: headers });
    };
    HumanTaskDefService.prototype.addEscalationStartDeadline = function (id, startDeadlineId, condition) {
        var headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Content-Type', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        var request = { condition: condition };
        var targetUrl = process.env.HUMANTASK_API_URL + "/humantasksdefs/" + id + "/deadlines/start/" + startDeadlineId + "/escalations";
        return this.http.post(targetUrl, JSON.stringify(request), { headers: headers }).pipe(map(function (_) {
            return _.id;
        }));
    };
    HumanTaskDefService.prototype.addEscalationCompletionDeadline = function (id, startDeadlineId, condition) {
        var headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Content-Type', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        var request = { condition: condition };
        var targetUrl = process.env.HUMANTASK_API_URL + "/humantasksdefs/" + id + "/deadlines/completion/" + startDeadlineId + "/escalations";
        return this.http.post(targetUrl, JSON.stringify(request), { headers: headers }).pipe(map(function (_) {
            return _.id;
        }));
    };
    HumanTaskDefService.prototype.updatePeopleAssignment = function (id, assignment) {
        var headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Content-Type', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        var request = { peopleAssignment: assignment };
        var targetUrl = process.env.HUMANTASK_API_URL + "/humantasksdefs/" + id + "/assignment";
        return this.http.put(targetUrl, JSON.stringify(request), { headers: headers });
    };
    HumanTaskDefService.prototype.updateStartEscalation = function (id, deadLineId, escalation) {
        var headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Content-Type', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        var request = { escalation: escalation };
        var targetUrl = process.env.HUMANTASK_API_URL + "/humantasksdefs/" + id + "/deadlines/start/" + deadLineId + "/escalations/" + escalation.id;
        return this.http.put(targetUrl, JSON.stringify(request), { headers: headers });
    };
    HumanTaskDefService.prototype.updateCompletionEscalation = function (id, deadLineId, escalation) {
        var headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Content-Type', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        var request = { escalation: escalation };
        var targetUrl = process.env.HUMANTASK_API_URL + "/humantasksdefs/" + id + "/deadlines/completion/" + deadLineId + "/escalations/" + escalation.id;
        return this.http.put(targetUrl, JSON.stringify(request), { headers: headers });
    };
    HumanTaskDefService.prototype.deleteCompletionEscalation = function (id, deadLineId, escalation) {
        var headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Content-Type', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        var targetUrl = process.env.HUMANTASK_API_URL + "/humantasksdefs/" + id + "/deadlines/completion/" + deadLineId + "/escalations/" + escalation.id;
        return this.http.delete(targetUrl, { headers: headers });
    };
    HumanTaskDefService.prototype.deleteStartEscalation = function (id, deadLineId, escalation) {
        var headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Content-Type', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        var targetUrl = process.env.HUMANTASK_API_URL + "/humantasksdefs/" + id + "/deadlines/start/" + deadLineId + "/escalations/" + escalation.id;
        return this.http.delete(targetUrl, { headers: headers });
    };
    HumanTaskDefService.prototype.addHumanTask = function (name) {
        var headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        var request = { name: name };
        var targetUrl = process.env.HUMANTASK_API_URL + "/humantasksdefs";
        return this.http.post(targetUrl, request, { headers: headers });
    };
    HumanTaskDefService.prototype.search = function (startIndex, count, order, direction) {
        var headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Content-Type', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        var targetUrl = process.env.HUMANTASK_API_URL + "/humantasksdefs/.search";
        var request = { startIndex: startIndex, count: count };
        if (order) {
            request["orderBy"] = order;
        }
        if (direction) {
            request["order"] = direction;
        }
        return this.http.post(targetUrl, JSON.stringify(request), { headers: headers });
    };
    HumanTaskDefService.prototype.updatePresentationElement = function (id, presentationElement) {
        var headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Content-Type', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        var targetUrl = process.env.HUMANTASK_API_URL + "/humantasksdefs/" + id + "/presentationelts";
        var request = { presentationElement: presentationElement };
        return this.http.put(targetUrl, JSON.stringify(request), { headers: headers });
    };
    HumanTaskDefService = __decorate([
        Injectable(),
        __metadata("design:paramtypes", [HttpClient, OAuthService])
    ], HumanTaskDefService);
    return HumanTaskDefService;
}());
export { HumanTaskDefService };
//# sourceMappingURL=humantaskdef.service.js.map