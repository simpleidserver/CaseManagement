import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { OAuthService } from 'angular-oauth2-oidc';
import { Observable, of } from 'rxjs';
import { Escalation } from '../../common/escalation.model';
import { Parameter } from '../../common/parameter.model';
import { HumanTaskDef } from '../models/humantaskdef.model';
import { SearchHumanTaskDefsResult } from '../models/searchhumantaskdef.model';
import { PresentationElement } from '../../common/presentationelement.model';
import { map } from 'rxjs/operators';
import { Deadline } from '../models/deadline';
import { RenderingElement } from '../../common/rendering.model';
import { PeopleAssignment } from '../../common/people-assignment.model';
import { PresentationParameter } from '../../common/presentationparameter.model';
import { ToPart } from '../../common/topart.model';

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

    getAll() : Observable<HumanTaskDef[]> {
        let headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        const targetUrl = process.env.HUMANTASK_API_URL + '/humantasksdefs';
        return this.http.get<HumanTaskDef[]>(targetUrl, { headers: headers });
    }

    update(humanTaskDef: HumanTaskDef): Observable<HumanTaskDef> {
        return of(humanTaskDef);
    }

    addDeadline(id: string, deadline: Deadline): Observable<Deadline> {
        let headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Content-Type', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        const targetUrl = process.env.HUMANTASK_API_URL + "/humantasksdefs/" + id + "/deadlines";
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

    addParameter(id: string, parameter: Parameter): Observable<string> {
        let headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Content-Type', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        const targetUrl = process.env.HUMANTASK_API_URL + "/humantasksdefs/" + id + "/parameters";
        const request: any = { parameter: parameter };
        return this.http.post<string>(targetUrl, JSON.stringify(request), { headers: headers });
    }

    deleteParameter(id: string, parameterId: string): Observable<boolean> {
        let headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Content-Type', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        const targetUrl = process.env.HUMANTASK_API_URL + "/humantasksdefs/" + id + "/parameters/" + parameterId;
        return this.http.delete<boolean>(targetUrl, { headers: headers });
    }

    updateRendering(id: string, renderingElements: RenderingElement[]): Observable<boolean> {
        let headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Content-Type', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        const targetUrl = process.env.HUMANTASK_API_URL + "/humantasksdefs/" + id + "/rendering";
        const request: any = { renderingElements: renderingElements};
        return this.http.put<boolean>(targetUrl, JSON.stringify(request), { headers: headers });
    }

    deleteDeadline(id: string, deadLineId: string): Observable<boolean> {
        let headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Content-Type', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        const targetUrl = process.env.HUMANTASK_API_URL + "/humantasksdefs/" + id + "/deadlines/" + deadLineId;
        return this.http.delete<boolean>(targetUrl, { headers: headers });
    }

    updateDealine(id: string, deadline: Deadline): Observable<boolean> {
        let headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Content-Type', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        const request: any = { deadLineInfo: deadline };
        const targetUrl = process.env.HUMANTASK_API_URL + "/humantasksdefs/" + id + "/deadlines/" + deadline.id;
        return this.http.put<boolean>(targetUrl, JSON.stringify(request), { headers: headers });
    }

    addEscalationDeadline(id: string, deadlineId: string, condition: string): Observable<string> {
        let headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Content-Type', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        const request: any = { condition: condition };
        const targetUrl = process.env.HUMANTASK_API_URL + "/humantasksdefs/" + id + "/deadlines/" + deadlineId + "/escalations";
        return this.http.post<string>(targetUrl, JSON.stringify(request), { headers: headers }).pipe(map((_: any) => {
            return _.id;
        }));
    }

    updateEscalation(id: string, deadLineId: string, escalation: Escalation): Observable<boolean> {
        let headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Content-Type', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        const request: any = { escalation: escalation};
        const targetUrl = process.env.HUMANTASK_API_URL + "/humantasksdefs/" + id + "/deadlines/" + deadLineId + "/escalations/" + escalation.id;
        return this.http.put<boolean>(targetUrl, JSON.stringify(request), { headers: headers });
    }

    deleteEscalation(id: string, deadLineId: string, escalation: Escalation): Observable<boolean> {
        let headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Content-Type', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        const targetUrl = process.env.HUMANTASK_API_URL + "/humantasksdefs/" + id + "/deadlines/" + deadLineId + "/escalations/" + escalation.id;
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

    updatePresentationElement(id: string, presentationElements: PresentationElement[], presentationParameters: PresentationParameter[]): Observable<boolean> {
        let headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Content-Type', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        const targetUrl = process.env.HUMANTASK_API_URL + "/humantasksdefs/" + id + "/presentationelts";
        const request: any = { presentationElements: presentationElements, presentationParameters: presentationParameters };
        return this.http.put<boolean>(targetUrl, JSON.stringify(request), { headers: headers });
    }

    addPresentationParameter(id: string, presentationParameter: PresentationParameter): Observable<boolean> {
        let headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Content-Type', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        const targetUrl = process.env.HUMANTASK_API_URL + "/humantasksdefs/" + id + "/presentationparameters";
        const request: any = { presentationParameter: presentationParameter };
        return this.http.post<boolean>(targetUrl, JSON.stringify(request), { headers: headers });
    }

    deletePresentationParameter(id: string, presentationParameter: PresentationParameter): Observable<boolean> {
        let headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Content-Type', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        const targetUrl = process.env.HUMANTASK_API_URL + "/humantasksdefs/" + id + "/presentationparameters/" + presentationParameter.name;
        return this.http.delete<boolean>(targetUrl, { headers: headers });
    }

    addPresentationElt(id: string, presentationElt: PresentationElement): Observable<boolean> {
        let headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Content-Type', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        const targetUrl = process.env.HUMANTASK_API_URL + "/humantasksdefs/" + id + "/presentationelts";
        const request: any = { presentationElement: presentationElt };
        return this.http.post<boolean>(targetUrl, JSON.stringify(request), { headers: headers });
    }

    deletePresentationElt(id: string, presentationElt: PresentationElement): Observable<boolean> {
        let headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Content-Type', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        const targetUrl = process.env.HUMANTASK_API_URL + "/humantasksdefs/" + id + "/presentationelts/" + presentationElt.usage + '/' + presentationElt.language;
        return this.http.delete<boolean>(targetUrl, { headers: headers });
    }

    addPeopleAssignment(id: string, peopleAssignment: PeopleAssignment): Observable<string> {
        let headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Content-Type', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        const targetUrl = process.env.HUMANTASK_API_URL + "/humantasksdefs/" + id + "/assignments";
        const request: any = { peopleAssignment: peopleAssignment };
        return this.http.post<string>(targetUrl, JSON.stringify(request), { headers: headers });
    }

    deletePeopleAssignment(id: string, peopleAssignment: PeopleAssignment): Observable<boolean> {
        let headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Content-Type', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        const targetUrl = process.env.HUMANTASK_API_URL + "/humantasksdefs/" + id + "/assignments/" + peopleAssignment.id;
        return this.http.delete<boolean>(targetUrl, { headers: headers });
    }

    addEscalationToPart(id: string, deadlineId: string, escalationId: string, toPart: ToPart) {
        let headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Content-Type', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        const request: any = { toPart: toPart };
        const targetUrl = process.env.HUMANTASK_API_URL + "/humantasksdefs/" + id + "/deadlines/" + deadlineId + "/escalations/" + escalationId + "/toparts";
        return this.http.post<boolean>(targetUrl, JSON.stringify(request),  { headers: headers });
    }

    deleteEscalationToPart(id: string, deadlineId: string, escalationId: string, toPartName: string) {
        let headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Content-Type', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        const targetUrl = process.env.HUMANTASK_API_URL + "/humantasksdefs/" + id + "/deadlines/" + deadlineId + "/escalations/" + escalationId + "/toparts/" + toPartName;
        return this.http.delete<boolean>(targetUrl, { headers: headers });

    }
}