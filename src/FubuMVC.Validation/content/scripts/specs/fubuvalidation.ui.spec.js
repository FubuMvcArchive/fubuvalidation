describe('RenderingContext Helpers', function () {
    var theContext = null;
    var theContinuation = null;

    beforeEach(function () {
        theContinuation = $fubu.continuations.create();
        theContinuation.success = true;
        theContext = new $.fubuvalidation.UI.RenderingContext(theContinuation, null, null);
    });

    it('isValid', function () {
        expect(theContext.isValid()).toEqual(true);
    });

    it('isValid (negative from success flag)', function () {
        theContinuation.success = false;
        expect(theContext.isValid()).toEqual(false);
    });

    it('isValid (negative from errors)', function () {
        theContinuation.errors.push({});
        expect(theContext.isValid()).toEqual(false);
    });

    it('isLive', function () {
        theContext.mode = $.fubuvalidation.Core.ValidationMode.Live;
        expect(theContext.isLive()).toEqual(true);
    });

    it('isLive (negative)', function () {
        theContext.mode = $.fubuvalidation.Core.ValidationMode.Triggered;
        expect(theContext.isLive()).toEqual(false);
    });

    it('isTriggered', function () {
        theContext.mode = $.fubuvalidation.Core.ValidationMode.Triggered;
        expect(theContext.isTriggered()).toEqual(true);
    });

    it('isTriggered (negative)', function () {
        theContext.mode = $.fubuvalidation.Core.ValidationMode.Live;
        expect(theContext.isTriggered()).toEqual(false);
    });

    it('isServerGenerated', function () {
        theContinuation.validationOrigin = 'server';
        expect(theContext.isServerGenerated()).toEqual(true);
    });

    it('isServerGenerated (negative)', function () {
        expect(theContext.isServerGenerated()).toEqual(false);
    });

    it('isEntireForm', function () {
        expect(theContext.isEntireForm()).toEqual(true);
    });

    it('isEntireForm (negative)', function () {
        theContext.element = $('<input type="text" />');
        expect(theContext.isEntireForm()).toEqual(false);
    });

});


describe('Enumerating errors for full form validation', function () {
    var theContext = null;
    var theContinuation = null;
    var e1 = null;
    var e2 = null;

    beforeEach(function () {
        e1 = '1';
        e2 = '2';

        theContinuation = $fubu.continuations.create();
        theContinuation.errors.push(e1);
        theContinuation.errors.push(e2);

        theContext = new $.fubuvalidation.UI.RenderingContext(theContinuation, null, 'triggered');
    });

    it('enumerates all errors', function () {
        var errors = [];
        theContext.eachError(function (error) {
            errors.push(error);
        });

        expect(errors).toEqual([e1, e2]);
    });
});

describe('Enumerating errors for an individual element', function () {
    var theContext = null;
    var theContinuation = null;
    var theElement = null;
    var e1 = null;
    var e2 = null;
    var e3 = null;
    var e4 = null;

    beforeEach(function () {
        theElement = $('<input type="text" id="Test" />');

        e1 = { element: theElement, id: '1' };
        e2 = { id: '2' };
        e3 = { element: theElement, id: '3' };
        e4 = { id: '4' };

        theContinuation = $fubu.continuations.create();
        theContinuation.errors.push(e1);
        theContinuation.errors.push(e2);
        theContinuation.errors.push(e3);
        theContinuation.errors.push(e4);

        theContext = new $.fubuvalidation.UI.RenderingContext(theContinuation, theElement, 'triggered');
    });

    it('enumerates element errors', function () {
        var errors = [];
        theContext.eachError(function (error) {
            errors.push(error);
        });

        expect(errors).toEqual([e1, e3]);
    });
});

