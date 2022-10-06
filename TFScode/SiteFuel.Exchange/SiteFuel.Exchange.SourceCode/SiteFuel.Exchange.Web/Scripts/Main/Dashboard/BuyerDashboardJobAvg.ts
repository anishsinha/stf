module BuyerDashboardJobAvgModule {

    declare var jobAvergesUrl: string;
    declare var jobAverageOrderUrl: string;
    declare var jobAveragefuelTypeDropDown: HTMLSelectElement;
    declare var dashboardFilter: { countryId: number, currencyType: number};
    declare var groupIds: string;
    export class BuyerDashboardJobAvgClass {

        GetDashboardJobAvg(): void {
            var currentObject = this;
            var selectedFuelType = $("#FuelTypeId").val();
            $("#jobAvgOrderLink").attr('href', jobAverageOrderUrl);
            $.get(jobAvergesUrl, { 'fuelTypeId': selectedFuelType, 'countryId': dashboardFilter.countryId, 'currency': dashboardFilter.currencyType, 'groupIds': groupIds }, function (response) {
                currentObject.SetFuelTypeDropDown(response);
                currentObject.SetJobAvgValues(response);
            });
        }

        SetFuelTypeDropDown(response) {
            $(jobAveragefuelTypeDropDown).empty(), $.each(response.FuelTypes, function (i, element) {
                $(jobAveragefuelTypeDropDown).append($('<option></option>').val(element.Id).html(element.Name));
            });
            $(jobAveragefuelTypeDropDown).selectedIndex = 0;
        }

        SetJobAvgValues(response) {
            $("#jobAvgOrderCount").text(new Number(response.OrderCount).toLocaleString("en-US", { maximumFractionDigits: 0 }));
            $("#jobAvgtotalDrops").text(new Number(response.TotalDrops).toLocaleString("en-US", { maximumFractionDigits: 0 }));
            $("#jobAvgAveragePpgPerDrop").text(new Number(response.AveragePpgPerDrop).toLocaleString("en-US", { maximumFractionDigits: 4 }));
            $("#jobAvgAverageGallonsPerDrop").text(new Number(response.AverageGallonsPerDrop).toLocaleString("en-US", { maximumFractionDigits: 2 }));
            $("#jobAvgOrderCount").closest(".widget").find('.loading-wrapper').hide();
        }
    }

    $(document).ready(function () {
        $(document).trigger('LoadCurrency');
        var jobAveragesObject = new BuyerDashboardJobAvgModule.BuyerDashboardJobAvgClass();
        jobAveragesObject.GetDashboardJobAvg();

        $("#FuelTypeId").select2({
            placeholder: "All FuelTypes",
            allowClear: false
        });

        $(jobAveragefuelTypeDropDown).on('change', function (e) {
            var selectedFuelType = this.value;
            var selectedJob = $("#SelectedJobId").val();
            $("#jobAvgOrderLink").attr('href', jobAverageOrderUrl + '&fuelTypeId=' + selectedFuelType);
            $("#jobAvgOrderCount").closest(".widget").find('.loading-wrapper').show();
            $.get(jobAvergesUrl, { 'jobId': selectedJob, 'fuelTypeId': selectedFuelType }, function (response) {
                jobAveragesObject.SetJobAvgValues(response);
            }).always(function () {
                $("#jobAvgOrderCount").closest(".widget").find('.loading-wrapper').hide();
            });
        });
    });
}
