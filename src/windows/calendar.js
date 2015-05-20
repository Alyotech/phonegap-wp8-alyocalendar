cordova.define("AlyoCalendar", function (require, exports, module) {

    var exec = require('cordova/exec');

    var AlyoCalendar = {
        createEvent: function (title, eventLocation, notes, startDate, endDate, onSuccess, onError) {
            exec(onSuccess, onError, "Calendar", "createEvent", [title, eventLocation, notes, startDate, endDate]);
        }
    }
    module.exports = AlyoCalendar;
});