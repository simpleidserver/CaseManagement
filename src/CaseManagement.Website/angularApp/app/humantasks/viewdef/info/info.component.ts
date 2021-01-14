import { Component, Inject, OnDestroy, OnInit, ViewEncapsulation } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { COMMA } from '@angular/cdk/keycodes';
import { MatDialog, MatDialogRef, MatSnackBar, MatTableDataSource, MAT_DIALOG_DATA, MatChipInputEvent } from '@angular/material';
import * as fromAppState from '@app/stores/appstate';
import { Parameter } from '@app/stores/common/parameter.model';
import { PeopleAssignment } from '@app/stores/common/people-assignment.model';
import { PresentationParameter } from '@app/stores/common/presentationparameter.model';
import * as fromHumanTaskDefActions from '@app/stores/humantaskdefs/actions/humantaskdef.actions';
import { Deadline } from '@app/stores/humantaskdefs/models/deadline';
import { HumanTaskDef } from '@app/stores/humantaskdefs/models/humantaskdef.model';
import { ScannedActionsSubject, select, Store } from '@ngrx/store';
import { TranslateService } from '@ngx-translate/core';
import { filter } from 'rxjs/operators';
import { PresentationElement } from '@app/stores/common/presentationelement.model';

export class ParameterType {
    constructor(public type: string, public displayName: string) { }
}

export class Language {
    constructor(public code: string, public displayName: string) { }
}

export class ContentType {
    constructor(public code: string, public displayName: string) { }
}

@Component({
    selector: 'add-parameter-dialog-component',
    templateUrl: './addparameter.dialog.component.html',
    styleUrls: ['./addparameter.dialog.component.scss'],
    encapsulation: ViewEncapsulation.None
})
export class AddParameterDialogComponent {
    parameterTypes: ParameterType[];
    addParameterForm: FormGroup;
    usages: string[] = ['INPUT', 'OUTPUT'];
    constructor(
        public dialogRef: MatDialogRef<AddParameterDialogComponent>,
        @Inject(MAT_DIALOG_DATA) public data: { json: string },
        private formBuilder: FormBuilder
    ) {
        this.parameterTypes = [];
        this.parameterTypes.push(new ParameterType("STRING", "string"));
        this.parameterTypes.push(new ParameterType("BOOL", "boolean"));
        this.addParameterForm = this.formBuilder.group({
            name: new FormControl('', [
                Validators.required
            ]),
            type: new FormControl('', [
                Validators.required
            ]),
            usage: new FormControl('', [
                Validators.required
            ]),
            isRequired: ''
        });
    }

    addParameter(parameter: Parameter) {
        if (!this.addParameterForm.valid) {
            return;
        }

        if (!parameter.isRequired) {
            parameter.isRequired = false;
        }

        this.dialogRef.close(parameter);
    }
}

@Component({
    selector: 'add-presentation-parameter-dialog-component',
    templateUrl: './addpresentationparameter.dialog.component.html',
    styleUrls: ['./addpresentationparameter.dialog.component.scss'],
    encapsulation: ViewEncapsulation.None
})
export class AddPresentationParameterDialogComponent {
    addPresentationParameterForm: FormGroup;
    types: ParameterType[] = [
        new ParameterType("string", "String")
    ];

    constructor(
        public dialogRef: MatDialogRef<AddPresentationParameterDialogComponent>,
        @Inject(MAT_DIALOG_DATA) public data: { json: string },
        private formBuilder: FormBuilder) {
        this.addPresentationParameterForm = this.formBuilder.group({
            name: new FormControl('', [
                Validators.required
            ]),
            type: new FormControl('', [
                Validators.required
            ]),
            expression: new FormControl('', [
                Validators.required
            ])
        });
    }

    addPresentationParameter(presentationParameter: PresentationParameter) {
        if (!this.addPresentationParameterForm.valid) {
            return;
        }

        this.dialogRef.close(presentationParameter);
    }
}

