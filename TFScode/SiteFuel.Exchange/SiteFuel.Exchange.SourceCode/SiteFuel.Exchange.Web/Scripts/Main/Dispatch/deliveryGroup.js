function createRequiredRule(ruleSelector, elemSelector) {
    $(elemSelector).each(function () {
        var $this = $(this);
        var name = $this.attr('name');
        var msg = $this.data('required');
        ruleSelector.rules[name] = { required: true };
        ruleSelector.messages[name] = { required: msg };
    });
}
function addValidation(formSelector) {    
    var ruleSelector = {
        rules: {},
        messages: {}
    }
    createRequiredRule(ruleSelector, formSelector + ' .common-fields select');
    createRequiredRule(ruleSelector, formSelector + ' .common-fields input');
    //createRequiredRule(ruleSelector, formSelector + ' .common-fields textarea');
    $(formSelector).validate(ruleSelector);
}
function removeValidation(formSelector) {
    var form = $(formSelector);
    var validator = form.validate();
    validator.resetForm();
    form.find(".error").removeClass("error");
    form.rules("remove");
    form[0].reset();
}
function getOrdersForCreateGroup(formSelector) {  
    $(formSelector + ' .s-spinner').show();
    $.get('/Supplier/DeliveryGroup/GetOrdersForRouteGroup',
        function (response) {
            setOrdersTab(formSelector, response);

            $(formSelector + ' div.list-group>a:first-child').click();
        }).done(function () { $(formSelector + ' .s-spinner').hide(); });
}
function getOrdersForEditGroup(formSelector) {
    $(formSelector + ' .s-spinner').show();
    $.get('/Supplier/DeliveryGroup/GetOrdersForRouteGroup',
        function (response) {
            setOrdersTab(formSelector, response);
            $(formSelector).trigger('editTabsLoaded');
        }).done(function () { $(formSelector + ' .s-spinner').hide(); });
}

function setOrdersTab(formSelector, response) {   
    var vtabs = '';
    for (var idx = 0; idx < response.length; idx++) {
		vtabs += '<a data-orderid="' + response[idx].Id + '" data-isretailjob="' + response[idx].Code+'" class="list-group-item">'
            + response[idx].Name + '</a>';
    }
    $(formSelector + ' .v-tab.list-group').html(vtabs);
}

function getDeliverySchedules(formSelector, data, targetUrl) {
    var orderId = (data.orderId == undefined ? data.orderIds[0] : data.orderId);
    var tabContent = getTabContentElem(formSelector, data.orderId, data.includeFutureSchedules);
    var futureDelLoaded = tabContent.Elememt.find('table [data-future="true"]').length > 0;
    if (!tabContent.Loaded || (!futureDelLoaded && data.includeFutureSchedules == true)) {
        $('#create-modal-loader').show();
		$.post(targetUrl, data, function (response) {
			setDeliverySchedulesHtml(formSelector, data.orderId, data.includeFutureSchedules, response, data.isRetailJob);
        }).done(function () { $('#create-modal-loader').hide(); });
    }
    else if (futureDelLoaded && data.includeFutureSchedules == false) {
        var tableSel = 'table[data-orderid="' + data.orderId + '"]';
        tabContent.Elememt.find(tableSel + ' [data-future="true"]').remove();
        var tableElem = tabContent.Elememt.find(tableSel);
		if (tableElem.find('tbody tr').length == 0) {
			tableElem.remove();
			var tableElem = getTableElem(tabContent.Elememt, orderId, data.isRetailJob);
            tabContent.Elememt.append('<div id="message" class="text-center">No schedules available for today</div>');
        }
    }
    tabContent.Elememt.addClass("active");
}