describe('Default Validation Handler Tester', function () {
    var theHandler = null;

    beforeEach(function () {
        theHandler = new $.fubuvalidation.UI.DefaultHandler();
    });

    it('registers the rendering strategy', function () {
        var theStrategy = '123546';
        theHandler.registerStrategy(theStrategy);

        expect(theHandler.strategies).toEqual([theStrategy]);
    });

    it('process resets all of the rendering strategies', function () {
        theHandler.reset = sinon.spy();
        theHandler.process({});
        expect(theHandler.reset.called).toEqual(true);
    });

    it('process renders the matching strategies', function () {
        var s1 = {
            matches: function () { return true; },
            render: sinon.spy()
        };

        var s2 = {
            matches: function () { return false; },
            render: sinon.spy()
        };

        var s3 = {
            matches: function () { return true; },
            render: sinon.spy()
        };

        theHandler.reset = sinon.spy();

        theHandler.registerStrategy(s1);
        theHandler.registerStrategy(s2);
        theHandler.registerStrategy(s3);

        theHandler.process({});

        expect(s1.render.called).toEqual(true);
        expect(s2.render.called).toEqual(false);
        expect(s3.render.called).toEqual(true);
    });

    it('reset resets the matching strategies', function () {
        var s1 = {
            matches: function () { return true; },
            reset: sinon.spy()
        };

        var s2 = {
            matches: function () { return false; },
            reset: sinon.spy()
        };

        var s3 = {
            matches: function () { return true; },
            reset: sinon.spy()
        };

        theHandler.registerStrategy(s1);
        theHandler.registerStrategy(s2);
        theHandler.registerStrategy(s3);

        theHandler.reset({});

        expect(s1.reset.called).toEqual(true);
        expect(s2.reset.called).toEqual(false);
        expect(s3.reset.called).toEqual(true);
    });
});

describe('ValidationProcessor Tests', function () {
    var theHandler = null;
    var theProcessor = null;

    beforeEach(function () {
        theHandler = {
            process: sinon.spy(),
            reset: sinon.spy()
        };

        theProcessor = new $.fubuvalidation.UI.ValidationProcessor(theHandler);
    });

    it('replaces the validation handler', function () {
        var newHandler = { id: '1234' };
        theProcessor.useValidationHandler(newHandler);

        expect(theProcessor.handler).toEqual(newHandler);
    });

    it('registers the element finder', function () {
        var finder = { id: '123' };
        theProcessor.findElementsWith(finder);

        expect(theProcessor.finders).toEqual([finder]);
    });

    it('resets the handler', function () {
        var context = {};
        theProcessor.reset(context);

        expect(theHandler.reset.called).toEqual(true);
        expect(theHandler.reset.getCall(0).args[0]).toEqual(context);
    });

    it('registers the rendering strategy when the function exists', function () {
        var theStrategy = { id: '1234' };
        theHandler.registerStrategy = sinon.spy();
        theProcessor.registerStrategy(theStrategy);

        expect(theHandler.registerStrategy.called).toEqual(true);
        expect(theHandler.registerStrategy.getCall(0).args[0]).toEqual(theStrategy);
    });

    it('does not register the rendering strategy when the function does not exists', function () {
        var theStrategy = { id: '1234' };
        theProcessor.registerStrategy(theStrategy);
    });
});

describe('when finding an element', function () {
    var theProcessor = null;
    var theContext = null;
    var theKey = null;
    var theError = null;
    var theForm = null;
    var theSearchContext = null;
    var theElement = null;
    var theActualElement = null;

    var f1 = null;
    var f2 = null;


    beforeEach(function () {
        theProcessor = new $.fubuvalidation.UI.ValidationProcessor();

        theKey = '123';
        theError = { mesasge: 'uh oh' };
        theForm = { id: '145' };
        theSearchContext = {
            key: theKey,
            error: theError,
            form: theForm
        };

        theElement = { id: 1029 };
        f1 = sinon.spy(function (context) {
            context.error.element = theElement;
        });

        theContext = {};

        f2 = sinon.spy();

        theProcessor.findElementsWith(f1);
        theProcessor.findElementsWith(f2);

        theActualElement = theProcessor.findElement(theContext, theKey, theError);
    });

    it('calls each finder', function () {
        expect(f1.called).toEqual(true);
        expect(f2.called).toEqual(true);
    });

    it('returns the element from the context', function () {
        expect(theActualElement).toEqual(theElement);
    });

});

