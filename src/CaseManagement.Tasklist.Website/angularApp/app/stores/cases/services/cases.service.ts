import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { OAuthService } from 'angular-oauth2-oidc';
import { Observable } from 'rxjs';
import { SearchCaseInstanceResult } from '../models/search-caseinstance.model';
import { CaseInstance } from '../models/caseinstance.model';

@Injectable()
export class CasesService {
    constructor(private http: HttpClient,
        private oauthService: OAuthService,
        private translate: TranslateService) { }

    search(startIndex: number, count: number, order: string, direction: string): Observable<SearchCaseInstanceResult> {
        let headers = new HttpHeaders();
        const defaultLang = this.translate.currentLang;
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Content-Type', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getAccessToken());
        headers = headers.set('Accept-Language', defaultLang);
        const targetUrl = process.env.CM_API_URL + "/case-plan-instances/search";
        const request: any = { startIndex: startIndex, count: count };
        if (order) {
            request["orderBy"] = order;
        }

        if (direction) {
            request["order"] = direction;
        }

        return this.http.post<SearchCaseInstanceResult>(targetUrl, JSON.stringify(request), { headers: headers });
    }

    get(id: string): Observable<CaseInstance> {
        let headers = new HttpHeaders();
        const defaultLang = this.translate.currentLang;
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Content-Type', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getAccessToken());
        headers = headers.set('Accept-Language', defaultLang);
        const targetUrl = process.env.CM_API_URL + "/case-plan-instances/" + id;
        return this.http.get<CaseInstance>(targetUrl, { headers: headers });
    }

    activate(id: string, elt: string): Observable<any> {
        let headers = new HttpHeaders();
        const defaultLang = this.translate.currentLang;
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Content-Type', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getAccessToken());
        headers = headers.set('Accept-Language', defaultLang);
        const targetUrl = process.env.CM_API_URL + "/case-plan-instances/" + id + "/activate/" + elt;
        const request: any = {};
        return this.http.post(targetUrl, JSON.stringify(request), { headers: headers });
    }

    disable(id: string, elt: string): Observable<any> {
        let headers = new HttpHeaders();
        const defaultLang = this.translate.currentLang;
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Content-Type', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getAccessToken());
        headers = headers.set('Accept-Language', defaultLang);
        const targetUrl = process.env.CM_API_URL + "/case-plan-instances/" + id + "/disable/" + elt;
        const request: any = {};
        return this.http.post(targetUrl, JSON.stringify(request), { headers: headers });
    }

    reenable(id: string, elt: string): Observable<any> {
        let headers = new HttpHeaders();
        const defaultLang = this.translate.currentLang;
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Content-Type', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getAccessToken());
        headers = headers.set('Accept-Language', defaultLang);
        const targetUrl = process.env.CM_API_URL + "/case-plan-instances/" + id + "/reenable/" + elt;
        const request: any = {};
        return this.http.post(targetUrl, JSON.stringify(request), { headers: headers });
    }

    complete(id: string, elt: string): Observable<any> {
        let headers = new HttpHeaders();
        const defaultLang = this.translate.currentLang;
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Content-Type', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getAccessToken());
        headers = headers.set('Accept-Language', defaultLang);
        const request: any = { };
        const targetUrl = process.env.CM_API_URL + "/case-plan-instances/" + id + "/complete/" + elt;
        return this.http.post<SearchCaseInstanceResult>(targetUrl, JSON.stringify(request), { headers: headers });
    }
}