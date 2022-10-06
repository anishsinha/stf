$(document).ready(function () {
    var createGrpurl = '/Supplier/DeliveryGroup/GetSchedulesForRouteGroup';
    var editGrpUrl = '/Supplier/DeliveryGroup/GetSchedulesForRouteGroupEdit';
    var editTabGrpUrl = '/Supplier/DeliveryGroup/GetTabSchedulesForRouteGroupEdit';

	$(document).on("click", "#create-dg-modal div.bhoechie-tab-menu>div.list-group>a", function (e) {
        e.preventDefault();
        var $this = $(this);
        activateTab($this, '#create-dg-modal');
		var orderId = $this.data('orderid');
		var isRetailJob = $this.data('isretailjob');
		var data = { orderId: orderId, includeSchedules: 3, includeFutureSchedules: null, isRetailJob: isRetailJob };
        getDeliverySchedules('#create-dg-modal form', data, createGrpurl);
    });

    $(document).on('click', '#create-dg-modal .future-deliveries', function () {
        var $this = $(this);
        var orderId = $this.data('orderid');
		var checked = $this.is(':checked');
		var isRetailJob = $this.data('isretailjob');
        var type = checked == true ? 2 : 1;
		var data = { orderId: orderId, includeSchedules: type, includeFutureSchedules: checked, isRetailJob: isRetailJob };
        getDeliverySchedules('#create-dg-modal form', data, createGrpurl);
    });

	$(document).on("click", "#edit-dg-modal div.bhoechie-tab-menu>div.list-group>a", function (e) {
        e.preventDefault();
        var $this = $(this);
        activateTab($this, '#edit-dg-modal');
        var groupId = $('#edit-dg-modal').data('groupid');
		var orderId = $this.data('orderid');
		var isRetailJob = $this.data('isretailjob');
        var checked = $('.bhoechie-tab-content[data-orderid="' + orderId + '"] .checkbox input').is(':checked');
		var data = { orderId: orderId, includeFutureSchedules: checked, deliveryGroupId: groupId, isRetailJob: isRetailJob };
        getEditDeliverySchedules('#edit-dg-modal form', data, editTabGrpUrl);
    });

    $(document).on('click', '#edit-dg-modal .future-deliveries', function () {
        var $this = $(this);
        var groupId = $('#edit-dg-modal').data('groupid');
        var orderId = $this.data('orderid');
		var checked = $this.is(':checked');
		var isRetailJob = $this.data('isretailjob');
		var data = { orderId: orderId, includeFutureSchedules: checked, deliveryGroupId: groupId, isRetailJob: isRetailJob };
        getEditDeliverySchedules('#edit-dg-modal form', data, editTabGrpUrl);
    });

    $(document).on('click', '#create-delivery-group', function (e) {
        var modalId = '#' + $(this).data('target');
        getOrdersForCreateGroup(modalId);
        addValidation(modalId + ' form');
    });
    $(document).on('click', 'a[data-target="edit-dg-modal"]', function (e) {
        var modalId = '#' + $(this).data('target');
        var groupId = $(this).data('groupid');
        $('#edit-dg-modal').data('groupid', groupId);
        getOrdersForEditGroup(modalId);
        addValidation(modalId + ' form');
    });
    $(document).on('click', '.close-panel', function () {
        var modalId = '#' + $(this).closest('.side-panel').attr('id');
        $(modalId + ' .v-tab.list-group').html('');
        $(modalId + ' .bhoechie-tab').html('');
        $('.selected-schedules table tbody').html('');
        $('.selected-schedules table tbody').html('');
        removeValidation(modalId + ' form');
    });
    $(document).on('editTabsLoaded', function () {
        var allTabs = $('#edit-dg-modal form div.list-group>a');
        var groupId = $('#edit-dg-modal').data('groupid');
		var oIds = allTabs.map(function (j, element) { return $(element).data('orderid'); }).get();
		var isRetailJobs = allTabs.map(function (j, element) { return $(element).data('isretailjob'); }).get();
		var data = { orderIds: oIds, includeFutureSchedules: true, deliveryGroupId: groupId, isRetailJobs: isRetailJobs};
        getEditDeliverySchedules('#edit-dg-modal form', data, editGrpUrl, showCommonInfo);
        $('#edit-dg-modal form div.list-group>a:first-child').addClass("active");
    });
    function showCommonInfo(response) {
        $('#edg-form .deliveryGroupDriver').val(response.DriverId);
        $('#edg-form .loadCode').val(response.LoadCode);
        $('#edg-form .routeInfo').val(response.RouteNote);
        setSelectedSchedulesForEdit(response);

    }
    $(document).on('click', '#create-modal-save', function () {
        var form = $('#cdg-form');
        if (form.valid()) {
            createDeliveryGroup();
        }
    });
    $(document).on('click', '#edit-modal-save', function () {
        var form = $('#edg-form');
        if (form.valid()) {
            editDeliveryGroup();
        }
    });
    $(document).on('click', 'a[data-groupid]', function (e) {
        var target = $(this).data('target');
        var groupId = $(this).data('groupid');
        $(target).data('groupid', groupId);
        $(target).modal('show');
    });
    function activateTab($this, modelId) {
        $this.siblings('a.active').removeClass("active");
        $this.addClass("active");
        //var index = $this.index();
        $(modelId + " div.bhoechie-tab>div.bhoechie-tab-content").removeClass("active");
    }
    $(document).on('click', '.btn-applytoall', function () {
        var formId = '#' + $(this).closest('form').attr('id');
        if ($(this).is(':checked')) {
            $(formId + ' .dg-sel-address').addClass('pntr-none subSectionOpacity');
            $(formId + ' a[data-target="#addPickupLocation"]').addClass('pntr-none subSectionOpacity');
            $(formId + ' input.btn[data-target="#addPickupLocation"]').removeClass('pntr-none subSectionOpacity');
        }
        else {
            $(formId + ' .dg-sel-address').removeClass('pntr-none subSectionOpacity');
            $(formId + ' a[data-target="#addPickupLocation"]').removeClass('pntr-none subSectionOpacity');
            $(formId + ' input.btn[data-target="#addPickupLocation"]').addClass('pntr-none subSectionOpacity');
        }
    });
    $(document).on('hidden.bs.modal', '#addPickupLocation', function () {
        destoryAutoComplete('#TerminalName');
    });
    
    function setStateCountry(formData) {
        var selState = $('#PickupStateId').find('option:selected');
        formData['StateName'] = selState.length > 0 ? selState.text() : '';
        var selCountry = $('#PickupCountryId').find('option:selected');
        formData['CountryCode'] = selCountry.length > 0 ? selCountry.text() : '';
    }
    $(document).on('click', '#btn-add-location', function () {
        var isValidAddress = true;
        var isBulkPlantPickUp = $(".dg-bulkplant-pickup:checked").val();
        if (isBulkPlantPickUp == 'True') {
            isValidAddress = validateBulkPlantAddress();
        }
        if (isValidAddress) {
            $('.loader').show();
            if ($('#dg-pickup-form').valid()) {
                var formData = getFormDataAsJson('#dg-pickup-form');
                setStateCountry(formData);
                var addressHtml = getAddressHtml(formData);
                var orderId = $(this).data('orderid');
                if (orderId == 0) {
                    $('.dg-common-pickup').html(addressHtml);
                    var schedules1 = $('.selected-schedules table tr[data-orderid]');
                    schedules1.each(function (idx, elem) {
                        $(elem).find('.dg-sel-address').html(addressHtml);
                        $(elem).find('.dg-sel-address').addClass('pntr-none subSectionOpacity');
                        $(elem).find('a[data-target="#addPickupLocation"]').hide();
                        $(elem).find('a.fa-edit[data-target="#addPickupLocation"]').addClass('pntr-none subSectionOpacity');
                        $(elem).find('a.fa-edit[data-target="#addPickupLocation"]').show();
                    });
                }
                else {
                    var scheduleId = $(this).data('id');
                    var schedules2 = $('.selected-schedules table tr[data-id="' + scheduleId + '"][data-orderid]');
                    schedules2.each(function (idx, elem) {
                        $(elem).find('.dg-sel-address').html(addressHtml);
                        $(elem).find('a[data-target="#addPickupLocation"]').hide();
                        $(elem).find('a.fa-edit[data-target="#addPickupLocation"]').removeClass('pntr-none subSectionOpacity');
                        $(elem).find('a.fa-edit[data-target="#addPickupLocation"]').show();
                    });
                }
                $('#addPickupLocation').modal('hide');
            }
            $('.loader').hide();
        }		
    });

    $(document).on('click', 'a[data-target="#addPickupLocation"]', function (e) {
        var orderId = $(this).closest('tr').data('orderid');
        var scheduleId = $(this).closest('tr').data('id');
        var orders = []; orders.push(orderId);
        var inputData = { OrderList: orders };
        if (orders.length > 0) {
            autoCompleteTerminal('#TerminalName', deliveryGroupTerminalsUrl, inputData,
                function (terminalId) { if (terminalId != undefined || terminalId != null) $('#TerminalId').val(terminalId); });
        }
        $('#btn-add-location').data('orderid', orderId);
        $('#btn-add-location').data('id', scheduleId);
    });

    $(document).on('click', 'input.btn[data-target="#addPickupLocation"]', function (e) {
        var orders = [];
        var schedules = $('.selected-schedules table').find('tr[data-orderid]');
        schedules.each(function (idx, elem) { orders.push($(elem).data('orderid')); });
        var inputData = { OrderList: orders };
        if (orders.length > 0) {
            autoCompleteTerminal('#TerminalName', deliveryGroupTerminalsUrl, inputData,
                function (terminalId) { if (terminalId != undefined || terminalId != null) $('#TerminalId').val(terminalId); });
        }
        $('#btn-add-location').data('orderid', 0);
        $('#btn-add-location').data('id', 0);
    });

    $(document).on('click', 'a.fa-edit[data-target="#addPickupLocation"], input.btn[data-target="#addPickupLocation"]', function (e) {
        var formId = '#' + $(this).closest('form').attr('id');
        var orderId = $(this).closest('tr').data('orderid');
        var scheduleId = $(this).closest('tr').data('id');
        fillEditForm(formId, orderId, scheduleId);
    });

    $(document).on('hidden.bs.modal', '#addPickupLocation', function () {
        clearEditForm();
    });
});

