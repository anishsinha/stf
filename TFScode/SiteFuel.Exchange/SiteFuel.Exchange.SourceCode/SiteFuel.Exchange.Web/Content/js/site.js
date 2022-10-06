$(document).ready(function () {
    documentReady(false);
    setNavigation();

    //re-parse form
    parseForm();
    // make select2 enable
    enableSelect2();

    $(document).on('change', '.zipcode', function () {
        getAddressByZip($(this));
    });

    $(document).on('change', '.billingZipCode', function () {
        getAddressByZip($(this));
    });

    $.validator.setDefaults({
        onkeyup: false
    });

    //SET COPYRIGHT YEAR
    $("#dynamic-footer-text").each(function (i, el) {
        $(el).html('&copy;' + ' ' + new Date().getFullYear() + ' TrueFill, Inc.');
    });

    checkUsersActivity();
});

var USER_INACTIVE_SINCE = 0;//IN MINUTE
var IDLE_TIME_ALLOWDED = 10;//IN MINUTES

function checkUsersActivity() {

    setInterval(function () {
        USER_INACTIVE_SINCE += 1;
    }, (1000 * 60));//1 MINUTE INTERVAL
    $(window).focus(function () {
        USER_INACTIVE_SINCE = 0;
    });
    $(window).blur(function () {
        USER_INACTIVE_SINCE = 0;
    });
    $(document).click(function () {
        USER_INACTIVE_SINCE = 0;
    });
    $(document).keypress(function () {
        USER_INACTIVE_SINCE = 0;
    });
}

function IsUserActive() {
    return IDLE_TIME_ALLOWDED > USER_INACTIVE_SINCE;
}

function SetUoMCurrencyAndRacklables() {
    // if (string1.localeCompare(string2) == 0)
    var dashboardFilter = getDashboardFilter();
    var uom = dashboardFilter.currencyType == 1 ? 'Gallons' : 'Litres';
    var racktext = dashboardFilter.currencyType == 1 ? 'Pricing (USD)' : 'Pricing (CAD)';
    var ppg = dashboardFilter.currencyType == 1 ? 'PPG' : 'PPL';
    var currency = dashboardFilter.currencyType == 1 ? 'USD' : 'CAD';
    $('.uom-by-country').each(function (ele, i) {
        $(this).find('.uom-lbl').text(uom);
    });
    $('.currency-by-country').each(function (ele, i) {
        $(this).text(currency);
    });
    $('.rack-ppg-by-country').each(function (ele, i) {
        $(this).text(racktext);
    });
    $('.ppg-lbl').each(function (ele, i) {
        $(this).text(ppg);
    });
}

function setSelectedCountryAndCurrency(country, currency) {
    SetLocalStorage("countryIdForDashboard", country);
    SetLocalStorage("currencyTypeForDashboard", currency);
}

function getAllJobsForCountry(url, target, country, selectedvalue) {
    $.get(url, { countryId: parseInt(country, 10) }, function (response) {
        target.empty().append('<option value=""></option>');
        $.each(response, function (i, element) {
            target.append($('<option ' + (selectedvalue == element.Id ? 'selected="selected"' : '') + '></option>').val(element.Id).html(element.Name));
        });
    });
}

var getQueryString = function (field, url) {
    var href = url ? url : window.location.href;
    var reg = new RegExp('[?&]' + field + '=([^&#]*)', 'i');
    var string = reg.exec(href);
    return string ? string[1] : null;
};

function getSelectedCountryAndCurrency() {
    var countryId = GetLocalStorage("countryIdForDashboard");
    var currencyType = GetLocalStorage("currencyTypeForDashboard");
    if (countryId === undefined || countryId === null || countryId == '') {
        countryId = 1;
        currencyType = 1;
    }
    return { countryId: countryId, currencyType: currencyType };
}

function documentReady(isThisCalledFromPartialView) {

    if (isNaN(isThisCalledFromPartialView)) {
        isThisCalledFromPartialView = false;
    }

    (function () {

        var xhrPool = [];

        $.ajaxSetup({
            cache: false,
            converters: {
                "text html": function (text) {
                    if (text && text.indexOf("/Account/Register") > -1 && text.indexOf("RememberMe") > -1) {
                        abort();
                        window.location.reload();
                    }
                    return text;
                }
            }
        });

        $(document).ajaxSend(function (e, jqXHR, options) {
            xhrPool.push(jqXHR);
        });

        $(document).ajaxComplete(function (e, jqXHR, options) {
            xhrPool = $.grep(xhrPool, function (x) { return x != jqXHR });
            if (jqXHR.responseText && jqXHR.responseText.indexOf("/Account/Register") > -1 && jqXHR.responseText.indexOf("RememberMe") > -1) {
                abort();
                window.location.reload();
            }
        });

        var abort = function () {
            $.each(xhrPool, function (idx, jqXHR) {
                jqXHR.abort();
            });
        };

    })();

    //Set time zone cookie
    $.cookie("timezoneoffset", new Date().getTimezoneOffset() * -1);

    //To enable/disable submit button
    $(document).on('invalid-form.validate', 'form', function () {
        var button = $(this).find('input[type="submit"]');
        var submitBtn = $(this).find('.form-submit');
        setTimeout(function () {
            button.removeAttr('disabled');
            submitBtn.removeAttr('disabled');
        }, 1);
    });

    $(document).on('submit', 'form', function () {
        if ($('input[type="submit"]').hasClass('no-disable') == false) {
            var button = $(this).find('input[type="submit"]');
            var submitBtn = $(this).find('.form-submit');
            setTimeout(function () {
                button.attr('disabled', 'disabled');
                submitBtn.attr('disabled', 'disabled');
            }, 0);
        }
    });

    //For enforcing input length using maxlength at client side
    $("input[data-val-length-max]").each(function () {
        var $this = $(this);
        var data = $this.data();
        $this.attr("maxlength", data.valLengthMax);
    });

    initDateTimePicker();

    //Radio button and toggle its sections
    $('.toggle-section').click(function () {
        var inputValue = $(this).attr("data-id");
        var targetBox = $("#" + inputValue);
        $(this).closest(".radio-group").find(".box").hide();

        // set value as 0 if textboxes are empty
        var alltextboxes = $(this).closest(".radio-group").find(".box input[type='text']");
        alltextboxes.each(function () {
            if ($(this).val().trim() == "") {
                $(this).val("0");
            }
        });

        // make textboxes empty if value is 0
        var sectiontextboxes = targetBox.find("input[type='text']")
        sectiontextboxes.each(function () {
            if (parseFloat($(this).val().trim()) == 0) {
                $(this).val("");
            }
        });

        $(targetBox).show();
    });

    if ($('.schedule-date').length > 0) {
        $('.schedule-date').each(function () {
            $(this).data("DateTimePicker").useCurrent(false);
            $(this).data("DateTimePicker").minDate(moment().millisecond(0).second(0).minute(0).hour(0));
        });
    }

    //Password group button code
    if ($('.pwstrength').length > 0) {
        $('.pwstrength').pwstrength();
    }

    $('.show-password').mouseup(function () {
        $('.pwstrength').attr('type', 'password');
    }).mousedown(function () {
        $('.pwstrength').attr('type', 'text');
    });

    //Single select drop down, hide/show
    if ($('.onchange-select-showhide').length > 0) {
        (function () {
            var previousSelectedValue;
            var previousSelectedText;

            $(".onchange-select-showhide").on("focus", function () {

                previousSelectedValue = $(this).find("option:selected").val();
                previousSelectedText = $(this).find("option:selected").text();

            }).on("change", function () {

                var currentSelectedValue = $(this).find("option:selected").val();
                var currentSelectedText = $(this).find("option:selected").text();

                if (previousSelectedValue == currentSelectedValue) {
                    return true;
                }
                else {
                    if (previousSelectedText) {
                        var previousBox = $(".selected-val-" + previousSelectedText.toLowerCase().replace(/\s/g, "").replace(/&/g, ""));
                        if (previousBox.length > 0) {
                            previousBox.hide();
                        }
                    }
                    if (currentSelectedText) {
                        var currentBox = $(".selected-val-" + currentSelectedText.toLowerCase().replace(/\s/g, "").replace(/&/g, ""));
                        if (currentBox.length > 0) {
                            currentBox.show();
                        }
                    }
                    previousSelectedText = currentSelectedText;
                    previousSelectedValue = currentSelectedValue;
                }
            });
        })();
    }

    initMultiSelect();

    //enabling/disabling controls
    $("div").find(".disable-controls :input").attr("disabled", "disabled");
    $("div").find(".enable-controls :input").removeAttr("disabled");

    $(".edit-data").click(function () {
        $(".edit-data").hide();
        $(".btn-wrapper .hide-element").removeClass('hide-element');
        $("div").find(".disable-controls :input").removeAttr("disabled");
        $("div").find(".disable-controls .disable-control").attr("disabled", "disabled");
        $("div").find(".show-hide-toggle .hide-element").toggle();
        $("div").find(".show-hide-toggle .show-element").toggle();
    });

    if ($('[data-toggle=confirmation]').length > 0) {
        $('[data-toggle=confirmation]').confirmation({
            rootSelector: '[data-toggle=confirmation]',
            html: true
        });
    }

    if ($('main').find('#display-custom-message')) {
        if ($('body').find('.login-box,.register-box').length > 0) {
            $("#custom-message>div").removeClass('container');
            $("#display-custom-message .close ").css('display', 'none');
            $("#display-custom-message").show().delay(12000).fadeOut('fast');
        }
        else {
            if (!isThisCalledFromPartialView) {
                $("#custom-message>div").addClass('container');
                $("#display-custom-message .container").css('width', '');
                $("#display-custom-message").show().delay(12000).fadeOut('fast');
            }
        }
    }

    $('.input-group-btn :input').attr('tabindex', -1);

    addRequiredSymbol();

    $('.main-wrapper :input,textarea,select,a').each(function () {
        if (!$(this).is('a') && $(this).is('[disabled=disabled]')) {
            $(this).closest('.defaultDisabled').addClass('subSectionOpacity');
        }
        else if ($(this).is('a') && $(this).css('pointer-events') == 'none') {
            $(this).closest('.defaultDisabled').addClass('subSectionOpacity');
        }
    });

    if (!isThisCalledFromPartialView) {
        $('.radio input:radio:checked').each(function () {
            var $this = $(this);
            if (!$this.hasClass('pntr-none') && !$this.hasClass('no-hidden-click')) {
                $this.trigger("click");
            }
        });

        $('.checkbox input:checkbox').each(function () {
            $this = $(this);
            if (!$this.hasClass('pntr-none') && !$this.hasClass("select-all") && !$this.hasClass('no-hidden-click')) {
                $this.trigger("click").trigger("click");
            }
        });
    }

    if ($.fn.dataTable != undefined) {
        $.extend(true, $.fn.dataTable.defaults, {
            "bStateSave": true,
            "iStateDuration": -1
        });

        //command to catch the datatable error instead alert
        $.fn.dataTable.ext.errMode = 'throw';
    }

    // enabling disabling elements
    $(".disableelements").click(function () {
        $(".optional-field").addClass("pntr-none subSectionOpacity").attr("readonly", "readonly").attr("disabled", "disabled");
    });
    $(".enableelements").click(function () {
        $(".optional-field").removeClass("pntr-none subSectionOpacity").removeAttr("readonly").removeAttr("disabled");
    })
};

