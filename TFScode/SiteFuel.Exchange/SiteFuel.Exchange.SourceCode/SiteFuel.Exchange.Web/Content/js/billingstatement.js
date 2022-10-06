$(document).ready(function () {
    $(".billing-buyer-company").on("change", function (e) {
        GetBuyerOrders(e);
    });

    $(".billing-country").on("change", function (e) {
        RefreshTimeZoneDropDown(e);
        RefreshBuyerDropDown(e);
        RefreshOrderDropDown(e);
    });
    SetWeekDayDropDown();
    //SetFrequencyValue();
});

function RefreshOrderDropDown(e) {
    var target = $('.billing-buyer-orders');
        target.empty();
}

function RefreshBuyerDropDown(e) {
    var selectedCountryId = $(e.target).val();
    var target = $('.billing-buyer-company');
    var firstOption = target.find('option:first');
    $.get(getBuyersListUrl, { countryId: selectedCountryId }, function (response) {
        target.empty();
        target.append(firstOption);
        $.each(response, function (i, element) {
            target.append($('<option></option>').val(element.Id).html(element.Name));
        });
    });
}

function RefreshTimeZoneDropDown(e) {
    var selectedCountryId = $(e.target).val();
    var target = $('.billing-timezone');
    $.get(getTimeZoneListUrl, { countryId: selectedCountryId }, function (response) {
        target.empty();
        $.each(response, function (i, element) {
            target.append($('<option></option>').val(element.Code).html(element.Name));
        });
    });
}

function SetFrequencyValue() {
    var currentValue = $('.billing-updatefrequencyvalue').val();
    if (currentValue == "0") {
        $('.billing-updatefrequencyvalue').val('');
    }
}

function GetBuyerOrders(e) {
    var selectedCompanyId = $(e.target).val(); //$('.tiers option:selected').val();
    var target = $('.billing-buyer-orders');
    var selectedCountryId = $('.billing-country option:selected').val();
    if (typeof selectedCountryId === "undefined")
        selectedCountryId = defaultCountryId;
    $.get(getBuyerOrdersListUrl, { companyId: selectedCompanyId, countryId: selectedCountryId }, function (response) {
        target.empty();
        $.each(response, function (i, element) {
            target.append($('<option></option>').val(element.Id).html(element.Name));
        });
    });
}

function GoToPreviousURL() {
    window.history.go(-1);
}

function hideWeekDayDropDown(e) {
    var selectedFrequencyId = $(e).find('option:selected').val();
    if (selectedFrequencyId == dailyFrequencyTypeId || selectedFrequencyId == monthlyFrequencyTypeId) {
        $('.billing-weekday-div').hide();
    }
    else {
        $('.billing-weekday-div').show();
    }
}

function SetWeekDayDropDown() {
    if (existingFrequencyTypeId == dailyFrequencyTypeId || existingFrequencyTypeId == monthlyFrequencyTypeId) {
        $('.billing-weekday-div').hide();
    }
    else {
        $('.billing-weekday-div').show();
    }
}

function validateUpdateFrequencyData() {
    var frequencyValue = $('.billing-updatefrequencyvalue').val();
    var frequencyType = $('.billing-updatefrequencytype').val();
    if (frequencyValue != "") {
        if (frequencyType == hourUpdateFrequencyTypeId) {
            if (frequencyValue > 24) {
                validationMessageFor($("#UpdateFrequencyValue").attr('name'), valMsgHourUpdateFreqValueIncorrect);
                return false;
            }
            validationMessageFor($("#UpdateFrequencyValue").attr('name'), '');
        }

        if (frequencyType == dayUpdateFrequencyTypeId) {
            if (frequencyValue > 30) {
                validationMessageFor($("#UpdateFrequencyValue").attr('name'), valMsgDayUpdateFreqValueIncorrect);
                return false;
            }
            validationMessageFor($("#UpdateFrequencyValue").attr('name'), '');
        }
    }
    else {
        validationMessageFor($("#UpdateFrequencyValue").attr('name'), valUpdateFreqValueisRequired);
        return false;
    }

    return true;
}