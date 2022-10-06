$('#demandCapture').on('hidden.bs.modal', function () {
    $('#demandCapture').html('');
});

function getDemandCaptureForm() {
    $.ajax({
        url: demandCaptureFormUri,
        type: 'GET'
    }).done(function (result) {
        $('#demandCapture').append(result);
        $('#demandCapture').modal('show');
    }).fail(function (a, b, c) {
        msgerror('Could not get demand capture form');
    });
}

function UploadFileToService(serviceparam) {
    var data = new FormData();
    var file = $('#csvapiFile')[0].files[0];
    data.append('file', file);
    $.ajax({
        url: serviceparam.BaseUrl + '/DemandCapture/CreateDemands',
        processData: false,
        contentType: false,
        data: data,
        type: 'POST',
        headers: { "token": serviceparam.Token },
        success: function (result) {
            updateSfxBulkUpload(file.name, result.Message);
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            msgerror('could not connect to Freight Service');
        }
    });
}


    function CallTargetFreightApiWithCallback(callback)  {
        $.ajax({
            url: getFreightServiceParameterUri,
            type: 'GET'
        }).done(function (result) {
            msginfo('Processing your upload file in our system. Visit Bulk Upload Status page for more details.');
            callback(result);
            $("#demandCapture").modal('hide');
        }).fail(function (a, b, c) {
            msgerror('Could not connect to Freight Service');
            $("#demandCapture").modal('hide');
        });
        
    }

    function updateSfxBulkUpload(filename, errors) {
        $.ajax({
            url: postFreightServiceSfxUri,
            data: { fileName: filename, errors: errors },
            type: 'POST'
        }).done(function (result) {
            msginfo(errors);
        }).fail(function (a, b, c) {
            msgerror(errors);
        });
    }