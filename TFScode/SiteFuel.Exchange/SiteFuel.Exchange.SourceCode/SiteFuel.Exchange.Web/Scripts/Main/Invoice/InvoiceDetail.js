$('[data-toggle=confirmation]').confirmation({
	rootSelector: '[data-toggle=confirmation]',
	html: true
});

SetPageCulture(culture);

$(document).ready(function () {
	if (isPendingAdj == 'True') {
		$(".badge-new").show();
	}
	else {
		$(".badge-new").hide();
	}

	var invoiceType = applicationQueryTypeInvoice;
	if (invoiceTypeId == 6 || invoiceTypeId == 7) {
		invoiceType = applicationQueryTypeDdt;
	}
	SetQuickMessageURL(invoiceType, invoiceId);
	thumbnailPreview();
	$(".download-images").click(function () {
		hideLoader();
	});
});

function editInvoiceNumber() {
	var invNumber = $('#Invoice_DisplayInvoiceNumber').val();
	if ($.trim(invNumber) == '') {
		validationMessageFor("Invoice.DisplayInvoiceNumber", valMessageInvoiceNumber);
		return false;
	}

	var targetUrl = editInvoiceNumberUrl + escape(invNumber);
	$("#invoice-number-edit-modal").find('.loading-wrapper').show();
	$.post(targetUrl, function (response) {
		if (response.StatusCode == 0) { // 0=Success, 1=Failed
			msgsuccess(response.StatusMessage);
			var newInvoiceId = response.EntityId;
			if ($("#sliderpanel").is(":visible")) {
				closeSlidePanel();
				if (supplierinvoice) {
					supplierinvoice.ajax.reload();
				}
				slideInvoiceDetails(newInvoiceId, '');
			}
			else {
				window.location.href = "/Supplier/Invoice/Details/" + newInvoiceId;
			}
		}
		else {
			msgerror(response.StatusMessage);
		}
	}).always(function () { $("#invoice-number-edit-modal").find('.loading-wrapper').hide(); });
}

function toggleEditWindow(elementClassName) {
	$('.' + elementClassName).toggle();
	var isVisible = $('.' + elementClassName).is(':visible');

	if (isVisible === true) {
		// element is Visible
		validationMessageFor("Invoice.DisplayInvoiceNumber", '');
		$('#Invoice_DisplayInvoiceNumber').val($('#lblInvoiceNumber').text());
	}
}

function uploadDtnFile() {
	var invNumber = $('#Invoice_DisplayInvoiceNumber').val();
	var uploadUrl = uploadDtnUrl + (escape(invNumber));
	$("#invoice-number-edit-modal").find('.loading-wrapper').show();
	$.get(uploadUrl, function (response) {
		if (response.StatusCode != 0) { // 0=Success, 1=Failed
			msgerror(response.StatusMessage);
		}
		else {
			msgsuccess(response.StatusMessage);
		}
	}).always(function () { $("#invoice-number-edit-modal").find('.loading-wrapper').hide(); });
}

function downloadInvoiceFile(ele, fileName) {
    if (fileName.indexOf(',') > -1) {
        var fileNameDetails = fileName.split(',');
        for (var i = 0; i < fileNameDetails.length; i++) {
            var url = downloadInvoiceUrl + fileNameDetails[i];
            window.open(url, "_blank");
        }
    }
    else {
	    var url = downloadInvoiceUrl + fileName;
        window.open(url, "_blank");
    }
}

function creditInvoice(targetUrl) {
	$("#ajax-loading").show();
	$.post(targetUrl, function (response) {
		if (response.StatusCode == 0) {
			msgsuccess(response.StatusMessage);
			var newInvoiceId = response.EntityId;
			if ($("#sliderpanel").is(":visible")) {
				closeSlidePanel();
				supplierinvoice.ajax.reload();
				slideInvoiceDetails(newInvoiceId, '');
			}
			else {
				window.location.href = "/Supplier/Invoice/Details/" + newInvoiceId;
			}
		}
		else {
			msgerror(response.StatusMessage);
		}
	}).always(function () { $("#ajax-loading").hide(); });
}

function onSaveDiscountSuccess(response) {
	msgsuccess(response.StatusMessage);
	closeSlidePanel();
	slideInvoiceDetails(invoiceId);
}

function onSaveDiscountFail(response) {
	msgerror(response.StatusMessage);
}


function createInvoiceFromDdt(element)
{
	$("#ajax-loading").show();
	$('#btnCreateInvoiceFromDDT').addClass("pointer_null");
	var isValidForm = $(element).closest('form').valid();
	var submitUrl = $(element).closest('form').attr("action");
	var data = $(element).closest('form').serialize();
	if (isValidForm) {
		$.post(submitUrl, data, function (response) {			
			if (response.StatusCode == 0) {
				msgsuccess(response.StatusMessage);
				var newInvoiceId = response.EntityId;
				if ($("#sliderpanel").is(":visible")) {
					closeSlidePanel();
					supplierDdt.ajax.reload();
					slideInvoiceDetails(newInvoiceId, '');
				}
				else {
					window.location.href = "/Supplier/Invoice/Details/" + newInvoiceId;
				}
			}
			else {
				msgerror(response.StatusMessage);
			}
		}).always(function () {
			$("#ajax-loading").hide();
			$('#btnCreateInvoiceFromDDT').removeClass("pointer_null");
		});
	}
}

function downloadInvFiles(fileUrl) {
    var tag = document.createElement('a'); // create <a> tag
    tag.href = fileUrl;
    tag.download = fileUrl;
    document.body.appendChild(tag);
    tag.click();
    document.body.removeChild(tag); // remove <a> tag from DOM after click event
    hideLoader();
}
