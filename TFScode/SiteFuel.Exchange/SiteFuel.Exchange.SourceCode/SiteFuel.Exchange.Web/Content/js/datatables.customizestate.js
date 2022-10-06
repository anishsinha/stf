$(document).ready(function () {
    $(document).on('stateLoadParams.dt', function (e, settings, data) {
        var searchKeyName = e.target.id + '-maintainSearch';
        var urlKeyName = e.target.id + '-maintainSearchUrl';
        var maintainSearch = GetLocalStorage(searchKeyName);
        var maintainSearchUrl = GetLocalStorage(urlKeyName);
        if (maintainSearch == 'yes' && maintainSearchUrl == window.location.href) {
            data.search.search = data.search.search;
        }
        else {
            data.search.search = "";
        }
    });

    $(document).on('mouseup', '.dataTable,.dataTable a', function (e) {
        if ($(this).is('.dataTable a')) {
            var tableId = $(this).parents('.dataTable').attr('id');
            if (tableId != null && tableId != undefined && tableId != false) {
                SetLocalStorage(tableId + '-maintainSearch', 'yes');
                SetLocalStorage(tableId + '-maintainSearchUrl', window.location.href);
            }
        }
        e.stopPropagation();
    });

    $(document).on('mouseup', '*:not(.dataTable a)', function () {
        var allStorage = GetLocalStorageItems();
        var allDatatables = $('.dataTable');
        for (var i = 0; i < allDatatables.length; i++) {
            var keySearch = allDatatables[i].id + '-maintainSearch';
            var valSearch = allStorage[keySearch];
            var keyUrl = allDatatables[i].id + '-maintainSearchUrl';
            var valUrl = allStorage[keyUrl];
            if (valSearch == 'yes' && valUrl == window.location.href) {
                RemoveLocalStorage(keySearch);
                RemoveLocalStorage(keyUrl);
            }
        }
    });
});