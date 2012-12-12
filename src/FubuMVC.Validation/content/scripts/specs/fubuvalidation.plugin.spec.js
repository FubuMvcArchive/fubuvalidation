function ValidationHarness() {
    this.form = $('#EndToEnd');
    this.container = $('.validation-container', this.form);
    this.posted = false;

    this.form.validate();
    this.form.resetForm();

    this.server = sinon.fakeServer.create();
    var self = this;
    $.continuations.bind('AjaxStarted', function () {
        self.posted = true;
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
        $.fubuvalidation.ui.reset();
        $.continuations.reset();
    }
};

describe('when submitting an invalid form', function () {
    var theHarness = null;

    beforeEach(function () {
        theHarness = new ValidationHarness();
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
});