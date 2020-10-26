import { Operation } from "../../common/operation.model";
import { PresentationElement } from "../../common/presentationelement.model";
import { Rendering } from "../../common/rendering.model";
import { HumanTaskDefAssignment } from "./humantaskdef-assignment.model";
import { HumanTaskDefinitionDeadLines } from "./humantaskdef-deadlines";
var HumanTaskDef = (function () {
    function HumanTaskDef() {
        this.rendering = new Rendering();
        this.operation = new Operation();
        this.deadLines = new HumanTaskDefinitionDeadLines();
        this.peopleAssignment = new HumanTaskDefAssignment();
        this.presentationElementResult = new PresentationElement();
    }
    return HumanTaskDef;
}());
export { HumanTaskDef };
//# sourceMappingURL=humantaskdef.model.js.map