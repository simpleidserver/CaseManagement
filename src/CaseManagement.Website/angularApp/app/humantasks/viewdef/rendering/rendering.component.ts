import { Component, Directive, ViewChild, ViewContainerRef, ViewEncapsulation } from '@angular/core';
import { DynamicComponent } from './components/dynamic.component';

@Directive({
    selector: '[adHost]',
})
export class AdDirective {
    constructor(public viewContainerRef: ViewContainerRef) { }
}

@Component({
    selector: 'view-humantaskdef-rendering-component',
    templateUrl: './rendering.component.html',
    styleUrls: ['./rendering.component.scss'],
    encapsulation: ViewEncapsulation.None
})
export class ViewHumanTaskDefRenderingComponent {
    @ViewChild(DynamicComponent) dynamicComponent: DynamicComponent;
    options: Array<{}> = [];

    constructor() { }

    dragColumn(evt: any) {
        const json : any = {
            type: 'row',
            children: [
                { type: 'column', width: '50%', children: [] },
                { type: 'column', width: '50%', children: [] }
            ]
        };
        evt.dataTransfer.setData('json', JSON.stringify(json));
    }

    dragTxt(evt: any) {
        const json = {
            type: 'txt',
            label: 'Label',
            name: 'name'
        };
        evt.dataTransfer.setData('json', JSON.stringify(json));
    }

    dragSelect(evt: any) {
        const json = {
            type: 'select',
            label: 'Label',
            name: 'name'
        };
        evt.dataTransfer.setData('json', JSON.stringify(json));
    }

    dragOver(evt: any) {
        evt.preventDefault();
    }

    dropColumn(evt: any) {
        const json = JSON.parse(evt.dataTransfer.getData('json'));
        this.options.push(json);
        // const node = BaseComponent.buildNode(this.dialog, json);
        // evt.target.appendChild(node);
    }

    addComponent(): void {
    }

    /*
    getJson(elt: any) {
        const self = this;
        let json : any = $(elt).data('json');
        if (!json) {
            json = { children: [] };
        } else {
            json.children = [];
        }

        let components = $(elt).find('> .component');
        components.each(function () {
            json.children.push(self.getJson(this));
        });

        return json;
    }
    */
}

