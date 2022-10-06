var Driver;
(function (Driver) {
    var Notification = /** @class */ (function () {
        function Notification(targetUrl, orderId, poNumber, driverId, groupId) {
            this.TargetUrl = targetUrl;
            this.PoNumber = poNumber;
            this.OrderId = parseInt(orderId, 10);
            this.DriverId = parseInt(driverId, 10);
            this.GroupId = parseInt(groupId, 10);
        }
        Notification.prototype.SendNotification = function () {
            $.post({
                data: { orderId: this.OrderId, poNumber: this.PoNumber, driverId: this.DriverId, groupId: this.GroupId },
                url: this.TargetUrl,
                success: function (data, textStatus, jqXHR) {
                    $('#display-custom-message').html(data).show().delay(8000).fadeOut('fast', function () {
                        $('#display-custom-message').empty();
                    });
                },
                error: function (xhr) {
                    console.log(xhr);
                }
            });
        };
        return Notification;
    }());
    Driver.Notification = Notification;
    ;
    $(document).ready(function () {
        $(document).on("click", ".notify-driver", function () {
            var driverId = $(this).attr('data-driverid'), groupId = $(this).attr('data-groupid');
            var newsfeeds = new Driver.Notification(notificationTargetUrl, notificationOrderId, notificationPoNumber, driverId, groupId);
            newsfeeds.SendNotification();
        });
    });
})(Driver || (Driver = {}));
//# sourceMappingURL=DriverNotification.js.map