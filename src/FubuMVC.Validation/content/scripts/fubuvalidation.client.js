(function ($, validation) {
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
        }
    };

    function ValidationTarget(fieldName, value) {
        this.fieldName = fieldName;
        this.rawValue = value;
    };

    ValidationTarget.prototype = {
        value: function () {
            if (typeof (this.element) != 'undefined') {
                return $(this.element).val();
            }
            return this.rawValue;
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
    };

    ValidationProvider.prototype = {
        registerSource: function (source) {
            this.sources.push(source);
        },
        rulesFor: function (target) {
            var rules = [];
            _.each(this.sources, function (src) {
                var targetRules = src.rulesFor(target);
                rules = rules.concat(targetRules);
            });

            return rules;
        }
    };

    function ValidationRuleRegistry() {
        this.rules = {};
    };

    ValidationRuleRegistry.prototype = {
        registerDefaults: function () {
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


    validation.Context = ValidationContext;
    validation.Notification = ValidationNotification;
    validation.Provider = ValidationProvider;
    validation.Target = ValidationTarget;
    validation.RuleRegistry = ValidationRuleRegistry;

    validation.Sources = {};
    validation.Sources.CssRules = CssValidationRuleSource;

} (jQuery, jQuery.fubuvalidation));