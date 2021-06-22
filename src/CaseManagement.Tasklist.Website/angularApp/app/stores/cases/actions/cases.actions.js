export var ActionTypes;
(function (ActionTypes) {
    ActionTypes["SEARCH_CASES"] = "[Cases] SEARCH_CASES";
    ActionTypes["ERROR_SEARCH_CASES"] = "[Cases] ERROR_SEARCH_CASES";
    ActionTypes["COMPLETE_SEARCH_CASES"] = "[Cases] COMPLETE_SEARCH_CASES";
    ActionTypes["GET_CASE"] = "[Cases] GET_CASE";
    ActionTypes["ERROR_GET_CASE"] = "[Cases] ERROR_GET_CASE";
    ActionTypes["COMPLETE_GET_CASE"] = "[Cases] COMPLETE_GET_CASE";
    ActionTypes["ACTIVATE"] = "[Cases] ACTIVATE";
    ActionTypes["COMPLETE_ACTIVATE"] = "[Cases] COMPLETE_ACTIVATE";
    ActionTypes["ERROR_ACTIVATE"] = "[Cases] ERROR_ACTIVATE";
    ActionTypes["DISABLE"] = "[Cases] DISABLE";
    ActionTypes["COMPLETE_DISABLE"] = "[Cases] COMPLETE_DISABLE";
    ActionTypes["ERROR_DISABLE"] = "[Cases] ERROR_DISABLE";
    ActionTypes["REENABLE"] = "[Cases] REENABLE";
    ActionTypes["COMPLETE_REENABLE"] = "[Cases] COMPLETE_REENABLE";
    ActionTypes["ERROR_REENABLE"] = "[Cases] ERROR_REENABLE";
    ActionTypes["COMPLETE"] = "[Cases] COMPLETE";
    ActionTypes["COMPLETED"] = "[Cases] COMPLETED";
    ActionTypes["ERROR_COMPLETE"] = "[Cases] ERROR_COMPLETE";
})(ActionTypes || (ActionTypes = {}));
var SearchCases = (function () {
    function SearchCases(order, direction, count, startIndex) {
        this.order = order;
        this.direction = direction;
        this.count = count;
        this.startIndex = startIndex;
        this.type = ActionTypes.SEARCH_CASES;
    }
    return SearchCases;
}());
export { SearchCases };
var CompleteSearchCases = (function () {
    function CompleteSearchCases(content) {
        this.content = content;
        this.type = ActionTypes.COMPLETE_SEARCH_CASES;
    }
    return CompleteSearchCases;
}());
export { CompleteSearchCases };
var GetCase = (function () {
    function GetCase(id) {
        this.id = id;
        this.type = ActionTypes.GET_CASE;
    }
    return GetCase;
}());
export { GetCase };
var CompleteGetCase = (function () {
    function CompleteGetCase(content) {
        this.content = content;
        this.type = ActionTypes.COMPLETE_GET_CASE;
    }
    return CompleteGetCase;
}());
export { CompleteGetCase };
var Activate = (function () {
    function Activate(id, elt) {
        this.id = id;
        this.elt = elt;
        this.type = ActionTypes.ACTIVATE;
    }
    return Activate;
}());
export { Activate };
var Disable = (function () {
    function Disable(id, elt) {
        this.id = id;
        this.elt = elt;
        this.type = ActionTypes.DISABLE;
    }
    return Disable;
}());
export { Disable };
var Reenable = (function () {
    function Reenable(id, elt) {
        this.id = id;
        this.elt = elt;
        this.type = ActionTypes.REENABLE;
    }
    return Reenable;
}());
export { Reenable };
var Complete = (function () {
    function Complete(id, elt) {
        this.id = id;
        this.elt = elt;
        this.type = ActionTypes.COMPLETE;
    }
    return Complete;
}());
export { Complete };
//# sourceMappingURL=cases.actions.js.map