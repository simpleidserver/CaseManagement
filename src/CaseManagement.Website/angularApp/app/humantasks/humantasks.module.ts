import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { MaterialModule } from '../shared/material.module';
import { SharedModule } from '../shared/shared.module';
import { HumanTasksComponent } from './humantasks.component';
import { HumanTasksRoutes } from './humantasks.routes';
import { AddHumanTaskDefDialog } from './listdefs/add-humantaskdef-dialog.component';
import { ListHumanTaskFilesComponent } from './listdefs/listfiles.component';
import { CreateHumanTaskInstanceDialog } from './viewdef/create-humantaskinstance-dialog.component';
import { ViewHumanTaskDefInfoComponent } from './viewdef/info/info.component';
import { ViewHumanTaskDef } from './viewdef/view.component';
import { CreateHumanTaskInstanceComponent } from './viewdef/common/createhumantaskinstance/createhumantaskinstance.component';
import { OperationParameterComponent } from './viewdef/common/operationparameter/operationparameter.component';
import { ViewTaskPeopleAssignmentComponent } from './viewdef/peopleassignment/view-peopleassignment.component';
import { ViewPresentationParametersComponent } from './viewdef/presentationparameters/view-presentationparameters.component';
import { ViewHumanTaskDefDeadlinesComponent } from './viewdef/deadlines/deadlines.component';
import { ViewHumanTaskDefRenderingComponent } from './viewdef/rendering/rendering.component';
import { PeopleAssignmentComponent } from './viewdef/common/peopleassignment/peopleassignment.component';
import { PresentationParameterComponent } from './viewdef/common/presentationelement/presentationparameter.component';
import { AddEscalationDialog } from './viewdef/deadlines/add-escalation-dialog.component';
import { EditEscalationDialog } from './viewdef/deadlines/edit-escalation-dialog.component';
import { TranslateFieldPipe } from '../infrastructure/pipes/translateFieldPipe';

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
        EditEscalationDialog
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
        TranslateFieldPipe
    ],
    providers: [ TranslateFieldPipe ]
})

export class HumanTasksModule { }