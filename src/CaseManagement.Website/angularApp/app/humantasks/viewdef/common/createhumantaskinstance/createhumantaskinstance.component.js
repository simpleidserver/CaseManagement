var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
import { Component, ViewEncapsulation, Input } from '@angular/core';
import { HumanTaskDef } from '@app/stores/humantaskdefs/models/humantaskdef.model';
import { CreateHumanTaskInstance } from '@app/stores/humantaskinstances/parameters/create-humantaskinstance.model';
var CreateHumanTaskInstanceComponent = (function () {
    function CreateHumanTaskInstanceComponent() {
        this.inputOperationParameters = [];
    }
    Object.defineProperty(CreateHumanTaskInstanceComponent.prototype, "humanTaskDef", {
        get: function () {
            return this._humanTaskDef;
        },
        set: function (v) {
            if (!v) {
                return;
            }
            this._humanTaskDef = v;
            this.inputOperationParameters = HumanTaskDef.getInputOperationParameters(v);
            this.refreshOperationParameter();
        },
        enumerable: false,
        configurable: true
    });
    Object.defineProperty(CreateHumanTaskInstanceComponent.prototype, "createHumanTaskInstance", {
        get: function () {
            return this._createHumanTaskInstance;
        },
        set: function (create) {
            if (!create) {
                return;
            }
            this._createHumanTaskInstance = create;
            this.refreshOperationParameter();
        },
        enumerable: false,
        configurable: true
    });
    CreateHumanTaskInstanceComponent.prototype.refreshOperationParameter = function () {
        var self = this;
        if (this._humanTaskDef && this._createHumanTaskInstance) {
            this._createHumanTaskInstance.operationParameters = {};
            this._humanTaskDef.operationParameters.filter(function (o) {
                return o.usage === 'INPUT';
            }).forEach(function (p) {
                self._createHumanTaskInstance.operationParameters[p.name] = '';
            });
        }
    };
    __decorate([
        Input(),
        __metadata("design:type", HumanTaskDef),
        __metadata("design:paramtypes", [HumanTaskDef])
    ], CreateHumanTaskInstanceComponent.prototype, "humanTaskDef", null);
    __decorate([
        Input(),
        __metadata("design:type", CreateHumanTaskInstance),
        __metadata("design:paramtypes", [CreateHumanTaskInstance])
    ], CreateHumanTaskInstanceComponent.prototype, "createHumanTaskInstance", null);
    CreateHumanTaskInstanceComponent = __decorate([
        Component({
            selector: 'createhumantaskinstance-component',
            templateUrl: './createhumantaskinstance.component.html',
            encapsulation: ViewEncapsulation.None
        })
    ], CreateHumanTaskInstanceComponent);
    return CreateHumanTaskInstanceComponent;
}());
export { CreateHumanTaskInstanceComponent };
//# sourceMappingURL=createhumantaskinstance.component.js.map