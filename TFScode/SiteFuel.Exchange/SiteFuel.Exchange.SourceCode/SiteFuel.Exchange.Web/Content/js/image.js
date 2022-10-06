$(document).ready(function () {
    imageDocumentReady();
    bolImageDocumentReady();
    signImageDocumentReady();
    additionalImageDocumentReady();
    taxAffidavitImageDocumentReady();
    taxBDNImageDocumentReady();
    coastGuardImageDocumentReady();
    inspectionRequestImageDocumentReady();
});
function imageDocumentReady() {
    $("#choose-file").click(function () {
        $("#btn-logo").trigger("click");
    });

    $("#remove-file").click(function () {
        $('#image-ctrl').removeAttr('src');
        $('#image-ctrl').attr('src', getDefaultImage());
        $('.image-remove-status').val(true);
        $('#btn-logo').val('');
        $('.pdfname_image').html('');
        $("#remove-file").hide();
    });

    $('#btn-logo').change(function (e) {
        $(".company-logo-loading").show();
        $("#image-block").addClass("pntr-none subSectionOpacity");
        if (e.target.files && e.target.files[0]) {
            $('#img-message').text("");
            $("#remove-file").show();
            var goUpload = true;
            var uploadFile = e.target.files[0];
            if (!(/\.(jpg|jpeg|png|pdf)$/i).test(uploadFile.name)) {
                $('#img-message').text(getImageFileError());
                $(".company-logo-loading").hide();
                $('#image-ctrl').hide();
                e.target.value = '';
                goUpload = false; return false;
            }
            if (uploadFile.size > 1000000) { // 1mb
                $('#img-message').text(getImageFileWarning());
                $(".company-logo-loading").hide();
                $('#image-ctrl').hide();
                e.target.value = '';
                goUpload = false; return false;
            }
            if (goUpload) {
                var reader = new FileReader();
                reader.onload = function (event) {
                    $('#image-ctrl').attr('src', event.target.result);
                    $('.image-remove-status').val(false);
                    $(".company-logo-loading").hide();
                    $("#image-block").removeClass("pntr-none subSectionOpacity");
                    goUpload = true; return true;
                };
                reader.readAsDataURL(e.target.files[0]);
            }
        }
    });
}


function bolImageDocumentReady() {
    $("#bolchoose-file").click(function () {
        $("#bolbtn-logo").trigger("click");
    });

    $("#bolremove-file").click(function () {
        $('#bolimage-ctrl').removeAttr('src');
        $('#bolimage-ctrl').attr('src', getDefaultImage());
        $('.bolimage-remove-status').val(true);
        $('#bolbtn-logo').val('');
        $('.pdfname_bolimage').html('');
        $("#bolremove-file").hide();
    });

    $('#bolbtn-logo').change(function (e) {
        $(".company-logo-loading").show();
        $("#bolimage-block").addClass("pntr-none subSectionOpacity");
        if (e.target.files && e.target.files[0]) {
            $('#bolimg-message').text("");
            $("#bolremove-file").show();
            var fileCountMsg = e.target.files.length + ' files selected';
            var goUpload = true;
            if (!isUploadedFilesValid(e.target.files, true, false)) { 
                $('#bolimg-message').text(getImageFileError());
                $(".company-logo-loading").hide();
                $('#bolimage-ctrl').hide();
                e.target.value = '';
                goUpload = false; return false;
            }
            if (!isUploadedFilesValid(e.target.files, false, true)) { 
                $('#bolimg-message').text(getImageFileWarning());
                $(".company-logo-loading").hide();
                $('#bolimage-ctrl').hide();
                e.target.value = '';
                goUpload = false; return false;
            }
            if (goUpload) {
                var reader = new FileReader();
                reader.onload = function (event) {
                    if (e.target.files.length > 1) {
                        $('#bolimage-ctrl').addClass('hide-element');
                        $('.bol-pdf-support').closest('.image-section').find('.image-selection').hide();
                        $('.bol-pdf-support').closest('.image-section').find('.pdfname').text(fileCountMsg).show();
                    }
                    else {
                        $('#bolimage-ctrl').attr('src', event.target.result);
                    }
                    $('.bolimage-remove-status').val(false);
                    $(".company-logo-loading").hide();
                    $("#bolimage-block").removeClass("pntr-none subSectionOpacity");
                    goUpload = true; return true;
                };
                reader.readAsDataURL(e.target.files[0]);
            }
        }
    });
}