@Component({
    selector: 'add-presentation-element-dialog-component',
    templateUrl: './addpresentationelement.dialog.component.html',
    styleUrls: ['./addpresentationelement.dialog.component.scss'],
    encapsulation: ViewEncapsulation.None
})
export class AddPresentationElementDialogComponent {
    languages: Language[] = [
        new Language("fr", "French"),
        new Language("en", "English")
    ];
    contentTypes: ContentType[] = [
        new ContentType("text/html", "HTML")
    ];
    types: ParameterType[] = [
        new ParameterType("string", "String")
    ];
    usages: string[] = ['NAME', 'SUBJECT', 'DESCRIPTION'];
    addPresentationElementForm: FormGroup;

    constructor(
        public dialogRef: MatDialogRef<AddPresentationParameterDialogComponent>,
        @Inject(MAT_DIALOG_DATA) public data: { json: string },
        private formBuilder: FormBuilder) {
        this.addPresentationElementForm = this.formBuilder.group({
            usage: new FormControl('', [
                Validators.required
            ]),
            language: new FormControl('', [
                Validators.required
            ]),
            value: new FormControl('', [
                Validators.required
            ]),
            contentType: new FormControl('', [
                Validators.required
            ])
        });
    }

    addPresentationElement(presentationElt: PresentationElement) {
        if (!this.addPresentationElementForm) {
            return;
        }

        this.dialogRef.close(presentationElt);
    }
}

@Component({
    selector: 'add-assignment-element-dialog-component',
    templateUrl: './addassignment.dialog.component.html',
    styleUrls: ['./addassignment.dialog.component.scss'],
    encapsulation: ViewEncapsulation.None
})
export class AddAssignmentDialogComponent {
    addAssignmentForm: FormGroup;
    selectable: boolean = true;
    removable: boolean = true;
    values: string[] = [];
    separatorKeysCodes: number[] = [COMMA];
    usages: string[] = ['POTENTIALOWNER', 'EXCLUDEDOWNER', 'TASKINITIATOR', 'TASKSTAKEHOLDER', 'BUSINESSADMINISTRATOR', 'RECIPIENT'];
    assignmentTypes: string[] = ['LOGICALPEOPLEGROUP', 'GROUPNAMES', 'USERIDENTIFIERS', 'EXPRESSION'];

    constructor(
        public dialogRef: MatDialogRef<AddPresentationParameterDialogComponent>,
        @Inject(MAT_DIALOG_DATA) public data: { json: string },
        private formBuilder: FormBuilder) {
        this.addAssignmentForm = this.formBuilder.group({
            usage: new FormControl('', [
                Validators.required
            ]),
            type: new FormControl('', [
                Validators.required
            ]),
            value: new FormControl('', [
                Validators.required
            ])
        });
    }

    add(event: MatChipInputEvent): void {
        console.log(event);
    }

    remove(str: string) : void {
        const index = this.values.indexOf(str);
        if (index >= 0) {
            this.values.splice(index, 1);
        }
    }

    addPeopleAssignment(peopleAssignment: PeopleAssignment) {
        if (!this.addAssignmentForm) {
            return;
        }

        this.dialogRef.close(peopleAssignment);
    }
}

@Component({
    selector: 'add-deadline-element-dialog-component',
    templateUrl: './adddeadline.dialog.component.html',
    styleUrls: ['./adddeadline.dialog.component.scss'],
    encapsulation: ViewEncapsulation.None
})
export class AddDeadlineComponentDialog {
    public addDeadlineForm: FormGroup;
    deadlineTypes: string[] = ['START', 'COMPLETION'];
    validityTypes: string[] = ['DURATION', 'EXPIRATION'];

    constructor(
        public dialogRef: MatDialogRef<AddDeadlineComponentDialog>,
        @Inject(MAT_DIALOG_DATA) public data: { json: string },
        private formBuilder: FormBuilder
    ) {
        this.addDeadlineForm = this.formBuilder.group({
            deadlineType: new FormControl('', [
                Validators.required
            ]),
            name: new FormControl('', [
                Validators.required
            ]),
            validityType: new FormControl('', [
                Validators.required
            ]),
            validity: new FormControl('', [
                Validators.required
            ])
        });
    }

