import { Injectable, Pipe, PipeTransform } from "@angular/core";
import { TranslateService } from "@ngx-translate/core";

@Injectable()
@Pipe({
    name: 'translateobj',
    pure: false
})
export class TranslateObjPipe implements PipeTransform {
    constructor(private ts: TranslateService) { }

    transform(translations: any) {
        if (!translations || translations.length === 0) {
            return null;
        }

        const lng = this.ts.currentLang;
        if (translations[lng]) {
            return translations[lng];
        }

        return "unknown";
    }
}