describe('Basic ValidationProcessor Tests', function () {

    it('basic finders find by id', function () {
        var error = {};
        var processor = $.fubuvalidation.UI.ValidationProcessor.basic();
        processor.findElement({ form: $('#FinderForm') }, 'LastName', error);

        expect(error.element).toEqual($('#LastName', '#FinderForm'));
    });

    it('basic finders find by name', function () {
        var error = {};
        var processor = $.fubuvalidation.UI.ValidationProcessor.basic();
        processor.findElement({ form: $('#FinderForm') }, 'FirstName', error);

        expect(error.element).toEqual($('input[name="FirstName"]', '#FinderForm'));
    });

    it('basic finders add to search context error', function () {
        var error = {},
            theSearchContext = {},
            processor = $.fubuvalidation.UI.ValidationProcessor.basic();
        processor.searchContext = theSearchContext;
        processor.findElement({ form: $('#FinderForm') }, 'FirstName', error);

        expect(theSearchContext.error.element).toEqual($('input[name="FirstName"]', '#FinderForm'));
    });
});

describe('when filling the element on the continuation error', function () {
    var theProcessor = null;

    beforeEach(function () {
        theProcessor = new $.fubuvalidation.UI.ValidationProcessor();
        theProcessor.findElement = sinon.stub();
    });

    it('does nothing if the element already exists', function () {
        var continuation = {
            errors: [{ element: '123', field: 'Test' }]
        };

        theProcessor.fillElements(continuation);

        expect(theProcessor.findElement.called).toEqual(false);
        expect(continuation.errors[0].element).toEqual('123');
    });

    it('does nothing if the field is not specified', function () {
        var continuation = {
            errors: [{}]
        };

        theProcessor.fillElements(continuation);

        expect(theProcessor.findElement.called).toEqual(false);
    });

    it('sets the element', function () {
        var error = { field: '123' };
        var element = '345';

        theProcessor.findElement.returns(element);

        var continuation = {
            errors: [error]
        };

        theProcessor.fillElements(continuation);

        expect(theProcessor.findElement.called).toEqual(true);
        expect(error.element).toEqual(element);
    });
});

describe('when processing the rendering context', function () {
    var theProcessor = null;
    var theContext = null;
    var theContinuation = null;
    var theHandler = null;

    beforeEach(function () {
        theContinuation = { id: '123' };

        theContext = {
            continuation: theContinuation
        };

        theHandler = {
            process: sinon.spy()
        };
        theProcessor = new $.fubuvalidation.UI.ValidationProcessor(theHandler);
        theProcessor.fillElements = sinon.spy();

        theProcessor.process(theContext);
    });

    it('fills the elements', function () {
        expect(theProcessor.fillElements.called).toEqual(true);
        expect(theProcessor.fillElements.getCall(0).args[0]).toEqual(theContinuation);
    });

    it('invokes the handler', function () {
        expect(theHandler.process.called).toEqual(true);
        expect(theHandler.process.getCall(0).args[0]).toEqual(theContext);
    });
});

describe('ValidationSummaryStrategy tests', function () {
    var theStrategy = null;

    beforeEach(function () {
        theStrategy = new $.fubuvalidation.UI.Strategies.Summary();
    });

    it('matches forms with data-validation-summary attribute', function () {
        var context = {
            continuation: {
                form: $('<form data-validation-summary="true" />')
            }
        };

        expect(theStrategy.matches(context)).toEqual(true);
    });

    it('does not match forms without data-validation-summary attribute', function () {
        var context = {
            continuation: {
                form: $('<form data-validation-summmmmmmmary="true" />')
            }
        };

        expect(theStrategy.matches(context)).toEqual(false);
    });
});

