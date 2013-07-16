function RuleHarness(rule) {
  this.rule = rule;
  this.element = $('<input type="text" name="Test" />');
  this.element2 = $('<input type="text" name="Test2" />');
  this.form = $('<form id="test"></form>');
  this.target = $.fubuvalidation.Core.Target.forElement(this.element, 'test', this.form);
  this.context = new $.fubuvalidation.Core.Context(this.target);

  this.form.append(this.element);
  this.form.append(this.element2);
}

RuleHarness.prototype = {
  messagesFor: function (value, element) {
    if (!element) {
      element = this.element;
    }
    
    element.val(value);

    return this.messagesForElement(element);
  },
  
  messagesForElement: function(element) {
    this.context.notification = new $.fubuvalidation.Core.Notification();
    this.rule.validate(this.context);

    return this.context.notification.messagesFor(element.attr('name'));
  }
};

describe('RequiredRuleTester', function () {
  var theHarness = null;

  beforeEach(function () {
    theHarness = new RuleHarness($.fubuvalidation.Rules.Required);
  });

  it('defines the name', function() {
    expect(theHarness.rule.name).toEqual('required');
  });

  it('registers a message when the value is empty', function () {
    var messages = theHarness.messagesFor('');
    expect(messages.length).toEqual(1);
  });

  it('registers a message when the value is nothing but whitespace', function () {
    var messages = theHarness.messagesFor('   ');
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
  
  it('defines the name', function () {
    expect(theHarness.rule.name).toEqual('minlength');
  });

  it('registers a message when the string is too short', function () {
    expect(theHarness.messagesFor('123').length).toEqual(1);
  });

  it('no message when the string is at the limit', function () {
    expect(theHarness.messagesFor('12345').length).toEqual(0);
  });

  it('no message when the string is within the limit', function () {
    expect(theHarness.messagesFor('123456').length).toEqual(0);
  });
});

describe('MaxLengthRuleTester', function () {
  var theHarness = null;

  beforeEach(function () {
    theHarness = new RuleHarness(new $.fubuvalidation.Rules.MaxLength(3));
  });
  
  it('defines the name', function () {
    expect(theHarness.rule.name).toEqual('maxlength');
  });

  it('registers a message when the string is too long', function () {
    expect(theHarness.messagesFor('1234').length).toEqual(1);
  });

  it('no message when the string is at the limit', function () {
    expect(theHarness.messagesFor('123').length).toEqual(0);
  });

  it('no message when the string is within the limit', function () {
    expect(theHarness.messagesFor('12').length).toEqual(0);
  });
});

describe('RangeLengthRuleTester', function () {
  var theHarness = null;

  beforeEach(function () {
    theHarness = new RuleHarness(new $.fubuvalidation.Rules.RangeLength(3, 5));
  });
  
  it('defines the name', function () {
    expect(theHarness.rule.name).toEqual('rangelength');
  });

  it('registers a message when the string is too long', function () {
    expect(theHarness.messagesFor('123456').length).toEqual(1);
  });

  it('no message when the string is at the upper bounds', function () {
    expect(theHarness.messagesFor('12345').length).toEqual(0);
  });

  it('registers a message when the string is too short', function () {
    expect(theHarness.messagesFor('12').length).toEqual(1);
  });

  it('no message when the string is at the lower bounds', function () {
    expect(theHarness.messagesFor('123').length).toEqual(0);
  });
});

describe('MinRuleTester', function () {
  var theHarness = null;

  beforeEach(function () {
    theHarness = new RuleHarness(new $.fubuvalidation.Rules.Min(5));
  });
  
  it('defines the name', function () {
    expect(theHarness.rule.name).toEqual('min');
  });

  it('registers a message when the value is too small', function () {
    expect(theHarness.messagesFor('1').length).toEqual(1);
  });

  it('no message when the value is equal', function () {
    expect(theHarness.messagesFor('5').length).toEqual(0);
  });

  it('no message when the value is greater', function () {
    expect(theHarness.messagesFor('100').length).toEqual(0);
  });
});

describe('MaxRuleTester', function () {
  var theHarness = null;

  beforeEach(function () {
    theHarness = new RuleHarness(new $.fubuvalidation.Rules.Max(10));
  });
  
  it('defines the name', function () {
    expect(theHarness.rule.name).toEqual('max');
  });

  it('registers a message when the value is too large', function () {
    expect(theHarness.messagesFor('11').length).toEqual(1);
  });

  it('no message when the value is equal', function () {
    expect(theHarness.messagesFor('10').length).toEqual(0);
  });

  it('no message when the value is smaller', function () {
    expect(theHarness.messagesFor('1').length).toEqual(0);
  });
});

describe('EmailRuleTester', function () {
  var theHarness = null;

  beforeEach(function () {
    theHarness = new RuleHarness($.fubuvalidation.Rules.Email);
  });

  it('defines the name', function () {
    expect(theHarness.rule.name).toEqual('email');
  });

  it('registers a message for invalid emails', function () {
    expect(theHarness.messagesFor('somebody@').length).toEqual(1);
    expect(theHarness.messagesFor('almost@there').length).toEqual(1);
  });

  it('no message when the email is valid', function () {
    expect(theHarness.messagesFor('user@domain.com').length).toEqual(0);
    expect(theHarness.messagesFor('user@sub.domain.com').length).toEqual(0);
    expect(theHarness.messagesFor('first.last@sub.domain.com').length).toEqual(0);
    expect(theHarness.messagesFor('gmail+style@sub.domain.com').length).toEqual(0);
    expect(theHarness.messagesFor('Uppercase@sub.domain.com').length).toEqual(0);
  });

  it('no message for an empty email', function () {
    var x = theHarness.messagesFor('');
    console.log(x);
    expect(theHarness.messagesFor('').length).toEqual(0);
  });
});

describe('DateRuleTester', function () {
  var theHarness = null;

  beforeEach(function () {
    theHarness = new RuleHarness($.fubuvalidation.Rules.Date);
  });
  
  it('defines the name', function () {
    expect(theHarness.rule.name).toEqual('date');
  });

  it('registers a message for invalid dates', function () {
    expect(theHarness.messagesFor('asdf').length).toEqual(1);
    expect(theHarness.messagesFor('01asdf').length).toEqual(1);
  });

  it('no message when the date is valid', function () {
    expect(theHarness.messagesFor('08/30/2011').length).toEqual(0);
  });
});

describe('NumberRuleTester', function () {
  var theHarness = null;

  beforeEach(function () {
    theHarness = new RuleHarness($.fubuvalidation.Rules.Number);
  });

  it('registers a message for invalid numbers', function () {
    expect(theHarness.messagesFor('asdf').length).toEqual(1);
  });

  it('no message when the number is valid', function () {
    expect(theHarness.messagesFor('100').length).toEqual(0);
  });
});

describe('RegularExpressionRuleTester', function () {
  var theHarness = null;

  beforeEach(function () {
    theHarness = new RuleHarness(new $.fubuvalidation.Rules.RegularExpression('[a-zA-Z0-9]+$'));
  });
  
  it('defines the name', function () {
    expect(theHarness.rule.name).toEqual('regularexpression');
  });

  it('registers a message when string does not match', function () {
    expect(theHarness.messagesFor('hello//').length).toEqual(1);
  });

  it('no message when the string is a match', function () {
    expect(theHarness.messagesFor('hello').length).toEqual(0);
  });
});

describe('FieldEqualityRuleTester', function () {
  var theHarness = null;
  var theRule = null;

  beforeEach(function () {
    theRule = new $.fubuvalidation.Rules.FieldEquality({
      property1: { field: 'Test', label: 'Test' },
      property2: { field: 'Test2', label: 'Test2' },
      targets: []
    });
    
    theHarness = new RuleHarness(theRule);
  });
  
  it('defines the name', function () {
    expect(theHarness.rule.name).toEqual('fieldequality');
  });

  it('sets the fields for substitutions', function() {
    expect(theRule.field1).toEqual('Test');
    expect(theRule.field2).toEqual('Test2');
  });

  it('registers a message on property1 when values do not match and property1 is targeted', function () {
    theRule.options.targets.push('Test');
    var messages = theHarness.messagesFor('hello//', theHarness.element);
    expect(messages.length).toEqual(1);
  });
  
  it('uses the explicit localization message when specified', function () {
    theRule.options.targets.push('Test');
    theRule.options.message = 'Testing';
    theHarness.messagesFor('hello//', theHarness.element);

    expect(theHarness.target.messages.fieldequality).toEqual('Testing');
  });

  it('does not set the explicit localiation message when not specified', function() {
    theRule.options.targets.push('Test');
    theHarness.messagesFor('hello//', theHarness.element);

    expect(theHarness.target.messages).toEqual(null);
  });

  it('does not register a message when the values match', function () {
    theRule.options.targets.push('Test');
    theRule.options.targets.push('Test2');

    theHarness.element.val('Hello');
    theHarness.element2.val('Hello');

    expect(theHarness.messagesForElement(theHarness.element).length).toEqual(0);
    expect(theHarness.messagesForElement(theHarness.element2).length).toEqual(0);
  });

  it('does not register a message when the values are blank', function() {
    theRule.options.targets.push('Test');
    theRule.options.targets.push('Test2');

    theHarness.element.val('');
    theHarness.element2.val('');

    expect(theHarness.messagesForElement(theHarness.element).length).toEqual(0);
    expect(theHarness.messagesForElement(theHarness.element2).length).toEqual(0);
  });
});