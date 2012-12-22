$(document).ready(function () {
    $('form.validated-form').each(function () {
        var mode = $(this).data('validateMode');
        $(this).validate({
            ajax: mode == 'ajax'
        });
    });
});