function signImageDocumentReady() {
    $("#signchoose-file").click(function () {
        $("#signbtn-logo").trigger("click");
    });

    $("#signremove-file").click(function () {
        $('#signimage-ctrl').removeAttr('src');
        $('#signimage-ctrl').attr('src', getDefaultImage());
        $('.signimage-remove-status').val(true);
        $('#signbtn-logo').val('');
        $('.pdfname_signimage').html('');
        $("#signremove-file").hide();
    });

    $('#signbtn-logo').change(function (e) {
        $(".company-logo-loading").show();
        $("#signimage-block").addClass("pntr-none subSectionOpacity");
        if (e.target.files && e.target.files[0]) {
            $('#signimg-message').text("");
            $("#signremove-file").show();
            var goUpload = true;
            var uploadFile = e.target.files[0];
            if (!(/\.(jpg|jpeg|png)$/i).test(uploadFile.name)) {
                $('#signimg-message').text(getSignImageFileError());
                $(".company-logo-loading").hide();
                $('#signimage-ctrl').hide();
                e.target.value = '';
                goUpload = false; return false;
            }
            if (uploadFile.size > 1000000) { // 1mb
                $('#signimg-message').text(getSignImageFileError());
                $(".company-logo-loading").hide();
                $('#signimage-ctrl').hide();
                e.target.value = '';
                goUpload = false; return false;
            }
            if (goUpload) {
                var reader = new FileReader();
                reader.onload = function (event) {
                    $('#signimage-ctrl').attr('src', event.target.result);
                    $('.signimage-remove-status').val(false);
                    $(".company-logo-loading").hide();
                    $("#signimage-block").removeClass("pntr-none subSectionOpacity");
                    goUpload = true; return true;
                };
                reader.readAsDataURL(e.target.files[0]);
            }
        }
    });
}

$('input.invoice-pdf-support').on('change', function () {
    Object.values(this.files).forEach(function (file) {
        if (file.type == 'application/pdf') {
            $('.invoice-pdf-support').closest('.image-section').find('.pdf-selection').show();
            $('.invoice-pdf-support').closest('.image-section').find('.image-selection').hide();
            $('.invoice-pdf-support').closest('.image-section').find('.no-image').hide();
            var selectedPdfName = file.name;
            $('.invoice-pdf-support').closest('.image-section').find('.pdfname').text(selectedPdfName);
        }
        else {
            $('.invoice-pdf-support').closest('.image-section').find('.pdf-selection').hide();
            $('.invoice-pdf-support').closest('.image-section').find('.image-selection').show();
            $('.invoice-pdf-support').closest('.image-section').find('.no-image').hide();
        }
    });
});

$('input.bol-pdf-support').on('change', function () {
    var fileCount = this.files.length;
    var selectedPdfName = '';
    Object.values(this.files).forEach(function (file) {
        if (file.type == 'application/pdf') {
            $('.bol-pdf-support').closest('.image-section').find('.pdf-selection').show();
            $('.bol-pdf-support').closest('.image-section').find('.image-selection').hide();
            $('.bol-pdf-support').closest('.image-section').find('.no-image').hide();
            if (fileCount == 1) {
                selectedPdfName = file.name;
            }
            else {
                selectedPdfName = fileCount + ' files selected';
                $('.bol-pdf-support').closest('.image-section').find('.pdf-selection').hide();
            }
            $('.bol-pdf-support').closest('.image-section').find('.pdfname').text(selectedPdfName).show();
        }
        else {
            $('.bol-pdf-support').closest('.image-section').find('.pdf-selection').hide();
            $('.bol-pdf-support').closest('.image-section').find('.image-selection').show();
            $('.bol-pdf-support').closest('.image-section').find('.no-image').hide();
            $('.bol-pdf-support').closest('.image-section').find('.pdfname').text('').hide();
        }
    });
});

