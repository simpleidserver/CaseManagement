var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
var __param = (this && this.__param) || function (paramIndex, decorator) {
    return function (target, key) { decorator(target, key, paramIndex); }
};
import { moveItemInArray } from '@angular/cdk/drag-drop';
import { Component, ViewChild, ViewEncapsulation, Inject } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialog, MatSnackBar, MAT_DIALOG_DATA, MatDialogRef } from '@angular/material';
import * as fromAppState from '@app/stores/appstate';
import { OptionValue, RenderingElement, Translation } from '@app/stores/common/rendering.model';
import * as fromHumanTaskDefActions from '@app/stores/humantaskdefs/actions/humantaskdef.actions';
import { ScannedActionsSubject, select, Store } from '@ngrx/store';
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
    function ViewHumanTaskDefRenderingComponent(store, formBuilder, snackBar, translateService, actions$, dialog) {
        this.store = store;
        this.formBuilder = formBuilder;
        this.snackBar = snackBar;
        this.translateService = translateService;
        this.actions$ = actions$;
        this.dialog = dialog;
        this.renderingElements = [];
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
        this.fieldTypes.push(new FieldType("SHARED.TEXT", "string"));
        this.fieldTypes.push(new FieldType("SHARED.SELECT", "select"));
    }
    ViewHumanTaskDefRenderingComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.actions$.pipe(filter(function (action) { return action.type === fromHumanTaskDefActions.ActionTypes.COMPLETE_UPDATE_RENDERING_PARAMETER; }))
            .subscribe(function () {
            _this.snackBar.open(_this.translateService.instant('HUMANTASK.MESSAGES.RENDERING_UPDATED'), _this.translateService.instant('undo'), {
                duration: 2000
            });
        });
        this.actions$.pipe(filter(function (action) { return action.type === fromHumanTaskDefActions.ActionTypes.ERROR_UPDATE_RENDERING_PARAMETER; }))
            .subscribe(function () {
            _this.snackBar.open(_this.translateService.instant('HUMANTASK.MESSAGES.ERROR_UPDATE_RENDERING'), _this.translateService.instant('undo'), {
                duration: 2000
            });
        });
        this.store.pipe(select(fromAppState.selectHumanTaskResult)).subscribe(function (e) {
            if (!e) {
                return;
            }
            _this.id = e.id;
            _this.renderingElements = e.renderingElements;
        });
    };
    ViewHumanTaskDefRenderingComponent.prototype.drop = function (event) {
        moveItemInArray(this.renderingElements, event.previousIndex, event.currentIndex);
    };
    ViewHumanTaskDefRenderingComponent.prototype.updateLabel = function (translation) {
        if (!this.selectedField) {
            return;
        }
        var filteredLabels = this.selectedField.labels.filter(function (l) {
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
    };
    ViewHumanTaskDefRenderingComponent.prototype.addValue = function (form) {
        if (!this.selectedField || this.selectedField.valueType !== 'select') {
            return;
        }
        var filteredValue = this.selectedField.values.filter(function (f) {
            return f.value === form.value && f.displayNames.filter(function (d) {
                return d.language === form.language;
            }).length > 0;
        });
        if (filteredValue.length === 1) {
            this.snackBar.open(this.translateService.instant('SHARED.VALUE_EXISTS'), this.translateService.instant('undo'), {
                duration: 2000
            });
            return;
        }
        var optValue = new OptionValue();
        optValue.value = form.value;
        optValue.displayNames.push(new Translation(form.language, form.translation));
        this.selectedField.values.push(optValue);
        this.addValueForm.reset();
    };
    ViewHumanTaskDefRenderingComponent.prototype.addField = function (fieldType) {
        var renderingElt = new RenderingElement();
        renderingElt.valueType = fieldType.fieldType;
        this.renderingElements.push(renderingElt);
    };
    ViewHumanTaskDefRenderingComponent.prototype.removeField = function (elt) {
        var index = this.renderingElements.indexOf(elt);
        this.renderingElements.splice(index, 1);
        this.selectedField = null;
    };
    ViewHumanTaskDefRenderingComponent.prototype.displaySettings = function (elt) {
        this.selectedField = elt;
    };
    ViewHumanTaskDefRenderingComponent.prototype.deleteValue = function (val) {
        if (!this.selectedField) {
            return;
        }
        var index = this.selectedField.values.indexOf(val);
        this.selectedField.values.splice(index, 1);
    };
    ViewHumanTaskDefRenderingComponent.prototype.deleteLabel = function (lbl) {
        if (!this.selectedField) {
            return;
        }
        var index = this.selectedField.labels.indexOf(lbl);
        this.selectedField.labels.splice(index, 1);
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
        var request = new fromHumanTaskDefActions.UpdateRenderingOperation(this.id, this.renderingElements);
        this.store.dispatch(request);
    };
    ViewHumanTaskDefRenderingComponent.prototype.dragColumn = function (evt) {
        var json = {
            type: 'row',
            children: [
                { type: 'column', width: '50%' },
                { type: 'column', width: '50%' }
            ]
        };
        evt.dataTransfer.setData('json', JSON.stringify(json));
    };
    ViewHumanTaskDefRenderingComponent.prototype.dragTxt = function (evt) {
        var json = {
            type: 'txt',
            label: 'Label',
            name: 'name'
        };
        evt.dataTransfer.setData('json', JSON.stringify(json));
    };
    ViewHumanTaskDefRenderingComponent.prototype.dragSelect = function (evt) {
        var json = {
            type: 'select',
            label: 'Label',
            name: 'name'
        };
        evt.dataTransfer.set('json', JSON.stringify(json));
    };
    ViewHumanTaskDefRenderingComponent.prototype.dragOver = function (evt) {
        evt.preventDefault();
    };
    ViewHumanTaskDefRenderingComponent.prototype.dropColumn = function (evt) {
        var json = JSON.parse(evt.dataTransfer.getData('json'));
        var node = BaseComponent.buildNode(this.dialog, json);
        evt.target.appendChild(node);
    };
    ViewHumanTaskDefRenderingComponent.prototype.getJson = function (elt) {
        var self = this;
        var json = $(elt).data('json');
        if (!json) {
            json = { children: [] };
        }
        else {
            json.children = [];
        }
        var components = $(elt).find('> .component');
        components.each(function () {
            json.children.push(self.getJson(this));
        });
        return json;
    };
    __decorate([
        ViewChild('uiContainer'),
        __metadata("design:type", Object)
    ], ViewHumanTaskDefRenderingComponent.prototype, "uiContainer", void 0);
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
            ScannedActionsSubject,
            MatDialog])
    ], ViewHumanTaskDefRenderingComponent);
    return ViewHumanTaskDefRenderingComponent;
}());
export { ViewHumanTaskDefRenderingComponent };
var BaseComponent = (function () {
    function BaseComponent() {
    }
    BaseComponent.buildNode = function (dialog, opt) {
        switch (opt.type) {
            case 'column':
                return CellComponent.buildNode(dialog, opt);
            case 'row':
                return RowComponent.buildNode(dialog, opt);
            case 'txt':
                return TextComponent.buildNode(dialog, opt);
            case 'select':
                return SelectComponent.buildNode(dialog, opt);
        }
    };
    BaseComponent.buildTemplate = function (style, cls, html, opt, dialog, componentTypeRef) {
        var newOpt = Object.assign({}, opt, {});
        newOpt.children = [];
        var result = this.TEMPLATE.replace('{0}', style);
        result = result.replace('{1}', cls);
        result = result.replace('{2}', opt.type);
        result = result.replace('{3}', html);
        result = result.replace('{json}', JSON.stringify(newOpt));
        var query = $(result);
        query.click(function (evt) {
            evt.stopPropagation();
            if (!$(this).hasClass('selected')) {
                $(this).addClass('selected');
            }
            else {
                $(this).removeClass('selected');
            }
        });
        query.find('.remove').click(function (evt) {
            evt.stopPropagation();
            $(this).closest('.component').remove();
        });
        query.find('.settings').click(function (evt) {
            var _this = this;
            evt.stopPropagation();
            var dialogRef = dialog.open(componentTypeRef, {
                data: { opt: opt }
            });
            dialogRef.afterClosed().subscribe(function (r) {
                if (!r) {
                    return;
                }
                var component = $(_this).closest('.component');
                var parent = component.parent();
                component.remove();
                var node = BaseComponent.buildNode(dialog, r);
                $(parent).append(node);
            });
        });
        return query[0];
    };
    BaseComponent.SETTINGS_SVG = "<svg xmlns='http://www.w3.org/2000/svg' enable-background='new 0 0 24 24' viewBox='0 0 24 24' fill='white' width='18px' height='18px'><g><path d='M0,0h24v24H0V0z' fill='none'/><path d='M19.14,12.94c0.04-0.3,0.06-0.61,0.06-0.94c0-0.32-0.02-0.64-0.07-0.94l2.03-1.58c0.18-0.14,0.23-0.41,0.12-0.61 l-1.92-3.32c-0.12-0.22-0.37-0.29-0.59-0.22l-2.39,0.96c-0.5-0.38-1.03-0.7-1.62-0.94L14.4,2.81c-0.04-0.24-0.24-0.41-0.48-0.41 h-3.84c-0.24,0-0.43,0.17-0.47,0.41L9.25,5.35C8.66,5.59,8.12,5.92,7.63,6.29L5.24,5.33c-0.22-0.08-0.47,0-0.59,0.22L2.74,8.87 C2.62,9.08,2.66,9.34,2.86,9.48l2.03,1.58C4.84,11.36,4.8,11.69,4.8,12s0.02,0.64,0.07,0.94l-2.03,1.58 c-0.18,0.14-0.23,0.41-0.12,0.61l1.92,3.32c0.12,0.22,0.37,0.29,0.59,0.22l2.39-0.96c0.5,0.38,1.03,0.7,1.62,0.94l0.36,2.54 c0.05,0.24,0.24,0.41,0.48,0.41h3.84c0.24,0,0.44-0.17,0.47-0.41l0.36-2.54c0.59-0.24,1.13-0.56,1.62-0.94l2.39,0.96 c0.22,0.08,0.47,0,0.59-0.22l1.92-3.32c0.12-0.22,0.07-0.47-0.12-0.61L19.14,12.94z M12,15.6c-1.98,0-3.6-1.62-3.6-3.6 s1.62-3.6,3.6-3.6s3.6,1.62,3.6,3.6S13.98,15.6,12,15.6z'/></g></svg>";
    BaseComponent.REMOVE_SVG = "<svg xmlns='http://www.w3.org/2000/svg' viewBox='0 0 24 24' fill='white' width='18px' height='18px'><path d='M0 0h24v24H0z' fill='none'/><path d='M6 19c0 1.1.9 2 2 2h8c1.1 0 2-.9 2-2V7H6v12zM19 4h-3.5l-1-1h-5l-1 1H5v2h14V4z'/></svg>";
    BaseComponent.TEMPLATE = "<div data-json='{json}' style='{0}' class='component {1}'><div class='title'>{2}</div><ul class='toolbar'><li class='settings'>" + BaseComponent.SETTINGS_SVG + "</li><li class='remove'>" + BaseComponent.REMOVE_SVG + "</li></ul>{3}</div>";
    return BaseComponent;
}());
export { BaseComponent };
var CellComponentDialog = (function () {
    function CellComponentDialog() {
    }
    return CellComponentDialog;
}());
export { CellComponentDialog };
var CellComponent = (function (_super) {
    __extends(CellComponent, _super);
    function CellComponent() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    CellComponent.buildNode = function (dialog, opt) {
        var style = 'max-width: ' + opt.width;
        return this.buildTemplate(style, 'cell', '', opt, dialog, CellComponentDialog);
    };
    return CellComponent;
}(BaseComponent));
export { CellComponent };
var RowComponentDialog = (function () {
    function RowComponentDialog(data, dialogRef) {
        this.data = data;
        this.dialogRef = dialogRef;
        this.configureRowForm = new FormGroup({
            nbColumns: new FormControl({ value: '' })
        });
        this.configureRowForm.get('nbColumns').setValue(data.opt.children.length);
    }
    RowComponentDialog.prototype.onSave = function (val) {
        var opt = this.data.opt;
        opt.children = [];
        var percentage = (100 / val.nbColumns) + '%';
        for (var i = 0; i < val.nbColumns; i++) {
            opt.children.push({ type: 'column', width: percentage });
        }
        this.dialogRef.close(opt);
    };
    RowComponentDialog = __decorate([
        Component({
            selector: 'view-row-dialog',
            templateUrl: 'rowdialog.component.html',
        }),
        __param(0, Inject(MAT_DIALOG_DATA)),
        __metadata("design:paramtypes", [Object, MatDialogRef])
    ], RowComponentDialog);
    return RowComponentDialog;
}());
export { RowComponentDialog };
var RowComponent = (function (_super) {
    __extends(RowComponent, _super);
    function RowComponent() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    RowComponent.buildNode = function (dialog, opt) {
        var result = this.buildTemplate('', 'row', '', opt, dialog, RowComponentDialog);
        opt.children.forEach(function (o) {
            $(result).append(BaseComponent.buildNode(dialog, o));
        });
        return result;
    };
    return RowComponent;
}(BaseComponent));
export { RowComponent };
var TextComponentDialog = (function () {
    function TextComponentDialog(data, dialogRef) {
        this.data = data;
        this.dialogRef = dialogRef;
        this.configureTxtForm = new FormGroup({
            label: new FormControl({ value: '' }),
            name: new FormControl({ value: '' })
        });
        this.configureTxtForm.get('label').setValue(data.opt.label);
        this.configureTxtForm.get('name').setValue(data.opt.name);
    }
    TextComponentDialog.prototype.onSave = function (val) {
        var opt = this.data.opt;
        opt.label = val.label;
        opt.name = val.name;
        this.dialogRef.close(opt);
    };
    TextComponentDialog = __decorate([
        Component({
            selector: 'view-text-dialog',
            templateUrl: 'textdialog.component.html',
        }),
        __param(0, Inject(MAT_DIALOG_DATA)),
        __metadata("design:paramtypes", [Object, MatDialogRef])
    ], TextComponentDialog);
    return TextComponentDialog;
}());
export { TextComponentDialog };
var TextComponent = (function (_super) {
    __extends(TextComponent, _super);
    function TextComponent() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    TextComponent.buildNode = function (dialog, opt) {
        if (opt) { }
        var html = "<div class='full-width mat-form-field ng-tns-c12-4 mat-primary mat-form-field-type-mat-input mat-form-field-appearance-outline mat-form-field-can-float mat-form-field-should-float mat-form-field-has-label ng-star-inserted'>" +
            "<div class='mat-form-field-wrapper'>" +
            "<div class='mat-form-field-flex'>" +
            "<div class='mat-form-field-outline ng-tns-c13-28 ng-star-inserted'>" +
            "<div class='mat-form-field-outline-start' style='width: 7px;'></div>" +
            "<div class='mat-form-field-outline-gap' style='width: 41.5px;'></div>" +
            "<div class='mat-form-field-outline-end'></div>" +
            "</div>" +
            "<div class='mat-form-field-infix'>" +
            "<input class='mat-input-element mat-form-field-autofill-control cdk-text-field-autofill-monitored ng-pristine ng-valid ng-touched' disabled >" +
            "<span class='mat-form-field-label-wrapper'>" +
            "<label class='mat-form-field-label ng-tns-c12-4 ng-star-inserted'>" + opt.label + "</label>" +
            "</span>" +
            "</div>" +
            "</div>" +
            "</div>" +
            "</div>";
        return this.buildTemplate('', 'txt', html, opt, dialog, TextComponentDialog);
    };
    return TextComponent;
}(BaseComponent));
export { TextComponent };
var SelectComponentDialog = (function () {
    function SelectComponentDialog(data, dialogRef) {
        this.data = data;
        this.dialogRef = dialogRef;
        this.configureSelectForm = new FormGroup({});
    }
    SelectComponentDialog.prototype.onSave = function (val) {
        if (val) { }
        var opt = this.data.opt;
        this.dialogRef.close(opt);
    };
    SelectComponentDialog = __decorate([
        Component({
            selector: 'view-select-dialog',
            templateUrl: 'selectdialog.component.html',
        }),
        __param(0, Inject(MAT_DIALOG_DATA)),
        __metadata("design:paramtypes", [Object, MatDialogRef])
    ], SelectComponentDialog);
    return SelectComponentDialog;
}());
export { SelectComponentDialog };
var SelectComponent = (function (_super) {
    __extends(SelectComponent, _super);
    function SelectComponent() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    SelectComponent.buildNode = function (dialog, opt) {
        if (opt) { }
        var html = "<div class='full-width mat-form-field ng-tns-c12-4 mat-primary mat-form-field-type-mat-input mat-form-field-appearance-outline mat-form-field-can-float mat-form-field-should-float mat-form-field-has-label ng-star-inserted'>" +
            "<div class='mat-form-field-wrapper'>" +
            "<div class='mat-form-field-flex'>" +
            "<div class='mat-form-field-outline ng-tns-c13-28 ng-star-inserted'>" +
            "<div class='mat-form-field-outline-start' style='width: 7px;'></div>" +
            "<div class='mat-form-field-outline-gap' style='width: 41.5px;'></div>" +
            "<div class='mat-form-field-outline-end'></div>" +
            "</div>" +
            "<div class='mat-form-field-infix'>" +
            "<input class='mat-input-element mat-form-field-autofill-control cdk-text-field-autofill-monitored ng-pristine ng-valid ng-touched' disabled >" +
            "<span class='mat-form-field-label-wrapper'>" +
            "<label class='mat-form-field-label ng-tns-c12-4 ng-star-inserted'>" + opt.label + "</label>" +
            "</span>" +
            "</div>" +
            "</div>" +
            "</div>" +
            "</div>";
        return this.buildTemplate('', 'dlg', html, opt, dialog, SelectComponentDialog);
    };
    return SelectComponent;
}(BaseComponent));
export { SelectComponent };
//# sourceMappingURL=rendering.component.js.map