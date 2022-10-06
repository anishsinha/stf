$.validator.addMethod("greaterthanzeroif", function (value, element, parameters) {
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
    var targetvalue = parameters['targetvalue'];

    var isDisbaledOrReadonly = $(element).is(':disabled') || $(element).attr("readonly");

    if ((targetvalue === actualvalue) && !isDisbaledOrReadonly) {
        if (value == 0) {
            return false;
        }
    }
    return true;
});

$.validator.unobtrusive.adapters.addSingleVal("greaterthanzeroif", "fieldvalue");
