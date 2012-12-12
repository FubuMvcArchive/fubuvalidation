(function ($, validation) {
    // You can compose this however you like
    validation.Validator = $.fubuvalidation.Core.Validator.basic();

    function submitHandler(form) {
        var elements = elementsFor(form);
        var notification = new validation.Core.Notification();

        elements.each(function () {
            var target = validation.Core.Target.forElement($(this));
            validation.Validator.validate(target, notification);
        });

        var continuation = notification.toContinuation();
        $.fubuvalidation.ui.process(continuation);

        return notification.isValid();
    }

    function elementsFor(form) {
        return $(form)
            .find("input, select, textarea")
            .not(":submit, :reset, :image, [disabled]");
    }

    $.fn.validate = function (options) {
        return this.each(function () {
            var settings = {
                ajax: true
            };

            settings = $.extend(true, settings, options);

            $(this).submit(function () {
                var submit = submitHandler(this);
                if (!submit) {
                    return false;
                }

                if (settings.ajax) {
                    $(this).ajaxSubmit();
                    return false;
                }

                return true;
            });
        });
    };

} (jQuery, jQuery.fubuvalidation));