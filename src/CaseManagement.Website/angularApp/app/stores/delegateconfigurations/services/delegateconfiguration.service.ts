import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { OAuthService } from 'angular-oauth2-oidc';
import { Observable } from 'rxjs';
import { DelegateConfiguration } from '../models/delegateconfiguration.model';
import { SearchDelegateConfigurationResult } from '../models/searchdelegateconfiguration.model';

@Injectable()
export class DelegateConfigurationService {
    constructor(private http: HttpClient, private oauthService: OAuthService) { }

    search(startIndex: number, count: number, order: string, direction: string): Observable<SearchDelegateConfigurationResult> {
        let headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        const targetUrl = process.env.BPMN_API_URL + "/delegateconfigurations/search";
        const request: any = { startIndex: startIndex, count: count };
        if (order) {
            request["orderBy"] = order;
        }

        if (direction) {
            request["order"] = direction;
        }

        return this.http.post<SearchDelegateConfigurationResult>(targetUrl, request, { headers: headers });
    }

    get(id: string): Observable<DelegateConfiguration> {
        let headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        const targetUrl = process.env.BPMN_API_URL + "/delegateconfigurations/" + id;
        return this.http.get<DelegateConfiguration>(targetUrl, { headers : headers });
    }

    getAll(): Observable<DelegateConfiguration[]> {
        let headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        const targetUrl = process.env.BPMN_API_URL + "/delegateconfigurations";
        return this.http.get<DelegateConfiguration[]>(targetUrl, { headers: headers });
    }

    update(id: string, records: any): Observable<any> {
        let headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        const targetUrl = process.env.BPMN_API_URL + "/delegateconfigurations/" + id;
        return this.http.put(targetUrl, { records: records }, { headers: headers });
    }
}