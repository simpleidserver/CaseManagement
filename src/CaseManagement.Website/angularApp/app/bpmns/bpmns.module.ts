import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { MaterialModule } from '../shared/material.module';
import { SharedModule } from '../shared/shared.module';
import { BpmnsComponent } from './bpmns.component';
import { BpmnsRoutes } from './bpmns.routes';

@NgModule({
    imports: [
        CommonModule,
        SharedModule,
        MaterialModule,
        BpmnsRoutes
    ],
    entryComponents: [],
    declarations: [ BpmnsComponent ],
    providers: [ ]
})

export class BpmnsModule { }