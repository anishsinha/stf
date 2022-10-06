$(document).ready(function () {
	$('.datatype-decimal').each(function () { removeZero(this); });
	if ($('#TableType').val() == "1")
	{
		$('#customer-company').hide();
	}
	else
	{
		$('#customer-company').show();
	}
	$('#StartDate').data("DateTimePicker").minDate(moment(surchargeEffectiveDate).startOf('d'));
	$('#TableType').on('change', function () {
		if ($(this).val() == "1")
		{
			$('#customer-company').hide();
		}
		else
		{
			$('#customer-company').show();
		}
	})

	$('#btnGenerateSurchargeTable').on('click', function () {
		if ($('#createFuelSurchargeForm').valid()) {
			var data = $('#createFuelSurchargeForm').serialize();
			$(".surcharge-loader").show();
			$.post(generateTableUrl, data).done(function (response) {
				if (response !== '') {
					$('#surchargeTable').empty();
					$('#surchargeTable').append(response);
					$('#fuel-surcharge-table').show();
				}
			}).always(function () {
				$(".surcharge-loader").hide();
			});
		}
	})

})