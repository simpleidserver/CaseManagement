var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
import { Component, Input, ViewEncapsulation } from "@angular/core";
import { FormBuilder, FormControl, Validators } from "@angular/forms";
var Language = (function () {
    function Language(code, displayName) {
        this.code = code;
        this.displayName = displayName;
    }
    return Language;
}());
export { Language };
var ContentType = (function () {
    function ContentType(code, displayName) {
        this.code = code;
        this.displayName = displayName;
    }
    return ContentType;
}());
export { ContentType };
var Type = (function () {
    function Type(code, displayName) {
        this.code = code;
        this.displayName = displayName;
    }
    return Type;
}());
export { Type };
var PresentationParameterComponent = (function () {
    function PresentationParameterComponent(formBuilder) {
        this.formBuilder = formBuilder;
        this.languages = [
            new Language("fr", "French"),
            new Language("en", "English")
        ];
        this.contentTypes = [
            new ContentType("text/html", "HTML")
        ];
        this.types = [
            new Type("string", "String")
        ];
        this.addNameForm = this.formBuilder.group({
            language: new FormControl('', [
                Validators.required
            ]),
            value: new FormControl('', [
                Validators.required
            ])
        });
        this.addSubjectForm = this.formBuilder.group({
            language: new FormControl('', [
                Validators.required
            ]),
            value: new FormControl('', [
                Validators.required
            ])
        });
        this.addDescriptionForm = this.formBuilder.group({
            language: new FormControl('', [
                Validators.required
            ]),
            value: new FormControl('', [
                Validators.required
            ]),
            contentType: new FormControl('', [
                Validators.required
            ])
        });
        this.addPresentationForm = this.formBuilder.group({
            name: new FormControl('', [
                Validators.required
            ]),
            type: new FormControl('', [
                Validators.required
            ]),
            expression: new FormControl('', [
                Validators.required
            ])
        });
    }
    PresentationParameterComponent.prototype.addName = function (txt) {
        if (!this.addNameForm.valid) {
            return;
        }
        this.names.push(txt);
    };
    PresentationParameterComponent.prototype.addSubject = function (sub) {
        if (!this.addSubjectForm.valid) {
            return;
        }
        this.subjects.push(sub);
    };
    PresentationParameterComponent.prototype.addDescription = function (desc) {
        if (!this.addDescriptionForm.valid) {
            return;
        }
        this.descriptions.push(desc);
    };
    PresentationParameterComponent.prototype.addPresentationParameter = function (pp) {
        if (!this.addPresentationForm.valid) {
            return;
        }
        this.presentationParameters.push(pp);
    };
    PresentationParameterComponent.prototype.deleteName = function (txt) {
        var index = this.names.indexOf(txt);
        this.names.splice(index, 1);
    };
    PresentationParameterComponent.prototype.deleteSubject = function (sub) {
        var index = this.subjects.indexOf(sub);
        this.subjects.splice(index, 1);
    };
    PresentationParameterComponent.prototype.deleteDescription = function (desc) {
        var index = this.descriptions.indexOf(desc);
        this.descriptions.splice(index, 1);
    };
    PresentationParameterComponent.prototype.deletePrescriptionParameter = function (pp) {
        var index = this.presentationParameters.indexOf(pp);
        this.presentationParameters.splice(index, 1);
    };
    __decorate([
        Input(),
        __metadata("design:type", Array)
    ], PresentationParameterComponent.prototype, "names", void 0);
    __decorate([
        Input(),
        __metadata("design:type", Array)
    ], PresentationParameterComponent.prototype, "subjects", void 0);
    __decorate([
        Input(),
        __metadata("design:type", Array)
    ], PresentationParameterComponent.prototype, "descriptions", void 0);
    __decorate([
        Input(),
        __metadata("design:type", Array)
    ], PresentationParameterComponent.prototype, "presentationParameters", void 0);
    PresentationParameterComponent = __decorate([
        Component({
            selector: 'presentationparameter-component',
            templateUrl: './presentationparameter.component.html',
            encapsulation: ViewEncapsulation.None
        }),
        __metadata("design:paramtypes", [FormBuilder])
    ], PresentationParameterComponent);
    return PresentationParameterComponent;
}());
export { PresentationParameterComponent };
//# sourceMappingURL=presentationparameter.component.js.map