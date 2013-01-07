function ValidationHarness(callback) {
    this.form = $('#EndToEnd');
    this.container = $('.validation-container', this.form);
    this.posted = false;

    this.form.validate({
        continuationSuccess: callback
    });
    this.form.resetForm();

    this.server = sinon.fakeServer.create();
    var self = this;
    $.continuations.bind('AjaxStarted', function (request) {
        self.posted = true;
        self.server.respondWith([200,
				{ 'Content-Type': 'application/json', 'X-Correlation-Id': request.correlationId }, '{"success":"true"}'
        ]);
    });
}

ValidationHarness.prototype = {
    elementFor: function (name) {
        return this.form.find('[name="' + name + '"]');
    },
    submit: function () {
        this.form.submit();
    },
    dispose: function () {
        this.form.resetForm();
        this.server.restore();
    }
};

describe('when submitting an invalid form', function () {
    var theHarness = null;
    var theCallback = null;

    beforeEach(function () {
        theCallback = sinon.spy();
        theHarness = new ValidationHarness(theCallback);
        theHarness.submit();
    });

    afterEach(function () {
        theHarness.dispose();
    });

    it('does not make the request', function () {
        expect(theHarness.posted).toEqual(false);
    });

    it('shows the validation container', function () {
        expect(theHarness.container.is(':visible')).toEqual(true);
    });

    it('highlights the fields with errors', function () {
        expect(theHarness.elementFor('Name').hasClass('error')).toEqual(true);
        expect(theHarness.elementFor('Email').hasClass('error')).toEqual(true);
    });

    it('does not invoke the callback', function() {
        expect(theCallback.called).toEqual(false);
    });
});

describe('when submitting a valid form', function () {
    var theHarness = null;
    var theCallback = null;

    beforeEach(function () {
        theCallback = sinon.spy();
        theHarness = new ValidationHarness(theCallback);
        
        $('#Name', '#EndToEnd').val('Joel');
        $('#Email', '#EndToEnd').val('joel@fubu-project.org');

        theHarness.submit();
        
        theHarness.server.respond();
    });

    afterEach(function () {
        theHarness.dispose();
    });

    it('invokes the callback', function () {
        expect(theCallback.called).toEqual(true);
    });
});