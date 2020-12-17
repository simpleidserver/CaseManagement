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
        this.usage = '';
        this.peopleAssignmentForm = this.formBuilder.group({
            type: new FormControl('', [
                Validators.required
            ]),
            value: new FormControl('', [
                Validators.required
            ]),
        });
        this.peopleAssignmentForm.get('type').valueChanges.subscribe(function () {
            if (!_this._peopleAssignments) {
                return;
            }
            _this._peopleAssignments.splice(0, _this._peopleAssignments.length);
        });
    }
    Object.defineProperty(PeopleAssignmentComponent.prototype, "peopleAssignments", {
        get: function () {
            return this._peopleAssignments;
        },
        set: function (v) {
            if (!v) {
                return;
            }
            if (v.length > 0) {
                this.peopleAssignmentForm.get('type').setValue(v[0].type);
            }
            this._peopleAssignments = v;
        },
        enumerable: false,
        configurable: true
    });
    PeopleAssignmentComponent.prototype.deleteValue = function (val) {
        var index = this._peopleAssignments.indexOf(val);
        this._peopleAssignments.splice(index, 1);
    };
    PeopleAssignmentComponent.prototype.addPeopleAssignment = function (form) {
        if (!this.peopleAssignmentForm.valid) {
            return;
        }
        var pa = new PeopleAssignment();
        var type = this.peopleAssignmentForm.get('type').value;
        pa.value = form.value;
        pa.type = type;
        pa.usage = this.usage;
        this._peopleAssignments.push(pa);
        this.peopleAssignmentForm.get('value').setValue('');
    };
    __decorate([
        Input(),
        __metadata("design:type", String)
    ], PeopleAssignmentComponent.prototype, "usage", void 0);
    __decorate([
        Input(),
        __metadata("design:type", Array),
        __metadata("design:paramtypes", [Array])
    ], PeopleAssignmentComponent.prototype, "peopleAssignments", null);
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