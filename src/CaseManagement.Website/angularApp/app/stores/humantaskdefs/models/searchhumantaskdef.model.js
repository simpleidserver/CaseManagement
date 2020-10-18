import { Operation } from "../../common/operation.model";
import { HumanTaskDefinitionDeadLines } from "./humantaskdef-deadlines";
import { Rendering } from "../../common/rendering.model";
var HumanTaskDef = (function () {
    function HumanTaskDef() {
        this.rendering = new Rendering();
        this.operation = new Operation();
        this.deadLines = new HumanTaskDefinitionDeadLines();
    }
    return HumanTaskDef;
}());
export { HumanTaskDef };
//# sourceMappingURL=humantaskdef.model.js.map