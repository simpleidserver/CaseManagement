import { Component, Input, ViewEncapsulation } from '@angular/core';
import { FormBuilder, FormGroup, FormControl, Validators } from '@angular/forms';
import { PeopleAssignment } from '@app/stores/common/people-assignment.model';

export class ParameterType {
    constructor(public type: string, public displayName: string) { }
}

@Component({
    selector: 'peopleassignment-component',
    templateUrl: './peopleassignment.component.html',
    encapsulation: ViewEncapsulation.None
})
export class PeopleAssignmentComponent {
    _peopleAssignments: PeopleAssignment[]; 
    peopleAssignmentForm: FormGroup;
    @Input() usage: string = '';
    @Input()
    get peopleAssignments() {
        return this._peopleAssignments;
    }
    set peopleAssignments(v: PeopleAssignment[]) {
        if (!v) {
            return;
        }

        
        if (v.length > 0) {
            this.peopleAssignmentForm.get('type').setValue(v[0].type);
        }

        this._peopleAssignments = v;
    }

    constructor(
        private formBuilder: FormBuilder) {
        this.peopleAssignmentForm = this.formBuilder.group({
            type: new FormControl('', [
                Validators.required
            ]),
            value: new FormControl('', [
                Validators.required
            ]),
        });
        this.peopleAssignmentForm.get('type').valueChanges.subscribe(() => {
            if (!this._peopleAssignments) {
                return;
            }

            this._peopleAssignments.splice(0, this._peopleAssignments.length);
        });
    }

    deleteValue(val: PeopleAssignment) {
        const index = this._peopleAssignments.indexOf(val);
        this._peopleAssignments.splice(index, 1);
    }

    addPeopleAssignment(form: any) {
        if (!this.peopleAssignmentForm.valid) {
            return;
        }

        const pa = new PeopleAssignment();
        const type = this.peopleAssignmentForm.get('type').value;
        pa.value = form.value;
        pa.type = type;
        pa.usage = this.usage;
        this._peopleAssignments.push(pa);
        this.peopleAssignmentForm.get('value').setValue('');
    }
}
