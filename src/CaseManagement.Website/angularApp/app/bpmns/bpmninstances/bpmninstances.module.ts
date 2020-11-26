import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { NgxChartsModule } from '@swimlane/ngx-charts';
import { MonacoEditorModule } from 'ngx-monaco-editor';
import { MaterialModule } from '../../shared/material.module';
import { SharedModule } from '../../shared/shared.module';
import { BpmnInstancesRoutes } from './bpmninstances.routes';
import { ViewBpmnInstanceComponent } from './view/view.component';
import { ListBpmnInstancesComponent } from './list/list.component';

@NgModule({
    imports: [
        CommonModule,
        NgxChartsModule,
        MonacoEditorModule.forRoot(),
        FormsModule,
        HttpClientModule,
        BpmnInstancesRoutes,
        MaterialModule,
        SharedModule
    ],
    entryComponents: [ ],
    declarations: [ListBpmnInstancesComponent, ViewBpmnInstanceComponent]
})

export class BpmnInstancesModule { }
