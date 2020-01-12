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
import { CaseInstancesService } from './services/caseinstances.service';
import { ViewCaseDefinitionEffects } from './view/view-effects';
import * as fromCaseDefinitionReducer from './view/view-reducer';
import { ViewCaseDefinitionComponent } from './view/view.component';

@NgModule({
    imports: [
        CommonModule,
        CaseDefinitionsRoutes,
        SharedModule,
        MaterialModule,
        EffectsModule.forRoot([ListCaseDefinitionsEffects, ViewCaseDefinitionEffects]),
        StoreModule.forRoot({
            caseDefinitions: fromListCaseDefinitionsReducer.reducer,
            caseDefinition: fromCaseDefinitionReducer.caseDefinitionReducer,
            caseInstances: fromCaseDefinitionReducer.caseInstancesReducer
        }),
        StoreDevtoolsModule.instrument({
            maxAge: 10
        })
    ],
    entryComponents: [ ],
    declarations: [
        ListCaseDefinitionsComponent,
        ViewCaseDefinitionComponent
    ],
    providers: [
        CaseFilesService,
        CaseDefinitionsService,
        CaseInstancesService
    ]
})

export class CaseDefinitionsModule { }