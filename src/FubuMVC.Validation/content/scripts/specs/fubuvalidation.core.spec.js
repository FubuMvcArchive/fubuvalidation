describe('ValidationNotificationTester', function () {
    var theNotification = null;

    beforeEach(function () {
        theNotification = new $.fubuvalidation.Core.Notification();
        $.fubuvalidation.localizer.clearCache();
    });

    it('the message collection is empty for an unknown field', function () {
        expect(theNotification.messagesFor('blah')).toEqual([]);
    });

    it('registers the message', function () {
        var theContext = { id: '234' };
        var theMessage = { field: 'Test', token: 'User-friendly message', element: '123', context: theContext };
        theNotification.registerMessage('Test', 'User-friendly message', '123', theContext);

        expect(theNotification.messagesFor('Test')).toEqual([theMessage]);
    });

    it('registers an empty context by default', function() {
        var theMessage = { field: 'Test', token: 'User-friendly message', element: '123', context: {} };
        theNotification.registerMessage('Test', 'User-friendly message', '123');

        expect(theNotification.messagesFor('Test')).toEqual([theMessage]);
    });

    it('gathers all messages', function () {
        var m1 = { field: 'Test 1', token: 'User-friendly message', element: '123', context: {} };
        var m2 = { field: 'Test 2', token: 'User-friendly message', element: '123', context: {} };

        theNotification.registerMessage(m1.field, m1.token, m1.element);
        theNotification.registerMessage(m2.field, m2.token, m2.element);

        expect(theNotification.allMessages()).toEqual([m1, m2]);
    });

    it('is valid', function () {
        expect(theNotification.isValid()).toEqual(true);
    });

    it('is valid (negative)', function () {
        theNotification.registerMessage('test', '', '');
        expect(theNotification.isValid()).toEqual(false);
    });
});

describe('Transforming a ValidationNotification to an AjaxContinuation', function () {
    var theNotification = null;

    beforeEach(function () {
        theNotification = new $.fubuvalidation.Core.Notification();
        $.fubuvalidation.localizer.clearCache();
    });

    it('sets the success flag', function () {
        expect(theNotification.toContinuation().success).toEqual(true);

        theNotification.registerMessage('Test', $.fubuvalidation.ValidationKeys.Required);
        expect(theNotification.toContinuation().success).toEqual(false);
    });

    it('sets the element on the error', function () {
        var theElement = $('<input type="text" name="Test" />');
        theNotification.registerMessage('field', $.fubuvalidation.ValidationKeys.Required, theElement);

        var theContinuation = theNotification.toContinuation();
        expect(theContinuation.errors[0].element).toEqual(theElement);
    });

    it('renders the message', function () {
        expect(new $.continuations.continuation().errors.length).toEqual(0);
        var token = new $.fubuvalidation.StringToken('Test', '{{Property}} is required');
        theNotification.registerMessage('Test', token, null, { Property: 'The Value' });

        var theContinuation = theNotification.toContinuation();
        var theError = theContinuation.errors[0];

        expect(theContinuation.errors.length).toEqual(1);

        expect(theError.field).toEqual('Test');
        expect(theError.label).toEqual('Test');
        expect(theError.message).toEqual('The Value is required');
    });

});

describe('ValidationTargetTester', function() {
    it('gets the value passed in', function() {
       var theValue = '123';
       var theTarget = new $.fubuvalidation.Core.Target('field', theValue);

        expect(theTarget.value()).toEqual(theValue);
    });

    it('uses the name of the element', function() {
        var theElement = $('<input type="text" value="test-test-test" name="Tester" />');
        var theTarget = new $.fubuvalidation.Core.Target.forElement(theElement);
        expect(theTarget.fieldName).toEqual('Tester');
    });

    it('uses the element for the value when specified', function() {
        var theElement = $('<input type="text" value="test-test-test" />');
        var theTarget = new $.fubuvalidation.Core.Target.forElement(theElement);
        expect(theTarget.value()).toEqual('test-test-test');
    });
});

describe('ValidationContextTester', function () {
    var theContext = null;
    var theTarget = null;
    var theRule = null;
    var theMessage = null;
    var theElement = null;

    beforeEach(function () {
        theElement = $('<input type="text" name="Test" value="Value" />');
        theTarget = $.fubuvalidation.Core.Target.forElement(theElement);
        theContext = new $.fubuvalidation.Core.Context(theTarget);
        theRule = $.fubuvalidation.Rules.Required;

        theContext.pushTemplateContext(theRule);
        theContext.registerMessage($.fubuvalidation.ValidationKeys.Required);

        var messages = theContext.notification.messagesFor('Test');
        expect(messages.length).toEqual(1);

        theMessage = messages[0];
    });

    it('uses the target element', function () {
        expect(theMessage.element).toEqual(theElement);
    });

    it('uses the template context', function () {
        expect(theMessage.context).toEqual(theRule);
    });
});

