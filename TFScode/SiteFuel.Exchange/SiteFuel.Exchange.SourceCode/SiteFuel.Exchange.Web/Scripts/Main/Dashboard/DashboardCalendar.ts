module DashboardCalendaModule {

    declare var getCustomersUrl: string;
    declare var getOrdersForCustomer: string;
    declare var getCustomersForDriver: string;
    declare var driversDropDown: HTMLSelectElement;
    declare var customersDropDown: HTMLSelectElement;
    declare var ordersDropDown: HTMLSelectElement;
    declare function getSelectedCountryAndCurrency(): any;

    export class DashboardCalendarClass {
        constructor() {
            getSelectedCountryAndCurrency();
        } 

        MultiselectOrdersList(): void {
            $('#CustomerOrders').multiselect({
                maxHeight: 170,
                includeSelectAllOption: true,
                nonSelectedText: 'All Orders',
                closeOnSelect: false,
                buttonWidth: '120px',
                onDropdownHide: function (event) {
                    $("#calendar").closest(".grid-loader").find('.loading-wrapper').show();
                    refreshCalendar();
                    $("#calendar").closest(".grid-loader").find('.loading-wrapper').hide();
                }
            });
        }
    }

    function refreshCalendar() {
        $("#calendar").closest(".grid-loader").find('.loading-wrapper').show();
        $(".fc-day-top a").removeClass("completedSchedule missedSchedule pendingSchedule missedActionTakenSchedule orderStartDate");
        $('.fc-widget-header').removeClass('bg-red');
        $('.fc-widget-header span').css('color', 'black');
        $('#calendar').fullCalendar('removeEvents');
        $('#calendar').fullCalendar('refetchEvents');
    }

    $(document).ready(function () {

        var dashboardCalendar = new DashboardCalendaModule.DashboardCalendarClass();
        dashboardCalendar.MultiselectOrdersList();

        $(".select2_customers").select2({
            placeholder: "All Customers",
            allowClear: false
        });

        $(driversDropDown).on('change', function () {
            $("#calendar").closest(".grid-loader").find('.loading-wrapper').show();
            var selectedDriverId = this.value;
            $(customersDropDown).multiselect('destroy');
            $(customersDropDown).empty();
            var selectedCurrency = getSelectedCountryAndCurrency();

            $.get(getCustomersForDriver, { 'driverId': selectedDriverId, 'countryId': selectedCurrency.countryId, 'currency': selectedCurrency.currencyType }, function (response) {
                $.each(response, function (i, element) {
                    $(customersDropDown).append($('<option></option>').val(element.Id).html(element.Name));
                });
                $(customersDropDown).selectedIndex = 0;
                $("#calendar").closest(".grid-loader").find('.loading-wrapper').hide();
                $(customersDropDown).trigger('change');
            });
        });

        $(customersDropDown).on('change', function () {
            $("#calendar").closest(".grid-loader").find('.loading-wrapper').show();
            var selectedDriverId = $(driversDropDown).val();
            var selectedCustomerCompanyId = this.value;
            $("#CustomerOrders").multiselect('destroy');
            $("#CustomerOrders").empty();
            var selectedCurrency = getSelectedCountryAndCurrency();

            $.get(getOrdersForCustomer, { 'driverId': selectedDriverId, 'customerCompanyId': selectedCustomerCompanyId, 'countryId': selectedCurrency.countryId, 'currency': selectedCurrency.currencyType }, function (response) {
                $.each(response, function (i, element) {
                    $(ordersDropDown).append($('<option></option>').val(element.Id).html(element.Name));
                });
                if (response.length == 0) {
                    $('#CustomerOrders').multiselect({
                        nonSelectedText: 'No Orders Found',
                    });
                    $('#CustomerOrders').parent().addClass('subSectionOpacity pntr-none');
                }
                else {
                    $('#CustomerOrders').parent().removeClass('subSectionOpacity pntr-none');
                    dashboardCalendar.MultiselectOrdersList();
                }
                $(ordersDropDown).selectedIndex = 0;
                refreshCalendar();
                $("#calendar").closest(".grid-loader").find('.loading-wrapper').hide();
            });
        });
    });
}