describe('ValidationSummary rendering tests', function () {
    var theContinuation = null;
    var theContext = null;
    var theStrategy = null;

    beforeEach(function () {

        theContinuation = {
            correlationId: '123',
            form: $('#ValidationSummaryStrategy'),
            success: false,
            errors: [{
                field: 'FirstName',
                label: 'FirstName',
                message: 'First Name is required'
            }]
        };

        theContext = new $.fubuvalidation.UI.RenderingContext(theContinuation);

        theStrategy = new $.fubuvalidation.UI.Strategies.Summary();
    });

    afterEach(function () {
        theStrategy.reset(theContext);
    });

    it('shows the validation summary', function () {
        theStrategy.render(theContext);
        expect($('#ValidationSummaryStrategy > .validation-container').is(':visible')).toEqual(true);
    });

    it('hides the summary when validation succeeds', function () {
        theStrategy.render(theContext);
        theContinuation.success = true;
        theContinuation.errors.length = 0;
        theStrategy.reset(theContext);

        expect($('#ValidationSummaryStrategy > .validation-container').is(':visible')).toEqual(false);
    });

    it('renders the messages in summary', function () {
        theStrategy.render(theContext);
        var error = theContinuation.errors[0];
        var token = $.fubuvalidation.UI.TokenFor(error);
        var found = false;

        $('#ValidationSummaryStrategy > .validation-container > .validation-summary > li').each(function () {
            if ($('a', this).html() == token) {
                found = true;
            }
        });

        expect(found).toEqual(true);
    });
});

describe('ElementHighlightingStrategy tests', function () {
    var theStrategy = null;
    var theContinuation = null;
    var theContext = null;

    beforeEach(function () {
        theStrategy = new $.fubuvalidation.UI.Strategies.Highlighting();

        theContinuation = {
            correlationId: '123',
            form: $('#ElementHighlightingStrategy'),
            success: false,
            errors: [{
                field: 'FirstName',
                label: 'FirstName',
                message: 'First Name is required',
                element: $('#FirstName', '#ElementHighlightingStrategy')
            }, {
                field: null,
                label: null,
                message: 'Something bad happened',
                element: null
            }]
        };

        theContext = new $.fubuvalidation.UI.RenderingContext(theContinuation);
    });

    afterEach(function () {
        theStrategy.reset(theContext);
    });

    it('matches forms with data-validation-higlight attribute', function () {
        var context = {
            continuation: {
                form: $('<form data-validation-highlight="true" />')
            }
        };

        expect(theStrategy.matches(context)).toEqual(true);
    });

    it('does not match forms without data-validation-higlight attribute', function () {
        var context = {
            continuation: {
                form: $('<form data-validation-summary="true" />')
            }
        };

        expect(theStrategy.matches(context)).toEqual(false);
    });

    it('only highlights fields with errors', function () {
        theStrategy.render(theContext);
        expect($('#FirstName', '#ElementHighlightingStrategy').hasClass('error')).toEqual(true);
        expect($('#LastName', '#ElementHighlightingStrategy').hasClass('error')).toEqual(false);
    });

    it('unhighlights fields when validation succeeds', function () {
        theStrategy.render(theContext);
        theContinuation.success = true;
        theContinuation.errors.length = 0;
        theStrategy.reset(theContext);

        expect($('#FirstName', '#ElementHighlightingStrategy').hasClass('error')).toEqual(false);
    });
});

describe('InlineErrorStrategy tests', function () {
    var theStrategy = null;

    beforeEach(function () {
        theStrategy = new $.fubuvalidation.UI.Strategies.Inline();
    });

    it('matches forms with data-validation-inline attribute', function () {
        var context = {
            continuation: {
                form: $('<form data-validation-inline="true" />')
            }
        };

        expect(theStrategy.matches(context)).toEqual(true);
    });

    it('does not match forms without data-validation-inline attribute', function () {
        var context = {
            continuation: {
                form: $('<form data-validation-highlight="true" />')
            }
        };

        expect(theStrategy.matches(context)).toEqual(false);
    });
});