describe('CssValidationAliasRegistryTester', function () {
    var theRegistry = null;

    beforeEach(function () {
        theRegistry = new $.fubuvalidation.Core.CssAliasRegistry();
    });

    it('defaults to null for an unknown key', function () {
        expect(theRegistry.ruleFor('unknown')).toEqual(null);
    });

    it('registers the rule', function () {
        var theRule = { test: 'test' };
        theRegistry.registerRule('test', theRule);
        expect(theRegistry.ruleFor('test')).toEqual(theRule);
    });
});

describe('CssValidationRuleSourceTester', function () {
    var theSource = null;
    var theElement = null;

    beforeEach(function () {
        theElement = $('<input type="text" name="Email" class="email required input-large" />');
        theSource = $.fubuvalidation.Sources.CssRules;
    });

    it('parses the classes', function () {
        expect(theSource.classesFor(theElement)).toEqual(['email', 'required', 'input-large']);
    });
});

describe('ValidatorTests', function() {
    var theValidator = null;

    beforeEach(function() {
       theValidator = new $.fubuvalidation.Core.Validator();
    });

    it('registers the validation source', function() {
       var theSource = function() { return '123';};
        theValidator.registerSource(theSource);

        expect(theValidator.sources).toEqual([theSource]);
    });

    it('aggregates the rules for a target', function() {
        var r1 = 'rule 1';
        var r2 = 'rule 2';

        var src1 = { rulesFor: function() { return [r1]; } };
        var src2 = { rulesFor: function() { return [r2]; } };

        theValidator.registerSource(src1);
        theValidator.registerSource(src2);

        expect(theValidator.rulesFor({})).toEqual([r1, r2]);
    })
});

describe('when validating a target', function () {
    var theValidator = null;
    var r1 = null;
    var r2 = null;
    var theTarget = null;
    var theTemplateContexts = null;


    beforeEach(function () {
        theTemplateContexts = [];
        var setContext = function (context) {
            theTemplateContexts.push(context.templateContext);
        };

        r1 = { id: 'r1', validate: sinon.spy(setContext) };
        r2 = { id: 'r2', validate: sinon.spy(setContext) };

        var theSource = {
            rulesFor: function () {
                return [r1, r2];
            }
        };

        theTarget = $.fubuvalidation.Core.Target.forElement($('<input type="text" name="Test" />'));

        theValidator = new $.fubuvalidation.Core.Validator([theSource]);
        theValidator.validate(theTarget);
    });

    it('invokes each rule', function () {
        expect(r1.validate.called).toEqual(true);
        expect(r1.validate.getCall(0).args[0].target).toEqual(theTarget);

        expect(r2.validate.called).toEqual(true);
        expect(r2.validate.getCall(0).args[0].target).toEqual(theTarget);
    });

    it('pushes the template context for each rule', function () {
        expect(theTemplateContexts[0]).toEqual(r1);
        expect(theTemplateContexts[1]).toEqual(r2);
    });
});

describe('Integrated CssValidationRuleSource Tests', function () {
    var theSource = null;
    var ruleFor = null;

    beforeEach(function () {
        theSource = $.fubuvalidation.Sources.CssRules;

        ruleFor = function (element, continuation) {
            var target = $.fubuvalidation.Core.Target.forElement(element);
            var rules = theSource.rulesFor(target);

            expect(rules.length).toEqual(1);

            continuation(rules[0]);
        };
    });

    it('builds the required rule', function () {
        ruleFor($('<input type="text" name="Test" class="required" />'), function (rule) {
            expect(rule).toEqual($.fubuvalidation.Rules.Required);
        });
    });

    it('builds the email rule', function() {
        ruleFor($('<input type="text" name="Test" class="email" />'), function (rule) {
            expect(rule).toEqual($.fubuvalidation.Rules.Email);
        });
    });

    it('builds the date rule', function () {
        ruleFor($('<input type="text" name="Test" class="date" />'), function (rule) {
            expect(rule).toEqual($.fubuvalidation.Rules.Date);
        });
    });

    it('builds the number rule', function () {
        ruleFor($('<input type="text" name="Test" class="number" />'), function (rule) {
            expect(rule).toEqual($.fubuvalidation.Rules.Number);
        });
    });
});

describe('MinLengthSourceTester', function () {
    var theSource = null;
    var ruleFor = null;
    var rulesFor = null;

    beforeEach(function () {
        theSource = $.fubuvalidation.Sources.MinLength;

        rulesFor = function (element) {
            var target = $.fubuvalidation.Core.Target.forElement(element);
            return theSource.rulesFor(target);
        };

        ruleFor = function (element, continuation) {
            var rules = rulesFor(element, continuation);

            expect(rules.length).toEqual(1);

            continuation(rules[0]);
        };
    });

    it('builds the MinLength rule', function () {
        ruleFor($('<input type="text" name="Test" data-minlength="3" />'), function (rule) {
            expect(rule.length).toEqual(3);
        });
    });

    it('no rule if minlength data does not exist', function () {
        var rules = rulesFor($('<input type="text" name="Test" />'));
        expect(rules.length).toEqual(0);
    });
});

