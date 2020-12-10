var HumanTaskDef = (function () {
    function HumanTaskDef() {
        this.operationParameters = [];
        this.peopleAssignments = [];
        this.presentationElements = [];
        this.deadLines = [];
    }
    HumanTaskDef.getInputOperationParameters = function (hd) {
        return hd.operationParameters.filter(function (v) {
            return v.usage === 'INPUT';
        });
    };
    HumanTaskDef.getOutputOperationParameters = function (hd) {
        return hd.operationParameters.filter(function (v) {
            return v.usage === 'OUTPUT';
        });
    };
    HumanTaskDef.getPotentialOwners = function (hd) {
        return hd.peopleAssignments.filter(function (p) {
            return p.usage === 'POTENTIALOWNER';
        });
    };
    HumanTaskDef.getExcludedOwners = function (hd) {
        return hd.peopleAssignments.filter(function (p) {
            return p.usage === 'EXCLUDEDOWNER';
        });
    };
    HumanTaskDef.getTaskInitiators = function (hd) {
        return hd.peopleAssignments.filter(function (p) {
            return p.usage === 'TASKINITIATOR';
        });
    };
    HumanTaskDef.getTaskStakeHolders = function (hd) {
        return hd.peopleAssignments.filter(function (p) {
            return p.usage === 'TASKSTAKEHOLDER';
        });
    };
    HumanTaskDef.getBusinessAdministrators = function (hd) {
        return hd.peopleAssignments.filter(function (p) {
            return p.usage === 'BUINESSADMINISTRATOR';
        });
    };
    HumanTaskDef.getRecipients = function (hd) {
        return hd.peopleAssignments.filter(function (p) {
            return p.usage === 'RECIPIENT';
        });
    };
    HumanTaskDef.getNames = function (hd) {
        return hd.presentationElements.filter(function (pe) {
            return pe.usage === 'NAME';
        });
    };
    HumanTaskDef.getDescriptions = function (hd) {
        return hd.presentationElements.filter(function (pe) {
            return pe.usage === 'DESCRIPTION';
        });
    };
    HumanTaskDef.getSubjects = function (hd) {
        return hd.presentationElements.filter(function (pe) {
            return pe.usage === 'SUBJECT';
        });
    };
    HumanTaskDef.getStartDeadlines = function (hd) {
        return hd.deadLines.filter(function (d) {
            return d.usage === 'START';
        });
    };
    HumanTaskDef.getCompletionDeadlines = function (hd) {
        return hd.deadLines.filter(function (d) {
            return d.usage === 'COMPLETION';
        });
    };
    return HumanTaskDef;
}());
export { HumanTaskDef };
//# sourceMappingURL=humantaskdef.model.js.map