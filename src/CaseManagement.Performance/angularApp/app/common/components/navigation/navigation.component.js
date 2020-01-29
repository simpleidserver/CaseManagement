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
var NavigationComponent = (function () {
    function NavigationComponent(translateService) {
        this.translateService = translateService;
        this.url = process.env.BASE_URL + "/assets/images/logo.svg";
    }
    NavigationComponent.prototype.chooseLanguage = function (lng) {
        this.translateService.use(lng);
    };
    NavigationComponent.prototype.ngOnInit = function () {
    };
    NavigationComponent = __decorate([
        Component({
            selector: 'app-navigation',
            templateUrl: 'navigation.component.html',
            styleUrls: ['./navigation.component.scss']
        }),
        __metadata("design:paramtypes", [TranslateService])
    ], NavigationComponent);
    return NavigationComponent;
}());
export { NavigationComponent };
//# sourceMappingURL=navigation.component.js.map