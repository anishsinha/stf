$(document).ready(function () {
    newImageDocumentReady("#sitechoose-file", "#sitebtn-logo", "#siteremove-file", '#siteimage-ctrl','#sitenoimage-ctrl', '.siteimage-remove-status', "#siteimage-block", '#siteimg-message', '.site-pdf-support');
    newImageDocumentReady("#additionalchoose-file", "#additionalbtn-logo", "#additionalremove-file", '#additionalimage-ctrl', '#additionalnoimage-ctrl', '.additionalimage-remove-status', "#additionalimage-block", '#additionalimg-message', '.additional-pdf-support');
    $('input.site-pdf-support').on('change', function () {
        newImagepdfsupport('.site-pdf-support', this);
    });
    $('input.additional-pdf-support').on('change', function () {
        newImagepdfsupport('.additional-pdf-support', this);
    });
});

function newImagepdfsupport(pdfsupport, element) {
    var fileCount = element.files.length;
    var selectedPdfName = '';
    Object.values(element.files).forEach(function (file) {
        if (file.type == 'application/pdf') {
            $(pdfsupport).closest('.image-section').find('.pdf-selection').show();
            $(pdfsupport).closest('.image-section').find('.image-selection').hide();
            $(pdfsupport).closest('.image-section').find('.no-image').hide();
            $(pdfsupport).closest('.image-section').find('.image-download-btn').text('').hide();
            if (fileCount == 1) {
                selectedPdfName = file.name;
            }
            else {
                selectedPdfName = fileCount + ' files selected';
                $(pdfsupport).closest('.image-section').find('.pdf-selection').hide();
            }
            $(pdfsupport).closest('.image-section').find('.pdfname').text(selectedPdfName).show();
        }
        else {
            $(pdfsupport).closest('.image-section').find('.pdf-selection').hide();
            $(pdfsupport).closest('.image-section').find('.image-selection').show();
            $(pdfsupport).closest('.image-section').find('.no-image').hide();
            $(pdfsupport).closest('.image-section').find('.pdfname').text('').hide();
            $(pdfsupport).closest('.image-section').find('.image-download-btn').text('').hide();
            
        }
    });
}

function newImageDocumentReady(choosefile, btnlogo, removefile, imagectrl, noimagectrl, removestatus, imageblock, imgmessage, pdfsupport) {
    $(choosefile).click(function () {
        $(btnlogo).trigger("click");
    });

    $(removefile).click(function () {
        $(imagectrl).removeAttr('src');
        $(imagectrl).attr('src', getDefaultImage());
        $(removestatus).val(true);
        $(btnlogo).val('');
    });

    $(btnlogo).change(function (e) {
        $(".company-logo-loading").show();
        $(imageblock).addClass("pntr-none subSectionOpacity");
        if (e.target.files && e.target.files[0]) {
            $(imgmessage).text("");
            var fileCountMsg = e.target.files.length + ' files selected';
            var goUpload = true;
            if (!isUploadedFilesValid(e.target.files, true, false)) {
                $(imgmessage).text(getImageFileError());
                $(".company-logo-loading").hide();
                $(imagectrl).hide();
                e.target.value = '';
                goUpload = false; return false;
            }
            if (!isUploadedFilesValid(e.target.files, false, true)) {
                $(imgmessage).text(getImageFileWarning());
                $(".company-logo-loading").hide();
                $(imagectrl).hide();
                e.target.value = '';
                goUpload = false; return false;
            }
            if (goUpload) {
                var reader = new FileReader();
                reader.onload = function (event) {
                    if (e.target.files.length > 1) {
                        $(noimagectrl).addClass('hide-element');
                        $(pdfsupport).closest('.image-section').find('.image-selection').hide();
                        $(pdfsupport).closest('.image-section').find('.pdfname').text(fileCountMsg).show();
                    }
                    else {
                        $(imagectrl).attr('src', event.target.result);
                        $(noimagectrl).addClass('hide-element');
                        $(imagectrl).removeClass('hide-element');
                    }
                    $(removestatus).val(false);
                    $(".company-logo-loading").hide();
                    $(imageblock).removeClass("pntr-none subSectionOpacity");
                    goUpload = true; return true;
                };
                reader.readAsDataURL(e.target.files[0]);
            }
        }
    });
}

function isUploadedFilesValid(files, isFileTypeChk, isFileSizeCheck) {
    var isValid = true;
    if (isFileSizeCheck) {
        for (var i = 0; i < files.length; i++) {
            if (files[i].size > 5242880) { // 5mb
                isValid = false;
                break;
            }
        }
    }
    if (isFileTypeChk) {
        for (var j = 0; j < files.length; j++) {
            if (!(/\.(jpg|jpeg|png|pdf|bmp)$/i).test(files[j].name)) {
                isValid = false;
                break;
            }
        }
    }
    return isValid;
}
