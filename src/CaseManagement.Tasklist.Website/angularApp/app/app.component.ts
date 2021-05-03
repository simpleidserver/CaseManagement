import { animate, state, style, transition, trigger } from '@angular/animations';
import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { JwksValidationHandler, OAuthService } from 'angular-oauth2-oidc';
import { authConfig } from './auth.config';

@Component({
    selector: 'app-component',
    templateUrl: './app.component.html',
    styleUrls: [
        './app.component.scss'
    ],
    encapsulation: ViewEncapsulation.None,
    animations: [
        trigger('indicatorRotate', [
            state('collapsed', style({ transform: 'rotate(0deg)' })),
            state('expanded', style({ transform: 'rotate(180deg)' })),
            transition('expanded <=> collapsed',
                animate('225ms cubic-bezier(0.4,0.0,0.2,1)')
            ),
        ])
    ]
})

export class AppComponent implements OnInit {
    sessionCheckTimer: any;
    isConnected: boolean = false;
    name: string;
    roles: any;
    url: string = process.env.BASE_URL + "assets/images/logo.svg";

    constructor(private route: Router, private translate: TranslateService, private oauthService: OAuthService, private router: Router) {
        translate.setDefaultLang('fr');
        translate.use('fr');
        this.configureAuth();
    }

    chooseLanguage(lng: string) {
        this.translate.use(lng);
    }

    login() {
        this.oauthService.customQueryParams = {
            'prompt': 'login'
        };
        this.oauthService.initImplicitFlow();
        return false;
    }

    chooseSession() {
        this.oauthService.customQueryParams = {
            'prompt': 'select_account'
        };
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
            } else if (e.type === "user_profile_loaded") {
                this.init();
            }
        });
    }

    private configureAuth() {
        this.oauthService.configure(authConfig);
        this.oauthService.tokenValidationHandler = new JwksValidationHandler();
        let self = this;
        this.oauthService.loadDiscoveryDocumentAndTryLogin({
            disableOAuth2StateCheck: true
        }).then(() => {
            this.oauthService.loadUserProfile();
        });
        this.sessionCheckTimer = setInterval(function () {
            if (!self.oauthService.hasValidIdToken()) {
                self.oauthService.logOut();
                self.route.navigate(["/"]);
            }            
        }, 3000);
    }
}
