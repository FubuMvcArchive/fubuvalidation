(function ($, validation) {
    var exports = {
        Strategies: {}
    };

    function defineCore(name, clazz) {
        exports[name] = clazz;
    }

    function defineStrategy(name, clazz) {
        exports.Strategies[name] = clazz;
    }

    function DefaultHandler() {
        this.strategies = [];
    }

    DefaultHandler.prototype = {
        registerStrategy: function (strategy) {
            this.strategies.push(strategy);
        },
        strategiesMatching: function (continuation, action) {
            _.each(this.strategies, function (strategy) {
                if (strategy.matches(continuation)) {
                    action(strategy);
                }
            });
        },
        process: function (continuation) {
            this.reset(continuation);
            this.strategiesMatching(continuation, function (strategy) {
                strategy.render(continuation);
            });
        },
        reset: function (continuation) {
            this.strategiesMatching(continuation, function (strategy) {
                strategy.reset(continuation);
            });
        }
    };

    DefaultHandler.basic = function () {
        var handler = new DefaultHandler();
        handler.registerStrategy(new ValidationSummaryStrategy());
        handler.registerStrategy(new ElementHighlightingStrategy());
        handler.registerStrategy(new InlineErrorStrategy());

        return handler;
    };

    function ValidationProcessor(handler) {
        this.handler = handler;
        this.finders = [];
    }

    ValidationProcessor.prototype = {
        searchContext: {},
        useValidationHandler: function (handler) {
            this.handler = handler;
        },
        findElementsWith: function (finder) {
            this.finders.push(finder);
        },
        findElement: function (continuation, key, error) {
            this.searchContext.key = key;
            this.searchContext.error = error;
            this.searchContext.form = continuation.form;

            for (var i = 0; i < this.finders.length; i++) {
                var finder = this.finders[i];
                finder(this.searchContext);
            }

            return this.searchContext.error.element;
        },
        fillElements: function (continuation) {
            for (var i = 0; i < continuation.errors.length; i++) {
                var error = continuation.errors[i];
                if (!error.element && error.field) {
                    error.element = this.findElement(continuation, error.field, error);
                }
            }
        },
        process: function (continuation) {
            this.fillElements(continuation);
            this.handler.process(continuation);
        },
        registerStrategy: function (strategy) {
            if (typeof (this.handler.registerStrategy) == 'function') {
                this.handler.registerStrategy(strategy);
            }
        },
        reset: function (continuation) {
            this.handler.reset(continuation);
        }
    };

    ValidationProcessor.basic = function () {
        var handler = DefaultHandler.basic();
        var processor = new ValidationProcessor(handler);

        processor.findElementsWith(function (context) {
            var element = context.form.find('input[name="' + context.key + '"]');
            if (element.size() == 1) {
                context.error.element = element;
            }
        });

        processor.findElementsWith(function (context) {
            if (context.error.element) return;
          
            var element = context.form.find('#' + context.key);
            if (element.size() == 1) {
                context.error.element = element;
            }
        });

        return processor;
    };

    function tokenFor(error) {
        return error.label + ' - ' + error.message;
    }

    function ValidationSummaryStrategy() {
    }

    ValidationSummaryStrategy.prototype = {
        matches: function (continuation) {
            var value = continuation.form.data('validationSummary');
            return typeof (value) != 'undefined';
        },
        summaryContext: function (continuation) {
            var container = $('.validation-container', continuation.form);
            return {
                container: container,
                summary: container.find('ul.validation-summary')
            };
        },
        reset: function (continuation, context) {
            if (!context) {
                context = this.summaryContext(continuation);
            }

            context.summary.html('');
            context.container.hide();
        },
        render: function (continuation) {
            var context = this.summaryContext(continuation);
            if (continuation.errors.length == 0) {
                return;
            }

            context.container.show();

            for (var i = 0; i < continuation.errors.length; i++) {
                var error = continuation.errors[i];
                this.append(context, error);
            }
        },
        append: function (context, error) {
            var found = false;
            context
				.summary
                .find("li[data-field='" + error.field + "']")
                .each(function () {
                    if (found) return;
                    if ($(this).find('a').html() == error.message) {
                        found = true;
                    }
                });

            if (!found) {
                var token = tokenFor(error);
                var item = $('<li data-field="' + error.field + '"><a href="javascript:void(0);">' + token + '</a></li>');

                if (error.element) {
                    item.find('a').click(function () {
                        item.element.focus();
                    });
                }

                context.summary.append(item);
            }
        }
    };

    function ElementHighlightingStrategy() {
    }

    ElementHighlightingStrategy.prototype = {
        matches: function (continuation) {
            var value = continuation.form.data('validationHighlight');
            return typeof (value) != 'undefined';
        },
        reset: function (continuation) {
            continuation.form.find('.error').each(function () {
                $(this).removeClass('error');
            });
        },
        render: function (continuation) {
            this.eachError(continuation, function (error) {
                error.element.addClass('error');
            });
        },
        eachError: function (continuation, action) {
            for (var i = 0; i < continuation.errors.length; i++) {
                var error = continuation.errors[i];
                if (error.element) {
                    action(error);
                }
            }
        }
    };

    function InlineErrorStrategy() {
    }

    InlineErrorStrategy.prototype = {
        matches: function (continuation) {
            var value = continuation.form.data('validationInline');
            return typeof (value) != 'undefined';
        },
        reset: function (continuation) {
            continuation.form.find('.fubu-inline-error').each(function () {
                $(this).remove();
            });
        },
        render: function (continuation) {
            this.eachError(continuation, function (error) {
                var message = $('<span class="help-inline fubu-inline-error" data-field="' + error.field + '">' + error.message + '</span>');

                error.element.after(message);
            });
        },
        eachError: function (continuation, action) {
            for (var i = 0; i < continuation.errors.length; i++) {
                var error = continuation.errors[i];
                if (error.element) {
                    action(error);
                }
            }
        }
    };
    
    function ValidationFormController(validator, processor) {
        this.validator = validator;
        this.processor = processor;
    }
    
    ValidationFormController.prototype = {
        submitHandler: function(form) {
            form = $(form);
            var notification = this.validateForm(form);
            this.processNotification(notification, form);
            return notification.isValid();
        },
        elementsFor: function(form) {
            return form.find("input, select, textarea").not(":submit, :reset, :image, [disabled]");
        },
        validateForm: function(form) {
            var self = this;
            var elements = this.elementsFor(form);
            var notification = new validation.Core.Notification();
            var options = validation.Core.Options.fromForm(form);

            elements.each(function () {
              var target = validation.Core.Target.forElement($(this), form.attr('id'), form);
              self.validator.validate(target, options, validation.Core.ValidationMode.Triggered, notification);
            });

            return notification;
        },
        processNotification: function(notification, form, element) {
            var continuation = notification.toContinuation();
            continuation.form = form;
            continuation.element = element;
            this.processor.process(continuation);

            form.storeNotification(notification);
            form.trigger('validation:processed', [continuation]);
        },
        elementHandler: function(element, form) {
            var notification = form.notification();
            var elementNotification = new validation.Core.Notification();
            var options = validation.Core.Options.fromForm(form);

            var target = validation.Core.Target.forElement(element, form.attr('id'), form);
            this.validator.validate(target, options, validation.Core.ValidationMode.Live, elementNotification);

            notification.importForTarget(elementNotification, target);

            this.processNotification(notification, form, element);
        },
        bindEvents: function(form) {
            var self = this;
            form
                .on("change", "input:not(:checkbox,:submit,:reset,:image,[disabled]),textarea:not([disabled])", function (e) {
                  var element = $(e.target);
                  self.elementHandler(element, form);
                })
                .on("keyup", "input:not(:checkbox,:submit,:reset,:image,[disabled]),textarea:not([disabled])", function (e) {
                  var element = $(e.target);
                  var timeout = element.data("validation-timeout");
                  if (timeout != undefined) {
                    clearTimeout(timeout);
                  }
                  
                  element.data("validation-timeout", setTimeout(function () {
                    self.elementHandler(element, form);
                  }, 500));
                })
                .on("change", "input:radio:not([disabled]),input:checkbox:not([disabled]),select:not([disabled])", function (e) {
                  var element = $(e.target);
                  self.elementHandler(element, form);
                });
        }
    };
    
    $.fn.storeNotification = function (notification) {
        $.data(this[0], 'fubu-notification', notification);
    };

    $.fn.notification = function () {
        var notification = $.data(this[0], 'fubu-notification');
        if (typeof (notification) == 'undefined' || !notification) {
            notification = new validation.Core.Notification();
            this.storeNotification(notification);
        }

        return notification;
    };


    defineCore('DefaultHandler', DefaultHandler);
    defineCore('ValidationProcessor', ValidationProcessor);
    defineCore('TokenFor', tokenFor);
    defineCore('Controller', ValidationFormController);

    defineStrategy('Summary', ValidationSummaryStrategy);
    defineStrategy('Highlighting', ElementHighlightingStrategy);
    defineStrategy('Inline', InlineErrorStrategy);

    $.extend(true, $, { 'fubuvalidation': { 'UI': exports } });

}(jQuery, jQuery.fubuvalidation));