function getEditDeliverySchedules(formSelector, data, targetUrl, callback) {  
	var orderId = (data.orderId == undefined ? data.orderIds[0] : data.orderId);
	var isRetailJob = (data.isRetailJob == undefined ? data.isRetailJobs[0] : data.isRetailJob);
    var tabContent = getTabContentElem(formSelector, orderId, data.includeFutureSchedules);
    var futureDelLoaded = tabContent.Elememt.find('table [data-future="true"]').length > 0;
    if (!tabContent.Loaded || (!futureDelLoaded && data.includeFutureSchedules == true)) {
        $('#create-modal-loader').show();
        $.post(targetUrl, data, function (response) {
            var oLength = data.orderId == undefined ? data.orderIds.length : 1;
            for (var idx = 0; idx < oLength; idx++) {
				var oId = data.orderId == undefined ? data.orderIds[idx] : orderId;
				var isRetail = data.isRetailJob == undefined ? data.isRetailJobs[idx] : isRetailJob;
                var schedules = response.TrackableSchedules.filter(function (el) { return el.OrderId == oId; });
				setDeliverySchedulesHtml(formSelector, oId, data.includeFutureSchedules, schedules, isRetail);
            }
            if (callback != undefined && callback != null) {
                callback(response);
            }
        }).done(function () { $('#create-modal-loader').hide(); });
    }
    else if (futureDelLoaded && data.includeFutureSchedules == false) {
        var tableSel = 'table[data-orderid="' + orderId + '"]';
        tabContent.Elememt.find(tableSel + ' [data-future="true"]').remove();
        var tableElem = tabContent.Elememt.find(tableSel);
        if (tableElem.find('tbody tr').length == 0) {
            tableElem.remove();
            tabContent.Elememt.append('<div id="message" class="text-center">No schedules available for today</div>');
        }
    }
    tabContent.Elememt.addClass("active");
}

function setDeliverySchedulesHtml(formSelector, orderId, futureDeliveries, response, isRetailJob) {
    var tabContent = getTabContentElem(formSelector, orderId, futureDeliveries);
    var futureDelLoaded = tabContent.Elememt.find('table [data-future="true"]').length > 0;
    if (!futureDelLoaded && (futureDeliveries == null || futureDeliveries == true)) {
        if (response.length > 0) {
			tabContent.Elememt.find('#message').remove();
			var tableElem = getTableElem(tabContent.Elememt, orderId, isRetailJob);
            var tbody = tableElem.find('tbody');
            for (var idx = 0; idx < response.length; idx++) {
                var html = getAddressHtml(response[idx].PickupLocation);
                var address = getPickupContainer(html, false, true);
                var tr = '<tr data-future="' + response[idx].IsFutureSchedule + '"><td><input type="checkbox" value="'
                    + response[idx].Id + '" ' + (response[idx].IsSelected ? 'checked="checked"' : '');
                if (response[idx].deliverystatus == 11) {
                    tr += 'class="chkMissedSchdules"';
                }                
                tr += ' /></td><td>' + response[idx].Name + '<br/>' + address + '</td>';
                if (response[idx].deliverystatus == 11) {
                    tr += '<td><label id="btnMissedSchedule"' + response[idx].Id + '  class="label-danger"> Missed </lable></td ></tr > ';
                } else {
                    tr += '<td><a id="btnCancelSchedule"' + response[idx].Id + ' data-deliveryschedulestatusid="5" class="label-danger" onclick="cancelSchedule(' + orderId + ', ' + response[idx].Id + ', this)" data-deliveryschedulestatusid="5"> Cancel </a></td ></tr >';
                }
                tbody.append(tr);
            }
        }
		else if (tabContent.Elememt.find('#message').length == 0) {
			var tableElem = getTableElem(tabContent.Elememt, orderId, isRetailJob);
            if (futureDeliveries)
                tabContent.Elememt.append('<div id="message" class="text-center">No future schedules available</div>');
            else
                tabContent.Elememt.append('<div id="message" class="text-center">No schedules available for today</div>');
        }
    }
}

function getTableRow(rowObject) {    
    return '<tr data-future="' + rowObject.IsFutureSchedule + '"><td><input type="checkbox" value="'
        + rowObject.Id + '" ' + (rowObject.IsSelected ? 'checked="checked"' : '')
        + ' /></td><td>' + rowObject.Name + '</td></tr>';
}

