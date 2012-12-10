(function ($, validation) {
    var rules = {};

    var define = function (key, rule) {
        rules[key] = rule;
    };

    var defineLambda = function (key, rule) {
        define(key, { validate: rule });
    };

    function MinLengthRule(length) {
        this.length = length;
    }

    MinLengthRule.prototype = {
        validate: function (context) {
            if (!context.notification.isValid()) {
                return;
            }

            var value = context.target.value();
            if (value.length <= this.length) {
                context.registerMessage('');
            }
        }
    };

    defineLambda('Required', function (context) {
        var value = context.target.value();
        if ($.trim(value).length == 0) {
            context.registerMessage('');
        }
    });

    define('MinLength', MinLengthRule);

    $.extend(true, validation, { "Rules": rules });

} (jQuery, jQuery.fubuvalidation));