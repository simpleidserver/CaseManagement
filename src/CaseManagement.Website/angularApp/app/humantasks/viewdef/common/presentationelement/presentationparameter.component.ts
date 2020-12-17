import { Component, Input, ViewEncapsulation } from "@angular/core";
import { FormBuilder, FormControl, FormGroup, Validators } from "@angular/forms";
import { PresentationParameter } from "@app/stores/common/presentationparameter.model";
import { PresentationElement } from "@app/stores/common/presentationelement.model";

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
    @Input() names: PresentationElement[];
    @Input() subjects: PresentationElement[];
    @Input() descriptions: PresentationElement[];
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

    addName(txt: PresentationElement) {
        if (!this.addNameForm.valid) {
            return;
        }

        txt.usage = 'NAME';
        this.names.push(txt);
        this.addNameForm.get('value').setValue('');
    }

    addSubject(sub: PresentationElement) {
        if (!this.addSubjectForm.valid) {
            return;
        }

        sub.usage = 'SUBJECT';
        this.subjects.push(sub);
        this.addSubjectForm.get('value').setValue('');
    }

    addDescription(desc: PresentationElement) {
        if (!this.addDescriptionForm.valid) {
            return;
        }

        desc.usage = 'DESCRIPTION';
        this.descriptions.push(desc);
        this.addDescriptionForm.get('value').setValue('');
    }

    addPresentationParameter(pp: PresentationParameter) {
        if (!this.addPresentationForm.valid) {
            return;
        }

        this.presentationParameters.push(pp);
        this.addPresentationForm.reset();
    }

    deleteName(txt: PresentationElement) {
        const index = this.names.indexOf(txt);
        this.names.splice(index, 1);
    }

    deleteSubject(sub: PresentationElement) {
        const index = this.subjects.indexOf(sub);
        this.subjects.splice(index, 1);
    }

    deleteDescription(desc: PresentationElement) {
        const index = this.descriptions.indexOf(desc);
        this.descriptions.splice(index, 1);
    }

    deletePrescriptionParameter(pp: PresentationParameter) {
        const index = this.presentationParameters.indexOf(pp);
        this.presentationParameters.splice(index, 1);
    }
}