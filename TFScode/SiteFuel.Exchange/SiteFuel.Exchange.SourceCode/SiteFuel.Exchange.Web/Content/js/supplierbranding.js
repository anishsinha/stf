function getSupplierPreferenceSetting() {
        $.get('/Settings/Profile/GetPreferencesSettingForBranding', function (response) {
            if (response && response.StatusCode == 0 && response.IsBrandMyWebsite) {

                ////SHOW HIDE LINK
                //if (response && response.IsThirdPartyInvitationEnabled) {
                //    $(".thirdPartyNetworkLink").removeClass('hide-element-important');
                //} else {
                //    $(".thirdPartyNetworkLink").addClass('hide-element-important');
                //}

                // Background color
                if (response.BackgroundColor) {
                    $('.page-bg').css("background", response.BackgroundColor);
                    $('#layoutFooter').css("background", response.BackgroundColor);
                    $("<style type='text/css'> .sticky-header-loc {background: " + response.BackgroundColor + "!important;} </style>").appendTo("head");
                    $("<style type='text/css'> .sticky-header-dash {background: " + response.BackgroundColor + "!important;} </style>").appendTo("head");
                    $("<style type='text/css'> .sticky-header-wmd {background: " + response.BackgroundColor + "!important;} </style>").appendTo("head");
                    //$('.sticky-header-loc').css("background", response.BackgroundColor + " !important");
                    //$('.sticky-header-dash').css("background", response.BackgroundColor + " !important");
                    //$('.sticky-header-wmd').css("background", response.BackgroundColor + " !important");
                   // $('.header_panel').css("background", response.BackgroundColor);
                    $('#sliderpanel').css("background", response.BackgroundColor);
                }

                // Header color
                if (response.HeaderColor) {
                    $('#layoutHeader').css("background", response.HeaderColor);
                }

                // Foreground color
                if (response.ForegroundColor) {
                    $("<style type='text/css'> .well {background: " + response.ForegroundColor + ";} </style>").appendTo("head");
                    $("<style type='text/css'> .table-responsive {background: " + response.ForegroundColor + ";} </style>").appendTo("head");
                    $('.tanks-wrapper').css("background", response.ForegroundColor);
                    $('.equal-container').css("background", response.ForegroundColor);
                }

                 // Icon color
                if (response.IconColor) {
                    $('#layoutHeader .btn-primary').css("background-color", response.IconColor + " !important");
                    $("<style type='text/css'> #layoutHeader .nav-icons {border: 1px solid " + response.IconColor + " !important} </style>").appendTo("head");
                    $("<style type='text/css'> #layoutHeader #mobile-menu-toggle span {background: " + response.IconColor + " !important} </style>").appendTo("head");
                    $('#layoutHeader .nav-icons i, #layoutHeader .color-lightgrey').css("color", response.IconColor + " !important");
                }

                // Button color
                if (response.ButtonColor) {
                    $("<style type='text/css'> .btn1-primary, .btn1-primary:hover, .btn1-primary:focus, .btn1-primary:active, .btn1-primary:visited { background-color: " +
                        response.ButtonColor + "!important;border-color: #FFFFFF!important;color: #FFFFFF;}  </style>").appendTo("head");

                    $(".btn-primary").each(function (entry, ele, array) {
                        $(ele).removeClass("btn-primary");
                        $(ele).addClass("btn1-primary");
                    });
                  
                    $("<style type='text/css'> .btn-toggle label.active {background-color: " + response.ButtonColor + "!important;} </style>").appendTo("head");

                    $("<style type='text/css'> .btn1-default, .btn1-default:hover, .btn1-default:active, .btn1-default:visited, .btn1-default:focus { background-color: transparent !important;color: " + response.ButtonColor + "!important;border-color: " +
                        response.ButtonColor + "!important;}  </style>").appendTo("head");

                    $(".btn-default").each(function (entry, ele, array) {
                        $(ele).removeClass("btn-default");
                        $(ele).addClass("btn1-default");
                    });

                    $("<style type='text/css'>.buyerdashboard-home-container input[type=radio]:checked+label {background-image: linear-gradient(96deg, " + response.ButtonColor + ", " + response.ButtonColor + ")!important;color: #FFFFFF;} </style>").appendTo("head");
                    // header button color
                    $("<style type='text/css'> header .btn1-primary, header .btn1-primary:hover, header .btn1-primary:focus, header .btn1-primary:active, header .btn1-primary:visited {border-radius: 50px;background-color: " +
                        response.ButtonColor + "!important;border-color:#FFFFFF!important;color: #FFFFFF;}  </style>").appendTo("head");

                    $("<style type='text/css'> header .btn1-default {border-radius: 50px;background-color: transparent !important;color: " + response.ButtonColor + "!important;border-color: " +
                        response.ButtonColor + "!important;}  </style>").appendTo("head");

                    $("<style type='text/css'> header .user-profile1 {border-radius: 50px;width : 30px;height: 30px;background-color: " +
                        response.ButtonColor + "!important;color: #FFFFFF;}  </style>").appendTo("head");

                    $("<style type='text/css'> header .user-profile1 {border-radius: 50px;width : 30px;height: 30px;background-color: " +
                        response.ButtonColor + "!important;color: #FFFFFF;}  </style>").appendTo("head");
                    $("#layoutHeader .btn-primary").each(function (entry, ele, array) {
                        $(ele).removeClass("btn-primary");
                        $(ele).addClass("btn1-primary");
                    });
                    $("#layoutHeader .btn-default").each(function (entry, ele, array) {
                        $(ele).removeClass("btn-default");
                        $(ele).addClass("btn1-default");
                    });
                    $("#layoutHeader .user-profile").each(function (entry, ele, array) {
                        $(ele).removeClass("user-profile");
                        $(ele).addClass("user-profile1");
                    });
                }
            }
        });
}