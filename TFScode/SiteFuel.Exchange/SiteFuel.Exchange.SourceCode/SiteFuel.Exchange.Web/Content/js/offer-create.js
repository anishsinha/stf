$(document).ready(function () {
    $(".tiers").on("change", function (e) {
        getSuppplierCustomersList(e);
    });

    $(".pricing-type").each(function (i, element) {
        SetDifferentFuelPriceRackAvgType(element);
    });

    disableElem();

    $(document).on("change", ".offerstate", function (e) {
        var selectedState = $(e.target).closest("#offer-location-list").find(".offerstate").val();
        var cityDropDown = $(e.target).closest("#offer-location-list").find(".offercity");
        var zipDropDown = $(e.target).closest("#offer-location-list").find(".offerzip");
        if (selectedState != "") {
            zipDropDown.empty();
            getCityList(cityDropDown, selectedState);
        }
    });

    $(document).on("change", ".offercity", function (e) {
        var selectedState = $(e.target).closest("#offer-location-list").find(".offerstate :selected").text();
        var cityName = $(e.target).closest("#offer-location-list").find(".offercity :selected").text();
        var target = $(e.target).closest("#offer-location-list").find(".offerzip");
        if (selectedState != "" && cityName != "" && cityName != "Select City") {
            getZipcodeList(target, selectedState, cityName);
        }
    });

    $('.offercity option:selected').each(function (idx, elem) {
        var container = $(elem).closest('#offer-location-list');
        var target = container.find(".offerzip");
        var selectedZips = target.val();
        var stateSelected = container.find('.offerstate :selected ').text();
        var city = $(elem).text();
        if (stateSelected != "" && city != "" && city != "Select City")
            getZipcodeList(target, stateSelected, city, selectedZips);
    });
});

function GoToPreviousURL() {
    window.history.go(-1);
}

function validateOfferTypeData() {
    if (isExclusiveOffer()) {
        if ($('#Tiers').val().length == 0 && $('#Customers').val().length == 0) {
            validationMessageFor($("#Tiers").attr('name'), valMsgTierRequired);
            return false;
        }
        validationMessageFor($("#Tiers").attr('name'), '');
    }

    if (isCityLocation()) {
        var keepRunning = true;
        $(".offerCity").each(function () {
            var cityVal = $(this).val();
            if (cityVal == '') {
                validationMessageFor($(this).attr('name'), valMsgCityRequired);
                keepRunning = false;
            }
            if (keepRunning)
                validationMessageFor($(this).attr('name'), '');
        });
        if (!keepRunning)
            return false;
    }
    return true;
}

function SetDifferentFuelPriceRackAvgType(element) {
    var target = $(element).closest(".partialTier").find(".differentPrice-rackAvgType");
    var selected = $(element).find('option:selected').val();
    if (selected == pricingTypePPG) {
        target.hide();
    }
    else {
        target.show();
    }
}

function getSuppplierCustomersList(e) {
    var selectedTiers = $(e.target).val(); //$('.tiers option:selected').val();
    var target = $("#Customers");
    $.get(getCustomersListUrl, { tiers: selectedTiers.toString() }, function (response) {
        target.empty();
        $.each(response, function (i, element) {
            target.append($('<option></option>').val(element.Id).html(element.Name));
        });
    });
}

function getCityList(target, selectedState) {
    $(".offer-cityZipServiceCall").show();
    $.get(getCityListUrl, { stateId: selectedState }, function (response) {
        target.find('option').not(':first').remove(), $.each(response, function (i, element) {
            target.append($('<option></option>').val(element.Id).html(element.Name));
        });
        $(".offer-cityZipServiceCall").hide();
    });
}

function getZipcodeList(target, selectedState, cityName, selectedZips) {
    $(".offer-cityZipServiceCall").show();
    $.get(getZipListUrl, { stateCode: selectedState, city: cityName }, function (response) {
        target.empty();
        target.append($('<option></option>').val(0).html('Select All'));
        $.each(response, function (i, element) {
            target.append($('<option></option>').val(element.Code).html(element.Name));
        });

        target.val(selectedZips);
        $(".offer-cityZipServiceCall").hide();
    });
}