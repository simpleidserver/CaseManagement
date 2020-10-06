import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { MaterialModule } from '../shared/material.module';
import { SharedModule } from '../shared/shared.module';
import { HumanTasksComponent } from './humantasks.component';
import { HumanTasksRoutes } from './humantasks.routes';

@NgModule({
    imports: [
        CommonModule,
        SharedModule,
        MaterialModule,
        HumanTasksComponent,
        HumanTasksRoutes
    ],
    entryComponents: [],
    declarations: [HumanTasksComponent ],
    providers: [ ]
})

export class HumanTasksModule { }