$(document).on('keyup', '#search', function () {
    var text = $(this).val();
    $('.list-group-item').hide();

    var textSplits = text.split(" ");
    if (textSplits.length > 1) {
        textSplits.forEach(function (ele, idx) {
            if (ele != "") {
                $('.list-group-item:contains("' + ele + '")').show();
            }
        });
    }
    else {
        if (text == "" || text == " ") {
            $('.list-group-item').show();
        }
        else {
            $('.list-group-item:contains("' + text + '")').show();
        }
    }

    if ($('.list-group-item.active').is(':visible') == false) {
        $('.list-group-item.active').removeClass('active');
    }
    if ($('.list-group-item:visible').length == 1) {
        $('.list-group-item:visible').click();
    }
});

$.expr[":"].contains = $.expr.createPseudo(function (arg) {
	return function (elem) {
		return $(elem).text().toUpperCase().indexOf(arg.toUpperCase()) >= 0;
	};
});

function validateBulkPlantAddress() {
    var isValid = true;
    var countryId = parseInt($('#PickupCountryId').val());
    var stateId = parseInt($("#PickupStateId").val());
    validationMessageFor('PickupStateId', '');
    validationMessageFor('PickupAddress', '');
    validationMessageFor('PickupCity', '');
    validationMessageFor('PickupZipCode', '');

    if (countryId == 4) {
        if (isNaN(stateId) || stateId <= 0) {
            validationMessageFor('PickupStateId', 'State is required');
            isValid = false;
        }
    }
    else {
        var city = $.trim($("#PickupCity").val());
        var zipCode = $.trim($("#PickupZipCode").val());
        var address = $.trim($("#PickupAddress").val());

        if (address == '') {
            validationMessageFor('PickupAddress', 'Address is required');
            isValid = false;
        }
        if (city == '') {
            validationMessageFor('PickupCity', 'City is required');
            isValid = false;
        }
        if (isNaN(stateId) || stateId <= 0) {
            validationMessageFor('PickupStateId', 'State is required');
            isValid = false;
        }
        if (zipCode == '') {
            validationMessageFor('PickupZipCode', 'ZipCode is required');
            isValid = false;
        }
    }
    return isValid;
}