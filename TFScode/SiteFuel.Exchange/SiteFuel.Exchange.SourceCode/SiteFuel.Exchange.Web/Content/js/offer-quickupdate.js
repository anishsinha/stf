function reloadOfferFueltype() {
    var target = $("#FuelTypeId");
    var selectedFuelType = target.val();
    target.empty();
    target.prepend($('<option></option>').val(0).html("Select Fuel Type"));
    reloadDropdownItem(offerFueltypeurl, target, selectedFuelType);
}

function reloadOfferStates() {
    var data = getSelectedFilterValues();
    var target = $("#States");
    var selectedStates = target.val();
    target.empty();
    $.get(offerStatesurl, data, function (response) {
        $.each(response, function (i, element) {
            if (selectedStates != '[]' && selectedStates.indexOf(element.Id.toString()) > -1)
                target.append($('<option selected></option>').val(element.Id).html(element.Name));
            else
                target.append($('<option></option>').val(element.Id).html(element.Name));
        });
    });
    loadFeesOrPicing();
}

function reloadOfferCities() {
    var zipDropDown = $("#ZipStringList");
    var target = $("#Cities");
    target.empty();
    target.prepend($('<option></option>'));
    if (getSelectedStates().length == 1) {
        target.prop('disabled', false);
        zipDropDown.prop('disabled', false);
        var selectedCity = target.val();
        reloadDropdownItem(offerCitiesurl, target, selectedCity);
    }
    else {
        zipDropDown.empty();
        target.prop('disabled', true);
        zipDropDown.prop('disabled', true);
    }
    loadFeesOrPicing();
}

function reloadOfferZips() {
    var data = getSelectedFilterValues();
    var target = $("#ZipStringList");
    var selectedZips = target.val();
    target.empty();
    if (data.CityId > 0) {
        $.post(offerZipsurl, data, function (response) {
            $.each(response, function (i, element) {
                if (selectedZips != '[]' && selectedZips.indexOf(element.Code) > -1)
                    target.append($('<option selected></option>').val(element.Code).html(element.Name));
                else
                    target.append($('<option></option>').val(element.Code).html(element.Name));
            });
        });
    }
    loadFeesOrPicing();
}

function reloadOfferFeeTypes() {
    var target = $("#FeeTypeId");
    var selectedFees = target.val();
    target.empty();
    target.prepend($('<option value="">Select Fee</option>'));
    reloadDropdownItem(offerFeeTypeurl, target, selectedFees);
}

function reloadOfferFeeSubTypes() {
    var target = $("#FeeSubTypeId");
    var feeTypeId = getFeeTypeId();
    if (feeTypeId == undefined || feeTypeId == '' || feeTypeId == null) {
        target.prop('disabled', true);
        target.find('option').not(':first').remove();
    }
    else {
        target.prop('disabled', false);
        var selectedFeeSubType = target.val();
        target.empty();
        target.prepend($('<option value="">Select Sub Type</option>'));
        reloadDropdownItem(offerFeeSubTypeurl, target, selectedFeeSubType);
    }
}

function reloadOfferPricingTypes() {
    var target = $("#PricingTypeId");
    var selectedPricing = target.val();
    target.empty();
    target.prepend($('<option value="">Select Pricing Type</option>'));
    reloadDropdownItem(offerPricingTypeurl, target, selectedPricing);
}

function reloadDropdownItem(url, target, selectedValue) {
    var data = getSelectedFilterValues();
    $.get(url, data, function (response) {
        $.each(response, function (i, element) {
            if (selectedValue != '' && element.Id == selectedValue)
                target.append($('<option selected></option>').val(element.Id).html(element.Name));
            else
                target.append($('<option></option>').val(element.Id).html(element.Name));
        });
    });
}
function loadFeesOrPicing() {
    if (getQuickUpdateType() == 2) {
        reloadOfferFeeTypes();
        $("#PricingTypeId").val("");
    }
    else {
        reloadOfferPricingTypes();
        $("#FeeTypeId").val("");
        $("#FeeSubTypeId").val("");
    }
}

function getSelectedFilterValues() {
    var offerTypeId = $("#OfferTypeId:checked").val();
    var selectedCustomers = [];
    var selectedTiers = [];
    if (offerTypeId == 1) {
        selectedCustomers = $("#select-customers").val();
        selectedTiers = $("#Tiers").val();
    }
    var data = {
        OfferTypeId: offerTypeId,
        FuelTypeId: $("#FuelTypeId").val(),
        StateId: JSON.stringify(getSelectedStates()),
        Customers: JSON.stringify(selectedCustomers),
        Tiers: JSON.stringify(selectedTiers),
        Zips: JSON.stringify($("#ZipStringList").val()),
        QuickUpdateType: getQuickUpdateType(),
        CityId: $("#Cities").val(),
        StateName: $("#States :selected").text(),
        CityName: $("#Cities :selected").text(),
        FeeTypeId: getFeeTypeId(),
        Country: selectedOfferCoutnry,
        CurrencyType: selectedOfferCurrency,
        TruckLoadType: $('select.truckLoadTypes option:selected').val(),
        PricingSource: $('select.ddl-pricing-source option:selected').val()
    };
    return data;
}
function getSelectedStates() {
    return $("#States").val();
}
function getFeeTypeId() {
    return $("#FeeTypeId").val();
}
function getQuickUpdateType() {
    return $("#QuickUpdatePreferenceSetting_QuickUpdateType:checked").val();
}

function ShowOfferFees(offerId) {
    var url = offerFeesUrl + '?OfferPricingId=' + offerId;
    $.get(url).done(function (response) {
        if (response != null) {
            $("#offer-fees").html(response);
            $(".offer-heading").html(feeHeading);
            $("#modal-offer-fees").modal("show");
        }
    });
}

function ShowOfferPricing(offerPricingId) {
    var url = offerPricingUrl + '?OfferPricingId=' + offerPricingId;
    $.get(url).done(function (response) {
        if (response != null) {
            $("#offer-fees").html(response);
            $(".offer-heading").html(priceHeading);
            $("#modal-offer-fees").modal("show");
        }
    });
}
function showPricingPrefSuccessMessage(response) {
    if (response.StatusCode == successCode) { 
        $('#modal-view-pricing-table-pref-setting').modal("hide");
        msgsuccess(offerPrefSuccessMessage);
        $('#tab-quickupdate').trigger("click");
    }
    else {
        msgerror(response.StatusMessage);
    }
}

function showPricingPrefFailedMessage(response) {
    msgerror(response.StatusText);
}

function showQuickUpdateSuccess(response) {
    if (response.StatusCode == successCode) {
        msgsuccess(response.StatusMessage);
        $("#tab-quickupdate").trigger("click");
    }
    else {
        msgerror(response.StatusMessage);
        $("#quick-update-submit").attr("disabled", false);
    }
}
