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
import { select, Store } from '@ngrx/store';
import { ActionTypes } from './case-def-actions';
import { ActivatedRoute } from '@angular/router';
var CmmnViewer = require('cmmn-js');
var CaseDefComponent = (function () {
    function CaseDefComponent(store, route) {
        this.store = store;
        this.route = route;
    }
    CaseDefComponent.prototype.ngOnInit = function () {
        var _this = this;
        var self = this;
        this.isLoading = true;
        this.isErrorLoadOccured = false;
        this.viewer = new CmmnViewer({
            container: '#canvas'
        });
        this.subscription = this.store.pipe(select('caseDef')).subscribe(function (st) {
            if (!st) {
                return;
            }
            _this.isLoading = st.isLoading;
            _this.isErrorLoadOccured = st.isErrorLoadOccured;
            if (st.content) {
                self.viewer.importXML(st.content.Xml, function (err) {
                    if (!err) {
                        self.viewer.get('canvas').zoom('fit-viewport');
                    }
                });
            }
        });
        this.refresh();
    };
    CaseDefComponent.prototype.refresh = function () {
        var id = this.route.snapshot.params['id'];
        var request = {
            type: ActionTypes.CASEDEFLOAD,
            id: id
        };
        this.isLoading = true;
        this.store.dispatch(request);
    };
    CaseDefComponent.prototype.ngOnDestroy = function () {
        this.subscription.unsubscribe();
    };
    CaseDefComponent = __decorate([
        Component({
            selector: 'case-def',
            templateUrl: './case-def.component.html',
            styleUrls: ['./case-def.component.scss']
        }),
        __metadata("design:paramtypes", [Store, ActivatedRoute])
    ], CaseDefComponent);
    return CaseDefComponent;
}());
export { CaseDefComponent };
//# sourceMappingURL=case-def.component.js.map