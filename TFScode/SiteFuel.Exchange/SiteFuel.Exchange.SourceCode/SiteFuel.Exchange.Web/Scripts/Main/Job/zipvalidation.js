$(document).ready(function () {
	var zipRegexList = [
		{ CountryId: 1, Regex: /^\d{5}(?:[-\s]\d{4})?$/ },
		{ CountryId: 2, Regex: /^[A-Za-z]\d[A-Za-z][ ]\d[A-Za-z]\d$/ }
	];

	$.validator.addMethod(
        'regex',
        function (value, element, regexp) {
        	var re = new RegExp(regexp);
        	return this.optional(element) || re.test(value);
        }
	);

	$('.country').change(function () {
        setZipcodeValidation(this);
	});

	$(document).on('updated', '.country', function (event, state, country) {
        setZipcodeValidation(event.target);
        var zipcode = $(event.target).closest('.address-container').find('.zipcode');
        zipcode.valid();
    });

    function setZipcodeValidation(target) {
        var countryId = $(target).find('option:selected').val();
        var zipcode = $(target).closest('.address-container').find('.zipcode');
        addZipCodeValidation(countryId, zipcode);
    }

    function addZipCodeValidation(countryId, zipcodeElem) {      
        //zipcodeElem.rules('remove', 'Regex');
        var zipRegex = $.grep(zipRegexList, function (zip) { return zip.CountryId == countryId; });
        if (zipRegex.length > 0) {
           //zipcodeElem.rules('add', { Regex: zipRegex[0].Regex, messages: { message: 'Zip is invalid' }});
            zipcodeElem.validate({
                rules: { Regex: zipRegex[0].Regex, messages: { message: 'Zip is invalid' } }
            });
        }
    }
    
    var countries = $('.country');
    if (countries != undefined && countries.length > 0) {
        $.each(countries, function (i, country) {
            var zipcode = $(countries[i]).closest('.address-container').find('.zipcode');
            var countryId = $(countries[i]).find('option:selected').val();
            addZipCodeValidation(countryId, zipcode);
        });
    }
});