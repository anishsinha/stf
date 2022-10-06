$(document).ready(function () {
    dashboardDocumentReady();
    $(".loader").hide();
    $.extend(true, $.fn.dataTable.defaults, {
        fixedHeader: true
    });
    noFixedHeaderSmallDevices();
});

function noFixedHeaderSmallDevices() {
    var windowWidth = $(document).width();
    if (windowWidth < 1200) {
        $.extend(true, $.fn.dataTable.defaults, { fixedHeader: false });
    }
}

function fixedHeader() {
    $('.dataTable').on('init.dt', function (e, settings, json) {
        var tableWidth = $(this).width();
        var windowWidth = $(document).width();
        if (tableWidth > 1200) {
            $.extend(true, $.fn.dataTable.defaults, { fixedHeader: false });
        }
    });
    noFixedHeaderSmallDevices();
}

function tabGridHeader(table) {
    var tablewidth = $(table).width();
    var wrapperwidth = $(table).closest(".ibox-content").width();
    if (tablewidth > wrapperwidth) {
        $.extend(true, $.fn.dataTable.defaults, { fixedHeader: false });
    }
    else if ($(table).hasClass("show-fixedheder")) {
        $.extend(true, $.fn.dataTable.defaults, { fixedHeader: true });
    }
    else {
        $.extend(true, $.fn.dataTable.defaults, { fixedHeader: true });
        $(".tab-container .dataTable").addClass("small-grid");
    }
    noFixedHeaderSmallDevices();
}

$(window).on('beforeunload', function () {
    $(".loader").show();
});

$(document).ajaxComplete(function () {
    $(".spinner-ajax").remove();
    $(".add-partial-block").removeClass("pntr-none subSectionOpacity");
    changePhoneFormat();
    fixedHeader();
});

function getDashboardFilter() {
    return getSelectedCountryAndCurrency();
}

function changePhoneFormat() {
    $(".phone").text(function (i, text) {
        text = text.replace(/(\d{3})(\d{3})(\d{4})/, "$1-$2-$3");
        return text;
    });
}
//Check to see if the window is top if not then display button
$(window).scroll(function () {
    if ($(this).scrollTop() > 100) {
        $('.scroll-to-top').show().removeClass("fadeOutDown").addClass("fadeInUp");
    } else {
        $('.scroll-to-top').removeClass("fadeInUp").addClass("fadeOutDown");
    }
});

//Click event to scroll to top
$(document).on('click', '.scroll-to-top', function (event) {
    event.preventDefault();
    $('html, body').animate({ scrollTop: 0 }, 400);
    return false;
});

// Loader for FR all-public-private toggle
function frToggle() {
    $(document).ajaxStart(function () {
        $(".fr-type").show();
    });
    $(document).ajaxComplete(function () {
        $(".fr-type").hide();
    });
}


