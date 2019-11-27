var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
import { Pipe } from '@angular/core';
var OrderByDescendingPipe = (function () {
    function OrderByDescendingPipe() {
    }
    OrderByDescendingPipe.prototype.transform = function (array, field) {
        if (!Array.isArray(array)) {
            return;
        }
        array.sort(function (a, b) {
            if (a[field] > b[field]) {
                return -1;
            }
            else if (a[field] < b[field]) {
                return 1;
            }
            else {
                return 0;
            }
        });
        return array;
    };
    OrderByDescendingPipe = __decorate([
        Pipe({ name: 'sortDesc', pure: false })
    ], OrderByDescendingPipe);
    return OrderByDescendingPipe;
}());
export { OrderByDescendingPipe };
//# sourceMappingURL=orderbydesc.pipe.js.map