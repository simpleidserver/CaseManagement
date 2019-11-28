var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { StoreDevtoolsModule } from '@ngrx/store-devtools';
import { CaseDefinitionsService } from '../casedefinitions/services/casedefinitions.service';
import { MaterialModule } from '../shared/material.module';
import { SharedModule } from '../shared/shared.module';
import { CaseInstanceComponent } from './case-instance/case-instance.component';
import { CaseInstancesRoutes } from './caseinstances.routes';
var CaseInstancesModule = (function () {
    function CaseInstancesModule() {
    }
    CaseInstancesModule = __decorate([
        NgModule({
            imports: [
                CommonModule,
                CaseInstancesRoutes,
                SharedModule,
                MaterialModule,
                StoreDevtoolsModule.instrument({
                    maxAge: 10
                })
            ],
            entryComponents: [],
            declarations: [
                CaseInstanceComponent
            ],
            providers: [
                CaseDefinitionsService
            ]
        })
    ], CaseInstancesModule);
    return CaseInstancesModule;
}());
export { CaseInstancesModule };
//# sourceMappingURL=caseinstances.module.js.map