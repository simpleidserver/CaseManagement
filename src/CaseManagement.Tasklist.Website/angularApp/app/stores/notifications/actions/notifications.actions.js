export var ActionTypes;
(function (ActionTypes) {
    ActionTypes["SEARCH_NOTIFICATIONS"] = "[Notifications] SEARCH_NOTIFICATIONS";
    ActionTypes["ERROR_SEARCH_NOTIFICATIONS"] = "[Notifications] ERROR_SEARCH_NOTIFICATIONS";
    ActionTypes["COMPLETE_SEARCH_NOTIFICATIONS"] = "[Notifications] COMPLETE_SEARCH_NOTIFICATIONS";
})(ActionTypes || (ActionTypes = {}));
var SearchNotifications = (function () {
    function SearchNotifications(order, direction, count, startIndex) {
        this.order = order;
        this.direction = direction;
        this.count = count;
        this.startIndex = startIndex;
        this.type = ActionTypes.SEARCH_NOTIFICATIONS;
    }
    return SearchNotifications;
}());
export { SearchNotifications };
var CompleteSearchNotifications = (function () {
    function CompleteSearchNotifications(content) {
        this.content = content;
        this.type = ActionTypes.COMPLETE_SEARCH_NOTIFICATIONS;
    }
    return CompleteSearchNotifications;
}());
export { CompleteSearchNotifications };
//# sourceMappingURL=notifications.actions.js.map