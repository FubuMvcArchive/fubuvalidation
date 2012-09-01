// jquery.continuations.forms v0.9.1
//
// Copyright (C)2011 Joshua Arnold, Jeremy Miller
// Distributed Under Apache License, Version 2.0
//
// https://github.com/DarthFubuMVC/jquery-continuations

(function (continuations) {

	$.fn.correlatedSubmit = function (options) {
		if(typeof(options) === 'undefined') {
			options = {};
		}
		
        return this.each(function () {
            var self = $(this);
            var correlationId = options.correlationId;
            if (typeof(correlationId) === 'undefined') {
                var id = self.attr('id');
                if (!id) {
                    id = 'form_' + new Date().getTime().toString();
                    self.attr('id', id);
                }

                correlationId = id;
            }

            self.ajaxSubmit({
				correlationId: correlationId,
				continuationSuccess: function(continuation) {
					continuation.form = self;
					continuation.options = options;
					
					if($.isFunction(options.continuationSuccess)) {
						options.continuationSuccess(continuation);
					}
				},
				continuationError: function(continuation) {
					continuation.form = self;
					continuation.options = options;
					
					if($.isFunction(options.continuationError)) {
						options.continuationError(continuation);
					}
				}
            });
        });
    };

}(jQuery.continuations));