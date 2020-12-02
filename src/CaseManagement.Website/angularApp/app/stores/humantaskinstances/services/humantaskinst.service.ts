import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { OAuthService } from 'angular-oauth2-oidc';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { CreateHumanTaskInstance } from '../parameters/create-humantaskinstance.model';

@Injectable()
export class HumanTaskInstService {
    constructor(private http: HttpClient, private oauthService: OAuthService) { }

    create(cmd: CreateHumanTaskInstance): Observable<string> {
        let headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Content-Type', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        const targetUrl = process.env.HUMANTASK_API_URL + "/humantaskinstances";
        return this.http.post<string>(targetUrl, JSON.stringify(cmd), { headers: headers }).pipe(map((_: any) => {
            return _.id;
        }));
    }

    createMe(cmd: CreateHumanTaskInstance): Observable<string> {
        let headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Content-Type', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        const targetUrl = process.env.HUMANTASK_API_URL + "/humantaskinstances/me";
        return this.http.post<string>(targetUrl, JSON.stringify(cmd), { headers: headers }).pipe(map((_: any) => {
            return _.id;
        }));
    }
}