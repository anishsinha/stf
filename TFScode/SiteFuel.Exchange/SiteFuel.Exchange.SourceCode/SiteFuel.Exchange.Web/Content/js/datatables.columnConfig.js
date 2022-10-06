$(document).on('preInit.dt', function (e, settings) {
    var $this = $(e.target).dataTable();
    if ($this.attr('data-gridname')) {
        SetGridColumns(e, settings);
    }
});

function SetGridColumns(e, settings) {
    var $this = $(e.target).dataTable();
    var gridSetting = [];
    var colLen = settings.aoColumns.length;
    var url = "/Home/GetUserGridConfigurationAsync";

    var gridName = $this.attr('data-gridname');
    if (gridName != null && gridName != undefined && gridName.trim() != '') {
        $.ajax({
            type: "get",
            url: url,
            async: false,
            dataType: "json",
            data: { 'gridId': gridName },
            success: function (data) {
                gridSetting = data;
                for (let i = 0; i < gridSetting.length; i++) {
                    var item = gridSetting[i].Key;

                    for (let j = 0; j < colLen; j++) {
                        if (settings.aoColumns[j].key == item && gridSetting[i].Value == 'false') {
                            //$this.fnSetColumnVis(j, false);
                            $this.api(!0).column(j).visible(false);
                            break;
                        }
                    }
                }
                $(document).off('column-visibility.dt');
                $(document).on('column-visibility.dt', saveUserGridConfig);
            }
        });
    }
}

var saveUserGridConfig = function (e, settings, column, state) {
    var $this = $(e.target).dataTable();
    var colLen = settings.aoColumns.length;
    var columnNames = [];
    for (let i = 0; i < colLen; i++) {
        //if (settings.aoColumns[i].bVisible == false) {
        columnNames.push({ Key: settings.aoColumns[i].key, Value: settings.aoColumns[i].bVisible });
        //}
    }

    var viewModel = {
        Setting: columnNames,
        GridId: $this.attr('data-gridname')
    };

    var url = "/Home/SaveUserGridConfiguration";
    $.post(url, viewModel).done(function (response) {});
}