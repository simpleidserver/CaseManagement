import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup } from '@angular/forms';
import * as fromAppState from '@app/stores/appstate';
import * as fromCmmnFileActions from '@app/stores/cmmnfiles/actions/cmmn-files.actions';
import { CmmnFile } from '@app/stores/cmmnfiles/models/cmmn-file.model';
import { select, Store } from '@ngrx/store';

@Component({
    selector: 'view-cmmn-information-file',
    templateUrl: './information.component.html',
    styleUrls: ['./information.component.scss']
})
export class ViewCmmnFileInformationComponent implements OnInit {
    saveForm: FormGroup;
    cmmnFile: CmmnFile = new CmmnFile();

    constructor(private store: Store<fromAppState.AppState>,
        private formBuilder: FormBuilder) {
        this.saveForm = this.formBuilder.group({
            id: new FormControl({ value: '', disabled: true }),
            name: new FormControl({ value: '' }),
            createDateTime: new FormControl({ value: '', disabled: true }),
            updateDateTime: new FormControl({ value: '', disabled: true }),
            description: new FormControl({ value: '' })
        });
    }

    ngOnInit() {
        this.store.pipe(select(fromAppState.selectCmmnFileResult)).subscribe((e: CmmnFile) => {
            if (!e) {
                return;
            }

            this.cmmnFile = e;
            this.saveForm.controls['id'].setValue(e.id);
            this.saveForm.controls['name'].setValue(e.name);
            this.saveForm.controls['createDateTime'].setValue(e.createDateTime);
            this.saveForm.controls['updateDateTime'].setValue(e.updateDateTime);
            this.saveForm.controls['description'].setValue(e.description);
        });
    }

    onSave(form: any) {
        const id = this.saveForm.get('id').value;
        const act = new fromCmmnFileActions.UpdateCmmnFile(id, form.name, form.description);
        this.store.dispatch(act);
    }
}
