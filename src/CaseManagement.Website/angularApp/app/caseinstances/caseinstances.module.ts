import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { EffectsModule } from '@ngrx/effects';
import { StoreModule } from '@ngrx/store';
import { StoreDevtoolsModule } from '@ngrx/store-devtools';
import { CaseDefinitionsService } from '../casedefinitions/services/casedefinitions.service';
import { CaseInstancesService } from '../casedefinitions/services/caseinstances.service';
import { MaterialModule } from '../shared/material.module';
import { SharedModule } from '../shared/shared.module';
import { CaseInstanceEffects } from './case-instance/case-instance-effects';
import * as fromCaseInstanceReducer from './case-instance/case-instance-reducer';
import { CaseInstanceComponent } from './case-instance/case-instance.component';
import { CaseInstancesRoutes } from './caseinstances.routes';

@NgModule({
    imports: [
        CommonModule,
        CaseInstancesRoutes,
        SharedModule,
        MaterialModule,
        EffectsModule.forRoot([CaseInstanceEffects]),
        StoreModule.forRoot({
            caseInstance: fromCaseInstanceReducer.reducer
        }),
        StoreDevtoolsModule.instrument({
            maxAge: 10
        })
    ],
    entryComponents: [ ],
    declarations: [
        CaseInstanceComponent
    ],
    providers: [
        CaseDefinitionsService,
        CaseInstancesService
    ]
})

export class CaseInstancesModule { }