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
import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material';
var ViewXmlDialog = (function () {
    function ViewXmlDialog(dialogRef, data) {
        this.dialogRef = dialogRef;
        this.data = data;
        this.editorOptions = { theme: 'vs-dark', language: 'xml', automaticLayout: true };
        this.xml = this.formatXML(data.xml);
    }
    ViewXmlDialog.prototype.update = function () {
        this.dialogRef.close({ xml: this.xml });
    };
    ViewXmlDialog.prototype.formatXML = function (xml) {
        var PADDING = ' '.repeat(2);
        var reg = /(>)(<)(\/*)/g;
        var pad = 0;
        xml = xml.replace(reg, '$1\r\n$2$3');
        return xml.split('\r\n').map(function (node) {
            var indent = 0;
            if (node.match(/.+<\/\w[^>]*>$/)) {
                indent = 0;
            }
            else if (node.match(/^<\/\w/) && pad > 0) {
                pad -= 1;
            }
            else if (node.match(/^<\w[^>]*[^\/]>.*$/)) {
                indent = 1;
            }
            else {
                indent = 0;
            }
            pad += indent;
            return PADDING.repeat(pad - indent) + node;
        }).join('\r\n');
    };
    ViewXmlDialog = __decorate([
        Component({
            selector: 'view-xml-dialog',
            templateUrl: 'view-xml-dialog.html',
        }),
        __param(1, Inject(MAT_DIALOG_DATA)),
        __metadata("design:paramtypes", [MatDialogRef, Object])
    ], ViewXmlDialog);
    return ViewXmlDialog;
}());
export { ViewXmlDialog };
//# sourceMappingURL=view-xml-dialog.js.map