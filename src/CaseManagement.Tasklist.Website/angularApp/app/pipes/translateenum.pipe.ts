import { Injectable, Pipe, PipeTransform } from "@angular/core";
import { TranslateService } from "@ngx-translate/core";

@Injectable()
@Pipe({
    name: 'translateenum',
    pure: false
})
export class TranslateEnumPipe implements PipeTransform {
    constructor(private ts: TranslateService) { }

    transform(translations: any[]) {
        if (!translations || translations.length === 0) {
            return null;
        }

        const lng = this.ts.currentLang;
        var filteredTranslations = translations.filter(function (tr: any) {
            return tr.language === lng;
        });
        if (filteredTranslations.length === 0) {
            return "unknown";
        }

        return filteredTranslations[0].value;
    }
}