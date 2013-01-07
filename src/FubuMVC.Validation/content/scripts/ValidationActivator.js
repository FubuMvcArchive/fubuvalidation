$(document).ready(function () {
    $('form.validated-form').each(function () {
        var mode = $(this).data('validationMode');
        $(this).validate({
            ajax: mode == 'ajax',
            continuationSuccess: function (continuation) {
                continuation.form.trigger('validation:success', continuation);
            }
        });
    });
});
