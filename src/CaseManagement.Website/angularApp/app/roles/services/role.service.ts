import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { OAuthService } from 'angular-oauth2-oidc';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { Role } from '../models/role.model';
import { SearchRolesResult } from '../models/search-roles.model';

@Injectable()
export class RolesService {
    constructor(private http: HttpClient, private oauthService: OAuthService) { }

    search(startIndex: number, count: number, order: string, direction: string): Observable<SearchRolesResult> {
        let headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        let targetUrl = process.env.API_URL + "/roles/search?start_index=" + startIndex + "&count=" + count;
        if (order) {
            targetUrl = targetUrl + "&order_by=" + order;
        }

        if (direction) {
            targetUrl = targetUrl + "&order=" + direction;
        }

        return this.http.get(targetUrl, { headers: headers }).pipe(map((res: any) => {
            return SearchRolesResult.fromJson(res);
        }));
    }

    get(role: string): Observable<Role> {
        let headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        let targetUrl = process.env.API_URL + "/roles/" + role;
        return this.http.get(targetUrl, { headers: headers }).pipe(map((res: any) => {
            return Role.fromJson(res);
        }));
    }

    add(role : string) : Observable<Role> {
        const request = JSON.stringify({ role: role });
        let headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Content-Type', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        let targetUrl = process.env.API_URL + "/roles";
        return this.http.post(targetUrl, request, { headers: headers }).pipe(map((res: any) => {
            return Role.fromJson(res);
        }));
    }

    update(role: string, users: Array<string>): Observable<any> {
        const request = JSON.stringify({ users: users });
        let headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Content-Type', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        let targetUrl = process.env.API_URL + "/roles/" + role;
        return this.http.put(targetUrl, request, { headers: headers });
    }

    delete(role: string): Observable<any> {
        let headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        let targetUrl = process.env.API_URL + "/roles/" + role;
        return this.http.delete(targetUrl, { headers: headers });
    }
}