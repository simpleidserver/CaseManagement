import { Component, Input, ViewEncapsulation } from "@angular/core";
import { FormBuilder, FormGroup, FormControl, Validators } from "@angular/forms";
import { Description } from "@app/stores/common/description.model";
import { PresentationParameter } from "@app/stores/common/presentationparameter.model";
import { TextDef } from "@app/stores/common/textdef.model";

export class Language {
    constructor(public code: string, public displayName: string) { }
}

export class ContentType {
    constructor(public code: string, public displayName: string) { }
}

export class Type {
    constructor(public code: string, public displayName: string) { }
}

@Component({
    selector: 'presentationparameter-component',
    templateUrl: './presentationparameter.component.html',
    encapsulation: ViewEncapsulation.None
})
export class PresentationParameterComponent {
    languages: Language[] = [
        new Language("fr", "French"),
        new Language("en", "English")
    ];
    contentTypes: ContentType[] = [
        new ContentType("text/html", "HTML")
    ];
    types: Type[] = [
        new Type("string", "String")
    ];
    addNameForm: FormGroup;
    addSubjectForm: FormGroup;
    addDescriptionForm: FormGroup;
    addPresentationForm: FormGroup;
    @Input() names: TextDef[];
    @Input() subjects: TextDef[];
    @Input() descriptions: Description[];
    @Input() presentationParameters: PresentationParameter[];

    constructor(
        private formBuilder: FormBuilder) {
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

    addName(txt: TextDef) {
        if (!this.addNameForm.valid) {
            return;
        }

        this.names.push(txt);
    }

    addSubject(sub: TextDef) {
        if (!this.addSubjectForm.valid) {
            return;
        }

        this.subjects.push(sub);
    }

    addDescription(desc: Description) {
        if (!this.addDescriptionForm.valid) {
            return;
        }

        this.descriptions.push(desc);
    }

    addPresentationParameter(pp: PresentationParameter) {
        if (!this.addPresentationForm.valid) {
            return;
        }

        this.presentationParameters.push(pp);
    }

    deleteName(txt: TextDef) {
        const index = this.names.indexOf(txt);
        this.names.splice(index, 1);
    }

    deleteSubject(sub: TextDef) {
        const index = this.subjects.indexOf(sub);
        this.subjects.splice(index, 1);
    }

    deleteDescription(desc: Description) {
        const index = this.descriptions.indexOf(desc);
        this.descriptions.splice(index, 1);
    }

    deletePrescriptionParameter(pp: PresentationParameter) {
        const index = this.presentationParameters.indexOf(pp);
        this.presentationParameters.splice(index, 1);
    }
}