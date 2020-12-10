import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { NgxChartsModule } from '@swimlane/ngx-charts';
import { MonacoEditorModule } from 'ngx-monaco-editor';
import { MaterialModule } from '../../shared/material.module';
import { SharedModule } from '../../shared/shared.module';
import { CmmnPlansRoutes } from './cmmninstances.routes';
import { ViewCmmnPlanInstanceComponent } from './view/view.component';
import { ViewCasePlanEltInstanceComponent } from './view/viewelt.component';
import { ViewTransitionHistoriesComponent } from './view/viewtransitionhistories.component';

@NgModule({
    imports: [
        CommonModule,
        NgxChartsModule,
        MonacoEditorModule.forRoot(),
        FormsModule,
        HttpClientModule,
        CmmnPlansRoutes,
        MaterialModule,
        SharedModule
    ],
    entryComponents: [ ],
    declarations: [
        ViewCmmnPlanInstanceComponent,
        ViewCasePlanEltInstanceComponent,
        ViewTransitionHistoriesComponent
    ],
    exports: [ ]
})

export class CmmnInstancesModule { }
