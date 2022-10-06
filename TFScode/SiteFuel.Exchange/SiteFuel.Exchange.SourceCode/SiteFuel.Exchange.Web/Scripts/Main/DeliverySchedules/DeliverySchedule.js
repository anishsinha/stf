var geocoder = new google.maps.Geocoder();
$(document).on("blur", ".lat,.long", function () {
	var latitude = $(this).closest('.partial-block').find('.lat').val().trim();
	var longitude = $(this).closest('.partial-block').find('.long').val().trim();
	if (latitude == "" || longitude == "")
		return;
	var address = $(this).closest('.partial-block').find(".address");
	var city = $(this).closest('.partial-block').find(".city");
	var county = $(this).closest('.partial-block').find(".county");
	var state = $(this).closest('.partial-block').find(".state");
	var stateName = $(this).closest('.partial-block').find(".statename");
	var stateCode = $(this).closest('.partial-block').find(".statecode");
	var country = $(this).closest('.partial-block').find(".country");
	var countrygroup = $(this).closest('.partial-block').find(".countrygroup");
	var countrycode = $(this).closest('.partial-block').find(".countrycode");
	var countryname = $(this).closest('.partial-block').find(".countryname");
	var zipCode = $(this).closest('.partial-block').find(".zipcode");
	var timeZoneElement = $(this).closest('.partial-block').find('.timeZoneName');
    var latlong = new google.maps.LatLng(latitude, longitude);

	geocoder.geocode({ 'latLng': latlong }, function (results, status) {
		if (status != google.maps.GeocoderStatus.OK || results.length <= 0)
			return;

		var result = results[0];

        var addressVal = "", stateVal = "", countryVal = "";
        var cityVal = '';
		for (var i = 0, len = result.address_components.length; i < len; i++) {
			var ac = result.address_components[i];
			if (ac.types.indexOf("street_number") >= 0)
				addressVal = ac.long_name + " ";
			if (ac.types.indexOf("route") >= 0)
				address.val(addressVal += ac.long_name);
            if (ac.types.indexOf("locality") >= 0) {
                city.val(ac.long_name);
                cityVal = ac.long_name;
            }				
            if (ac.types.indexOf("administrative_area_level_2") >= 0) {
				if (ac.short_name != '' && ac.short_name != undefined && ac.short_name != null) {
					county.val(ac.short_name);
				}
				else if ((cityVal != '') && (cityVal != undefined) && (cityVal != null)) {
                    county.val(cityVal);
                }
            }               			
			if (ac.types.indexOf("administrative_area_level_1") >= 0) {
				state.find("option:contains(" + ac.long_name + ")").prop("selected", true);
				stateVal = ac.long_name;
				stateName.val(ac.long_name);
				stateCode.val(ac.short_name);
			}
			if (ac.types.indexOf("country") >= 0) {
				if (country.find("option:contains(" + ac.short_name + ")").length > 0) {
					country.find("option:contains(" + ac.short_name + ")").prop("selected", true);
					countryVal = ac.short_name;
					countrycode.val(ac.short_name);
					countryname.val(ac.long_name);
                    $(".countrygroup-div").hide();
				} else if (countrygroup.find("option:contains(" + ac.short_name + ")").length > 0) {
					country.find("option:contains(CAR)").prop("selected", true);
					countryVal = "CAR";
					countrycode.val(countryVal); countryname.val("Caribbean");
					countrygroup.find("option:contains(" + ac.short_name + ")").prop("selected", true);
					$(".countrygroup-div").show();
				}
			}
            
			if (ac.types.indexOf("postal_code") >= 0)
				zipCode.val(ac.long_name);
		}
		if (stateVal != "" && countryVal != "") {
			$('.country').trigger('updated', [stateVal, countryVal]);
			$(".state").trigger('StateChanged');
		}
	});

	getTimeZoneFromLatLng(latitude, longitude, timeZoneElement);
});
$(document).on("blur", ".address,.city,.state,.country,.zipcode", function (results, status) {
	var address = $(this).closest('.partial-block').find(".address").val().trim(),
        city = $(this).closest('.partial-block').find(".city").val().trim(),
		zipcode = $(this).closest('.partial-block').find(".zipcode").val().trim(),
		state = $(this).closest('.partial-block').find(".state").find("option:selected").text(),
        country = $(this).closest('.partial-block').find(".country").find("option:selected").text();

    var stateName = $(this).closest('.partial-block').find(".statename");
    var stateCode = $(this).closest('.partial-block').find(".statecode");
    var countrycode = $(this).closest('.partial-block').find(".countrycode");
    var countryname = $(this).closest('.partial-block').find(".countryname");
    var county = $(this).closest('.partial-block').find(".county");
	var cityname = $(this).closest('.partial-block').find(".city");

	var latitude = $(this).closest('.partial-block').find('.lat');
    var longitude = $(this).closest('.partial-block').find('.long');
    //if (latitude.val() == undefined) {
    //    latitude = $(this).closest('.partial-block').find('.latitude');
    //}
    //if (longitude.val() == undefined) {
    //    longitude = $(this).closest('.partial-block').find('.longitude');
    //}
	var timeZoneElement = $(this).closest('.partial-block').find('.timeZoneName');
	if (address == "" || state == "" || country == "" || zipcode == "")
		return;

	address = address + " " + city + " " + state + " " + country + " " + zipcode;
	geocoder.geocode({ 'address': address }, function (gresults, status) {
		if (gresults && gresults.length <= 0)
			return;

		var results = gresults.filter(t => t.address_components.some(t1 => (t1.short_name == zipcode && t1.types.some(t2 => t2 = "postal_code"))));
		if (results.length == 0)
			results = gresults;
		if (status != google.maps.GeocoderStatus.OK || results.length <= 0)
			return;

        var result = results[0].geometry.location;

        for (var i = 0; i < results[0].address_components.length; i++) {
            var component = results[0].address_components[i];
            if (component.types[0] == 'administrative_area_level_2') {
                county.val(component.short_name);
            }
            if (component.types[0] == "administrative_area_level_1") {
                stateName.val(component.long_name);
                stateCode.val(component.short_name);
            }
            if (component.types[0] == "country") {
                countrycode.val(component.short_name); 
                countryname.val(component.long_name);
			}
			if (component.types[0] == "locality") {
				cityname.val(component.short_name);
			}
        }


		var latitudeVal = parseFloat(result.lat()).toFixed(8);
		var longitudeVal = parseFloat(result.lng()).toFixed(8);

		latitude.val(latitudeVal);
		longitude.val(longitudeVal);
		getTimeZoneFromLatLng(latitudeVal, longitudeVal, timeZoneElement);
	});
});

function getTimeZoneFromLatLng(lat, lng, timeZoneElement) {
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
			$(timeZoneElement).val(timeZoneName);
		}
	});
}