function addRequiredSymbol() {
    $('main :input,select').each(function () {
        var result = null;
        result = $(this).attr('data-val-required') || $(this).attr('data-val-requiredif');
        if (result != null) {
            if ($(this).is('select')) {
                var lblFor = $(this).attr('id').split("_Id")[0];
                if ($('main').find('label[for=' + lblFor + ']').length > 0 && $('main').find('label[for=' + lblFor + ']').children().length == 0) {
                    $('main').find('label[for=' + lblFor + ']').append('<span class="required pl4">*</span>');
                }
            }
            else {
                if ($(this).attr('id') == $(this).closest(".form-group").find('label').attr('for')) {
                    if ($(this).closest(".form-group").find('label').children().length == 0) {
                        $(this).closest(".form-group").find('label').append('<span class="required pl4">*</span>');
                    }
                }
            }
        }
    });
}

//Select all in multiselect
function selectall(checkboxname, select2dd) {
    if ($("." + checkboxname).is(':checked')) {
        $("." + select2dd + " > option").prop("selected", "selected");
        $("." + select2dd).trigger("change");
    } else {
        $("." + select2dd).val(null).trigger("change");
        if (select2dd == "states") {
            getDefaultState();
        }
    }
};

function selectallExternal(checkboxname, select2dd) {
    if ($(checkboxname).is(':checked')) {
        $(checkboxname).closest(".form-group").find("." + select2dd + " > option").prop("selected", "selected");
        $(checkboxname).closest(".form-group").find("." + select2dd).trigger("change");
    } else {
        $(checkboxname).closest(".form-group").find("." + select2dd).val(null).trigger("change");
        if (select2dd == "states") {
            getDefaultState();
        }
    }
};
// select all checkbox status based drop-down option selection
function checkStatus(checkboxname, select2dd) {
    if ($("." + select2dd + " option:selected").length == $("." + select2dd + " option").length) {
        $("." + checkboxname).prop('checked', true);
    }
    else {
        $("." + checkboxname).prop('checked', false);
    }
}

function checkStatusExternal(checkboxname, select2dd) {
    var elementId = $(select2dd).attr('id');
    if ($('#' + elementId + " option:selected").length == $('#' + elementId + " option").length) {
        $(select2dd).closest(".form-group").find("." + checkboxname).prop('checked', true);
    }
    else {
        $(select2dd).closest(".form-group").find("." + checkboxname).prop('checked', false);
    }
}

function enableSelect2() {
    $(".select2-container").removeClass("pntr-none");
}

function validationMessageFor(id, message) {
    var span = $("span[data-valmsg-for=\"" + id + "\"]");
    if (span && span.length > 0) {
        span.html(message);
        if (message && message != "") {
            span.removeClass("field-validation-valid");
            span.addClass("field-validation-error");
        } else {
            span.removeClass("field-validation-error");
            span.addClass("field-validation-valid");
        }
    }
};

function parseForm() {
    var form = $('form')
        .removeData("validator") /* added by the raw jquery.validate plugin */
        .removeData("unobtrusiveValidation");  /* added by the jquery unobtrusive plugin*/

    $.validator.unobtrusive.parse(form);
};

function parseFormById(selector) {
    var form = $(selector)
        .removeData("validator") /* added by the raw jquery.validate plugin */
        .removeData("unobtrusiveValidation");  /* added by the jquery unobtrusive plugin*/

    $.validator.unobtrusive.parse(form);
};

var onAjaxSuccess = function () {
    parseForm();
};

function setNavigation() {
    var path = window.location.pathname;
    path = path.replace(/\/$/, "");
    path = decodeURIComponent(path);

    $(".nav a").each(function () {
        var href = $(this).attr('href');
        if (path === href) {
            $(this).closest('li').addClass('active');
            $(this).closest('div').addClass('in');
            $(this).addClass('active');
            $(this).closest('.main-menu').find('> a').addClass('active');
            $(this).closest('.main-menu')
                .find('> a span:last-child')
                .removeClass('fa-angle-right')
                .addClass('fa-angle-down');
        }
    });
};

function GetLocalStorage(key) {
    var item = undefined;
    if (typeof Storage !== "undefined") {
        item = localStorage.getItem(key);
    }
    return (typeof item === "undefined" ? '' : item);
}

function SetLocalStorage(key, val) {
    if (typeof Storage !== "undefined") {
        localStorage.setItem(key, val);
    }
}

function RemoveLocalStorage(key) {
    if (typeof Storage !== "undefined") {
        localStorage.removeItem(key);
    }
}

function GetLocalStorageItems() {
    var keyvalue = {};
    if (typeof Storage !== "undefined") {
       var keys = Object.keys(localStorage),  i = keys.length;
        while (i--) {
            keyvalue[keys[i]] = localStorage.getItem(keys[i]);
        }
    }
    return keyvalue;
}

function GetSessionStorage(key) {
    if (typeof Storage !== "undefined") {
        return sessionStorage.getItem(key);
    }
}

function SetSessionStorage(key, val) {
    if (typeof Storage !== "undefined") {
        sessionStorage.setItem(key, val);
    }
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
        AddZeroIfEmpty(this);
    });
}

function AddZeroIfEmpty(element) {
    if (($(element).is(':visible') || $(element).hasClass('always')) &&
        ($(element).hasClass('datatype-decimal') || $(element).hasClass('datatype-int'))) {
        var value = $(element).val(), num = parseFloat(value);
        if (value == '' || (!isNaN(num) && num == 0)) {
            $(element).val('0');
        }
    }
}

function enableElement(element) {
    $(element).removeClass('subSectionOpacity');
    $(".select2-container").removeClass("pntr-none")
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
        removeZero(this);
    });
}
function enableSection(section) {
    $('.' + section).removeClass('pntr-none');
}
function disableSection(section) {
    $('.' + section).addClass('pntr-none');
}
function removeZero(element) {
    if (($(element).is(':visible') || $(element).hasClass('always')) &&
        ($(element).hasClass('datatype-decimal') || $(element).hasClass('datatype-int'))) {
        var value = parseFloat($(element).val());
        if (!isNaN(value) && value == 0) {
            $(element).val('');
        }
    }
}

function radioChange(radioBtn, enableClassArr, disableClassArr) {
    if (radioBtn != null && $(radioBtn).is('Input[type="checkbox"]')) {
        if ($(radioBtn).is(':checked')) {
            enableElement($("." + enableClassArr));
            disableElement($("." + disableClassArr));
        }
        else {
            enableElement($("." + disableClassArr));
            disableElement($("." + enableClassArr));
        }
    }
    else {
        var enableClass = [];
        var disableClass = [];
        if (enableClassArr != null) {
            enableClass = enableClassArr.split(",");
        }
        if (disableClassArr != null) {
            disableClass = disableClassArr.split(",");
        }

        for (var temp2 = 0; temp2 < disableClass.length; temp2++) {
            disableElement($("." + disableClass));
        }
        for (var temp2 = 0; temp2 < enableClass.length; temp2++) {
            enableElement($("." + enableClass));
        }
        for (var temp = 0; temp < enableClass.length; temp++) {
            if (enableClass[temp] == null) {
                $(radioBtn).closest(".main-section").find(".marginValueDiv input[type='text']").prop('readonly', true);
            }
            else {
                $(radioBtn).closest(".main-section").find("." + enableClass[temp]).prop('readonly', false);
                $("." + enableClass[temp] + " :input").each(function () {
                    enableElement(this);
                });
                $("." + enableClass[temp] + " select").each(function () {
                    enableElement(this);
                    $(".enable-list").css('pointer-events', 'visible')
                });
                $("." + enableClass[temp] + " a").each(function () {
                    if (!$(this).hasClass('tier')) {
                        $(this).removeClass('subSectionOpacity');
                        $(this).css('pointer-events', 'visible');
                    }
                });
            }
            $('.' + enableClass[temp]).removeClass('subSectionOpacity');
        }

        for (var temp1 = 0; temp1 < disableClass.length; temp1++) {
            $("." + disableClass[temp1] + " :input").each(function () {
                disableElement(this);
            });
            $("." + disableClass[temp1] + " select").each(function () {
                disableElement(this);
                $(".enable-list").css('pointer-events', 'none');
            });
            $("." + disableClass[temp1] + " a").each(function () {
                if (!$(this).hasClass('tier')) {
                    $(this).css('pointer-events', 'none');
                }
            });
            $("." + disableClass[temp1] + " div").each(function () {
                if ($(this).css('pointer-events') == 'none') {
                    $(this).css('pointer-events', 'visible')
                }
            })
            $('.' + disableClass[temp1]).addClass('subSectionOpacity');
        }
    }
}

function inputBoxShowHide(showElement, hideElement) {
    $("." + showElement).show();
    $("." + hideElement).hide();
}


function checkBoxChange(element, className) {
    var targetElement = $('.' + className);
    targetElement.closest('.defaultDisabled').removeClass('defaultDisabled');
    if ($(element).is(':checked')) {
        targetElement.closest('.row+.subSectionOpacity').removeClass('subSectionOpacity');
        targetElement.removeClass('subSectionOpacity');
        targetElement.find('.pntr-none').removeClass('pntr-none');
        targetElement.find('a').css('pointer-events', 'visible');
        enableElement(targetElement);
    }
    else {
        targetElement.addClass('subSectionOpacity');
        disableElement(targetElement);
    }
}

