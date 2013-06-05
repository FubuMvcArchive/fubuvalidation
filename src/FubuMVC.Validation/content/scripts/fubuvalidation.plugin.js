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

    function processNotification(notification, form, element) {
        var continuation = notification.toContinuation();
        continuation.form = form;
        continuation.element = element;
        validation.Processor.process(continuation);

        form.storeNotification(notification);
    }
    
    function elementHandler(element, form) {
        var notification = form.notification();
        var elementNotification = new validation.Core.Notification();

        var target = validation.Core.Target.forElement(element, form.attr('id'));
        validation.Validator.validate(target, elementNotification);

        notification.importForTarget(elementNotification, target);

        processNotification(notification, form, element);
    }

    function bindEvents(form) {
        form
            .on("change", "input:not(:checkbox,:submit,:reset,:image,[disabled]),textarea:not([disabled])", function (e) {
                var element = $(e.target);
                if (element.data("validation-onchange-fired") === true) {
                    return;
                }
                elementHandler(element, form);
                element.data("validation-onchange-fired", true);
            })
            .on("keyup", "input:not(:checkbox,:submit,:reset,:image,[disabled]),textarea:not([disabled])", function (e) {
                var element = $(e.target);
                if (element.data("validation-onchange-fired") === true) {
                    var timeout = element.data("validation-timeout");
                    if (timeout != undefined) {
                        clearTimeout(timeout);
                    }
                    element.data("validation-timeout", setTimeout(function () {
                        elementHandler(element, form);
                    }, 500));
                }
            })
            .on("change", "input:radio:not([disabled]),input:checkbox:not([disabled]),select:not([disabled])", function (e) {
                var element = $(e.target);
                elementHandler(element, form);
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

	$.fn.activateValidation = function () {
		return this.each(function () {
			var container = $(this);
			$('form.validated-form', container).each(function () {
			  var form = $(this);
			  form.off('submit.fubu');

				var mode = form.data('formMode');
				$(this).validate({
					ajax: mode == 'ajax',
					continuationSuccess: function (continuation) {
						if (continuation.success && continuation.form) {
							continuation.form.trigger('validation:success', [continuation]);
						}
					}
				});
			});
		});
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