$('input.additional-img-pdf-support').on('change', function () {
    var fileCount = this.files.length;
    var selectedPdfName = '';
    Object.values(this.files).forEach(function (file) {
        if (file.type == 'application/pdf') {
            $('.additional-img-pdf-support').closest('.image-section').find('.pdf-selection').show();
            $('.additional-img-pdf-support').closest('.image-section').find('.image-selection').hide();
            $('.additional-img-pdf-support').closest('.image-section').find('.no-image').hide();
            if (fileCount == 1) {
                selectedPdfName = file.name;
            }
            else {
                selectedPdfName = fileCount + ' files selected';
                $('.additional-img-pdf-support').closest('.image-section').find('.pdf-selection').hide();
            }
            $('.additional-img-pdf-support').closest('.image-section').find('.pdfname').text(selectedPdfName).show();
        }
        else {
            $('.additional-img-pdf-support').closest('.image-section').find('.pdf-selection').hide();
            $('.additional-img-pdf-support').closest('.image-section').find('.image-selection').show();
            $('.additional-img-pdf-support').closest('.image-section').find('.no-image').hide();
            $('.additional-img-pdf-support').closest('.image-section').find('.pdfname').text('').hide();
        }
    });
});

function additionalImageDocumentReady() {
    $("#additional-img-choose-file").click(function () {
        $("#additional-img-btn-logo").trigger("click");
    });

    $("#additional-remove-file").click(function () {
        $('#additional-img-ctrl').removeAttr('src');
        $('#additional-img-ctrl').attr('src', getDefaultImage());
        $('.additional-img-remove-status').val(true);
        $('#additional-img-btn-logo').val('');
        $('.pdfname_additional').html('');
        $("#additional-remove-file").hide();
    });
    $('#additional-img-btn-logo').change(function (e) {
        $(".company-logo-loading").show();
        $("#additional-img-block").addClass("pntr-none subSectionOpacity");
        if (e.target.files && e.target.files[0]) {
            $('#additional-img-message').text("");
            $("#additional-remove-file").show();
            var fileCountMsg = e.target.files.length + ' files selected';
            var goUpload = true;
            if (!isUploadedFilesValid(e.target.files, true, false)) { 
                $('#additional-img-message').text(getImageFileError());
                $(".company-logo-loading").hide();
                $('#additional-img-ctrl').hide();
                e.target.value = '';
                goUpload = false; return false;
            }
            if (!isUploadedFilesValid(e.target.files, false, true)) { 
                $('#additional-img-message').text(getImageFileWarning());
                $(".company-logo-loading").hide();
                $('#additional-img-ctrl').hide();
                e.target.value = '';
                goUpload = false; return false;
            }
            if (goUpload) {
                var reader = new FileReader();
                reader.onload = function (event) {
                    if (e.target.files.length > 1) {
                        $('#additional-img-ctrl').hide();
                        $('.additional-img-pdf-support').closest('.image-section').find('.image-selection').hide();
                        $('.additional-img-pdf-support').closest('.image-section').find('.pdfname').text(fileCountMsg).show();
                    }
                    else {
                        $('#additional-img-ctrl').attr('src', event.target.result);
                    }
                    $('.additional-img-remove-status').val(false);
                    $(".company-logo-loading").hide();
                    $("#additional-img-block").removeClass("pntr-none subSectionOpacity");
                    goUpload = true; return true;
                };
                reader.readAsDataURL(e.target.files[0]);
            }
        }
    });
}


