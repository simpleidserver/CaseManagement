import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { OAuthService } from 'angular-oauth2-oidc';
import { Observable, of } from 'rxjs';
import { Escalation } from '../../common/escalation.model';
import { Parameter } from '../../common/operation.model';
import { Rendering } from '../../common/rendering.model';
import { HumanTaskDefAssignment } from '../models/humantaskdef-assignment.model';
import { HumanTaskDefinitionDeadLine } from '../models/humantaskdef-deadlines';
import { HumanTaskDef } from '../models/humantaskdef.model';
import { SearchHumanTaskDefsResult } from '../models/searchhumantaskdef.model';
import { PresentationElement } from '../../common/presentationelement.model';
import { map } from 'rxjs/operators';

@Injectable()
export class HumanTaskDefService {
    constructor(private http: HttpClient, private oauthService: OAuthService) { }

    get(humanTaskDefId: string): Observable<HumanTaskDef> {
        let headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        const targetUrl = process.env.HUMANTASK_API_URL + "/humantasksdefs/" + humanTaskDefId;
        return this.http.get<HumanTaskDef>(targetUrl, { headers: headers });
    }

    update(humanTaskDef: HumanTaskDef): Observable<HumanTaskDef> {
        return of(humanTaskDef);
    }

    addStartDeadline(id: string, deadline: HumanTaskDefinitionDeadLine): Observable<HumanTaskDefinitionDeadLine> {
        let headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Content-Type', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        const targetUrl = process.env.HUMANTASK_API_URL + "/humantasksdefs/" + id + "/deadlines/start";
        const request: any = { deadLine: deadline };
        return this.http.post<string>(targetUrl, JSON.stringify(request), { headers: headers }).pipe(map((_: any) => {
            deadline.id = _.id;
            return deadline;
        }));
    }

    addCompletionDeadline(id: string, deadline: HumanTaskDefinitionDeadLine): Observable<HumanTaskDefinitionDeadLine> {
        let headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Content-Type', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        const targetUrl = process.env.HUMANTASK_API_URL + "/humantasksdefs/" + id + "/deadlines/completion";
        const request: any = { deadLine: deadline };
        return this.http.post<string>(targetUrl, JSON.stringify(request), { headers: headers }).pipe(map((_: any) => {
            deadline.id = _.id;
            return deadline;
        }));
    }

    updateInfo(id: string, name: string, priority: number): Observable<boolean> {
        let headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Content-Type', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        const targetUrl = process.env.HUMANTASK_API_URL + "/humantasksdefs/" + id + "/info";
        const request: any = { name: name, priority: priority};
        return this.http.put<boolean>(targetUrl, JSON.stringify(request), { headers: headers });
    }

    addInputParameter(id: string, parameter: Parameter): Observable<boolean> {
        let headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Content-Type', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        const targetUrl = process.env.HUMANTASK_API_URL + "/humantasksdefs/" + id + "/parameters/input";
        const request: any = { parameter: parameter };
        return this.http.post<boolean>(targetUrl, JSON.stringify(request), { headers: headers });
    }

    addOutputParameter(id: string, parameter: Parameter): Observable<boolean> {
        let headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Content-Type', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        const targetUrl = process.env.HUMANTASK_API_URL + "/humantasksdefs/" + id + "/parameters/output";
        const request: any = { parameter: parameter };
        return this.http.post<boolean>(targetUrl, JSON.stringify(request), { headers: headers });
    }

    deleteInputParameter(id: string, name: string): Observable<boolean> {
        let headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Content-Type', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        const targetUrl = process.env.HUMANTASK_API_URL + "/humantasksdefs/" + id + "/parameters/input/" + name;
        return this.http.delete<boolean>(targetUrl, { headers: headers });
    }

    deleteOutputParameter(id: string, name: string): Observable<boolean> {
        let headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Content-Type', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        const targetUrl = process.env.HUMANTASK_API_URL + "/humantasksdefs/" + id + "/parameters/output/" + name;
        return this.http.delete<boolean>(targetUrl, { headers: headers });
    }

    updateRendering(id: string, rendering: Rendering): Observable<boolean> {
        if (id) { }
        if (rendering) { }

        return of(true);
    }

    deleteStartDeadline(id: string, deadLineId: string): Observable<boolean> {
        let headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Content-Type', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        const targetUrl = process.env.HUMANTASK_API_URL + "/humantasksdefs/" + id + "/deadlines/start/" + deadLineId;
        return this.http.delete<boolean>(targetUrl, { headers: headers });
    }

    deleteCompletionDeadline(id: string, deadLineId: string): Observable<boolean> {
        let headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Content-Type', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        const targetUrl = process.env.HUMANTASK_API_URL + "/humantasksdefs/" + id + "/deadlines/completion/" + deadLineId;
        return this.http.delete<boolean>(targetUrl, { headers: headers });
    }

