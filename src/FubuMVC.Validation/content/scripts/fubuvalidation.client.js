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

        registerMessage: function (field, message, element) {
            var messages = this.messagesFor(field);
            messages.push({ field: field, message: message, element: element });
        },

        allMessages: function () {
            messages = [];

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

    function ValidationContext(target) {
        this.target = target;
        this.notification = new ValidationNotification();
    };

    ValidationContext.prototype = {
        registerMessage: function (message) {
            this.notification.registerMessage(this.target.fieldName, message, this.target.element);
        }
    };

    function ValidationProvider(sources) {
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

    ValidationProvider.prototype = {
        registerSource: function (source) {
            this.sources.push(source);
        }
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

    function rulesForData(target, data, continuation) {
        var rules = [];

        var value = target.element.data(data);
        if (typeof (value) != 'undefined') {
            rules.push(continuation(value));
        }

        return rules;
    };

    defineSource('CssRules', CssValidationRuleSource);
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


    validation.Context = ValidationContext;
    validation.Notification = ValidationNotification;
    validation.Provider = ValidationProvider;
    validation.Target = ValidationTarget;
    validation.CssAliasRegistry = CssValidationAliasRegistry;
    validation.Sources = sources;

} (jQuery, jQuery.fubuvalidation));

(function ($) {
    $.fn.validate = function() {
        
    };
}(jQuery));