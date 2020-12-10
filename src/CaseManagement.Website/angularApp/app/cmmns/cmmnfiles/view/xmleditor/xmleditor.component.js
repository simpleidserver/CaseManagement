var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
import { Component } from '@angular/core';
import * as fromAppState from '@app/stores/appstate';
import * as fromCmmnFileActions from '@app/stores/cmmnfiles/actions/cmmn-files.actions';
import { CmmnFile } from '@app/stores/cmmnfiles/models/cmmn-file.model';
import { select, Store } from '@ngrx/store';
var ViewCmmnFileXmlEditorComponent = (function () {
    function ViewCmmnFileXmlEditorComponent(store) {
        this.store = store;
        this.editorOptions = { theme: 'vs-dark', language: 'xml', automaticLayout: true };
        this.cmmnFile = new CmmnFile();
    }
    ViewCmmnFileXmlEditorComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.store.pipe(select(fromAppState.selectCmmnFileResult)).subscribe(function (e) {
            if (!e) {
                return;
            }
            _this.xml = _this.formatXML(e.payload);
            _this.cmmnFile = e;
        });
    };
    ViewCmmnFileXmlEditorComponent.prototype.onSave = function () {
        var id = this.cmmnFile.id;
        var act = new fromCmmnFileActions.UpdateCmmnFilePayload(id, this.xml);
        this.store.dispatch(act);
    };
    ViewCmmnFileXmlEditorComponent.prototype.formatXML = function (xml) {
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
    ViewCmmnFileXmlEditorComponent = __decorate([
        Component({
            selector: 'view-cmmn-xmleditor-file',
            templateUrl: './xmleditor.component.html',
            styleUrls: ['./xmleditor.component.scss']
        }),
        __metadata("design:paramtypes", [Store])
    ], ViewCmmnFileXmlEditorComponent);
    return ViewCmmnFileXmlEditorComponent;
}());
export { ViewCmmnFileXmlEditorComponent };
//# sourceMappingURL=xmleditor.component.js.map