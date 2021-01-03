import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { NgxChartsModule } from '@swimlane/ngx-charts';
import { MaterialModule } from '../shared/material.module';
import { SharedModule } from '../shared/shared.module';
import { CaseRoutes } from './cases.routes';
import { ListCasesComponent } from './list/list.component';

@NgModule({
    imports: [
        CommonModule,
        NgxChartsModule,
        FormsModule,
        HttpClientModule,
        CaseRoutes,
        MaterialModule,
        SharedModule
    ],

    entryComponents: [
    ],

    declarations: [
        ListCasesComponent
    ],

    exports: [
    ],

    providers: [  ]
})

export class CasesModule { }