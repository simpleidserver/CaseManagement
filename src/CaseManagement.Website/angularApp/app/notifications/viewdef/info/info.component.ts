import { COMMA } from '@angular/cdk/keycodes';
import { Component, Inject, OnDestroy, OnInit, ViewEncapsulation } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatChipInputEvent, MatDialog, MatDialogRef, MatSnackBar, MatTableDataSource, MAT_DIALOG_DATA } from '@angular/material';
import * as fromAppState from '@app/stores/appstate';
import { Parameter } from '@app/stores/common/parameter.model';
import { PeopleAssignment } from '@app/stores/common/people-assignment.model';
import { PresentationParameter } from '@app/stores/common/presentationparameter.model';
import * as fromNotificationDefActions from '@app/stores/notificationdefs/actions/notificationdef.actions';
import { NotificationDefinition } from '@app/stores/notificationdefs/models/notificationdef.model';
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

    remove(str: string): void {
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
    selector: 'view-notificationdef-info-component',
    templateUrl: './info.component.html',
    styleUrls: ['./info.component.scss'],
    encapsulation: ViewEncapsulation.None
})
export class ViewNotificationDefInfoComponent implements OnInit, OnDestroy {
    notificationListener: any;
    notificationDef: NotificationDefinition;
    parameterDisplayedColumns: string[] = ['usage', 'name', 'type', 'isRequired', 'actions'];
    presentationParameterDisplayedColumns: string[] = ['name', 'type', 'expression', 'actions'];
    peopleAssignmentDisplayedColumns: string[] = ['type', 'usage', 'value', 'actions'];
    presentationEltDisplayedColumns: string[] = ['usage', 'value', 'actions'];
    operationParameters$: MatTableDataSource<Parameter> = new MatTableDataSource<Parameter>();
    presentationParameters$: MatTableDataSource<PresentationParameter> = new MatTableDataSource<PresentationParameter>();
    peopleAssignments$: MatTableDataSource<PeopleAssignment> = new MatTableDataSource<PeopleAssignment>();
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
        this.presentationElts$.data = [new PresentationElement(), new PresentationElement()];
        this.actions$.pipe(
            filter((action: any) => action.type === fromNotificationDefActions.ActionTypes.COMPLETE_UPDATE_NOTIFICATION_INFO))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant('NOTIFICATION.MESSAGES.INFO_UPDATED'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromNotificationDefActions.ActionTypes.ERROR_UPDATE_NOTIFICATION_INFO))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant('NOTIFICATION.MESSAGES.ERROR_UPDATE_INFO'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromNotificationDefActions.ActionTypes.COMPLETE_ADD_OPERATION_PARAMETER))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant('NOTIFICATION.MESSAGES.PARAMETER_ADDED'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromNotificationDefActions.ActionTypes.ERROR_ADD_OPERATION_PARAMETER))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant('NOTIFICATION.MESSAGES.ERROR_ADD_PARAMETER'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromNotificationDefActions.ActionTypes.COMPLETE_DELETE_OPERATION_PARAMETER))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant('NOTIFICATION.MESSAGES.PARAMETER_REMOVED'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromNotificationDefActions.ActionTypes.ERROR_DELETE_OPERATION_PARAMETER))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant('NOTIFICATION.MESSAGES.ERROR_DELETE_PARAMETER'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromNotificationDefActions.ActionTypes.COMPLETE_ADD_PRESENTATION_PARAMETER))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant('NOTIFICATION.MESSAGES.PRESENTATION_PARAMETER_ADDED'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromNotificationDefActions.ActionTypes.COMPLETE_DELETE_PRESENTATION_PARAMETER))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant('NOTIFICATION.MESSAGES.PRESENTATION_PARAMETER_REMOVED'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromNotificationDefActions.ActionTypes.ERROR_ADD_PRESENTATION_PARAMETER))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant('NOTIFICATION.MESSAGES.ERROR_ADD_PRESENTATION_PARAMETER'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromNotificationDefActions.ActionTypes.ERROR_DELETE_PRESENTATION_PARAMETER))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant('NOTIFICATION.MESSAGES.ERROR_DELETE_PRESENTATION_PARAMETER'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromNotificationDefActions.ActionTypes.COMPLETE_ADD_PRESENTATION_ELT))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant('NOTIFICATION.MESSAGES.PRESENTATION_ELT_ADDED'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromNotificationDefActions.ActionTypes.COMPLETE_DELETE_PRESENTATION_ELT))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant('NOTIFICATION.MESSAGES.PRESENTATION_ELT_REMOVED'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromNotificationDefActions.ActionTypes.ERROR_ADD_PRESENTATION_ELT))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant('NOTIFICATION.MESSAGES.ERROR_ADD_PRESENTATION_ELT'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromNotificationDefActions.ActionTypes.ERROR_DELETE_PRESENTATION_ELT))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant('NOTIFICATION.MESSAGES.ERROR_DELETE_PRESENTATION_ELT'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromNotificationDefActions.ActionTypes.COMPLETE_ADD_PEOPLE_ASSIGNMENT))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant('NOTIFICATION.MESSAGES.PEOPLE_ASSIGNMENT_ADDED'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromNotificationDefActions.ActionTypes.COMPLETE_DELETE_PEOPLE_ASSIGNMENT))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant('NOTIFICATION.MESSAGES.PEOPLE_ASSIGNMENT_REMOVED'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromNotificationDefActions.ActionTypes.ERROR_DELETE_PEOPLE_ASSIGNMENT))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant('NOTIFICATION.MESSAGES.ERROR_DELETE_PEOPLE_ASSIGNMENT'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromNotificationDefActions.ActionTypes.ERROR_ADD_PEOPLE_ASSIGNMENT))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant('NOTIFICATION.MESSAGES.ERROR_ADD_PEOPLE_ASSIGNMENT'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });
        this.notificationListener = this.store.pipe(select(fromAppState.selectNotificationResult)).subscribe((e: NotificationDefinition) => {
            if (!e) {
                return;
            }

            this.notificationDef = e;
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

            if (e.presentationElements.length > 0) {
                this.presentationElts$.data = e.presentationElements;
            } else {
                this.presentationElts$.data = [new PresentationElement(), new PresentationElement()];
            }
        });
    }

    ngOnDestroy(): void {
        this.notificationListener.unsubscribe();
    }

    updateInfo(form: any) {
        if (!this.infoForm.valid) {
            return;
        }

        const request = new fromNotificationDefActions.UpdateNotificationInfo(this.notificationDef.id, form.name, form.priority);
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

            const filteredOutputParam = this.notificationDef.operationParameters.filter(function (p: Parameter) {
                return newParameter.name === p.name && newParameter.usage === p.usage;
            });
            if (filteredOutputParam.length === 1) {
                this.snackBar.open(this.translateService.instant('SHARED.PARAMETER_EXISTS'), this.translateService.instant('undo'), {
                    duration: 2000
                });
                return;
            }

            const request = new fromNotificationDefActions.AddOperationParameterOperation(this.notificationDef.id, newParameter);
            this.store.dispatch(request);
        });
    }

    deleteParameter(p: Parameter) {
        const request = new fromNotificationDefActions.DeleteOperationParameterOperation(this.notificationDef.id, p.id);
        this.store.dispatch(request);
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

            const filteredOutputParam = this.notificationDef.presentationParameters.filter(function (p: PresentationParameter) {
                return newParameter.name === p.name;
            });
            if (filteredOutputParam.length === 1) {
                this.snackBar.open(this.translateService.instant('SHARED.PARAMETER_EXISTS'), this.translateService.instant('undo'), {
                    duration: 2000
                });
                return;
            }

            const request = new fromNotificationDefActions.AddPresentationParameter(this.notificationDef.id, newParameter);
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

            const filteredOutputParam = this.notificationDef.presentationElements.filter(function (p: PresentationElement) {
                return p.usage === newPresentationElt.usage && p.language === newPresentationElt.language;
            });
            if (filteredOutputParam.length === 1) {
                this.snackBar.open(this.translateService.instant('SHARED.PARAMETER_EXISTS'), this.translateService.instant('undo'), {
                    duration: 2000
                });
                return;
            }

            const request = new fromNotificationDefActions.AddPresentationElt(this.notificationDef.id, newPresentationElt);
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

            const request = new fromNotificationDefActions.AddPeopleAssignment(this.notificationDef.id, pa);
            this.store.dispatch(request);
        });
    }

    deletePresentationParameter(param: PresentationParameter) {
        const request = new fromNotificationDefActions.DeletePresentationParameter(this.notificationDef.id, param);
        this.store.dispatch(request);
    }

    deletePresentationElt(param: PresentationElement) {
        const request = new fromNotificationDefActions.DeletePresentationElt(this.notificationDef.id, param);
        this.store.dispatch(request);
    }

    deletePeopleAssignment(param: PeopleAssignment) {
        const request = new fromNotificationDefActions.DeletePeopleAssignment(this.notificationDef.id, param);
        this.store.dispatch(request);
    }
}