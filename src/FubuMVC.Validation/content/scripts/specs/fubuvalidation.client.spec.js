describe('ValidationNoficationTester', function() {
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

describe('ValiationRuleRegistry', function () {
    var theRegistry = null;

    beforeEach(function () {
        theRegistry = new $.fubuvalidation.RuleRegistry();
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

        theRegistry = new $.fubuvalidation.RuleRegistry();
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