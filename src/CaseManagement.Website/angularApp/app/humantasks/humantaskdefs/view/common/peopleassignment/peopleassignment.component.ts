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
    private _peopleAssignment: PeopleAssignment = new PeopleAssignment();
    peopleAssignmentForm: FormGroup;
    values: string[] = [];
    @Input()
    get peopleAssignment(): PeopleAssignment { return this._peopleAssignment; }
    set peopleAssignment(pa: PeopleAssignment) {
        this._peopleAssignment = pa;
        this.peopleAssignmentForm.get('type').setValue(pa.type);
        switch (pa.type) {
            case 'GROUPNAMES':
                this.values = pa.groupNames;
                break;
            case 'USERIDENTIFIERS':
                this.values = pa.userIdentifiers;
                break;
            case 'EXPRESSION':
                this.values = [pa.expression];
                break;
            case 'LOGICALPEOPLEGROUP':
                this.values = [pa.logicalPeopleGroup];
                break;
        }
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
        this._peopleAssignment.type = type;
        switch (type) {
            case 'GROUPNAMES':
                this._peopleAssignment.groupNames = this.values;
                break;
            case 'USERIDENTIFIERS':
                this._peopleAssignment.userIdentifiers = this.values;
                break;
            case 'EXPRESSION':
                this._peopleAssignment.expression = this.values[0];
                break;
            case 'LOGICALPEOPLEGROUP':
                this._peopleAssignment.logicalPeopleGroup = this.values[0];
                break;
        }
    }
}
