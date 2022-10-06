var geocoder = new google.maps.Geocoder();
$(document).on("blur", ".latitude,.longitude", function () {
    var ele = $(this);
    var latitude = ele.closest('.address-container').find(".latitude").val().trim(),
        longitude = ele.closest('.address-container').find(".longitude").val().trim();
    if (latitude == "" || longitude == "")
        return;

    var latlong = new google.maps.LatLng(latitude, longitude);

    geocoder.geocode({ 'latLng': latlong }, function (results, status) {
        if (status != google.maps.GeocoderStatus.OK || results.length <= 0)
            return;

        var result = results[0];
        var faddress = ele.closest('.address-container').find(".address").val().trim(),
            fcity = ele.closest('.address-container').find(".city").val().trim(),
            fzipcode = ele.closest('.address-container').find(".zipcode").val().trim(),
            fstate = ele.closest('.address-container').find(".state").find("option:selected").text(),
            fcountry = ele.closest('.address-container').find(".country").find("option:selected").text();

        if (faddress == "" || fcity == "" || fstate == "" || fcountry == "" || fzipcode == "") {
            updateAddressData(result, ele, false);
        }
        else {
            bootbox.confirm({
                message: "Geo Codes shifted to a new location!",
                closeButton: false,
                buttons: {
                    cancel: {
                        label: "Keep existing Address",
                        className: "btn-sm btn-warning"
                    },
                    confirm: {
                        label: "Update new Address",
                        className: "btn-sm btn-primary"
                    }
                },
                callback: function (res) {
                    if (res) {
                        updateAddressData(result, ele, true);
                    }
                }
            });
        }
    });

    getTimeZoneUsingLatLng(latitude, longitude);
    updateMapData();
});

$(document).on("blur", ".address,.city,.state,.country,.zipcode", function (results, status) {
    var ele = $(this);
    var address = ele.closest('.address-container').find(".address").val().trim(),
        city = ele.closest('.address-container').find(".city").val().trim(),
        zipcode = ele.closest('.address-container').find(".zipcode").val().trim(),
        state = ele.closest('.address-container').find(".state").find("option:selected").html(),
        country = ele.closest('.address-container').find(".country").find("option:selected").html();
        let countrygroup = ele.closest('.address-container').find(".countrygroup").find("option:selected").html();

    var eCounty = ele.closest('.address-container').find(".county");
    var estatename = ele.closest('.address-container').find(".statename");
    var estatecode = ele.closest('.address-container').find(".statecode");
    var ecountrycode = ele.closest('.address-container').find(".countrycode");
    var ecountryname = ele.closest('.address-container').find(".countryname");
    var ecity = ele.closest('.address-container').find(".city");
    if (address == "" || state == "" || country == "" || zipcode == "")
        return;
    if (country == "CAR") {
        address = state + ", Caribbean";
    } else {
        address = address + " " + city + " " + state + " " + country + " " + zipcode;
    }
    geocoder.geocode({ 'address': address }, function (gresults, status) {
        var results = gresults.filter(t => t.address_components.some(t1 => (t1.short_name == zipcode && t1.types.some(t2 => t2 = "postal_code"))));

        if (results.length == 0)
            results = gresults;

        if (status != google.maps.GeocoderStatus.OK || results.length <= 0)
            return;

        var result = results[0].geometry.location;
        for (var i = 0; i < results[0].address_components.length; i++) {
            var component = results[0].address_components[i];
            if (component.types[0] == 'administrative_area_level_2') {
                eCounty.val(component.short_name);
            }
            if (component.types[0] == "administrative_area_level_1") {
                if (country == "CAR") {
                    let stateName = getStateName(countrygroup, component.long_name);
                    estatename.val(stateName); estatecode.val(stateName);
                } else {
                    estatename.val(component.long_name); estatecode.val(component.short_name);
                }
            }
            if (component.types[0] == "country") {
                ecountrycode.val(component.short_name); ecountryname.val(component.long_name);
            }
            if (component.types[0] == "locality") {
                ecity.val(component.short_name);
            }
        }

        var latitude = parseFloat(result.lat()).toFixed(8);
        var longitude = parseFloat(result.lng()).toFixed(8);

        $(".latitude").val(latitude);
        $(".longitude").val(longitude);
        $(".latitude").trigger('change');
        getTimeZoneUsingLatLng(latitude, longitude);
        updateMapData();
    });
});

