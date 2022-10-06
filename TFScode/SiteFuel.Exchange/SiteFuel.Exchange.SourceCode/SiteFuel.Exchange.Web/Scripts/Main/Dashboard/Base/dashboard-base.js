$(document).ready(function () {
    $(document).on('click', '.add-remove-tile', function () {
        var tileName = $(this).attr('tileName');
        var isClosed = !($(this).prop('checked'));

        $.each(tileDetails, function (index, tile) {
            if (tile.TileName.toLowerCase() == tileName.toLowerCase()) {
                tile.IsClosed = isClosed;
            }
        });
    });
});

function updateTileSetting(tile, isClosed) {
    var tileId = $(tile).closest('.tile-head').attr('id');
    var isCollapsed = false;

    if ($("#" + tileId + " .toggle-tiledata i").hasClass("fa-chevron-circle-down")) {
        isCollapsed = true;
    }

    $.each(tileDetails, function (index, tile) {
        if (tile.TileName.toLowerCase() == tileId.toLowerCase()) {
            tile.IsClosed = isClosed;
            tile.IsCollapsed = isCollapsed;
        }
    });

    saveTileSetting(false);
}

function saveTileSetting(isMultipleTilesUpdated) {
    $.ajax({
        url: saveTileSettingUrl,
        type: "POST",
        data: {
            pageId: pageId,
            isMultipleTilesUpdated: isMultipleTilesUpdated,
            settingsModel: tileDetails
        },
        datatype: 'json',
        ContentType: 'application/json;utf-8',
        success: function (response) {   
            if (isMultipleTilesUpdated) {
                msgsuccess(response.StatusMessage);
                $("#supp-tile-preferences.in").modal("hide");
                window.location.reload(true);
            }
        }
    });
}

function showHideTiles() {
    saveTileSetting(true);
}

function toggleData(toggleLink, targetTile) {
    $("#" + targetTile + " .tile-content").not("div.group-element").slideToggle(500);
    $("#" + targetTile + " .toggle-tiledata i").toggleClass("fa-chevron-circle-up fa-chevron-circle-down");
}

function ToggleTileHeader(targetTile) {
    $("#" + targetTile + " .toggle-header").toggleClass("show-element").slideToggle(500);
}

function removeTile(removeLink, targetTile) {
    $("#" + targetTile).slideUp().hide();
    $('#chk' + targetTile).prop('checked', false);
}

function showHideTile(chkStatus, targetTile) {
    if (chkStatus) {
        $("#" + targetTile + ".tile-head").show();
        $("#" + targetTile + " .tile-body").removeClass('hide-element');
    }
    else {
        $("#" + targetTile + " .tile-body").addClass('hide-element');
    }
}