var OffersModule;
(function (OffersModule) {
    $(document).ready(function () {
        $("div").find(".disable-controls a").addClass("pntr-none");
        $('input[name="@Html.NameFor(m => m.FuelPricing.PricingTypeId)"]').click(function () {
            var selectedPricingType = parseInt($('input[name="@Html.NameFor(m => m.FuelPricing.PricingTypeId)"]:checked').val());
            if (selectedPricingType != suppliercostPricingType) {
                //validationMessageFor($("#@Html.IdFor(m=>m.FuelPricing.PricingTypeId)").attr('name'), '');
            }
        });
        $('.chk-cityrack').hide();
    });
    function GoToPreviousURL() {
        window.history.go(-1);
    }
})(OffersModule || (OffersModule = {}));
//# sourceMappingURL=Offers.js.map