// start taxAffidavit
$('input.taxAffidavit-img-pdf-support').on('change', function () {
    var fileCount = this.files.length;
    var selectedPdfName = '';
    Object.values(this.files).forEach(function (file) {
        if (file.type == 'application/pdf') {
            $('.taxAffidavit-img-pdf-support').closest('.image-section').find('.pdf-selection').show();
            $('.taxAffidavit-img-pdf-support').closest('.image-section').find('.image-selection').hide();
            $('.taxAffidavit-img-pdf-support').closest('.image-section').find('.no-image').hide();
            if (fileCount == 1) {
                selectedPdfName = file.name;
            }
            else {
                selectedPdfName = fileCount + ' files selected';
                $('.taxAffidavit-img-pdf-support').closest('.image-section').find('.pdf-selection').hide();
            }
            $('.taxAffidavit-img-pdf-support').closest('.image-section').find('.pdfname').text(selectedPdfName).show();
        }
        else {
            $('.taxAffidavit-img-pdf-support').closest('.image-section').find('.pdf-selection').hide();
            $('.taxAffidavit-img-pdf-support').closest('.image-section').find('.image-selection').show();
            $('.taxAffidavit-img-pdf-support').closest('.image-section').find('.no-image').hide();
            $('.taxAffidavit-img-pdf-support').closest('.image-section').find('.pdfname').text('').hide();
        }
    });
});
function taxAffidavitImageDocumentReady() {
    $("#taxAffidavit-img-choose-file").click(function () {
        $("#taxAffidavit-img-btn-logo").trigger("click");
    });
    $("#taxAffidavit-remove-file").click(function () {
        $('#taxAffidavit-img-ctrl').removeAttr('src');
        $('#taxAffidavit-img-ctrl').attr('src', getDefaultImage());
        $('.taxAffidavit-img-remove-status').val(true);
        $('#taxAffidavit-img-btn-logo').val('');
        $('.pdfname_taxAffidavit').html('');
        $("#taxAffidavit-remove-file").hide();
    });

    $('#taxAffidavit-img-btn-logo').change(function (e) {
        $(".company-logo-loading").show();
        $("#taxAffidavit-img-block").addClass("pntr-none subSectionOpacity");
        if (e.target.files && e.target.files[0]) {
            $("#taxAffidavit-remove-file").show();
            $('#taxAffidavit-img-message').text("");
            var fileCountMsg = e.target.files.length + ' files selected';
            if (e.target.files.length>1) {
                $('#taxAffidavit-img-message').text(fileCountMsg);
            }
            var goUpload = true;
            if (!isUploadedFilesValid(e.target.files, true, false)) {
                $('#taxAffidavit-img-message').text(getImageFileError());
                $(".company-logo-loading").hide();
                $('#taxAffidavit-img-ctrl').hide();
                e.target.value = '';
                goUpload = false; return false;
            }
            if (!isUploadedFilesValid(e.target.files, false, true)) {
                $('#taxAffidavit-img-message').text(getImageFileWarning());
                $(".company-logo-loading").hide();
                $('#taxAffidavit-img-ctrl').hide();
                e.target.value = '';
                goUpload = false; return false;
            }
            if (goUpload) {
                var reader = new FileReader();
                reader.onload = function (event) {
                    if (e.target.files.length > 1) {
                        $('#taxAffidavit-img-ctrl').hide();
                        $('.taxAffidavit-img-pdf-support').closest('.image-section').find('.image-selection').hide();
                        $('.taxAffidavit-img-pdf-support').closest('.image-section').find('.pdfname').text(fileCountMsg).show();
                    }
                    else {
                        $('#taxAffidavit-img-ctrl').attr('src', event.target.result);
                    }
                    $('.taxAffidavit-img-remove-status').val(false);
                    $(".company-logo-loading").hide();
                    $("#taxAffidavit-img-block").removeClass("pntr-none subSectionOpacity");
                    goUpload = true; return true;
                };
                reader.readAsDataURL(e.target.files[0]);
            }
        }
    });
}

//end taxAffidavit


