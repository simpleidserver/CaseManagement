import { NgModule } from "@angular/core";
import { OrderByAscendingPipe } from "./pipes/orderbyasc.pipe";
import { OrderByDescendingPipe } from "./pipes/orderbydesc.pipe";

@NgModule({
    imports: [],
    declarations: [OrderByAscendingPipe, OrderByDescendingPipe ],
    exports: [OrderByAscendingPipe, OrderByDescendingPipe ],
})
export class PipesModule { }