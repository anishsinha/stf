$(document).ready(function () {
	$('.datatype-decimal').each(function () { removeZero(this); });
})

function getSurchargeTable(element)
{
	var viewModel = {
		"TableType": $(element).attr('data-tableType'),
		"ProductType": $(element).attr('data-productType'),
		"StartDate": $(element).attr('data-startDate'),
		"EndDate": $(element).attr('data-endDate'),
		"PriceRangeStartValue": $(element).attr('data-startValue').replace('$',''),
		"PriceRangeEndValue": $(element).attr('data-endValue').replace('$', ''),
		"PriceRangeInterval": $(element).attr('data-priceInterval'),
		"FuelSurchargeStartPercentage": $(element).attr('data-startPerc')
	}
	$.ajax({
		type: "post",
		url: viewSurchargeTableUrl,
		data: viewModel,
		datatype: "json",
		cache: false,
		success: function (response) {
			$('#fuel-surcharge-table').html(response);
			if ($('#hdnSupplierId').val() == '')
			{
				$('.span-surcharge-percent').show();
				$('.div-surcharge-percent').hide();
				$('#btn-edit-surcharge').hide();
			}
			else
			{
				$('.span-surcharge-percent').hide();
				$('.div-surcharge-percent').show();
				$('#btn-edit-surcharge').show();
			}
			parseForm();
		},
		error: function (xhr) {
			console('No Valid Data');
		}
	}); 
}

function postEditSurchargeEvent(response)
{
	if (response.StatusCode == "0")
	{
		$('#btn-close-editsurchargemodal').click();
		$('#btnViewSurchargeTable').click();
	}
}

function GetSurchargeDataTable(response) {
	if (typeof response == 'undefined' || response == null) {
		response = [];
	}
	var fuelSurchargeSummary = $('#fuelsurchargesummary-datatable').DataTable({
		data: response,
		columns: [
			{
				"data": function (data, type, row, meta) {
					return '<a data-startDate="' + data.StartDate + '" data-endDate="' + data.EndDate + '" data-tableType="' + data.TableType + '" data-productType="' + data.ProductType + '" data-startValue="' + data.StartValue + '" data-endValue="' + data.EndValue + '" data-priceInterval="' + data.PriceInterval + '" data-startPerc="' + data.SurchargePercentage + '" onclick="getSurchargeTable(this);" data-toggle="modal" data-target="#surcharge-table">' + data.DateRange + '</a>';
				}, "autowidth": true
			},
			{
				"data": function (data, type, row, meta) {
					return data.TableType == 1 ? masterType : customerSpecificType;
				}, "autowidth": true
			},
			{ "data": "ProductType", "autowidth": true },
			{ "data": "StartValue", "autowidth": true },
			{ "data": "EndValue", "autowidth": true },
			{ "data": "PriceInterval", "autowidth": true },
			{ "data": "SurchargePercentage", "autowidth": true }
		],
		pageLength: 10,
		"ordering": false,
		"info": false,
		"searching": true,
		dom: 'lTfgitp',
		"destroy": true,
		"initComplete": function (settings, json) {
			wrapperHeight();
			$('#fuelsurchargesummary-datatable th').addClass('filtering-disabled');
		}
	});
	$('#fuel-surcharge-grid').show();
	$("#btnViewSurchargeTable").removeAttr("disabled");
}