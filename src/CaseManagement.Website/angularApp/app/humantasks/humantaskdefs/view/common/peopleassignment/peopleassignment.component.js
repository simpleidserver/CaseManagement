var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
import { Component, Input, ViewEncapsulation } from '@angular/core';
import { FormBuilder, FormControl, Validators } from '@angular/forms';
import { PeopleAssignment } from '@app/stores/common/people-assignment.model';
var ParameterType = (function () {
    function ParameterType(type, displayName) {
        this.type = type;
        this.displayName = displayName;
    }
    return ParameterType;
}());
export { ParameterType };
var PeopleAssignmentComponent = (function () {
    function PeopleAssignmentComponent(formBuilder) {
        var _this = this;
        this.formBuilder = formBuilder;
        this._peopleAssignment = new PeopleAssignment();
        this.values = [];
        this.peopleAssignmentForm = this.formBuilder.group({
            type: new FormControl('', [
                Validators.required
            ]),
            value: new FormControl('', [
                Validators.required
            ]),
        });
        this.peopleAssignmentForm.get('type').valueChanges.subscribe(function () {
            _this.values = [];
        });
    }
    Object.defineProperty(PeopleAssignmentComponent.prototype, "peopleAssignment", {
        get: function () { return this._peopleAssignment; },
        set: function (pa) {
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
        },
        enumerable: false,
        configurable: true
    });
    PeopleAssignmentComponent.prototype.deleteValue = function (val) {
        var index = this.values.indexOf(val);
        this.values.splice(index, 1);
        this.update();
    };
    PeopleAssignmentComponent.prototype.addPeopleAssignment = function (form) {
        if (!this.peopleAssignmentForm.valid) {
            return;
        }
        this.peopleAssignmentForm.get('value').setValue('');
        this.values.push(form.value);
        this.update();
    };
    PeopleAssignmentComponent.prototype.update = function () {
        var type = this.peopleAssignmentForm.get('type').value;
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
    };
    __decorate([
        Input(),
        __metadata("design:type", PeopleAssignment),
        __metadata("design:paramtypes", [PeopleAssignment])
    ], PeopleAssignmentComponent.prototype, "peopleAssignment", null);
    PeopleAssignmentComponent = __decorate([
        Component({
            selector: 'peopleassignment-component',
            templateUrl: './peopleassignment.component.html',
            encapsulation: ViewEncapsulation.None
        }),
        __metadata("design:paramtypes", [FormBuilder])
    ], PeopleAssignmentComponent);
    return PeopleAssignmentComponent;
}());
export { PeopleAssignmentComponent };
//# sourceMappingURL=peopleassignment.component.js.map