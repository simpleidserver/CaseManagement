import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { NgxChartsModule } from '@swimlane/ngx-charts';
import { TranslateFieldPipe } from '../infrastructure/pipes/translateFieldPipe';
import { MaterialModule } from '../shared/material.module';
import { SharedModule } from '../shared/shared.module';
import { ListTasksComponent } from './list/list.component';
import { NominateTaskDialogComponent } from './list/nominate-task-dialog.component';
import { HomeRoutes } from './tasks.routes';
import { ViewTaskComponent } from './view/view.component';

@NgModule({
    imports: [
        CommonModule,
        NgxChartsModule,
        FormsModule,
        HttpClientModule,
        HomeRoutes,
        MaterialModule,
        SharedModule
    ],

    entryComponents: [
        NominateTaskDialogComponent
    ],

    declarations: [
        ListTasksComponent,
        ViewTaskComponent,
        NominateTaskDialogComponent,
        TranslateFieldPipe
    ],

    exports: [
        ListTasksComponent,
        NominateTaskDialogComponent
    ],

    providers: [  ]
})

export class TasksModule { }