function dashboardDocumentReady() {
    $(document).on('blur', ".input-phoneformat", function () {
        var phonenumber = $(this).val().toString().replace(/(\d{3})(\d{3})(\d{4})/, "$1-$2-$3");
        $(this).val(phonenumber);
    });

    changePhoneFormat();
    //dashboards - allowing user to click on row and see the fuel request
    $('#table-last5ActiveOrders tbody, #table-last5ActiveQuoteRequests tbody').on('click', 'tr', function (event) {
        var requestUrl = $(this).find("td:first-child a").attr("href");
        if (requestUrl != undefined) {
            window.location.href = requestUrl;
        }
    });
    // Hide loading modal on click of all download buttons
	$(document).on('click', ".btn-download", function () {
		hideLoader();
    });
    // Insert spinner next to partial view link
    $(document).on('click', '.add-partial-block', function () {
        $("<span class='spinner-ajax pull-left ml10 mt3'></span>").insertAfter(this);
        $(".spinner-ajax").show();
        $(this).addClass("pntr-none subSectionOpacity");
    });

    //Close nav
    $(".close-menu").click(function () {
        $("nav").toggleClass("slideInLeft slideOutLeft");
        $("nav").css("animation-fill-mode", "both");
    });
    // nav takes height of screen
    $('nav,.bg-auth,.controls-auth').css("height", $(window).height() - 45);
    var notification_count = $(".notification-count").text();
    if (notification_count >= 100) {
        $(".notification-count").text("99+");
    }

    $(".tab-headers > a").click(function () {
        wrapperHeight();
    });
    // expand-collapse left navigation
    $(".nav > li > a").click(function () {
        var clickedElement = $(this);
        if ($(this).attr("aria-expanded") === "true") {
            $(this).attr("aria-expanded", false);
            //$(this).collapse({ "toggle": false, "parent": "#navParent" });
            //$(this).removeClass("in");
        }
        else {
            $("nav ul li a").each(function () {
                if ($(this) !== clickedElement) {
                    $(this).attr("aria-expanded", false);
                    //$(this).collapse({ "toggle": false, "parent": "#navParent" });
                    $(this).next(".show").attr("aria-expanded", false);
                    $(this).next(".submenu").removeClass("show");
                    $(this).find("span .fa-angle-right,.fa-angle-down").toggleClass("fa-angle-right fa-angle-down");
                }
            });
        }

        $(this).find(".fa-angle-right,.fa-angle-down").toggleClass("fa-angle-right fa-angle-down");
    });

    //Slide effect to nav
    $("#mobile-menu-toggle").click(function () {
        if ($("nav").hasClass("hide-element")) {
            $("nav").toggleClass("hide-element show-element");
        }
        $("nav").toggleClass("slideInLeft slideOutLeft");
    });
    $("body").click(function (event) {
        if ($("nav").hasClass("slideInLeft") && event.target.localName != "nav" && event.target.parentElement.parentElement.localName != "nav" && !$(event.target).closest("a").parent().hasClass("main-menu")) {
            $("nav").toggleClass("slideInLeft slideOutLeft");
        }
        $("nav").css("animation-fill-mode", "both");

        //if (!$(".panel-section").hasClass("hide-element") && !$(".panel-icon").hasClass("icon-active")) {
        //    $(".panel-section").toggleClass("hide-element");
        //}
        //$(".calendar-section").addClass("hide-element");
        //$(".calendar-icon").removeClass("calendar-active");
    });

    $('#mobile-menu-toggle').on('click', function (event) {
        event.stopPropagation();
        if ($(".calendar-section").is(":visible")) {
            $(".calendar-section").toggleClass("hide-element");
            $(".calendar-icon").toggleClass("calendar-active");
        }
    });
    //$('.calendar-icon,.calendar-section').on('click', function (event) {
    //    event.stopPropagation();
    //});


    //wrapperHeight();
    if ($('main').find('.ibox').length == 0) {
        wrapperHeight();
    }

    tabNextPrev();

};
$(document).on('keyup', function (evt) {
    if (evt.keyCode == 27) {
        $("nav").removeClass("slideInLeft");
        $("nav").addClass("slideOutLeft");
        $(".calendar-section,.panel-section").addClass("hide-element");
        $(".calendar-icon").removeClass("calendar-active");
        $(".panel-icon").removeClass("icon-active");
        $("nav").css("animation-fill-mode", "both");
    }
});
$(window).resize(function () {
    wrapperHeight();
    $('nav,.bg-auth,.controls-auth').css("height", $(window).height() - 60)
    if ($(window).width() < 991) {
        $("nav").removeClass("cozy-menu");
        $('header').removeClass("large-header");
        $('main').removeClass("large-main");
    }
    else if ($(window).width() > 991) {
        $('header').removeClass("mobile-header");
    }
});
function showsection(sectionid) {
    $(".tab-container > div").hide();
    $(".tab-container #" + sectionid).show();
    wrapperHeight();
}
function resetsection(sectionid) {
    $(".tab-container #" + sectionid).siblings().children().html("");
    $(".tab-container > div").hide();
    $(".tab-container #" + sectionid).show();
    wrapperHeight();
}
function showPanelsection(sectionid) {
	if ($('#' + sectionid).closest('#slider-content').length > 0) {
		$('#' + sectionid).closest('#slider-content').find('.tab-container > div').hide();
		$('#' + sectionid).closest('#slider-content').find(".tab-container #" + sectionid).show();
	}
	else {
		$(".tab-container > div").hide();
		$(".tab-container #" + sectionid).show();
	}
	wrapperHeight();
}
function activelink(element) {
    $(element).closest(".tab-headers").find("> a").removeClass("active");
    $(element).addClass("active");
    tabNextPrev();
}
function activePanellink(element) {
	if($(element).closest('#slider-content').length > 0){
		$(element).closest('#slider-content').find(".tab-headers > a").removeClass("active");
		$(element).addClass("active");
	}
	else
	{
		$(".tab-headers > a").removeClass("active");
		$(element).addClass("active");
	}
	tabNextPrev();
}
function wrapperHeight() {
    $(window).scrollTop(0);
}

function tabNextPrev() {
    $('.tab-headers a:not(:hidden)').each(function () {
        if ($(this).is('.active') == 0) {
            $('.btnNext').show();
            $('.btnSubmit').hide();

            if ($($('.tab-headers a:not(:hidden)')[0]).is('.active')) {
                $('.btnPrev').hide();
            } else {
                $('.btnPrev').show();
            }
        }
        else if ($(this).is('.active') == 1) {
            $('.btnSubmit').show();
            $('.btnPrev').show();
            $('.btnNext').hide();
        }
    });
}

function changeTab(element, action) {
    var selectTab = $('.tab-headers .active');
    var activeTab = selectTab[0].innerText;
    $(".tab-headers > a").removeClass("active");
    var activeIndex = "";
    $('.tab-headers a').each(function () {
        if ($(this)[0].innerText == activeTab) {
            activeIndex = $(this).index();
            if (action == 'next') {
                activeIndex = activeIndex + 1;
            }
            if (action == 'prev') {
                activeIndex = activeIndex - 1;
            }
            var temp = $('.tab-headers a')[activeIndex];
            var tab = $('.tab-container .subTabs')[activeIndex];
            $(".tab-container > div").hide();
            $(".tab-container #" + $(tab)[0].id).show();
            $(temp).addClass('active');
        }
    });
    tabNextPrev();
}

function removeSection(section) {
    $(section).remove();
}
function hideLoader() {
    $(".loader").hide();
    $(window).on('beforeunload', function () {
        $(".loader").hide();
    });
}

// Invoice thumbnail preview script
this.thumbnailPreview = function () {
  var  xOffset = 400;
   var  yOffset = 30;
    $("a.previewthumbnail").hover(function (e) {
        this.t = this.title;
        this.title = "";
        var c = (this.t != "") ? "<br/>" + this.t : "";
        $("body").append("<p id='previewthumbnail'><img src='" + this.rel + "' alt='url preview' style='height:450px' />" + c + "</p>");
        $("#previewthumbnail")
            .css("top", (e.pageY - xOffset) + "px")
            .css("left", (e.pageX + yOffset) + "px")
            .fadeIn("fast");
    },
        function () {
            this.title = this.t;
            $("#previewthumbnail").remove();
        });
    $("a.previewthumbnail").mousemove(function (e) {
        $("#previewthumbnail")
            .css("top", (e.pageY - xOffset) + "px")
            .css("left", (e.pageX + yOffset) + "px");
    });
};