    addDeadline(v: {deadlineType: string, name: string, validityType: string, validity: string}) {
        const deadline = new Deadline();
        deadline.name = v.name;
        deadline.usage = v.deadlineType;
        if (v.validityType === 'DURATION') {
            deadline.for = v.validity;
        } else {
            deadline.until = v.validity;
        }

        this.dialogRef.close(deadline);
    }
}

@Component({
    selector: 'view-humantaskdef-info-component',
    templateUrl: './info.component.html',
    styleUrls: ['./info.component.scss'],
    encapsulation: ViewEncapsulation.None
})
export class ViewHumanTaskDefInfoComponent implements OnInit, OnDestroy {
    humanTaskListener: any;
    humanTaskDef: HumanTaskDef;
    parameterDisplayedColumns: string[] = ['usage', 'name', 'type', 'isRequired', 'actions'];
    presentationParameterDisplayedColumns: string[] = ['name', 'type', 'expression', 'actions'];
    peopleAssignmentDisplayedColumns: string[] = ['type', 'usage', 'value', 'actions'];
    presentationEltDisplayedColumns: string[] = ['usage', 'value', 'actions'];
    deadlineDisplayedColumns: string[] = ['usage', 'name', 'for', 'until', 'escalation', 'actions'];
    operationParameters$: MatTableDataSource<Parameter> = new MatTableDataSource<Parameter>();
    presentationParameters$: MatTableDataSource<PresentationParameter> = new MatTableDataSource<PresentationParameter>();
    peopleAssignments$: MatTableDataSource<PeopleAssignment> = new MatTableDataSource<PeopleAssignment>();
    deadlines$: MatTableDataSource<Deadline> = new MatTableDataSource<Deadline>();
    presentationElts$: MatTableDataSource<PresentationElement> = new MatTableDataSource<PresentationElement>();
    infoForm: FormGroup;

    constructor(
        private store: Store<fromAppState.AppState>,
        private formBuilder: FormBuilder,
        private snackBar: MatSnackBar,
        private translateService: TranslateService,
        private actions$: ScannedActionsSubject,
        private matDialog: MatDialog) {
        this.infoForm = this.formBuilder.group({
            id: new FormControl({value: '', disabled: true}),
            name: new FormControl('', [
                Validators.required
            ]),
            priority: ''
        });
    }

