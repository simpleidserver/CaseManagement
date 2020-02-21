var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { EffectsModule } from '@ngrx/effects';
import { StoreModule } from '@ngrx/store';
import { StoreDevtoolsModule } from '@ngrx/store-devtools';
import { MaterialModule } from '../shared/material.module';
import { SharedModule } from '../shared/shared.module';
import { CaseDefEffects } from './case-def/case-def-effects';
import * as fromCaseDefReducer from './case-def/case-def-reducer';
import { CaseDefComponent } from './case-def/case-def.component';
import { CaseDefinitionsRoutes } from './casedefinitions.routes';
import { ListCaseDefsEffects } from './list-case-defs/list-case-defs-effects';
import * as fromListCaseDefsReducer from './list-case-defs/list-case-defs-reducer';
import { ListCaseDefsComponent } from './list-case-defs/list-case-defs.component';
import { CaseDefinitionsService } from './services/casedefinitions.service';
import { CaseInstancesService } from './services/caseinstances.service';
var CaseDefinitionsModule = (function () {
    function CaseDefinitionsModule() {
    }
    CaseDefinitionsModule = __decorate([
        NgModule({
            imports: [
                CommonModule,
                CaseDefinitionsRoutes,
                SharedModule,
                MaterialModule,
                EffectsModule.forRoot([ListCaseDefsEffects, CaseDefEffects]),
                StoreModule.forRoot({
                    caseDefs: fromListCaseDefsReducer.reducer,
                    caseDef: fromCaseDefReducer.reducer
                }),
                StoreDevtoolsModule.instrument({
                    maxAge: 10
                })
            ],
            entryComponents: [],
            declarations: [
                ListCaseDefsComponent,
                CaseDefComponent
            ],
            providers: [
                CaseDefinitionsService,
                CaseInstancesService
            ]
        })
    ], CaseDefinitionsModule);
    return CaseDefinitionsModule;
}());
export { CaseDefinitionsModule };
//# sourceMappingURL=casedefinitions.module.js.map