
$(document).ready(function () {
    partialDocumentReady();
    RemoveCheck();
});

function partialDocumentReady() {

    $(document).on('click', '.add-partial-block', function () {
        var current = $(this), url = current.attr("data-url");
        $.get(url, function (response) {
            var parentId = current.closest('.partial-section').attr('id');
            $("#" + parentId + " > .partial-block-collection-section").append(response);

            var child = $("#" + parentId + " > .partial-block-collection-section").find("div.partial-block:last-child");
            child.find("input[type='text']:not([disabled]):not([readonly])").each(function () {
                if ($(this).val() == '0') $(this).val('');
            });

            parseForm();
            documentReady(true);
            enableSubmit();
        });
    });

    $(document).on('click', '.add-tier-pricing-partial-block', function () {
        var current = $(this), url = current.attr("data-url");
        $.get(url, function (response) {
            var parentId = current.closest('.partial-section').attr('id');
            var aboveQtySection = $("#" + parentId + " > .partial-block-collection-section").find('.above-qty-section');
            if (aboveQtySection.length > 0)
                aboveQtySection.closest('.partial-block').before(response);
            else
                $("#" + parentId + " > .partial-block-collection-section").append(response);

            var child = $("#" + parentId + " > .partial-block-collection-section").find("div.partial-block:last-child");
            child.find("input[type='text']:not([disabled]):not([readonly])").each(function () {
                if ($(this).val() == '0') $(this).val('');
            });

            parseForm();
            documentReady(true);
            enableSubmit();
            hideAddButtonPrevRow($(this));
        });
    });
}

function removePartial(element) {
    $(element).closest('.partial-block').prev("input[type='hidden']").remove();
    $(element).closest('.partial-block').remove();
    enableSubmit();
}

function RemoveCheck(element) {
   var SelectedDays = $('#JobSiteAvailability .DeliveryDays-ddl').length;
    if (SelectedDays < 7) {
        $("#AddAnother").show();
    } else {
        $("#AddAnother").hide();
    }
};


function enableSubmit() {
    var numItems = $('.partial-block-collection-section > .partial-block').length;
    if (numItems > 0) {
        $("form :submit").prop('disabled', false);
    }
    else if ($("form :submit").hasClass('disable-when-no-partialblock')) {
        $("form :submit").prop('disabled', true);
    }
}