$.validator.addMethod("emailaddress", function (value, element, params) {
    if (!this.optional(element)) {
        if (value == "") {
            return true;
        }
        var emailRegExp = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
        var emailAddress = value;

        if (!emailRegExp.test(emailAddress.trim())) {
            return false;
        }
    }
    return true;
});

$.validator.unobtrusive.adapters.addSingleVal("emailaddress", "pattern");
