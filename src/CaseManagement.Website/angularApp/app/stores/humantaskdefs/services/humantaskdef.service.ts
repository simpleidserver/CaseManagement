import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { OutputRenderingElement, OutputRenderingElementValue, Translation, OptionValue } from '../../common/rendering.model';
import { HumanTaskDef } from '../models/humantaskdef.model';
import { Parameter } from '../../common/operation.model';

@Injectable()
export class HumanTaskDefService {
    constructor() { }

    get(humanTaskDefId: string): Observable<HumanTaskDef> {
        console.log(humanTaskDefId);
        const record = new HumanTaskDef();
        record.name = "AddClient";

        const inputParameter = new Parameter();
        inputParameter.isRequired = true;
        inputParameter.name = "firstname";
        inputParameter.type = "string";
        record.operation.inputParameters.push(inputParameter);

        const outputParameter = new Parameter();
        outputParameter.isRequired = false;
        outputParameter.name = "wage";
        outputParameter.type = "bool";
        record.operation.outputParameters.push(outputParameter);

        const firstNameField = new OutputRenderingElement();
        const firstName = new Translation("fr", "Prenom");
        firstNameField.id = "firstName";
        firstNameField.label.push(firstName);
        firstNameField.default = "Firstname";
        firstNameField.value = new OutputRenderingElementValue();
        firstNameField.value.type = "string";

        const lastNameField = new OutputRenderingElement();
        const lastName = new Translation("fr", "Nom");
        lastNameField.id = "lastName";
        lastNameField.label.push(lastName);
        lastNameField.default = "Lastname";
        lastNameField.value = new OutputRenderingElementValue();
        lastNameField.value.type = "string";

        const gendersField = new OutputRenderingElement();
        const gender = new Translation("fr", "Genre");
        const maleFR = new Translation("fr", "M");
        const femaleFR = new Translation("fr", "F");
        const maleEN = new Translation("en", "M");
        const male = new OptionValue();
        const female = new OptionValue();
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
    }

    update(humanTaskDef: HumanTaskDef): Observable<HumanTaskDef> {
        return of(humanTaskDef);
    }
}