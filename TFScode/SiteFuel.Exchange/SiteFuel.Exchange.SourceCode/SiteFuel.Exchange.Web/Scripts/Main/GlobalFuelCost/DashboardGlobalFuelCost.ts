module DashboardGlobalFuelCostModule {

    declare var addGlobalFuelCostUrl: string;
    declare var getAllGlobalFuelCostsUrl: string;
    declare var deleteGlobalFuelCostUrl: string;
    declare var updateGlobalFuelCostUrl: string;
    declare var checkExistingGlobalFuelCostUrl: string;
    declare var notDefinedGFCUrl: string;
    declare var btnLabelSave: string;
    declare var btnLabelCancel: string;
    declare var msgSaved: string;
    declare var ishidden: string;
    declare var servingStatesList: { Id: number, Name: string }[];
    declare var selectedStateIds: { CurrentCostId: number, StateId: number }[];
    declare var iseditable: boolean;
    declare var orderfuelTypeId: number;
    declare var dashboardFilter: { countryId: number, currencyType: number };

    export class DashboardGlobalFuelCostClass {
		GetGlobalFuelCosts(): void {
            var currentObject = this;
			$.get(getAllGlobalFuelCostsUrl, { 'countryId': dashboardFilter.countryId, 'currency': dashboardFilter.currencyType }, function (data) {
				$.get(notDefinedGFCUrl, { countryId: dashboardFilter.countryId }, function (response) {
					if (response.length > 0) {
						servingStatesList = new Array();
						for (var i = 0; i < response.length; i++) {
							servingStatesList.push({ Id: response[i].Id, Name: response[i].Name });
						}
					}
				}).always(function () {
					if (orderfuelTypeId > 0) {
						var filteredData = $.grep(data, function (el: any) {
							return el.FuelTypeId == orderfuelTypeId;
						});
						currentObject.BindGlobalFuelCosts(filteredData);
					}
					else {
						currentObject.BindGlobalFuelCosts(data);
					}
				});
			});
		}

		BindGlobalFuelCosts(data) {

            var costCurrency = dashboardFilter.currencyType == 1 ? 'USD' : 'CAD';
            if (data.length > 0) {
                $("#table-fuelcost tbody").html("");
                var rows = "";
				$.each(data, function (i, CurrentCostGridViewModel) {
                    rows += '<tr>'
                        + '<td valign="top" width="30%">' + CurrentCostGridViewModel.FuelTypeName + '</td>'
                        + '<td  width="28%">'
                        + '<div class="input-group">'
                        + '<div class="input-group-addon">$</div>'
                        + '<input type="hidden" class="fuel-type-id" value=' + CurrentCostGridViewModel.FuelTypeId + '>'
                        + '<input type="hidden" class="current-cost-id" value=' + CurrentCostGridViewModel.Id + '>'
                        + '<input type="hidden" id="txtOriginalCost_' + CurrentCostGridViewModel.Id + '" class="original-cost" value=' + CurrentCostGridViewModel.CurrentCostOfFuel + '>'
                        + '<input type="text" id="txtCurrentCost_' + CurrentCostGridViewModel.Id + '" class="form-control current-cost" value=' + CurrentCostGridViewModel.CurrentCostOfFuel + ' disabled>'
                        + '<span class="input-group-addon">' + costCurrency + '</span>'
                        + '</div></td>'
                        + '<td valign="top" class="pl10 ' + ishidden + '"  width="21%">'
                        + '<div class="serving-statecodes">' + CurrentCostGridViewModel.stateCodes + '</div>'
                        + '<div class="serving-statelist hide-element"><select id="FuelCostStates_' + CurrentCostGridViewModel.Id + '" class="serving-states" multiple="multiple" >';
					for (var j = 0; j < servingStatesList.length; j++) {
						var isSelected = "";
                        for (var k = 0; k < CurrentCostGridViewModel.stateId.length; k++) {
                            if (CurrentCostGridViewModel.stateId[k] == servingStatesList[j].Id) { isSelected = "selected"; selectedStateIds.push({ CurrentCostId: CurrentCostGridViewModel.Id, StateId: servingStatesList[j].Id }); }
                        }
                        rows += '<option value="' + servingStatesList[j].Id + '" ' + isSelected + '>' + servingStatesList[j].Name + '</option>';
                    }
                    rows += '</select></div></td>'
                        + '<td valign="top" class="vmiddle pl10" width="21%">'
                        + '<span id="spinner_' + CurrentCostGridViewModel.Id + '" class="hide-element spinner-updategfc"><span class="spinner-xsmall"></span></span><input type="button" data-current-cost-id="' + CurrentCostGridViewModel.Id + '" data-fuel-type-id="' + CurrentCostGridViewModel.FuelTypeId + '" class="btn btn-primary btn-xs edit-action save-cost hide-element" value="' + btnLabelSave + '">'
                        + '<input type="button" class="btn btn-default btn-xs edit-action cancel-edit hide-element ml5" value="' + btnLabelCancel + '">'
                        + '<span id="responseMsg_' + CurrentCostGridViewModel.Id + '" class="responseMsg"></span>'
                        + '</td>'
                    if (iseditable) {
                        rows += '<td class="text-right vmiddle text-nowrap" width="12%">'
                            + '<i class="fa fa-edit fs15 edit-fuel-cost handpointer"></i>'
                            + '<i class="fa fa-trash-alt fs15 color-maroon ml10 handpointer delete-confirm" data-placement="bottom" data-popout="true" data-singleton="true" data-toggle="confirmation" data-current-cost-id="' + CurrentCostGridViewModel.Id + '"  data-fuel-type-id="' + CurrentCostGridViewModel.FuelTypeId + '"></i>'
                            + '</td>'
                            + '</tr>';
                    }
                    else {
                        rows += '</tr>';
                    }
                });
                $("#table-fuelcost tbody").append(rows);
                $("#widget-global-fuel-cost").show();
                $(".gfc-wrapper").closest(".grid-loader").find('.loading-wrapper').hide();
            }
            else {
                $(".gfc-wrapper").closest(".grid-loader").find('.loading-wrapper').hide();
            }
        }

        AddGlobalFuelCost(fuelTypeId, cost, stateIds) {
            var states = JSON.stringify(stateIds);
            var currentObject = this;
            $.post(addGlobalFuelCostUrl, { fuelTypeId: fuelTypeId, cost: cost, isNewEntry: true, stateIds: states, countryId: dashboardFilter.countryId, currency: dashboardFilter.currencyType }, function (data) {
                currentObject.BindGlobalFuelCosts(data);
            }).always(function () {
                $(".gfc-wrapper").closest(".grid-loader").find('.loading-wrapper').hide();
            });
        }

        UpdateGlobalFuelCost(currentCostId, cost, stateIds) {
            var currentObject = this;
            $.post(updateGlobalFuelCostUrl, { currentCostId: currentCostId, cost: cost, stateIds: stateIds, countryId: dashboardFilter.countryId, currency: dashboardFilter.currencyType }, function (data) {
                if (data.StatusCode == 1) {
                    $("#responseMsg_" + currentCostId + ".responseMsg").removeClass("color-green").addClass("db color-red fs12").html(data.StatusMessage).fadeIn('fast');
                    $("#responseMsg_" + currentCostId + ".responseMsg").delay(3000).fadeOut('fast');
                    var originalCost = $("#txtOriginalCost_" + currentCostId).val();
                    $("#txtCurrentCost_" + currentCostId).val(originalCost);
                }
                else if (data.length > 0) {
                    $("#responseMsg_" + currentCostId + ".responseMsg").removeClass("color-red").addClass("color-green fs12").fadeIn('fast').html(msgSaved).delay(3000).fadeOut('fast');
                    if (orderfuelTypeId > 0) {
                        var filteredData = $.grep(data, function (el: any) {
                            return el.FuelTypeId == orderfuelTypeId;
                        });
                        currentObject.BindGlobalFuelCosts(filteredData);
                    }
                    else {
                        currentObject.BindGlobalFuelCosts(data);
                    }
                }
            }).always(function () {
                $("#spinner_" + currentCostId + ".spinner-updategfc").hide();
            });
        }

        DeleteGlobalFuelCost(fuelTypeId, currentCostId) {
            var currentObject = this;
            $.post(deleteGlobalFuelCostUrl, { fuelTypeId: fuelTypeId, currentCostId: currentCostId, countryId: dashboardFilter.countryId, currency: dashboardFilter.currencyType }, function (data) {
                if (data.StatusCode == 1) {
                    $("#responseMsg_" + currentCostId + ".responseMsg").removeClass("color-green").addClass("db color-red fs12").html(data.StatusMessage).fadeIn('fast');
                    $("#responseMsg_" + currentCostId + ".responseMsg").delay(3000).fadeOut('fast');
                }
                else if (data.length > 0) {
                    if (orderfuelTypeId > 0) {
                        var filteredData = $.grep(data, function (el: any) {
                            return el.FuelTypeId == orderfuelTypeId;
                        });
                        currentObject.BindGlobalFuelCosts(filteredData);
                    }
                    else {
                        currentObject.BindGlobalFuelCosts(data);
                    }
                }
                else {
                    $("#widget-global-fuel-cost").hide();
                }
            });
        }
    }

    $(document).ready(function () {
        var globalFuelCostObject = new DashboardGlobalFuelCostModule.DashboardGlobalFuelCostClass();
        $(document).trigger('LoadFuelCostCurrency');
        globalFuelCostObject.GetGlobalFuelCosts();

        $("#btnSubmitGlobalFuelCost").click(function () {
            var fuelTypeId = $("#GlobalCostFuelType").val();
            var currentCost = $("#txtCurrentCost").val();
            var stateIds = $("#FuelCostStates").val();
            if (currentCost != "" && fuelTypeId != "" && !isNaN(currentCost) && currentCost > 0) {
                $(".gfc-wrapper").closest(".grid-loader").find('.loading-wrapper').show();
                $('#GlobalCostFuelType').prop('selectedIndex', 0);
                $('#GlobalCostFuelType').trigger('change');
                $("#txtCurrentCost").val("");
                $(".fuelCostExistMsg").hide();
                $('#FuelCostStates').val(null).trigger('change');
                $("#btnSubmitGlobalFuelCost").attr("disabled", "disabled");
                globalFuelCostObject.AddGlobalFuelCost(fuelTypeId, currentCost, stateIds);
            }
            else if (isNaN(currentCost) || currentCost <= 0) {
                $(".fuelCostGreaterThanZero").show();
            }
        });
        
        $("#txtCurrentCost").keyup(function () {
            var currentCost = $("#txtCurrentCost").val();
            if (!isNaN(currentCost) && currentCost > 0) { $(".fuelCostGreaterThanZero").hide(); }
            else { $(".fuelCostGreaterThanZero").show(); }
        });

        $("#GlobalCostFuelType").change(function () {
            $(".fuelCostGreaterThanZero").hide();
            var fuelType = $("#GlobalCostFuelType").val();
            if (fuelType != "") {
                $.get(checkExistingGlobalFuelCostUrl, { fuelTypeId: fuelType, stateIds: JSON.stringify($("#FuelCostStates").val()), countryId: dashboardFilter.countryId, currency: dashboardFilter.currencyType }, function (data) {
                    if (data) {
                        $(".fuelCostExistMsg").show();
                        $("#btnSubmitGlobalFuelCost").attr("disabled", "disabled");
                    }
                    else {
                        $(".fuelCostExistMsg").hide();
                        $("#btnSubmitGlobalFuelCost").removeAttr("disabled");
                    }
                });
                $.get(notDefinedGFCUrl, { fuelTypeId: fuelType, currentCostId: 0, countryId: dashboardFilter.countryId, currency: dashboardFilter.currencyType }, function (data) {
                    if (data.length > 0) {
                        servingStatesList = new Array();
                        for (var i = 0; i < data.length; i++) {
                            servingStatesList.push({ Id: data[i].Id, Name: data[i].Name });
                        }
                    }
                }).always(function () {
                    var rows = '';
                    for (var i = 0; i < servingStatesList.length; i++) {
                        rows += '<option value="' + servingStatesList[i].Id + '" >' + servingStatesList[i].Name + '</option>';
                    }
                    $("#FuelCostStates").html(rows);
                    $("#FuelCostStates").removeAttr("disabled");
                });
            }
            else {
                $("#FuelCostStates").attr("disabled", "disabled");
            }
        });

        $("#FuelCostStates").change(function () {
            $(".fuelCostGreaterThanZero").hide();
            var fuelType = $("#GlobalCostFuelType").val();
            if (fuelType != "") {
                $.get(checkExistingGlobalFuelCostUrl, { fuelTypeId: fuelType, stateIds: JSON.stringify($("#FuelCostStates").val()) }, function (data) {
                    if (data) {
                        $(".fuelCostExistMsg").show();
                        $("#btnSubmitGlobalFuelCost").attr("disabled", "disabled");
                    }
                    else {
                        $(".fuelCostExistMsg").hide();
                        $("#btnSubmitGlobalFuelCost").removeAttr("disabled");
                    }
                });
            }
            else {
                $("#FuelCostStates").attr("disabled", "disabled");
            }
        });

		$(document).on("click", '.edit-fuel-cost', function (event) {
            var $this = $(this).parent().parent().children();
            var fuelTypeId = $this.find(".fuel-type-id").val();
            var currentCostId = $this.find(".current-cost-id").val();
            $.get(notDefinedGFCUrl, { fuelTypeId: fuelTypeId, currentCostId: currentCostId, countryId: dashboardFilter.countryId, currency: dashboardFilter.currencyType }, function (data) {
                if (data.length > 0) {
                    servingStatesList = new Array();
                    for (var i = 0; i < data.length; i++) {
                        servingStatesList.push({ Id: data[i].Id, Name: data[i].Name });
                    }
                }
            }).always(function () {
				var rows = '';
                var selectedState = $this.find("#FuelCostStates_" + currentCostId).val();
                for (var i = 0; i < servingStatesList.length; i++) {
                    var isSelected = "";
                    for (var j = 0; j < selectedStateIds.length; j++) {
                        if (currentCostId == selectedStateIds[j].CurrentCostId && servingStatesList[i].Id == selectedStateIds[j].StateId) { isSelected = "Selected"; }
                    }
                    rows += '<option value="' + servingStatesList[i].Id + '" ' + isSelected + ' >' + servingStatesList[i].Name + '</option>';
                }
                $this.find(".serving-states").html(rows);
                $this.find(".edit-action").show();
                $this.find(".current-cost").removeAttr("disabled").focus();
                $this.find(".responseMsg").html("");
                $this.find(".serving-statecodes").hide();
                $this.find(".serving-statelist").show();
            });
        });
        
        $(document).on("click", '.edit-action.save-cost', function (event) {
            var fuelTypeId = $(this).data("fuel-type-id");
            var currentCostId = $(this).data("current-cost-id");
            var servinStateIds = $("#FuelCostStates_" + currentCostId).val();
            var stateIds = JSON.stringify(servinStateIds);
            $("#spinner_" + currentCostId + ".spinner-updategfc").show();
            var $this = $(this).parent().parent().children();
            var curCost = $this.find(".current-cost").val();
            if (curCost != "" && !isNaN(curCost) && fuelTypeId != "" && curCost > 0) {
                globalFuelCostObject.UpdateGlobalFuelCost(currentCostId, curCost, stateIds);
                $this.find(".current-cost").attr("disabled", "disabled");
                $this.find(".edit-action").hide();
                $this.find(".serving-statelist").hide();
                $this.find(".serving-statecodes").show();
            }
            else {
                $("#spinner_" + currentCostId + ".spinner-updategfc").hide();
               
            }
        });
        $(document).on("click", '.edit-action.cancel-edit', function (event) {
            var $this = $(this).parent().parent().children();
            var curCost = $this.find(".current-cost");
            curCost.val($this.find(".original-cost").val());
            $this.find(".current-cost").attr("disabled", "disabled");
            $this.find(".edit-action").hide();
            $this.find(".serving-statelist").hide();
            $this.find(".serving-statecodes").show();
        });

        $(document).on("confirmed.bs.confirmation", '[data-current-cost-id]', function (event) {
            var fuelTypeId = $(this).data("fuel-type-id");
            var currentCostId = $(this).data("current-cost-id");
            if (fuelTypeId != "") {
                globalFuelCostObject.DeleteGlobalFuelCost(fuelTypeId, currentCostId);
            }
        });
    });

}
