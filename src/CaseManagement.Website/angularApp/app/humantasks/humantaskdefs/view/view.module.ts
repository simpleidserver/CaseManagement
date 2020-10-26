import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { TranslateFieldPipe } from '@app/infrastructure/pipes/translateFieldPipe';
import { MaterialModule } from '@app/shared/material.module';
import { SharedModule } from '@app/shared/shared.module';
import { OperationParameterComponent } from './common/operationparameter/operationparameter.component';
import { PeopleAssignmentComponent } from './common/peopleassignment/peopleassignment.component';
import { PresentationParameterComponent } from './common/presentationelement/presentationparameter.component';
import { AddEscalationDialog } from './deadlines/add-escalation-dialog.component';
import { ViewHumanTaskDefDeadlinesComponent } from './deadlines/deadlines.component';
import { EditEscalationDialog } from './deadlines/edit-escalation-dialog.component';
import { ViewHumanTaskDefInfoComponent } from './info/info.component';
import { ViewTaskPeopleAssignmentComponent } from './peopleassignment/view-peopleassignment.component';
import { ViewPresentationParametersComponent } from './presentationparameters/view-presentationparameters.component';
import { ViewHumanTaskDefRenderingComponent } from './rendering/rendering.component';
import { HumanTaskDefsViewRoutes } from './view.routes';

@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        HttpClientModule,
        HumanTaskDefsViewRoutes,
        MaterialModule,
        SharedModule
    ],
    entryComponents: [AddEscalationDialog, EditEscalationDialog ],
    declarations: [
        ViewHumanTaskDefRenderingComponent,
        ViewHumanTaskDefInfoComponent,
        ViewHumanTaskDefDeadlinesComponent,
        ViewPresentationParametersComponent,
        EditEscalationDialog,
        AddEscalationDialog,
        OperationParameterComponent,
        PresentationParameterComponent,
        PeopleAssignmentComponent,
        ViewTaskPeopleAssignmentComponent,
        TranslateFieldPipe
    ],
    exports: [ ]
})

export class HumanTaskDefsViewModule { }
