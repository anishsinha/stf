var NextDeliverySchedule;
(function (NextDeliverySchedule) {
    var NextSchedule = /** @class */ (function () {
        function NextSchedule(schedule) {
            this.ScheduleDate = schedule.ScheduleDate;
            this.ScheduleTime = schedule.ScheduleTime;
            this.Quantity = schedule.Quantity;
        }
        NextSchedule.prototype.getHtml = function () {
            var scheduleHtml = '<h4 class="l-height22 fs16">' + this.ScheduleDate + '<br />';
            scheduleHtml += this.ScheduleTime + '<br />';
            scheduleHtml += '<span class="fs14">' + this.Quantity + '</span></h4>';
            return scheduleHtml;
        };
        return NextSchedule;
    }());
    NextDeliverySchedule.NextSchedule = NextSchedule;
    var NextSchedules = /** @class */ (function () {
        function NextSchedules(schedules) {
            this.DeliverySchedules = new Array();
            for (var idx = 0; idx < schedules.length; idx++) {
                this.DeliverySchedules.push(new NextSchedule(schedules[idx]));
            }
        }
        NextSchedules.prototype.getAllSchedules = function () {
            var allSchedulesHtml = '';
            if (this.DeliverySchedules.length == 0) {
                allSchedulesHtml = '<h3 class="l-height22 f-normal fs16 mt10">--</h3>';
            }
            else {
                for (var idx = 0; idx < this.DeliverySchedules.length; idx++) {
                    allSchedulesHtml += this.DeliverySchedules[idx].getHtml();
                }
            }
            return allSchedulesHtml;
        };
        return NextSchedules;
    }());
    NextDeliverySchedule.NextSchedules = NextSchedules;
})(NextDeliverySchedule || (NextDeliverySchedule = {}));
//# sourceMappingURL=NextDeliverySchedules.js.map