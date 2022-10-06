module Driver {
    declare var notificationOrderId: number;
    declare var notificationPoNumber: number;
    declare var notificationTargetUrl: string;

    export class Notification {
        TargetUrl: string;
        PoNumber: string;
        OrderId: number;
        DriverId: number;
        GroupId: number;

        constructor(targetUrl, orderId, poNumber, driverId, groupId) {
            this.TargetUrl = targetUrl;
            this.PoNumber = poNumber;
            this.OrderId = parseInt(orderId, 10);
            this.DriverId = parseInt(driverId, 10);
            this.GroupId = parseInt(groupId, 10);
        }

        SendNotification() {
            $.post({
                data: { orderId: this.OrderId, poNumber: this.PoNumber, driverId: this.DriverId, groupId: this.GroupId },
                url: this.TargetUrl,
                success: function (data: any, textStatus: string, jqXHR: JQueryXHR) {
                    $('#display-custom-message').html(data).show().delay(8000).fadeOut('fast', function () {
                        $('#display-custom-message').empty();
                    });
                },
                error: function (xhr) {
                    console.log(xhr);
                }
            });
        }
    };

    $(document).ready(function () {
        $(document).on("click", ".notify-driver", function () {
            var driverId = $(this).attr('data-driverid'), groupId = $(this).attr('data-groupid');
            var newsfeeds = new Driver.Notification(notificationTargetUrl, notificationOrderId, notificationPoNumber, driverId, groupId);
            newsfeeds.SendNotification();
        });
    });
}