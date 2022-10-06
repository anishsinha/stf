var ServingStateModule;
(function (ServingStateModule) {
    var StateOption = /** @class */ (function () {
        function StateOption(data, selected) {
            this.Id = data.StateId;
            this.Name = data.StateName;
            this.CountryId = data.CountryId;
            this.Selected = selected;
        }
        StateOption.prototype.GetOption = function () {
            return '<option ' + (this.Selected ? 'selected="selected" ' : '') + 'value="' + this.Id + '">' + this.Name + '</option>';
        };
        return StateOption;
    }());
    ServingStateModule.StateOption = StateOption;
    var StateDropdown = /** @class */ (function () {
        function StateDropdown(states, servingStates) {
            this.StateOptions = new Array();
            for (var idx = 0; idx < states.length; idx++) {
                var selected = $.grep(servingStates, function (num) { return num == states[idx].StateId; }).length > 0;
                this.StateOptions.push(new StateOption(states[idx], selected));
            }
        }
        StateDropdown.prototype.FillStates = function (countryId, stateDdl) {
            if (this.StateOptions === undefined || this.StateOptions === null || this.StateOptions.length == 0) {
                console.error('States list is empty for country id:' + countryId);
                return;
            }
            var states = $.grep(this.StateOptions, function (state) { return state.CountryId == countryId; });
            stateDdl.empty();
            for (var idx = 0; idx < states.length; idx++) {
                stateDdl.append(states[idx].GetOption());
            }
        };
        return StateDropdown;
    }());
    ServingStateModule.StateDropdown = StateDropdown;
    $(document).ready(function () {
        var stateDropdown = new ServingStateModule.StateDropdown(allStates, servingStates);
        $('.companyaddress .country').change(function () {
            var state = $(this).closest('.address-container').find('.state').text();
            SetServingStates(this, state);
        });
        $(document).on('updated', '.companyaddress .country', function (event, state, country) {
            SetServingStates(event.target, state);
        });
        function SetServingStates(target, state) {
            var countryId = $(target).find('option:selected').val();
            var servingStateDdl = $(target).closest('.companyprofile').find('.multi-select.states');
            stateDropdown.FillStates(countryId, servingStateDdl);
            $(target).closest('.companyprofile').find(".multi-select.states option")
                .filter(function (index) { return $(this).html() == state; })
                .prop("selected", true);
        }
        SetServingStates($('.country').first(), $('.state').first().text());
        $('.states').trigger('servingstatecheck');
    });
})(ServingStateModule || (ServingStateModule = {}));
//# sourceMappingURL=ServingStateAutofill.js.map