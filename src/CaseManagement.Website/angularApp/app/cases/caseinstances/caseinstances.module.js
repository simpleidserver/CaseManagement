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
import { CaseDefinitionsRoutes } from './casedefinitions.routes';
import { ListCaseDefinitionsEffects } from './list/list-effects';
import * as fromListCaseDefinitionsReducer from './list/list-reducer';
import { ListCaseDefinitionsComponent } from './list/list.component';
import { CaseDefinitionsService } from './services/casedefinitions.service';
import { CaseFilesService } from './services/casefiles.service';
import { CaseFormInstancesService } from './services/caseforminstances.service';
import { CaseInstancesService } from './services/caseinstances.service';
import { CaseActivationsService } from './services/caseactivations.service';
import { ViewCaseDefinitionEffects } from './view/view-effects';
import * as fromCaseDefinitionReducer from './view/view-reducer';
import { ViewCaseDefinitionComponent } from './view/view.component';
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
                EffectsModule.forRoot([ListCaseDefinitionsEffects, ViewCaseDefinitionEffects]),
                StoreModule.forRoot({
                    caseDefinitions: fromListCaseDefinitionsReducer.reducer,
                    caseDefinition: fromCaseDefinitionReducer.caseDefinitionReducer,
                    caseInstances: fromCaseDefinitionReducer.caseInstancesReducer,
                    formInstances: fromCaseDefinitionReducer.formInstancesReducer,
                    caseActivations: fromCaseDefinitionReducer.caseActivationsReducer
                }),
                StoreDevtoolsModule.instrument({
                    maxAge: 10
                })
            ],
            entryComponents: [],
            declarations: [
                ListCaseDefinitionsComponent,
                ViewCaseDefinitionComponent
            ],
            providers: [
                CaseFilesService,
                CaseDefinitionsService,
                CaseInstancesService,
                CaseFormInstancesService,
                CaseActivationsService
            ]
        })
    ], CaseDefinitionsModule);
    return CaseDefinitionsModule;
}());
export { CaseDefinitionsModule };
//# sourceMappingURL=casedefinitions.module.js.map