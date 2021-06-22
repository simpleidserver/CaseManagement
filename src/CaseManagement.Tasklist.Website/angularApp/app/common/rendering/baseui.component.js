import { EventEmitter } from "@angular/core";
var BaseUIComponent = (function () {
    function BaseUIComponent() {
        this.onInitialized = new EventEmitter();
    }
    BaseUIComponent.prototype.openDialog = function () {
    };
    BaseUIComponent.prototype.ngOnInit = function () {
        this.onInitialized.emit();
        this.init();
    };
    BaseUIComponent.prototype.init = function () {
    };
    return BaseUIComponent;
}());
export { BaseUIComponent };
//# sourceMappingURL=baseui.component.js.map