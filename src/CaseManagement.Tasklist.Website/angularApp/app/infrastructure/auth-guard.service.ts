import { Injectable } from "@angular/core";
import { CanActivate, Router, ActivatedRouteSnapshot } from "@angular/router";
import { OAuthService } from "angular-oauth2-oidc";
import { Observable } from "rxjs";

@Injectable()
export class AuthGuard implements CanActivate {
    constructor(private authService: OAuthService, private router: Router) {

    }

    canActivate(next: ActivatedRouteSnapshot): Observable<boolean> | Promise<boolean> | boolean {
        let claims : any = this.authService.getIdentityClaims();
        if (!claims) {
            this.router.navigate(['/status/404']);
            return false;
        }

        if (next.data && next.data.role) {
            var filteredRoles = next.data.role.filter(function (role: string) {
                if (claims.role.includes) {
                    return claims.role.includes(role);
                }
                    
                return role === claims.role;
            });

            if (filteredRoles.length === 0) {
                this.router.navigate(['/status/401']);
                return false;
            }
        }

        return true;
    }
}