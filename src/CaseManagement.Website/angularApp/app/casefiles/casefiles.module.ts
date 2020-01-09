import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { EffectsModule } from '@ngrx/effects';
import { StoreModule } from '@ngrx/store';
import { StoreDevtoolsModule } from '@ngrx/store-devtools';
import { MaterialModule } from '../shared/material.module';
import { SharedModule } from '../shared/shared.module';
import { CaseFilesRoutes } from './casefiles.routes';
import { ListCaseFilesEffects } from './list-case-files/list-case-files-effects';
import * as fromListCaseFilesReducer from './list-case-files/list-case-files-reducer';
import { ListCaseFilesComponent } from './list-case-files/list-case-files.component';
import { CaseDefinitionsService } from './services/casedefinitions.service';
import { CaseFilesService } from './services/casefiles.service';
import { CaseInstancesService } from './services/caseinstances.service';
import * as fromCaseDefinitionsReducer from './view-case-file/case-definitions-reducer';
import * as fromCaseFileReducer from './view-case-file/case-file-reducer';
import * as fromCaseDefinitionReducer from './view-case-definition/case-definition-reducer';
import * as fromCaseInstancesReducer from './view-case-definition/case-instances-reducer';
import { ViewCaseFileEffects } from './view-case-file/view-case-file-effects';
import { ViewCaseFileComponent } from './view-case-file/view-case-file.component';
import { ViewCaseDefinitionEffects } from './view-case-definition/view-case-definition-effects';
import { ViewCaseDefinitionComponent } from './view-case-definition/view-case-definition.component';

@NgModule({
    imports: [
        CommonModule,
        CaseFilesRoutes,
        SharedModule,
        MaterialModule,
        EffectsModule.forRoot([ListCaseFilesEffects, ViewCaseFileEffects, ViewCaseDefinitionEffects]),
        StoreModule.forRoot({
            caseFiles: fromListCaseFilesReducer.reducer,
            caseDefinitions: fromCaseDefinitionsReducer.reducer,
            caseFile: fromCaseFileReducer.reducer,
            caseDefinition: fromCaseDefinitionReducer.reducer,
            caseInstances: fromCaseInstancesReducer.reducer
        }),
        StoreDevtoolsModule.instrument({
            maxAge: 10
        })
    ],
    entryComponents: [ ],
    declarations: [
        ListCaseFilesComponent,
        ViewCaseFileComponent,
        ViewCaseDefinitionComponent
    ],
    providers: [
        CaseFilesService,
        CaseDefinitionsService,
        CaseInstancesService
    ]
})

export class CaseFilesModule { }