function getTabContentElem(formSelector, orderId, futureDeliveries) {   
        var result = { Elememt: null, Loaded: true };
    var tabContainer = $(formSelector + ' .bhoechie-tab');
    var tabContent = tabContainer.find('.bhoechie-tab-content[data-orderid="' + orderId + '"]');
    if (tabContent.length == 0) {
        tabContent = $('<div data-orderid="' + orderId + '" class="bhoechie-tab-content">'
            + '<div class="row ml0 mr0 border-bottom"><div class="col-sm-4 checkbox pl10">'
            + '<label><input type="checkbox" checked disabled />Today\'s Deliveries</label></div><div class="col-sm-6">'
            + '<div class="checkbox pl0"><label><input data-orderid="' + orderId
            + '" type="checkbox" class="future-deliveries" ' + (futureDeliveries == true || futureDeliveries == null ? 'checked="checked"' : '')
            + '/>Next 7 days deliveries</label></div></div></div>');
        tabContainer.append(tabContent);
        result.Loaded = false;
    }
    result.Elememt = tabContent;
    return result;
}
function getTableElem(tabContent, orderId, isRetailJob) {
	var isRetail = isRetailJob == 'True';
    var tableElem = tabContent.find('table[data-orderid="' + orderId + '"]');
	if (tableElem.length == 0) {
		var header = '';
		if (isRetail) {
			header = '<input id="' + orderId + '" type="checkbox"></th><th class="fs14 nohiddenchange"><span class="pull-left">Schedules</span>';
		}
		else {
			header = '<input id="' + orderId + '" type="checkbox"></th><th class="fs14 nohiddenchange"><span class="pull-left">Schedules</span><a onclick="addScheduleDetails(this)" data-toggle="modal" data-orderId="' + orderId + '" data-target="#add-delivery-modal" class="fs12 pull-left mb10 mt2" id="auto-linkaddnewschedule"><i class="fa fa-plus-circle fs12 mt3 ml10 pull-left"></i><span class="fs12 pull-left f-normal">Add New</span></a>';
		}
		tableElem = $('<table data-orderid="' + orderId + '" class="table table-condensed"><thead><tr><th>'
			+ header + '</th></tr></thead><tbody></tbody></table>');
        tabContent.append(tableElem);
    }
    return tableElem;
}

function createDeliveryGroup() {    
    var driverId = $('#cdg-form .deliveryGroupDriver').val();
    var loadCode = $('#cdg-form .loadCode').val();
    var routeInfo = $('#cdg-form .routeInfo').val();
    var deliveryList = $('#cdg-form .selected-schedules tr[data-id]');
    var isCommonLoc = $('#cdg-form .btn-applytoall').is(':checked');
    var commonPickup = isCommonLoc ? getPickupLocation($('#cdg-form .dg-common-pickup')) : null;
    if (deliveryList.length < 1) {
        msgerror('Please select at least 1 delivery to create a group');
    }
    else {
        if (isCommonLoc && (commonPickup == null || commonPickup == undefined || (commonPickup.SiteName == '' && commonPickup.TerminalId == ''))) {
            msgerror('Common pickup location is required');
        }
        else {
            $(".create-group").hide();
            $(".create-grouploader").show();
            var sIds = getSelectedScheduleData(deliveryList, isCommonLoc);
            var formData = {
                DriverId: driverId,
                LoadCode: loadCode,
                RouteNote: routeInfo,
                GroupTrackableSchedules: sIds,
                PickupLocation: commonPickup,
                IsCommonForGroup: isCommonLoc
            };
            $.post('/Supplier/DeliveryGroup/CreateDeliveryGroup'
                , formData, function (response) {
                    if (response.StatusCode == 0) {
                        msgsuccess(response.StatusMessage);
                        $('#create-modal-cancel').click();
                        $(".create-group").show();
                        $(".create-grouploader").hide();
                        ReloadScheduleGrid();
                        refreshDeliveryGroups();
                    }
                    else {
                        msgerror(response.StatusMessage);
                        $(".create-group").show();
                        $(".create-grouploader").hide();
                    }
                });
        }
    }
}

