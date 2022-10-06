$(document).ready(function () {
    QuantityRangeReady();
});

var maxQuantity = 0; var source = null;
function AjaxStart(addNewElement) {
    source = $(addNewElement);
    var parent = source.closest(".quantity-range");
    var lastMaxInput = parent.find(".max-quantity:last");
    if (lastMaxInput.length > 0) {
        maxQuantity = parseFloat(lastMaxInput.val());
    }
}

$(document).ajaxComplete(function (event, xhr, settings) {
    source = typeof source != 'undefined' ? source : null;
    if (source != null) {
        var parent = source.closest(".quantity-range");
        var lastMinInput = parent.find(".min-quantity:last");
        if (lastMinInput.length > 0 && maxQuantity > 0) {
            lastMinInput.val(maxQuantity), maxQuantity = 0;
        }
        source = null;
        QuantityRangeReady();
    }

});

function QuantityRangeReady() {
    $('.quantity-range').each(function () {
        var $quantity = $(this);
        $(this).on('keyup', '.max-quantity', function () {
            var value = parseFloat($(this).val());
            var parent = $(this).closest('.partial-block').nextAll('.partial-block').eq(0),
                nextMinInput = parent.find('.min-quantity'), nextMaxInput = parent.find('.max-quantity');
            if (nextMinInput.length > 0 && value >= 0) {
                nextMinInput.val(value);
                nextMaxInput.trigger('blur');
            }
        });

        var maxQuantityList = $quantity.find('.max-quantity');
        var lastElement = maxQuantityList.last();
        maxQuantityList.each(function () {
           var currentElement = $(this);
            if (lastElement != undefined && lastElement.is(currentElement)) {
                currentElement.attr('data-val-is-passonnull', 'True');
            }
            else {
                currentElement.removeAttr('data-val-is-passonnull');
            }
        });

        var deleteButtons = $quantity.find("a.remove-partial-block");
        var lastButton = deleteButtons.last();
        deleteButtons.each(function () {
           var currentButton = $(this);
            if (currentButton.is(lastButton))
                currentButton.removeClass("pntr-none subSectionOpacity tier");
            else
                currentButton.addClass("pntr-none subSectionOpacity tier").removeAttr("style");
        });
    });
    parseForm();
};
function enableLastButton(element) {
    var deleteButtons = $(element).closest('.quantity-range').find("a.remove-partial-block");
    var lastButton = $(deleteButtons.length > 1 ? deleteButtons[deleteButtons.length - 2] : deleteButtons.last());
    if (lastButton != undefined) {
        lastButton.removeClass("pntr-none subSectionOpacity tier");
    }
}

function ValidateRangeCoversTotalQuantity(message) {
    var isValid = true;
    var deliveryFeesByQuantity = $('.feesubtype-ddl option[value="3"]:selected');
    if (deliveryFeesByQuantity != undefined && deliveryFeesByQuantity.length > 0) {
        $.each(deliveryFeesByQuantity, function (i, elem) {
            var bFixedQuantity = isFixedQuantity();
            var totalQuantity = bFixedQuantity ? $('.total-gallons-required').first().val() : $('.total-gallons-required').last().val();
            var lastMaxQuantity = $(elem).closest('.fee-types').find('.max-quantity').last().val();

            var totalQuantityNum = parseFloat(totalQuantity), lastMaxQuantityNum = parseFloat(lastMaxQuantity);
            if (!isNaN(totalQuantityNum) && !isNaN(lastMaxQuantityNum) && totalQuantityNum != lastMaxQuantityNum) {
                validationMessageFor($(elem).closest('.fee-type-row').find('.byquantity-fee').find('input[type=hidden]').attr('name').replace(".index", ""), message);
                isValid = false;
            }
            else {
                validationMessageFor($(elem).closest('.fee-type-row').find('.byquantity-fee').find('input[type=hidden]').attr('name').replace(".index", ""), "");
            }
        });
    }
    return isValid;
}

function isFixedQuantity() {
    var bFixedQuantity = $('.quantity-type-id').is(':radio')
        ? $('.quantity-type-id[value="1"]').is(':checked')
        : ($('.quantity-type-id').length == 0 || $('.quantity-type-id').val() == '1');
    return bFixedQuantity;
}

function isQuantityGreaterThanZero(message) {
    var isValid = true;
    var bFixedQuantity = isFixedQuantity();
    var quantityElement = bFixedQuantity ? $('.total-gallons-required').first() : $('.total-gallons-required').last();
    if (parseFloat(quantityElement.val()) <= 0) {
        isValid = false;
        validationMessageFor(quantityElement.attr('name'), message);
        quantityElement.focus();
    }
    return isValid;
}