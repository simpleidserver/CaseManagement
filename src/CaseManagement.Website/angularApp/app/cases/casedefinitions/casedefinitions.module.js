var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { EffectsModule } from '@ngrx/effects';
import { StoreModule } from '@ngrx/store';
import { StoreDevtoolsModule } from '@ngrx/store-devtools';
import { NgxChartsModule } from '@swimlane/ngx-charts';
import { MonacoEditorModule } from 'ngx-monaco-editor';
import { MaterialModule } from '../../shared/material.module';
import { SharedModule } from '../../shared/shared.module';
import { CaseFilesEffects } from '../casefiles/effects/case-files';
import { CaseFilesService } from '../casefiles/services/casefiles.service';
import { CaseDefinitionsRoutes } from './casedefinitions.routes';
import { CaseActivationsEffects } from './effects/case-activations';
import { CaseDefinitionsEffects } from './effects/case-definitions';
import { CaseFormInstancesEffects } from './effects/case-form-instances';
import { CaseInstancesEffects } from './effects/case-instances';
import { ListCaseDefinitionsComponent } from './list/list.component';
import * as reducers from './reducers';
import { CaseActivationsService } from './services/caseactivations.service';
import { CaseDefinitionsService } from './services/casedefinitions.service';
import { CaseFormInstancesService } from './services/caseforminstances.service';
import { CaseInstancesService } from './services/caseinstances.service';
import { ViewCaseDefinitionComponent } from './view/view.component';
var CaseDefinitionsModule = (function () {
    function CaseDefinitionsModule() {
    }
    CaseDefinitionsModule = __decorate([
        NgModule({
            imports: [
                CommonModule,
                NgxChartsModule,
                MonacoEditorModule.forRoot(),
                FormsModule,
                HttpClientModule,
                CaseDefinitionsRoutes,
                MaterialModule,
                SharedModule,
                EffectsModule.forRoot([CaseDefinitionsEffects, CaseActivationsEffects, CaseFormInstancesEffects, CaseInstancesEffects, CaseFilesEffects]),
                StoreModule.forRoot(reducers.appReducer),
                StoreDevtoolsModule.instrument({
                    maxAge: 10
                })
            ],
            entryComponents: [],
            declarations: [ListCaseDefinitionsComponent, ViewCaseDefinitionComponent],
            exports: [ListCaseDefinitionsComponent, ViewCaseDefinitionComponent],
            providers: [CaseDefinitionsService, CaseActivationsService, CaseFormInstancesService, CaseInstancesService, CaseFilesService]
        })
    ], CaseDefinitionsModule);
    return CaseDefinitionsModule;
}());
export { CaseDefinitionsModule };
//# sourceMappingURL=casedefinitions.module.js.map