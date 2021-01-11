import { Component, ViewEncapsulation } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatSnackBar, MatTableDataSource } from '@angular/material';
import { ActivatedRoute } from '@angular/router';
import * as fromAppState from '@app/stores/appstate';
import { Parameter } from '@app/stores/common/parameter.model';
import { PeopleAssignment } from '@app/stores/common/people-assignment.model';
import { PresentationParameter } from '@app/stores/common/presentationparameter.model';
import * as fromHumanTaskDefActions from '@app/stores/humantaskdefs/actions/humantaskdef.actions';
import { HumanTaskDef } from '@app/stores/humantaskdefs/models/humantaskdef.model';
import { ScannedActionsSubject, select, Store } from '@ngrx/store';
import { TranslateService } from '@ngx-translate/core';
import { filter } from 'rxjs/operators';
import { GuidGenerator } from './guidgenerator';
import { Deadline } from '@app/stores/humantaskdefs/models/deadline';

@Component({
    selector: 'view-humantaskdef-rendering-component',
    templateUrl: './rendering.component.html',
    styleUrls: ['./rendering.component.scss'],
    encapsulation: ViewEncapsulation.None
})
export class ViewHumanTaskDefRenderingComponent {
    humanTaskDef: HumanTaskDef;
    parameterDisplayedColumns: string[] = ['usage', 'name', 'type', 'isRequired'];
    presentationParameterDisplayedColumns: string[] = ['name', 'type', 'expression'];
    peopleAssignmentDisplayedColumns: string[] = ['type', 'value', 'usage'];
    deadlineDisplayedColumns: string[] = ['name'];
    operationParameters$: MatTableDataSource<Parameter> = new MatTableDataSource<Parameter>();
    presentationParameters$: MatTableDataSource<PresentationParameter> = new MatTableDataSource<PresentationParameter>();
    peopleAssignments$: MatTableDataSource<PeopleAssignment> = new MatTableDataSource<PeopleAssignment>();
    deadlines$: MatTableDataSource<Deadline> = new MatTableDataSource<Deadline>();
    option: any = {
        type: 'container',
        children: []
    };
    infoForm: FormGroup;

    constructor(
        private store: Store<fromAppState.AppState>,
        private formBuilder: FormBuilder,
        private actions$: ScannedActionsSubject,
        private snackBar: MatSnackBar,
        private translateService: TranslateService,
        private route: ActivatedRoute) {
        this.infoForm = this.formBuilder.group({
            name: new FormControl('', [
                Validators.required
            ]),
            priority: ''
        });
        this.actions$.pipe(
            filter((action: any) => action.type === fromHumanTaskDefActions.ActionTypes.COMPLETE_UPDATE_HUMANTASK_INFO))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant('HUMANTASK.MESSAGES.TASK_INFO_UPDATED'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromHumanTaskDefActions.ActionTypes.ERROR_UPDATE_HUMANTASK_INFO))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant('HUMANTASK.MESSAGES.ERROR_UPDATE_TASK_INFO'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromHumanTaskDefActions.ActionTypes.ERROR_GET_HUMANTASKDEF))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant('HUMANTASK.MESSAGES.UNKNOWN_HUMANTASK'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });
        this.operationParameters$.data = [new Parameter(), new Parameter()];
        this.presentationParameters$.data = [new PresentationParameter(), new PresentationParameter()];
        this.peopleAssignments$.data = [new PeopleAssignment(), new PeopleAssignment()];
        this.deadlines$.data = [new Deadline(), new Deadline()];
        this.store.pipe(select(fromAppState.selectHumanTaskResult)).subscribe((e: HumanTaskDef) => {
            if (!e) {
                return;
            }

            this.humanTaskDef = e;
            this.infoForm.get('name').setValue(e.name);
            this.infoForm.get('priority').setValue(e.priority);
            if (e.operationParameters.length > 0) {
                this.operationParameters$.data = e.operationParameters;
            }

            if (e.presentationParameters.length > 0) {
                this.presentationParameters$.data = e.presentationParameters;
            }

            if (e.peopleAssignments.length > 0) {
                this.peopleAssignments$.data = e.peopleAssignments;
            }

            if (e.deadLines.length > 0) {
                this.deadlines$.data = e.deadLines;
            }
        });
        this.refresh();
    }

    updateInfo(form: any) {
        if (!this.infoForm.valid) {
            return;
        }

        const request = new fromHumanTaskDefActions.UpdateHumanTaskInfo(this.humanTaskDef.id, form.name, form.priority);
        this.store.dispatch(request);
    }

    dragColumn(evt: any) {
        const json: any = {
            id: GuidGenerator.newGUID(),
            type: 'row',
            children: [
                { id: GuidGenerator.newGUID(), type: 'column', width: '50%', children: [] },
                { id: GuidGenerator.newGUID(), type: 'column', width: '50%', children: [] }
            ]
        };
        evt.dataTransfer.setData('json', JSON.stringify(json));
    }

    dragTxt(evt: any) {
        const json = {
            id: GuidGenerator.newGUID(),
            type: 'txt',
            label: 'Label',
            name: 'name'
        };
        evt.dataTransfer.setData('json', JSON.stringify(json));
    }

    dragSelect(evt: any) {
        const json = {
            id: GuidGenerator.newGUID(),
            type: 'select',
            label: 'Label',
            name: 'name'
        };
        evt.dataTransfer.setData('json', JSON.stringify(json));
    }

    dragHeader(evt: any) {
        const json = {
            id: GuidGenerator.newGUID(),
            type: 'header',
            txt: 'Header',
            class: 'mat-display-1'
        };
        evt.dataTransfer.setData('json', JSON.stringify(json));
    }

    dragOver(evt: any) {
        evt.preventDefault();
    }

    dropColumn(evt: any) {
        const json = JSON.parse(evt.dataTransfer.getData('json'));
        this.option.children.push(json);
    }

    refresh() {
        const id = this.route.parent.snapshot.params['id'];
        const request = new fromHumanTaskDefActions.GetHumanTaskDef(id);
        this.store.dispatch(request);
    }
}