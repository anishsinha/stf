function ReloadDataTable() {
	var countryId = $('#Country_Id').val();
	var currencyId = $('#Country_Currency').val();

	document.location.href = document.location.pathname + '?' + 'countryId=' + countryId + '&currencyId=' + currencyId;
}

function setPageCountryAndCurrency() {
	var PageCountryId = getQueryString('countryId');
	var PageCurrencyId = getQueryString('currencyId');
	if (PageCountryId == null || PageCurrencyId == null) {
		var selectedCountryAndCurrency = getSelectedCountryAndCurrency();
		PageCountryId = selectedCountryAndCurrency.countryId;
		PageCurrencyId = selectedCountryAndCurrency.currencyType;
	}
}

function ClosePopup() {
	$('.popover').remove();
}

function CloseReassignPopup() {
	if ($(document).find(".confirmation-box:visible").length > 0) {
		$(document).find(".confirmation-box:visible").closest(".ddlist").find(".dd").val(oldDriver);
	}
	$(document).find(".confirmation-box:visible").hide();
}

function toggleLocation(element) {
	if ($(element).is(':checked')) {
		$('#location').show();
		if ($('#splitload-addresses').find('.splitload-address').length == 0) {
			$('#LinkAddLocation').click();
		}
	}
	else {
		$('#location').hide();
	}
}

function getTrackableScheduleDetails(element) {
	var orderId = $(element).attr("data-orderId");
	var trackableScheduleId = $(element).attr("data-trackableScheduleId");
	var deliveryScheduleId = $(element).attr("data-deliveryScheduleId");
	var isFtlOrder = $(element).attr("data-IsFtlOrder");
	var enrouteStatus = $(element).attr("data-EnrouteStatus");
	var countryId = $(element).attr("data-countryId");
	var countryCode = $(element).attr("data-countryCode");
	var currency = $(element).attr("data-currency");
	$("#nearest-terminals").empty();
	var viewModel = { "OrderId": orderId , "TrackableScheduleId":  trackableScheduleId, "DeliveryScheduleId":  deliveryScheduleId , "IsFtlOrder": isFtlOrder, "EnrouteStatus": enrouteStatus , "CountryId": countryId, "CountryCode": countryCode,"Currency":currency }	;
	$.get(getTscheduleUrl, viewModel, function (response) {
		$("#nearest-terminals").html(response);
		initDateTimePicker();
		parseForm();
	}).always(function () {
		$("#TerminalDetails_IsNewLocation:checked").trigger("click");
	});
}

function addScheduleDetails(element) {
	$('.loader').show();
	var orderId = $(element).attr("data-orderId");
	$.get(getOrderDetailsUrl + '?orderId=' + orderId, function (response) {
		$("#newschedule-form").html(response);
		initDateTimePicker();
		parseFormById('#add-delivery-form');
		$("#newschedule-form input[name='TerminalDetails.IsNewLocation']:checked").trigger('click');
	}).always(function () {
		$('.loader').hide();
	});
}

function showLocationUpdateMessage(result) {
	$("#btn-save-pickupterminal").removeAttr("disabled");
	$("#nearest-terminals-modal").modal('hide');
	if (result.StatusCode == 0) {
		msgsuccess(result.StatusMessage);
	}
    else { msgerror(result.StatusMessage); }
}

function showLocationUpdateErrorMessage() {
	$("#btn-save-pickupterminal").removeAttr("disabled");
	$("#nearest-terminals-modal").modal('hide');
	msgerror(errMessagePickupLocationNotUpdated);
}

function autoCompleteTerminal(element, targetUrl, inputData, callback) {
	var target = $(element);
	var prevTerminal = $(element).val();
    var terminalId = inputData.terminalId;
	target.focus(function () {
		target.val('');
		target.autocomplete("search", '  ');
	});
	target.blur(function () {
		target.val(prevTerminal);
        callback(terminalId);
	}).autocomplete({
		search: function (e, u) {
			$(".terminal-loader").show();
		},
		source: function (request, response) {
            inputData["terminal"] = request.term;
			$.ajax({
                url: targetUrl,
				type: "POST",
				dataType: "json",
                data: inputData,
				success: function (data) {
					response($.map(data, function (item) {
						return {
							label: item.Name,
							value: item.Id
						};
					}))
				},
				complete: function (event, xhr, settings) {
					$(".terminal-loader").hide();
				}
			})
		},
		select: function (event, ui) {
            target.val(ui.item.label); // display the selected text
			terminalId = ui.item ? ui.item.value : 0;
			prevTerminal = ui.item ? ui.item.label : prevTerminal;
            if (terminalId != 0)
                callback(terminalId);
			return false;
		},
		messages: {
			noResults: '',
			results: function () { }
		},
		minLength: 2,
		maxShowItems: 10,
		scroll: true
	});
	$.ui.autocomplete.prototype._resizeMenu = function () {
		var ul = this.menu.element;
		ul.outerWidth(this.element.outerWidth());
	}
}

