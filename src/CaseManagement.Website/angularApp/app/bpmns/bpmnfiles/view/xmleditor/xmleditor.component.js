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
import * as fromBpmnFileActions from '@app/stores/bpmnfiles/actions/bpmn-files.actions';
import { BpmnFile } from '@app/stores/bpmnfiles/models/bpmn-file.model';
import { select, Store } from '@ngrx/store';
var ViewBpmnFileXMLEditorComponent = (function () {
    function ViewBpmnFileXMLEditorComponent(store) {
        this.store = store;
        this.editorOptions = { theme: 'vs-dark', language: 'xml', automaticLayout: true };
        this.bpmnFile = new BpmnFile();
    }
    ViewBpmnFileXMLEditorComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.store.pipe(select(fromAppState.selectBpmnFileResult)).subscribe(function (e) {
            if (!e) {
                return;
            }
            _this.xml = _this.formatXML(e.payload);
            _this.bpmnFile = e;
        });
    };
    ViewBpmnFileXMLEditorComponent.prototype.onSave = function () {
        var id = this.bpmnFile.id;
        var act = new fromBpmnFileActions.UpdateBpmnFilePayload(id, this.xml);
        this.store.dispatch(act);
    };
    ViewBpmnFileXMLEditorComponent.prototype.formatXML = function (xml) {
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
    ViewBpmnFileXMLEditorComponent = __decorate([
        Component({
            selector: 'view-bpmn-xmleditor-file',
            templateUrl: './xmleditor.component.html',
            styleUrls: ['./xmleditor.component.scss']
        }),
        __metadata("design:paramtypes", [Store])
    ], ViewBpmnFileXMLEditorComponent);
    return ViewBpmnFileXMLEditorComponent;
}());
export { ViewBpmnFileXMLEditorComponent };
//# sourceMappingURL=xmleditor.component.js.map