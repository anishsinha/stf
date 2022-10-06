function showSurchargeTable()
{
	if (customerCompany != '') {
		var surchargeurl = fuelSurchargeLink + '?buyerCompanyId=' + customerCompany+'&isSurchargeTabSelected=true';
		window.open(surchargeurl, '_blank');
	}
}

function ToggleFscControlsByProductTypeId(productId) {
	var url = getProductTypeUrl;
	$.get(url, { 'productId': productId }, function (response) {
		if (response.Code == '1' || response.Code == '5' || response.Code == '6') {
			$('.surcharge-section').show();
		}
		else {
			$('.surcharge-section').hide();
			$('#OrderAdditionalDetailsViewModel_IsFuelSurcharge').prop("checked", false);
		}
	});
}

function hideFscControls() {
	$('.surcharge-section').hide();
	$('#OrderAdditionalDetailsViewModel_IsFuelSurcharge').prop("checked", false);
}

function hidePDITaxControls() {
		$('.pdieTax-confirmation').hide();
	//$('#OrderAdditionalDetailsViewModel_IsPdiTaxRequired').prop("checked", false);
}

function showPDITaxControls() {
	var isSupressPricingEnabled = $('#IsSupressOrderPricing').prop('checked');
	if (!isSupressPricingEnabled)
		$('.pdieTax-confirmation').show();
	//$('#OrderAdditionalDetailsViewModel_IsPdiTaxRequired').prop("checked", false);
}

function addRemoveValidation() {
    var regionId = $("#RegionId").val();
    if ($('input[name="FuelDeliveryDetails.DeliveryTypeId"]:checked').val() == '1') {
        if (!regionId) {
            $('#spnErrorRegionId').html('Dispatch region is mandatory for single delivery');
            $('#spnErrorRegionId').removeClass('hide-element');
        }
        else {
            $('#spnErrorRegionId').html('');
            $('#spnErrorRegionId').addClass('hide-element');
        }
    }
    else {
        $('#spnErrorRegionId').html('');
        $('#spnErrorRegionId').addClass('hide-element');
    }
}

