import { Operation } from "../../common/operation.model";
import { Rendering } from "../../common/rendering.model";
var HumanTaskDef = (function () {
    function HumanTaskDef() {
        this.rendering = new Rendering();
        this.operation = new Operation();
    }
    return HumanTaskDef;
}());
export { HumanTaskDef };
//# sourceMappingURL=humantaskdef.model.js.map