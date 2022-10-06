function checkDuplicateTankSequenceForTankDetails(targetUrl,tankId,tankSequence,jobId) {
    var data = {
        AssetId: tankId,
        TankSequence: tankSequence,
        JobId: jobId
    }
    if (tankSequence == "" || tankSequence == null || tankSequence == 0) {
        SubmitAsset();
    } else {
        $.post(targetUrl, data, function (response) {
            if (response == true) {
                $('#duplicateSequenceForTankDetails').modal('show');
            } else {
                SubmitAsset();
            }
        })
    }  
}
function SubmitAsset() {
    $("#duplicateSequenceForTankDetails").modal('hide');
    $('#createAssetForm').submit();
}
function ConfirmYesForTankDetails() {
    SubmitAsset();
}
function ConfirmNoForTankDetails() {
    $("#duplicateSequenceForTankDetails").modal('hide');
}