    ngOnInit(): void {
        this.infoForm = this.formBuilder.group({
            name: new FormControl('', [
                Validators.required
            ]),
            priority: ''
        });
        this.operationParameters$.data = [new Parameter(), new Parameter()];
        this.presentationParameters$.data = [new PresentationParameter(), new PresentationParameter()];
        this.peopleAssignments$.data = [new PeopleAssignment(), new PeopleAssignment()];
        this.deadlines$.data = [new Deadline(), new Deadline()];
        this.presentationElts$.data = [new PresentationElement(), new PresentationElement()];
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
            filter((action: any) => action.type === fromHumanTaskDefActions.ActionTypes.COMPLETE_ADD_OPERATION_INPUT_PARAMETER))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant('HUMANTASK.MESSAGES.INPUT_PARAMETER_ADDED'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromHumanTaskDefActions.ActionTypes.COMPLETE_ADD_OPERATION_OUTPUT_PARAMETER))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant('HUMANTASK.MESSAGES.OUTPUT_PARAMETER_ADDED'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromHumanTaskDefActions.ActionTypes.ERROR_ADD_OPERATION_INPUT_PARAMETER))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant('HUMANTASK.MESSAGES.ERROR_ADD_INPUT_PARAMETER'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromHumanTaskDefActions.ActionTypes.ERROR_ADD_OPERATION_OUTPUT_PARAMETER))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant('HUMANTASK.MESSAGES.ERROR_ADD_OUTPUT_PARAMETER'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromHumanTaskDefActions.ActionTypes.COMPLETE_DELETE_OPERATION_INPUT_PARAMETER))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant('HUMANTASK.MESSAGES.INPUT_PARAMETER_REMOVED'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromHumanTaskDefActions.ActionTypes.COMPLETE_DELETE_OPERATION_OUTPUT_PARAMETER))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant('HUMANTASK.MESSAGES.OUTPUT_PARAMETER_REMOVED'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromHumanTaskDefActions.ActionTypes.ERROR_DELETE_OPERATION_INPUT_PARAMETER))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant('HUMANTASK.MESSAGES.ERROR_DELETE_INPUT_PARAMETER'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromHumanTaskDefActions.ActionTypes.ERROR_DELETE_OPERATION_OUTPUT_PARAMETER))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant('HUMANTASK.MESSAGES.ERROR_DELETE_OUTPUT_PARAMETER'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromHumanTaskDefActions.ActionTypes.COMPLETE_ADD_PRESENTATION_PARAMETER))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant('HUMANTASK.MESSAGES.PRESENTATION_PARAMETER_ADDED'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromHumanTaskDefActions.ActionTypes.COMPLETE_DELETE_PRESENTATION_PARAMETER))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant('HUMANTASK.MESSAGES.PRESENTATION_PARAMETER_REMOVED'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromHumanTaskDefActions.ActionTypes.ERROR_ADD_PRESENTATION_PARAMETER))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant('HUMANTASK.MESSAGES.ERROR_ADD_PRESENTATION_PARAMETER'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromHumanTaskDefActions.ActionTypes.ERROR_DELETE_PRESENTATION_PARAMETER))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant('HUMANTASK.MESSAGES.ERROR_DELETE_PRESENTATION_PARAMETER'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromHumanTaskDefActions.ActionTypes.COMPLETE_ADD_PRESENTATION_ELT))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant('HUMANTASK.MESSAGES.PRESENTATION_ELT_ADDED'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromHumanTaskDefActions.ActionTypes.COMPLETE_DELETE_PRESENTATION_ELT))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant('HUMANTASK.MESSAGES.PRESENTATION_ELT_REMOVED'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromHumanTaskDefActions.ActionTypes.ERROR_ADD_PRESENTATION_ELT))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant('HUMANTASK.MESSAGES.ERROR_ADD_PRESENTATION_ELT'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromHumanTaskDefActions.ActionTypes.ERROR_DELETE_PRESENTATION_ELT))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant('HUMANTASK.MESSAGES.ERROR_DELETE_PRESENTATION_ELT'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromHumanTaskDefActions.ActionTypes.COMPLETE_ADD_PEOPLE_ASSIGNMENT))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant('HUMANTASK.MESSAGES.PEOPLE_ASSIGNMENT_ADDED'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromHumanTaskDefActions.ActionTypes.COMPLETE_DELETE_PEOPLE_ASSIGNMENT))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant('HUMANTASK.MESSAGES.PEOPLE_ASSIGNMENT_REMOVED'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromHumanTaskDefActions.ActionTypes.ERROR_DELETE_PEOPLE_ASSIGNMENT))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant('HUMANTASK.MESSAGES.ERROR_DELETE_PEOPLE_ASSIGNMENT'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromHumanTaskDefActions.ActionTypes.ERROR_ADD_PEOPLE_ASSIGNMENT))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant('HUMANTASK.MESSAGES.ERROR_ADD_PEOPLE_ASSIGNMENT'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromHumanTaskDefActions.ActionTypes.COMPLETE_ADD_START_DEADLINE))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant('HUMANTASK.MESSAGES.START_DEADLINE_ADDED'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromHumanTaskDefActions.ActionTypes.ERROR_ADD_START_DEADLINE))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant('HUMANTASK.MESSAGES.ERROR_ADD_START_DEADLINE'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromHumanTaskDefActions.ActionTypes.COMPLETE_ADD_COMPLETION_DEADLINE))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant('HUMANTASK.MESSAGES.COMPLETION_DEADLINE_ADDED'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromHumanTaskDefActions.ActionTypes.ERROR_ADD_COMPLETION_DEADLINE))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant('HUMANTASK.MESSAGES.ERROR_ADD_COMPLETION_DEADLINE'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromHumanTaskDefActions.ActionTypes.COMPLETE_DELETE_START_DEALINE))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant('HUMANTASK.MESSAGES.START_DEADLINE_REMOVED'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromHumanTaskDefActions.ActionTypes.ERROR_DELETE_START_DEALINE))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant('HUMANTASK.MESSAGES.ERROR_REMOVE_START_DEADLINE'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromHumanTaskDefActions.ActionTypes.COMPLETE_DELETE_COMPLETION_DEADLINE))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant('HUMANTASK.MESSAGES.COMPLETION_DEADLINE_REMOVED'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromHumanTaskDefActions.ActionTypes.ERROR_DELETE_COMPLETION_DEADLINE))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant('HUMANTASK.MESSAGES.ERROR_REMOVE_COMPLETION_DEADLINE'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });
        this.humanTaskListener = this.store.pipe(select(fromAppState.selectHumanTaskResult)).subscribe((e: HumanTaskDef) => {
            if (!e) {
                return;
            }

            this.humanTaskDef = e;
            this.infoForm.get('name').setValue(e.name);
            this.infoForm.get('priority').setValue(e.priority);
            if (e.operationParameters.length > 0) {
                this.operationParameters$.data = e.operationParameters;
            } else {
                this.operationParameters$.data = [new Parameter(), new Parameter()]; 
            }

            if (e.presentationParameters.length > 0) {
                this.presentationParameters$.data = e.presentationParameters;
            } else {
                this.presentationParameters$.data = [new PresentationParameter(), new PresentationParameter()];
            }

            if (e.peopleAssignments.length > 0) {
                this.peopleAssignments$.data = e.peopleAssignments;
            } else {
                this.peopleAssignments$.data = [new PeopleAssignment(), new PeopleAssignment()];
            }

            if (e.deadLines.length > 0) {
                this.deadlines$.data = e.deadLines;
            } else {
                this.deadlines$.data = [ new Deadline(), new Deadline() ];
            }

            if (e.presentationElements.length > 0) {
                this.presentationElts$.data = e.presentationElements;
            } else {
                this.presentationElts$.data = [new PresentationElement(), new PresentationElement()];
            }
        });
    }

    ngOnDestroy(): void {
        this.humanTaskListener.unsubscribe();
    }

    updateInfo(form: any) {
        if (!this.infoForm.valid) {
            return;
        }

        const request = new fromHumanTaskDefActions.UpdateHumanTaskInfo(this.humanTaskDef.id, form.name, form.priority);
        this.store.dispatch(request);
    }

    addParameter(evt: any) {
        evt.preventDefault();
        const dialogRef = this.matDialog.open(AddParameterDialogComponent, {
            width: '800px'
        });
        dialogRef.afterClosed().subscribe((newParameter: Parameter) => {
            if (!newParameter) {
                return;
            }

            const filteredOutputParam = this.humanTaskDef.operationParameters.filter(function (p: Parameter) {
                return newParameter.name === p.name && newParameter.usage === p.usage;
            });
            if (filteredOutputParam.length === 1) {
                this.snackBar.open(this.translateService.instant('SHARED.PARAMETER_EXISTS'), this.translateService.instant('undo'), {
                    duration: 2000
                });
                return;
            }

            switch (newParameter.usage) {
                case 'INPUT':
                    this.addInputParameter(newParameter);
                    break;
                case 'OUTPUT':
                    this.addOutputParameter(newParameter);
                    break;
            }
        });
    }

    deleteParameter(p: Parameter) {
        switch (p.usage) {
            case 'INPUT':
                this.deleteInputParameter(p);
                break;
            case 'OUTPUT':
                this.deleteOutputParameter(p);
                break;
        }
    }

    addPresentationParameter(evt: any) {
        evt.preventDefault();
        const dialogRef = this.matDialog.open(AddPresentationParameterDialogComponent, {
            width: '800px'
        });
        dialogRef.afterClosed().subscribe((newParameter: PresentationParameter) => {
            if (!newParameter) {
                return;
            }

            const filteredOutputParam = this.humanTaskDef.presentationParameters.filter(function (p: PresentationParameter) {
                return newParameter.name === p.name;
            });
            if (filteredOutputParam.length === 1) {
                this.snackBar.open(this.translateService.instant('SHARED.PARAMETER_EXISTS'), this.translateService.instant('undo'), {
                    duration: 2000
                });
                return;
            }

            const request = new fromHumanTaskDefActions.AddPresentationParameter(this.humanTaskDef.id, newParameter);
            this.store.dispatch(request);
        });
    }

    addPresentationElt(evt: any) {
        evt.preventDefault();
        const dialogRef = this.matDialog.open(AddPresentationElementDialogComponent, {
            width: '800px'
        });
        dialogRef.afterClosed().subscribe((newPresentationElt: PresentationElement) => {
            if (!newPresentationElt) {
                return;
            }

            const filteredOutputParam = this.humanTaskDef.presentationElements.filter(function (p: PresentationElement) {
                return p.usage === newPresentationElt.usage && p.language === newPresentationElt.language;
            });
            if (filteredOutputParam.length === 1) {
                this.snackBar.open(this.translateService.instant('SHARED.PARAMETER_EXISTS'), this.translateService.instant('undo'), {
                    duration: 2000
                });
                return;
            }

            const request = new fromHumanTaskDefActions.AddPresentationElt(this.humanTaskDef.id, newPresentationElt);
            this.store.dispatch(request);
        });
    }

    addAssignment(evt: any) {
        evt.preventDefault();
        const dialogRef = this.matDialog.open(AddAssignmentDialogComponent, {
            width: '800px'
        });
        dialogRef.afterClosed().subscribe((pa: PeopleAssignment) => {
            if (!pa) {
                return;
            }

            const request = new fromHumanTaskDefActions.AddPeopleAssignment(this.humanTaskDef.id, pa);
            this.store.dispatch(request);
        });
    }

    addDeadline(evt: any) {
        evt.preventDefault();
        const dialogRef = this.matDialog.open(AddDeadlineComponentDialog, {
            width: '800px'
        });
        dialogRef.afterClosed().subscribe((d: Deadline) => {
            if (!d) {
                return;
            }

            switch (d.usage) {
                case 'START':
                    {
                        const request = new fromHumanTaskDefActions.AddStartDeadLine(this.humanTaskDef.id, d);
                        this.store.dispatch(request);
                    }
                    break;
                case 'COMPLETION':
                    {
                        const request = new fromHumanTaskDefActions.AddCompletionDeadLine(this.humanTaskDef.id, d);
                        this.store.dispatch(request);
                    }
                    break;
            }
        });
    }

    deletePresentationParameter(param: PresentationParameter) {
        const request = new fromHumanTaskDefActions.DeletePresentationParameter(this.humanTaskDef.id, param);
        this.store.dispatch(request);
    }

    deletePresentationElt(param: PresentationElement) {
        const request = new fromHumanTaskDefActions.DeletePresentationElt(this.humanTaskDef.id, param);
        this.store.dispatch(request);
    }

    deletePeopleAssignment(param: PeopleAssignment) {
        const request = new fromHumanTaskDefActions.DeletePeopleAssignment(this.humanTaskDef.id, param);
        this.store.dispatch(request);
    }

    deleteDeadline(deadline: Deadline) {
        switch (deadline.usage) {
            case 'START':
                {
                    const request = new fromHumanTaskDefActions.DeleteStartDeadlineOperation(this.humanTaskDef.id, deadline.id);
                    this.store.dispatch(request);
                }
                break;
            case 'COMPLETION':
                {
                    const request = new fromHumanTaskDefActions.DeleteCompletionDeadlineOperation(this.humanTaskDef.id, deadline.id);
                    this.store.dispatch(request);
                }
                break;
        }
    }

    private addInputParameter(param: Parameter) {
        const request = new fromHumanTaskDefActions.AddInputParameterOperation(this.humanTaskDef.id, param);
        this.store.dispatch(request);
    }

    private addOutputParameter(param: Parameter) {
        const request = new fromHumanTaskDefActions.AddOutputParameterOperation(this.humanTaskDef.id, param);
        this.store.dispatch(request);
    }

    private deleteInputParameter(param: Parameter) {
        const request = new fromHumanTaskDefActions.DeleteInputParameterOperation(this.humanTaskDef.id, param.name);
        this.store.dispatch(request);
    }

    private deleteOutputParameter(param: Parameter) {
        const request = new fromHumanTaskDefActions.DeleteOutputParameterOperation(this.humanTaskDef.id, param.name);
        this.store.dispatch(request);
    }
}