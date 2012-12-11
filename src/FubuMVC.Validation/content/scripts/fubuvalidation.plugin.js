(function ($, validation) {
    // You can compose this however you like
    validation.Validator = $.fubuvalidation.Core.Validator.basic();
    
    $.fn.validate = function () {
    };

} (jQuery, validation));