function resetDisabledSection(element, className) {
    var targetElement = $('.' + className);
    if ($(element).is(':checked')) {
        targetElement.find("input:checkbox").prop('checked', true);
    }
    else {
        targetElement.find("input:checkbox").prop('checked', false);
    }
}

function toggleElement(elementClassName) {
    //$('.privateSelect *').prop('readonly', function (i, v) { return !v; });
    $('.privateSelect *').attr('readonly', function(i, v) { return !v; });
    $('.' + elementClassName).toggle();
}

function toggleTileElement(toggleLink, targetTile) {
    $("#" + targetTile + " .element-container").slideToggle(500);
    $("#" + targetTile + " .toggle-tiledata i").toggleClass("fa-chevron-circle-up fa-chevron-circle-down");
}

function isTileCollapsed(targetTile) {
    return $("#" + targetTile + " .toggle-tiledata i").hasClass('fa-chevron-circle-down');
}

function isTileExpanded(targetTile) {
    return $("#" + targetTile + " .toggle-tiledata i").hasClass('fa-chevron-circle-up');
}

function focusOnFirst(element, className) {
    if ($('.' + className).find('*').filter(':input:visible:first').hasClass('noTrigger')) {
        $('.' + className).find('*').filter(':input:visible:first').focusout();
        $('.' + className).removeClass('pntr-none');
    }
    else {
        $('.' + className).find('*').filter(':input:not([readonly]):visible:first').focus();
        $('.' + className).removeClass('pntr-none');
    }
}

//$(function () {
//    $('[data-toggle="tooltip"]').tooltip()
//})

$('[data-toggle="tooltip"]').tooltip({
    sanitizeFn: function (content) { return content; }
});

function SubTabSelection() {
    $('.subTab*').removeClass('show-element');
    $('.subTab*').addClass('hide-element');
    $('.container').find('.input-validation-error:eq(0)').closest('.subTab').toggleClass('hide-element show-element');
}

function brockerRadioChange(radioBtn, enableClassArr, disableClassArr) {
    var enableClass = [];
    var disableClass = [];
    if (enableClassArr != null) {
        enableClass = enableClassArr.split(",");
    }
    if (disableClassArr != null) {
        disableClass = disableClassArr.split(",");
    }

    for (var temp = 0; temp < enableClass.length; temp++) {
        $(radioBtn).closest(".main-section").find("." + enableClass[temp]).prop('readonly', false);
        $(radioBtn).closest(".main-section").find("." + enableClass[temp] + " :input").each(function () {
            enableElement(this);
        });
        $(radioBtn).closest(".main-section").find("." + enableClass[temp] + " select").each(function () {
            enableElement(this);
        });
        $(radioBtn).closest(".main-section").find("." + enableClass[temp] + " a").each(function () {
            $(this).css('pointer-events', 'visible');
        });
        $(radioBtn).closest(".main-section").find("." + enableClass[temp]).removeClass('subSectionOpacity');
        $(radioBtn).closest(".main-section").find("." + enableClass[temp] + " :radio:checked").click();
    }

    for (var temp1 = 0; temp1 < disableClass.length; temp1++) {
        if (disableClass[temp1] == "override") {
            $(".inputPricePerGallon").val($("#hdnPricePerGallon").val());
        }
        if (disableClass[temp1] == "margin") {
            $('.inputMargin').val("");
        }
        $(radioBtn).closest(".main-section").find("." + disableClass[temp1] + " :input").each(function () {
            disableElement(this);
        });
        $(radioBtn).closest(".main-section").find("." + disableClass[temp1] + " select").each(function () {
            disableElement(this);
        });
        $(radioBtn).closest(".main-section").find("." + disableClass[temp1] + " a").each(function () {
            $(this).css('pointer-events', 'none');
        });
        $(radioBtn).closest(".main-section").find("." + disableClass[temp1] + " div").each(function () {
            if ($(this).css('pointer-events') == 'none') {
                $(this).css('pointer-events', 'visible');
            }
        })
        $(radioBtn).closest(".main-section").find("." + disableClass[temp1]).addClass('subSectionOpacity');
        $(radioBtn).closest(".main-section").find("." + disableClass[temp1]).prop('readonly', true);
    }
}

function setReadOnlyMode(element, readClass, writeClass) {
    var isChecked = $(element).is(":checked");
    $('.' + readClass).each(function () {
        if (isChecked) {
            enableElement(this);
            $('.' + readClass).closest('.combineDiv').removeClass('subSectionOpacity').addClass('opacity1');
            $('.' + writeClass).closest('.combineDiv').removeClass('opacity1').addClass('subSectionOpacity');
        }
        else {
            disableElement(this);
            $('.' + readClass).closest('.combineDiv').removeClass('opacity1').addClass('subSectionOpacity');
            $('.' + writeClass).closest('.combineDiv').removeClass('subSectionOpacity').addClass('opacity1');
        }
    });
    $('.' + writeClass).each(function () {
        if (isChecked) {
            disableElement(this);
        }
        else {
            enableElement(this);
        }
    });
}
// error message fade in/out
function showSuccessErrorMsg(response) {
    $('#display-custom-message').html(response).show().delay(12000).fadeOut('fast', function () {
        $('#display-custom-message').empty();
    });
}

function checkBoxUncheck(checkBoxClasss) {
    $('.' + checkBoxClasss + ":input").each(function () {
        if ($(this).is(':checkbox') && $(this)[0].checked) {
            $(this)[0].checked = false;
        }
    });
}

function clickEventTrigger(element, className) {
    if ($(element).is(':checked')) {
        $('.' + className + ' .radio input:radio').each(function () {
            if ($(this).attr('checked') == 'checked') {
                $(this).trigger("click");
            }
        })
    }
}

function convertTo24Hour(time) {
    if (/(1[2]):[0][0] ?[Aa][mM]$/.test(time)) {
        return '00:00';
    }
    if (/(1[2]):[0][0]:[0][0] ?[Aa][mM]$/.test(time)) {
        return '00:00:00';
    }
    var timewithFormat = time.split(' ');
    var response = timewithFormat[0].split(':');
    var hours = response[0], minutes = response[1];
    if (time.toLowerCase().indexOf('am') !== -1 && hours == 12) {
        hours = 0;
    }
    if (time.toLowerCase().indexOf('pm') !== -1 && hours < 12) {
        hours = parseInt(hours) + 12;
    }
    response = hours + ':' + minutes + (response.length == 3 ? ':' + response[2] : '');
    return response;
}

function getZoomArea(data) {
    var zoomarea = 4;
    if (data.length > 1) {
        var a = 0;
        var maxDistance = 0;
        for (var i = 0; i < data.length; i++) {
            for (var j = a; j < data.length; j++) {
                if (i != j) {
                    var Distance = distance(data[i].Latitude, data[i].Longitude, data[j].Latitude, data[j].Longitude);
                    if (Distance > maxDistance) {
                        maxDistance = Distance;
                    }
                }
            }
            a = a + 1;
        }

        if (maxDistance <= 200) {
            zoomarea = 6;
        }
        else if (maxDistance > 200 && maxDistance <= 500) {
            zoomarea = 5;
        }
        else if (maxDistance > 500 && maxDistance <= 1000) {
            zoomarea = 4;
        }
        else if (maxDistance > 1000 && maxDistance <= 2999) {
            zoomarea = 3;
        }
        else if (maxDistance > 3000) {
            zoomarea = 2;
        }
    }
    else if (data.length == 0) {
        zoomarea = 4;
    }
    else {
        zoomarea = 12;
    }
    return zoomarea;
}

function distance(lat1, lon1, lat2, lon2, unit) {
    var radlat1 = Math.PI * lat1 / 180
    var radlat2 = Math.PI * lat2 / 180
    var theta = lon1 - lon2
    var radtheta = Math.PI * theta / 180
    var dist = Math.sin(radlat1) * Math.sin(radlat2) + Math.cos(radlat1) * Math.cos(radlat2) * Math.cos(radtheta);
    dist = Math.acos(dist)
    dist = dist * 180 / Math.PI
    dist = dist * 60 * 1.1515
    if (unit == "K") { dist = dist * 1.609344 }
    if (unit == "N") { dist = dist * 0.8684 }
    return dist
}

function getMinutes(time) {
    var splitTime = time.split(':');
    var minutes = (splitTime[0] * 60) + splitTime[1];
    return minutes;
}

function getSeconds(time) {
    var splitTime = time.split(':');
    var seconds = (getMinutes(time) * 60) + (splitTime.length === 3 ? splitTime[2] : 0);
    return seconds;
}

function showHideElements(showelement, hideelement) {
    $("." + showelement).show();
    $("." + hideelement).hide();
}

function enableserviceradius(thisElement) {
    var specificRadiusId = $(thisElement).attr("id").replace("IsStateWideService", "Radius");
    $("#" + specificRadiusId).removeClass("pntr-none subSectionOpacity").removeAttr("readonly");
    $("#" + specificRadiusId).closest(".form-group").find(".radius-warning").show();
}
function disableserviceradius(thisElement) {
    var specificRadiusId = $(thisElement).attr("id").replace("IsStateWideService", "Radius");
    $("#" + specificRadiusId).addClass("pntr-none subSectionOpacity");
    $("#" + specificRadiusId).closest(".form-group").find(".radius-warning").hide();
}
function triggerEvent(element, eventName) {
    $(element).trigger(eventName)
}

function showInvalidTab() {
    var errorElement = $("form").find(".field-validation-error").last();
    if (errorElement != undefined) {
        var parent = errorElement.parents("div[data-parent-tab]");
        if (parent != undefined && !parent.is(":visible")) {
            var targetId = parent.attr("data-parent-tab");
            $("#" + targetId).click();
        }
    }
}

function showAjaxImages() {
    $("img[data-image-url]").each(function () {
        var image = $(this), imageUrl = image.attr("data-image-url");
        $.get(imageUrl, function (response) {
            if (response != undefined && response != '')
                image.attr('src', 'data:image/jpg;base64,' + response);
        });
    });
}

$(document).on('init.dt xhr.dt column-visibility.dt', function (e, settings) {
    BindDataTableFilter(e, settings);
});

function BindDataTableFilter(e, settings) {
    $this = $(e.target).dataTable();
    var columns = $this.api().columns().visible(), columnIndexes = [];
    for (var i = 0; i < columns.length; i++) {
        if (columns[i] == true)
            columnIndexes.push(i);
    }
    configFilter($this, columnIndexes);
}

