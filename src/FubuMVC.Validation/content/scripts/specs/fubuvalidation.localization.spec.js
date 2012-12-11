describe('LocalizerTester', function () {
    var theLocalizer = null;
    var theToken = null;

    beforeEach(function () {
        theToken = new $.fubuvalidation.StringToken('Son', 'Joel');
        theLocalizer = new $.fubuvalidation.LocalizationManager();
    });

    it('retrieves the value from the cache', function () {
        var theValue = 'Joel Arnold';
        theLocalizer.cache[theToken.key] = theValue;

        expect(theLocalizer.valueFor(theToken)).toEqual(theValue);
    });

    it('uses the default value', function () {
        expect(theLocalizer.valueFor(theToken)).toEqual(theToken.defaultValue);
    });
});

describe('Integrated StringToken Tests', function () {
    var theToken = null;
    var theValue = null;

    beforeEach(function () {
        theToken = new $.fubuvalidation.StringToken('Son', 'Joel');
        $.fubuvalidation.localizer.cache[theToken.key] = 'Joel Arnold';

        theValue = theToken.toString();
    });

    it('toString uses the value from the localizer cache', function () {
        expect(theValue).toEqual('Joel Arnold');
    });
});