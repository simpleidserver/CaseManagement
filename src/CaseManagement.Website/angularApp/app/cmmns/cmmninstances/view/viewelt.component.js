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
import { ActivatedRoute, Router } from '@angular/router';
import * as fromAppState from '@app/stores/appstate';
import { select, Store } from '@ngrx/store';
var ViewCasePlanEltInstanceComponent = (function () {
    function ViewCasePlanEltInstanceComponent(store, route, router) {
        this.store = store;
        this.route = route;
        this.router = router;
        this.children = [];
        this.cmmnPlanInstance = null;
        this.id = null;
    }
    ViewCasePlanEltInstanceComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.store.pipe(select(fromAppState.selectCmmnPlanInstanceResult)).subscribe(function (e) {
            if (!e) {
                return;
            }
            _this.cmmnPlanInstance = e;
            _this.refresh();
        });
        this.route.params.subscribe(function () {
            _this.refreshSelectedId();
            _this.refresh();
        });
        this.refreshSelectedId();
    };
    ViewCasePlanEltInstanceComponent.prototype.refresh = function () {
        if (!this.cmmnPlanInstance) {
            return;
        }
        var eltId = this.route.snapshot.params['eltid'];
        this.children = this.cmmnPlanInstance.children.filter(function (r) {
            return r.eltId === eltId;
        }).sort(function (a, b) {
            return b.nbOccurrence - a.nbOccurrence;
        });
    };
    ViewCasePlanEltInstanceComponent.prototype.refreshSelectedId = function () {
        if (this.route.children && this.route.children[0]) {
            this.id = this.route.children[0].snapshot.params['instid'];
        }
    };
    ViewCasePlanEltInstanceComponent.prototype.navigate = function (evt, elt) {
        evt.preventDefault();
        var id = this.route.parent.snapshot.params['id'];
        this.id = elt.id;
        this.router.navigate(['/cmmns/cmmninstances/' + id + '/' + elt.eltId + '/' + elt.id + '/history']);
    };
    ViewCasePlanEltInstanceComponent = __decorate([
        Component({
            selector: 'view-cmmn-planinstance-elt',
            templateUrl: './viewelt.component.html',
            styleUrls: ['./viewelt.component.scss']
        }),
        __metadata("design:paramtypes", [Store,
            ActivatedRoute,
            Router])
    ], ViewCasePlanEltInstanceComponent);
    return ViewCasePlanEltInstanceComponent;
}());
export { ViewCasePlanEltInstanceComponent };
//# sourceMappingURL=viewelt.component.js.map