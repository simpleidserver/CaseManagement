import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { NgxChartsModule } from '@swimlane/ngx-charts';
import { MaterialModule } from '../shared/material.module';
import { SharedModule } from '../shared/shared.module';
import { ListNotificationsComponent } from './list/list.component';
import { NotificationsRoutes } from './notifications.routes';

@NgModule({
    imports: [
        CommonModule,
        NgxChartsModule,
        FormsModule,
        HttpClientModule,
        NotificationsRoutes,
        MaterialModule,
        SharedModule
    ],

    entryComponents: [
    ],

    declarations: [
        ListNotificationsComponent
    ],

    exports: [
        ListNotificationsComponent
    ],

    providers: [  ]
})

export class NotificationsModule { }