// start taxBDN
$('input.taxBDN-img-pdf-support').on('change', function () {
    var fileCount = this.files.length;
    var selectedPdfName = '';
    Object.values(this.files).forEach(function (file) {
        if (file.type == 'application/pdf') {
            $('.taxBDN-img-pdf-support').closest('.image-section').find('.pdf-selection').show();
            $('.taxBDN-img-pdf-support').closest('.image-section').find('.image-selection').hide();
            $('.taxBDN-img-pdf-support').closest('.image-section').find('.no-image').hide();
            if (fileCount == 1) {
                selectedPdfName = file.name;
            }
            else {
                selectedPdfName = fileCount + ' files selected';
                $('.taxBDN-img-pdf-support').closest('.image-section').find('.pdf-selection').hide();
            }
            $('.taxBDN-img-pdf-support').closest('.image-section').find('.pdfname').text(selectedPdfName).show();
        }
        else {
            $('.taxBDN-img-pdf-support').closest('.image-section').find('.pdf-selection').hide();
            $('.taxBDN-img-pdf-support').closest('.image-section').find('.image-selection').show();
            $('.taxBDN-img-pdf-support').closest('.image-section').find('.no-image').hide();
            $('.taxBDN-img-pdf-support').closest('.image-section').find('.pdfname').text('').hide();
        }
    });
});
function taxBDNImageDocumentReady() {
    $("#taxBDN-img-choose-file").click(function () {
        $("#taxBDN-img-btn-logo").trigger("click");
    });
    $("#taxBDN-remove-file").click(function () {
        $('#taxBDN-img-ctrl').removeAttr('src');
        $('#taxBDN-img-ctrl').attr('src', getDefaultImage());
        $('.taxBDN-img-remove-status').val(true);
        $('#taxBDN-img-btn-logo').val('');
        $('.pdfname_taxBDN').html('');
        $("#taxBDN-remove-file").hide();
    });

    $('#taxBDN-img-btn-logo').change(function (e) {
        $(".company-logo-loading").show();
        $("#taxBDN-img-block").addClass("pntr-none subSectionOpacity");
        if (e.target.files && e.target.files[0]) {
            $("#taxBDN-remove-file").show();
            $('#taxBDN-img-message').text("");
            var fileCountMsg = e.target.files.length + ' files selected';
            if (e.target.files.length > 1) {
                $('#taxBDN-img-message').text(fileCountMsg);
            }
            var goUpload = true;
            if (!isUploadedFilesValid(e.target.files, true, false)) {
                $('#taxBDN-img-message').text(getImageFileError());
                $(".company-logo-loading").hide();
                $('#taxBDN-img-ctrl').hide();
                e.target.value = '';
                goUpload = false; return false;
            }
            if (!isUploadedFilesValid(e.target.files, false, true)) {
                $('#taxBDN-img-message').text(getImageFileWarning());
                $(".company-logo-loading").hide();
                $('#taxBDN-img-ctrl').hide();
                e.target.value = '';
                goUpload = false; return false;
            }
            if (goUpload) {
                var reader = new FileReader();
                reader.onload = function (event) {
                    if (e.target.files.length > 1) {
                        $('#taxBDN-img-ctrl').hide();
                        $('.taxBDN-img-pdf-support').closest('.image-section').find('.image-selection').hide();
                        $('.taxBDN-img-pdf-support').closest('.image-section').find('.pdfname').text(fileCountMsg).show();
                    }
                    else {
                        $('#taxBDN-img-ctrl').attr('src', event.target.result);
                    }
                    $('.taxBDN-img-remove-status').val(false);
                    $(".company-logo-loading").hide();
                    $("#taxBDN-img-block").removeClass("pntr-none subSectionOpacity");
                    goUpload = true; return true;
                };
                reader.readAsDataURL(e.target.files[0]);
            }
        }
    });
}

//end taxAffidavit


