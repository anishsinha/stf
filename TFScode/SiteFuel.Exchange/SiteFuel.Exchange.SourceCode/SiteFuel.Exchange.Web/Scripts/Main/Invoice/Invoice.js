var originalName = $("#spnTerminalName").text();
var isSplitInvoice = false;
function GetTerminalDetails(terminalId, fuelId, pricingCodeId, cityTerminalId, currency) {
    var scheduleId = $("#TrackableScheduleId").val();
    if (scheduleId > 0) {
        var date = $("#DeliveryDate").val();
        if (date != undefined && date != null && date != '' && date.length == 10) {
            $('.terminal-loader').show();
            var data = {
                productId: fuelId,
                pricingCodeId: pricingCodeId,
                deliveryDate: date,
                cityGroupTerminalId: cityTerminalId,
                trackableScheduleId: scheduleId,
                currency: currency
            }
            $.post(getTerminalDetailsUrl, data, function (data) {
                if (data.Price < 0) {
                    $("#spnTerminalName").text(originalName);
                }
                else {
                    if (!isNaN(parseFloat(data.Price))) $('#terminalPrice').val(data.Price);
                    $("#spnTerminalName").text(data.Name);
                }
            }).done(function () {
                $('.terminal-loader').hide();
            });
        }
    }
}

function UpdateBulkPlantAddress(orderId, pickupAddress) {
    var scheduleId = $("#TrackableScheduleId").val();
    if (scheduleId > 0) {
        $.get(getBulkplantAddressUrl + '?trackableScheduleId=' + scheduleId + '&orderId=' + orderId, function (data) {
            if (data.Address != null) {
                SetBulkplantAddress(data);
            }
            else {
                SetBulkplantAddress(pickupAddress);
            }
        }).done(function () {
            $('.terminal-loader').hide();
        });
    }
    else {
        SetBulkplantAddress(pickupAddress);
    }
}

function SetBulkplantAddress(address) {
    var isAddressAvailable = $('#PickUpAddress_IsAddressAvailable').is(":checked");
    if (address.IsAddressAvailable == false && isAddressAvailable) {
        $('#PickUpAddress_IsAddressAvailable').trigger('click');
        $('#PickUpAddress_Address').val('');
    }
    else if (address.IsAddressAvailable) {
        if (isAddressAvailable == false) {
            $('#PickUpAddress_IsAddressAvailable').trigger('click');
        }
        $('#PickUpAddress_Address').val(address.Address);
        $('#PickUpAddress_ZipCode').val(address.ZipCode).trigger('change');
    }
    ShowHideBolControls()
}

function ShowHideBolControls() {
    var isbulkPlant = $('#PickUpAddress_IsAddressAvailable').is(":checked");
    if (isbulkPlant) {
        $('.bulkplant-pickup').show();
        $('.terminal-pickup').hide();
    }
    else {
        $('.bulkplant-pickup').hide();
        $('.terminal-pickup').show();
    }
    if ($('#IsBolRequired').val() == 'True') {
        $('#FuelDropped').attr('readonly', 'readonly');
    }
    else
        $('#FuelDropped').removeAttr('readonly');
    if (isSplitInvoice)
        $('#FuelDropped').removeAttr('readonly');
}

function validateBolControls(isValidForm) {

    var isbulkPlant = $('#PickUpAddress_IsAddressAvailable').is(":checked") == true;
    if (isbulkPlant) {
        $('#BolDetails_BolNumber').val('');
    }
    else {
        $('#BolDetails_LiftTicketNumber').val('');
    }
    if (isBolRequired == true) {//isFtl == true ||
        if (isbulkPlant) {
            if ($('#BolDetails_LiftTicketNumber').val() == '') {
                validationMessageFor('BolDetails.LiftTicketNumber', 'Lift Ticket # is required');
                isValidForm = false;
            }
            else {
                validationMessageFor('BolDetails.LiftTicketNumber', '');
            }
            if ($('#BolDetails_LiftDate').val() == '') {
                validationMessageFor('BolDetails.LiftDate', 'Lift Date is required');
                isValidForm = false;
            }
            else {
                validationMessageFor('BolDetails.LiftDate', '');
            }
        }
        else {
            if ($('#BolDetails_BolNumber').val() == '') {
                validationMessageFor('BolDetails.BolNumber', 'BOL # is required');
                isValidForm = false;
            }
            else {
                validationMessageFor('BolDetails.BolNumber', '');
            }
        }
        if ($('#BolDetails_NetQuantity').val() == '') {
            validationMessageFor('BolDetails.NetQuantity', 'Net Quantity is required');
            isValidForm = false;
        }
        else {
            validationMessageFor('BolDetails.NetQuantity', '');
        }
        if ($('#BolDetails_GrossQuantity').val() == '') {
            validationMessageFor('BolDetails.GrossQuantity', 'Gross Quantity is required');
            isValidForm = false;
        }
        else {
            validationMessageFor('BolDetails.GrossQuantity', '');
        }
        if ($('#BolDetails_Carrier').val() == '') {
            validationMessageFor('BolDetails.Carrier', 'Carrier is required');
            isValidForm = false;
        }
        else {
            validationMessageFor('BolDetails.Carrier', '');
        }

        if ($('#BolDetails_LiftTicketNumber').val() != "" || $('#BolDetails_BolNumber').val() != "") {
            if ($("#IsBadgeMandatory").val() == "True" && $('#BolDetails_BadgeNumber').val() == "") {
                validationMessageFor('BolDetails.BadgeNumber', 'Badge # is required');
                isValidForm = false;
            }
        }
    }
    return isValidForm;
}



