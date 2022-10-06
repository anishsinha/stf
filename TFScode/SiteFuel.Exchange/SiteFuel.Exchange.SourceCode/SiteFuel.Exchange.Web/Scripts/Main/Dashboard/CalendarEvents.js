var CalendarEvents;
(function (CalendarEvents) {
    var DeliverySchedule = /** @class */ (function () {
        function DeliverySchedule(status, title, subtitle) {
            this.Status = status;
            this.Title = title;
            this.SubTitle = subtitle;
        }
        DeliverySchedule.prototype.getEventHtml = function () {
            var contentHtml = this.getContentHtml();
            var statusHtml = this.getStatusHtml();
            var eventHtml = new EventHtml();
            var div = eventHtml.getHtml(contentHtml, statusHtml);
            return div;
        };
        DeliverySchedule.prototype.getContentHtml = function () {
            var contentHtml = new ContentHtml(this.Title, this.SubTitle);
            var textcolorClass = (this.Status === 11 ? 'text-danger' : ''); //missed schedule
            var div = '<div class="' + textcolorClass + ' pt5 pb5 pull-left">';
            div += contentHtml.getHtml();
            div += '</div>';
            return div;
        };
        DeliverySchedule.prototype.getStatusHtml = function () {
            var statusText = this.getStatusText(this.Status);
            var div = '<div class="pull-right">';
            if (statusText != '') {
                if (this.Status === 20 || this.Status === 21) { // canceled || rescheduled
                    div += '<i class="mt10 mr10 pull-left fa fa-check"></i>';
                }
                var statusClass = ((statusText == missed || statusText == canceled) ? 'btn-danger' : 'btn-success');
                div += '<label class="btn ' + statusClass + ' btn-xs mt5 fs10">' + statusText + '</label>';
            }
            div += '</div>';
            return div;
        };
        DeliverySchedule.prototype.getStatusText = function (statusId) {
            var statusText = '';
            switch (statusId) {
                case 11:
                case 12:
                case 20:
                case 21:
                    statusText = missed;
                    break;
                case 7:
                case 8:
                case 9:
                case 10:
                    statusText = completed;
                    break;
                case 19:
                    statusText = discontinued;
                    break;
                case 6:
                    statusText = rescheduled;
                    break;
                case 5:
                    statusText = canceled;
                    break;
            }
            return statusText;
        };
        return DeliverySchedule;
    }());
    CalendarEvents.DeliverySchedule = DeliverySchedule;
    var Order = /** @class */ (function () {
        function Order(title, subtitle, status) {
            this.Title = title;
            this.SubTitle = subtitle;
            this.Status = status;
        }
        Order.prototype.getEventHtml = function () {
            var contentHtml = this.getContentHtml();
            var statusHtml = this.getStatusHtml();
            var eventHtml = new EventHtml();
            var div = eventHtml.getHtml(contentHtml, statusHtml);
            return div;
        };
        Order.prototype.getContentHtml = function () {
            var contentHtml = new ContentHtml(this.Title, this.SubTitle);
            var div = '<div class="pt5 pb5 pull-left"><div class="pull-left"><i class="pull-left fa fs18  fa-flag mt5 mr5"></i></div>';
            div += '<div class="pull-left">' + contentHtml.getHtml() + '</div>';
            div += '</div>';
            return div;
        };
        Order.prototype.getStatusHtml = function () {
            var div = '<div class="pull-right">';
            if (this.Status != null && this.Status != undefined && this.Status != '') {
                var statusClass = (this.Status.toLowerCase() === 'open' ? 'btn-success' : 'btn-danger');
                div += '<label class="btn ' + statusClass + ' btn-xs mt5 fs10">' + this.Status + '</label>';
            }
            div += '</div>';
            return div;
        };
        return Order;
    }());
    CalendarEvents.Order = Order;
    var ContentHtml = /** @class */ (function () {
        function ContentHtml(title, subtitle) {
            this.Title = title;
            this.SubTitle = subtitle;
        }
        ContentHtml.prototype.getHtml = function () {
            var element = '<h2 class="fs14 font-bold pt0 pb0 mt0 mb0">' + this.Title + '</h2>';
            element += '<h4 class="fs12 pt3 pb0 mt0 mb0 f-normal">' + this.SubTitle + '</h4>';
            return element;
        };
        return ContentHtml;
    }());
    var EventHtml = /** @class */ (function () {
        function EventHtml() {
        }
        EventHtml.prototype.getHtml = function (contentHtml, statusHtml) {
            var div = '<div class="border-b pb5 pt5 overflow-h full-width">';
            div += (contentHtml + statusHtml) + '</div>';
            return div;
        };
        return EventHtml;
    }());
})(CalendarEvents || (CalendarEvents = {}));
//# sourceMappingURL=CalendarEvents.js.map