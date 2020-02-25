import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatSnackBar } from '@angular/material';
import { ActivatedRoute } from '@angular/router';
import { select, Store } from '@ngrx/store';
import { TranslateService } from '@ngx-translate/core';
import { StartGet } from '../actions/caseplaninstance';
import { CasePlanElementInstance, CasePlanInstance, FormElement, Translation } from '../models/caseplaninstance.model';
import * as fromCasePlanInstance from '../reducers';
import { CasePlanInstanceService } from '../services/caseplaninstance.service';

export class FormElementView {
    Id: string;
    IsRequired: boolean;
    Title: string;
    Description: string;
    Type: string;
}

export class FormView {
    constructor() {
        this.Elements = [];
    }

    CasePlanInstanceId: string;
    CasePlanElementInstanceId: string;
    Title: string;
    Elements: Array<FormElementView>;
}

@Component({
    selector: 'view-case-instances',
    templateUrl: './view.component.html',  
    styleUrls: ['./view.component.scss'],
    encapsulation: ViewEncapsulation.None
})
export class ViewCasePlanInstanceComponent implements OnInit {
    form: FormView = null;
    formGroup: FormGroup;
    casePlanInstance: CasePlanInstance = new CasePlanInstance();
    activeActivities: CasePlanElementInstance[] = [];
    enableActivities: CasePlanElementInstance[] = [];
    completeActivities: CasePlanElementInstance[] = [];
    milestones: CasePlanElementInstance[] = [];
    constructor(private store: Store<fromCasePlanInstance.CasePlanInstanceState>, private route: ActivatedRoute, private casePlanInstanceService: CasePlanInstanceService, private translateService: TranslateService, private snackBar: MatSnackBar, private formBuilder: FormBuilder) { }

    ngOnInit() {
        this.store.pipe(select(fromCasePlanInstance.selectGetResult)).subscribe((casePlanInstance : CasePlanInstance) => {
            if (!casePlanInstance) {
                return;
            }

            this.casePlanInstance = casePlanInstance;
            this.activeActivities = casePlanInstance.Elements.filter((cp) => {
                return this.isActivity(cp) && cp.State == "Active";
            });
            this.enableActivities = casePlanInstance.Elements.filter((cp) => {
                return this.isActivity(cp) && cp.State == "Enabled";
            });
            this.completeActivities = casePlanInstance.Elements.filter((cp) => {
                return this.isActivity(cp) && cp.State == "Completed";
            });
            this.milestones = casePlanInstance.Elements.filter((cp) => {
                return cp.Type == "milestone";
            });
        });
        this.refresh();
    }

    onSubmit(v: any) {
        this.casePlanInstanceService.confirmForm(this.form.CasePlanInstanceId, this.form.CasePlanElementInstanceId, v).subscribe(() => {
            this.snackBar.open(this.translateService.instant('FORM_IS_SUBMITTED'), this.translateService.instant('UNDO'), {
                duration: 2000
            });
        }, () => {
            this.snackBar.open(this.translateService.instant('CANNOT_SUBMIT_THE_FORM'), this.translateService.instant('UNDO'), {
                duration: 2000
            });
        });
    }

    enable(casePlanElementInstance : CasePlanElementInstance) {
        this.casePlanInstanceService.enable(this.casePlanInstance.Id, casePlanElementInstance.Id).subscribe(() => {
            this.snackBar.open(this.translateService.instant('ACTIVITY_IS_ENABLED'), this.translateService.instant('UNDO'), {
                duration: 2000
            });
        }, () => {
            this.snackBar.open(this.translateService.instant('CANNOT_ENABLE_ACTIVITY'), this.translateService.instant('UNDO'), {
                duration: 2000
            });
        });
    }

    viewForm(casePlanElementInstance: CasePlanElementInstance) {
        let lng = this.translateService.getDefaultLang();
        let formView = new FormView();
        formView.CasePlanInstanceId = this.casePlanInstance.Id;
        formView.CasePlanElementInstanceId = casePlanElementInstance.Id;
        let form = casePlanElementInstance.Form;
        form.Titles.filter(function (tr: Translation) {
            if (tr.Language == lng) {
                formView.Title = tr.Value;
            }
        });

        let obj : any= {};
        form.Elements.forEach(function (e: FormElement) {
            let formElementView = new FormElementView();
            obj[e.Id] = '';
            formElementView.Id = e.Id;
            formElementView.IsRequired = e.IsRequired;
            formElementView.Type = e.Type;
            e.Titles.filter(function (tr: Translation) {
                if (tr.Language == lng) {
                    formElementView.Title = tr.Value;
                }
            });
            e.Descriptions.filter(function (tr: Translation) {
                if (tr.Language == lng) {
                    formElementView.Description = tr.Value;
                }
            });

            formView.Elements.push(formElementView);
        });

        this.formGroup = this.formBuilder.group(obj);
        this.form = formView;
    }

    refresh() {
        var id = this.route.snapshot.params['id'];
        let request = new StartGet(id);
        this.store.dispatch(request);
    }

    private isActivity(casePlanElementInstance: CasePlanElementInstance) {
        return casePlanElementInstance.Type == "task" ||
            casePlanElementInstance.Type == "humantask" ||
            casePlanElementInstance.Type == "processtask";
    }
}