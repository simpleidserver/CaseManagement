import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { MaterialModule } from '../shared/material.module';
import { SharedModule } from '../shared/shared.module';
import { HumanTasksComponent } from './humantasks.component';
import { HumanTasksRoutes } from './humantasks.routes';
import { AddHumanTaskDefDialog } from './listdefs/add-humantaskdef-dialog.component';
import { ListHumanTaskFilesComponent } from './listdefs/listfiles.component';
import { AddEscalationDialog } from './viewdeadline/add-escalation-dialog.component';
import { ViewDeadlineComponent } from './viewdeadline/viewdeadline.component';
import { AddToPartDialog } from './viewdeadline/viewescalation/add-topart-dialog.component';
import { ViewEscalationComponent } from './viewdeadline/viewescalation/viewescalation.component';
import { CreateHumanTaskInstanceComponent } from './viewdef/common/createhumantaskinstance/createhumantaskinstance.component';
import { OperationParameterComponent } from './viewdef/common/operationparameter/operationparameter.component';
import { PeopleAssignmentComponent } from './viewdef/common/peopleassignment/peopleassignment.component';
import { PresentationParameterComponent } from './viewdef/common/presentationelement/presentationparameter.component';
import { CreateHumanTaskInstanceDialog } from './viewdef/create-humantaskinstance-dialog.component';
import { AddAssignmentDialogComponent, AddDeadlineComponentDialog, AddParameterDialogComponent, AddPresentationElementDialogComponent, AddPresentationParameterDialogComponent, ViewHumanTaskDefInfoComponent } from './viewdef/info/info.component';
import { ColumnComponent } from './viewdef/rendering/components/column/column.component';
import { ConfirmPwdComponent, ConfirmPwdComponentDialog } from './viewdef/rendering/components/confirmpwd/confirmpwd.component';
import { ContainerComponent } from './viewdef/rendering/components/container/container.component';
import { DynamicComponent } from './viewdef/rendering/components/dynamic.component';
import { HeaderComponent, HeaderComponentDialog } from './viewdef/rendering/components/header/header.component';
import { PwdComponent, PwdComponentDialog } from './viewdef/rendering/components/pwd/pwd.component';
import { RowComponent, RowComponentDialog } from './viewdef/rendering/components/row/row.component';
import { SelectComponent, SelectComponentDialog } from './viewdef/rendering/components/select/select.component';
import { SubmitBtnComponent, SubmitBtnComponentDialog } from './viewdef/rendering/components/submitbtn/submitbtn.component';
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
        RowComponentDialog,
        TxtComponentDialog,
        SelectComponentDialog,
        HeaderComponentDialog,
        AddParameterDialogComponent,
        AddPresentationParameterDialogComponent,
        AddPresentationElementDialogComponent,
        AddAssignmentDialogComponent,
        AddDeadlineComponentDialog,
        AddToPartDialog,
        PwdComponentDialog,
        RowComponent,
        ColumnComponent,
        TxtComponent,
        PwdComponent,
        SelectComponent,
        HeaderComponent,
        ContainerComponent,
        ConfirmPwdComponent,
        ConfirmPwdComponentDialog,
        SubmitBtnComponentDialog,
        SubmitBtnComponent
    ],
    declarations: [
        AddHumanTaskDefDialog,
        PwdComponent,
        CreateHumanTaskInstanceDialog,
        AddEscalationDialog,
        SelectComponentDialog,
        HumanTasksComponent,
        ListHumanTaskFilesComponent,
        ViewHumanTaskDef,
        ViewHumanTaskDefInfoComponent,
        CreateHumanTaskInstanceComponent,
        OperationParameterComponent,
        ViewHumanTaskDefRenderingComponent,
        AddParameterDialogComponent,
        AddDeadlineComponentDialog,
        PeopleAssignmentComponent,
        RowComponentDialog,
        TxtComponentDialog,
        PwdComponentDialog,
        HeaderComponentDialog,
        PresentationParameterComponent,
        AddPresentationParameterDialogComponent,
        AddPresentationElementDialogComponent,
        AddAssignmentDialogComponent,
        ViewEscalationComponent,
        ViewDeadlineComponent,
        AddToPartDialog,
        DynamicComponent,
        ColumnComponent,
        SelectComponent,
        RowComponent,
        TxtComponent,
        HeaderComponent,
        ContainerComponent,
        ConfirmPwdComponent,
        ConfirmPwdComponentDialog,
        SubmitBtnComponentDialog,
        SubmitBtnComponent
    ]
})

export class HumanTasksModule { }