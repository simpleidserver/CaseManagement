import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { OAuthService } from 'angular-oauth2-oidc';
import { Observable } from 'rxjs';
import { NotificationDefinition } from '../models/notificationdef.model';
import { SearchNotificationDefsResult } from '../models/searchnotificationdef.model';
import { Parameter } from '../../common/parameter.model';
import { PresentationParameter } from '../../common/presentationparameter.model';
import { PresentationElement } from '../../common/presentationelement.model';
import { PeopleAssignment } from '../../common/people-assignment.model';

@Injectable()
export class NotificationDefService {
    constructor(private http: HttpClient, private oauthService: OAuthService) { }

    search(startIndex: number, count: number, order: string, direction: string): Observable<SearchNotificationDefsResult> {
        let headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Content-Type', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        const targetUrl = process.env.HUMANTASK_API_URL + "/notificationdefs/.search";
        const request: any = { startIndex: startIndex, count: count };
        if (order) {
            request["orderBy"] = order;
        }

        if (direction) {
            request["order"] = direction;
        }

        return this.http.post<SearchNotificationDefsResult>(targetUrl, JSON.stringify(request), { headers: headers });
    }

    addNotification(name: string): Observable<NotificationDefinition> {
        let headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        const request: any = { name: name };
        const targetUrl = process.env.HUMANTASK_API_URL + "/notificationdefs";
        return this.http.post<NotificationDefinition>(targetUrl, request, { headers: headers });
    }

    getNotification(id: string): Observable<NotificationDefinition> {
        let headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        const targetUrl = process.env.HUMANTASK_API_URL + "/notificationdefs/" + id;
        return this.http.get<NotificationDefinition>(targetUrl, { headers: headers });
    }

    updateInfo(id: string, name: string, priority: number): Observable<boolean> {
        let headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Content-Type', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        const targetUrl = process.env.HUMANTASK_API_URL + "/notificationdefs/" + id + "/info";
        const request: any = { name: name, priority: priority };
        return this.http.put<boolean>(targetUrl, JSON.stringify(request), { headers: headers });
    }

    addParameter(id: string, parameter: Parameter): Observable<string> {
        let headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Content-Type', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        const targetUrl = process.env.HUMANTASK_API_URL + "/notificationdefs/" + id + "/parameters";
        const request: any = { parameter: parameter };
        return this.http.post<string>(targetUrl, JSON.stringify(request), { headers: headers });
    }

    deleteParameter(id: string, parameterId: string): Observable<boolean> {
        let headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Content-Type', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        const targetUrl = process.env.HUMANTASK_API_URL + "/notificationdefs/" + id + "/parameters/" + parameterId;
        return this.http.delete<boolean>(targetUrl, { headers: headers });
    }

    addPresentationParameter(id: string, presentationParameter: PresentationParameter): Observable<boolean> {
        let headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Content-Type', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        const targetUrl = process.env.HUMANTASK_API_URL + "/notificationdefs/" + id + "/presentationparameters";
        const request: any = { presentationParameter: presentationParameter };
        return this.http.post<boolean>(targetUrl, JSON.stringify(request), { headers: headers });
    }

    deletePresentationParameter(id: string, presentationParameter: PresentationParameter): Observable<boolean> {
        let headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Content-Type', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        const targetUrl = process.env.HUMANTASK_API_URL + "/notificationdefs/" + id + "/presentationparameters/" + presentationParameter.name;
        return this.http.delete<boolean>(targetUrl, { headers: headers });
    }

    addPresentationElt(id: string, presentationElt: PresentationElement): Observable<boolean> {
        let headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Content-Type', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        const targetUrl = process.env.HUMANTASK_API_URL + "/notificationdefs/" + id + "/presentationelts";
        const request: any = { presentationElement: presentationElt };
        return this.http.post<boolean>(targetUrl, JSON.stringify(request), { headers: headers });
    }

    deletePresentationElt(id: string, presentationElt: PresentationElement): Observable<boolean> {
        let headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Content-Type', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        const targetUrl = process.env.HUMANTASK_API_URL + "/notificationdefs/" + id + "/presentationelts/" + presentationElt.usage + '/' + presentationElt.language;
        return this.http.delete<boolean>(targetUrl, { headers: headers });
    }

    addPeopleAssignment(id: string, peopleAssignment: PeopleAssignment): Observable<string> {
        let headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Content-Type', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        const targetUrl = process.env.HUMANTASK_API_URL + "/notificationdefs/" + id + "/assignments";
        const request: any = { peopleAssignment: peopleAssignment };
        return this.http.post<string>(targetUrl, JSON.stringify(request), { headers: headers });
    }

    deletePeopleAssignment(id: string, peopleAssignment: PeopleAssignment): Observable<boolean> {
        let headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Content-Type', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        const targetUrl = process.env.HUMANTASK_API_URL + "/notificationdefs/" + id + "/assignments/" + peopleAssignment.id;
        return this.http.delete<boolean>(targetUrl, { headers: headers });
    }

    getAll(): Observable<NotificationDefinition[]> {
        let headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Content-Type', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        const targetUrl = process.env.HUMANTASK_API_URL + "/notificationdefs";
        return this.http.get<NotificationDefinition[]>(targetUrl, { headers: headers });
    }
}