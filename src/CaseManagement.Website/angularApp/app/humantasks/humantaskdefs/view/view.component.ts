import { CdkDragDrop, moveItemInArray } from '@angular/cdk/drag-drop';
import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatSnackBar } from '@angular/material';
import { ActivatedRoute } from '@angular/router';
import * as fromAppState from '@app/stores/appstate';
import { OptionValue, OutputRenderingElement, Translation } from '@app/stores/common/rendering.model';
import * as fromHumanTaskDefActions from '@app/stores/humantaskdefs/actions/humantaskdef.actions';
import { HumanTaskDef } from '@app/stores/humantaskdefs/models/humantaskdef.model';
import { select, Store } from '@ngrx/store';
import { TranslateService } from '@ngx-translate/core';

export class Language {
    constructor(public code: string, public display: string) { }
}

export class FieldType {
    constructor(public displayName: string, public fieldType: string) { }
}

@Component({
    selector: 'view-humantaskdef-component',
    templateUrl: './view.component.html',
    styleUrls: ['./view.component.scss'],
    encapsulation: ViewEncapsulation.None
})
export class ViewHumanTaskDef implements OnInit {
    baseTranslationKey: string = "HUMANTASK.DEF.VIEW";
    humanTaskDef: HumanTaskDef = new HumanTaskDef();
    addLabelForm: FormGroup;
    addValueForm: FormGroup;
    selectedField: OutputRenderingElement;
    languages: Language[];
    fieldTypes: FieldType[];

    constructor(
        private store: Store<fromAppState.AppState>,
        private route: ActivatedRoute,
        private formBuilder: FormBuilder,
        private snackBar: MatSnackBar,
        private translateService: TranslateService) {
        this.addLabelForm = this.formBuilder.group({
            language: '',
            value: ''
        });
        this.addValueForm = this.formBuilder.group({
            value: '',
            language: '',
            translation: ''
        });
        this.languages = [];
        this.fieldTypes = [];
        const french = new Language("fr", "French");
        const english = new Language("en", "English");
        this.languages.push(french);
        this.languages.push(english);
        this.fieldTypes.push(new FieldType(this.baseTranslationKey + ".TEXT", "string"));
        this.fieldTypes.push(new FieldType(this.baseTranslationKey + ".SELECT", "select"));
    }

    ngOnInit(): void {
        this.store.pipe(select(fromAppState.selectHumanTaskResult)).subscribe((e: HumanTaskDef) => {
            if (!e) {
                return;
            }

            this.humanTaskDef = e;
        });

        this.refresh();
    }

    drop(event: CdkDragDrop<string[]>) {
        moveItemInArray(this.humanTaskDef.rendering.output, event.previousIndex, event.currentIndex);
    }

    updateLabel(translation: Translation) {
        if (!this.selectedField) {
            return;
        }

        const filteredLabels = this.selectedField.label.filter(function (l: Translation) {
            return l.language === translation.language;
        });
        if (filteredLabels.length === 1) {
            this.snackBar.open(this.translateService.instant(this.baseTranslationKey + '.TRANSLATION_EXISTS'), this.translateService.instant('undo'), {
                duration: 2000
            });
            return;
        }

        this.selectedField.label.push(translation);
        this.addLabelForm.reset();
    }

    addValue(form : any) {
        if (!this.selectedField || this.selectedField.value.type === 'select') {
            return;
        }

        const filteredValue = this.selectedField.value.values.filter(function (f: OptionValue) {
            return f.value === form.value && f.displayNames.filter(function (d: Translation) {
                return d.language === form.language && d.value === form.translation;
            }).length > 0;
        });
        if (filteredValue.length === 1) {
            this.snackBar.open(this.translateService.instant(this.baseTranslationKey + '.VALUE_EXISTS'), this.translateService.instant('undo'), {
                duration: 2000
            });
            return;
        }

        const optValue = new OptionValue();
        optValue.value = form.value;
        optValue.displayNames.push(new Translation(form.language, form.translation));
        this.selectedField.value.values.push(optValue);
        this.addValueForm.reset();
    }

    addField(fieldType: FieldType) {
        const renderingElt = new OutputRenderingElement();
        renderingElt.value.type = fieldType.fieldType;
        this.humanTaskDef.rendering.output.push(renderingElt);
    }

    removeField(elt: OutputRenderingElement) {
        const index = this.humanTaskDef.rendering.output.indexOf(elt);
        this.humanTaskDef.rendering.output.splice(index, 1);
    }

    displaySettings(elt: OutputRenderingElement) {
        this.selectedField = elt;
    }

    refresh() {
        const id = this.route.snapshot.params['id'];
        const request = new fromHumanTaskDefActions.GetHumanTaskDef(id);
        this.store.dispatch(request);
    }

    deleteValue(val: OptionValue) {
        if (!this.selectedField) {
            return;
        }

        const index = this.selectedField.value.values.indexOf(val);
        this.selectedField.value.values.splice(index, 1);
    }

    deleteLabel(lbl: Translation) {
        if (!this.selectedField) {
            return;
        }

        const index = this.selectedField.label.indexOf(lbl);
        this.selectedField.label.splice(index, 1);
    }

    joinDisplayNames(val: OptionValue) {
        const map = new Map();
        val.displayNames.forEach((item) => {
            const key = item.language;
            const collection = map.get(key);
            if (!collection) {
                map.set(key, [item]);
            } else {
                collection.push(item);
            }
        });

        var arr : any = [];
        map.forEach(function (value, key) {
            arr.push(key + "="+value[0].value);
        });

        return val.value + "("+arr.join(',')+")";
    }
}
