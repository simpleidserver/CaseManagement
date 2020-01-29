import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { CountResult } from '../models/count-result.model';
import { OAuthService } from 'angular-oauth2-oidc';

@Injectable()
export class CaseFilesService {
    constructor(private http: HttpClient, private oauthService: OAuthService) { }

    count(): Observable<CountResult> {
        let headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        let targetUrl = process.env.API_URL + "/case-files/count";
        return this.http.get(targetUrl, { headers: headers }).pipe(map((res: any) => {
            return CountResult.fromJson(res);
        }));
    }
}