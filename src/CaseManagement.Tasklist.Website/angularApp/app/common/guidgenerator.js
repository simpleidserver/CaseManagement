var GuidGenerator = (function () {
    function GuidGenerator() {
    }
    GuidGenerator.newGUID = function () {
        return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
            var r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
            return v.toString(16);
        });
    };
    return GuidGenerator;
}());
export { GuidGenerator };
//# sourceMappingURL=guidgenerator.js.map