import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { EffectsModule } from '@ngrx/effects';
import { StoreModule } from '@ngrx/store';
import { StoreDevtoolsModule } from '@ngrx/store-devtools';
import { CaseDefinitionsService } from '../casedefinitions/services/casedefinitions.service';
import { CaseFilesService } from '../casedefinitions/services/casefiles.service';
import { CaseInstancesService } from '../casedefinitions/services/caseinstances.service';
import { MaterialModule } from '../shared/material.module';
import { SharedModule } from '../shared/shared.module';
import { CaseInstancesRoutes } from './caseinstances.routes';
import { ListCaseInstancesComponent } from './list/list.component';
import { ViewCaseInstanceEffects } from './view/view-effects';
import * as fromCaseInstanceReducer from './view/view-reducer';
import { CaseElementInstanceDialog, ViewCaseInstanceComponent } from './view/view.component';

@NgModule({
    imports: [
        CommonModule,
        CaseInstancesRoutes,
        SharedModule,
        MaterialModule,
        EffectsModule.forRoot([ViewCaseInstanceEffects]),
        StoreModule.forRoot({
            caseInstance: fromCaseInstanceReducer.caseInstanceReducer
        }),
        StoreDevtoolsModule.instrument({
            maxAge: 10
        })
    ],
    entryComponents: [ CaseElementInstanceDialog ],
    declarations: [
        ViewCaseInstanceComponent,
        ListCaseInstancesComponent,
        CaseElementInstanceDialog
    ],
    providers: [
        CaseInstancesService,
        CaseDefinitionsService,
        CaseFilesService
    ]
})

export class CaseInstancesModule { }