var DashboardGlobalFuelCostModule;
(function (DashboardGlobalFuelCostModule) {
    var servingStatesList = new Array();
    var DashboardGlobalFuelCostClass = /** @class */ (function () {
        function DashboardGlobalFuelCostClass() {
        }
        DashboardGlobalFuelCostClass.prototype.GetGlobalFuelCosts = function () {
            var currentObject = this;
            $.get(getAllGlobalFuelCostsUrl, { 'countryId': dashboardFilter.countryId, 'currency': dashboardFilter.currencyType }, function (data) {
                $.get(notDefinedGFCUrl, { countryId: dashboardFilter.countryId }, function (response) {
                    if (response.length > 0) {
                        var servingStatesList = new Array();
                        for (var i = 0; i < response.length; i++) {
                            servingStatesList.push({ Id: response[i].Id, Name: response[i].Name });
                        }
                    }
                }).always(function () {
                    if (orderfuelTypeId > 0) {
                        var filteredData = $.grep(data, function (el) {
                            return el.FuelTypeId == orderfuelTypeId;
                        });
                        currentObject.BindGlobalFuelCosts(filteredData);
                    }
                    else {
                        currentObject.BindGlobalFuelCosts(data);
                    }
                });
            });
        };
        DashboardGlobalFuelCostClass.prototype.BindGlobalFuelCosts = function (data) {
            var costCurrency = dashboardFilter.currencyType == 1 ? 'USD' : 'CAD';                  
            if (data.length > 0) {
                $("#table-fuelcost tbody").html("");
                var rows = "";
                $.each(data, function (i, CurrentCostGridViewModel) {   
                    var uom = "";
                    if (CurrentCostGridViewModel.UoM == 1) { uom = 'Gallons'; }
                    else if (CurrentCostGridViewModel.UoM == 2) { uom = 'Litres'; }
                    else if (CurrentCostGridViewModel.UoM == 3) { uom = 'Barrels'; }
                    else if (CurrentCostGridViewModel.UoM == 4) { uom = 'MetricTons'; }
                    rows += '<tr>'
                        + '<td valign="top" width="20%">' + CurrentCostGridViewModel.FuelTypeName + '</td>' //1st column 
                        + '<td  width="28%">'
                        + '<div class="input-group">'
                        + '<div class="input-group-addon">$</div>'
                        + '<input type="hidden" class="fuel-type-id" value=' + CurrentCostGridViewModel.FuelTypeId + '>'
                        + '<input type="hidden" class="current-cost-id" value=' + CurrentCostGridViewModel.Id + '>'
                        + '<input type="hidden" id="txtOriginalCost_' + CurrentCostGridViewModel.Id + '" class="original-cost" value=' + CurrentCostGridViewModel.CurrentCostOfFuel + '>'
                        + '<input type="text" id="txtCurrentCost_' + CurrentCostGridViewModel.Id + '" class="form-control current-cost" value=' + CurrentCostGridViewModel.CurrentCostOfFuel + ' disabled>'
                        + '<span class="input-group-addon fs11">' + costCurrency + '</span>'
                        + '</div></td>' // 2nd column
                        + '<td width="35%">'
                        + '<input type= "hidden" id="selectedUoM_'+CurrentCostGridViewModel.Id+'" class="selected-uom" value = '+ CurrentCostGridViewModel.UoM +'>'
                        + '<div class="selected-uom-text"> ' + uom + '</div>'
                        + '<div class="all-uom-list hide-element"> <select id="UoM_' + CurrentCostGridViewModel.Id + '" class="uom-list form-control">'
                        + '<option value="1">Gallons</option> <option value="2">Litres</option> <option value="3">Barrels</option> <option value="4">Metric Tons</option> </select>'
                        + '</div> '
                        + '</td>' //3rd column
                        + '<td valign="top" class="pl10 ' + ishidden + '"  width="21%">' 
                        + '<div class="serving-statecodes">' + CurrentCostGridViewModel.stateCodes + '</div>'
                        + '<div class="serving-statelist hide-element"><select id="FuelCostStates_' + CurrentCostGridViewModel.Id + '" class="serving-states" multiple="multiple" >';
                    for (var j = 0; j < servingStatesList.length; j++) {
                        var isSelected = "";
                        for (var k = 0; k < CurrentCostGridViewModel.stateId.length; k++) {
                            if (CurrentCostGridViewModel.stateId[k] == servingStatesList[j].Id) {
                                isSelected = "selected";
                                selectedStateIds.push({ CurrentCostId: CurrentCostGridViewModel.Id, StateId: servingStatesList[j].Id });
                            }
                        }
                        rows += '<option value="' + servingStatesList[j].Id + '" ' + isSelected + '>' + servingStatesList[j].Name + '</option>';
                    }
                    rows += '</select></div></td>'
                        + '<td valign="top" class="vmiddle pl10" width="15%">'
                        + '<span id="spinner_' + CurrentCostGridViewModel.Id + '" class="hide-element spinner-updategfc"><span class="spinner-xsmall"></span></span><input type="button" data-current-cost-id="' + CurrentCostGridViewModel.Id + '" data-fuel-type-id="' + CurrentCostGridViewModel.FuelTypeId + '" class="btn btn-primary float-left btn-sm ml0 mr10 edit-action save-cost hide-element" value="' + btnLabelSave + '">'
                        + '<input type="button" class="btn btn-sm edit-action cancel-edit hide-element btn btn-link" value="' + btnLabelCancel + '">'
                        + '<span id="responseMsg_' + CurrentCostGridViewModel.Id + '" class="responseMsg"></span>'
                        + '</td>'; //4th column 
                    if (iseditable) {
                        rows += '<td class="text-right vmiddle text-nowrap" width="12%">'
                            + '<i class="fa fa-edit fs15 edit-fuel-cost handpointer"></i>'
                            + '<i class="fa fa-trash-alt fs15 color-maroon ml10 handpointer delete-confirm" data-placement="bottom" data-popout="true" data-singleton="true" data-toggle="confirmation" data-current-cost-id="' + CurrentCostGridViewModel.Id + '"  data-fuel-type-id="' + CurrentCostGridViewModel.FuelTypeId + '"></i>'
                            + '</td>'
                            + '</tr>'; // 5th column
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
        };
        DashboardGlobalFuelCostClass.prototype.AddGlobalFuelCost = function (fuelTypeId, cost, stateIds,uom) {
            var states = JSON.stringify(stateIds);
            var currentObject = this;
            $.post(addGlobalFuelCostUrl, { fuelTypeId: fuelTypeId, cost: cost, isNewEntry: true, stateIds: states, countryId: dashboardFilter.countryId, currency: dashboardFilter.currencyType, uom: uom }, function (data) {
                currentObject.BindGlobalFuelCosts(data);
            }).always(function () {
                $(".gfc-wrapper").closest(".grid-loader").find('.loading-wrapper').hide();
            });
        };
        DashboardGlobalFuelCostClass.prototype.UpdateGlobalFuelCost = function (currentCostId, cost, stateIds,uom) {
            var currentObject = this;
            $.post(updateGlobalFuelCostUrl, { currentCostId: currentCostId, cost: cost, uom: uom ,stateIds: stateIds, countryId: dashboardFilter.countryId, currency: dashboardFilter.currencyType }, function (data) {
                if (data.StatusCode == 1) {
                    $("#responseMsg_" + currentCostId + ".responseMsg").removeClass("color-green").addClass("db color-red fs12").html(data.StatusMessage).fadeIn('fast');
                    $("#responseMsg_" + currentCostId + ".responseMsg").delay(3000).fadeOut('fast');
                    var originalCost = $("#txtOriginalCost_" + currentCostId).val();
                    $("#txtCurrentCost_" + currentCostId).val(originalCost);
                }
                else if (data.length > 0) {
                    $("#responseMsg_" + currentCostId + ".responseMsg").removeClass("color-red").addClass("color-green fs12").fadeIn('fast').html(msgSaved).delay(3000).fadeOut('fast');
                    if (orderfuelTypeId > 0) {
                        var filteredData = $.grep(data, function (el) {
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
        };
        DashboardGlobalFuelCostClass.prototype.DeleteGlobalFuelCost = function (fuelTypeId, currentCostId,uom) {
            var currentObject = this;
            $.post(deleteGlobalFuelCostUrl, { fuelTypeId: fuelTypeId, uom:uom ,currentCostId: currentCostId, countryId: dashboardFilter.countryId, currency: dashboardFilter.currencyType }, function (data) {
                if (data.StatusCode == 1) {
                    $("#responseMsg_" + currentCostId + ".responseMsg").removeClass("color-green").addClass("db color-red fs12").html(data.StatusMessage).fadeIn('fast');
                    $("#responseMsg_" + currentCostId + ".responseMsg").delay(3000).fadeOut('fast');
                }
                else if (data.length > 0) {
                    if (orderfuelTypeId > 0) {
                        var filteredData = $.grep(data, function (el) {
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
        };
        return DashboardGlobalFuelCostClass;
    }());
    DashboardGlobalFuelCostModule.DashboardGlobalFuelCostClass = DashboardGlobalFuelCostClass;
    $(document).ready(function () {
        var globalFuelCostObject = new DashboardGlobalFuelCostModule.DashboardGlobalFuelCostClass();
        $(document).trigger('LoadFuelCostCurrency');
        globalFuelCostObject.GetGlobalFuelCosts();
        $("#btnSubmitGlobalFuelCost").click(function () {
            var fuelTypeId = $("#GlobalCostFuelType").val();
            var currentCost = $("#txtCurrentCost").val();
            var stateIds = $("#FuelCostStates").val();
            var uom = $("#GlobalCostFuelUoM").val();
            if (currentCost != "" && fuelTypeId != "" && !isNaN(currentCost) && currentCost > 0 && (uom != "" && uom != undefined && uom != null)) {
                $(".gfc-wrapper").closest(".grid-loader").find('.loading-wrapper').show();
                $('#GlobalCostFuelType').prop('selectedIndex', 0);
                $('#GlobalCostFuelUoM').prop('selectedIndex', 0);
                $('#GlobalCostFuelType').trigger('change');
                $("#txtCurrentCost").val("");
                $(".fuelCostExistMsg").hide();
                $('#FuelCostStates').val(null).trigger('change');
                $("#btnSubmitGlobalFuelCost").attr("disabled", "disabled");
                globalFuelCostObject.AddGlobalFuelCost(fuelTypeId, currentCost, stateIds, uom);
            }
            else if (isNaN(currentCost) || currentCost <= 0) {
                $(".fuelCostGreaterThanZero").show();
            }
            else if (uom == "" || uom == undefined || uom == null) {
                $(".fuelCostUoMRequired").show();
            }
        });
        $("#txtCurrentCost").keyup(function () {
            var currentCost = $("#txtCurrentCost").val();
            if (!isNaN(currentCost) && currentCost > 0) {
                $(".fuelCostGreaterThanZero").hide();
            }
            else {
                $(".fuelCostGreaterThanZero").show();
            }
        });
        $("#GlobalCostFuelType").change(function () {
            $(".fuelCostGreaterThanZero").hide();
            var fuelType = $("#GlobalCostFuelType").val();
            var uom = $('#GlobalCostFuelUoM').val();
            if (fuelType != "" && uom != "") {
                $.get(checkExistingGlobalFuelCostUrl, { fuelTypeId: fuelType, uom:uom ,stateIds: JSON.stringify($("#FuelCostStates").val()), countryId: dashboardFilter.countryId, currency: dashboardFilter.currencyType }, function (data) {
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
            var uom = $('#GlobalCostFuelUoM').val();
            if (fuelType != "" && uom !="") {
                $.get(checkExistingGlobalFuelCostUrl, { fuelTypeId: fuelType, uom: uom, stateIds: JSON.stringify($("#FuelCostStates").val()), currency: dashboardFilter.currencyType, countryId: dashboardFilter.countryId }, function (data) {
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
        $("#GlobalCostFuelUoM").change(function () {
            var uom = $("#GlobalCostFuelUoM").val();
            if (uom != "" && uom != undefined && uom != null) {
                $(".fuelCostUoMRequired").hide();
            } else {
                $(".fuelCostUoMRequired").show();
            }
            var fuelType = $("#GlobalCostFuelType").val();
            if (fuelType != "" && uom != "") {
                $.get(checkExistingGlobalFuelCostUrl, { fuelTypeId: fuelType, uom: uom, stateIds: JSON.stringify($("#FuelCostStates").val()), countryId: dashboardFilter.countryId, currency: dashboardFilter.currencyType }, function (data) {
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
        $(document).on("click", '.edit-fuel-cost', function (event) {
            var $this = $(this).parent().parent().children();
            var fuelTypeId = $this.find(".fuel-type-id").val();
            var currentCostId = $this.find(".current-cost-id").val();
            var currentUom = $this.find(".selected-uom").val();
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
                        if (currentCostId == selectedStateIds[j].CurrentCostId && servingStatesList[i].Id == selectedStateIds[j].StateId) {
                            isSelected = "Selected";
                        }
                    }
                    rows += '<option value="' + servingStatesList[i].Id + '" ' + isSelected + ' >' + servingStatesList[i].Name + '</option>';
                }
                $this.find(".serving-states").html(rows);
                $this.find(".edit-action").show();
                $this.find(".current-cost").removeAttr("disabled").focus();
                $this.find(".responseMsg").html("");
                $this.find(".serving-statecodes").hide();
                $this.find(".serving-statelist").show();
                var countryId = dashboardFilter.countryId;
                if (countryId == 1) {
                    $("#UoM_" + currentCostId).children('option[value="1"]').show();;
                    $("#UoM_" + currentCostId).children('option[value="2"]').hide();
                } else if (countryId == 2) {
                    $("#UoM_" + currentCostId).children('option[value="1"]').hide();;
                    $("#UoM_" + currentCostId).children('option[value="2"]').show();
                }
                $("#UoM_" + currentCostId).val(currentUom).change();
                $this.find(".selected-uom-text").hide();
                $this.find(".all-uom-list").show();

            });
        });
        $(document).on("click", '.edit-action.save-cost', function (event) {
            var fuelTypeId = $(this).data("fuel-type-id");
            var currentCostId = $(this).data("current-cost-id");
            var servinStateIds = $("#FuelCostStates_" + currentCostId).val();
            var stateIds = JSON.stringify(servinStateIds);
            var uom = $("#UoM_" + currentCostId).val();
            $("#spinner_" + currentCostId + ".spinner-updategfc").show();
            var $this = $(this).parent().parent().children();
            var curCost = $this.find(".current-cost").val();
            if (curCost != "" && !isNaN(curCost) && fuelTypeId != "" && curCost > 0) {
                globalFuelCostObject.UpdateGlobalFuelCost(currentCostId, curCost, stateIds, uom);
                $this.find(".current-cost").attr("disabled", "disabled");
                $this.find(".edit-action").hide();
                $this.find(".serving-statelist").hide();
                $this.find(".serving-statecodes").show();
                $this.find(".selected-uom-text").show();
                $this.find(".all-uom-list").hide();
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
            $this.find(".selected-uom-text").show();
            $this.find(".all-uom-list").hide();
        });
        $(document).on("confirmed.bs.confirmation", '[data-current-cost-id]', function (event) {
            var fuelTypeId = $(this).data("fuel-type-id");
            var currentCostId = $(this).data("current-cost-id");
            var uom = $("#selectedUoM_"+ currentCostId).val();
            if (fuelTypeId != "") {
                globalFuelCostObject.DeleteGlobalFuelCost(fuelTypeId, currentCostId, uom);
            }
        });
    });
})(DashboardGlobalFuelCostModule || (DashboardGlobalFuelCostModule = {}));
//# sourceMappingURL=DashboardGlobalFuelCost.js.map