function destoryAutoComplete(element) {
    var $emement = $(element);
    if ($emement.data('autocomplete')) {
        $emement.autocomplete("destroy");
        $emement.removeData('autocomplete');
    }
}

function getbulkplantaddress(element) {
	var siteId = parseInt($(element).closest('.address-container').find('.siteId').val());
    var bulkPlantName = $(element).closest('.address-container').find('.sitename').val();
    var isGeoCoded= $("#TerminalDetails_AddressDetails_IsGeocodeUsed").is(':checked');
    if ((bulkPlantName == null || bulkPlantName == '' || siteId == 0) && !isGeoCoded) {
		clearPickupControls(element);
		return false;
    }
    if (!isGeoCoded) { // Logic inside this If fetches existing bulkplant details by bulkplantname no need of this when address is geocoded
        var tBulkPlantDetailsurl = bulkPlantDetailsurl + '?name=' + bulkPlantName;
        $.ajax({
            type: "GET",
            url: tBulkPlantDetailsurl,
            dataType: "json",
            success: function (data) {
                if (data.ZipCode == null || data.ZipCode == '' || data.ZipCode == undefined) {
                    clearPickupControls(element);
                }
                else {
                    var address = data.Address;
                    var city = data.City;
                    var countyname = data.CountyName;
                    var zipcode = data.ZipCode;
                    var code = data.Country.code;
                    var statecode = data.Code;
					var countryId = data.Country.Id;
					var countryGroupId = data.CountryGroup.Id;
                    var stateId = data.State.Id;
					var siteId = data.SiteId;
					var lat = data.Latitude;
					var lng = data.Longitude;
                    $(element).closest('.address-container').find('.address').val(address);
                    $(element).closest('.address-container').find('.city').val(city);
                    $(element).closest('.address-container').find('.county').val(countyname);
                    $(element).closest('.address-container').find('.zipcode').val(zipcode);
                    $(element).closest('.address-container').find('.country').val(countryId);
                    $(element).closest('.address-container').find('.state').val(stateId);
					$(element).closest('.address-container').find('.siteId').val(siteId);
					$(element).closest('.address-container').find('.latitude').val(lat);
					$(element).closest('.address-container').find('.longitude').val(lng);
					
					if (countryGroupId > 0) {
						$(element).closest('.address-container').find('.countrygroup').val(countryGroupId);
						$(element).closest('.address-container').find('.countrygroup-div').show();
					} else {
						$(element).closest('.address-container').find('.countrygroup-div').hide();
                    }
                    $(".disable-pickup-controls").attr('readonly', true);
                }
            },
            error: function (error) {
                $(".disable-pickup-controls").removeAttr('readonly');
                console.log(error);
            }
        });
    }
	
}

function clearPickupControls(element) {
	$(element).closest('.address-container').find('.address').val('');
	$(element).closest('.address-container').find('.city').val('');
	$(element).closest('.address-container').find('.county').val('');
	$(element).closest('.address-container').find('.zipcode').val('');
	$(element).closest('.address-container').find('.country').val('1');
	$(element).closest('.address-container').find('.countrygroup').val('0');
	$(element).closest('.address-container').find('.state').val('1');
	$(element).closest('.address-container').find('.siteId').val(0);
	$(element).closest('.address-container').find('.countrygroup-div').hide();
    $(".latitude").val('');
    $(".longitude").val('');
    $(".longitude").val('');
    $(".timeZoneName").val('');
	$(".disable-pickup-controls").each(function () {
		$(this).removeAttr('readonly');
	});
}

function autoCompleteTextBoxforAddress(element, url) {
	var target = $(element);
	target.focus(function () {
		$(this).autocomplete("search", $(this).val());
	});
	target.autocomplete({
		source: function (request, response) {
			$.ajax({
				url: url,
				type: "GET",
				dataType: "json",
				data: { Prefix: request.term },
				success: function (data) {
					response($.map(data, function (item) {
						return {
							label: item.Name,
						};

					}))
				}
			});
		},
		messages: {
			noResults: '',
			results: function () { }
		},
		minLength: 0,
		maxShowItems: 5,
		scroll: true,
		select: function (event, ui) {
			$(".disable-pickup-controls").attr('readonly', true);
			var bulkPlantName = ui.item.label;
			var bulkPlantId = ui.item ? ui.item.value : 0;
			$(element).closest('.address-container').find('.siteId').val(bulkPlantId);
			$(element).closest('.address-container').find('.sitename').val(bulkPlantName);
			return false;
		}
	});
	$.ui.autocomplete.prototype._resizeMenu = function () {
		var ul = this.menu.element;
		ul.outerWidth(this.element.outerWidth());
	};
}

function validateScheduleQuantityType(element) {
	var selectedType = $(element).val();
	var container = $('.schedule-qty-container');
	if (selectedType == '1')
		container.show();
	else
		container.hide();

	return false;
}