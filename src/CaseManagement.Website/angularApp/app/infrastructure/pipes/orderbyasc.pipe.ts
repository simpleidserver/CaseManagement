import { PipeTransform, Pipe } from '@angular/core'

// Un pipe pure est exécuté uniquement lorsque la référence de l'objet change. 
// Dans le cas d'une liste nous ne le souhaitons pas.
@Pipe({ name: 'sortAsc', pure: false })
export class OrderByAscendingPipe implements PipeTransform {
    transform(array: any, field: string): any[] {
        if (!Array.isArray(array)) {
            return;
        }

        array.sort((a: any, b: any) => {
            if (a[field] < b[field]) {
                return -1;
            } else if (a[field] > b[field]) {
                return 1;
            } else {
                return 0;
            }
        });

        return array;
    }
}