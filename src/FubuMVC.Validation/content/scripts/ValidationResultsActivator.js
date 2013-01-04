$(document).ready(function () {
    $('form').each(function () {
        var results = $(this).data('validationResults');
    	if(typeof (results) == 'undefined') return;

		// Make it a canonical continuation. Probably need to make a helper in jquery.continuations
    	var continuation = new $.continuations.continuation();
    	$.extend(true, continuation, results);

    	continuation.form = $(this);
    	$.fubuvalidation.Processor.process(continuation);
    });
});