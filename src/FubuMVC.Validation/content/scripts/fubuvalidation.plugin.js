(function ($, validation, continuations) {

  // You can compose these however you like
  validation.Validator = $.fubuvalidation.Core.Validator.basic();
  validation.Processor = $.fubuvalidation.UI.ValidationProcessor.basic();
  validation.Controller = new $.fubuvalidation.UI.Controller(validation.Validator, validation.Processor);
  

  $.fn.validate = function (options) {
    return this.each(function () {
      var settings = {
        ajax: true,
        continuationSuccess: function (continuation) {
          // no -op
        }
      };

      validation.Controller.bindEvents($(this));

      settings = $.extend(true, settings, options);

      $(this).submit(function () {
        if (!validation.Controller.submitHandler(this)) {
          return false;
        }

        if (settings.ajax) {
          $(this).correlatedSubmit({
            continuationSuccess: settings.continuationSuccess
          });
          return false;
        }

        return true;
      });
    });
  };

  var _reset = $.fn.resetForm;
  $.fn.resetForm = function () {
    var continuation = new continuations.continuation();
    continuation.success = true;
    continuation.form = $(this);

    validation.Processor.reset(continuation);

    return _reset.call(this);
  };

  $.fn.activateValidation = function () {
    return this.each(function () {
      var container = $(this);
      $('form.validated-form', container).each(function () {
        var form = $(this);
        form.off('submit.fubu');

        var mode = form.data('formMode');
        $(this).validate({
          ajax: mode == 'ajax',
          continuationSuccess: function (continuation) {
            if (continuation.success && continuation.form) {
              continuation.form.trigger('validation:success', [continuation]);
            }
          }
        });
      });
    });
  };

  continuations.applyPolicy({
    matches: function (continuation) {
      return continuation.matchOnProperty('form', function (form) {
        return form.size() != 0;
      });
    },
    execute: function (continuation) {
      if (!continuation.errors) {
        continuation.errors = [];
      }

      validation.Processor.process(continuation);
    }
  });

}(jQuery, jQuery.fubuvalidation, jQuery.continuations));