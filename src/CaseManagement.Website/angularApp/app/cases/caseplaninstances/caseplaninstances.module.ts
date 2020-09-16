import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { MaterialModule } from '../../shared/material.module';
import { SharedModule } from '../../shared/shared.module';
import { CasePlanInstancesRoutes } from './caseplaninstances.routes';
import { ListCasePlanInstancesComponent } from './list/list.component';
import { ViewCasePlanInstanceComponent } from './view/view.component';

@NgModule({
    imports: [
        CommonModule,
        CasePlanInstancesRoutes,
        SharedModule,
        MaterialModule
    ],
    declarations: [ListCasePlanInstancesComponent, ViewCasePlanInstanceComponent]
})

export class CasePlanInstancesModule { }