function getSelectedScheduleData(deliveryList, isCommonLoc) { 
    var data = [];
    deliveryList.each(function (idx, elem) {
        var element = $(elem);
        var dataObj = {
            Id: element.data('id'),
            PickupLocation: isCommonLoc ? null : getPickupLocation(element)
        };
        data.push(dataObj);
    });
    return data;
}

function getPickupLocation(element) {  
    return {
        SiteName: getChildText(element, '.sel-sitename'),
        IsPickupLocation: getChildText(element, '.sel-IsPickupLocation'),
        TerminalId: getChildText(element, '.sel-TerminalId '),
        TerminalName: getChildText(element, '.sel-TerminalName '),
        PickupAddress: getChildText(element, '.sel-address'),
        PickupCity: getChildText(element, '.sel-city'),
        PickupStateId: getChildText(element, '.sel-stateid'),
        PickupStateCode: getChildText(element, '.sel-statecode'),
        PickupZipCode: getChildText(element, '.sel-zipcode'),
        PickupCountryId: getChildText(element, '.sel-countryId'),
        PickupCountryCode: getChildText(element, '.sel-countrycode'),
        PickupCountyName: getChildText(element, '.sel-county'),
        IsPickupGeocodeUsed: getChildText(element, '.sel-isgeocode'),
        PickupLatitude: getChildText(element, '.sel-latitude'),
        PickupLongitude: getChildText(element, '.sel-longitude'),
        PickupTimeZone: getChildText(element, '.sel-timezone')
    };
}

function getChildText(element, selector) {
    var text = '';
    var elem = element.find(selector);
    if (elem.length > 0) {
        text = elem.text();
    }
    return text;
}

function editDeliveryGroup() {
    var groupId = $('#edit-dg-modal').data('groupid');
    var driverId = $('#edg-form .deliveryGroupDriver').val();
    var loadCode = $('#edg-form .loadCode').val();
    var routeInfo = $('#edg-form .routeInfo').val();
    var deliveryList = $('#edg-form .selected-schedules tr[data-id]');
    var isCommonLoc = $('#edg-form .btn-applytoall').is(':checked');
    var commonPickup = isCommonLoc ? getPickupLocation($('#edg-form .dg-common-pickup')) : null;
    var isMissedScheduleSelected = $('#edg-form .chkMissedSchdules').is(':checked');
    if (isMissedScheduleSelected) {
        msgerror('Missed schedules can not be saved.');
    }
    else if (deliveryList.length < 1) {
        msgerror('Delivery group must contain atleast 1 delivery');
    }
    else {
        if (isCommonLoc && (commonPickup == null || commonPickup == undefined || (commonPickup.PickupZipCode == '' && commonPickup.TerminalId == ''))) {
            msgerror('Common pickup location is required');
        }
        else {
            $(".edit-group").hide();
            $(".edit-grouploader").show();
            var sIds = getSelectedScheduleData(deliveryList, isCommonLoc);
            var formData = {
                DeliveryGroupId: groupId,
                DriverId: driverId,
                LoadCode: loadCode,
                RouteNote: routeInfo,
                TrackableSchedules: sIds,
                PickupLocation: commonPickup,
                IsCommonForGroup: isCommonLoc
            };
            $.post('/Supplier/DeliveryGroup/EditDeliveryGroup'
                , formData, function (response) {
                    if (response.StatusCode == 0) {
                        msgsuccess(response.StatusMessage);
                        $('#edit-modal-cancel').click();
                        $(".edit-group").show();
                        $(".edit-grouploader").hide();
                        ReloadScheduleGrid();
                        refreshDeliveryGroups();
                    }
                    else {
                        msgerror(response.StatusMessage);
                        $(".edit-group").show();
                        $(".edit-grouploader").hide();
                    }
                });
        }
    }
}
$(document).on('click', 'th input[type="checkbox"]', function () {
    var $this = $(this);
    var parent = $this.parents('.bhoechie-tab-content');
    parent.find('td input[type="checkbox"]').each(function (ixd, elem) {
        $(elem).prop('checked', $this.is(':checked')).change();
    });
});

