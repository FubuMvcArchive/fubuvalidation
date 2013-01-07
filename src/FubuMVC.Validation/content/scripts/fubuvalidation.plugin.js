(function ($, validation, continuations) {
	
    // You can compose these however you like
    validation.Validator = $.fubuvalidation.Core.Validator.basic();
	validation.Processor = $.fubuvalidation.UI.ValidationProcessor.basic();

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
        validation.Processor.process(continuation);

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

    $.fn.bindAll = function (delegate, type, handler) {
        return this.bind(type, function (event) {
            var target = $(event.target);
            if (target.is(delegate)) {
                return handler.apply(target, arguments);
            }
        });
    };

    function bindEvents(form) {
        var selector = ":text, [type='password'], [type='file'], select, textarea, " +
            "[type='number'], [type='search'] ,[type='tel'], [type='url'], " +
                "[type='email'], [type='datetime'], [type='date'], [type='month'], " +
                    "[type='week'], [type='time'], [type='datetime-local'], " +
                        "[type='range'], [type='color'] ";

        form.bindAll(selector, 'focusin focusout keyup', function () {
            elementHandler($(this), form);
        });

        form.bindAll('select', 'change click', function () {
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
                ajax: true,
                continuationSuccess: function (continuation) {
                    // no -op
                }
            };

            bindEvents($(this));

            settings = $.extend(true, settings, options);

            $(this).submit(function () {
                if (!submitHandler(this)) {
                    return false;
                }

                if (settings.ajax) {
                    $(this).correlatedSubmit({
                        continuationSuccess: settings.continuationSuccess
                    });
                    return false;
                }

                return true;
            });
        });
    };

	var _reset = $.fn.resetForm;
	$.fn.resetForm = function () {
		var continuation = new continuations.continuation();
		continuation.success = true;
		continuation.form = $(this);

		validation.Processor.reset(continuation);
		
		return _reset.call(this);
    };
	
	continuations.applyPolicy({
        matches: function (continuation) {
            return continuation.matchOnProperty('form', function (form) {
                return form.size() != 0;
            });
        },
        execute: function (continuation) {
            if (!continuation.errors) {
                continuation.errors = [];
            }
        	
            validation.Processor.process(continuation);
        }
    });

} (jQuery, jQuery.fubuvalidation, jQuery.continuations));