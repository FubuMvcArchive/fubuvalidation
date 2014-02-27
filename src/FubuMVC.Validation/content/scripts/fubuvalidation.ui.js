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

    function RenderingContext(continuation, element, mode) {
        this.continuation = continuation;
        this.element = element;
        this.mode = mode;
    }

    RenderingContext.prototype = {
        isValid: function () {
            return this.continuation.success && this.continuation.errors.length == 0;
        },
        isTriggered: function () {
            return this.mode === validation.Core.ValidationMode.Triggered;
        },
        isLive: function () {
            return this.mode === validation.Core.ValidationMode.Live;
        },
        isServerGenerated: function () {
            return this.continuation.validationOrigin === 'server';
        },
        isEntireForm: function () {
            return !this.element || typeof (this.element) === 'undefined';
        },
        eachError: function (action) {
            if (!this.element) {
                var errors = this.continuation.errors;
                for (var i = 0; i < errors.length; i++) {
                    action(errors[i]);
                }

                return;
            }

            this.errorsForElement(action);
        },
        errorsForElement: function (action) {
            var errors = this.continuation.errors;
            for (var i = 0; i < errors.length; i++) {
                var error = errors[i];
                if (error.element == this.element) {
                    action(error);
                }
            }
        }
    };

    function DefaultHandler() {
        this.strategies = [];
    }

    DefaultHandler.prototype = {
        registerStrategy: function (strategy) {
            this.strategies.push(strategy);
        },
        strategiesMatching: function (context, action) {
            _.each(this.strategies, function (strategy) {
                if (strategy.matches(context)) {
                    action(strategy);
                }
            });
        },
        init: function(element, target, rules) {
            var context = {
              element: element,
              target: target,
              rules: rules
            };

            _.each(this.strategies, function (strategy) {
              if (_.isFunction(strategy.initMatches) &&
                  strategy.initMatches(context) &&
                  _.isFunction(strategy.init)) {

                  strategy.init(context);
                }
            });
        },
        process: function (context) {
            this.reset(context);
            this.strategiesMatching(context, function (strategy) {
                strategy.render(context);
            });
        },
        reset: function (context) {
            this.strategiesMatching(context, function (strategy) {
                strategy.reset(context);
            });
        }
    };

    DefaultHandler.basic = function () {
        var handler = new DefaultHandler();
        handler.registerStrategy(new ValidationSummaryStrategy());
        handler.registerStrategy(new ElementHighlightingStrategy());
        handler.registerStrategy(new InlineErrorStrategy());
        handler.registerStrategy(new CountStrategy());

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
        process: function (context) {
            this.fillElements(context.continuation);
            this.handler.process(context);
        },
        registerStrategy: function (strategy) {
            if (typeof (this.handler.registerStrategy) == 'function') {
                this.handler.registerStrategy(strategy);
            }
        },
        reset: function (context) {
            this.handler.reset(context);
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
        matches: function (context) {
            var continuation = context.continuation;
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
        reset: function (renderingContext, summaryContext) {
            var continuation = renderingContext.continuation;
            if (!summaryContext) {
                summaryContext = this.summaryContext(continuation);
            }

            summaryContext.summary.html('');
            summaryContext.container.hide();
        },
        render: function (context) {
            var continuation = context.continuation;
            var summaryContext = this.summaryContext(continuation);
            if (continuation.errors.length == 0) {
                return;
            }

            summaryContext.container.show();

            for (var i = 0; i < continuation.errors.length; i++) {
                var error = continuation.errors[i];
                this.append(summaryContext, error);
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
        matches: function (context) {
            var continuation = context.continuation;
            var value = continuation.form.data('validationHighlight');
            return typeof (value) != 'undefined';
        },
        reset: function (context) {
            var continuation = context.continuation;
            if (context.isEntireForm()) {
                continuation.form.find('.error').each(function () {
                    $(this).removeClass('error');
                });
            }
            else {
                context.eachError(function (error) {
                    if (error.element) {
                        error.element.removeClass('error');
                    }
                });
            }
        },
        render: function (context) {
            context.eachError(function (error) {
                if (error.element) {
                    error.element.addClass('error');
                }
            });
        }
    };

    function InlineErrorStrategy() {
    }

    InlineErrorStrategy.prototype = {
        matches: function (context) {
            var continuation = context.continuation;
            var value = continuation.form.data('validationInline');
            return typeof (value) != 'undefined';
        },
        reset: function (context) {
            var continuation = context.continuation;
            continuation.form.find('.fubu-inline-error').each(function () {
                $(this).remove();
            });
        },
        render: function (context) {
            context.eachError(function (error) {
                var message = $('<span class="help-inline fubu-inline-error" data-field="' + error.field + '">' + error.message + '</span>');

                error.element.after(message);
            });
        }
    };

    function CountStrategy() {
      // jQuery $(<selector>).data('validation-count', <value>) will not update the DOM. One of the primary reasons for this strategy is for
      // a WebDriver ElementHandler to wait for validation to complete before proceeding. We will need the attribute set on the DOM so that
      // element handler can retrieve the value.
      this.dataKey = 'data-validation-count';
    };

    CountStrategy.prototype = {
      initMatches: function (context) {
        return context.rules !== undefined && context.rules.length > 0;
      },
      matches: function (context) {
        return true;
      },
      init: function(context) {
        context.element.attr(this.dataKey, 0);
      },
      reset: function (context) {
      },
      render: function (context) {
        if (!context.element) {
          return;
        }

        var element = context.element,
            version = parseInt(element.attr(this.dataKey)) + 1;

        element.attr(this.dataKey, version);
      }
    };

    function FormValidated(notification) {
        this.notification = notification;
        this._submit = true;
    }

    FormValidated.prototype = {
        preventSubmission: function () {
            this._submit = false;
        },
        shouldSubmit: function () {
            return this._submit && this.notification.isValid();
        }
    };

    function ValidationFormController(validator, processor) {
        this.validator = validator;
        this.processor = processor;
        this.targetCache = {};
    }

    ValidationFormController.prototype = {
        submitHandler: function (form) {
            form = $(form);
            var self = this;

            return this.validateForm(form).done(function (notification) {
                self.processNotification(notification, form);
            });
        },
        elementsFor: function (form) {
            return form.find("input, select, textarea").not(":submit, :reset, :image, [disabled]");
        },
        validateForm: function (form) {
            var self = this;
            var elements = this.elementsFor(form);
            var notification = form.notification();
            var options = validation.Core.Options.fromForm(form);

            form.trigger('validation:start');

            var results = [];
            elements.each(function () {
                var target = validation.Core.Target.forElement($(this), form.attr('id'), form);
                var mode = validation.Core.ValidationMode.Triggered;

                if (!self.shouldValidate(target, mode)) {
                    return;
                }

                var result = self.validator.validate(target, options, mode, notification);
                result.done(function () {
                    self.targetValidated(target, mode);
                });

                results.push(result);
            });

            var promise = $.Deferred();
            $.when.apply($, results).always(function () {
                promise.resolve(notification);
            });

            return promise;
        },
        processNotification: function (notification, form, element) {
            var continuation = notification.toContinuation();
            form.storeNotification(notification);
            this.processContinuation(continuation, form, element);
        },
        processContinuation: function (continuation, form, element) {
            if (form) {
                continuation.form = form;
            }

            continuation.element = element;

            var context = new RenderingContext(continuation, element, continuation.mode);
            this.processor.process(context);

            var formToTrigger = form || continuation.form;
            if (formToTrigger) {
                formToTrigger.trigger('validation:processed', [continuation]);
            }
        },
        elementHandler: function (element, form) {
            var notification = form.notification();
            var options = validation.Core.Options.fromForm(form);
            var target = validation.Core.Target.forElement(element, form.attr('id'), form);
            var mode = validation.Core.ValidationMode.Live;
            var plan = this.validator.planFor(target, options, mode);
            var self = this;

            if (!this.shouldValidate(target, mode) || plan.isEmpty()) {
                return $.when({});
            }

            var result = this.validator.validate(target, options, mode);
            result.done(function (elementNotification) {
                notification.importForTarget(elementNotification, target);
                self.processNotification(notification, form, element);
                self.targetValidated(target, mode);
            });

            return result;
        },
        bindEvents: function (form) {
            var self = this;
            var options = validation.Core.Options.fromForm(form);
            var mode = validation.Core.ValidationMode.Live;
            var initSelector = "input:not(:submit,:reset,:image,[disabled]),textarea:not([disabled])";
            var init = function() {
              var element = $(this)
                , target = validation.Core.Target.forElement(element, form.attr('id'), form)
                , rules = self.validator.rulesFor(target);

              self.processor.handler.init(element, target, rules);
            };

          form.find(initSelector).each(init);

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
                  }, options.elementTimeout));
              })
              .on("validation:bustCache", "input:not(:checkbox,:submit,:reset,:image,[disabled]),textarea:not([disabled])", function (e) {
                  var element = $(e.target);
                  var target = validation.Core.Target.forElement(element, form.attr('id'), form);
                  self.invalidateTarget(target, mode);
              })
              .on("change", "input:radio:not([disabled]),input:checkbox:not([disabled]),select:not([disabled])", function (e) {
                  var element = $(e.target);
                  self.elementHandler(element, form);
              })
              .on("load", initSelector, init);
        },
        targetValidated: function (target, mode) {
            var key = this.hashFor(target, mode);
            this.targetCache[key] = target.value();
        },
        shouldValidate: function (target, mode) {
            var key = this.hashFor(target, mode);
            if (typeof (this.targetCache[key]) === 'undefined') {
                return true;
            }

            return this.targetCache[key] != target.value();
        },
        invalidateTarget: function (target, mode) {
            var key = this.hashFor(target, mode);
            if (this.targetCache[key] !== undefined) {
                delete this.targetCache[key];
                target.element.trigger('change');
            }
        },
        hashFor: function (target, mode) {
            return target.toHash() + '&mode=' + mode;
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

    defineCore('RenderingContext', RenderingContext);
    defineCore('DefaultHandler', DefaultHandler);
    defineCore('ValidationProcessor', ValidationProcessor);
    defineCore('TokenFor', tokenFor);
    defineCore('Controller', ValidationFormController);
    defineCore('FormValidated', FormValidated);

    defineStrategy('Summary', ValidationSummaryStrategy);
    defineStrategy('Highlighting', ElementHighlightingStrategy);
    defineStrategy('Inline', InlineErrorStrategy);
    defineStrategy('Count', CountStrategy);

    $.extend(true, $, { 'fubuvalidation': { 'UI': exports } });

}(jQuery, jQuery.fubuvalidation));