$(document).on('click', 'td input[type="checkbox"]', function () {
    var $this = $(this);
    var parent = $this.parents('.bhoechie-tab-content');
    var allChecked = parent.find('td input[type="checkbox"]').length == parent.find('td input[type="checkbox"]:checked').length;
    parent.find('th input[type="checkbox"]').prop('checked', allChecked);
});

$(document).on('change', '.bhoechie-tab-content table td input[type="checkbox"]', function () {
    var formId = '#' + $(this).closest('form').attr('id');
    $(formId + ' .btn-applytoall').removeAttr('disabled');
    var $this = $(this), scheduleId = $this.val();
    var isChecked = $this.is(':checked');
    if (isChecked) {
        var orderId = $this.closest("table").data("orderid");
        var delivery = $this.closest("tr").find("td:nth-last-child(2)").html();
        if ($(formId + ' .btn-applytoall').is(':checked')) {
            var $delivery = $('<span>' + delivery + '</span>');
            $delivery.find('a.fa-edit').remove();
            $delivery.find('.dg-sel-address').remove();
            $delivery.append(getCommonPickupLocation(formId));
            delivery = $delivery.html();
        }
        //delivery += '<br/>' + getCommonPickupLocation(formId);

        var markup = '<tr data-id="' + scheduleId + '" data-orderid="' + orderId + '"><td>' + delivery + '</td></tr>';
        $('.selected-schedules table tbody').append(markup);
        $('.selected-schedules table tbody').find('.dg-sel-address').show();
        $('.selected-schedules table tbody').find('a.fa-edit').show();
    }
    else {
        var selector = 'tr[data-id="' + scheduleId + '"]';
        $('.selected-schedules table tbody').find(selector).remove();
    }
});

function getCommonPickupLocation(formId) {
    var addressHtml = '', isCommon = $(formId + ' .btn-applytoall').is(':checked');
    if (isCommon) {
        addressHtml = $(formId + ' .dg-common-pickup').html();
    }
    return getPickupContainer(addressHtml, isCommon);
}

function getPickupContainer(addressHtml, isCommon, hideAddress) {   
    hideAddress = hideAddress == undefined ? false : hideAddress;
    var style = hideAddress == true ? 'style="display:none;"' : '';
    if (addressHtml == null || addressHtml == undefined || addressHtml == '') {
        return '<span class="dg-sel-address" ' + style + '></span> '
            //+ '<a class="mt5" data-toggle="modal" data-target="#addPickupLocation">+ Pick-up location</a> '
            + '<a class="mt5 fa fa-edit" data-toggle="modal" data-target="#addPickupLocation" style="display:none;"></a>';
    }
    else {
        var disabled = isCommon == true ? 'pntr-none subSectionOpacity' : '';
        return '<span class="dg-sel-address ' + disabled +'" ' + style + '>' + addressHtml + '</span> '
            //+ '<a class="mt5 hide-element" data-toggle="modal" data-target="#addPickupLocation">+ Pick-up location</a>'
            + '<a class="mt5 fa fa-edit ' + disabled + '" data-toggle="modal" data-target="#addPickupLocation" ' + style + '></a>';
    }
}

