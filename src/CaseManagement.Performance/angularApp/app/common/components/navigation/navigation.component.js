var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
import { Component } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { OAuthService } from 'angular-oauth2-oidc';
import { Router } from '@angular/router';
var NavigationComponent = (function () {
    function NavigationComponent(translateService, oauthService, router) {
        this.translateService = translateService;
        this.oauthService = oauthService;
        this.router = router;
        this.url = process.env.BASE_URL + "assets/images/logo.svg";
        this.isConnected = false;
    }
    NavigationComponent.prototype.chooseLanguage = function (lng) {
        this.translateService.use(lng);
    };
    NavigationComponent.prototype.login = function () {
        this.oauthService.customQueryParams = {
            'prompt': 'login'
        };
        this.oauthService.initImplicitFlow();
        return false;
    };
    NavigationComponent.prototype.chooseSession = function () {
        this.oauthService.customQueryParams = {
            'prompt': 'select_account'
        };
        this.oauthService.initImplicitFlow();
        return false;
    };
    NavigationComponent.prototype.disconnect = function () {
        this.oauthService.logOut();
        this.router.navigate(['/home']);
        return false;
    };
    NavigationComponent.prototype.init = function () {
        var claims = this.oauthService.getIdentityClaims();
        if (!claims) {
            this.isConnected = false;
            return;
        }
        this.name = claims.given_name;
        this.roles = claims.role;
        this.isConnected = true;
    };
    NavigationComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.init();
        this.oauthService.events.subscribe(function (e) {
            if (e.type === "logout") {
                _this.isConnected = false;
            }
            else if (e.type === "token_received") {
                _this.init();
            }
        });
    };
    NavigationComponent = __decorate([
        Component({
            selector: 'app-navigation',
            templateUrl: 'navigation.component.html',
            styleUrls: ['./navigation.component.scss']
        }),
        __metadata("design:paramtypes", [TranslateService, OAuthService, Router])
    ], NavigationComponent);
    return NavigationComponent;
}());
export { NavigationComponent };
//# sourceMappingURL=navigation.component.js.map