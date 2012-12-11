(function ($, validation) {
    var sources = {};
    var defineSource = function (key, source) {
        sources[key] = source;
    };


    function ValidationNotification() {
        this.messages = {};
    };

    ValidationNotification.prototype = {
        messagesFor: function (field) {
            var messages = this.messages[field];
            if (typeof (messages) == 'undefined') {
                messages = [];
                this.messages[field] = messages;
            }

            return messages;
        },

        registerMessage: function (field, token, element, context) {
            context = context || {};
            var messages = this.messagesFor(field);
            messages.push({ field: field, token: token, element: element, context: context });
        },

        allMessages: function () {
            var messages = [];

            for (var key in this.messages) {
                var values = this.messages[key];
                _.each(values, function (value) {
                    messages.push(value);
                });
            }

            return messages;
        },

        isValid: function () {
            return this.allMessages().length == 0;
        },

        toContinuation: function () {
            var continuation = new $.continuations.continuation();
            continuation.success = this.isValid();

            var messages = this.allMessages();
            for (var i = 0; i < messages.length; i++) {
                var message = messages[i];
                continuation.errors.push({
                    field: message.field,
                    label: message.field, // TODO -- maybe do a localization trick here
                    message: _.template(message.token.toString(), message.context)
                });
            }

            return continuation;
        }
    };

    function ValidationTarget(fieldName, value) {
        this.fieldName = fieldName;
        this.rawValue = value;
    };

    ValidationTarget.prototype = {
        value: function () {
            var value = this.rawValue;
            if (typeof (this.element) != 'undefined') {
                value = $(this.element).val();
            }

            return $.trim(value);
        }
    };

    ValidationTarget.forElement = function (element) {
        var target = new ValidationTarget(element.attr('name'));
        target.element = element;

        return target;
    };

    function ValidationContext(target, notification) {
        this.target = target;
        this.notification = notification || new ValidationNotification();
    };

    ValidationContext.prototype = {
        pushTemplateContext: function (context) {
            this.templateContext = context;
        },
        registerMessage: function (message) {
            this.notification.registerMessage(this.target.fieldName, message, this.target.element, this.templateContext);
        }
    };

    function Validator(sources) {
        this.sources = sources || [];

        var self = this;
        this.rulesFor = _.memoize(function (target) {
            var rules = [];
            _.each(self.sources, function (src) {
                var targetRules = src.rulesFor(target);
                rules = rules.concat(targetRules);
            });

            return rules;
        });
    };

    Validator.prototype = {
        registerSource: function (source) {
            this.sources.push(source);
        },
        validate: function (target) {
            var notification = new ValidationNotification();
            var context = new ValidationContext(target, notification);
            var rules = this.rulesFor(target);

            _.each(rules, function (rule) {
                context.pushTemplateContext(rule);
                rule.validate(context);
            });

            return notification;
        }
    };

    Validator.basic = function () {
        var validationSources = [];
        for (var key in sources) {
            validationSources.push(sources[key]);
        }

        return new Validator(validationSources);

    };

    function CssValidationAliasRegistry() {
        this.rules = {};
        this.registerDefaults();
    };

    CssValidationAliasRegistry.prototype = {
        registerDefaults: function () {
            this.registerRule('required', validation.Rules.Required);
            this.registerRule('email', validation.Rules.Email);
            this.registerRule('date', validation.Rules.Date);
            this.registerRule('number', validation.Rules.Number);
        },

        ruleFor: function (alias, target) {
            var builder = this.rules[alias];
            if (typeof (builder) == 'undefined') {
                return null;
            }

            return builder(target);
        },

        registerRule: function (alias, rule) {
            var builder = rule;
            if (typeof (builder) != 'function') {
                builder = function () { return rule; }
            }
            this.rules[alias] = builder;
        }
    };

    function CssValidationRuleSource(registry) {
        this.registry = registry;
    };

    CssValidationRuleSource.prototype = {
        classesFor: function (element) {
            var classes = element.attr('class');
            if (!classes || classes == '' || typeof (classes) == 'undefined') {
                return [];
            }

            return classes.split(' ');
        },
        rulesFor: function (target) {
            var rules = [];
            var classes = this.classesFor(target.element);
            var registry = this.registry;

            _.each(classes, function (alias) {
                var rule = registry.ruleFor(alias, target);
                if (rule) {
                    rules.push(rule);
                }
            });

            return rules;
        }
    };

    CssValidationRuleSource.basic = function () {
        return new CssValidationRuleSource(new CssValidationAliasRegistry());
    };

    function rulesForData(target, data, continuation) {
        var rules = [];

        var value = target.element.data(data);
        if (typeof (value) != 'undefined') {
            rules.push(continuation(value));
        }

        return rules;
    };

    defineSource('CssRules', CssValidationRuleSource.basic());
    defineSource('MinLength', {
        rulesFor: function (target) {
            return rulesForData(target, 'minlength', function (value) {
                return new validation.Rules.MinLength(value);
            });
        }
    });
    defineSource('MaxLength', {
        rulesFor: function (target) {
            var rules = [];

            var value = target.element.attr('maxlength');
            if (typeof (value) != 'undefined') {
                rules.push(new validation.Rules.MaxLength(parseInt(value)));
            }

            return rules;
        }
    });
    defineSource('RangeLength', {
        rulesFor: function (target) {
            return rulesForData(target, 'rangelength', function (value) {
                return new validation.Rules.RangeLength(value.min, value.max);
            });
        }
    });
    defineSource('Min', {
        rulesFor: function (target) {
            return rulesForData(target, 'min', function (value) {
                return new validation.Rules.Min(value);
            });
        }
    });
    defineSource('Max', {
        rulesFor: function (target) {
            return rulesForData(target, 'max', function (value) {
                return new validation.Rules.Max(value);
            });
        }
    });

    $.extend(true, validation, {
        'Core': {
            'Context': ValidationContext,
            'Notification': ValidationNotification,
            'Validator': Validator,
            'Target': ValidationTarget,
            'CssAliasRegistry': CssValidationAliasRegistry
        },
        'Sources': sources
    });

} (jQuery, jQuery.fubuvalidation));