describe('ValidationNotificationTester', function() {
    var theNotification = null;

    beforeEach(function() {
       theNotification = new $.fubuvalidation.Notification();
    });

    it('the message collection is empty for an unknown field', function() {
       expect(theNotification.messagesFor('blah')).toEqual([]);
    });

    it('registers the message', function() {
        var theMessage = { field: 'Test', message: 'User-friendly message', element: '123' };
        theNotification.registerMessage('Test', 'User-friendly message', '123');

        expect(theNotification.messagesFor('Test')).toEqual([theMessage]);
    });

    it('gathers all messages', function() {
        var m1 = { field: 'Test 1', message: 'User-friendly message', element: '123' };
        var m2 = { field: 'Test 2', message: 'User-friendly message', element: '123' };

        theNotification.registerMessage(m1.field, m1.message, m1.element);
        theNotification.registerMessage(m2.field, m2.message, m2.element);

        expect(theNotification.allMessages()).toEqual([m1, m2]);
    });

    it('is valid', function() {
        expect(theNotification.isValid()).toEqual(true);
    });

    it('is valid (negative)', function() {
        theNotification.registerMessage('test', '', '');
        expect(theNotification.isValid()).toEqual(false);
    });
});

describe('Transforming ValidationNotification to an AjaxContinuation', function () {
    var theNotification = null;

    beforeEach(function () {
        theNotification = new $.fubuvalidation.Notification();
    });

    it('sets the success flag', function () {
        expect(theNotification.toContinuation().success).toEqual(true);

        theNotification.registerMessage($.fubuvalidation.ValidationKeys.Required);
        expect(theNotification.toContinuation().success).toEqual(false);
    });

    // TODO -- gotta check the messages
    
});

describe('ValidationTargetTester', function() {
    it('gets the value passed in', function() {
       var theValue = '123';
       var theTarget = new $.fubuvalidation.Target('field', theValue);

        expect(theTarget.value()).toEqual(theValue);
    });

    it('uses the name of the element', function() {
        var theElement = $('<input type="text" value="test-test-test" name="Tester" />');
        var theTarget = new $.fubuvalidation.Target.forElement(theElement);
        expect(theTarget.fieldName).toEqual('Tester');
    });

    it('uses the element for the value when specified', function() {
        var theElement = $('<input type="text" value="test-test-test" />');
        var theTarget = new $.fubuvalidation.Target.forElement(theElement);
        expect(theTarget.value()).toEqual('test-test-test');
    });
});

describe('CssValidationAliasRegistryTester', function () {
    var theRegistry = null;

    beforeEach(function () {
        theRegistry = new $.fubuvalidation.CssAliasRegistry();
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
    var theRegistry = null;
    var theEmailRule = null;
    var theRequiredRule = null;
    var theTarget = null;

    beforeEach(function () {
        theElement = $('<input type="text" name="Email" class="email required input-large" />');
        theEmailRule = 'email rule';
        theRequiredRule = 'required rule';

        theRegistry = new $.fubuvalidation.CssAliasRegistry();
        theRegistry.registerRule('email', theEmailRule);
        theRegistry.registerRule('required', theRequiredRule);

        theSource = new $.fubuvalidation.Sources.CssRules(theRegistry);

        theTarget = new $.fubuvalidation.Target.forElement(theElement);
    });

    it('parses the classes', function () {
        expect(theSource.classesFor(theElement)).toEqual(['email', 'required', 'input-large']);
    });

    it('finds the registered rules', function () {
        var theRules = theSource.rulesFor(theTarget);
        expect(theRules).toEqual([theEmailRule, theRequiredRule]);
    });
});

describe('ValidationProviderTest', function() {
    var theProvider = null;

    beforeEach(function() {
       theProvider = new $.fubuvalidation.Provider();
    });

    it('registers the validation source', function() {
       var theSource = function() { return '123';};
        theProvider.registerSource(theSource);

        expect(theProvider.sources).toEqual([theSource]);
    });

    it('aggregates the rules for a target', function() {
        var r1 = 'rule 1';
        var r2 = 'rule 2';

        var src1 = { rulesFor: function() { return [r1]; } };
        var src2 = { rulesFor: function() { return [r2]; } };

        theProvider.registerSource(src1);
        theProvider.registerSource(src2);

        expect(theProvider.rulesFor({})).toEqual([r1, r2]);
    })
});

describe('Integrated CssValidationRuleSource Tests', function () {
    var theRegistry = null;
    var theSource = null;
    var ruleFor = null;

    beforeEach(function () {
        theRegistry = new $.fubuvalidation.CssAliasRegistry();
        theSource = new $.fubuvalidation.Sources.CssRules(theRegistry);

        ruleFor = function (element, continuation) {
            var target = $.fubuvalidation.Target.forElement(element);
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
            var target = $.fubuvalidation.Target.forElement(element);
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
            var target = $.fubuvalidation.Target.forElement(element);
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
            var target = $.fubuvalidation.Target.forElement(element);
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
            var target = $.fubuvalidation.Target.forElement(element);
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
            var target = $.fubuvalidation.Target.forElement(element);
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