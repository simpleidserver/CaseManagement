import { Component, Input, ViewEncapsulation } from "@angular/core";
import { FormBuilder, FormGroup } from "@angular/forms";
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
            language: '',
            value: ''
        });
        this.addSubjectForm = this.formBuilder.group({
            language: '',
            value: ''
        });
        this.addDescriptionForm = this.formBuilder.group({
            language: '',
            value: '',
            contentType: ''
        });
        this.addPresentationForm = this.formBuilder.group({
            name: '',
            type: '',
            expression: ''
        });
    }

    addName(txt: TextDef) {
        this.names.push(txt);
    }

    addSubject(sub: TextDef) {
        this.subjects.push(sub);
    }

    addDescription(desc: Description) {
        this.descriptions.push(desc);
    }

    addPresentationParameter(pp: PresentationParameter) {
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