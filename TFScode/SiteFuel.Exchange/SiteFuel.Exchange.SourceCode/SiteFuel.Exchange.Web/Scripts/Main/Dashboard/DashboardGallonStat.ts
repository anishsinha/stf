module DashboardGallonStatModule {

    declare var gallonStatUrl: string;
    declare var fuelTypeDropDown: HTMLSelectElement;
    declare var dashboardFilter: { countryId: number, currencyType: number };
    declare var Chart: any;
    declare var AcceptedFrUrl: string;
    declare var MissedFrUrl: string;
    declare var ExpiredFrUrl: string;
    declare var DeclinedFrUrl: string;
    declare var CounterFrUrl: string;
    declare var TotalQrUrl: string;
    declare var AcceptedQrUrl: string;
    declare var MissedQrUrl: string;
    declare var OpenQrUrl: string;
    declare var DeclinedQrUrl: string;

    export class DashboardGallonStatClass {

        GetDashboardGallonStat(): void {
            var currentObject = this;
            $.get(gallonStatUrl, { 'countryId': dashboardFilter.countryId, 'currency': dashboardFilter.currencyType }, function (response) {
                currentObject.SetFuelTypeDropDown(response);
                currentObject.SetGallonStatValues(response);
                currentObject.SetBusinessStatValues(response);
                currentObject.SetFuelRequestStatValues(response);
                currentObject.SetQuoteRequestStatValues(response);
            });
        }

        SetFuelTypeDropDown(response) {
            var ddl = $(fuelTypeDropDown);
            if (ddl.length > 0) {
                $(fuelTypeDropDown).empty(), $.each(response.FuelTypes, function (i, element) {
                    $(fuelTypeDropDown).append($('<option></option>').val(element.Id).html(element.Name));
                });
                $(fuelTypeDropDown).selectedIndex = 0;
            }
        }

        SetGallonStatValues(response) {
            $("#totalRequestedGallonsStat").text(new Number(response.TotalRequestedGallons).toLocaleString("en-US", { maximumFractionDigits: 0 }));
            $("#acceptedGallonsStat").text(new Number(response.AcceptedGallons).toLocaleString("en-US", { maximumFractionDigits: 0 }));
            $("#deliveredGallonsStat").text(new Number(response.DeliveredGallons).toLocaleString("en-US", { maximumFractionDigits: 0 }));
            $("#missedGallonsStat").text(new Number(response.MissedGallons).toLocaleString("en-US", { maximumFractionDigits: 0 }));
            $("#expiredGallonsStat").text(new Number(response.ExpiredGallons).toLocaleString("en-US", { maximumFractionDigits: 0 }));
            $("#totalRequestedGallonsStat").closest(".widget").find('.loading-wrapper').remove();
        }

        SetBusinessStatValues(response) {
            $("#totalBusinessYouWon").text(new Number(response.BusinessYouWon).toLocaleString("en-US", { maximumFractionDigits: 0 }));
            $("#totalBusinessYouMissed").text(new Number(response.BusinessYouMissed).toLocaleString("en-US", { maximumFractionDigits: 0 }));
            $("#totalBusinessInYourArea").text(new Number(response.BusinessInYourArea).toLocaleString("en-US", { maximumFractionDigits: 0 }));
        }

        SetFuelRequestStatValues(response) {
            var canvas = <HTMLCanvasElement>document.getElementById("FRBarChart");
            if (canvas == null || canvas == undefined)
                return;

            var ctx1 = canvas.getContext("2d");
            var frchart = new Chart(ctx1, {
                type: 'bar',
                data: {
                    labels: [],
                    datasets: [{
                        label: "Accepted: " + response.AcceptedFrCount,
                        data: [response.AcceptedFrCount],
                        backgroundColor: [
                            '#aadb8d',
                        ],
                        borderColor: [
                            '#aadb8d',
                        ],
                        borderWidth: 1,
                    },
                    {
                        label: "Missed: " + response.MissedFrCount,
                        data: [response.MissedFrCount],
                        backgroundColor: [
                            '#eb826a',
                        ],
                        borderColor: [
                            '#eb826a',
                        ],
                        borderWidth: 1,
                    },
                    {
                        label: "Expired: " + response.ExpiredFrCount,
                        data: [response.ExpiredFrCount],
                        backgroundColor: [
                            '#bb877b',
                        ],
                        borderColor: [
                            '#bb877b',
                        ],
                        borderWidth: 1,
                    },
                    {
                        label: "Declined: " + response.DeclinedFrCount,
                        data: [response.DeclinedFrCount],
                        backgroundColor: [
                            '#d9ca76',
                        ],
                        borderColor: [
                            '#d9ca76',
                        ],
                        borderWidth: 1,
                    },
                    {
                        label: "Counter Offer: " + response.CounterOfferCount,
                        data: [response.CounterOfferCount],
                        backgroundColor: [
                            '#91b6e8',
                        ],
                        borderColor: [
                            '#91b6e8',
                        ],
                        borderWidth: 1,
                    }
                        ]
                },
                options: {
                    responsive: true,
                    legend: {
                        position: 'right',
                        labels: {
                            boxWidth: 10,
                            boxHeight: 1,
                            fontSize: 11
                        }
                    },
                    labels: { fontColor: "grey", },
                    scales: {
                        xAxes: [{
                            barPercentage: 0.6,
                            barThickness: 2,
                            maxBarThickness: 4
                        }]
                    },
                    hover: {
                        mode: 'single',
                        onHover: function (e) {
                            $("#FRBarChart").css("cursor", e[0] ? "pointer" : "default");
                        }
                    },
                    tooltips: {
                        callbacks: {
                            label: function (tooltipItem, data) {
                                return data.datasets[tooltipItem['datasetIndex']].label;
                            }
                        }
                    }
                }
            });

            var frBarChart = document.getElementById("FRBarChart");
            if (frBarChart == null || frBarChart == undefined)
                return;

            frBarChart.onclick = function (evt) {
                var activePoint = frchart.getElementAtEvent(evt)[0];
                var label = frchart.data.datasets[activePoint._datasetIndex].label;

                var spaceChar = label.indexOf(" ");
                var labelStr = label.substring(0, spaceChar);

                switch (labelStr) {
                    case 'Accepted:':
                        window.open(AcceptedFrUrl, "_self");
                        break;
                    case 'Missed:':
                        window.open(MissedFrUrl, "_self");
                        break;
                    case 'Expired:':
                        window.open(ExpiredFrUrl, "_self");
                        break;
                    case 'Declined:':
                        window.open(DeclinedFrUrl, "_self");
                        break;
                    case 'Counter':
                        window.open(CounterFrUrl, "_self");
                        break;
                }
            };
        }

        SetQuoteRequestStatValues(response) {
            var canvas = <HTMLCanvasElement>document.getElementById("QuoteBarChart");
            if (canvas == null || canvas == undefined)
                return;

            var ctx2 = canvas.getContext("2d");
            var quotechart = new Chart(ctx2, {
                type: 'bar',
                data: {
                    labels: [],
                    datasets: [{
                        label: "Open: " + response.OpenQrCount,
                        data: [response.OpenQrCount],
                                backgroundColor: [
                                    '#91b6e8',
                                ],
                                borderColor: [
                                    '#91b6e8',
                                ],
                                borderWidth: 1,
                            },
                            {
                        label: "Accepted: " + response.AcceptedQrCount,
                        data: [response.AcceptedQrCount],
                                backgroundColor: [
                                    '#aadb8d',
                                ],
                                borderColor: [
                                    '#aadb8d',
                                ],
                                borderWidth: 1,
                            },
                            {
                        label: "Missed: " + response.MissedQrCount,
                        data: [response.MissedQrCount],
                                backgroundColor: [
                                    '#eb826a',
                                ],
                                borderColor: [
                                    '#eb826a',
                                ],
                                borderWidth: 1,
                            },
                            {
                        label: "Declined: " + response.DeclinedQrCount,
                        data: [response.DeclinedQrCount],
                                backgroundColor: [
                                    '#d9ca76',
                                ],
                                borderColor: [
                                    '#d9ca76',
                                ],
                                borderWidth: 1,
                            }
                    ]

                },
                options: {
                    responsive: true,
                    legend: {
                        position: 'right',
                        labels: {
                            boxWidth: 10,
                            boxHeight: 1,
                            fontSize: 11
                        }
                    },
                    labels: { fontColor: "grey", },
                    scales: {
                        xAxes: [{
                            barPercentage: 0.6,
                            barThickness: 2,
                            maxBarThickness: 4
                        }]
                    },
                    hover: {
                        mode: 'single',
                        onHover: function (e) {
                            $("#QuoteBarChart").css("cursor", e[0] ? "pointer" : "default");
                        }
                    },
                    tooltips: {
                        callbacks: {
                            label: function (tooltipItem, data) {
                                return data.datasets[tooltipItem['datasetIndex']].label;
                            }
                        }
                    }
                }
            });
            
            document.getElementById("QuoteBarChart").onclick = function (evt) {
                var activePoint = quotechart.getElementAtEvent(evt)[0];
                var label = quotechart.data.datasets[activePoint._datasetIndex].label;

                var spaceChar = label.indexOf(" ");
                var labelStr = label.substring(0, spaceChar);

                switch (labelStr) {
                    case 'Accepted:':
                        window.open(AcceptedQrUrl, "_self");
                        break;
                    case 'Missed:':
                        window.open(MissedQrUrl, "_self");
                        break;
                    case 'Open:':
                        window.open(OpenQrUrl, "_self");
                        break;
                    case 'Declined:':
                        window.open(DeclinedQrUrl, "_self");
                        break;
                }
            };
        }
    }

    $(document).ready(function () {
        var gallonStatObject = new DashboardGallonStatModule.DashboardGallonStatClass();
        $(document).trigger('LoadCurrency');
        gallonStatObject.GetDashboardGallonStat();

        if (fuelTypeDropDown != undefined && $(fuelTypeDropDown).length > 0) {
            $(fuelTypeDropDown).on('change', function (e) {
                var selectedFuelType = this.value;

                $("#totalRequestedGallonsStat").closest(".widget").find('.loading-wrapper').show();
                $.get(gallonStatUrl, { 'fuelTypeId': selectedFuelType }, function (response) {
                    gallonStatObject.SetGallonStatValues(response)
                })
            });
        }

        $(".select2_demo_3").select2({
            placeholder: "All FuelTypes",
            allowClear: false
        });
    });

}
