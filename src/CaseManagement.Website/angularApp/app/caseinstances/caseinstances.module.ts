import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { StoreDevtoolsModule } from '@ngrx/store-devtools';
import { CaseDefinitionsService } from '../casedefinitions/services/casedefinitions.service';
import { MaterialModule } from '../shared/material.module';
import { SharedModule } from '../shared/shared.module';
import { CaseInstanceComponent } from './case-instance/case-instance.component';
import { CaseInstancesRoutes } from './caseinstances.routes';

@NgModule({
    imports: [
        CommonModule,
        CaseInstancesRoutes,
        SharedModule,
        MaterialModule,
        StoreDevtoolsModule.instrument({
            maxAge: 10
        })
    ],
    entryComponents: [ ],
    declarations: [
        CaseInstanceComponent
    ],
    providers: [
        CaseDefinitionsService
    ]
})

export class CaseInstancesModule { }