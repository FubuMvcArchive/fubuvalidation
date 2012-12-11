(function () {
    function StringToken(key, defaultValue) {
        this.key = key;
        this.defaultValue = defaultValue;
    }

    StringToken.prototype.toString = function () {
        return $.fubuvalidation.localizer.valueFor(this);
    };

    function LocalizationManager() {
        this.cache = {};
    }

    LocalizationManager.prototype = {
        // TODO -- This obviously gets smarter later
        valueFor: function (token) {
            var value = this.cache[token.key];
            if (typeof (value) == 'undefined') {
                value = token.defaultValue;
                this.cache[token.key] = value;
            }

            return value;
        },
        clearCache: function () {
            this.cache = {};
        }
    };

    $.extend(true, $, {
        'fubuvalidation': {
            'StringToken': StringToken,
            'LocalizationManager': LocalizationManager,

            // singleton scope
            'localizer': new LocalizationManager()
        }
    })
} (jQuery.fubuvalidation));