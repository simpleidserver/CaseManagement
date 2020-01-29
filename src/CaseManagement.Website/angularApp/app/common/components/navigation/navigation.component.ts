import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { OAuthService } from 'angular-oauth2-oidc';

@Component({
    selector: 'app-navigation',
    templateUrl: 'navigation.component.html',
    styleUrls: ['./navigation.component.scss']
})
export class NavigationComponent implements OnInit {
    url: string = process.env.BASE_URL + "assets/images/logo.svg";
    isConnected: boolean = false;
    name: string;
    roles: any;

    constructor(private translateService: TranslateService, private oauthService: OAuthService, private router : Router) { }

    chooseLanguage(lng: string) {
        this.translateService.use(lng);
    }

    login() {
        this.oauthService.initImplicitFlow();
        return false;
    }

    disconnect() {
        this.oauthService.logOut();
        this.router.navigate(['/home']);
        return false;
    }

    init() {
        var claims: any = this.oauthService.getIdentityClaims();
        if (!claims) {
            this.isConnected = false;;
            return;
        }

        this.name = claims.given_name;
        this.roles = claims.role;
        this.isConnected = true;
    }

    ngOnInit() {
        this.init();
        this.oauthService.events.subscribe((e: any) => {
            if (e.type === "logout") {
                this.isConnected = false;
            } else if (e.type === "token_received") {
                this.init();
            }
        });
    }
}
