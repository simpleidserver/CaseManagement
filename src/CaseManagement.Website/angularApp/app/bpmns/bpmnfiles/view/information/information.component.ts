import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup } from '@angular/forms';
import * as fromAppState from '@app/stores/appstate';
import * as fromBpmnFileActions from '@app/stores/bpmnfiles/actions/bpmn-files.actions';
import { BpmnFile } from '@app/stores/bpmnfiles/models/bpmn-file.model';
import { select, Store } from '@ngrx/store';

@Component({
    selector: 'view-bpmn-information-file',
    templateUrl: './information.component.html',
    styleUrls: ['./information.component.scss']
})
export class ViewBpmnFileInformationComponent implements OnInit {
    saveForm: FormGroup;
    bpmnFile: BpmnFile = new BpmnFile();

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
        this.store.pipe(select(fromAppState.selectBpmnFileResult)).subscribe((e: BpmnFile) => {
            if (!e) {
                return;
            }

            this.bpmnFile = e;
            this.saveForm.controls['id'].setValue(e.id);
            this.saveForm.controls['name'].setValue(e.name);
            this.saveForm.controls['createDateTime'].setValue(e.createDateTime);
            this.saveForm.controls['updateDateTime'].setValue(e.updateDateTime);
            this.saveForm.controls['description'].setValue(e.description);
        });
    }

    onSave(form: any) {
        const id = this.saveForm.get('id').value;
        const act = new fromBpmnFileActions.UpdateBpmnFile(id, form.name, form.description);
        this.store.dispatch(act);
    }
}
