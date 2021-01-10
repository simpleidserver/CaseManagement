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
import { ViewHumanTaskDefInfoComponent } from './viewdef/info/info.component';
import { ViewTaskPeopleAssignmentComponent } from './viewdef/peopleassignment/view-peopleassignment.component';
import { ViewPresentationParametersComponent } from './viewdef/presentationparameters/view-presentationparameters.component';
import { ColumnComponent } from './viewdef/rendering/components/column/column.component';
import { DynamicComponent } from './viewdef/rendering/components/dynamic.component';
import { RowComponent } from './viewdef/rendering/components/row/row.component';
import { ViewHumanTaskDefRenderingComponent } from './viewdef/rendering/rendering.component';
import { ViewHumanTaskDef } from './viewdef/view.component';
import { TxtComponent } from './viewdef/rendering/components/txt/txt.component';
import { SelectComponent } from './viewdef/rendering/components/select/select.component';

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
        RowComponent,
        ColumnComponent,
        TxtComponent,
        SelectComponent
    ],
    declarations: [
        AddHumanTaskDefDialog,
        CreateHumanTaskInstanceDialog,
        AddEscalationDialog,
        EditEscalationDialog,
        HumanTasksComponent,
        ListHumanTaskFilesComponent,
        ViewHumanTaskDef,
        ViewHumanTaskDefInfoComponent,
        CreateHumanTaskInstanceComponent,
        OperationParameterComponent,
        ViewTaskPeopleAssignmentComponent,
        ViewPresentationParametersComponent,
        ViewHumanTaskDefDeadlinesComponent,
        ViewHumanTaskDefRenderingComponent,
        PeopleAssignmentComponent,
        PresentationParameterComponent,
        DynamicComponent,
        ColumnComponent,
        SelectComponent,
        RowComponent,
        TxtComponent,
        TranslateFieldPipe
    ],
    providers: [ TranslateFieldPipe ]
})

export class HumanTasksModule { }