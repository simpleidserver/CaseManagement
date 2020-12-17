import { CdkDragDrop, moveItemInArray } from '@angular/cdk/drag-drop';
import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material';
import * as fromAppState from '@app/stores/appstate';
import { OptionValue, RenderingElement, Translation } from '@app/stores/common/rendering.model';
import * as fromHumanTaskDefActions from '@app/stores/humantaskdefs/actions/humantaskdef.actions';
import { HumanTaskDef } from '@app/stores/humantaskdefs/models/humantaskdef.model';
import { ScannedActionsSubject, select, Store } from '@ngrx/store';
import { TranslateService } from '@ngx-translate/core';
import { filter } from 'rxjs/operators';

export class Language {
    constructor(public code: string, public display: string) { }
}

export class FieldType {
    constructor(public displayName: string, public fieldType: string) { }
}

@Component({
    selector: 'view-humantaskdef-rendering-component',
    templateUrl: './rendering.component.html',
    styleUrls: ['./rendering.component.scss'],
    encapsulation: ViewEncapsulation.None
})
export class ViewHumanTaskDefRenderingComponent implements OnInit {
    addLabelForm: FormGroup;
    addValueForm: FormGroup;
    id: string;
    selectedField: RenderingElement;
    renderingElements: RenderingElement[] = [];
    languages: Language[];
    fieldTypes: FieldType[];

    constructor(
        private store: Store<fromAppState.AppState>,
        private formBuilder: FormBuilder,
        private snackBar: MatSnackBar,
        private translateService: TranslateService,
        private actions$: ScannedActionsSubject) {
        this.addLabelForm = this.formBuilder.group({
            language: new FormControl('', [
                Validators.required
            ]),
            value: new FormControl('', [
                Validators.required
            ])
        });
        this.addValueForm = this.formBuilder.group({
            value: new FormControl('', [
                Validators.required
            ]),
            language: new FormControl('', [
                Validators.required
            ]),
            translation: new FormControl('', [
                Validators.required
            ])
        });
        this.languages = [];
        this.fieldTypes = [];
        const french = new Language("fr", "French");
        const english = new Language("en", "English");
        this.languages.push(french);
        this.languages.push(english);
        this.fieldTypes.push(new FieldType("SHARED.TEXT", "string"));
        this.fieldTypes.push(new FieldType("SHARED.SELECT", "select"));
    }

    ngOnInit(): void {
        this.actions$.pipe(
            filter((action: any) => action.type === fromHumanTaskDefActions.ActionTypes.COMPLETE_UPDATE_RENDERING_PARAMETER))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant('HUMANTASK.MESSAGES.RENDERING_UPDATED'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromHumanTaskDefActions.ActionTypes.ERROR_UPDATE_RENDERING_PARAMETER))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant('HUMANTASK.MESSAGES.ERROR_UPDATE_RENDERING'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });
        this.store.pipe(select(fromAppState.selectHumanTaskResult)).subscribe((e: HumanTaskDef) => {
            if (!e) {
                return;
            }

            this.id = e.id;
            this.renderingElements = e.renderingElements;
        });
    }

    drop(event: CdkDragDrop<string[]>) {
        moveItemInArray(this.renderingElements, event.previousIndex, event.currentIndex);
    }

    updateLabel(translation: Translation) {
        if (!this.selectedField) {
            return;
        }

        const filteredLabels = this.selectedField.labels.filter(function (l: Translation) {
            return l.language === translation.language;
        });
        if (filteredLabels.length === 1) {
            this.snackBar.open(this.translateService.instant('HUMANTASK.MESSAGE.TRANSLATION_EXISTS'), this.translateService.instant('undo'), {
                duration: 2000
            });
            return;
        }

        this.selectedField.labels.push(translation);
        this.addLabelForm.reset();
    }

    addValue(form : any) {
        if (!this.selectedField || this.selectedField.valueType !== 'select') {
            return;
        }

        const filteredValue = this.selectedField.values.filter(function (f: OptionValue) {
            return f.value === form.value && f.displayNames.filter(function (d: Translation) {
                return d.language === form.language;
            }).length > 0;
        });
        if (filteredValue.length === 1) {
            this.snackBar.open(this.translateService.instant('SHARED.VALUE_EXISTS'), this.translateService.instant('undo'), {
                duration: 2000
            });
            return;
        }

        const optValue = new OptionValue();
        optValue.value = form.value;
        optValue.displayNames.push(new Translation(form.language, form.translation));
        this.selectedField.values.push(optValue);
        this.addValueForm.reset();
    }

    addField(fieldType: FieldType) {
        const renderingElt = new RenderingElement();
        renderingElt.valueType = fieldType.fieldType;
        this.renderingElements.push(renderingElt);
    }

    removeField(elt: RenderingElement) {
        const index = this.renderingElements.indexOf(elt);
        this.renderingElements.splice(index, 1);
        this.selectedField = null;
    }

    displaySettings(elt: RenderingElement) {
        this.selectedField = elt;
    }

    deleteValue(val: OptionValue) {
        if (!this.selectedField) {
            return;
        }

        const index = this.selectedField.values.indexOf(val);
        this.selectedField.values.splice(index, 1);
    }

    deleteLabel(lbl: Translation) {
        if (!this.selectedField) {
            return;
        }

        const index = this.selectedField.labels.indexOf(lbl);
        this.selectedField.labels.splice(index, 1);
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

    update() {
        const request = new fromHumanTaskDefActions.UpdateRenderingOperation(this.id ,this.renderingElements);
        this.store.dispatch(request);
    }
}
