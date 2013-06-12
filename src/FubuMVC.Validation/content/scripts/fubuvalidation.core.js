(function ($, validation) {
  var sources = {};
  var defineSource = function (key, source) {
    sources[key] = source;
  };

  function ValidationMessage(field, token, element, context) {
    this.field = field;
    this.token = token;
    this.element = element;
    this.context = context || {};
  }

  ValidationMessage.prototype = {
    toHash: function() {
      return this.field + ':' + this.token.toString();
    },
    toString: function() {
      var message = this.token.toString();
      var context = this.context;

      if (this.element) {
        message = message.replace('{field}', this.element.attr('name'));
      }

      for (var key in context) {
        message = message.replace('{' + key + '}', context[key]);
      }

      return message;
    }
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
      var message = new ValidationMessage(field, token, element, context);
      var existing = null;
      this.eachMessage(function(msg) {
        if (message.toHash() == msg.toHash()) {
          existing = msg;
        }
      });

      if (existing != null) return existing;

      var messages = this.messagesFor(field);
      messages.push(message);
      
      return message;
    },

    allMessages: function () {
      var messages = [];

      this.eachMessage(function(msg) { messages.push(msg); });

      return messages;
    },

    eachMessage: function(action) {
      for (var key in this.messages) {
        var values = this.messages[key];
        _.each(values, function (value) {
          action(value);
        });
      }
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
        var label = message.element.data('localized-label');
        if (typeof (label) == 'undefined') {
          label = message.field;
        }

        continuation.errors.push({
          field: message.field,
          label: label,
          message: message.toString(),
          element: message.element,
          source: typeof(message.context)
        });
      }

      return continuation;
    },

    importForTarget: function (notification, target) {
      this.messages[target.fieldName] = notification.messagesFor(target.fieldName);
    }
  };

  function ValidationTarget(fieldName, value, correlationId) {
    this.fieldName = fieldName;
    this.rawValue = value;
    this.correlationId = correlationId;
    this.messages = null;
  };

  ValidationTarget.prototype = {
    useLocalizationMessages: function(messages) {
      this.messages = messages;
    },
    localizedMessageFor: function (key) {

      var messages = this.messages;
      if (messages == null) {
        var data = this.element.data('localization');
        if (typeof (data) == 'undefined') return null;

        messages = data.Messages;
      }

      var message = messages[key];
      if (typeof (message) == 'undefined') return null;

      return message;
    },
    value: function () {
      var value = this.rawValue;
      if (typeof (this.element) != 'undefined') {
        value = $(this.element).val();
      }

      return $.trim(value);
    },
    toHash: function () {
      return 'correlationId:' + this.correlationId + '&fieldName=' + this.fieldName;
    }
  };

  ValidationTarget.forElement = function (element, correlationId, form) {
    var target = new ValidationTarget(element.attr('name'), null, correlationId);
    target.element = element;
    target.form = form;

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
    registerMessage: function (token) {
      return this.registerMessageForElement(token, this.target.fieldName, this.target.element);
    },
    registerMessageForElement: function (token, fieldName, element) {
      var message = token;
      if (typeof (token.key) != 'undefined') {
        var key = token.key.toLowerCase();
        var localizedMsg = this.target.localizedMessageFor(key);
        if (localizedMsg) {
          message = localizedMsg;
        }
      }

      return this.notification.registerMessage(fieldName, message, element, this.templateContext);
    }
  };

  function Validator(sources) {
    this.sources = sources || [];

    var self = this;
    var hash = function (target) { return target.toHash(); };
    this.rulesFor = _.memoize(function (target) {
      var rules = [];
      _.each(self.sources, function (src) {
        var targetRules = src.rulesFor(target);
        rules = rules.concat(targetRules);
      });

      return rules;
    }, hash);
  };

  Validator.prototype = {
    registerSource: function (source) {
      this.sources.push(source);
    },
    validate: function (target, notification) {

      notification = notification || new ValidationNotification();
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
  defineSource('RegularExpression', {
    rulesFor: function (target) {
      return rulesForData(target, 'regex', function (value) {
        return new validation.Rules.RegularExpression(value);
      });
    }
  });
  defineSource('FieldEquality', {
    rulesFor: function (target) {
      if (!target.form) return [];
      
      var data = target.form.data('fieldEquality');
      var rules = [];

      if (typeof(data) != 'undefined') {
        _.each(data.rules, function (ruleDef) {
          if (ruleDef.property1.field == target.fieldName || ruleDef.property2.field == target.fieldName) {
            rules.push(new validation.Rules.FieldEquality(ruleDef));
          }
        });
      }

      return rules;
    }
  });
  defineSource('Remote', {
    rulesFor: function (target) {
      var value = target.element.data('remoteRule');
      if (typeof (value) == 'undefined') {
        return [];
      }

      var rules = [];

      _.each(value.rules, function (hash) {
        rules.push(new validation.Rules.Remote(value.url, hash));
      });

      return rules;
    }
  });

  $.extend(true, validation, {
    'Core': {
      'Context': ValidationContext,
      'Message': ValidationMessage,
      'Notification': ValidationNotification,
      'Validator': Validator,
      'Target': ValidationTarget,
      'CssAliasRegistry': CssValidationAliasRegistry
    },
    'Sources': sources
  });

}(jQuery, jQuery.fubuvalidation));