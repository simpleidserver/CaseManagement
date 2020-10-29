var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
import { Pipe } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
var TranslateFieldPipe = (function () {
    function TranslateFieldPipe(translate) {
        this.translate = translate;
    }
    TranslateFieldPipe.prototype.transform = function (translations) {
        var defaultLang = this.translate.currentLang;
        if (!translations || translations.length === 0) {
            return '';
        }
        var filtered = translations.filter(function (t) {
            return t.language === defaultLang;
        });
        if (filtered.length !== 1) {
            return "UNDEFINED";
        }
        return filtered[0].value;
    };
    TranslateFieldPipe = __decorate([
        Pipe({ name: 'translateField', pure: false }),
        __metadata("design:paramtypes", [TranslateService])
    ], TranslateFieldPipe);
    return TranslateFieldPipe;
}());
export { TranslateFieldPipe };
//# sourceMappingURL=translateFieldPipe.js.map