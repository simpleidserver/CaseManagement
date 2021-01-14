import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { TranslateFieldPipe } from '../infrastructure/pipes/translateFieldPipe';
import { MaterialModule } from '../shared/material.module';
import { SharedModule } from '../shared/shared.module';
import { HumanTasksComponent } from './humantasks.component';
import { HumanTasksRoutes } from './humantasks.routes';
import { AddHumanTaskDefDialog } from './listdefs/add-humantaskdef-dialog.component';
import { ListHumanTaskFilesComponent } from './listdefs/listfiles.component';
import { CreateHumanTaskInstanceComponent } from './viewdef/common/createhumantaskinstance/createhumantaskinstance.component';
import { OperationParameterComponent } from './viewdef/common/operationparameter/operationparameter.component';
import { PeopleAssignmentComponent } from './viewdef/common/peopleassignment/peopleassignment.component';
import { PresentationParameterComponent } from './viewdef/common/presentationelement/presentationparameter.component';
import { CreateHumanTaskInstanceDialog } from './viewdef/create-humantaskinstance-dialog.component';
import { AddEscalationDialog } from './viewdef/deadlines/add-escalation-dialog.component';
import { ViewHumanTaskDefDeadlinesComponent } from './viewdef/deadlines/deadlines.component';
import { EditEscalationDialog } from './viewdef/deadlines/edit-escalation-dialog.component';
import { AddAssignmentDialogComponent, AddDeadlineComponentDialog, AddParameterDialogComponent, AddPresentationElementDialogComponent, AddPresentationParameterDialogComponent, ViewHumanTaskDefInfoComponent } from './viewdef/info/info.component';
import { ViewTaskPeopleAssignmentComponent } from './viewdef/peopleassignment/view-peopleassignment.component';
import { ColumnComponent } from './viewdef/rendering/components/column/column.component';
import { ContainerComponent } from './viewdef/rendering/components/container/container.component';
import { DynamicComponent } from './viewdef/rendering/components/dynamic.component';
import { HeaderComponent, HeaderComponentDialog } from './viewdef/rendering/components/header/header.component';
import { RowComponent, RowComponentDialog } from './viewdef/rendering/components/row/row.component';
import { SelectComponent, SelectComponentDialog } from './viewdef/rendering/components/select/select.component';
import { TxtComponent, TxtComponentDialog } from './viewdef/rendering/components/txt/txt.component';
import { ViewHumanTaskDefRenderingComponent } from './viewdef/rendering/rendering.component';
import { ViewHumanTaskDef } from './viewdef/view.component';

@NgModule({
    imports: [
        CommonModule,
        SharedModule,
        MaterialModule,
        HumanTasksRoutes
    ],
    entryComponents: [
        AddHumanTaskDefDialog,
        CreateHumanTaskInstanceDialog,
        AddEscalationDialog,
        EditEscalationDialog,
        RowComponentDialog,
        TxtComponentDialog,
        SelectComponentDialog,
        HeaderComponentDialog,
        AddParameterDialogComponent,
        AddPresentationParameterDialogComponent,
        AddPresentationElementDialogComponent,
        AddAssignmentDialogComponent,
        AddDeadlineComponentDialog,
        RowComponent,
        ColumnComponent,
        TxtComponent,
        SelectComponent,
        HeaderComponent,
        ContainerComponent
    ],
    declarations: [
        AddHumanTaskDefDialog,
        CreateHumanTaskInstanceDialog,
        AddEscalationDialog,
        EditEscalationDialog,
        SelectComponentDialog,
        HumanTasksComponent,
        ListHumanTaskFilesComponent,
        ViewHumanTaskDef,
        ViewHumanTaskDefInfoComponent,
        CreateHumanTaskInstanceComponent,
        OperationParameterComponent,
        ViewTaskPeopleAssignmentComponent,
        ViewHumanTaskDefDeadlinesComponent,
        ViewHumanTaskDefRenderingComponent,
        AddParameterDialogComponent,
        AddDeadlineComponentDialog,
        PeopleAssignmentComponent,
        RowComponentDialog,
        TxtComponentDialog,
        HeaderComponentDialog,
        PresentationParameterComponent,
        AddPresentationParameterDialogComponent,
        AddPresentationElementDialogComponent,
        AddAssignmentDialogComponent,
        DynamicComponent,
        ColumnComponent,
        SelectComponent,
        RowComponent,
        TxtComponent,
        HeaderComponent,
        ContainerComponent,
        TranslateFieldPipe
    ],
    providers: [ TranslateFieldPipe ]
})

export class HumanTasksModule { }