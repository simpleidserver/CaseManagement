import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { TranslateFieldPipe } from '@app/infrastructure/pipes/translateFieldPipe';
import { MaterialModule } from '@app/shared/material.module';
import { SharedModule } from '@app/shared/shared.module';
import { NgxChartsModule } from '@swimlane/ngx-charts';
import { MonacoEditorModule } from 'ngx-monaco-editor';
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
    entryComponents: [],
    declarations: [ViewHumanTaskDef, TranslateFieldPipe ],
    exports: [ ]
})

export class HumanTaskDefsModule { }
