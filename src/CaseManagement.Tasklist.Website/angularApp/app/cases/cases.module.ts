import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { NgxChartsModule } from '@swimlane/ngx-charts';
import { MaterialModule } from '../shared/material.module';
import { SharedModule } from '../shared/shared.module';
import { CaseRoutes } from './cases.routes';
import { ListCasesComponent } from './list/list.component';
import { ViewCaseComponent } from './view/view.component';
import { ViewFormComponent } from './view/viewform.component';
import { PipesModule } from '../infrastructure/pipes.module';

@NgModule({
    imports: [
        CommonModule,
        NgxChartsModule,
        FormsModule,
        HttpClientModule,
        CaseRoutes,
        MaterialModule,
        SharedModule,
        PipesModule
    ],

    entryComponents: [
    ],

    declarations: [
        ListCasesComponent,
        ViewCaseComponent,
        ViewFormComponent
    ],

    exports: [ ],

    providers: [  ]
})

export class CasesModule { }