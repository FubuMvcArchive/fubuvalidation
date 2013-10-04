(function ($, StringToken) {
  var rules = {};
  var validationKeys = {};

  var define = function (key, rule) {
    rules[key] = rule;
  };

  var defineLambda = function (key, rule) {
    define(key, { name: key.toLowerCase(), validate: rule });
  };

  var defineToken = function (key, value) {
    validationKeys[key] = new StringToken(key, value);
  };

  defineToken('Required', 'This field is required.');
  defineToken('Email', 'Please enter a valid email address.');
  defineToken('Date', 'Please enter a valid date (e.g., 01/01/2012).');
  defineToken('Number', 'Please enter a valid number.');
  defineToken('MinimumLength', 'Please enter at least {{length}} characters.');
  defineToken('MaximumLength', 'Please enter no more than {{length}} characters.');
  defineToken('RangeLength', 'Please enter a value between {{min}} and {{max}} characters.');
  defineToken('MinValue', 'Please enter a value less than or equal to {{bounds}}.');
  defineToken('MaxValue', 'Please enter a value greater than or equal to {{bounds}}.');
  defineToken('RegularExpression', 'The data is in an invalid format.');
  defineToken('FieldEquality', '{{field1}} must equal {{field2}}');


  function MinLengthRule(length) {
    this.name = 'minlength';
    this.length = length;
  }

  MinLengthRule.prototype = {
    validate: function (context) {
      var value = context.target.value();
      if (value.length < this.length) {
        context.registerMessage(validationKeys.MinimumLength);
      }
    }
  };

  function MaxLengthRule(length) {
    this.name = 'maxlength';
    this.length = length;
  }

  MaxLengthRule.prototype = {
    validate: function (context) {
      var value = context.target.value();
      if (value.length > this.length) {
        context.registerMessage(validationKeys.MaximumLength);
      }
    }
  };

  function RangeLengthRule(min, max) {
    this.name = 'rangelength';
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
    this.name = 'min';
    this.bounds = bounds;
  }

  MinRule.prototype = {
    validate: function (context) {
      var value = context.target.value();
      if (value < this.bounds) {
        context.registerMessage(validationKeys.MinValue);
      }
    }
  };

  function MaxRule(bounds) {
    this.name = 'max';
    this.bounds = bounds;
  }

  MaxRule.prototype = {
    validate: function (context) {
      var value = context.target.value();
      if (value > this.bounds) {
        context.registerMessage(validationKeys.MaxValue);
      }
    }
  };

  function RemoteRule(url, hash) {
    this.name = 'remote';
    this.url = url;
    this.hash = hash;
    this.async = true;
  }

  RemoteRule.prototype = {
    validate: function (context) {
      var target = context.target;
      var hash = this.hash;

      return $.ajax({
        url: this.url,
        data: { Hash: hash, Value: target.value() },
        async: false,
        acceptType: 'application/json',
        success: function (continuation) {
          if (!continuation.errors) {
            continuation.errors = [];
          }

          if (continuation.errors.length == 0) return;

          var token = new StringToken(hash, continuation.errors[0].message);
          context.registerMessage(token);
        }
      });
    }
  };

  function RegularExpressionRule(value) {
    this.name = 'regularexpression';
    this.expression = new RegExp(value);
  }

  RegularExpressionRule.prototype = {
    validate: function (context) {
      var value = context.target.value();
      if (value == '' || this.expression.test(value)) return;

      context.registerMessage(validationKeys.RegularExpression);
    }
  };
  
  function FieldEqualityRule(options) {
    this.name = 'fieldequality';
    this.options = options;

    // Makes the message substitutions flow a lot easier
    this.field1 = options.property1.field;
    this.field2 = options.property2.field;
  }

  FieldEqualityRule.prototype = {
    validate: function(context) {
      var targetField;
      if (context.target.fieldName == this.field1) {
        targetField = this.field2;
      }
      else {
        targetField = this.field1;
      }

      var element2 = context.target.form.find('input[name="' + targetField + '"]');

      var value1 = context.target.value();
      var value2 = element2.val();

      if (value1 == '' && value2 == '') return;
      if (value1 == value2) {
        element2.trigger('validation:bustCache');
        return; 
      }

      if (this.options.message) {
        context.target.useLocalizationMessages({ 'fieldequality': this.options.message });
      }

      _.each(this.options.targets, function(field) {
        var element = (field == context.target.fieldName) ? context.target.element : element2;
        context.registerMessageForElement(validationKeys.FieldEquality, field, element);
      });
      
    }
  };

  defineLambda('Required', function (context) {
    var value = context.target.value();
    if (value.length == 0) {
      context.registerMessage(validationKeys.Required);
    }
  });

  var emailExpression = /^((([a-zA-Z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-zA-Z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-zA-Z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-zA-Z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-zA-Z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-zA-Z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-zA-Z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-zA-Z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-zA-Z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-zA-Z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))$/
  defineLambda('Email', function (context) {
    var value = context.target.value();
    if (value.length == 0) return;

    if (!emailExpression.test(value)) {
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
  define('RegularExpression', RegularExpressionRule);
  define('FieldEquality', FieldEqualityRule);

  $.extend(true, $, {
    'fubuvalidation': {
      "Rules": rules,
      'ValidationKeys': validationKeys
    }
  });

}(jQuery, jQuery.fubuvalidation.StringToken));