function getTimeZoneUsingLatLng(lat, lng) {
    var times_Stamp = (Math.round((new Date().getTime()) / 1000)).toString();
    $.ajax({
        url: "https://maps.googleapis.com/maps/api/timezone/json?location=" + lat + "," + lng + "&timestamp=" + times_Stamp + "&key=" + mapsApiKey,
        cache: false,
        type: "POST",
        async: false,
    }).done(function (response) {

        if (response.timeZoneId != null) {
            var timeZoneName = response.timeZoneName;
            timeZoneName = ParseTimeZone(timeZoneName);
            $(".timeZoneName").val(timeZoneName);
        }
    });
}

function GetLatLong(state, country) {
    var address = state + " " + country;
    geocoder.geocode({ 'address': address }, function (results, status) {
        if (status != google.maps.GeocoderStatus.OK || results.length <= 0)
            return;

        var result = results[0].geometry.location;
        var latitude = parseFloat(result.lat()).toFixed(8);
        var longitude = parseFloat(result.lng()).toFixed(8);

        $(".latitude").val(latitude);
        $(".longitude").val(longitude);
        getTimeZoneUsingLatLng(latitude, longitude);
    });
}

function updateMapData() {
    var map = $('.map-controls');
    if (map != undefined && map.length == 1) {
        refreshJobMap();
    }
}

function getStateName(countryVal, stateVal) {
    if (countryVal == "Anguilla") {
        return countryVal;
    }
    return stateVal;
}

function getCountryName(countryVal) {
    if (countryVal == "CA" || countryVal == "CAN") {
        return 'CAN';
    } else if (countryVal == "US" || countryVal == "USA") {
        return 'USA';
    } else {
        return "CAR";
    }
}
function updateAddressData(result, element, isLocationChange) {
    var address = "", city = "", state = "", country = "", countrygroup = "";
    var ecounty = element.closest('.address-container').find(".county");
    var estate = element.closest('.address-container').find(".state");
    var estatename = element.closest('.address-container').find(".statename");
    var estatecode = element.closest('.address-container').find(".statecode");
    var ecountry = element.closest('.address-container').find(".country");
    var ecountrycode = element.closest('.address-container').find(".countrycode");
    var ecountryname = element.closest('.address-container').find(".countryname");
    var ecountrygroupdiv = element.closest('.address-container').find(".countrygroup-div");
    var ecountrygroup = element.closest('.address-container').find(".countrygroup");
    var ezipcode = element.closest('.address-container').find(".zipcode");
    for (var i = 0, len = result.address_components.length; i < len; i++) {
        var ac = result.address_components[i];
        if (ac.types.indexOf("street_number") >= 0) address = ac.long_name + " ";
        if (ac.types.indexOf("route") >= 0) address += ac.long_name;
        if (ac.types.indexOf("locality") >= 0) $(".city").val(ac.long_name);
        if (ac.types.indexOf("administrative_area_level_2") >= 0) ecounty.val(ac.short_name);
        if (ac.types.indexOf("administrative_area_level_1") >= 0) {
            state = ac.long_name;
            estate.find("option:contains(" + ac.long_name + ")").prop("selected", true);
            estatename.val(ac.long_name);
            estatecode.val(ac.short_name);
        }
        if (ac.types.indexOf("country") >= 0) {
            var cntryVal = getCountryName(ac.short_name);
            country = cntryVal;
            ecountry.find("option:contains(" + cntryVal + ")").prop("selected", true); country = cntryVal;
            ecountrycode.val(cntryVal);

            if (cntryVal == "CAR") {
                ecountrygroup.find("option:contains(" + ac.long_name + ")").prop("selected", true);
                countrygroup = ac.long_name;
                ecountryname.val("Caribbean");
                ecountrygroupdiv.show();
            } else {
                ecountryname.val(ac.long_name);
                if (ecountrygroupdiv.length > 0)
                    ecountrygroupdiv.hide();
            }
        }
        
        if (ac.types.indexOf("postal_code") >= 0) ezipcode.val(ac.long_name);
    }

    if ($(".address").val().trim() == "" || isLocationChange) {
        if (address != "") {
            $(".address").val(address);
        }
        else if (result.formatted_address != "") {
            $(".address").val(result.formatted_address.split(",", 1)[0]);
        } else {
            $(".address").val("Unnamed");
        }
    }
    if (country == "CAR") {
        let stateName = getStateName(countrygroup, state);
        state = stateName;
        estate.find("option:contains(" + stateName + ")").prop("selected", true);
        estatename.val(stateName);
        estatecode.val(stateName);
    } 
    if (state != "" && country != "") {
        element.closest('.address-container').find('.country').trigger('updated', [state, country]);
        element.closest('.address-container').find(".state").trigger('StateChanged');
    }
}