describe('InlineErrorStrategy rendering tests', function () {
    var theStrategy = null;
    var theContinuation = null;
    var theContext = null;

    beforeEach(function () {
        theStrategy = new $.fubuvalidation.UI.Strategies.Inline();

        theContinuation = {
            correlationId: '123',
            form: $('#InlineErrorStrategy'),
            success: false,
            errors: [{
                field: 'FirstName',
                label: 'FirstName',
                message: 'First Name is required',
                element: $('#FirstName', '#InlineErrorStrategy')
            }]
        };

        theContext = new $.fubuvalidation.UI.RenderingContext(theContinuation);
    });

    afterEach(function () {
        theStrategy.reset(theContext);
    });

    it('appends span next to the originating element', function () {
        theStrategy.render(theContext);
        expect($('[data-field="FirstName"]', '#InlineErrorStrategy').html()).toEqual('First Name is required');
    });

    it('removes the inline errors when validation succeeds', function () {
        theStrategy.render(theContext);
        theContinuation.success = true;
        theContinuation.errors.length = 0;
        theStrategy.reset(theContext);

        expect($('[data-inline-error="FirstName"]', '#InlineErrorStrategy').size()).toEqual(0);
    });
});

describe('validating a form', function () {
    var theValidator = null;
    var theProcessor = null;
    var theController = null;
    var thePromises = null;

    beforeEach(function () {

        thePromises = [];

        theProcessor = {};

        theNotification = { id: '123' };

        theValidator = {
            validate: sinon.spy(function () {
                var promise = $.Deferred();
                thePromises.push(promise);

                return promise;
            })
        };

        theController = new $.fubuvalidation.UI.Controller(theValidator, theProcessor);
    });

    it('aggregates the promises from validating each element', function () {
        var elements = $([$('<input type="text" value="Test1" />'), $('<input type="text" value="Test2" />')]);
        theController.elementsFor = function () {
            return elements;
        };

        var theNotification = null;
        var form = $('<form></form>');

        var aggregate = theController.validateForm(form).done(function (notification) {
            theNotification = notification;
        });

        _.each(thePromises, function (promise) {
            promise.resolve();
        });

        waitsFor(function () {
            return aggregate.state() == 'resolved';
        });

        expect(theNotification).not.toBeNull();
    });

});

describe('Controller submit handler', function () {
    var theValidator = null;
    var theProcessor = null;
    var theController = null;
    var theForm = null;

    var theValidationPromise;

    beforeEach(function () {
        theProcessor = {};
        theNotification = { id: '123' };
        theValidator = {};

        theForm = $("<form></form>");

        theValidationPromise = $.Deferred();

        theController = new $.fubuvalidation.UI.Controller(theValidator, theProcessor);
        theController.validateForm = function () {
            return theValidationPromise;
        };
        theController.processNotification = sinon.spy();
    });

    it('chains the process notification call to the promise', function () {

        var invoked = false;
        theController.submitHandler(theForm).done(function () {
            invoked = true;
        });

        theValidationPromise.resolve(theNotification);

        waitsFor(function () {
            return theValidationPromise.state() == 'resolved';
        });

        expect(invoked).toEqual(true);

        var process = theController.processNotification;
        expect(process.called).toEqual(true);
        expect(process.getCall(0).args[0]).toEqual(theNotification);
        expect(process.getCall(0).args[1]).toEqual(theForm);
    });

});