function getAddressHtml(formData) {   
    if (formData == null || formData == undefined) {
        return '';
    }
    var html = '<span class="sel-IsPickupLocation hide-element">' + formData.IsPickupLocation + '</span>';
    if (formData.IsPickupLocation == "True" || formData.IsPickupLocation == true) {
        html += '<b>Location:</b> <span class="sel-sitename">' + formData.SiteName + '</span>, ' 
            +  '<span class="sel-address">' + formData.PickupAddress + '</span>, '
            + '<span class="sel-city">' + formData.PickupCity + '</span>, '
            + '<span class="sel-statecode">' + formData.PickupStateCode + '</span>'
            + '<span class="sel-statename hide-element">' + formData.StateName + '</span> '
            + '<span class="sel-zipcode">' + formData.PickupZipCode + '</span> '
            + '<span class="sel-stateid hide-element">' + formData.PickupStateId + '</span>'
            + '<span class="sel-county hide-element">' + formData.PickupCountyName + '</span>'
            + '<span class="sel-countrycode hide-element">' + formData.PickupCountryCode + '</span>'
            + '<span class="sel-countryId hide-element">' + formData.PickupCountryId + '</span>'
            + '<span class="sel-isgeocode hide-element">' + formData.IsPickupGeocodeUsed + '</span>'
            + '<span class="sel-latitude hide-element">' + formData.PickupLatitude + '</span>'
            + '<span class="sel-longitude hide-element">' + formData.PickupLongitude + '</span>'
            + '<span class="sel-timezone hide-element">' + formData.PickupTimeZone + '</span>';
        return html;
    }
    else {
        html += '<b>Terminal:</b> <span class="sel-TerminalId hide-element">' + formData.TerminalId + '</span>'
            + '<span class="sel-TerminalName">' + formData.TerminalName + '</span>';
        return html;
    }
}

function getFormDataAsJson(formSelector) {
    var unindexed_array = $(formSelector).serializeArray();
    var indexed_array = {};
    $.map(unindexed_array, function (n, i) {
        indexed_array[n['name']] = n['value'];
    });
    return indexed_array;
}

function fillEditForm(formId, orderId, scheduleId) {    
    var isCommonLoc = $(formId + ' .btn-applytoall').is(':checked');
    var addressElem = isCommonLoc ? $(formId + ' .dg-common-pickup') :
        $(formId + ' .selected-schedules table tr[data-id="' + scheduleId + '"][data-orderid="' + orderId + '"]');
    var pickupLoc = getPickupLocation(addressElem);
    if (pickupLoc.IsPickupLocation.toLocaleLowerCase() == 'true') {
        $('#IsPickupLocation[value="True"]').click();
        $('#SiteName').val(pickupLoc.SiteName);
        $('#PickupAddress').val(pickupLoc.PickupAddress);
        $('#PickupCity').val(pickupLoc.PickupCity);
        $('#PickupStateId').val(pickupLoc.PickupStateId);
        $('#PickupZipCode').val(pickupLoc.PickupZipCode);
        $('#PickupCountryId').val(pickupLoc.PickupCountryId);
        $('#PickupCountyName').val(pickupLoc.PickupCountyName);
        $('#PickupLatitude').val(pickupLoc.PickupLatitude);
        $('#PickupLongitude').val(pickupLoc.PickupLongitude);
        $('#PickupLongitude').val(pickupLoc.PickupLongitude);
        $('#PickupTimeZone').val(pickupLoc.PickupTimeZone);
        $('#IsPickupGeocodeUsed').val(pickupLoc.IsPickupGeocodeUsed);
    }
    else {
        $('#IsPickupLocation[value="False"]').click();
        $('#TerminalId').val(pickupLoc.TerminalId);
        $('#TerminalName').val(pickupLoc.TerminalName);
    }
}

function clearEditForm() {    
    $('#IsPickupLocation[value="False"]').click();
    $('#SiteName').val('');
    $('#PickupAddress').val('');
    $('#PickupCity').val('');
    $('#PickupStateId').val('');
    $('#PickupZipCode').val('');
    $('#PickupCountryId').val('');
    $('#PickupCountyName').val('');
    $('#PickupLatitude').val('');
    $('#PickupLongitude').val('');
    $('#PickupLongitude').val('');
    $('#PickupTimeZone').val('');
    $('#IsPickupGeocodeUsed').val('');
    $('#TerminalId').val('');
    $('#TerminalName').val('');
}

