var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { MaterialModule } from '../../shared/material.module';
import { SharedModule } from '../../shared/shared.module';
import { CasePlanInstancesRoutes } from './caseplaninstances.routes';
import { ListCasePlanInstancesComponent } from './list/list.component';
import { ViewCasePlanInstanceComponent } from './view/view.component';
var CasePlanInstancesModule = (function () {
    function CasePlanInstancesModule() {
    }
    CasePlanInstancesModule = __decorate([
        NgModule({
            imports: [
                CommonModule,
                CasePlanInstancesRoutes,
                SharedModule,
                MaterialModule
            ],
            declarations: [ListCasePlanInstancesComponent, ViewCasePlanInstanceComponent]
        })
    ], CasePlanInstancesModule);
    return CasePlanInstancesModule;
}());
export { CasePlanInstancesModule };
//# sourceMappingURL=caseplaninstances.module.js.map