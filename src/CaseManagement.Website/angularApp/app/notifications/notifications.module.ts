import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { MaterialModule } from '../shared/material.module';
import { SharedModule } from '../shared/shared.module';
import { AddNotificationDefDialog } from './listdefs/add-notificationdef-dialog.component';
import { ListNotificationDefinitionsComponent } from './listdefs/listdefs.component';
import { NotificationsComponent } from './notifications.component';
import { NotificationsRoutes } from './notifications.routes';
import { AddAssignmentDialogComponent, AddParameterDialogComponent, AddPresentationElementDialogComponent, AddPresentationParameterDialogComponent, ViewNotificationDefInfoComponent } from './viewdef/info/info.component';
import { ViewNotificationDef } from './viewdef/view.component';

@NgModule({
    imports: [
        CommonModule,
        SharedModule,
        MaterialModule,
        NotificationsRoutes
    ],
    entryComponents: [
        AddNotificationDefDialog,
        AddParameterDialogComponent,
        AddAssignmentDialogComponent,
        AddPresentationElementDialogComponent,
        AddPresentationParameterDialogComponent
    ],
    declarations: [
        NotificationsComponent,
        ListNotificationDefinitionsComponent,
        ViewNotificationDef,
        ViewNotificationDefInfoComponent,
        AddNotificationDefDialog,
        AddParameterDialogComponent,
        AddAssignmentDialogComponent,
        AddPresentationElementDialogComponent,
        AddPresentationParameterDialogComponent
    ]
})

export class NotificationsModule { }