function PreventBackNavigation() {
    if (window.history && window.history.pushState) {
        window.history.pushState('', null, window.location.href);
        $(window).on('popstate ', function () {
            window.history.forward();
        });
    }
}

function clearLocalStorage() {
    localStorage.clear();
    sessionStorage.clear();
}

function initDateTimePicker() {
    //Default application to datepicker
    if ($('.datepicker').length > 0) {
        $('.datepicker').datetimepicker({
            format: 'L'
        });
    }

    if ($('.timepicker').length) {
        $('.timepicker').datetimepicker({
            format: 'LT'
        });
    };

    if ($('.timepicker-withseconds').length) {
        $('.timepicker-withseconds').datetimepicker({
            format: 'hh:mm:ss A'
        });
    };
}

function matchStart(params, data) {
    params.term = params.term || '';
    if (data.text.toUpperCase().indexOf(params.term.toUpperCase()) == 0) {
        return data;
    }
    return null;
}

function initMultiSelect() {
    //Multi select drop down
    var allMultiSelect = $('.multi-select');
    if (allMultiSelect.length > 0) {
        $.each(allMultiSelect, function (idx, elem) {
            var placeholderText = $(elem).attr('placeholder');
            if (placeholderText === undefined || placeholderText === null)
                placeholderText = "Select one or more ...";
            $(elem).select2({
                placeholder: placeholderText,
                closeOnSelect: false,
                selectAll: true
            });
        });
    }

    // used as min 3 chars required to show private suppliers list
    if ($('.private-supplier-list').length > 0) {
        $('.private-supplier-list').select2({
            placeholder: "Select one or more ...",
            closeOnSelect: false,
            selectAll: true,
            matcher: function (params, data) {
                return matchStart(params, data)
            },
            "language": {
                "noResults": function () {
                    return "No matches are found";
                }
            },
            minimumInputLength: 3
        });
    }
}

function SetQuickMessageURL(type, id) {
    if (id > 0) {
        var quickMessageURL = '?queryType=' + type + '&number=' + id;
        var quickMessageLink = $('#ancQuickMessage');
        quickMessageLink.attr('href', quickMessageLink.attr('href') + quickMessageURL);
    }
}

function toggleCloseOrderElement(hideElement) {
    if (hideElement == true) {
        $('.order-close-threshold').hide();
    }
    else {
        $('.order-close-threshold').show();
    }
}

var gridcount = 0, emptycount = 0, tabId;
$(document).on('init.dt', '[data-grid-parent]', function (e, settings) {
    gridcount = $('[data-grid-parent]').length;
    var rows = $(e.target).children('tbody').children('tr');
    if (rows.length === 1 && $(rows[0]).children('td').hasClass('dataTables_empty')) {
        var parent = $(e.target).parents($(e.target).attr('data-grid-parent'));
        if (typeof parent !== typeof undefined && parent !== false) {
            parent.hide();
        }
        emptycount++;
    }
    tabId = $(e.target).attr('data-grid-tab');
});

$(document).ajaxStop(function () {
    if (typeof tabId !== typeof undefined && tabId !== false) {
       var parentTab = $(tabId);
        if (typeof parentTab !== typeof undefined && parentTab !== false) {
            if (gridcount > 0 && gridcount === emptycount)
                parentTab.hide();
            else
                parentTab.show();
        }
    }
});

$(document).on('click', 'a.single-ajax-mode[data-ajax="true"]', function () {
    $(this).attr('data-ajax', false);
});

function thousandSeperator(n) {
    if (typeof n === 'number') {
        n += '';
        var x = n.split('.');
        var x1 = x[0];
        var x2 = x.length > 1 ? '.' + x[1] : '';
        var rgx = /(\d+)(\d{3})/;
        while (rgx.test(x1)) {
            x1 = x1.replace(rgx, '$1' + ',' + '$2');
        }
        return x1 + x2;
    } else {
        return n;
    }
}

function hideElementById(elementId) {
    $('#' + elementId).hide();
}

function showElementById(elementId) {
    $('#' + elementId).show();
}

function ValidateAssetUploadFileSize(htmlElement, maxAllowedSize, buttonId, divId) {
    if (htmlElement.files[0].size < maxAllowedSize) {
        $("#" + buttonId).prop('disabled', false);
        hideElementById(divId);
    }
    else {
        showElementById(divId);
        $("#" + buttonId).prop('disabled', true);
    }
}

function showHideControls(radioBtn, showClassArr, hideClassArr) {
    var hideSectionList = hideClassArr.split(",");
    var showSectionList = showClassArr.split(",");
    for (var i = 0; i < hideSectionList.length; i++) {
        $("." + hideSectionList[i]).css("display", "none");
    }
    for (var s = 0; s < showSectionList.length; s++) {
        $("." + showSectionList[s]).css("display", "block");
    }
}

function showHideGeoControls(radioBtn, showClassArr, hideClassArr) {
    var hideSectionList = hideClassArr.split(",");
    var showSectionList = showClassArr.split(",");
    for (var i = 0; i < hideSectionList.length; i++) {
        $(radioBtn).closest('.partial-block').find("." + hideSectionList[i]).css("display", "none");
    }
    for (var s = 0; s < showSectionList.length; s++) {
        $(radioBtn).closest('.partial-block').find("." + showSectionList[s]).css("display", "block");
    }
}