describe('Controller element handler', function () {
    var theValidator = null;
    var theProcessor = null;
    var theController = null;
    var theForm = null;
    var theElement = null;
    var thePromise = null;

    beforeEach(function () {
        theProcessor = {};
        theNotification = new $.fubuvalidation.Core.Notification();

        theForm = $("<form></form>");
        theElement = $("<input />");

        thePromise = $.Deferred();
        theValidator = {
            validate: function () {
                return thePromise;
            },
            planFor: function () {
                return {
                    isEmpty: function () { return false; }
                }
            }
        };

        theController = new $.fubuvalidation.UI.Controller(theValidator, theProcessor);
        theController.processNotification = sinon.spy();
        theController.targetValidated = sinon.spy();
    });

    it('chains the process notification call to the promise', function () {

        var invoked = false;
        theController.elementHandler(theElement, theForm).done(function () {
            invoked = true;
        });

        thePromise.resolve(theNotification);

        waitsFor(function () {
            return thePromise.state() == 'resolved';
        });

        expect(invoked).toEqual(true);

        expect(theController.targetValidated.called).toEqual(true);

        var process = theController.processNotification;
        expect(process.called).toEqual(true);

        expect(process.getCall(0).args[0]).toEqual(theNotification);
        expect(process.getCall(0).args[1]).toEqual(theForm);
    });

    it('does not process the notification if the plan is empty', function () {

        theValidator.planFor = function () {
            return {
                isEmpty: function() {
                    return true;
                }
            };
        };

        var invoked = false;
        theController.elementHandler(theElement, theForm).done(function () {
            invoked = true;
        });

        thePromise.resolve(theNotification);

        waitsFor(function () {
            return thePromise.state() == 'resolved';
        });

        expect(theController.targetValidated.called).toEqual(false);

        var process = theController.processNotification;
        expect(process.called).toEqual(false);
    });

});

describe('ValidationFormController', function () {
    var theController = null;

    beforeEach(function () {
        theController = new $.fubuvalidation.UI.Controller(null, null);
    });

    it('caches the targets by hash and mode', function () {
        var element = $('<input type="text" value="Test" />');
        var target = $.fubuvalidation.Core.Target.forElement(element, '123');

        theController.targetValidated(target, 'live');

        var key = theController.hashFor(target, 'live');

        expect(theController.targetCache[key]).toEqual('Test');

        var differentMode = theController.hashFor(target, 'triggered');
        expect(typeof (theController.targetCache[differentMode])).toEqual('undefined');
    });

    it('should validate when no value is found', function () {
        var element = $('<input type="text" value="Test" />');
        var target = $.fubuvalidation.Core.Target.forElement(element, '123');

        expect(theController.shouldValidate(target)).toEqual(true);
    });

    it('should validate when the values are different', function () {
        var element = $('<input type="text" value="Test" />');
        var target = $.fubuvalidation.Core.Target.forElement(element, '123');

        theController.targetValidated(target, 'live');

        element.val('Testing...');

        expect(theController.shouldValidate(target, 'live')).toEqual(true);
    });

    it('should validate when the modes are different', function () {
        var element = $('<input type="text" value="Test" />');
        var target = $.fubuvalidation.Core.Target.forElement(element, '123');

        theController.targetValidated(target, 'live');

        expect(theController.shouldValidate(target, 'mode')).toEqual(true);
    });

    it('should not validate when the values are the same', function () {
        var element = $('<input type="text" value="Test" />');
        var target = $.fubuvalidation.Core.Target.forElement(element, '123');

        theController.targetValidated(target);

        expect(theController.shouldValidate(target)).toEqual(false);
    });

    it('invalidate target erases targetcache on key', function () {
        var element = $('<input type="text" value"Text" />');
        var target = $.fubuvalidation.Core.Target.forElement(element, '123');
        var key = theController.hashFor(target, 'live');
        theController.targetCache[key] = 'Test';
        theController.invalidateTarget(target, 'live');
        expect(theController.targetCache[key]).toEqual(undefined);
    });

    it('invalidate target does nothing when targetcache key undefined', function () {
        var element = $('<input type="text" value"Text" />');
        var target = $.fubuvalidation.Core.Target.forElement(element, '123');
        var key = theController.hashFor(target, 'live');
        theController.targetCache[key] = undefined;
        theController.invalidateTarget(target, 'live');
        expect(theController.targetCache[key]).toEqual(undefined);
    });

});

describe('Processing a continuation', function () {
    var theValidator = null;
    var theProcessor = null;
    var theController = null;
    var theForm = null;
    var theElement = null;
    var theContinuation = null;

    beforeEach(function () {
        theProcessor = {
            process: sinon.spy()
        };
        theValidator = {};

        theContinuation = { id: '123', mode: 'triggered' };
        theForm = $("<form></form>");
        theElement = $("<input />");

        theController = new $.fubuvalidation.UI.Controller(theValidator, theProcessor);
        theController.processContinuation(theContinuation, theForm, theElement);
    });

    it('processes the rendering context', function () {
        var context = new $.fubuvalidation.UI.RenderingContext(theContinuation, theElement, 'triggered');
        expect(theProcessor.process.called).toEqual(true);
        expect(theProcessor.process.getCall(0).args[0]).toEqual(context);
    });

});

