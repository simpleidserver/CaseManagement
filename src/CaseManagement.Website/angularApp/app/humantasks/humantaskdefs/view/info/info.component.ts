import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatSnackBar } from '@angular/material';
import * as fromAppState from '@app/stores/appstate';
import { Parameter } from '@app/stores/common/operation.model';
import * as fromHumanTaskDefActions from '@app/stores/humantaskdefs/actions/humantaskdef.actions';
import { HumanTaskDef } from '@app/stores/humantaskdefs/models/humantaskdef.model';
import { select, Store } from '@ngrx/store';
import { TranslateService } from '@ngx-translate/core';

export class ParameterType {
    constructor(public type: string, public displayName: string) { }
}

@Component({
    selector: 'view-humantaskdef-info-component',
    templateUrl: './info.component.html',
    styleUrls: ['./info.component.scss'],
    encapsulation: ViewEncapsulation.None
})
export class ViewHumanTaskDefInfoComponent implements OnInit {
    baseTranslationKey: string = "HUMANTASK.DEF.VIEW.TASK";
    humanTaskDef: HumanTaskDef = new HumanTaskDef();
    infoForm: FormGroup;
    inputParameterForm: FormGroup;
    outputParameterForm: FormGroup;
    parameterTypes: ParameterType[];

    constructor(
        private store: Store<fromAppState.AppState>,
        private formBuilder: FormBuilder,
        private snackBar: MatSnackBar,
        private translateService: TranslateService) {
        this.infoForm = this.formBuilder.group({
            name: '',
            priority: ''
        });
        this.inputParameterForm = this.formBuilder.group({
            name: '',
            type: '',
            isRequired:''
        });
        this.outputParameterForm = this.formBuilder.group({
            name: '',
            type: '',
            isRequired: ''
        });
        this.parameterTypes = [];
        this.parameterTypes.push(new ParameterType("string", "string"));
        this.parameterTypes.push(new ParameterType("bool", "boolean"));
    }

    ngOnInit(): void {
        this.store.pipe(select(fromAppState.selectHumanTaskResult)).subscribe((e: HumanTaskDef) => {
            if (!e) {
                return;
            }

            this.infoForm.get('name').setValue(e.name);
            this.infoForm.get('priority').setValue(e.priority);
            this.humanTaskDef = e;
        });
    }

    updateInfo(form : any) {
        this.humanTaskDef.name = form.name;
        this.humanTaskDef.priority = form.priority;
    }

    addInputParameter(param: Parameter) {
        const filteredInputParam = this.humanTaskDef.operation.inputParameters.filter(function (p: Parameter) {
            return p.name === param.name
        });
        if (filteredInputParam.length === 1) {
            this.snackBar.open(this.translateService.instant(this.baseTranslationKey + '.INPUT_PARAMETER_EXISTS'), this.translateService.instant('undo'), {
                duration: 2000
            });
            return;
        }

        if (!param.isRequired) {
            param.isRequired = false;
        }

        this.inputParameterForm.reset();
        this.humanTaskDef.operation.inputParameters.push(param);
    }

    addOutputParameter(param: Parameter) {
        const filteredOutputParam = this.humanTaskDef.operation.outputParameters.filter(function (p: Parameter) {
            return p.name === param.name
        });
        if (filteredOutputParam.length === 1) {
            this.snackBar.open(this.translateService.instant(this.baseTranslationKey + '.OUTPUT_PARAMETER_EXISTS'), this.translateService.instant('undo'), {
                duration: 2000
            });
            return;
        }

        if (!param.isRequired) {
            param.isRequired = false;
        }

        this.outputParameterForm.reset();
        this.humanTaskDef.operation.outputParameters.push(param);
    }

    deleteInputParameter(param: Parameter) {
        const index = this.humanTaskDef.operation.inputParameters.indexOf(param);
        this.humanTaskDef.operation.inputParameters.splice(index, 1);
    }

    deleteOutputParameter(param: Parameter) {
        const index = this.humanTaskDef.operation.outputParameters.indexOf(param);
        this.humanTaskDef.operation.outputParameters.splice(index, 1);
    }

    update() {
        const request = new fromHumanTaskDefActions.UpdateHumanTaskDef(this.humanTaskDef);
        this.store.dispatch(request);
    }
}
