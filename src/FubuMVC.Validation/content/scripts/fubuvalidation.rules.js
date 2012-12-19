(function ($, StringToken) {
    var rules = {};
    var validationKeys = {};

    var define = function (key, rule) {
        rules[key] = rule;
    };

    var defineLambda = function (key, rule) {
        define(key, { validate: rule });
    };

    var defineToken = function (key, value) {
        validationKeys[key] = new StringToken(key, value);
    };

    defineToken('Required', 'This field is required.');
    defineToken('Email', 'Please enter a valid email address.');
    defineToken('Date', 'Please enter a valid date (e.g., 01/01/2012).');
    defineToken('Number', 'Please enter a valid number.');
    defineToken('MinLength', 'Please enter at least {{length}} characters.');
    defineToken('MaxLength', 'Please enter no more than {{length}} characters.');
    defineToken('RangeLenth', 'Please enter a value between {{min}} and {{max}} characters.');
    defineToken('Min', 'Please enter a value less than or equal to {{bounds}}.');
    defineToken('Max', 'Please enter a value greater than or equal to {{bounds}}.');


    function MinLengthRule(length) {
        this.length = length;
    }

    MinLengthRule.prototype = {
        validate: function (context) {
            var value = context.target.value();
            if (value.length < this.length) {
                context.registerMessage(validationKeys.MinLength);
            }
        }
    };

    function MaxLengthRule(length) {
        this.length = length;
    }

    MaxLengthRule.prototype = {
        validate: function (context) {
            var value = context.target.value();
            if (value.length > this.length) {
                context.registerMessage(validationKeys.MaxLength);
            }
        }
    };

    function RangeLengthRule(min, max) {
        this.min = min;
        this.max = max;
    }

    RangeLengthRule.prototype = {
        validate: function (context) {
            var value = context.target.value();
            var length = value.length;

            if (this.min > length || this.max < length) {
                context.registerMessage(validationKeys.RangeLength);
            }
        }
    };

    function MinRule(bounds) {
        this.bounds = bounds;
    }

    MinRule.prototype = {
        validate: function (context) {
            var value = context.target.value();
            if (value < this.bounds) {
                context.registerMessage(validationKeys.Min);
            }
        }
    };

    function MaxRule(bounds) {
        this.bounds = bounds;
    }

    MaxRule.prototype = {
        validate: function (context) {
            var value = context.target.value();
            if (value > this.bounds) {
                context.registerMessage(validationKeys.Max);
            }
        }
    };

    function RemoteRule(url, hash) {
        this.url = url;
        this.hash = hash;
    }

    RemoteRule.prototype = {
        validate: function (context) {
            var target = context.target;
            var hash = this.hash;

            $.ajax({
                url: this.url,
                data: { Hash: hash, Value: target.value() },
                asynx: false,
                acceptType: 'application/json',
                success: function (continuation) {
                    if (!continuation.errors) {
                        continuation.errors = [];
                    }

                    if (continuation.errors.length == 0) return;
                    // TODO -- FINISH THIS
                    var token = new StringToken(hash, continuation.errors[0].message);
                    context.registerMessage(token);
                }
            });
        }
    };

    defineLambda('Required', function (context) {
        var value = context.target.value();
        if (value.length == 0) {
            context.registerMessage(validationKeys.Required);
        }
    });

    var emailExpression = /^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))$/
    defineLambda('Email', function (context) {
        if (!emailExpression.test(context.target.value())) {
            context.registerMessage(validationKeys.Email);
        }
    });

    defineLambda('Date', function (context) {
        var value = context.target.value();
        var date = new Date(value);

        if (/Invalid|NaN/.test(date.toString())) {
            context.registerMessage(validationKeys.Date);
        }
    });

    defineLambda('Number', function (context) {
        var value = context.target.value();
        var valid = /^\d+$/.test(value);
        if (!valid) {
            context.registerMessage(validationKeys.Number);
        }
    });

    define('MinLength', MinLengthRule);
    define('MaxLength', MaxLengthRule);
    define('RangeLength', RangeLengthRule);
    define('Min', MinRule);
    define('Max', MaxRule);
    define('Remote', RemoteRule);

    $.extend(true, $, {
        'fubuvalidation': {
            "Rules": rules,
            'ValidationKeys': validationKeys
        }
    });

} (jQuery, jQuery.fubuvalidation.StringToken));