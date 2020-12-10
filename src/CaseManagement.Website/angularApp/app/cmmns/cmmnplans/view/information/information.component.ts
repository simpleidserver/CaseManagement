import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup } from '@angular/forms';
import * as fromAppState from '@app/stores/appstate';
import { CmmnPlan } from '@app/stores/cmmnplans/models/cmmn-plan.model';
import { select, Store } from '@ngrx/store';

@Component({
    selector: 'view-cmmn-information-plan',
    templateUrl: './information.component.html',
    styleUrls: ['./information.component.scss']
})
export class ViewCmmnPlanInformationComponent implements OnInit {
    saveForm: FormGroup;
    cmmnPlan: CmmnPlan = new CmmnPlan();

    constructor(
        private store: Store<fromAppState.AppState>,
        private formBuilder: FormBuilder) {
        this.saveForm = this.formBuilder.group({
            id: new FormControl({ value: '', disabled: true }),
            name: new FormControl({ value: '' }),
            createDateTime: new FormControl({ value: '', disabled: true }),
            description: new FormControl({ value: '' })
        });
    }

    ngOnInit() {
        this.store.pipe(select(fromAppState.selectCmmnPlanResult)).subscribe((e: CmmnPlan) => {
            if (!e) {
                return;
            }

            this.cmmnPlan = e;
            this.saveForm.controls['id'].setValue(e.id);
            this.saveForm.controls['name'].setValue(e.name);
            this.saveForm.controls['createDateTime'].setValue(e.createDateTime);
            this.saveForm.controls['description'].setValue(e.description);
        });
    }
}