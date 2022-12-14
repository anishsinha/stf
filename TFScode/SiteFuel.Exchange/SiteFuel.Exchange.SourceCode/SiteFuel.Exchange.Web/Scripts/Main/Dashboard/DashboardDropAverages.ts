module DashboardDropAveragesModule {

    declare var dropAveragesUrl: string;
    declare var dashboardFilter: { countryId: number, currencyType: number };
    declare var dropAvgdivId: string;
    export class DashboardDropAveragesClass {      
        GetDashboardDropAverages(): void {
            var currentObject = this;
            $.get(dropAveragesUrl, { 'countryId': dashboardFilter.countryId, 'currency': dashboardFilter.currencyType }, function (response) {
                currentObject.SetDropAveragesValues(response);              
                $("#ddFuelType").selectedIndex = 0;
            });
        }

        SetDropAveragesValues(response) {
            $("#totalNumberOfOrders").text(new Number(response.TotalOrders).toLocaleString("en-US", { maximumFractionDigits: 0 }));
            $("#totalNumberOfDrops").text(new Number(response.TotalDrops).toLocaleString("en-US", { maximumFractionDigits: 0 }));
            $("#avgPPGPerDrop").text(new Number(response.AvgPPGPerDrop).toLocaleString("en-US", { maximumFractionDigits: 2 }));
            $("#avgGallonsPerDrop").text(new Number(response.AvgGallonPerDrop).toLocaleString("en-US", { maximumFractionDigits: 0 }));
            $("#totalNumberOfOrders").closest(".widget").find('.loading-wrapper').remove();
        }

        GetDropAveragesOnDropdownChange(selectedFuelType, selectedCustomerId) {
            var currentObject = this;
            $.get(dropAveragesUrl, { 'fuelTypeId': selectedFuelType, 'customerId': selectedCustomerId }, function (response) {
                currentObject.SetDropAveragesValues(response);
            });
        }
    }

    $(document).ready(function () {
        var dropAveragesObject = new DashboardDropAveragesModule.DashboardDropAveragesClass();

        if ($('#' + dropAvgdivId).length > 0) {
            dropAveragesObject.GetDashboardDropAverages();

            $("#ddFuelType").on('change', function () {
                var selectedFuelType = this.value;
                var selectedCustomerId = $("#ddCustomer").val();
                dropAveragesObject.GetDropAveragesOnDropdownChange(selectedFuelType, selectedCustomerId);
            });

            $("#ddCustomer").on('change', function () {
                var selectedCustomerId = this.value;
                var selectedFuelType = $("#ddFuelType").val();
                dropAveragesObject.GetDropAveragesOnDropdownChange(selectedFuelType, selectedCustomerId);
            });
        }

        $(".select2_demo_4").select2({
            allowClear: false
        });
    });

}
