import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MaterialModule } from '@app/shared/material.module';
import { SharedModule } from '@app/shared/shared.module';
import { NgxChartsModule } from '@swimlane/ngx-charts';
import { MonacoEditorModule } from 'ngx-monaco-editor';
import { ActivityStatesComponent } from './activitystates/activitystates.component';
import { ViewBpmnInstanceComponent } from './view.component';
import { ViewBpmnInstanceRoutes } from './view.routes';
import { ViewExecutionPathComponent } from './viewexecutionpath.component';
import { ViewExecutionPointerComponent } from './viewpointer.component';
import { IncomingTokensComponent } from './incomingtokens/incomingtokens.component';
import { OutgoingTokensComponent } from './outgoingtokens/outgoingtokens.component';

@NgModule({
    imports: [
        CommonModule,
        NgxChartsModule,
        MonacoEditorModule.forRoot(),
        FormsModule,
        HttpClientModule,
        ViewBpmnInstanceRoutes,
        MaterialModule,
        SharedModule
    ],
    entryComponents: [ ],
    declarations: [ActivityStatesComponent, ViewBpmnInstanceComponent, ViewExecutionPathComponent, ViewExecutionPointerComponent, IncomingTokensComponent, OutgoingTokensComponent]
})

export class ViewBpmnInstanceModule { }
