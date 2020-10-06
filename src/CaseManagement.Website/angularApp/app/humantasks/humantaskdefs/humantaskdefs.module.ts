import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { NgxChartsModule } from '@swimlane/ngx-charts';
import { MonacoEditorModule } from 'ngx-monaco-editor';
import { MaterialModule } from '../../shared/material.module';
import { SharedModule } from '../../shared/shared.module';
import { HumanTaskDefsRoutes } from './humantaskdefs.routes';
import { ViewHumanTaskDef } from './view/view.component';

@NgModule({
    imports: [
        CommonModule,
        NgxChartsModule,
        MonacoEditorModule.forRoot(),
        FormsModule,
        HttpClientModule,
        HumanTaskDefsRoutes,
        MaterialModule,
        SharedModule
    ],
    entryComponents: [  ],
    declarations: [ViewHumanTaskDef ],
    exports: [ ]
})

export class HumanTaskDefsModule { }