// start coastGuard
function coastGuardImageDocumentReady() {
    $("#coastGuard-img-choose-file").click(function () {
        $("#coastGuard-img-btn-logo").trigger("click");
    });
    $("#coastGuard-remove-file").click(function () {
        $('#coastGuard-img-ctrl').removeAttr('src');
        $('#coastGuard-img-ctrl').attr('src', getDefaultImage());
        $('.coastGuard-img-remove-status').val(true);
        $('#coastGuard-img-btn-logo').val('');
        $('.pdfname_coastGuard').html('');
        $("#coastGuard-remove-file").hide();
    });
    $('#coastGuard-img-btn-logo').change(function (e) {
        $(".company-logo-loading").show();
        $("#coastGuard-img-block").addClass("pntr-none subSectionOpacity");
        if (e.target.files && e.target.files[0]) {
            $('#coastGuard-img-message').text("");
            $("#coastGuard-remove-file").show();
            var fileCountMsg = e.target.files.length + ' files selected';
            if (e.target.files.length > 1) {
                $('#coastGuard-img-message').text(fileCountMsg);
            }
            var goUpload = true;
            if (!isUploadedFilesValid(e.target.files, true, false)) {
                $('#coastGuard-img-message').text(getImageFileError());
                $(".company-logo-loading").hide();
                $('#coastGuard-img-ctrl').hide();
                e.target.value = '';
                goUpload = false; return false;
            }
            if (!isUploadedFilesValid(e.target.files, false, true)) {
                $('#coastGuard-img-message').text(getImageFileWarning());
                $(".company-logo-loading").hide();
                $('#coastGuard-img-ctrl').hide();
                e.target.value = '';
                goUpload = false; return false;
            }
            if (goUpload) {
                var reader = new FileReader();
                reader.onload = function (event) {
                    if (e.target.files.length > 1) {
                        $('#coastGuard-img-ctrl').hide();
                        $('.coastGuard-img-pdf-support').closest('.image-section').find('.image-selection').hide();
                        $('.coastGuard-img-pdf-support').closest('.image-section').find('.pdfname').text(fileCountMsg).show();
                    }
                    else {
                        $('#coastGuard-img-ctrl').attr('src', event.target.result);
                    }
                    $('.coastGuard-img-remove-status').val(false);
                    $(".company-logo-loading").hide();
                    $("#coastGuard-img-block").removeClass("pntr-none subSectionOpacity");
                    goUpload = true; return true;
                };
                reader.readAsDataURL(e.target.files[0]);
            }
        }
    });
}
$('input.coastGuard-img-pdf-support').on('change', function () {
    var fileCount = this.files.length;
    var selectedPdfName = '';
    Object.values(this.files).forEach(function (file) {
        if (file.type == 'application/pdf') {
            $('.coastGuard-img-pdf-support').closest('.image-section').find('.pdf-selection').show();
            $('.coastGuard-img-pdf-support').closest('.image-section').find('.image-selection').hide();
            $('.coastGuard-img-pdf-support').closest('.image-section').find('.no-image').hide();
            if (fileCount == 1) {
                selectedPdfName = file.name;
            }
            else {
                selectedPdfName = fileCount + ' files selected';
                $('.coastGuard-img-pdf-support').closest('.image-section').find('.pdf-selection').hide();
            }
            $('.coastGuard-img-pdf-support').closest('.image-section').find('.pdfname').text(selectedPdfName).show();
        }
        else {
            $('.coastGuard-img-pdf-support').closest('.image-section').find('.pdf-selection').hide();
            $('.coastGuard-img-pdf-support').closest('.image-section').find('.image-selection').show();
            $('.coastGuard-img-pdf-support').closest('.image-section').find('.no-image').hide();
            $('.coastGuard-img-pdf-support').closest('.image-section').find('.pdfname').text('').hide();
        }
    });
});
//end coastGuard
// start inspectionRequestVoucher
function inspectionRequestImageDocumentReady() {
    $("#inspectionRequestVoucher-img-choose-file").click(function () {
        $("#inspectionRequestVoucher-img-btn-logo").trigger("click");
    });
    $("#inspectionRequestVoucher-remove-file").click(function () {
        $('#inspectionRequestVoucher-img-ctrl').removeAttr('src');
        $('#inspectionRequestVoucher-img-ctrl').attr('src', getDefaultImage());
        $('.inspectionRequestVoucher-img-remove-status').val(true);
        $('#inspectionRequestVoucher-img-btn-logo').val('');
        $('.pdfname_inspectionRequestVoucher').html('');
        $("#inspectionRequestVoucher-remove-file").hide();
    });

    $('#inspectionRequestVoucher-img-btn-logo').change(function (e) {
        $(".company-logo-loading").show();
        $("#inspectionRequestVoucher-img-block").addClass("pntr-none subSectionOpacity");
        if (e.target.files && e.target.files[0]) {
            $('#inspectionRequestVoucher-img-message').text("");
            $("#inspectionRequestVoucher-remove-file").show();
            var fileCountMsg = e.target.files.length + ' files selected';
            if (e.target.files.length > 1) {
                $('#inspectionRequestVoucher-img-message').text(fileCountMsg);
            }
            var goUpload = true;
            if (!isUploadedFilesValid(e.target.files, true, false)) {
                $('#inspectionRequestVoucher-img-message').text(getImageFileError());
                $(".company-logo-loading").hide();
                $('#inspectionRequestVoucher-img-ctrl').hide();
                e.target.value = '';
                goUpload = false; return false;
            }
            if (!isUploadedFilesValid(e.target.files, false, true)) {
                $('#inspectionRequestVoucher-img-message').text(getImageFileWarning());
                $(".company-logo-loading").hide();
                $('#inspectionRequestVoucher-img-ctrl').hide();
                e.target.value = '';
                goUpload = false; return false;
            }
            if (goUpload) {
                var reader = new FileReader();
                reader.onload = function (event) {
                    if (e.target.files.length > 1) {
                        $('#inspectionRequestVoucher-img-ctrl').hide();
                        $('.inspectionRequestVoucher-img-pdf-support').closest('.image-section').find('.image-selection').hide();
                        $('.inspectionRequestVoucher-img-pdf-support').closest('.image-section').find('.pdfname').text(fileCountMsg).show();
                    }
                    else {
                        $('#inspectionRequestVoucher-img-ctrl').attr('src', event.target.result);
                    }
                    $('.inspectionRequestVoucher-img-remove-status').val(false);
                    $(".company-logo-loading").hide();
                    $("#inspectionRequestVoucher-img-block").removeClass("pntr-none subSectionOpacity");
                    goUpload = true; return true;
                };
                reader.readAsDataURL(e.target.files[0]);
            }
        }
    });
}
$('input.inspectionRequestVoucher-img-pdf-support').on('change', function () {
    var fileCount = this.files.length;
    var selectedPdfName = '';
    Object.values(this.files).forEach(function (file) {
        if (file.type == 'application/pdf') {
            $('.inspectionRequestVoucher-img-pdf-support').closest('.image-section').find('.pdf-selection').show();
            $('.inspectionRequestVoucher-img-pdf-support').closest('.image-section').find('.image-selection').hide();
            $('.inspectionRequestVoucher-img-pdf-support').closest('.image-section').find('.no-image').hide();
            if (fileCount == 1) {
                selectedPdfName = file.name;
            }
            else {
                selectedPdfName = fileCount + ' files selected';
                $('.inspectionRequestVoucher-img-pdf-support').closest('.image-section').find('.pdf-selection').hide();
            }
            $('.inspectionRequestVoucher-img-pdf-support').closest('.image-section').find('.pdfname').text(selectedPdfName).show();
        }
        else {
            $('.inspectionRequestVoucher-img-pdf-support').closest('.image-section').find('.pdf-selection').hide();
            $('.inspectionRequestVoucher-img-pdf-support').closest('.image-section').find('.image-selection').show();
            $('.inspectionRequestVoucher-img-pdf-support').closest('.image-section').find('.no-image').hide();
            $('.inspectionRequestVoucher-img-pdf-support').closest('.image-section').find('.pdfname').text('').hide();
        }
    });
});
//end inspectionRequestVoucher

function isUploadedFilesValid(files, isFileTypeChk, isFileSizeCheck) {
    var isValid = true;
    if (isFileSizeCheck) {
        for (var i = 0; i < files.length; i++) {
            if (files[i].size > 1000000) { // 1mb
                isValid = false;
                break;
            }
        }
    }
    if (isFileTypeChk) {
        for (var j = 0; j < files.length; j++) {
            if (!(/\.(jpg|jpeg|png|pdf)$/i).test(files[j].name)) {
                isValid = false;
                break;
            }
        }
    }
    return isValid;
}