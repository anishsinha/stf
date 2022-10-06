$.validator.addMethod('requiredifmultiple',
    function (value, element, parameters) {
        var dependentElement = '#' + parameters['dependentproperty'];
        var controltype = $(dependentElement).attr("type");
        var actualvalue = $(dependentElement).val();

        if (controltype === "radio" || controltype === "checkbox") {
            actualvalue = $("input[name='" + $(dependentElement).attr("name") + "']:checked").val();
        } else {
            if (typeof actualvalue === "string") {
                actualvalue = actualvalue.replace(/\r/g, "");
            }
        }

        // get the target value (as a string, 
        // as that's what actual value will be)
        var targetvalue = parameters['targetvalue'];
        targetvalue = (targetvalue == null ? '' : targetvalue).toString();

        var targetvaluearray = targetvalue.split('|');

        for (var i = 0; i < targetvaluearray.length; i++) {

            // if the condition is true, reuse the existing 
            // required field validator functionality
            if (targetvaluearray[i] === actualvalue) {
                return $.validator.methods.required.call(this, actualvalue, element, targetvaluearray[i]);
            }
        }

        return true;
    }
);

$.validator.unobtrusive.adapters.add(
    'requiredifmultiple',
    ['dependentproperty', 'targetvalue'],
    function (options) {
        options.rules['requiredifmultiple'] = {
            dependentproperty: options.params['dependentproperty'],
            targetvalue: options.params['targetvalue']
        };
        options.messages['requiredifmultiple'] = options.message;
    }
);