describe('MaxLengthSourceTester', function () {
    var theSource = null;
    var ruleFor = null;
    var rulesFor = null;

    beforeEach(function () {
        theSource = $.fubuvalidation.Sources.MaxLength;

        rulesFor = function (element) {
            var target = $.fubuvalidation.Core.Target.forElement(element);
            return theSource.rulesFor(target);
        };

        ruleFor = function (element, continuation) {
            var rules = rulesFor(element, continuation);

            expect(rules.length).toEqual(1);

            continuation(rules[0]);
        };
    });

    it('builds the MaxLength rule', function () {
        ruleFor($('<input type="text" name="Test" maxlength="5" />'), function (rule) {
            expect(rule.length).toEqual(5);
        });
    });

    it('no rule if maxlength does not exist', function () {
        var rules = rulesFor($('<input type="text" name="Test" />'));
        expect(rules.length).toEqual(0);
    });
});

describe('RangeLengthSourceTester', function () {
    var theSource = null;
    var ruleFor = null;
    var rulesFor = null;

    beforeEach(function () {
        theSource = $.fubuvalidation.Sources.RangeLength;

        rulesFor = function (element) {
            var target = $.fubuvalidation.Core.Target.forElement(element);
            return theSource.rulesFor(target);
        };

        ruleFor = function (element, continuation) {
            var rules = rulesFor(element, continuation);

            expect(rules.length).toEqual(1);

            continuation(rules[0]);
        };
    });

    it('builds the RangeLength rule', function () {
        ruleFor($("<input type=\"text\" name=\"Test\" data-rangelength='{\"min\":5, \"max\":10}' />"), function (rule) {
            expect(rule.min).toEqual(5);
            expect(rule.max).toEqual(10);
        });
    });

    it('no rule if rangelength data does not exist', function () {
        var rules = rulesFor($('<input type="text" name="Test" />'));
        expect(rules.length).toEqual(0);
    });
});

describe('MinSourceTester', function () {
    var theSource = null;
    var ruleFor = null;
    var rulesFor = null;

    beforeEach(function () {
        theSource = $.fubuvalidation.Sources.Min;

        rulesFor = function (element) {
            var target = $.fubuvalidation.Core.Target.forElement(element);
            return theSource.rulesFor(target);
        };

        ruleFor = function (element, continuation) {
            var rules = rulesFor(element, continuation);

            expect(rules.length).toEqual(1);

            continuation(rules[0]);
        };
    });

    it('builds the Min rule', function () {
        ruleFor($('<input type="text" name="Test" data-min="8" />'), function (rule) {
            expect(rule.bounds).toEqual(8);
        });
    });

    it('no rule if min data does not exist', function () {
        var rules = rulesFor($('<input type="text" name="Test" />'));
        expect(rules.length).toEqual(0);
    });
});

describe('MaxSourceTester', function () {
    var theSource = null;
    var ruleFor = null;
    var rulesFor = null;

    beforeEach(function () {
        theSource = $.fubuvalidation.Sources.Max;

        rulesFor = function (element) {
            var target = $.fubuvalidation.Core.Target.forElement(element);
            return theSource.rulesFor(target);
        };

        ruleFor = function (element, continuation) {
            var rules = rulesFor(element, continuation);

            expect(rules.length).toEqual(1);

            continuation(rules[0]);
        };
    });

    it('builds the Min rule', function () {
        ruleFor($('<input type="text" name="Test" data-max="12" />'), function (rule) {
            expect(rule.bounds).toEqual(12);
        });
    });

    it('no rule if max data does not exist', function () {
        var rules = rulesFor($('<input type="text" name="Test" />'));
        expect(rules.length).toEqual(0);
    });
});

describe('Integrated Validator Tests', function () {
    var theValidator = null;
    var notificationFor = null;

    beforeEach(function () {
        theValidator = $.fubuvalidation.Core.Validator.basic();

        notificationFor = function (element) {
            var target = $.fubuvalidation.Core.Target.forElement(element);
            return theValidator.validate(target);
        }
    });

    it('required with message', function () {
        var theNotification = notificationFor($('<input type="text" name="Test" class="required" />'));
        expect(theNotification.messagesFor('Test').length).toEqual(1);
    });
});

describe('Integrated Continuation Tests', function() {
    var theValidator = null;
    var continuationFor = null;

    beforeEach(function () {
        theValidator = $.fubuvalidation.Core.Validator.basic();

        continuationFor = function (element) {
            var target = $.fubuvalidation.Core.Target.forElement(element);
            return theValidator.validate(target).toContinuation();
        }
    });

    it('required with rendered message', function () {
        var theContinuation = continuationFor($('<input type="text" name="Test" class="required" />'));
        expect(theContinuation.errors[0].message).toEqual($.fubuvalidation.ValidationKeys.Required.toString());
    });
});