function setSelectedSchedulesForEdit(response) {
    var schedules = response.TrackableSchedules.filter(function (el) { return el.IsSelected == true; });
	if (response.PickupLocation != null && response.IsCommonForGroup) {
		$('#edg-form .btn-applytoall').prop('checked', response.IsCommonForGroup);
        var html = getAddressHtml(response.PickupLocation);
        $('#edg-form .dg-common-pickup').html(html);
        $('#edg-form .dg-common-pickup').removeClass('pntr-none subSectionOpacity');
        $('input.btn[data-target="#addPickupLocation"]').removeClass('pntr-none subSectionOpacity');
        $('#edg-form .btn-applytoall').removeAttr('disabled');
    }
    for (var idx = 0; idx < schedules.length; idx++) {
        var schedule = schedules[idx];
        var delivery = schedule.Name + '<br/>';
        if (response.PickupLocation != null && response.IsCommonForGroup) {
            delivery += getCommonPickupLocation('#edg-form');
        }
        else {
            var html = getAddressHtml(schedule.PickupLocation);
            delivery += getPickupContainer(html, false);
        }
        var markup = '<tr data-id="' + schedule.Id + '" data-orderid="' + schedule.OrderId + '"><td>' + delivery + '</td></tr>';
        $('#edit-dg-modal .selected-schedules table tbody').append(markup);
    }
}

function onAddScheduleSuccess(data) {  
	if (data.StatusCode == 0) { 
		var isScheduleGridUpdated = true;
		msgsuccess(data.StatusMessage);		
		$('#add-delivery-modal').modal('hide');
		var orderDiv = $('div[data-orderid=' + data.OrderId + ']');
		if (orderDiv.length > 0) {
			var html = getAddressHtml(data.PickupLocation);
			var address = getPickupContainer(html, false, true);
			var tr = '<tr data-future="' + data.IsFutureSchedule + '"><td><input type="checkbox" value="'
				+ data.Id + '" ' + (data.IsSelected ? 'checked="checked"' : '')
				+ ' /></td><td>' + data.Name + '<br/>' + address + '</td><td><a id="btnCancelSchedule"' + data.Id + ' data-deliveryschedulestatusid="5" class="label-danger" onclick="cancelSchedule('+data.OrderId+', '+data.Id+', this);" data-deliveryschedulestatusid="5"> Cancel </a></td></tr>';
			$(orderDiv).find('tbody').append(tr);
			$(orderDiv).find('#message').remove();
		}
	}
	else {
		msgerror(data.StatusMessage);
	}
	$('.loader').hide();
}

function onAddScheduleFail(data) {
	$('.loader').hide();
}

$('#add-delivery-modal').on('hidden.bs.modal', function () {
	$('#newschedule-form').empty();
});

function reloadDataGrid() {
    if (isScheduleGridUpdated) {
        ReloadScheduleGrid();
    }
    else {
        return false;
    }
}

function cancelSchedule(orderId, tsId, element) {
	$('.loader').show();
	var deliveryScheduleStatusId = $(element).attr("data-deliveryScheduleStatusId");
	var url = cancelScheduleUrl + "?orderId=" + orderId + "&trackableScheduleId=" + tsId + "&deliveryScheduleStatusId=" + deliveryScheduleStatusId;
	$.get(url, function (response) {
		if (response.StatusCode == 0) {
			isScheduleGridUpdated = true;
			msgsuccess(response.StatusMessage);
			$(element).closest('tr').remove();
			if ($('.selected-schedules').find('tr[data-id="' + tsId + '"]').length > 0) {
				$('.selected-schedules').find('tr[data-id="' + tsId + '"]').remove();
			}
		}
		else {
			msgerror(response.StatusMessage);
		}
	}).always(function () {
		$('.loader').hide();
	});
}
