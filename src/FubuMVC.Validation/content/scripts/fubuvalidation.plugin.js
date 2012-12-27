(function ($, validation) {
    // You can compose this however you like
    validation.Validator = $.fubuvalidation.Core.Validator.basic();

    function submitHandler(form) {
        form = $(form);
        var elements = elementsFor(form);
        var notification = new validation.Core.Notification();

        elements.each(function () {
            var target = validation.Core.Target.forElement($(this), form.attr('id'));
            validation.Validator.validate(target, notification);
        });

        processNotification(notification, form);

        return notification.isValid();
    }

    function elementsFor(form) {
        return form.find("input, select, textarea").not(":submit, :reset, :image, [disabled]");
    }

    function processNotification(notification, form) {
        var continuation = notification.toContinuation();
        continuation.form = form;
        $.fubuvalidation.ui.process(continuation);

        form.storeNotification(notification);
    }

    function elementHandler(element, form) {
        var notification = form.notification();
        var elementNotification = new validation.Core.Notification();

        var target = validation.Core.Target.forElement(element, form.attr('id'));
        validation.Validator.validate(target, elementNotification);

        notification.importForTarget(elementNotification, target);

        processNotification(notification, form);
    }

    function bindEvents(form) {
        var selector = ":text, [type='password'], [type='file'], select, textarea, " +
            "[type='number'], [type='search'] ,[type='tel'], [type='url'], " +
                "[type='email'], [type='datetime'], [type='date'], [type='month'], " +
                    "[type='week'], [type='time'], [type='datetime-local'], " +
                        "[type='range'], [type='color'] ";

        $(selector, form).bind('focusin focusout keyup', function () {
            // TODO -- Consider scoping the notification to the single element
            elementHandler($(this), form);
        });
    }

    $.fn.storeNotification = function (notification) {
        $.data(this[0], 'fubu-notification', notification);
    };

    $.fn.notification = function () {
        var notification = $.data(this[0], 'fubu-notification');
        if (!notification || typeof (notification) == 'undefined') {
            notification = new validation.Core.Notification();
            this.storeNotification(notification);
        }

        return notification;
    };

    $.fn.validate = function (options) {
        return this.each(function () {
            var settings = {
                ajax: true
            };

            bindEvents($(this));

            settings = $.extend(true, settings, options);

            $(this).submit(function () {
                if (!submitHandler(this)) {
                    return false;
                }

                if (settings.ajax) {
                    $(this).correlatedSubmit();
                    return false;
                }

                return true;
            });
        });
    };

} (jQuery, jQuery.fubuvalidation));