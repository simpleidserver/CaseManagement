var NotificationDefinition = (function () {
    function NotificationDefinition() {
        this.operationParameters = [];
        this.peopleAssignments = [];
        this.presentationElements = [];
        this.presentationParameters = [];
    }
    NotificationDefinition.getBusinessAdministrators = function (notifDef) {
        return notifDef.peopleAssignments.filter(function (p) {
            return p.usage === 'BUSINESSADMINISTRATOR';
        });
    };
    NotificationDefinition.getRecipients = function (notifDef) {
        return notifDef.peopleAssignments.filter(function (p) {
            return p.usage === 'RECIPIENT';
        });
    };
    NotificationDefinition.getInputParameters = function (notifDef) {
        return notifDef.operationParameters.filter(function (p) {
            return p.usage === 'INPUT';
        });
    };
    NotificationDefinition.getOutputParameter = function (notifDef) {
        return notifDef.operationParameters.filter(function (p) {
            return p.usage === 'OUTPUT';
        });
    };
    NotificationDefinition.getNames = function (hd) {
        return hd.presentationElements.filter(function (pe) {
            return pe.usage === 'NAME';
        });
    };
    NotificationDefinition.getDescriptions = function (hd) {
        return hd.presentationElements.filter(function (pe) {
            return pe.usage === 'DESCRIPTION';
        });
    };
    NotificationDefinition.getSubjects = function (hd) {
        return hd.presentationElements.filter(function (pe) {
            return pe.usage === 'SUBJECT';
        });
    };
    return NotificationDefinition;
}());
export { NotificationDefinition };
//# sourceMappingURL=notificationdefinition.model.js.map