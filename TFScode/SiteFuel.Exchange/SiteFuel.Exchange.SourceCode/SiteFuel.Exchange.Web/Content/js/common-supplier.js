function autoCompleteTextBox(element, url) {
    var target = $(element);
    target.focus(function () {
        $(this).autocomplete("search", $(this).val());
    });
    target.autocomplete({
        source: function (request, response) {
            $.ajax({
                url: url,
                type: "GET",
                dataType: "json",
                data: { Prefix: request.term },
                success: function (data) {
                    response($.map(data, function (item) {
                        return item.Name;
                    }))
                }
            })
        },
        messages: {
            noResults: '',
            results: function () { }
        },
        minLength: 0,
        maxShowItems: 5,
        scroll: true
    });
    $.ui.autocomplete.prototype._resizeMenu = function () {
        var ul = this.menu.element;
        ul.outerWidth(this.element.outerWidth());
    }
}