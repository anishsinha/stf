var StateModule;
(function (StateModule) {
    var StateOption = /** @class */ (function () {
        function StateOption(data) {
            this.Id = data.StateId;
            this.Name = data.StateName;
            this.CountryId = data.CountryId;
            this.CountryGroupId = data.CountryGroupId;
        }
        StateOption.prototype.GetOption = function () {
            return '<option value="' + this.Id + '">' + this.Name + '</option>';
        };
        return StateOption;
    }());
    StateModule.StateOption = StateOption;
    var StateDropdown = /** @class */ (function () {
        function StateDropdown(states) {
            this.StateOptions = new Array();
            for (var idx = 0; idx < states.length; idx++) {
                this.StateOptions.push(new StateOption(states[idx]));
            }
        }
        StateDropdown.prototype.FillStates = function (countryId, stateDdl, isCountryGroup = false) {
            if (this.StateOptions === undefined || this.StateOptions === null || this.StateOptions.length == 0) {
                console.error('States list is empty for country id:' + countryId);
                return;
            }
            var states = [];
            if (isCountryGroup) {
                states = $.grep(this.StateOptions, function (state) { return state.CountryGroupId == countryId; });
            } else {
                states = $.grep(this.StateOptions, function (state) { return state.CountryId == countryId; });
            }
            
            var selectOption = stateDdl.find('option').first();
            stateDdl.empty().append(selectOption);
            for (var idx = 0; idx < states.length; idx++) {
                stateDdl.append(states[idx].GetOption());
            }
        };
        return StateDropdown;
    }());
    StateModule.StateDropdown = StateDropdown;
    $(function () {
        var stateDropdown = new StateModule.StateDropdown(allStates);
        var countries = $('.country');
        if (countries != undefined && countries.length > 0) {
            $.each(countries, function (i, country) {
                var closestStates = $(countries[i]).closest('.address-container').find('.state');
                var countryId = $(countries[i]).find('option:selected').val();
                stateDropdown.FillStates(countryId, closestStates);
                if (closestStates.hasClass('pickup')) {
                    closestStates.find('option[value="' + pickupStateId + '"]').prop("selected", true);
                }
                else if (closestStates.hasClass('billinfostate')) {
                    closestStates.find('option[value="' + billInfoStateId + '"]').prop("selected", true);
                }
                else {
                    closestStates.find('option[value="' + stateId + '"]').prop("selected", true);
                }
            });
        }
        //$('.country').change(function () {
        //changing to On event as previous event not working for dyanmaically loaded controls
        $(document).on('change', '.country', function (){
            var countryId = $(this).find('option:selected').val();
            var closestStates = $(this).closest('.address-container').find('.state');
            var closestcountrygroup = $(this).closest('.address-container').find('.countrygroup-div');
            let isCompanyGroup = false;
            if (closestcountrygroup && closestcountrygroup != undefined) {
                if (countryId == 4) {
                    var cgroupId = closestcountrygroup.find('.countrygroup').find('option:selected').val();
                    if (cgroupId && cgroupId > 0) {
                        countryId = cgroupId;
                        isCompanyGroup = true;
                    }
                    closestcountrygroup.find('.countrygroup').find("option:contains(Select Country group)").prop("selected", true);
                    closestcountrygroup.show();
                } else {
                    closestcountrygroup.hide();
                }
            }
            stateDropdown.FillStates(countryId, closestStates, isCompanyGroup);
        });
        //$('.countrygroup').change(function () {
        $(document).on('change', '.countrygroup', function () {
            var countrygroupId = $(this).find('option:selected').val();
            var closestStates = $(this).closest('.address-container').find('.state');
            if (countrygroupId == "") {
                countrygroupId = $('.country').find('option:selected').val();
                closestStates = $('.country').closest('.address-container').find('.state');
                stateDropdown.FillStates(countrygroupId, closestStates);
            } else {
                stateDropdown.FillStates(countrygroupId, closestStates, true);
            }
        });
        $(document).on('updated', '.country', function (event, state, country) {
            var countryId = $(event.target).find('option:selected').val();
            var closestStates = $(this).closest('.address-container').find('.state');
            var closestcountrygroup = $(this).closest('.address-container').find('.countrygroup-div');
            let isCompanyGroup = false;
            if (closestcountrygroup && closestcountrygroup != undefined) {
                if (countryId == 4) {
                    var cgroupId = closestcountrygroup.find('.countrygroup').find('option:selected').val();
                    if (cgroupId && cgroupId > 0) {
                        countryId = cgroupId;
                        isCompanyGroup = true;
                    }
                    closestcountrygroup.find('.countrygroup').find("option:contains(Select Country group)").prop("selected", true);
                    closestcountrygroup.show();
                } else {
                    closestcountrygroup.hide();
                }
            }
            stateDropdown.FillStates(countryId, closestStates, isCompanyGroup);
            $(this).closest('.address-container').find(".state option")
                .filter(function (index) { return $(this).html() == state; })
                .prop("selected", true);
        });
    });
})(StateModule || (StateModule = {}));
//# sourceMappingURL=StateAutofill.js.map