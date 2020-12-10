var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
import { Injectable } from "@angular/core";
var SidenavService = (function () {
    function SidenavService() {
    }
    SidenavService.prototype.setSidnav = function (sidnav) {
        this.sidenav = sidnav;
    };
    SidenavService.prototype.open = function () {
        this.sidenav.open();
    };
    SidenavService.prototype.close = function () {
        this.sidenav.close();
    };
    SidenavService.prototype.toggle = function () {
        this.sidenav.toggle();
    };
    SidenavService = __decorate([
        Injectable()
    ], SidenavService);
    return SidenavService;
}());
export { SidenavService };
//# sourceMappingURL=SidenavService.js.map