/*

export class BaseComponent {
    private static SETTINGS_SVG = "<svg xmlns='http://www.w3.org/2000/svg' enable-background='new 0 0 24 24' viewBox='0 0 24 24' fill='white' width='18px' height='18px'><g><path d='M0,0h24v24H0V0z' fill='none'/><path d='M19.14,12.94c0.04-0.3,0.06-0.61,0.06-0.94c0-0.32-0.02-0.64-0.07-0.94l2.03-1.58c0.18-0.14,0.23-0.41,0.12-0.61 l-1.92-3.32c-0.12-0.22-0.37-0.29-0.59-0.22l-2.39,0.96c-0.5-0.38-1.03-0.7-1.62-0.94L14.4,2.81c-0.04-0.24-0.24-0.41-0.48-0.41 h-3.84c-0.24,0-0.43,0.17-0.47,0.41L9.25,5.35C8.66,5.59,8.12,5.92,7.63,6.29L5.24,5.33c-0.22-0.08-0.47,0-0.59,0.22L2.74,8.87 C2.62,9.08,2.66,9.34,2.86,9.48l2.03,1.58C4.84,11.36,4.8,11.69,4.8,12s0.02,0.64,0.07,0.94l-2.03,1.58 c-0.18,0.14-0.23,0.41-0.12,0.61l1.92,3.32c0.12,0.22,0.37,0.29,0.59,0.22l2.39-0.96c0.5,0.38,1.03,0.7,1.62,0.94l0.36,2.54 c0.05,0.24,0.24,0.41,0.48,0.41h3.84c0.24,0,0.44-0.17,0.47-0.41l0.36-2.54c0.59-0.24,1.13-0.56,1.62-0.94l2.39,0.96 c0.22,0.08,0.47,0,0.59-0.22l1.92-3.32c0.12-0.22,0.07-0.47-0.12-0.61L19.14,12.94z M12,15.6c-1.98,0-3.6-1.62-3.6-3.6 s1.62-3.6,3.6-3.6s3.6,1.62,3.6,3.6S13.98,15.6,12,15.6z'/></g></svg>";
    private static REMOVE_SVG = "<svg xmlns='http://www.w3.org/2000/svg' viewBox='0 0 24 24' fill='white' width='18px' height='18px'><path d='M0 0h24v24H0z' fill='none'/><path d='M6 19c0 1.1.9 2 2 2h8c1.1 0 2-.9 2-2V7H6v12zM19 4h-3.5l-1-1h-5l-1 1H5v2h14V4z'/></svg>";
    private static TEMPLATE: string = "<div data-json='{json}' style='{0}' class='component {1}'><div class='title'>{2}</div><ul class='toolbar'><li class='settings'>" + BaseComponent.SETTINGS_SVG +"</li> " + BaseComponent.REMOVE_SVG +"</li></ul>{3}</div>";

    public static buildNode(dialog: MatDialog, opt: any) {
        switch (opt.type) {
            case 'column':
                return CellComponent.buildNode(dialog, opt);
            // case 'row':
            //    return RowComponent.buildNode(dialog, opt);
            case 'txt':
                return TextComponent.buildNode(dialog, opt);
            case 'select':
                return SelectComponent.buildNode(dialog, opt);
        }
    }

    protected static buildTemplate<T>(style: string, cls: string, html: string, opt: any, dialog: MatDialog, componentTypeRef: TemplateRef<T> | ComponentType<T>): string {
        const newOpt = Object.assign({}, opt, {});
        newOpt.children = [];
        let result = this.TEMPLATE.replace('{0}', style);
        result = result.replace('{1}', cls);
        result = result.replace('{2}', opt.type);
        result = result.replace('{3}', html);
        result = result.replace('{json}', JSON.stringify(newOpt));
        const query = $(result);
        query.click(function (evt: any) {
            evt.stopPropagation();
            if (!$(this).hasClass('selected')) {
                $(this).addClass('selected');
            } else {
                $(this).removeClass('selected');
            }
        });
        query.find('.remove').click(function (evt: any) {
            evt.stopPropagation();
            $(this).closest('.component').remove();
        });
        query.find('.settings').click(function (evt: any) {
            evt.stopPropagation();
            const dialogRef = dialog.open(componentTypeRef, {
                data: { opt: opt }
            });
            dialogRef.afterClosed().subscribe((r: any) => {
                if (!r) {
                    return;
                }

                const component = $(this).closest('.component');
                const parent = component.parent();
                component.remove();
                const node = BaseComponent.buildNode(dialog, r);
                $(parent).append(node);
            });
        });
        return query[0];
    }
}

export class CellComponentDialog { }

export class CellComponent extends BaseComponent {
    public static buildNode(dialog: MatDialog, opt: any) {
        const style = 'max-width: ' + opt.width;
        return this.buildTemplate(style, 'cell', '', opt, dialog, CellComponentDialog);
    }
}

@Component({
    selector: 'view-row-dialog',
    templateUrl: 'rowdialog.component.html',
})
export class RowComponentDialog {
    configureRowForm: FormGroup;
    constructor(
        @Inject(MAT_DIALOG_DATA) public data: any,
        public dialogRef: MatDialogRef<RowComponentDialog>
    ) {
        this.configureRowForm = new FormGroup({
            nbColumns: new FormControl({ value: '' })
        });
        this.configureRowForm.get('nbColumns').setValue(data.opt.children.length);
    }

    onSave(val: { nbColumns: number }) {
        const opt = this.data.opt;
        opt.children = [];
        const percentage = (100 / val.nbColumns) + '%';
        for (let i = 0; i < val.nbColumns; i++) {
            opt.children.push({ type: 'column', width: percentage });
        }

        this.dialogRef.close(opt);
    }
}

export class RowComponent extends BaseComponent {
    public static buildNode(dialog: MatDialog, opt: any) {
        const result = this.buildTemplate('', 'row', '', opt, dialog, RowComponentDialog);
        opt.children.forEach((o: any) => {
            $(result).append(BaseComponent.buildNode(dialog, o));
        });
        return result;
    }
}

@Component({
    selector: 'view-text-dialog',
    templateUrl: 'textdialog.component.html',
})
export class TextComponentDialog {
    configureTxtForm: FormGroup;

    constructor(
        @Inject(MAT_DIALOG_DATA) public data: any,
        public dialogRef: MatDialogRef<TextComponentDialog>) {
        this.configureTxtForm = new FormGroup({
            label: new FormControl({ value: '' }),
            name: new FormControl({ value: '' })
        });
        this.configureTxtForm.get('label').setValue(data.opt.label);
        this.configureTxtForm.get('name').setValue(data.opt.name);
    }

    onSave(val: { label: string, name: string }) {
        const opt = this.data.opt;
        opt.label = val.label;
        opt.name = val.name;
        this.dialogRef.close(opt);
    }
}

export class TextComponent extends BaseComponent {
    public static buildNode(dialog: MatDialog, opt: any) {
        if (opt) { }
        const html = "<div class='full-width mat-form-field ng-tns-c12-4 mat-primary mat-form-field-type-mat-input mat-form-field-appearance-outline mat-form-field-can-float mat-form-field-should-float mat-form-field-has-label ng-star-inserted'>" +
            "<div class='mat-form-field-wrapper'>" +
                "<div class='mat-form-field-flex'>" +
                    "<div class='mat-form-field-outline ng-tns-c13-28 ng-star-inserted'>"+
                        "<div class='mat-form-field-outline-start' style='width: 7px;'></div>" +
                        "<div class='mat-form-field-outline-gap' style='width: 41.5px;'></div>" +
                        "<div class='mat-form-field-outline-end'></div>" +
                    "</div>"+
                    "<div class='mat-form-field-infix'>"+
                        "<input class='mat-input-element mat-form-field-autofill-control cdk-text-field-autofill-monitored ng-pristine ng-valid ng-touched' disabled >"+
                        "<span class='mat-form-field-label-wrapper'>"+
                            "<label class='mat-form-field-label ng-tns-c12-4 ng-star-inserted'>"+opt.label+"</label>"+
                        "</span>"+
                    "</div>"+
                "</div>" +
            "</div>" +
        "</div>";
        return this.buildTemplate('', 'txt', html, opt, dialog, TextComponentDialog);
    }
}

@Component({
    selector: 'view-select-dialog',
    templateUrl: 'selectdialog.component.html',
})
export class SelectComponentDialog {
    configureSelectForm: FormGroup;

    constructor(
        @Inject(MAT_DIALOG_DATA) public data: any,
        public dialogRef: MatDialogRef<SelectComponentDialog>) {
        this.configureSelectForm = new FormGroup({
        });
    }

    onSave(val: {}) {
        if (val) { }
        const opt = this.data.opt;
        this.dialogRef.close(opt);
    }

}

export class SelectComponent extends BaseComponent {
    public static buildNode(dialog: MatDialog, opt: any) {
        if (opt) { }
        const html = "<div class='full-width mat-form-field ng-tns-c12-4 mat-primary mat-form-field-type-mat-input mat-form-field-appearance-outline mat-form-field-can-float mat-form-field-should-float mat-form-field-has-label ng-star-inserted'>" +
            "<div class='mat-form-field-wrapper'>" +
                "<div class='mat-form-field-flex'>" +
                    "<div class='mat-form-field-outline ng-tns-c13-28 ng-star-inserted'>"+
                        "<div class='mat-form-field-outline-start' style='width: 7px;'></div>" +
                        "<div class='mat-form-field-outline-gap' style='width: 41.5px;'></div>" +
                        "<div class='mat-form-field-outline-end'></div>" +
                    "</div>"+
                    "<div class='mat-form-field-infix'>"+
                        "<input class='mat-input-element mat-form-field-autofill-control cdk-text-field-autofill-monitored ng-pristine ng-valid ng-touched' disabled >"+
                        "<span class='mat-form-field-label-wrapper'>"+
                            "<label class='mat-form-field-label ng-tns-c12-4 ng-star-inserted'>"+opt.label+"</label>"+
                        "</span>"+
                    "</div>"+
                "</div>" +
            "</div>" +
        "</div>";
        return this.buildTemplate('', 'dlg', html, opt, dialog, SelectComponentDialog);
    }
}
*/