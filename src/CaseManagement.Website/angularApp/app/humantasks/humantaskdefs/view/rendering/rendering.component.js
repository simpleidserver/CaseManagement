var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
import { moveItemInArray } from '@angular/cdk/drag-drop';
import { Component, ViewEncapsulation } from '@angular/core';
import { FormBuilder, FormControl, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material';
import * as fromAppState from '@app/stores/appstate';
import { OptionValue, OutputRenderingElement, Rendering, Translation } from '@app/stores/common/rendering.model';
import * as fromHumanTaskDefActions from '@app/stores/humantaskdefs/actions/humantaskdef.actions';
import { select, Store, ScannedActionsSubject } from '@ngrx/store';
import { TranslateService } from '@ngx-translate/core';
import { filter } from 'rxjs/operators';
var Language = (function () {
    function Language(code, display) {
        this.code = code;
        this.display = display;
    }
    return Language;
}());
export { Language };
var FieldType = (function () {
    function FieldType(displayName, fieldType) {
        this.displayName = displayName;
        this.fieldType = fieldType;
    }
    return FieldType;
}());
export { FieldType };
var ViewHumanTaskDefRenderingComponent = (function () {
    function ViewHumanTaskDefRenderingComponent(store, formBuilder, snackBar, translateService, actions$) {
        this.store = store;
        this.formBuilder = formBuilder;
        this.snackBar = snackBar;
        this.translateService = translateService;
        this.actions$ = actions$;
        this.baseTranslationKey = "HUMANTASK.DEF.VIEW.RENDERING";
        this.rendering = new Rendering();
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
        var french = new Language("fr", "French");
        var english = new Language("en", "English");
        this.languages.push(french);
        this.languages.push(english);
        this.fieldTypes.push(new FieldType(this.baseTranslationKey + ".TEXT", "string"));
        this.fieldTypes.push(new FieldType(this.baseTranslationKey + ".SELECT", "select"));
    }
    ViewHumanTaskDefRenderingComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.actions$.pipe(filter(function (action) { return action.type === fromHumanTaskDefActions.ActionTypes.COMPLETE_UPDATE_RENDERING_PARAMETER; }))
            .subscribe(function () {
            _this.snackBar.open(_this.translateService.instant(_this.baseTranslationKey + '.RENDERING_UPDATED'), _this.translateService.instant('undo'), {
                duration: 2000
            });
        });
        this.actions$.pipe(filter(function (action) { return action.type === fromHumanTaskDefActions.ActionTypes.ERROR_UPDATE_RENDERING_PARAMETER; }))
            .subscribe(function () {
            _this.snackBar.open(_this.translateService.instant(_this.baseTranslationKey + '.ERROR_UPDATE_RENDERING'), _this.translateService.instant('undo'), {
                duration: 2000
            });
        });
        this.store.pipe(select(fromAppState.selectHumanTaskResult)).subscribe(function (e) {
            if (!e) {
                return;
            }
            _this.id = e.id;
            _this.rendering = e.rendering;
        });
    };
    ViewHumanTaskDefRenderingComponent.prototype.drop = function (event) {
        moveItemInArray(this.rendering.output, event.previousIndex, event.currentIndex);
    };
    ViewHumanTaskDefRenderingComponent.prototype.updateLabel = function (translation) {
        if (!this.selectedField) {
            return;
        }
        var filteredLabels = this.selectedField.label.filter(function (l) {
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
    };
    ViewHumanTaskDefRenderingComponent.prototype.addValue = function (form) {
        if (!this.selectedField || this.selectedField.value.type !== 'select') {
            return;
        }
        var filteredValue = this.selectedField.value.values.filter(function (f) {
            return f.value === form.value && f.displayNames.filter(function (d) {
                return d.language === form.language;
            }).length > 0;
        });
        if (filteredValue.length === 1) {
            this.snackBar.open(this.translateService.instant(this.baseTranslationKey + '.VALUE_EXISTS'), this.translateService.instant('undo'), {
                duration: 2000
            });
            return;
        }
        var optValue = new OptionValue();
        optValue.value = form.value;
        optValue.displayNames.push(new Translation(form.language, form.translation));
        this.selectedField.value.values.push(optValue);
        this.addValueForm.reset();
    };
    ViewHumanTaskDefRenderingComponent.prototype.addField = function (fieldType) {
        var renderingElt = new OutputRenderingElement();
        renderingElt.value.type = fieldType.fieldType;
        this.rendering.output.push(renderingElt);
    };
    ViewHumanTaskDefRenderingComponent.prototype.removeField = function (elt) {
        var index = this.rendering.output.indexOf(elt);
        this.rendering.output.splice(index, 1);
        this.selectedField = null;
    };
    ViewHumanTaskDefRenderingComponent.prototype.displaySettings = function (elt) {
        this.selectedField = elt;
    };
    ViewHumanTaskDefRenderingComponent.prototype.deleteValue = function (val) {
        if (!this.selectedField) {
            return;
        }
        var index = this.selectedField.value.values.indexOf(val);
        this.selectedField.value.values.splice(index, 1);
    };
    ViewHumanTaskDefRenderingComponent.prototype.deleteLabel = function (lbl) {
        if (!this.selectedField) {
            return;
        }
        var index = this.selectedField.label.indexOf(lbl);
        this.selectedField.label.splice(index, 1);
    };
    ViewHumanTaskDefRenderingComponent.prototype.joinDisplayNames = function (val) {
        var map = new Map();
        val.displayNames.forEach(function (item) {
            var key = item.language;
            var collection = map.get(key);
            if (!collection) {
                map.set(key, [item]);
            }
            else {
                collection.push(item);
            }
        });
        var arr = [];
        map.forEach(function (value, key) {
            arr.push(key + "=" + value[0].value);
        });
        return val.value + "(" + arr.join(',') + ")";
    };
    ViewHumanTaskDefRenderingComponent.prototype.update = function () {
        var request = new fromHumanTaskDefActions.UpdateRenderingOperation(this.id, this.rendering);
        this.store.dispatch(request);
    };
    ViewHumanTaskDefRenderingComponent = __decorate([
        Component({
            selector: 'view-humantaskdef-rendering-component',
            templateUrl: './rendering.component.html',
            styleUrls: ['./rendering.component.scss'],
            encapsulation: ViewEncapsulation.None
        }),
        __metadata("design:paramtypes", [Store,
            FormBuilder,
            MatSnackBar,
            TranslateService,
            ScannedActionsSubject])
    ], ViewHumanTaskDefRenderingComponent);
    return ViewHumanTaskDefRenderingComponent;
}());
export { ViewHumanTaskDefRenderingComponent };
//# sourceMappingURL=rendering.component.js.map