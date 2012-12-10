function RuleHarness(rule) {
    this.rule = rule;
    this.element = $('<input type="text" name="Test" />');
    this.target = $.fubuvalidation.Target.forElement(this.element);
    this.context = new $.fubuvalidation.Context(this.target);
}

RuleHarness.prototype = {
    messagesFor: function (value) {
        this.element.val(value);
        this.rule.validate(this.context);

        return this.context.notification.messagesFor(this.element.attr('name'));
    }
};

describe('RequiredRuleTester', function () {
    var theHarness = null;

    beforeEach(function () {
        theHarness = new RuleHarness($.fubuvalidation.Rules.Required);
    });

    it('registers a message when the value is empty', function () {
        var messages = theHarness.messagesFor('');
        expect(messages.length).toEqual(1);
    });

    it('registers a message when the value is null', function () {
        var messages = theHarness.messagesFor(null);
        expect(messages.length).toEqual(1);
    });

    it('does not register a message when the value is not empty', function () {
        var messages = theHarness.messagesFor('123');
        expect(messages.length).toEqual(0);
    });
});

describe('MinLengthRuleTester', function () {
    var theHarness = null;

    beforeEach(function () {
        theHarness = new RuleHarness(new $.fubuvalidation.Rules.MinLength(5));
    });

    it('registers a message when the string is too short', function () {
        expect(theHarness.messagesFor('123').length).toEqual(1);
    });

    it('registers a message when the string is the limit', function () {
        expect(theHarness.messagesFor('12345').length).toEqual(1);
    });

    it('no message when the string is within the limit', function () {
        expect(theHarness.messagesFor('123456').length).toEqual(0);
    });

    it('no message the notification is not empty', function () {
        theHarness.context.registerMessage('something else');
        expect(theHarness.messagesFor('123').length).toEqual(1);
    });
});