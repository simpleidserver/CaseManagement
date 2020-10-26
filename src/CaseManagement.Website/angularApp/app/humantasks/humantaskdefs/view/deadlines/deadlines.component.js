var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
import { Component, ViewEncapsulation } from '@angular/core';
import { FormBuilder, FormControl, Validators } from '@angular/forms';
import { MatDialog, MatSnackBar, MatTableDataSource } from '@angular/material';
import * as fromAppState from '@app/stores/appstate';
import * as fromHumanTaskDefActions from '@app/stores/humantaskdefs/actions/humantaskdef.actions';
import { HumanTaskDefinitionDeadLine } from '@app/stores/humantaskdefs/models/humantaskdef-deadlines';
import { HumanTaskDef } from '@app/stores/humantaskdefs/models/humantaskdef.model';
import { ScannedActionsSubject, select, Store } from '@ngrx/store';
import { TranslateService } from '@ngx-translate/core';
import { filter } from 'rxjs/operators';
import { AddEscalationDialog } from './add-escalation-dialog.component';
import { EditEscalationDialog } from './edit-escalation-dialog.component';
var ParameterType = (function () {
    function ParameterType(type, displayName) {
        this.type = type;
        this.displayName = displayName;
    }
    return ParameterType;
}());
export { ParameterType };
var ViewHumanTaskDefDeadlinesComponent = (function () {
    function ViewHumanTaskDefDeadlinesComponent(store, formBuilder, actions$, snackBar, dialog, translateService) {
        this.store = store;
        this.formBuilder = formBuilder;
        this.actions$ = actions$;
        this.snackBar = snackBar;
        this.dialog = dialog;
        this.translateService = translateService;
        this.baseTranslationKey = "HUMANTASK.DEF.VIEW.DEADLINES";
        this.humanTaskDef = new HumanTaskDef();
        this.displayedDeadLineColumns = ['name', 'for', 'until', 'actions'];
        this.displayedEscalationColumns = ['condition', 'actions'];
        this.startDeadlines = new MatTableDataSource();
        this.completionDeadlines = new MatTableDataSource();
        this.escalations = new MatTableDataSource();
        this.addDeadlineForm = this.formBuilder.group({
            name: new FormControl('', [
                Validators.required
            ]),
            deadlineType: new FormControl('', [
                Validators.required
            ]),
            validityType: new FormControl('', [
                Validators.required
            ]),
            validity: new FormControl('', [
                Validators.required
            ])
        });
        this.updateDeadlineForm = this.formBuilder.group({
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
    ViewHumanTaskDefDeadlinesComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.actions$.pipe(filter(function (action) { return action.type === fromHumanTaskDefActions.ActionTypes.COMPLETE_ADD_START_DEADLINE; }))
            .subscribe(function () {
            _this.snackBar.open(_this.translateService.instant(_this.baseTranslationKey + '.START_DEADLINE_ADDED'), _this.translateService.instant('undo'), {
                duration: 2000
            });
        });
        this.actions$.pipe(filter(function (action) { return action.type === fromHumanTaskDefActions.ActionTypes.COMPLETE_ADD_COMPLETION_DEADLINE; }))
            .subscribe(function () {
            _this.snackBar.open(_this.translateService.instant(_this.baseTranslationKey + '.COMPLETION_DEADLINE_ADDED'), _this.translateService.instant('undo'), {
                duration: 2000
            });
        });
        this.actions$.pipe(filter(function (action) { return action.type === fromHumanTaskDefActions.ActionTypes.ERROR_ADD_START_DEADLINE; }))
            .subscribe(function () {
            _this.snackBar.open(_this.translateService.instant(_this.baseTranslationKey + '.ERROR_ADD_START_DEADLINE'), _this.translateService.instant('undo'), {
                duration: 2000
            });
        });
        this.actions$.pipe(filter(function (action) { return action.type === fromHumanTaskDefActions.ActionTypes.ERROR_ADD_COMPLETION_DEADLINE; }))
            .subscribe(function () {
            _this.snackBar.open(_this.translateService.instant(_this.baseTranslationKey + '.ERROR_ADD_COMPLETION_DEADLINE'), _this.translateService.instant('undo'), {
                duration: 2000
            });
        });
        this.actions$.pipe(filter(function (action) { return action.type === fromHumanTaskDefActions.ActionTypes.COMPLETE_DELETE_START_DEALINE; }))
            .subscribe(function () {
            _this.snackBar.open(_this.translateService.instant(_this.baseTranslationKey + '.START_DEADLINE_REMOVED'), _this.translateService.instant('undo'), {
                duration: 2000
            });
        });
        this.actions$.pipe(filter(function (action) { return action.type === fromHumanTaskDefActions.ActionTypes.ERROR_DELETE_START_DEALINE; }))
            .subscribe(function () {
            _this.snackBar.open(_this.translateService.instant(_this.baseTranslationKey + '.ERROR_REMOVE_START_DEADLINE'), _this.translateService.instant('undo'), {
                duration: 2000
            });
        });
        this.actions$.pipe(filter(function (action) { return action.type === fromHumanTaskDefActions.ActionTypes.COMPLETE_DELETE_COMPLETION_DEADLINE; }))
            .subscribe(function () {
            _this.snackBar.open(_this.translateService.instant(_this.baseTranslationKey + '.COMPLETION_DEADLINE_REMOVED'), _this.translateService.instant('undo'), {
                duration: 2000
            });
        });
        this.actions$.pipe(filter(function (action) { return action.type === fromHumanTaskDefActions.ActionTypes.COMPLETE_UPDATE_START_DEADLINE; }))
            .subscribe(function () {
            _this.snackBar.open(_this.translateService.instant(_this.baseTranslationKey + '.UPDATE_DEADLINE'), _this.translateService.instant('undo'), {
                duration: 2000
            });
        });
        this.actions$.pipe(filter(function (action) { return action.type === fromHumanTaskDefActions.ActionTypes.ERROR_UPDATE_START_DEADLINE; }))
            .subscribe(function () {
            _this.snackBar.open(_this.translateService.instant(_this.baseTranslationKey + '.ERROR_UPDATE_DEADLINE'), _this.translateService.instant('undo'), {
                duration: 2000
            });
        });
        this.actions$.pipe(filter(function (action) { return action.type === fromHumanTaskDefActions.ActionTypes.COMPLETE_UPDATE_COMPLETION_DEADLINE; }))
            .subscribe(function () {
            _this.snackBar.open(_this.translateService.instant(_this.baseTranslationKey + '.UPDATE_DEADLINE'), _this.translateService.instant('undo'), {
                duration: 2000
            });
        });
        this.actions$.pipe(filter(function (action) { return action.type === fromHumanTaskDefActions.ActionTypes.ERROR_UPDATE_COMPLETION_DEADLINE; }))
            .subscribe(function () {
            _this.snackBar.open(_this.translateService.instant(_this.baseTranslationKey + '.ERROR_UPDATE_DEADLINE'), _this.translateService.instant('undo'), {
                duration: 2000
            });
        });
        this.actions$.pipe(filter(function (action) { return action.type === fromHumanTaskDefActions.ActionTypes.COMPLETE_ADD_ESCALATION_STARTDEADLINE; }))
            .subscribe(function () {
            _this.snackBar.open(_this.translateService.instant(_this.baseTranslationKey + '.ADD_ESCALATION'), _this.translateService.instant('undo'), {
                duration: 2000
            });
        });
        this.actions$.pipe(filter(function (action) { return action.type === fromHumanTaskDefActions.ActionTypes.ERROR_ADD_ESCALATION_STARTDEADLINE; }))
            .subscribe(function () {
            _this.snackBar.open(_this.translateService.instant(_this.baseTranslationKey + '.ERROR_ADD_ESCALATION'), _this.translateService.instant('undo'), {
                duration: 2000
            });
        });
        this.actions$.pipe(filter(function (action) { return action.type === fromHumanTaskDefActions.ActionTypes.COMPLETE_ADD_ESCALATION_COMPLETIONDEADLINE; }))
            .subscribe(function () {
            _this.snackBar.open(_this.translateService.instant(_this.baseTranslationKey + '.ADD_ESCALATION'), _this.translateService.instant('undo'), {
                duration: 2000
            });
        });
        this.actions$.pipe(filter(function (action) { return action.type === fromHumanTaskDefActions.ActionTypes.ERROR_ADD_ESCALATION_COMPLETIONDEADLINE; }))
            .subscribe(function () {
            _this.snackBar.open(_this.translateService.instant(_this.baseTranslationKey + '.ERROR_ADD_ESCALATION'), _this.translateService.instant('undo'), {
                duration: 2000
            });
        });
        this.actions$.pipe(filter(function (action) { return action.type === fromHumanTaskDefActions.ActionTypes.COMPLETE_UPDATE_START_ESCALATION; }))
            .subscribe(function () {
            _this.snackBar.open(_this.translateService.instant(_this.baseTranslationKey + '.UPDATE_ESCALATION'), _this.translateService.instant('undo'), {
                duration: 2000
            });
        });
        this.actions$.pipe(filter(function (action) { return action.type === fromHumanTaskDefActions.ActionTypes.ERROR_UPDATE_START_ESCALATION; }))
            .subscribe(function () {
            _this.snackBar.open(_this.translateService.instant(_this.baseTranslationKey + '.ERROR_UPDATE_ESCALATION'), _this.translateService.instant('undo'), {
                duration: 2000
            });
        });
        this.actions$.pipe(filter(function (action) { return action.type === fromHumanTaskDefActions.ActionTypes.COMPLETE_UPDATE_COMPLETION_ESCALATION; }))
            .subscribe(function () {
            _this.snackBar.open(_this.translateService.instant(_this.baseTranslationKey + '.UPDATE_ESCALATION'), _this.translateService.instant('undo'), {
                duration: 2000
            });
        });
        this.actions$.pipe(filter(function (action) { return action.type === fromHumanTaskDefActions.ActionTypes.ERROR_UPDATE_COMPLETION_ESCALATION; }))
            .subscribe(function () {
            _this.snackBar.open(_this.translateService.instant(_this.baseTranslationKey + '.ERROR_UPDATE_ESCALATION'), _this.translateService.instant('undo'), {
                duration: 2000
            });
        });
        this.actions$.pipe(filter(function (action) { return action.type === fromHumanTaskDefActions.ActionTypes.COMPLETE_DELETE_START_ESCALATION; }))
            .subscribe(function () {
            _this.snackBar.open(_this.translateService.instant(_this.baseTranslationKey + '.ESCALATION_REMOVED'), _this.translateService.instant('undo'), {
                duration: 2000
            });
        });
        this.actions$.pipe(filter(function (action) { return action.type === fromHumanTaskDefActions.ActionTypes.COMPLETE_DELETE_COMPLETION_ESCALATION; }))
            .subscribe(function () {
            _this.snackBar.open(_this.translateService.instant(_this.baseTranslationKey + '.ESCALATION_REMOVED'), _this.translateService.instant('undo'), {
                duration: 2000
            });
        });
        this.actions$.pipe(filter(function (action) { return action.type === fromHumanTaskDefActions.ActionTypes.ERROR_DELETE_COMPLETION_DEADLINE; }))
            .subscribe(function () {
            _this.snackBar.open(_this.translateService.instant(_this.baseTranslationKey + '.ESCALATION_REMOVED'), _this.translateService.instant('undo'), {
                duration: 2000
            });
        });
        this.actions$.pipe(filter(function (action) { return action.type === fromHumanTaskDefActions.ActionTypes.ERROR_DELETE_START_ESCALATION; }))
            .subscribe(function () {
            _this.snackBar.open(_this.translateService.instant(_this.baseTranslationKey + '.ERROR_DELETE_ESCALATION'), _this.translateService.instant('undo'), {
                duration: 2000
            });
        });
        this.actions$.pipe(filter(function (action) { return action.type === fromHumanTaskDefActions.ActionTypes.COMPLETE_DELETE_COMPLETION_DEADLINE; }))
            .subscribe(function () {
            _this.snackBar.open(_this.translateService.instant(_this.baseTranslationKey + '.ESCALATION_REMOVED'), _this.translateService.instant('undo'), {
                duration: 2000
            });
        });
        this.actions$.pipe(filter(function (action) { return action.type === fromHumanTaskDefActions.ActionTypes.ERROR_DELETE_COMPLETION_DEADLINE; }))
            .subscribe(function () {
            _this.snackBar.open(_this.translateService.instant(_this.baseTranslationKey + '.ERROR_DELETE_ESCALATION'), _this.translateService.instant('undo'), {
                duration: 2000
            });
        });
        this.store.pipe(select(fromAppState.selectHumanTaskResult)).subscribe(function (e) {
            if (!e) {
                return;
            }
            _this.humanTaskDef = e;
            _this.startDeadlines.data = _this.humanTaskDef.deadLines.startDeadLines;
            _this.completionDeadlines.data = _this.humanTaskDef.deadLines.completionDeadLines;
            if (_this.currentDeadline) {
                var id_1 = _this.currentDeadline.id;
                if (_this.currentDeadlineType === 'start') {
                    _this.currentDeadline = _this.humanTaskDef.deadLines.startDeadLines.filter(function (v) {
                        return v.id === id_1;
                    })[0];
                }
                else {
                    _this.currentDeadline = _this.humanTaskDef.deadLines.completionDeadLines.filter(function (v) {
                        return v.id === id_1;
                    })[0];
                }
                _this.escalations.data = _this.currentDeadline.escalations;
            }
        });
    };
    ViewHumanTaskDefDeadlinesComponent.prototype.addDeadLine = function (form) {
        if (!this.addDeadlineForm.valid) {
            return;
        }
        var deadline = new HumanTaskDefinitionDeadLine();
        deadline.name = form.name;
        if (form.validityType === 'duration') {
            deadline.for = form.validity;
        }
        else {
            deadline.until = form.validity;
        }
        if (form.deadlineType === 'start') {
            var request = new fromHumanTaskDefActions.AddStartDeadLine(this.humanTaskDef.id, deadline);
            this.store.dispatch(request);
        }
        else {
            var request = new fromHumanTaskDefActions.AddCompletionDeadLine(this.humanTaskDef.id, deadline);
            this.store.dispatch(request);
        }
        this.addDeadlineForm.reset();
    };
    ViewHumanTaskDefDeadlinesComponent.prototype.updateDeadline = function (form) {
        if (!this.updateDeadlineForm.valid || !this.currentDeadline) {
            return;
        }
        var deadline = new HumanTaskDefinitionDeadLine();
        deadline.id = this.currentDeadline.id;
        deadline.name = form.name;
        if (form.validityType === 'duration') {
            deadline.for = form.validity;
        }
        else {
            deadline.until = form.validity;
        }
        if (this.currentDeadlineType === 'start') {
            var request = new fromHumanTaskDefActions.UpdateStartDeadlineOperation(this.humanTaskDef.id, deadline);
            this.store.dispatch(request);
        }
        else {
            var request = new fromHumanTaskDefActions.UpdateCompletionDeadlineOperation(this.humanTaskDef.id, deadline);
            this.store.dispatch(request);
        }
    };
    ViewHumanTaskDefDeadlinesComponent.prototype.removeStartDeadline = function (deadline) {
        this.currentDeadline = null;
        var request = new fromHumanTaskDefActions.DeleteStartDeadlineOperation(this.humanTaskDef.id, deadline.id);
        this.store.dispatch(request);
    };
    ViewHumanTaskDefDeadlinesComponent.prototype.removeCompletionDeadline = function (deadline) {
        this.currentDeadline = null;
        var request = new fromHumanTaskDefActions.DeleteCompletionDeadlineOperation(this.humanTaskDef.id, deadline.id);
        this.store.dispatch(request);
    };
    ViewHumanTaskDefDeadlinesComponent.prototype.edit = function (deadlineType, deadline) {
        this.currentDeadlineType = deadlineType;
        this.currentDeadline = deadline;
        this.escalations.data = this.currentDeadline.escalations;
        this.updateDeadlineForm.get('name').setValue(deadline.name);
        if (deadline.for) {
            this.updateDeadlineForm.get('validityType').setValue('duration');
            this.updateDeadlineForm.get('validity').setValue(deadline.for);
        }
        else {
            this.updateDeadlineForm.get('validityType').setValue('expiration');
            this.updateDeadlineForm.get('validity').setValue(deadline.until);
        }
    };
    ViewHumanTaskDefDeadlinesComponent.prototype.addEscalation = function () {
        var _this = this;
        var dialogRef = this.dialog.open(AddEscalationDialog, {
            width: '800px'
        });
        dialogRef.afterClosed().subscribe(function (e) {
            if (!e) {
                return;
            }
            if (_this.currentDeadlineType === 'start') {
                var request = new fromHumanTaskDefActions.AddEscalationStartDeadlineOperation(_this.humanTaskDef.id, _this.currentDeadline.id, e.condition);
                _this.store.dispatch(request);
            }
            else {
                var request = new fromHumanTaskDefActions.AddEscalationCompletionDeadlineOperation(_this.humanTaskDef.id, _this.currentDeadline.id, e.condition);
                _this.store.dispatch(request);
            }
        });
    };
    ViewHumanTaskDefDeadlinesComponent.prototype.editEscalation = function (escalation) {
        var _this = this;
        var dialogRef = this.dialog.open(EditEscalationDialog, {
            width: '800px',
            data: escalation
        });
        dialogRef.afterClosed().subscribe(function (e) {
            if (!e) {
                return;
            }
            if (_this.currentDeadlineType === 'start') {
                var request = new fromHumanTaskDefActions.UpdateStartEscalationOperation(_this.humanTaskDef.id, _this.currentDeadline.id, e);
                _this.store.dispatch(request);
            }
            else {
                var request = new fromHumanTaskDefActions.UpdateCompletionEscalationOperation(_this.humanTaskDef.id, _this.currentDeadline.id, e);
                _this.store.dispatch(request);
            }
        });
    };
    ViewHumanTaskDefDeadlinesComponent.prototype.deleteEscalation = function (escalation) {
        if (this.currentDeadlineType === 'start') {
            var request = new fromHumanTaskDefActions.DeleteStartEscalationOperation(this.humanTaskDef.id, this.currentDeadline.id, escalation);
            this.store.dispatch(request);
        }
        else {
            var request = new fromHumanTaskDefActions.DeleteCompletionEscalationOperation(this.humanTaskDef.id, this.currentDeadline.id, escalation);
            this.store.dispatch(request);
        }
    };
    ViewHumanTaskDefDeadlinesComponent = __decorate([
        Component({
            selector: 'view-humantaskdef-deadlines-component',
            templateUrl: './deadlines.component.html',
            styleUrls: ['./deadlines.component.scss'],
            encapsulation: ViewEncapsulation.None
        }),
        __metadata("design:paramtypes", [Store,
            FormBuilder,
            ScannedActionsSubject,
            MatSnackBar,
            MatDialog,
            TranslateService])
    ], ViewHumanTaskDefDeadlinesComponent);
    return ViewHumanTaskDefDeadlinesComponent;
}());
export { ViewHumanTaskDefDeadlinesComponent };
//# sourceMappingURL=deadlines.component.js.map