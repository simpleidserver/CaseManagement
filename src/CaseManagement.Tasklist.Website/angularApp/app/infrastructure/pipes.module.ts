import { NgModule } from "@angular/core";
import { OrderByAscendingPipe } from "./pipes/orderbyasc.pipe";
import { OrderByDescendingPipe } from "./pipes/orderbydesc.pipe";
import { TranslateFieldPipe } from "./pipes/translateFieldPipe";

@NgModule({
    imports: [],
    declarations: [OrderByAscendingPipe, OrderByDescendingPipe, TranslateFieldPipe],
    exports: [OrderByAscendingPipe, OrderByDescendingPipe, TranslateFieldPipe],
})
export class PipesModule { }