    updateStartDealine(id: string, deadline: HumanTaskDefinitionDeadLine): Observable<boolean> {
        let headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Content-Type', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        const request: any = { deadLineInfo: deadline };
        const targetUrl = process.env.HUMANTASK_API_URL + "/humantasksdefs/" + id + "/deadlines/start/" + deadline.id;
        return this.http.put<boolean>(targetUrl, JSON.stringify(request), { headers: headers });
    }

    updateCompletionDealine(id: string, deadline: HumanTaskDefinitionDeadLine): Observable<boolean> {
        let headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Content-Type', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        const request: any = { deadLineInfo: deadline };
        const targetUrl = process.env.HUMANTASK_API_URL + "/humantasksdefs/" + id + "/deadlines/completion/" + deadline.id;
        return this.http.put<boolean>(targetUrl, JSON.stringify(request), { headers: headers });
    }

    addEscalationStartDeadline(id: string, startDeadlineId: string, condition: string): Observable<string> {
        let headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Content-Type', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        const request: any = { condition: condition };
        const targetUrl = process.env.HUMANTASK_API_URL + "/humantasksdefs/" + id + "/deadlines/start/" + startDeadlineId + "/escalations";
        return this.http.post<string>(targetUrl, JSON.stringify(request), { headers: headers }).pipe(map((_: any) => {
            return _.id;
        }));
    }

    addEscalationCompletionDeadline(id: string, startDeadlineId: string, condition: string): Observable<string> {
        let headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Content-Type', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        const request: any = { condition: condition };
        const targetUrl = process.env.HUMANTASK_API_URL + "/humantasksdefs/" + id + "/deadlines/completion/" + startDeadlineId + "/escalations";
        return this.http.post<string>(targetUrl, JSON.stringify(request), { headers: headers }).pipe(map((_: any) => {
            return _.id;
        }));
    }

    updatePeopleAssignment(id: string, assignment: HumanTaskDefAssignment): Observable<boolean> {
        let headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Content-Type', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        const request: any = { peopleAssignment: assignment };
        const targetUrl = process.env.HUMANTASK_API_URL + "/humantasksdefs/" + id + "/assignment";
        return this.http.put<boolean>(targetUrl, JSON.stringify(request), { headers: headers });
    }

    updateStartEscalation(id: string, deadLineId: string, escalation: Escalation): Observable<boolean> {
        let headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Content-Type', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        const request: any = { escalation: escalation};
        const targetUrl = process.env.HUMANTASK_API_URL + "/humantasksdefs/" + id + "/deadlines/start/" + deadLineId + "/escalations/" + escalation.id;
        return this.http.put<boolean>(targetUrl, JSON.stringify(request), { headers: headers });
    }

    updateCompletionEscalation(id: string, deadLineId: string, escalation: Escalation): Observable<boolean> {
        let headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Content-Type', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        const request: any = { escalation: escalation };
        const targetUrl = process.env.HUMANTASK_API_URL + "/humantasksdefs/" + id + "/deadlines/completion/" + deadLineId + "/escalations/" + escalation.id;
        return this.http.put<boolean>(targetUrl, JSON.stringify(request), { headers: headers });
    }

    deleteCompletionEscalation(id: string, deadLineId: string, escalation: Escalation): Observable<boolean> {
        let headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Content-Type', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        const targetUrl = process.env.HUMANTASK_API_URL + "/humantasksdefs/" + id + "/deadlines/completion/" + deadLineId + "/escalations/" + escalation.id;
        return this.http.delete<boolean>(targetUrl, { headers: headers });
    }

    deleteStartEscalation(id: string, deadLineId: string, escalation: Escalation): Observable<boolean> {
        let headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Content-Type', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        const targetUrl = process.env.HUMANTASK_API_URL + "/humantasksdefs/" + id + "/deadlines/start/" + deadLineId + "/escalations/" + escalation.id;
        return this.http.delete<boolean>(targetUrl, { headers: headers });
    }

    addHumanTask(name: string): Observable<HumanTaskDef> {
        let headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        const request: any = { name: name };
        const targetUrl = process.env.HUMANTASK_API_URL + "/humantasksdefs";
        return this.http.post<HumanTaskDef>(targetUrl, request, { headers: headers });
    }

    search(startIndex: number, count: number, order: string, direction: string): Observable<SearchHumanTaskDefsResult> {
        let headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Content-Type', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        const targetUrl = process.env.HUMANTASK_API_URL + "/humantasksdefs/.search";
        const request: any = { startIndex: startIndex, count: count };
        if (order) {
            request["orderBy"] = order;
        }

        if (direction) {
            request["order"] = direction;
        }

        return this.http.post<SearchHumanTaskDefsResult>(targetUrl, JSON.stringify(request), { headers: headers });
    }

    updatePresentationElement(id: string, presentationElement: PresentationElement): Observable<boolean> {
        let headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Content-Type', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        const targetUrl = process.env.HUMANTASK_API_URL + "/humantasksdefs/" + id + "/presentationelts";
        const request: any = { presentationElement: presentationElement };
        return this.http.put<boolean>(targetUrl, JSON.stringify(request), { headers: headers });
    }
}