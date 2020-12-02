import { animate, state, style, transition, trigger } from '@angular/animations';
import { Component, OnInit, ViewEncapsulation, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { JwksValidationHandler, OAuthService } from 'angular-oauth2-oidc';
import { authConfig } from './auth.config';
import { SidenavService } from './shared/SidenavService';
import { MatSidenav } from '@angular/material';

@Component({
    selector: 'app-component',
    templateUrl: './app.component.html',
    styleUrls: [
        './app.component.scss',
        '../../node_modules/leaflet/dist/leaflet.css',
        '../../node_modules/leaflet-search/dist/leaflet-search.src.css',
        '../../node_modules/cmmn-js/dist/assets/diagram-js.css',
        '../../node_modules/cmmn-js/dist/assets/cmmn-font/css/cmmn.css',
        '../../node_modules/cmmn-js/dist/assets/cmmn-font/css/cmmn-codes.css',
        '../../node_modules/cmmn-js/dist/assets/cmmn-font/css/cmmn-embedded.css',
        '../../node_modules/bpmn-js/dist/assets/diagram-js.css',
        '../../node_modules/bpmn-js/dist/assets/bpmn-font/css/bpmn.css',
        '../../node_modules/bpmn-js/dist/assets/bpmn-font/css/bpmn-codes.css',
        '../../node_modules/bpmn-js/dist/assets/bpmn-font/css/bpmn-embedded.css'
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
    casesExpanded: boolean = false;
    humanTasksExpanded: boolean = false;
    bpmnsExpanded: boolean = false;
    @ViewChild('sidenav') public sidenav: MatSidenav;

    constructor(private route: Router, private translate: TranslateService, private oauthService: OAuthService, private router: Router,
        private sidenavService: SidenavService) {
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

    toggleCases() {
        this.casesExpanded = !this.casesExpanded;
    }

    toggleHumanTasks() {
        this.humanTasksExpanded = !this.humanTasksExpanded;
    }

    toggleBpmnFiles() {
        this.bpmnsExpanded = !this.bpmnsExpanded;
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

        this.router.events.subscribe((opt : any) => {
            var url = opt.urlAfterRedirects;
            if (!url || this.humanTasksExpanded || this.casesExpanded) {
                return;
            }

            if (url.startsWith('/cases')) {
                this.casesExpanded = true;
            }

            if (url.startsWith('/humantasks')) {
                this.humanTasksExpanded = true;
            }

            if (url.startsWith('/bpmns')) {
                this.bpmnsExpanded = true;
            }
        });
        this.sidenavService.setSidnav(this.sidenav);
    }

    private configureAuth() {
        this.oauthService.configure(authConfig);
        this.oauthService.setStorage(localStorage);
        this.oauthService.tokenValidationHandler = new JwksValidationHandler();
        let self = this;
        this.oauthService.loadDiscoveryDocumentAndTryLogin({
            disableOAuth2StateCheck: true
        });
        this.sessionCheckTimer = setInterval(function () {
            if (!self.oauthService.hasValidIdToken()) {
                self.oauthService.logOut();
                self.route.navigate(["/"]);
            }            
        }, 3000);
    }
}
