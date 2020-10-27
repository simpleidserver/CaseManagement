import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { OAuthService } from 'angular-oauth2-oidc';
import { Observable } from 'rxjs';
import { SearchTasksResult } from '../models/search-tasks-result.model';
import { TranslateService } from '@ngx-translate/core';

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
}