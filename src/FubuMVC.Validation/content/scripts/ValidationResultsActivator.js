$(document).ready(function () {
  $('form').each(function () {
    var results = $(this).data('validationResults');
    if (typeof (results) == 'undefined') return;

    var continuation = new $fubu.continuations.create(results);

    continuation.form = $(this);
    $.fubuvalidation.Processor.process(continuation);
  });
});