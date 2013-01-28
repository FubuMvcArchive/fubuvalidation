$(document).ready(function () {
    $.fn.activateAjaxForms = function () {
        return this.each(function () {
            var container = $(this);
            $('form.validated-form', container).each(function () {
                var mode = $(this).data('validationMode');
                $(this).validate({
                    ajax: mode == 'ajax',
                    continuationSuccess: function (continuation) {
                        continuation.form.trigger('validation:success', continuation);
                    }
                });
            });
        });
    };
    $(document).activateAjaxForms();
});
