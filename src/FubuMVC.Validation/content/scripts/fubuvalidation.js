// fubuvalidation.js v0.5.7
//
// Copyright (C)2011 Joshua Arnold
// Distributed under Apache License, Version 2.0
//
// https://github.com/DarthFubuMVC/fubuvalidation-js

(function ($, continuations) {
    _.templateSettings = { interpolate: /\{\{(.+?)\}\}/g };

    var handlers = [];
    var finders = [];

    var defaultHandler = function () { };
    defaultHandler.prototype = {
        matches: function (context) { return true; },
        reset: function (context) {
            var self = this;
            context.container.find('.validation-summary').html('');
            context.container.hide();

            $('.error', context.form).each(function () {
                self.unhighlight($(this));
            });
        },
        process: function (context) {
            var self = this;
            var container = $('.validation-container', context.form);
            context.container = container;
            context.summary = container.find('ul.validation-summary');
            this.reset(context);

            if (context.errors.length == 0) {
                return;
            }

            container.show();

            $.fubuvalidation.eachError(context, function (error) {
                self.append(context, error);
                self.highlight(error);
            });
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
                        return;
                    }
                });

            if (!found) {
                var self = this;
                var token = $(_.template('<li data-field="{{ label }}"><a href="javascript:void(0);">{{ token }}</a></li>', {
                    field: error.field,
                    label: error.label,
                    token: self.generateToken(error)
                }));
                token.find('a').click(function () {
                    $.fubuvalidation.findElement(context, error.field).focus();
                });
                context.summary.append(token);
            }
        },
        generateToken: function (error) {
            return _.template('{{ label }} - {{ message }}', error);
        },
        highlight: function (error) {
            if (error.element) {
                $(error.element).addClass('error');
            }
        },
        unhighlight: function (element) {
            element.removeClass('error');
        }
    };
    // This instance is registered by default and made public via $.fubuvalidation.defaultHandler
    var theDefault = new defaultHandler();

    var validation = function () { };
    validation.prototype = {
        init: function () {
            this.setupDefaults();
        },
        // this is here for testing
        reset: function () {
            handlers.length = 0;
            finders.length = 0;

            this.setupDefaults();
        },
        setupDefaults: function () {
            this.findElementsWith(function (searchContext) {
                searchContext.element = $('#' + searchContext.key, searchContext.form);
            });

            this.registerHandler(theDefault);
        },
        registerHandler: function (handler) {
            handlers.push(handler);
            return this;
        },
        findHandler: function (context) {
            var handler;
            for (var i = 0; i < handlers.length; i++) {
                var x = handlers[i];
                if (x.matches(context)) {
                    handler = x;
                }
            }

            return handler;
        },
        findElementsWith: function (finder) {
            finders.push(finder);
            return this;
        },
        findElement: function (context, key, error) {
            var searchContext = {
                key: key,
                error: error,
                form: context.form
            };

            for (var i = 0; i < finders.length; i++) {
                var finder = finders[i];
                finder(searchContext);
            }

            return searchContext.element;
        },
        toValidationContext: function (continuation) {
            var self = this;
            this.eachError(continuation, function (e) {
                if (!e.element) {
                    e.element = self.findElement(continuation, e.field, e);
                }
            });

            return continuation;
        },
        eachError: function (context, action) {
            for (var i = 0; i < context.errors.length; i++) {
                action(context.errors[i]);
            }
        },
        process: function (continuation) {
            var context = this.toValidationContext(continuation);
            var handler = this.findHandler(context);

            handler.process(context);
        }
    };

    var module = new validation();
    module.init();

    $.fubuvalidation = module;
    // export the class for extension
    $.fubuvalidation.defaultHandlerClass = defaultHandler;
    $.fubuvalidation.defaultHandler = theDefault;

    var reset = $.fn.resetForm;
    $.fn.resetForm = function () {
        var context = {
            form: $(this),
            container: $('.validation-container', $(this))
        };
        $.fubuvalidation.defaultHandler.reset(context);
        reset.call(this);
    };

    $.continuations.applyPolicy({
        matches: function (continuation) {
            return continuation.matchOnProperty('form', function(form) {
				return form.size() != 0;
			});
        },
        execute: function (continuation) {
            if (!continuation.errors) {
                continuation.errors = [];
            }
            $.fubuvalidation.process(continuation);
        }
    });
} (jQuery));