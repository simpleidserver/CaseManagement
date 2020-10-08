var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
import { Injectable } from '@angular/core';
import { of } from 'rxjs';
import { OutputRenderingElement, OutputRenderingElementValue, Translation, OptionValue } from '../../common/rendering.model';
import { HumanTaskDef } from '../models/humantaskdef.model';
var HumanTaskDefService = (function () {
    function HumanTaskDefService() {
    }
    HumanTaskDefService.prototype.get = function (humanTaskDefId) {
        console.log(humanTaskDefId);
        var record = new HumanTaskDef();
        record.name = "AddClient";
        var firstNameField = new OutputRenderingElement();
        var firstName = new Translation("fr", "Prenom");
        firstNameField.id = "firstName";
        firstNameField.label.push(firstName);
        firstNameField.default = "Firstname";
        firstNameField.value = new OutputRenderingElementValue();
        firstNameField.value.type = "string";
        var lastNameField = new OutputRenderingElement();
        var lastName = new Translation("fr", "Nom");
        lastNameField.id = "lastName";
        lastNameField.label.push(lastName);
        lastNameField.default = "Lastname";
        lastNameField.value = new OutputRenderingElementValue();
        lastNameField.value.type = "string";
        var gendersField = new OutputRenderingElement();
        var gender = new Translation("fr", "Genre");
        var maleFR = new Translation("fr", "M");
        var femaleFR = new Translation("fr", "F");
        var maleEN = new Translation("en", "M");
        var male = new OptionValue();
        var female = new OptionValue();
        male.value = "m";
        male.displayNames.push(maleFR);
        male.displayNames.push(maleEN);
        female.value = "f";
        female.displayNames.push(femaleFR);
        gendersField.id = "gender";
        gendersField.label.push(gender);
        gendersField.default = "Gender";
        gendersField.value = new OutputRenderingElementValue();
        gendersField.value.type = "select";
        gendersField.value.values.push(male);
        gendersField.value.values.push(female);
        record.rendering.output.push(firstNameField);
        record.rendering.output.push(lastNameField);
        record.rendering.output.push(gendersField);
        return of(record);
    };
    HumanTaskDefService.prototype.update = function (humanTaskDef) {
        return of(humanTaskDef);
    };
    HumanTaskDefService = __decorate([
        Injectable(),
        __metadata("design:paramtypes", [])
    ], HumanTaskDefService);
    return HumanTaskDefService;
}());
export { HumanTaskDefService };
//# sourceMappingURL=humantaskdef.service.js.map