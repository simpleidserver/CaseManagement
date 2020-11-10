import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { OAuthService } from 'angular-oauth2-oidc';
import { Observable, of } from 'rxjs';
import { SearchTasksResult } from '../models/search-tasks-result.model';
import { Task } from '../models/task.model';
import { NominateParameter } from '../parameters/nominate-parameter';
import { map, catchError } from 'rxjs/operators';
import { SearchTaskHistoryResult } from '../models/search-task-history-result.model';
import { RenderingElement } from '../models/rendering';

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

    getRendering(humanTaskInstanceId: string): Observable<RenderingElement[]> {
        let headers = new HttpHeaders();
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        headers = headers.set('Content-Type', 'application/json');
        const targetUrl = process.env.API_URL + "/humantaskinstances/" + humanTaskInstanceId + "/rendering";
        return this.http.get<RenderingElement[]>(targetUrl, { headers: headers }).pipe(
            map((r) => { return r; }),
            catchError(() => { return of([]); }));
    }

    getDetails(humanTaskInstanceId: string): Observable<Task> {
        let headers = new HttpHeaders();
        const defaultLang = this.translate.currentLang;
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        headers = headers.set('Content-Type', 'application/json');
        headers = headers.set('Accept-Language', defaultLang);
        const targetUrl = process.env.API_URL + "/humantaskinstances/" + humanTaskInstanceId + "/details";
        return this.http.get<Task>(targetUrl, { headers: headers }).pipe(
            map((r) => { return r; }),
            catchError(() => { return of(new Task()); }));
    }

    getDescription(humanTaskInstanceId: string): Observable<string> {
        let headers = new HttpHeaders();
        const defaultLang = this.translate.currentLang;
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        headers = headers.set('Accept-Language', defaultLang);
        const targetUrl = process.env.API_URL + "/humantaskinstances/" + humanTaskInstanceId + "/description";
        return this.http.get<string>(targetUrl, { headers: headers }).pipe(
            map((r) => { return r; }),
            catchError(() => { return of(""); }));
    }

    searchTaskHistory(humanTaskInstanceId: string, startIndex: number, count: number, order: string, direction: string): Observable<SearchTaskHistoryResult> {
        let headers = new HttpHeaders();
        const defaultLang = this.translate.currentLang;
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Content-Type', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        headers = headers.set('Accept-Language', defaultLang);
        const targetUrl = process.env.API_URL + "/humantaskinstances/" + humanTaskInstanceId + "/history";
        const request: any = { startIndex: startIndex, count: count };
        if (order) {
            request["orderBy"] = order;
        }

        if (direction) {
            request["order"] = direction;
        }

        return this.http.post<SearchTaskHistoryResult>(targetUrl, JSON.stringify(request), { headers: headers });
    }

    completeTask(humanTaskInstanceId: string, operationParameters: any) : Observable<boolean> {
        let headers = new HttpHeaders();
        const defaultLang = this.translate.currentLang;
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Content-Type', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        headers = headers.set('Accept-Language', defaultLang);
        const targetUrl = process.env.API_URL + "/humantaskinstances/" + humanTaskInstanceId + "/complete";
        const request: any = { operationParameters: operationParameters };
        return this.http.post<boolean>(targetUrl, JSON.stringify(request), { headers: headers });
    }
}