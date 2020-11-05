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
    peopleAssignmentForm: FormGroup;
    values: string[] = [];
    @Input() peopleAssignments: PeopleAssignment[] = [];

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
            this.values = [];
        });
    }

    deleteValue(val: string) {
        const index = this.values.indexOf(val);
        this.values.splice(index, 1);
        this.update();
    }

    addPeopleAssignment(form: any) {
        if (!this.peopleAssignmentForm.valid) {
            return;
        }

        this.peopleAssignmentForm.get('value').setValue('');
        this.values.push(form.value);
        this.update();
    }

    private update() {
        const type = this.peopleAssignmentForm.get('type').value;
        const self = this;
        this.values.forEach(function (v: string) {
            const pa = new PeopleAssignment();
            pa.type = type;
            pa.value = v;
            self.peopleAssignments.push(pa);
        });
    }
}
