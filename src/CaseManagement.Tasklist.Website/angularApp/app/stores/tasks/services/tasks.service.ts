import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { OAuthService } from 'angular-oauth2-oidc';
import { Observable } from 'rxjs';
import { SearchTasksResult } from '../models/search-tasks-result.model';
import { NominateParameter } from '../parameters/nominate-parameter';

@Injectable()
export class TasksService {
    constructor(private http: HttpClient,
        private oauthService: OAuthService,
        private translate: TranslateService) { }

    search(startIndex: number, count: number, order: string, direction: string, owner: string, status: string[]): Observable<SearchTasksResult> {
        let headers = new HttpHeaders();
        const defaultLang = this.translate.currentLang;
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Content-Type', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        headers = headers.set('Accept-Language', defaultLang);
        const targetUrl = process.env.API_URL + "/humantaskinstances/.search";
        const request: any = { startIndex: startIndex, count: count, actualOwner: owner, statusLst: status };
        if (order) {
            request["orderBy"] = order;
        }

        if (direction) {
            request["order"] = direction;
        }

        return this.http.post<SearchTasksResult>(targetUrl, JSON.stringify(request), { headers: headers });
    }

    start(humanTaskInstanceId: string): Observable<boolean> {
        let headers = new HttpHeaders();
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        const targetUrl = process.env.API_URL + "/humantaskinstances/" + humanTaskInstanceId + "/start";
        return this.http.get<boolean>(targetUrl, { headers: headers });
    }

    claim(humanTaskInstanceId: string): Observable<boolean> {
        let headers = new HttpHeaders();
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        const targetUrl = process.env.API_URL + "/humantaskinstances/" + humanTaskInstanceId + "/claim";
        return this.http.get<boolean>(targetUrl, { headers: headers });
    }

    nominate(humanTaskInstanceId: string, nominate: NominateParameter): Observable<boolean> {
        let headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Content-Type', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        const targetUrl = process.env.API_URL + "/humantaskinstances/" + humanTaskInstanceId + "/nominate";
        return this.http.post<boolean>(targetUrl, JSON.stringify(nominate), { headers: headers });
    }
}