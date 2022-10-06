var FtlModule;
(function (FtlModule) {
    var FtlClass = /** @class */ (function () {
        function FtlClass() {
        }
        FtlClass.prototype.RemoveZeroElementFromDropdown = function () {
            $(".enum-ddl option[value='0']").remove();
        };
        FtlClass.prototype.ToggleTruckLoadType = function () {
            var truckLoadType = $(truckLoadDropDown).val();
            if ($(truckLoadDropDown).val() == '2') {
                enableElement('.freightOnBoardTypes');
                $('.freightOnBoardTypes').removeClass('subSectionOpacity');
                //$('.ftl-pricing-controls').show();
                $('.ftl-controls').show();
                updateJobSpecificHeaders('Job', 'Site');
            }
            else {
                disableElement('.freightOnBoardTypes');
                $('.freightOnBoardTypes').addClass('subSectionOpacity');
                //$('.ftl-pricing-controls').hide();
                $('.ftl-controls').hide();
                updateJobSpecificHeaders('Site', 'Job');
                $('.opis-controls,.feed-type').hide();
                //$('.fuel-display-group').show();
            }
            UpdateDeliveryScheduleCarriers();
            updateFeesHeader(truckLoadType);
            updateFeesUrls(truckLoadType);
            $('.freightOnBoardTypes').trigger('change');
        };
        return FtlClass;
    }());
    FtlModule.FtlClass = FtlClass;
    $(document).ready(function () {
        var ftl = new FtlModule.FtlClass();
        ftl.RemoveZeroElementFromDropdown();
        ftl.ToggleTruckLoadType();
        $('.ddl-pricing-source').trigger('change');
        $(quantityIndicatorDropDown).val(defaultQuantityIndicator);
        updateQuantityIndicator();
        $(truckLoadDropDown).on('change', function (e) {
            ftl.ToggleTruckLoadType();
            removeFeesForThirdPartyOrder();
        });
        $(quantityIndicatorDropDown).on("change", function () {
            updateQuantityIndicator();
        });
    });
    function updateQuantityIndicator() {
        if (defaultQuantityIndicator > 0 && $(quantityIndicatorDropDown).val() != defaultQuantityIndicator) {
            $(".qty-ind-err").show();
        }
        else {
            $(".qty-ind-err").hide();
        }
    }
    function updateFeesUrls(truckLoad) {
        if (typeof truckLoad !== "undefined") {
            var sections = $('.add-partial-block');
            $.each(sections, function (idx, ele) {
                var url = $(this).attr('data-url');
                if (url != undefined && url.split('?').length > 1) {
                    var newUrl = replaceUrlParam(url, 'truckLoadType', truckLoad);
                    $(this).attr('data-url', newUrl);
                }
            });
        }
    }
    function updateFeesHeader(truckLoadType) {
        if (truckLoadType === '2') {
            $('.freight-cost').text('Freight Cost');
            $('.freight-cost-weekend').text('Weekend / Holiday Freight Cost');
        }
        else {
            $('.freight-cost').text('Fee(s)');
            $('.freight-cost-weekend').text('Weekend / Holiday Fee(s)');
        }
    }
    function removeFeesForThirdPartyOrder() {
        var feeTypes = $('.fee-type-row');
        if (feeTypes != undefined && feeTypes.length > 0) {
            $.each(feeTypes, function (i, element) {
                var removeLink = $(element).find('.remove-fee');
                if (removeLink != undefined) {
                    removeLink.trigger('click');
                }
            });
        }
    }
    function updateJobSpecificHeaders(oldString, newString) {
        $(".job-section .job-site-info").each(function () {
            var text = $(this).text();
            text = text.replace(oldString, newString);
            $(this).text(text);
        });
    }
    function replaceUrlParam(url, paramName, paramValue) {
        if (paramValue == null) {
            paramValue = '';
        }
        var pattern = new RegExp('\\b(' + paramName + '=).*?(&|#|$)');
        if (url.search(pattern) >= 0) {
            return url.replace(pattern, '$1' + paramValue + '$2');
        }
        url = url.replace(/[?#]$/, '');
        return url + (url.indexOf('?') > 0 ? '&' : '?') + paramName + '=' + paramValue;
    }
    //function ShowHideFTLPricingControls() {
    //    if ($('#@Html.IdFor(m => m.FuelPricingDetailsViewModel.TruckLoadTypes)').val() == '@((int)TruckLoadTypes.FullTruckLoad)') {
    //        $('.ftl-pricing-controls').show();
    //    }
    //    else {
    //        $('.ftl-pricing-controls').hide();
    //    }
    //}
    function enableElement(element) {
        $(element).removeClass('subSectionOpacity');
        $(".select2-container").removeClass("pntr-none");
        $(element).each(function () {
            if ($(this).is('input:radio') || $(this).is('input:checkbox')) {
                $(this).removeClass('pntr-none').parent().removeClass('pntr-none');
            }
            else if ($(this).is('input:text')) {
                if (!$(this).hasClass('always-readonly')) {
                    $(this).prop('readonly', false).parent().removeClass('pntr-none');
                }
            }
            else {
                $(this).removeClass('pntr-none');
            }
        });
    }
    function disableElement(element) {
        $(element).find(".select2-container").addClass("pntr-none");
        $(element).each(function () {
            if ($(this).is('input:radio') || $(this).is('input:checkbox')) {
                $(this).addClass('pntr-none').parent().addClass('pntr-none');
            }
            else if ($(this).is('input:text') && $(this).hasClass("multiselect-search") == false) {
                $(this).prop('readonly', true);
            }
            else {
                $(this).addClass('pntr-none');
            }
        });
    }
    function selectAxxis() {
        $('.ddl-pricing-source').val('1');
        $('.ddl-pricing-source').trigger('change');
    }
    function isFTLSelected() {
        return $(truckLoadDropDown).val() == '2';
    }
    function UpdateDeliveryScheduleCarriers() {
        if (isFTLSelected()) {
            $('.tpo-carrier').show();
        }
        else {
            $('.carrier-name').val('');
            $('.tpo-carrier').hide();
        }
    }
})(FtlModule || (FtlModule = {}));
//# sourceMappingURL=Ftl.js.map