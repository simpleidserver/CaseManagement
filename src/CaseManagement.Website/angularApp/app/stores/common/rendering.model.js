var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var Rendering = (function () {
    function Rendering() {
        this.input = [];
        this.output = [];
    }
    return Rendering;
}());
export { Rendering };
var Translation = (function () {
    function Translation(lng, val) {
        this.language = lng;
        this.value = val;
    }
    return Translation;
}());
export { Translation };
var RenderingElement = (function () {
    function RenderingElement() {
        this.label = [];
    }
    return RenderingElement;
}());
export { RenderingElement };
var InputRenderingElement = (function (_super) {
    __extends(InputRenderingElement, _super);
    function InputRenderingElement() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    return InputRenderingElement;
}(RenderingElement));
export { InputRenderingElement };
var OutputRenderingElementValue = (function () {
    function OutputRenderingElementValue() {
        this.values = [];
    }
    return OutputRenderingElementValue;
}());
export { OutputRenderingElementValue };
var OutputRenderingElement = (function (_super) {
    __extends(OutputRenderingElement, _super);
    function OutputRenderingElement() {
        var _this = _super.call(this) || this;
        _this.value = new OutputRenderingElementValue();
        return _this;
    }
    return OutputRenderingElement;
}(RenderingElement));
export { OutputRenderingElement };
var OptionValue = (function () {
    function OptionValue() {
        this.displayNames = [];
    }
    return OptionValue;
}());
export { OptionValue };
//# sourceMappingURL=rendering.model.js.map