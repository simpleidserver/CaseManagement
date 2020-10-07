import { Pipe, PipeTransform } from '@angular/core';
import { Translation } from '@app/stores/common/rendering.model';
import { TranslateService } from '@ngx-translate/core';

@Pipe({ name: 'translateField', pure: false })
export class TranslateFieldPipe implements PipeTransform {
    constructor(private translate: TranslateService) { }

    transform(translations: Translation[]): string {
        const defaultLang = this.translate.currentLang;
        if (!translations || translations.length === 0) {
            return '';
        }

        const filtered = translations.filter(function (t: Translation) {
            return t.language === defaultLang;
        });
        if (filtered.length !== 1) {
            return "UNDEFINED";
        }

        return filtered[0].value;
    }
}