import { Component, Input, ViewEncapsulation } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
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
        switch (pa.type) {
            case 'groupnames':
                this.values = pa.groupNames;
                break;
            case 'useridentifiers':
                this.values = pa.userIdentifiers;
                break;
            case 'expression':
                this.values = [pa.expression];
                break;
            case 'logicalpeoplegroup':
                this.values = [pa.logicalPeopleGroup];
                break;
        }
    }

    constructor(
        private formBuilder: FormBuilder) {
        this.peopleAssignmentForm = this.formBuilder.group({
            type: '',
            value: ''
        });
    }

    deleteValue(val: string) {
        const index = this.values.indexOf(val);
        this.values.splice(index, 1);
        this.update();
    }

    addPeopleAssignment(form: any) {
        this.peopleAssignmentForm.get('value').setValue('');
        this.values.push(form.value);
        this.update();
    }

    private update() {
        const type = this.peopleAssignmentForm.get('type').value;
        this._peopleAssignment.type = type;
        switch (type) {
            case 'groupnames':
                this._peopleAssignment.groupNames = this.values;
                break;
            case 'useridentifiers':
                this._peopleAssignment.userIdentifiers = this.values;
                break;
            case 'expression':
                this._peopleAssignment.expression = this.values[0];
                break;
            case 'logicalpeoplegroup':
                this._peopleAssignment.logicalPeopleGroup = this.values[0];
                break;
        }
    }
}
