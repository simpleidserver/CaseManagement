import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { NgxChartsModule } from '@swimlane/ngx-charts';
import { MonacoEditorModule } from 'ngx-monaco-editor';
import { MaterialModule } from '../../shared/material.module';
import { SharedModule } from '../../shared/shared.module';
import { CasePlansRoutes } from './caseplans.routes';
import { HistoryCasePlanComponent } from './history/history.component';
import { ListCasePlansComponent } from './list/list.component';
import { ViewCaseDefinitionComponent } from './view/view.component';


@NgModule({
    imports: [
        CommonModule,
        NgxChartsModule,
        MonacoEditorModule.forRoot(),
        FormsModule,
        HttpClientModule,
        CasePlansRoutes,
        MaterialModule,
        SharedModule
    ],
    entryComponents: [ ],
    declarations: [ListCasePlansComponent, ViewCaseDefinitionComponent, HistoryCasePlanComponent],
    exports: [ListCasePlansComponent, ViewCaseDefinitionComponent, HistoryCasePlanComponent]
})

export class CasePlansModule { }
