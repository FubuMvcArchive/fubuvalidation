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
        var continuation = {};
        theProcessor.reset(continuation);

        expect(theHandler.reset.called).toEqual(true);
        expect(theHandler.reset.getCall(0).args[0]).toEqual(continuation);
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
    var theContinuation = null;
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
            context.element = theElement;
        });

        theContinuation = {};

        f2 = sinon.spy();

        theProcessor.findElementsWith(f1);
        theProcessor.findElementsWith(f2);

        theActualElement = theProcessor.findElement(theContinuation, theKey, theError);
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

describe('when processing the continuation', function () {
    var theProcessor = null;
    var theContinuation = null;
    var theHandler = null;

    beforeEach(function () {
        theContinuation = {};
        theHandler = {
            process: sinon.spy()
        };
        theProcessor = new $.fubuvalidation.UI.ValidationProcessor(theHandler);
        theProcessor.fillElements = sinon.spy();

        theProcessor.process(theContinuation);
    });

    it('fills the elements', function () {
        expect(theProcessor.fillElements.called).toEqual(true);
        expect(theProcessor.fillElements.getCall(0).args[0]).toEqual(theContinuation);
    });

    it('invokes the handler', function () {
        expect(theHandler.process.called).toEqual(true);
        expect(theHandler.process.getCall(0).args[0]).toEqual(theContinuation);
    });
});

describe('ValidationSummaryStrategy tests', function () {
    var theStrategy = null;

    beforeEach(function () {
        theStrategy = new $.fubuvalidation.UI.Strategies.Summary();
    });

    it('matches forms with data-validation-summary attribute', function () {
        var continuation = {
            form: $('<form data-validation-summary="true" />')
        };

        expect(theStrategy.matches(continuation)).toEqual(true);
    });

    it('does not match forms without data-validation-summary attribute', function () {
        var continuation = {
            form: $('<form data-validation-summmmmmmmary="true" />')
        };

        expect(theStrategy.matches(continuation)).toEqual(false);
    });
});

describe('ValidationSummary rendering tests', function () {
    var theContinuation = null;
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

        theStrategy = new $.fubuvalidation.UI.Strategies.Summary();
    });

    afterEach(function () {
        theStrategy.reset(theContinuation);
    });

    it('shows the validation summary', function () {
        theStrategy.render(theContinuation);
        expect($('#ValidationSummaryStrategy > .validation-container').is(':visible')).toEqual(true);
    });

    it('hides the summary when validation succeeds', function () {
        theStrategy.render(theContinuation);
        theContinuation.success = true;
        theContinuation.errors.length = 0;
        theStrategy.reset(theContinuation);

        expect($('#ValidationSummaryStrategy > .validation-container').is(':visible')).toEqual(false);
    });

    it('renders the messages in summary', function () {
        theStrategy.render(theContinuation);
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
            }]
        };
    });

    afterEach(function () {
        theStrategy.reset(theContinuation);
    });

    it('matches forms with data-validation-higlight attribute', function () {
        var continuation = {
            form: $('<form data-validation-highlight="true" />')
        };

        expect(theStrategy.matches(continuation)).toEqual(true);
    });

    it('does not match forms without data-validation-higlight attribute', function () {
        var continuation = {
            form: $('<form data-validation-summary="true" />')
        };

        expect(theStrategy.matches(continuation)).toEqual(false);
    });

    it('only highlights fields with errors', function () {
        theStrategy.render(theContinuation);
        expect($('#FirstName', '#ElementHighlightingStrategy').hasClass('error')).toEqual(true);
        expect($('#LastName', '#ElementHighlightingStrategy').hasClass('error')).toEqual(false);
    });

    it('unhighlights fields when validation succeeds', function () {
        theStrategy.render(theContinuation);
        theContinuation.success = true;
        theContinuation.errors.length = 0;
        theStrategy.reset(theContinuation);

        expect($('#FirstName', '#ElementHighlightingStrategy').hasClass('error')).toEqual(false);
    });
});

describe('InlineErrorStrategy tests', function () {
    var theStrategy = null;

    beforeEach(function () {
        theStrategy = new $.fubuvalidation.UI.Strategies.Inline();
    });

    it('matches forms with data-validation-inline attribute', function () {
        var continuation = {
            form: $('<form data-validation-inline="true" />')
        };

        expect(theStrategy.matches(continuation)).toEqual(true);
    });

    it('does not match forms without data-validation-inline attribute', function () {
        var continuation = {
            form: $('<form data-validation-highlight="true" />')
        };

        expect(theStrategy.matches(continuation)).toEqual(false);
    });
});

describe('InlineErrorStrategy rendering tests', function () {
    var theStrategy = null;
    var theContinuation = null;

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
    });

    afterEach(function () {
        theStrategy.reset(theContinuation);
    });

    it('appends span next to the originating element', function () {
        theStrategy.render(theContinuation);
        expect($('[data-field="FirstName"]', '#InlineErrorStrategy').html()).toEqual('First Name is required');
    });

    it('removes the inline errors when validation succeeds', function () {
        theStrategy.render(theContinuation);
        theContinuation.success = true;
        theContinuation.errors.length = 0;
        theStrategy.reset(theContinuation);

        expect($('[data-inline-error="FirstName"]', '#InlineErrorStrategy').size()).toEqual(0);
    });
});