describe('FormValidated', function () {
    var theEvent = null;
    var theNotification = null;
    var isValid = null;

    beforeEach(function () {
        theNotification = {
            isValid: function () {
                return isValid;
            }
        };
        theEvent = new $.fubuvalidation.UI.FormValidated(theNotification);
    });

    it('shouldSubmit defaults to notification validity', function () {
        isValid = true;
        expect(theEvent.shouldSubmit()).toEqual(true);
    });

    it('shouldSubmit defaults to notification validity (negative)', function () {
        isValid = false;
        expect(theEvent.shouldSubmit()).toEqual(false);
    });

    it('prevents the submission', function () {
        isValid = true;
        theEvent.preventSubmission();
        expect(theEvent.shouldSubmit()).toEqual(false);
    });

});

describe('CountStrategy tests', function() {
  var theStrategy = null;

  beforeEach(function() {
    theStrategy = new $.fubuvalidation.UI.Strategies.Count();
  });

  it('does not match init when context has no fields defined', function() {
    var context = {
      target: {
        fieldName: 'TestName'
      },
      options: {
        fields: []
      }
    };

    expect(theStrategy.initMatches(context)).toEqual(false);
  });

  it('does not match init when context does not define options for field of interest', function() {
    var context = {
      target: {
        fieldName: 'TestName'
      },
      options: {
        fields: [{
          field: 'OtherField'
        }]
      }
    };

    expect(theStrategy.initMatches(context)).toEqual(false);
  });

  it('does not match init field is triggered validation', function() {
    var context = {
      target: {
        fieldName: 'TestName'
      },
      options: {
        fields: [{
          field: 'TestName',
          mode: 'triggered'
        }]
      }
    };

    expect(theStrategy.initMatches(context)).toEqual(false);
  });

  it('does not match init when field is live validation with no plan runners', function() {
    var context = {
      target: {
        fieldName: 'TestName'
      },
      options: {
        fields: [{
          field: 'TestName',
          mode: 'live',
          rules: []
        }]
      }
    };

    expect(theStrategy.initMatches(context)).toEqual(false);
  });

  it('matches init when field is live validation with plan runners', function() {
    var context = {
      target: {
        fieldName: 'TestName'
      },
      options: {
        fields: [{
          field: 'TestName',
          mode: 'live'
        }]
      },
      plan: {
        runners: [{}]
      }
    };

    expect(theStrategy.initMatches(context)).toEqual(true);
  });

  it('does not match when no element provided', function() {
    var context = { };
    expect(theStrategy.matches(context)).toEqual(false);
  });

  it('does not match when element does not have count attr', function() {
    var context = {
      element: $('<input type="text" />')
    };

    expect(theStrategy.matches(context)).toEqual(false);
  });

  it('matches when element has count attr', function() {
    var context = {
      element: $('<input type="text" data-validation-count="0" />')
    };

    expect(theStrategy.matches(context)).toEqual(true);
  });

  it('initializes a default validation count of zero', function() {
    var context = {
      element: $('<input type="text" />')
    };

    theStrategy.init(context);

    expect(parseInt(context.element.attr(theStrategy.dataKey))).toEqual(0);
  });

  it('initializes a default validation count of zero', function() {
    var context = {
      element: $('<input type="text" data-validation-count="0" />')
    };

    theStrategy.render(context);
    expect(parseInt(context.element.attr(theStrategy.dataKey))).toEqual(1);

    theStrategy.render(context);
    expect(parseInt(context.element.attr(theStrategy.dataKey))).toEqual(2);
  });

  it('does not fail if there is no element on the context', function() {
    var context = { };
    theStrategy.render(context);
  });
});