function getAddressByZip(zip) {
    var address = zip.closest('.address-container').find('.address');
    var state = zip.closest('.address-container').find('.state');
    var city = zip.closest('.address-container').find('.city');
    var county = zip.closest('.address-container').find('.county');
    var country = zip.closest('.address-container').find('.country');
    var countrygroupdiv = zip.closest('.address-container').find('.countrygroup-div');
    var countrycode = zip.closest('.address-container').find('.countrycode');
    var stateCode = zip.closest('.address-container').find('.statecode');
    var zipCode = zip.val();
    if (zipCode == null || zipCode == '' || !(zipCode.length == 5 || zipCode.length == 7)) {
        state.val('');
        city.val('');
        county.val('');
        stateCode.val('');
        return;
    }

    var inputModel = { 'zipCode': zipCode };
    if (address && address.length > 0) {
        inputModel['address'] = address.val();
    }

    var url = "/Validation/GetAddressByZip";
    $.ajax({
        type: "GET",
        url: url,
        data: inputModel,
        dataType: "json",
        success: function (data) {
            if (data != null) {
                //state.val('');
                if (data.City != null && data.City != '') {
                    city.val(data.City);
                    city.trigger('blur');
                }
                else {
                    city.val('');
                }

                if (data.CountryCode != null && data.CountryCode != '') {
                    country.find("option:contains(" + data.CountryCode + ")").prop("selected", true);
                    countrycode.val(data.CountryCode);
                    country.trigger('updated', [data.StateName, data.CountryCode]);
                }

                if (data.CountryGroupCode != null && data.CountryGroupCode != '') {
                    var countrygroup = zip.closest('.address-container').find('.countrygroup');
                    countrygroup.find("option:contains(" + data.CountryGroupCode + ")").prop("selected", true);
                    countrygroupdiv.show();
                } else {
                    countrygroupdiv.hide();
                }
                
                if (data.StateName != null && data.StateName != '') {
                    state.find("option").filter(function (index) { return $(this).html() == data.StateName; }).prop("selected", true);
                    stateCode.val(data.StateCode);
                    state.trigger('StateChanged', [data.StateCode]);
                }
                else {
                    state.val('');
                    stateCode.val('');
                }

                if (data.CountyName != null && data.CountyName != '')
                    county.val(data.CountyName);
                else {
                    county.val('');
                }
                validateForcastingData(data.CountryCode);
            }
            else {
                state.val('');
                city.val('');
                county.val('');
                stateCode.val('');
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            var error = errorThrown;
        }
    });
}
$(document).on("change", ".revalidate", function () {
    var validator = $(this).closest('form').validate();
    for (var name in validator.invalid) {
        if (name != '') {
            var error_control = '[name="' + name + '"]';
            var result = $(error_control).valid();
            if (result) {
                delete validator.invalid[name];
                delete validator.submitted[name];
            }
        }
    }
});

function hideOnFirstClick(elem) {
    $(elem).hide();
}
function maintainTabState() {
    var currenturl = window.location.href, urlpath = window.location.pathname;
    var uniqueId = urlpath.replace(new RegExp('/', 'g'), '_');//.replace(new RegExp(/_\d/, 'g'), '');
    var linkId = "LinkId_" + uniqueId, pageId = "PageId_" + uniqueId;

    $(document).on("click", ".tab-headers a", function () {
        var tabId = $(this).attr("id");
        localStorage.setItem(linkId, tabId);
        localStorage.setItem(pageId, currenturl);
        $($.fn.dataTable.tables()).DataTable().fixedHeader.adjust();
        $(document).ajaxComplete(function () {
            fixedHeader();
        });
    });

    var pageTabs = $(".tab-headers a");
    if (currenturl.indexOf(localStorage.getItem(pageId)) > -1) {
        var target = localStorage.getItem(linkId);
        if (target != undefined && !$('.tab-headers').hasClass('no-tab-remembering')) {
            $(".tab-headers a#" + target).trigger("click");
        }
    }
    else if (pageTabs.length > 0) {
        $(pageTabs[0]).trigger("click");
    }


    $(document).on("submit", "form:not(.no-tab-change)", function () {
        if (currenturl.indexOf(localStorage.getItem(pageId)) > -1) {
            localStorage.removeItem(linkId);
            localStorage.removeItem(pageId);
        }
    });
}

function uploadedFileNames() {
    var selDiv = "";
    document.addEventListener("DOMContentLoaded", init, false);
    function init() {
        document.querySelector('#files').addEventListener('change', handleFileSelect, false);
        selDiv = document.querySelector("#selectedFiles");
    }
    function handleFileSelect(e) {

        if (!e.target.files) return;
        selDiv.innerHTML = "";
        var files = e.target.files;
        for (var i = 0; i < files.length; i++) {
            var f = files[i];
            selDiv.innerHTML += "<tr><td class='border-b pb5 pt5'>" + f.name + "</td></tr>";
        }
    }
}
function ddCityRackToggle() {
    if ($(".enablecityrack").prop("checked") == true && !$(".enablecityrack").hasClass("pntr-none")) {
        $(".dd-cityrack").css("pointer-events", "visible").removeClass('subSectionOpacity');
        $(".dd-cityrack,.cityrackterminallist,.dd-cityrack a").css("pointer-events", "visible");
        $(".multiselect,.multiselect-container *").removeClass("pntr-none");
        $(".cityrack-validation").addClass("hide-element");
    }
}

function clearPricingCode() {
    $(".pricing-code input").val("");
    $('#pricing-code-info label').text('');
}

function showHideonCheckbox(checkbox, toggleSection) {
    var toggleSections = toggleSection.split(',');
    for (var i = 0; i < toggleSections.length; i++) {
        if ($(checkbox).prop("checked") === true) {
            $("." + toggleSections[i]).removeClass("hide-element");
            $("." + toggleSections[i]).show();

        }
        else {
            $("." + toggleSections[i]).addClass("hide-element");
            $("." + toggleSections[i]).hide();

        }
    }
}
function ParseTimeZone(timeZoneName) {
    if (timeZoneName != undefined && timeZoneName != null) {
        timeZoneName = timeZoneName.replace("Daylight", "Standard");
        timeZoneName = timeZoneName.replace("Alaska ", "Alaskan ");
        timeZoneName = timeZoneName.replace("Hawaii-Aleutian", "Hawaiian");
    }
    return timeZoneName
}

function msgsuccess(msgText, msgHeader, msgDuration) {
    msgDuration = typeof msgDuration == undefined || msgDuration == null ? 10000 : msgDuration;
    msgHeader = typeof msgHeader === 'undefined' || msgHeader === null ? 'Success' : msgHeader;
    $.toast({
        heading: msgHeader,
        text: msgText,
        hideAfter: msgDuration,
        icon: 'success',
        loader: false
    });
}
function msginfo(msgText, msgHeader, msgDuration) {
    msgDuration = typeof msgDuration == undefined || msgDuration == null ? 10000 : msgDuration;
    msgHeader = typeof msgHeader === 'undefined' || msgHeader === null ? 'Info' : msgHeader;
    $.toast({
        heading: msgHeader,
        text: msgText,
        hideAfter: msgDuration,
        icon: 'info',
        loader: false
    });
}
function msgwarning(msgText, msgHeader, msgDuration) {
    msgDuration = typeof msgDuration == undefined || msgDuration == null ? 10000 : msgDuration;
    msgHeader = typeof msgHeader === 'undefined' || msgHeader === null ? 'Warning' : msgHeader;
    $.toast({
        heading: msgHeader,
        text: msgText,
        hideAfter: msgDuration,
        icon: 'warning',
        loader: false
    });
}
function msgerror(msgText, msgHeader, msgDuration, msgPosition) {
    msgDuration = typeof msgDuration == undefined || msgDuration == null ? 10000 : msgDuration;
    msgHeader = typeof msgHeader === 'undefined' || msgHeader === null ? 'Error' : msgHeader;
    msgPosition = typeof msgPosition === 'undefined' || msgPosition === null ? 'bottom-left' : msgPosition;
    $.toast({
        heading: msgHeader,
        text: msgText,
        hideAfter: msgDuration,
        icon: 'error',
        loader: false,
        position : msgPosition 
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

function updateDefaultUrls(currency, uom) {
    var sections = $('.add-partial-block');
    $.each(sections, function (idx, ele) {
        var url = $(this).attr('data-url');
        if (url != undefined && url.split('?').length > 1) {
            var urlCurr = replaceUrlParam(url, 'currency', currency);
            var newUrl = replaceUrlParam(urlCurr, 'uoM', uom);
            $(this).attr('data-url', newUrl);
        }
    });
}

function validateWeekendAndSpecialFees(specialDateVal, msg) {
    var isValidForm = true;
    $('.special-date-feetype-ddl').each(function (i, elem) {
        var currentConstraintType = $(elem).val();
        var currentFeeType = $(elem).closest(".fee-type-row").find('.special-feetype-ddl').val();
        var currentFeeDescription = $(elem).closest(".fee-type-row").find('.otherFeeDesc').val();
        var currentSpecialDate = 'none';
        if (currentConstraintType == specialDateVal) {
            currentSpecialDate = $(elem).closest(".fee-type-row").find('.datepicker').val();
        }

        $(elem).closest('.partial-block').nextAll('.partial-block').each(function (j, nextElem) {
            var nextConstraintType = $(nextElem).find('.special-date-feetype-ddl').val();
            var nextFeeType = $(nextElem).find('.special-feetype-ddl').val();
            var nextFeeDescription = $(nextElem).find('.otherFeeDesc').val();
            var nextSpecialDate = 'none';
            if (nextConstraintType == specialDateVal) {
                nextSpecialDate = $(nextElem).find('.datepicker').val();
            }
            if (currentConstraintType == nextConstraintType && currentFeeType == nextFeeType && currentSpecialDate == nextSpecialDate && currentFeeDescription == nextFeeDescription) {
                validationMessageFor($(nextElem).find('.special-date-feetype-ddl').attr('name'), msg);
                validationMessageFor($(elem).attr('name'), msg);
                isValidForm = false;
            }
        });
    });
    return isValidForm;
}

function showHideCityRack(countryId, canadaId) {
    if (countryId == canadaId) {
        $("#chk-enable-cityrack").prop('checked', false);
        $("#IsBuyAndSellOrder").prop('checked', false);
        $("#IsThirdPartyHardwareUsed").prop('checked', false);
        $(".hdr-pricing").text("Pricing");
        inputBoxShowHide(null, 'terminal-price-check');
        inputBoxShowHide(null, 'buy-sell');
        inputBoxShowHide(null, 'third-party-hardware');
    }
    else {
        inputBoxShowHide('terminal-price-check', null);
        inputBoxShowHide('buy-sell', null);
        inputBoxShowHide('third-party-hardware', null);
    }
}

function SetPageCulture(culture) {
    $("#hdnCurrencyCulture").val(culture);
}

function RemoveDryRunOptionForInvoice() {
    var parentForm = $.find('form')[0];
    if (parentForm.id === 'invoice-form' || $(parentForm).hasClass('edit-invoice') || $(parentForm).hasClass('balance-invoice')) {
        var feetypes = $.find("select.feetype-ddl option[value='4']");
        $.each(feetypes, function (idx, dropdown) {
            dropdown.remove();
        });
    }
}

function RemoveProcessingFeeOption() {
    var parentForm = $.find('form')[0];
    if (parentForm.id === 'createofferform' || parentForm.id === 'createquotationform' || parentForm.id === 'createOrderForm' || parentForm.id === 'createFuelRequestForm' || parentForm.id === 'orderBulkUploadForm' || parentForm.id === 'createBrokerOrderForm' || $(parentForm).hasClass('remove-processingfee')) {
        var feetypes = $.find("select.feetype-ddl option[value='13']");
        $.each(feetypes, function (idx, dropdown) {
            dropdown.remove();
        });
    }
}

function focusonErrorControl() {
    var invalidElem = $(".field-validation-error:first");
    if (invalidElem.length > 0) {
        invalidElem.closest(".form-group").find(".form-control").focus();
        $('html,body').animate({
            scrollTop: '+=50px'
        });
    }
}

function fixedButtons(btn, isFormValid) {
    if (isFormValid === undefined || isFormValid === null)
        isFormValid = $(btn).closest("form").valid();

    if (!isFormValid) {
        //$(btn).closest("form").append("<div class='floating-buttons white-bg pt10'><div class='container-fluid'><div class='row'><div class='col-sm-12 text-right buttons-container'></div></div></div></div>");
        //$(".floating-buttons .buttons-container").append($(".form-buttons").html());
        //$(".form-buttons").hide();
        //$('.btnPrev,.btnNext').addClass("hide-onsubmit");
        $(".btnSubmit").addClass("show-onsubmit");
    }
}
function editData(editLink) {
    var stringSection, stringToEdit, editbox;
    stringSection = $(editLink).closest(".edit-section");
    stringToEdit = $(stringSection).find("label").text();
    editbox = "<input type='text' class='form-control'><div class='pull-left dib'><input type='button' class='btn btn-xs ml5 mt5' onClick='cancelEdit(this)' value='Cancel'><input type='button' class='btn btn-xs btn-primary ml5 mt5 saveData' value='Save'></div>";
    $(editLink).closest(".edit-section").prepend(editbox);
    $(stringSection).find("input[type='text']").attr("value", stringToEdit).focus();
    $(editLink).hide();
    $(stringSection).find("label").hide();
}

function cancelEdit(btnCancel) {
    var stringSection;
    stringSection = $(btnCancel).closest(".edit-section");
    $(stringSection).find("input").hide();
    $(stringSection).find("label").show();
    $(stringSection).find("a").show();
    $(stringSection).find('span[data-valmsg-for]').hide();
}
function authbg() {
    //var images = ['authbg-01', 'authbg-02', 'authbg-03', 'authbg-04'];
    var images = ['authbg-02'];
    var randomImage = images[Math.floor(Math.random() * images.length)];
    $(".authbg").addClass(randomImage);
}

function hideLinksForCompanyGroup(ctrl) {
    var groupIds = getCompanyGroup().groupIds;
    if (groupIds !== "" && groupIds !== "-1" && groupIds !== "0") {
        $(ctrl).find('tbody').addClass('disable-anchor-link');
        $(ctrl).find('th.col-group-company-hide, td.col-group-company-hide').hide();
    }
}

function setCompanyGroup(groupIds, selectedGroupName) {
    SetLocalStorage("companyGroupId", groupIds);
    SetLocalStorage("companyGroupName", selectedGroupName);
}

function getCompanyGroup() {
    var groupIds = $.urlParam('groupIds');
    var decodeGroupIds = decodeURIComponent(groupIds);
    var groupName = GetLocalStorage("companyGroupName");

    if (groupIds == "" || groupIds == "-1" || groupIds == "0") {
        groupName = '';
    }
    return { groupIds: decodeGroupIds, groupName: groupName };
}

$.urlParam = function (name) {
    var results = new RegExp('[\?&]' + name + '=([^&#]*)').exec(window.location.href);
    return results === null ? -1 : results[1] || 0;
};

function setDefaultGroupUrl() {
    var groupIds = GetLocalStorage("companyGroupId");
    if (groupIds != '' && groupIds != null && groupIds != "-1" && groupIds != "0") {
        var isParamExists = $.urlParam('groupIds');
        if (isParamExists == -1) {
            var url = replaceUrlParam(window.location.href, 'groupIds', groupIds);
            window.location.href = url;
        }
    }
}

$(document).click(function (event) {
    $('.dropdown-menu[data-parent]').hide();
});

function actionDropdown() {
    $(document).on('click', '.table-responsive [data-toggle="dropdown"], .drlist [data-toggle="dropdown"]', function () {
        $('.dropdown-menu[data-parent]').hide();
       var $buttonGroup = $(this).parent();
        if (!$buttonGroup.attr('data-attachedUl')) {
            var ts = +new Date;
           var $ul = $(this).siblings('ul');
            $ul.attr('data-parent', ts);
            $buttonGroup.attr('data-attachedUl', ts);
            $(window).resize(function () {
                $ul.css('display', 'none').data('top');
            });
        } else {
            $ul = $('[data-parent=' + $buttonGroup.attr('data-attachedUl') + ']');
        }
        if (!$buttonGroup.hasClass('open')) {
            $ul.css('display', 'none');
            return;
        }
        function dropDownFixPosition(button, dropdown) {
            var dropDownTop = button.offset().top + button.outerHeight();
            var dropDownRight = $(window).width() - (button.offset().left + button.outerWidth());
            dropdown.css('position', "absolute");
            dropdown.css('top', dropDownTop);
            dropdown.css('right', dropDownRight);

            dropdown.css('display', 'block');
            dropdown.appendTo('body');
        }
        dropDownFixPosition($(this), $ul);
    });
}
actionDropdown();
$(document).ajaxComplete(function () {
    actionDropdown();
});

function ToggleSingleDeliverySubTypes() {
    if ($('.rdb-deliverytype[value=2]:checked').length === 1) { // if multiple delivery is checked
        disableElement($('.singleDeliverySubTypes').addClass('subSectionOpacity'));
    }
    else {
        enableElement($('.singleDeliverySubTypes').removeClass('subSectionOpacity'));
    }
}

function ToggleEndDate() {
    if ($('.rdb-deliverytype[value=1]:checked').length === 1 && $('.singleDeliverySubTypes option[value=0]:selected').length === 1) { // single delivery & specific date option selected
        $('.delivery-end-date').val('');
        $('.delivery-end-date').attr('disabled', 'disabled');
    }
    else {
        $('.delivery-end-date').removeAttr('disabled');
        $('.delivery-end-date').removeAttr('readonly');
    }

    if ($('.rdb-deliverytype[value=1]:checked').length === 1) {
        $('.delivery-expiry-date').val('');
        $('.delivery-expiry-date').attr('disabled', 'disabled');
    }
}

// mini jQuery plugin that formats to spcified decimal places
(function ($) {
    $.fn.decimalPlaceFormat = function (digit) {
        this.each(function (i) {
            $(this).change(function (e) {
                if (isNaN(parseFloat(this.value))) { return; }
                this.value = parseFloat(this.value).toFixed(digit);
            });
        });
        return this; //for chaining
    };
})(jQuery);

function AllowOnlyIntegers(elem) {
    elem.value = elem.value.replace(/[^0-9]/g, '');
}

function getMapDotUrl(statusId) {
    var url = 'https://maps.google.com/mapfiles/ms/icons/';
    switch (statusId) {
        case 1:
            return url + 'purple-dot.png';
        case 11:
            return url + 'yellow-dot.png';
        case 12:
            return url + 'orange-dot.png';
        case 13:
            return url + 'red-dot.png';
        case 16:
            return url + 'pink-dot.png';
        case 18:
            return url + 'green-dot.png';
        case 20:
            return url + 'red-dot.png';
        default:
            return url + 'blue-dot.png';
    }
}

function showHideElementOnRadioChange(ele, eleName) {
    if ($(ele).is(':checked')) {
        $('.' + eleName).slideDown();
    }
    else {
        $('.' + eleName).slideUp();
    }
}

function togglediv(element, className) {
    var isChecked = $(element).is(":checked");
    if (isChecked) {
        $("." + className).find("input,select").removeAttr("disabled");
    }
    else {
        $("." + className).find("input,select").attr("disabled", true);
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

function getPricingSource() {
    var source = 1;
    var sourceElem = $('.ddl-pricing-source');
    if (sourceElem.length > 0) {
        source = parseInt(sourceElem.val(), 10);
    }
    return source;
}

function EnableRackHighLow() {
    $('.ddl-market-types').removeAttr('readonly');
}

function DisableRackHighLow() {
    //$('.ddl-market-types').val(1); // set rack avg
    $(".ddl-market-types option:first").prop('selected', true);
    $('.ddl-market-types').attr('readonly', 'readonly');
}

function IsNumeric(input) {
    return (input - 0) == input && ('' + input).trim().length > 0;
}

function showEmailDocPopup(id, number, desc, companyTypeId, docName, url) {
    var emailPopupContainer = $('#email-document-popup-container');
    var emailPopup = emailPopupContainer.children('#email-document-modal-popup');
    if (emailPopup !== undefined && emailPopup.length > 0) {
        clearEmailPopupControls();
        showEmailDocumentPopup();
    }

    var data = {
        id: id,
        number: number,
        companyType: companyTypeId,
        desc: desc,
        docName: docName
    };

    $.get(url, data, function (response) {
        $('#email-document-popup-container').html(response);
    }).always(function () { });
}

function calculateTankTaxAmount(elem) {
    var taxAmountElem = $(elem).closest('.tank-row').find('.tax-amount');
    var taxPercentageElem = $(elem).closest('.tank-row').find('.tax-percentage').val();
    var feeElem = $(elem).closest('.tank-row').find('.calculated-rental-amount').val();

    if (IsNumeric(feeElem) && IsNumeric(taxPercentageElem)) {
        taxAmountElem.val((feeElem * taxPercentageElem / 100).toFixed(2));
    }
    else {
        taxAmountElem.val('');
    }
}
function removeParentSection(elem, sectionClass) {
    $(elem).closest("." + sectionClass).remove();
}
function loadSidePanel() {
    $(".panel-icon").toggleClass("icon-active");
    $(".panel-section").toggleClass("hide-element");
}
function loadLeftSidePanel(paneltab) {
    if ($(paneltab).closest('#slider-content').length > 0) {
        $(paneltab).closest('#slider-content').find(".panel-icon").toggleClass("icon-active");
        $(paneltab).closest('#slider-content').find(".panel-section").toggleClass("hide-element");
    }
    else {
        $(".panel-icon").toggleClass("icon-active");
        $(".panel-section").toggleClass("hide-element");
    }
}
function closeLeftSidePanel(paneltab) {
    if ($(paneltab).closest('#slider-content').length > 0) {
        $(paneltab).closest('#slider-content').find(".panel-icon").toggleClass("icon-active");
        $(paneltab).closest('#slider-content').find(".panel-section").toggleClass("hide-element");
    }
    else {
        $(".panel-icon").toggleClass("icon-active");
        $(".panel-section").toggleClass("hide-element");
    }
}
function closePanel() {
    $(".panel-icon").toggleClass("icon-active");
    $(".panel-section").toggleClass("hide-element");
}


var dataGridRq;
function registerExportDataEvent(dataGridId, exportRestrictionCount, dataGridUrl, messageExportRestriction, responseCallback) {
    var $DataTable = $(dataGridId);
    jQuery.fn.DataTable.Api.register('buttons.exportData()', function (options) {
        if (this.context.length) {
            var setting = $DataTable.dataTable().fnSettings();
            var totalRecs = setting.fnRecordsTotal();
            dataGridRq.length = 99999999;
            if (totalRecs > exportRestrictionCount) {
                msgerror(messageExportRestriction);
                return;
            }

            var jsonResult = $.ajax({
                url: dataGridUrl,
                type: 'POST',
                data: dataGridRq,
                success: function (result) {
                    //Do nothing
                },
                async: false
            });

            var filteredJson = responseCallback(jsonResult);

            return {
                body: filteredJson.map(function (el) {
                    return Object.keys(el).map(function (key) {
                        return el[key];
                    });
                }),
                header: $(dataGridId + " thead tr th").map(function () {
                    if (!$(this).hasClass('exclude-export')) {
                            return this.innerText;
                    }                      
                }).get()
            };
        }
    });
}


function ExportDataEvent(dataGridId, exportRestrictionCount, dataGridUrl, messageExportRestriction) {
    var $DataTable = $(dataGridId);
    jQuery.fn.DataTable.Api.register('buttons.exportData()', function (options) {
        if (this.context.length) {
            var setting = $DataTable.dataTable().fnSettings();
            var totalRecs = setting.fnRecordsTotal();
            dataGridRq.length = 99999999;
            if (totalRecs > exportRestrictionCount) {
                msgerror(messageExportRestriction);
                return;
            }

            var jsonResult = $.ajax({
                url: dataGridUrl,
                type: 'POST',
                data: dataGridRq,
                success: function (result) {
                    //Do nothing
                },
                async: false
            });

            var filteredJson = $.map(jsonResult.responseJSON.data, function (item) {
                var retObj = {};
                for (var idx = 0; idx < setting.aoColumns.length; idx++) {
                    var currentCol = setting.aoColumns[idx];
                    if (currentCol.bVisible === false || currentCol.sName === undefined || currentCol.sName === null || currentCol.sName === '')
                        continue;

                    retObj[currentCol.sName] = item[currentCol.sName];
                }
                return retObj;
            });

            return {
                body: filteredJson.map(function (el) {
                    return Object.keys(el).map(function (key) {
                        if (!el[key]) {
                            el[key] = "";
                        }
                        return el[key];
                    });
                }),
                header: $(dataGridId + " thead tr th").map(function () {
                    if (!$(this).hasClass('exclude-export')) {
                        return this.innerText;
                    }
                }).get()
            };
        }
    });
}

Element.prototype.remove = function () {
    this.parentElement.removeChild(this);
}
NodeList.prototype.remove = HTMLCollection.prototype.remove = function () {
    for (var i = this.length - 1; i >= 0; i--) {
        if (this[i] && this[i].parentElement) {
            this[i].parentElement.removeChild(this[i]);
        }
    }
}

$(function () {
    // Multiple images preview in browser
    var imagesPreview = function (input, placeToInsertImagePreview) {

        if (input.files) {
            var filesAmount = input.files.length;


            for (i = 0; i < filesAmount; i++) {
                var reader = new FileReader();
                console.log(input.files.item(i).name);

                reader.onload = function (event) {
                    $($.parseHTML('<img  style="border:1px dashed black;margin:0 10px 10px 0;padding:5px;width:31.5%;height:150px">')).attr({ src: event.target.result }).appendTo(placeToInsertImagePreview);
                };
                reader.readAsDataURL(input.files[i]);
            }
        }
    };

    $('#gallery-photo-add').on('change', function () {
        imagesPreview(this, 'div.gallery');
    });
});

function closeSlidePanel() {
    var sidePanel = $('.side-panel');
    sidePanel.animate({ "right": -docWidth + 'px' }, "fast");
    $('#slider-content').html('');
    $('.side-panel').css('display', 'none');
    $('.side-panel-preview').css('display', 'none');
    showBodyScroll();
}
$(function () {
    docWidth = $(window).width();
    docHeight = $(window).height();
    $('.side-panel').css('right', -docWidth + 'px');
    $('.side-panel').css('width', '95%');
    $('.side-panel').height(docHeight);
});

function slidePanel(PanelId, PanelWidth) {
    //$(PanelId).animate({ "right": "0px" }, 10000, 'linear').css('display', 'inline-block');
    $(PanelId).toggle('slide', { direction: 'right' }, 'fast');
    //$(PanelId).width(PanelWidth);
    $('.side-panel-wrapper').width(PanelWidth);
    showBodyScroll();
    docHeight = $(window).height();
    $('.side-panel').height(docHeight);
}
$(window).resize(function () {
    docHeight = $(window).height();
    $('.side-panel').height(docHeight);
});
function removeBodyScroll() {
    //$("body,html").css("overflow-y", "hidden");
}
function showBodyScroll() {
    //$("body,html").css("overflow", "auto");
}

function slideInvoiceDetails(currentId) {
    $('#slider-content').removeClass('side-panel-body');
    $('#sliderpanel').animate({ "right": "0px" }, "fast").css('display', 'inline-block').width('95%');
    $('#slider-loader').show();
    var url = '/Supplier/Invoice/DetailsTab/';
    removeBodyScroll();
    $.ajax({
        type: "Get",
        url: url + currentId,
        data: { id: currentId },
        success: function (data) {
            $('#slider-heading').html($(data).find('#lblInvoiceNumber').html());
            $('#slider-content').html(data);
            $('.slidedetails').removeAttr('href');
            $('.slidedetails').each(function () {
                $(this).on('click', function () {
                    slideInvoiceDetails($(this).data('invoiceid'));
                });
            });
            $('.tab-scroll').height(docHeight);
            $('#slider-loader').hide();
        }
    }).always(function () { $('#slider-loader').hide(); });
}

function slideAssetTankDetails(currentId, type, allowEdit, isDetails, isBuyer, jobId, orderId) {
    $('#sliderpanel').animate({ "right": "0px" }, "fast").css('display', 'inline-block').width('95%');
    $('#slider-loader').show();
    var url = "";
    var inputData = {
        id: currentId,
        type: type,
        isJobDetails: isDetails
    };
    if (isBuyer) {
        if (allowEdit) {
            url = '/Buyer/Asset/Create/';
        }
        else {
            url = '/Buyer/Asset/Details/';
        }
    }
    else {
        inputData = {
            oId: orderId,
            jId: jobId,
            type: type,
            assetId: currentId,
            isCallFromOrderDetails: isDetails
        };
        if (allowEdit) {
            url = '/Supplier/Order/CreateAsset/';
        }
        else {
            url = '/Supplier/Asset/Details/';
        }
    }
    $.ajax({
        type: "Get",
        url: url + currentId,
        data: inputData,
        success: function (data) {
            $('#slider-content').html(data);
            parseForm();
            if (type == 1 && allowEdit) {
                $("#slider-heading").text('Edit Asset');
            }
            else if (type == 2 && allowEdit) {
                $("#slider-heading").text('Edit Tank');
            }
            else if (type == 1 && !allowEdit) {
                $("#slider-heading").text('Asset Details');
            }
            else if (type == 2 && !allowEdit) {
                $("#slider-heading").text('Tank Details');
            }
            $('.tab-scroll').height(docHeight);
            $('#slider-loader').hide();
            $(".lblforcastingNote").text('');
            $(".lblforcastingNote").hide();
        }
    })
        .done(function () {
            getSupplierPreferenceSetting();
            $('.timepicker').datetimepicker({
                format: 'LT',
            });
        })
        .always(function () { $('#slider-loader').hide(); });
}

function closeDG() {
    $('.delivery-panel').removeClass('show-element').addClass('hide-element');
    $('.delivery-panel_body').removeClass('display_body');
}
function showDG() {
    $('.delivery-panel').removeClass('hide-element').addClass('show-element');
    $('.delivery-panel').addClass('animated slideInRight').removeClass('slideOutRight');
    ($('.delivery-panel').hasClass('show-element') == true) ? $('.delivery-panel').removeClass('slideInRight').addClass('bounce') : $('.delivery-panel').addClass('slideInRight');
    setTimeout(function () {
        $('.delivery-panel').removeClass('bounce');
    }, 800);
}

function showOpenDG() {
    $('.delivery-panel').removeClass('hide-element').addClass('show-element');
    $('.delivery-panel_body').addClass('display_body');
    toggleGroupIcon();
}

function toggleGroupIcon() {
    var delivery_group_icon = $('#delivery_group_icon i');
    (delivery_group_icon.hasClass('fa-chevron-up') == true) ? delivery_group_icon.removeClass('fa-chevron-up').addClass("fa-chevron-down") : delivery_group_icon.removeClass('fa-chevron-down').addClass("fa-chevron-up");
    ($('.delivery-panel').hasClass('bounce') == true) ? $('.delivery-panel').removeClass('bounce') : '';
}

$(document).ready(function () {
 
    $(document).on('click', "#delivery_group_icon", function () {
        ($('#delivery_group_icon').hasClass('fa-chevron-down') == true) ? $('.delivery-panel').addClass('animated bounce') : $('.delivery-panel').removeClass('bounce');
        $('.delivery-panel_body').toggleClass('display_body');
        toggleGroupIcon();
    });
    //$(document).on('click', "#toggle_dr_panel_btn", function () {
    //    ($('#toggle_dr_panel_id').hasClass('col-sm-2') == true) ? $(".toggle-dr-panel").removeClass('col-sm-2 slideInRight').addClass("col-sm-5 animated slideInLeft") : $(".toggle-dr-panel").removeClass('col-sm-5 slideInLeft').addClass("col-sm-2");
    //    ($('#schedule-panel_id').hasClass('col-sm-10') == true) ? $(".schedule-panel").removeClass('col-sm-10').addClass("col-sm-7") : $(".schedule-panel").removeClass('col-sm-7').addClass("col-sm-10");
    //    //($('#toggle_dr_panel_id').hasClass('col-sm-2') == true) ? $(".dr_cards").removeClass('col-sm-4').addClass("col-sm-12") : $(".dr_cards").removeClass('col-sm-12').addClass("col-sm-4");
    //    ($('#toogle_panel_icon').hasClass('fa-arrow-circle-right') == true) ? $("#toogle_panel_icon").removeClass('fa-arrow-circle-right').addClass("fa-arrow-circle-left") : $("#toogle_panel_icon").removeClass('fa-arrow-circle-left').addClass("fa-arrow-circle-right");
    //    ($('#toggle_dr_panel_id').hasClass('col-sm-2') == true) ? $(".dr_cards_new").removeClass('col-sm-4').addClass("col-sm-12") : $(".dr_cards_new").removeClass('col-sm-12').addClass("col-sm-4");
    //});
    $(document).on('click', "#icon_collapse1", function () {
        ($('.collapse1_icon').hasClass('fa-chevron-up') == true) ? $(".collapse1_icon").removeClass('fa-chevron-up').addClass("fa-chevron-down") : $(".collapse1_icon").removeClass('fa-chevron-down').addClass("fa-chevron-up");
        //$("#collapse2").removeClass('in');
        //$("#collapse3").removeClass('in');
        //$(".collapse2_icon").removeClass('fa-chevron-up').addClass("fa-chevron-down");
        //$(".collapse3_icon").removeClass('fa-chevron-up').addClass("fa-chevron-down");
    });
    $(document).on('click', "#icon_collapse2", function () {
        ($('.collapse2_icon').hasClass('fa-chevron-up') == true) ? $(".collapse2_icon").removeClass('fa-chevron-up').addClass("fa-chevron-down") : $(".collapse2_icon").removeClass('fa-chevron-down').addClass("fa-chevron-up");
        //$("#collapse1").removeClass('in');
        //$("#collapse3").removeClass('in');
        //$(".collapse1_icon").removeClass('fa-chevron-up').addClass("fa-chevron-down");
        //$(".collapse3_icon").removeClass('fa-chevron-up').addClass("fa-chevron-down");
    });
    $(document).on('click', "#icon_collapse3", function () {
        ($('.collapse3_icon').hasClass('fa-chevron-up') == true) ? $(".collapse3_icon").removeClass('fa-chevron-up').addClass("fa-chevron-down") : $(".collapse3_icon").removeClass('fa-chevron-down').addClass("fa-chevron-up");
        //$("#collapse2").removeClass('in');
        //$("#collapse1").removeClass('in');
        //$(".collapse2_icon").removeClass('fa-chevron-up').addClass("fa-chevron-down");
        //$(".collapse1_icon").removeClass('fa-chevron-up').addClass("fa-chevron-down");
    });
    $(document).on('click', "#icon_collapse4", function () {
        ($('.collapse4_icon').hasClass('fa-chevron-up') == true) ? $(".collapse4_icon").removeClass('fa-chevron-up').addClass("fa-chevron-down") : $(".collapse4_icon").removeClass('fa-chevron-down').addClass("fa-chevron-up");
    });

    //$(document).mouseup(function (e) {
    //    var container = $("#sliderpanel");
    //    if (!container.is(e.target) && container.has(e.target).length === 0) {
    //        container.hide();
    //    }
    //});
    $(document).on("hide.bs.collapse show.bs.collapse", "#accordion-location", function (e) {
        $(e.target).prev().find("i:last-child").toggleClass("fa-minus fa-plus");
        $(e.target).prev('.card-header').toggleClass("bg-change", "");
        $(e.target).prev().find(".btn-link").toggleClass("f-bold", "");
    });

    $(document).on("hide.bs.collapse show.bs.collapse", "#accordionExitingDrReq", function (e) {
        $(e.target).prev().find("i:last-child").toggleClass("fa-angle-up fa-angle-down");
        $(e.target).prev('.card-header').toggleClass("bg-change", "");
        $(e.target).prev().find(".btn-link").toggleClass("f-bold", "");
    });

    //coloumn visibility dropdown position reintialize code///
    $(document).on('click', ".buttons-colvis", function (event) {
        var x = {}, top = 0, left = 0;
        x = $(this).offset();
        top = x.top + $(this).outerHeight();
        left = x.left - $(".dt-button-collection").outerWidth() + $(this).outerWidth();
        $(".dt-button-collection").css({ 'top': top + 'px', 'left': left + 'px' });
    });
    //coloumn visibility dropdown position reintialize code///
});

//Multiple select box customization code
function applyCssToMultiSelect() {
    $(".multiselect.dropdown-toggle").parent(".btn-group").addClass("w-100");
    $(".multiselect.dropdown-toggle").next(".multiselect-container.dropdown-menu").addClass("w-100");
    //$(".multiselect-container.dropdown-menu").find(".checkbox").css({ "display": "flex", "align-items": "flex-end" });
    $(".multiselect-container.dropdown-menu").find('input[type=checkbox]').css({ "margin-right": "5px" });
    $(".multiselect.dropdown-toggle").css({ "display": "flex", "align-items": "center", "justify-content": "space-between", "margin-left": "0px" });
}


function hideModal(selector) {
    $(selector).modal('hide');
}

function showModal(selector) {
    $(selector).modal('show');
}

function openBuyerTankMakeGrid() {
    $('#sliderpanel').animate({ "right": "0px" }, "fast").css('display', 'inline-block').width('70%');
    $("#slider-heading").text('Tank Dip Chart');

    $('#slider-loader').show();
    var url = '/Buyer/Asset/GetTankTypesGrid/';
    $.ajax({
        type: "Get",
        url: url,
        success: function (data) {
            $('#slider-content').html(data);
            $('.tab-scroll').height(docHeight);
        }
    }).always(function () { $('#slider-loader').hide(); });
}
function openSupplierTankMakeGrid() {

    $('#sliderpanel').animate({ "right": "0px" }, "fast").css('display', 'inline-block').width("70%");
    $("#slider-heading").text('Tank Dip Chart');

    $('#slider-loader').show();
    var url = '/Supplier/Order/GetTankTypesGrid';
    $.ajax({
        type: "Get",
        url: url,
        success: function (data) {
            $('#slider-content').html(data);
            $('.tab-scroll').height(docHeight);
        }
    }).always(function () { $('#slider-loader').hide(); });
}
function formatDate(date) {
    var hours = date.getHours();
    var minutes = date.getMinutes();
    var ampm = hours >= 12 ? 'pm' : 'am';
    hours = hours % 12;
    hours = hours ? hours : 12; // the hour '0' should be '12'
    minutes = minutes < 10 ? '0' + minutes : minutes;
    var strTime = hours + ':' + minutes + ' ' + ampm;
    return (date.getMonth() + 1) + "/" + date.getDate() + "/" + date.getFullYear() + "  " + strTime;
}
function ReplaceNumberWithCommas(number) {
    //Seperates the components of the number
    var components = number.toString().split(".");
    //Comma-fies the first part
    components[0] = components[0].replace(/\B(?=(\d{3})+(?!\d))/g, ",");
    //Combines the two sections
    return components.join(".");
}

function slideSupplierProductMapping(currentId) {
    $('#sliderpanel').animate({ "right": "0px" }, "fast").css('display', 'inline-block').width('95%');
    $('#slider-loader').show();
    var url = '/Supplier/ProductMapping/Details/';
    var inputData = { id: currentId };

    $.ajax({
        type: "Get",
        url: url + currentId,
        data: inputData,
        success: function (result) {
            $('#slider-content').html(result);
            parseForm();
            $("#slider-heading").text('Edit Mapped Product');
            $('.tab-scroll').height(docHeight);
            $('#slider-loader').hide();
        }
    })
        .done(function () {
            getSupplierPreferenceSetting();
        })
        .always(function () { $('#slider-loader').hide(); });
}
function toggleTrailerType(checkboxId, selectListId) {
    if ($('#' + checkboxId).is(':checked')) {
        $('.divTrailerType').show();
        $('#' + selectListId + ' > option').prop("selected", "selected");
        $('#' + selectListId).trigger("change");
    }
    else {
        $('.divTrailerType').hide();
        $('#' + selectListId + ' > option').prop("selected", "selected");
        $('#' + selectListId).trigger("change");
    }
}

function ShowTrailerType(selectListId) {
   
        $('.divTrailerType').show();
        $('#' + selectListId + ' > option').prop("selected", "selected");
        $('#' + selectListId).trigger("change");
   
}
function showSliderPanel() {
    $('#slider-content').removeClass('side-panel-body');
    $('#sliderpanel').animate({ "right": "0px" }, "fast").css('display', 'inline-block').width('95%');
    $('#slider-loader').show();
}
function hideSliderLoader() {
    $('#slider-loader').hide();
}
function appendHTMLSliderContent(selector) {
    $('#slider-content').html($(selector).html());
    $('#slider-content').addClass('side-panel-body');
}
function validatePositiveNumber(event, obj) {
    var response = true;
    response = (event.charCode == 8 || event.charCode == 0 || event.charCode == 13) ? null : event.charCode >= 48 && event.charCode <= 57;
    if (response) {
        if (((obj.value == "" || obj.value <= 0) && event.charCode == 48)) {
            response = false;
        }
    }
    return response;
}
function editAccountingCompanyIdForJob(BuyerCompanyId, JobId, url) {
    $(".edit-loader").show();
    $.get(url, { buyerCompanyId: BuyerCompanyId, jobId: JobId }, function (response) {
        if (response) {
            $("#edit-Id-form").html(response);
            parseForm();
        }
    }).always(
        function () {
            $(".edit-loader").hide();
        }
    );
}
function saveAccountCompanyIdForJob() {
    var $form = $("#EditAccCompForm");
    var isValid = $form.valid();
    var url = $form.attr("action");
    var data = $form.serialize();
    $.post(url, data, function (response) {
        if (response.StatusCode == 0) {
            $("#accoutingCompanyId-modal").modal("hide");
            msgsuccess(response.StatusMessage);
            window.location.reload(true);
        } else {
            msgerror(response.StatusMessage);
        }
    });

}
function CallConfirmBootBox(msg, yesFunction, noFunction) {
    bootbox.confirm({
        message: msg,
        closeButton: false,
        buttons: {
            cancel: {
                label: "No",
                className: "btn-sm btn-danger"
            },
            confirm: {
                label: "Yes",
                className: "btn-sm btn-primary"
            }
        },
        callback: function (res) {
            if (res) {
                //savePoNumber();
                yesFunction();
            } else {
                //noFunction();
            }
        }
    });
}


function CallCustomBootBox(title, html, yesFunction, noFuction) {
    var dialog = bootbox.dialog({
        title: title,
        message: html,
        size: 'large',
        buttons: {
            cancel: {
                label: "Cancel",
                className: 'btn-danger',
                callback: function () {
                    noFuction();
                }
            },
            ok: {
                label: "Save/Ok",
                className: 'btn-info',
                callback: function () {
                    yesFunction();
                }
            }
        }
    });
}
function validateForcastingData(countryCode) {
    if (window.location.href.indexOf("Onboarding/Company") > -1) {
        var uomDetails = 'Gallons';
        if (countryCode == 'CA') {
            uomDetails = "Litres";
        }
        $(".minuomName").text(uomDetails);
        $(".avguomName").text(uomDetails);
        var inventoryPriorityType = $("input[name='ForcastingPreference.ForcastingServiceSetting.InventoryPriorityType']:checked").val();
        if ($("input[name='ForcastingServiceSetting.InventoryPriorityType']").length > 0) {
            inventoryPriorityType = $("input[name='ForcastingServiceSetting.InventoryPriorityType']:checked").val();
        }
        var forcastingInventoryUOM = $(".forcastingInventoryUOM ").val();
        if (inventoryPriorityType == 1 && forcastingInventoryUOM == 1) {
            $(".uomName").each(function () {
                $(this).html(uomDetails);
            });
        }
    }
}

function validateCompanyName(isNewCompany, companyName) {
    var url = "/Validation/IsExistingCompany";
    var inputModel = { 'IsNewCompany': isNewCompany, 'CompanyName': companyName };
    $.ajax({
        type: "GET",
        url: url,
        data: inputModel,
        dataType: "json",
        success: function (data) {
            if (data != null && data == true) {                
                validationMessageFor("CustomerDetails.CompanyName", 'Company already exists');
                return true;
            }
            else {
                return false;
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            var error = errorThrown;
        }
    });
}

function formatTime(timeInput) {
    
    intValidNum = timeInput.value;

    if (intValidNum <= "99" && intValidNum.length == 2) {
        timeInput.value = timeInput.value + ":";
        return false;
    }
    if (intValidNum > "99" && intValidNum.length == 2) {
        timeInput.value = "";
        return false;
    }
    if (intValidNum.length == 5 && intValidNum.slice(-2) < "6") {
        timeInput.value = timeInput.value.substr(0, timeInput.value.length - 1);
        return false;
    }
    if (intValidNum.length == 5 && intValidNum.slice(-1) > "60") {
        timeInput.value = timeInput.value.substr(0, timeInput.value.length - 1);
        return false;
    }

    if (intValidNum.length == 5 && intValidNum.slice(-2) == "60") {
        timeInput.value = timeInput.value.slice(0, 2) + ":00:";
        return false;
    }
    if (intValidNum.length == 4 && intValidNum.slice(-1) > "5") {
        timeInput.value = timeInput.value.substr(0, timeInput.value.length - 1);
        return false;
    }
}

function convertDateToUTCMilliseconds(strDate) {
    if (strDate) {
        var date = new Date(strDate);
        //Convert Date to UTC Date in Milliseconds
        var utcMilliseconds = Date.UTC(date.getUTCFullYear(), date.getUTCMonth(), date.getUTCDate(),
                              date.getUTCHours(), date.getUTCMinutes(), date.getUTCSeconds());

        //convert long milliseconds to string milliseconds and return
        var strUTCMilliseconds = utcMilliseconds.toString();

        return strUTCMilliseconds;
    }

}