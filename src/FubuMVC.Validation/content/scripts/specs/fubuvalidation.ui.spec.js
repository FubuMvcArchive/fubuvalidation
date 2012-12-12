var ObjectMother = {
	continuation: function() {
		return {
			correlationId: '123',
			form: $('#test'),
			success: false,
			errors: [{
				field: 'FirstName',
                label: 'FirstName',
				message: 'First Name is required'
			}]
		};
	}
};

describe('fubuvalidation module tests', function() {
	beforeEach(function() {
		$.fubuvalidation.ui.reset();
	});
	it('should invoke the last registered handler', function() {
		var invoked = false;
		var myHandler = {
			matches: function() { return true; },
			process: function(context) { invoked = true; }
		};
		
		$.fubuvalidation.ui.registerHandler(myHandler);
		$.fubuvalidation.ui.process(ObjectMother.continuation());
		
		expect(invoked).toEqual(true);
	});
	
	it('should use registered element finders', function() {
		var context;
		var myFinder = function(searchContext) {
			var element = searchContext.element;
			if(element && element.attr('type') == 'hidden') {
				var hidden = $('#' + searchContext.key + 'Value', searchContext.form);
				if(hidden.size() != 0) {
					searchContext.element = hidden;
				}
			}
			
			context = searchContext;
		};
		
		$.fubuvalidation.ui.findElementsWith(myFinder);
		var continuation = ObjectMother.continuation();
		continuation.errors.push({
			field: 'LookupProperty',
            label: 'LookupProperty',
			message: 'LookupProperty is required'
		});
		
		$.fubuvalidation.ui.process(continuation);
		
		expect(context.element.attr('id')).toEqual('LookupPropertyValue');
	});
	
	it('handles empty fields', function() {
		var continuation = ObjectMother.continuation();
		continuation.errors.push({
			field: '',
			label: '',
			message: 'LookupProperty is required'
		});
		
		$.fubuvalidation.ui.process(continuation);
	});
});

describe('Default validation handler integrated tests', function () {
	var theContinuation;
	var process;
	beforeEach(function() {
		$.fubuvalidation.ui.reset();
		theContinuation = ObjectMother.continuation();
		process = function() {
			$.fubuvalidation.ui.process(theContinuation);
		};
	});
	
	it('should show validation summary', function() {
		process();
		expect($('#test > .validation-container').is(':visible')).toEqual(true);
	});

	it('should only highlight fields with errors', function() {
		process();
		expect($('#FirstName', '#test').hasClass('error')).toEqual(true);
		expect($('#LastName', '#test').hasClass('error')).toEqual(false);
	});
	
	it('should hide summary when validation succeeds', function() {
		process();
		theContinuation.success = true;
		theContinuation.errors.length = 0;
		process();
		
		expect($('#test > .validation-container').is(':visible')).toEqual(false);
	});
	
	it('should unhighlight fields when validation succeeds', function() {
		process();
		theContinuation.success = true;
		theContinuation.errors.length = 0;
		process();
		
		expect($('#FirstName', '#test').hasClass('error')).toEqual(false);
	});
	
	it('should render messages in summary', function() {
		process();
		var error = theContinuation.errors[0];
		var token = $.fubuvalidation.ui.defaultHandler.generateToken(error);
		var found = false;
		
		$('#test > .validation-container > .validation-summary > li').each(function() {
			if($('a', this).html() == token) {
				found = true;
			}
		});
		
		expect(found).toEqual(true);
	});
	
	// this is such a common usage that it should come for free
	it('should reset default handler when jquery form reset is invoked', function() {
		$('#LastName', '#test').val('Test');
		process();
		$('#test').resetForm();
		expect($('#FirstName', '#test').hasClass('error')).toEqual(false);
		expect($('#LastName', '#test').val()).toEqual('');
	});
});

describe('jquery.continuations and fubuvalidation.js integration tests', function () {
    var server;
    beforeEach(function () {
        server = sinon.fakeServer.create();
        $.fubuvalidation.ui.reset();
    });
    afterEach(function () {
        server.restore();
        $.continuations.reset();
    });

    it('should render errors then clear previous errors when validation succeeds', function () {
        var theContinuation = ObjectMother.continuation();
        var continuation = function () {
            var c = $.extend({}, theContinuation);
            c.form = null;
            return JSON.stringify(c);
        };
        $.continuations.bind('AjaxStarted', function (request) {
            server.respondWith([200,
				{ 'Content-Type': 'application/json', 'X-Correlation-Id': request.correlationId }, continuation()
			]);
        });

        runs(function () {
            $('#test').correlatedSubmit();
            server.respond();
        });

        waits(500);

        runs(function () {
            expect($('#FirstName', '#test').hasClass('error')).toEqual(true);

            var error = theContinuation.errors[0];
            var token = $.fubuvalidation.ui.defaultHandler.generateToken(error);
            var found = false;

            $('#test > .validation-container > .validation-summary > li').each(function () {
                if ($('a', this).html() == token) {
                    found = true;
                }
            });

            expect(found).toEqual(true);

            theContinuation.errors = null; // make sure we can handle the absence of errors
            theContinuation.success = true;

            $('#test').correlatedSubmit();
            server.respond();
        });

        waits(500);

        runs(function () {
            expect($('#test > .validation-container').is(':visible')).toEqual(false);
            expect($('#test > .validation-container > .validation-summary > li').size()).toEqual(0);

            expect($('#FirstName', '#test').hasClass('error')).toEqual(false);
        });
    });
});