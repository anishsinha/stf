import * as $ from 'jquery';

module StateModule {
    declare var allStates;
    declare var stateId: number;
	declare var pickupStateId: number;
    declare var billInfoStateId: number;
    
    export class StateOption {
        Id: number;
        Name: string;
        CountryId: number;
        CountryGroupId: number;
        constructor(data) {
            this.Id = data.StateId;
            this.Name = data.StateName;
            this.CountryId = data.CountryId;
            this.CountryGroupId = data.CountryGroupId;
        }
        GetOption() {
            return '<option value="' + this.Id + '">' + this.Name + '</option>';
        }
    }

    export class StateDropdown {
        public StateOptions: StateOption[];

        constructor(states) {
            this.StateOptions = new Array();
            for (var idx = 0; idx < states.length; idx++) {
                this.StateOptions.push(new StateOption(states[idx]));
            }
        }

        FillStates(countryId: number, stateDdl: JQuery, isCountryGroup: boolean = false) {
            if (this.StateOptions === undefined || this.StateOptions === null || this.StateOptions.length == 0) {
                console.error('States list is empty for country id:' + countryId); return;
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
        }
    }

    $(() => {
        var stateDropdown = new StateModule.StateDropdown(allStates);
        var countries = $('.country');
        if (countries != undefined && countries.length > 0) {
            $.each(countries, function (i, country) {
                var closestStates = $(countries[i]).closest('.address-container').find('.state');
                var countryId = $(countries[i]).find('option:selected').val() as number;
				stateDropdown.FillStates(countryId, closestStates);
				if (closestStates.hasClass('pickup')) {
					closestStates.find('option[value="' + pickupStateId + '"]').prop("selected", true);
                }
                else if (closestStates.hasClass('billinfostate')) {
                    closestStates.find('option[value="' + billInfoStateId + '"]').prop("selected", true);
                }
				else
				{
					closestStates.find('option[value="' + stateId + '"]').prop("selected", true);
				}
            });
        }

        $('.country').change(function () {
            var countryId = $(this).find('option:selected').val() as number;
            var closestStates = $(this).closest('.address-container').find('.state');
            var closestcountrygroup = $(this).closest('.address-container').find('.countrygroup-div');
            let isCompanyGroup = false;
            if (closestcountrygroup && closestcountrygroup != undefined) {
                if (countryId == 4) {
                    var cgroupId = closestcountrygroup.find('.countrygroup').find('option:selected').val() as number;
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

        $('.countrygroup').change(function () {
            var countrygroupId = $(this).find('option:selected').val() as number;
            var closestStates = $(this).closest('.address-container').find('.state');
            stateDropdown.FillStates(countrygroupId, closestStates, true);
        });
        $(document).on('updated', '.country', function (event, state, country) {
            var countryId = $(event.target).find('option:selected').val() as number;
            var closestStates = $(this).closest('.address-container').find('.state');
            var closestcountrygroup = $(this).closest('.address-container').find('.countrygroup-div');
            let isCompanyGroup = false;
            if (closestcountrygroup && closestcountrygroup != undefined) {
                if (countryId == 4) {
                    var cgroupId = closestcountrygroup.find('.countrygroup').find('option:selected').val() as number;
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
}