(function (validation) {
    function StringToken(key, defaultValue) {
        this.key = key;
        this.defaultValue = defaultValue;
    }

    StringToken.prototype.toString = function () {
        return validation.localizer.valueFor(this);
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

    validation.StringToken = StringToken;
    validation.LocalizationManager = LocalizationManager;

    // singleton scope
    validation.localizer = new LocalizationManager();

} (jQuery.fubuvalidation));