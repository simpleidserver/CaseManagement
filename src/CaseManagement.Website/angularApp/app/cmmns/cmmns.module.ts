import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { MaterialModule } from '../shared/material.module';
import { SharedModule } from '../shared/shared.module';
import { CmmnsRoutes } from './cmmns.routes';
import { CmmnsComponent } from './cmmns.component';

@NgModule({
    imports: [
        CommonModule,
        SharedModule,
        MaterialModule,
        CmmnsRoutes
    ],
    entryComponents: [],
    declarations: [ CmmnsComponent ],
    providers: [ ]
})

export class CmmnsModule { }