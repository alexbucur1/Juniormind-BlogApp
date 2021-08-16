$(document).ready(function () {
    $("#file-upload-image").change(function () {
        var extension = $(this).val().split('.').pop().toLowerCase();
        var validFileExtensions = ['jpeg', 'jpg', 'png', 'gif', 'bmp', 'tif'];
        if ($.inArray(extension, validFileExtensions) == -1) {
            $('#spn-err-msg').text("Invalid file! Upload only .jpg, .jpeg, .png, .gif, .bmp, or .tif files.");
            $('#spn-err-msg').removeClass("d-none");
            $(this).replaceWith($(this).val('').clone(true));
            $('#btn-submit').prop('disabled', true);
            $('#img-preview').addClass('d-none');
        }
        else {

            $('#spn-err-msg').text('');
            $('#spn-err-msg').addClass('d-none');
            $('#btn-submit').prop('disabled', false);
        }
    });
});