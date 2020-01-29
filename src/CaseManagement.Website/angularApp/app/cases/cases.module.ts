import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { MaterialModule } from '../shared/material.module';
import { SharedModule } from '../shared/shared.module';
import { CasesRoutes } from './cases.routes';
import { CasesComponent } from './cases.component';

@NgModule({
    imports: [
        CommonModule,
        SharedModule,
        MaterialModule,
        CasesRoutes
    ],
    entryComponents: [],
    declarations: [ CasesComponent ],
    providers: [ ]
})

export class CasesModule { }