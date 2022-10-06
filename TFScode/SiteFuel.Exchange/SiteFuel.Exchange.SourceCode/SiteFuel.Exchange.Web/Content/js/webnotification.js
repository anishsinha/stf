var notificationNewFrInterval = 0;
var currentWebNotificationId = 0;
$(document).ready(function () {
    //GetWebNotifications();

    //notificationNewFrInterval = setTimeout(GetWebNotifications, 2000);

    function GetWebNotifications() {
        if (notificationUrl && (isBuyerCompanyWebNotification == 'True' || isSupplierCompanyWebNotification == 'True')) {
            var url = notificationUrl + currentWebNotificationId;
            $.get(url, function (data) {
                if (!(typeof data.LatestId === 'undefined') && data.LatestId > 0) {

                    if (data.OfferNotificationCount > 0) {
                        $("#OfferNewNotificationsCount").show();
                    }
                    else {
                        $("#OfferNewNotificationsCount").hide();
                    }
                    if (data.OfferWebNotifications.length > 0) {
                        $("#OfferNewNotificationsCount").html(data.OfferNotificationCount);
                        refreshOfferWebNotificaitons(data.OfferWebNotifications);
                        AddViewAllElement('ul.offer-notificaitons-ul', notificationOfferUrl);
                    }
                    else {
                        NoOfferNotificationsUl();
                    }

                    if (data.InvoiceNotificationCount > 0) {
                        $("#InvoiceNewNotificationsCount").show();
                    }
                    else {
                        $("#InvoiceNewNotificationsCount").hide();
                    }

                    if (data.InvoiceWebNotifications.length > 0) {
                        $("#InvoiceNewNotificationsCount").html(data.InvoiceNotificationCount);
                        refreshInvoiceWebNotificaitons(data.InvoiceWebNotifications);
                        AddViewAllElement('ul.invoice-notifications-ul', viewAllInvoiceUrl);
                    }
                    else {
                        NoInvoiceNotificationsUl();
                    }

                    if (data.DispatchNotificationCount > 0) {
                        $("#DispatchNewNotificationsCount").show();
                    }
                    else {
                        $("#DispatchNewNotificationsCount").hide();
                    }

                    if (data.DispatchWebNotifications.length > 0) {
                        $("#DispatchNewNotificationsCount").html(data.DispatchNotificationCount);
                        refreshDispatchWebNotificaitons(data.DispatchWebNotifications);

                    }
                    else {
                        NoDispatchNotificationsUl();
                    }
                    currentWebNotificationId = data.LatestId;
                }
                else {
                    if (typeof data === 'string' && data.indexOf('<title>TrueFill - Unauthorized Access</title>') > 0) {
                        clearInterval(notificationNewFrInterval);
                    }
                    NoOfferNotificationsUl();
                    NoInvoiceNotificationsUl();
                    NoDispatchNotificationsUl();
                }
            });
        }
        else {
            clearInterval(notificationNewFrInterval);
        }
    }
});

function NoOfferNotificationsUl() {
    $('ul.offer-notificaitons-ul').empty();
    AddZeroNotificationMessage('ul.offer-notificaitons-ul');
}

function NoInvoiceNotificationsUl() {
    $('ul.invoice-notifications-ul').empty();
    AddZeroNotificationMessage('ul.invoice-notifications-ul');
}

function NoDispatchNotificationsUl() {
    $('ul.dispatch-notifications-ul').empty();
    AddZeroNotificationMessage('ul.dispatch-notifications-ul');
}
function refreshOfferWebNotificaitons(data) {
    $('ul.offer-notificaitons-ul').empty();
    for (var i = 0; i < data.length; i++) {
        var appendNotifications = '<li>'
            + '<a class="pa10" href=' + notificationOfferUrl + '>' + data[i].NotificationDetails.CreatedByCompanyName + ' ' + data[i].NotificationDetails.NotificaitonText + ' ' + data[i].NotificationDetails.FuelTypeName + '<br>'
            + '<i class="fs10 color-grey text-left db">' + data[i].CreatedDate + '</i></a>'
            + '</li>'
            + '<li role="separator" class="divider mt9 mb5"></li>';
        $('ul.offer-notificaitons-ul').prepend(appendNotifications);
    }
}

function refreshInvoiceWebNotificaitons(data) {
    $('ul.invoice-notifications-ul').empty();
    for (var i = 0; i < data.length; i++) {
        var appendNotifications = '<li>'
            + '<a class="pa10" href=' + notificationInvoiceUrl + '?Id=' + data[i].NotificationDetails.InvoiceId + '>' + data[i].NotificationDetails.CreatedByCompanyName + ' ' + data[i].NotificationDetails.NotificaitonText + ' ' + data[i].NotificationDetails.InvoiceNumber + '<br>'
            + '<i class="fs10 color-grey text-left db">' + data[i].CreatedDate + '</i></a>'
            + '</li>'
            + '<li role="separator" class="divider mt9 mb5"></li>';
        $('ul.invoice-notifications-ul').prepend(appendNotifications);
    }
}

function refreshDispatchWebNotificaitons(data) {
    $('ul.dispatch-notificaitons-ul').empty();
    for (var i = 0; i < data.length; i++) {
        var appendDispatchNotifications = '<li>'
            + '<a class="pa10" href=' + notificationDispatchUrl + '>' + data[i].NotificationDetails.NotificaitonText + '<br>'
            + '<i class="fs10 color-grey text-left db">' + data[i].CreatedDate + '</i></a>'
            + '</li>'
            + '<li role="separator" class="divider mt9 mb5"></li>';
        $('ul.dispatch-notificaitons-ul').prepend(appendDispatchNotifications);
    }
}

//call when user clicks on notification button
function SetWebNotificationsReadFlag(notificationType) {
    if (currentWebNotificationId > 0) {
       var updateNotificationUrl = updateNotificationUrl + notificationType;
        $.post(updateNotificationUrl, function (data) {
            if (data > 0) {
                //alert('read flag set to true');
            }
        });
    }
}

function AddViewAllElement(element, url) {
    var viewallElement = '<li role="separator" class="divider mt9 mb5"></li>' +
        '<li class="notification-viewall"> <a href=' + url + '>View All</a></li>';
    $(element).append(viewallElement);
}

function AddZeroNotificationMessage(element) {
    var noNotificationElement = '<li role="separator" class="divider mt9 mb5"></li>' +
        '<li class="text-center pa10" > <i class="fa fa-bell fs21 color-grey"></i><div class="fs14 color-lightgrey pt5">No New Notification</div></li>';
    $(element).append(noNotificationElement);
}

function ClearOfferNotificationCount() {
    $("#OfferNewNotificationsCount").hide();
}

function ClearInvoiceNotificationCount() {
    $("#InvoiceNewNotificationsCount").hide();
}
function ClearDispatchNotificationCount() {
    $("#DispatchNewNotificationsCount").hide();
}