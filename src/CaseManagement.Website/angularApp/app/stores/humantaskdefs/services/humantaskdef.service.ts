import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { HumanTaskDef } from '../models/humantaskdef.model';
import { OutputRenderingElement, OutputRenderingElementValue } from '../../common/rendering.model';

@Injectable()
export class HumanTaskDefService {
    constructor() { }

    get(humanTaskDefId: string): Observable<HumanTaskDef> {
        console.log(humanTaskDefId);
        const record = new HumanTaskDef();
        record.name = "AddClient";

        const firstNameField = new OutputRenderingElement();
        firstNameField.id = "firstName";
        firstNameField.label = "Firstname";
        firstNameField.default = "Firstname";
        firstNameField.value = new OutputRenderingElementValue();
        firstNameField.value.type = "string";

        const lastNameField = new OutputRenderingElement();
        lastNameField.id = "lastName";
        lastNameField.label = "Lastname";
        lastNameField.default = "Lastname";
        lastNameField.value = new OutputRenderingElementValue();
        lastNameField.value.type = "string";

        const gendersField = new OutputRenderingElement();
        gendersField.id = "gender";
        gendersField.label = "Gender";
        gendersField.default = "Gender";
        gendersField.value = new OutputRenderingElementValue();
        gendersField.value.type = "select";
        gendersField.value.values = ["Male", "Female"];
        

        record.rendering.output.push(firstNameField);
        record.rendering.output.push(lastNameField);
        record.rendering.output.push(gendersField);
        return of(record);
    }
}