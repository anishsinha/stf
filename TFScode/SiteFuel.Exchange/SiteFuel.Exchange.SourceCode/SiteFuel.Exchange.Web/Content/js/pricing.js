var pricingCodesArr = [{
    "PricingTypes": {
        "Fixed": { 
                    "Id": 1,
                    "Code": "A-120000"
        },
        "FuelCost": {
                    "Id": 4,
                    "Code": "A-140000"
        }
    }
}];

function getPricingCode(pricingTypeId, pricingSourceId) {
    var pricingType = pricingTypeId == 2 ? 'Fixed' : pricingTypeId == 4 ? 'FuelCost' : '';

    if (pricingType == '')
        return null;

    var pricing = pricingCodesArr.map(function (prc) {
        return prc.PricingTypes[pricingType];
    });

    if (pricing.length > 0)
        pricing = pricing[0];
    else
        pricing = null;

    return pricing;
}
