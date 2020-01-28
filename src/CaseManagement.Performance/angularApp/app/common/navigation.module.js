var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
import { NgModule } from '@angular/core';
import { NavigationComponent } from './components/navigation/navigation.component';
import { SharedModule } from '../shared/shared.module';
import { MaterialModule } from '../shared/material.module';
import { FormsModule } from '@angular/forms';
var NavigationModule = (function () {
    function NavigationModule() {
    }
    NavigationModule = __decorate([
        NgModule({
            imports: [
                SharedModule,
                MaterialModule,
                FormsModule
            ],
            declarations: [
                NavigationComponent
            ],
            exports: [
                NavigationComponent
            ]
        })
    ], NavigationModule);
    return NavigationModule;
}());
export { NavigationModule };
//# sourceMappingURL=navigation.module.js.map