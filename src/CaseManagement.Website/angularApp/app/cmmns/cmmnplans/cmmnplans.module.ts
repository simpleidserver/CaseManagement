import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { NgxChartsModule } from '@swimlane/ngx-charts';
import { MonacoEditorModule } from 'ngx-monaco-editor';
import { MaterialModule } from '../../shared/material.module';
import { SharedModule } from '../../shared/shared.module';
import { CmmnPlansRoutes } from './cmmnplans.routes';
import { ViewCmmnPlanInformationComponent } from './view/information/information.component';
import { ViewCmmnPlanInstancesComponent } from './view/instances/instances.component';
import { ViewCmmnPlanComponent } from './view/view.component';

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
        ViewCmmnPlanComponent,
        ViewCmmnPlanInformationComponent,
        ViewCmmnPlanInstancesComponent 
    ],
    exports: [ ]
})

export class CmmnPlansModule { }
